using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using BranchSdk.Net;
using BranchSdk.Net.Requests;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Core;

namespace BranchSdk {
    public class BranchInitCallbackWrapper {
        private enum Types {
            WithDictionary,
            WithBUO
        }
        private Types type;

        public Branch.BranchInitCallbackWithDictionary dictionaryCallback;
        public Branch.BranchInitCallbackWithBUO buoCallback;
        public Action CommonCallback;

        public BranchInitCallbackWrapper(Branch.BranchInitCallbackWithDictionary dictionaryCallback) {
            this.dictionaryCallback = dictionaryCallback;
            type = Types.WithDictionary;
        }

        public BranchInitCallbackWrapper(Branch.BranchInitCallbackWithBUO buoCallback) {
            this.buoCallback = buoCallback;
            type = Types.WithBUO;
        }

        public void Invoke(JObject jsonData, string error) {
            if (CommonCallback != null) CommonCallback.Invoke();

            string serializedJson = jsonData["data"].ToString();
            serializedJson = serializedJson.Replace(@"\", "");

            if (type == Types.WithDictionary) {
                jsonData = JObject.Parse(serializedJson);
                dictionaryCallback.Invoke(jsonData.ToObject<Dictionary<string, object>>(), null);
            } else {
                buoCallback.Invoke(new BranchUniversalObject(serializedJson), new BranchLinkProperties(serializedJson), null);
            }
        }
    }

    public class Branch {
        #region Singleton

        private static Branch instance;
        public static Branch Instance {
            get {
                if (instance == null) {
                    instance = GetAutoInstance();

                    if (!BranchDeviceInfo.IsInit) BranchDeviceInfo.Init();
                    if (!BranchTrackingController.IsInit) BranchTrackingController.Init();
                } else {
                    if (!BranchDeviceInfo.IsInit) BranchDeviceInfo.Init();
                    if (!BranchTrackingController.IsInit) BranchTrackingController.Init();
                }

                return instance;
            }
        }

        public static Branch I {
            get {
                return Instance;
            }
        }

        public static bool IsInit {
            get {
                return instance != null;
            }
        }

        public static Branch GetInstance() {
            if (IsInit) return I;
            return GetBranchInstance(true, string.Empty);
        }

        public static Branch GetTestInstance() {
            if (IsInit) return I;
            return GetBranchInstance(false, string.Empty);
        }

        public static Branch GetAutoInstance() {
            if (IsInit) return I;
            return GetBranchInstance(!BranchUtil.IsCustomDebugEnabled, string.Empty);
        }

        private static Branch GetBranchInstance(bool isLive, string branchKey) {
            if (string.IsNullOrEmpty(branchKey)) {
                string lastBranchKey = LibraryAdapter.GetPrefHelper().GetBranchKey();
                LibraryAdapter.GetPrefHelper().SetBranchKey(isLive ? BranchConfigManager.GetLiveBranchKey() : BranchConfigManager.GetTestBranchKey());

                if(!lastBranchKey.Equals(LibraryAdapter.GetPrefHelper().GetBranchKey())) {
                    LibraryAdapter.GetPrefHelper().ClearUserValues();
                }
            } else {
                LibraryAdapter.GetPrefHelper().SetBranchKey(branchKey);
            }
            Debug.WriteLine("Current branch key: " + LibraryAdapter.GetPrefHelper().GetBranchKey());
            instance = new Branch();
            return instance;
        }

        #endregion

        public const int LINK_TYPE_UNLIMITED_USE = 0;
        public const int LINK_TYPE_ONE_TIME_USE = 1;

        private JObject deeplinkDebugParams;
        private BranchShareLinkManager branchShareLinkManager;

        public delegate void BranchGetRewardHistoryCallback(JArray jArray, BranchError error);
        public delegate void BranchRedeemRewardsCallback(bool changed, BranchError error);
        public delegate void BranchGetRewardsCallback(bool changed, BranchError error);
        public delegate void BranchLogoutCallback(bool loggedOut, BranchError error);
        public delegate void BranchIdentityUserCallback(JObject referringParams, BranchError error);
        public delegate void BranchInitCallbackWithDictionary(Dictionary<string, object> parameters, BranchError error);
        public delegate void BranchInitCallbackWithBUO(BranchUniversalObject universalObject, BranchLinkProperties linkProperties, BranchError error);
        public delegate void BranchCreateLinkCallback(string url, BranchError error);

        private enum SessionState {
            Initialised, Initialising, Uninitialised
        }

        public enum CreditHistoryOrder {
            KMostRecentFirst, KLeastRecentFirst
        }

        private SessionState initState = SessionState.Uninitialised;

        public bool IsSimulatingInstalls;

        public BranchInitCallbackWrapper CachedInitCallback { get; private set; }
        public bool IsFirstSessionInited { get; private set; }

        public void EnableTestMode() {
            BranchUtil.IsCustomDebugEnabled = true;
        }

        public void DisableTestMode() {
            BranchUtil.IsCustomDebugEnabled = false;
        }

        public void SetDebug(bool isDebug) {
            if (isDebug) EnableTestMode();
            else DisableTestMode();
        }

        public void DisableTracking(bool disableTracking) {
            BranchTrackingController.DisableTracking(disableTracking);
        }

        public bool IsTrackingDisabled() {
            return BranchTrackingController.TrackingDisabled;
        }

        public void SetNetworkTimeout(int timeout) {
            if (timeout > 0) {
                LibraryAdapter.GetPrefHelper().SetNetworkTimeout(timeout);
            }
        }

        public void SetMaxRetries(int maxRetries) {
            if (maxRetries >= 0) {
                LibraryAdapter.GetPrefHelper().SetMaxRetries(maxRetries);
            }
        }

        public void SetRetryInterval(int retryInterval) {
            if (retryInterval > 0) {
                LibraryAdapter.GetPrefHelper().SetRetryInterval(retryInterval);
            }
        }

        public void SetRequestMetadata(string key, string value) {
            LibraryAdapter.GetPrefHelper().SetRequestMetadata(key, value);
        }

        public void EnableSimulateInstalls() {
            IsSimulatingInstalls = true;
        }

        public void DisableSimulateInstalls() {
            IsSimulatingInstalls = false;
        }

        public void InitSession(string linkUrl = "", bool autoInitSession = false) {
            if (autoInitSession) {
                if (IsFirstSessionInited) {
                    InitUserSessionInternal(CachedInitCallback, false, linkUrl);
                }
            } else {
                InitUserSessionInternal(null, false, linkUrl);
            }
        }

        public void InitSession(bool isReferrable, string linkUrl = "") {
            InitUserSessionInternal(null, isReferrable, linkUrl);
        }

        public void InitSession(BranchInitCallbackWrapper callback, string linkUrl = "") {
            InitUserSessionInternal(callback, false, linkUrl);
        }

        public void InitSession(bool isReferrable, BranchInitCallbackWrapper callback, string linkUrl = "") {
            InitUserSessionInternal(callback, isReferrable, linkUrl);
        }

        private void InitUserSessionInternal(BranchInitCallbackWrapper callback, bool isReferrable, string linkUrl = "") {
            if (HasUser() && HasSession() && initState == SessionState.Initialised) {
                
            } else {
                if (isReferrable) {
                    LibraryAdapter.GetPrefHelper().SetIsReferrable();
                } else {
                    LibraryAdapter.GetPrefHelper().ClearIsReferrable();
                }
            }

            initState = SessionState.Initialising;
            InitializeSession(callback, linkUrl);
        }

        private void InitializeSession(BranchInitCallbackWrapper callback, string linkUrl = "") {
            if (string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetBranchKey())) {
                initState = SessionState.Uninitialised;
                Debug.WriteLine("Please create \"live_branch_key\" and  \"test_branch_key\" txt file with branch key inside in \"Configs\" folder in \"Assets\" folder");
                return;
            } else {
                Debug.WriteLine("BranchSDK", "Branch Warning: You are using your test app's Branch Key. Remember to change it to live Branch Key during deployment.");
            }

            CachedInitCallback = callback;

            RegisterAppInit(callback, linkUrl);
        }

        public void RegisterAppInit(BranchInitCallbackWrapper callback, string linkUrl = "") {
            if (callback != null) callback.CommonCallback = () => { IsFirstSessionInited = true; };

            BranchServerRequest request = GetInstallOrOpenRequest(callback, linkUrl);
            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }

        private BranchServerRequest GetInstallOrOpenRequest(BranchInitCallbackWrapper callback, string linkUrl = "") {
            BranchServerRequest request = null;
            if (HasUser()) {
                request = new BranchServerRegisterOpen(callback, linkUrl);
            } else {
                //temp installatian id
                request = new BranchServerRegisterInstall(callback, "345345667453646");
            }
            request.RequestType = RequestTypes.POST;
            return request;
        }

        public JObject GetFirstParams() {
            string storedParam = LibraryAdapter.GetPrefHelper().GetInstallParams();
            JObject firstReferringParams = ConvertParamsStringToDictionary(storedParam);
            firstReferringParams = AppendDebugParams(firstReferringParams);
            return firstReferringParams;
        } 

        public JObject GetSessionParams() {
            string storedParam = LibraryAdapter.GetPrefHelper().GetSessionParams();
            JObject latestParams = ConvertParamsStringToDictionary(storedParam);
            latestParams = AppendDebugParams(latestParams);
            return latestParams;
        }

        public void SetIdentity(string userID, BranchIdentityUserCallback callback) {
            BranchServerIdentifyUser request = new BranchServerIdentifyUser(callback, userID);
            request.RequestType = RequestTypes.POST;

            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }

        public void Logout(BranchLogoutCallback callback) {
            BranchServerRequest request = new BranchServerLogout(callback);
            request.RequestType = RequestTypes.POST;

            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }

        public void LoadRewards(Branch.BranchGetRewardsCallback callback) {
            BranchServerGetRewards request = new BranchServerGetRewards(callback);
            request.RequestType = RequestTypes.GET;

            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }

        public void RedeemRewards(int count) {
            RedeemRewards(BranchJsonKey.DefaultBucket.GetKey(), count, null);
        }

        public void RedeemRewards(int count, BranchRedeemRewardsCallback callback) {
            RedeemRewards(BranchJsonKey.DefaultBucket.GetKey(), count, callback);
        }

        public void RedeemRewards(string bucket, int count) {
            RedeemRewards(bucket, count, null);
        }

        public void RedeemRewards(string bucket, int count, BranchRedeemRewardsCallback callback) {
            BranchServerRedeemRewards request = new BranchServerRedeemRewards(bucket, count, callback);
            request.RequestType = RequestTypes.POST;

            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }

        public void UserCompletedAction(string action, JObject metadata) {
            UserCompletedAction(action, metadata, null);
        }

        public void UserCompletedAction(string action) {
            UserCompletedAction(action, null, null);
        }

        //TODO CHANGE CALLBACK
        public void UserCompletedAction(string action, Action callback) {
            UserCompletedAction(action, null, callback);
        }

        //TODO CHANGE CALLBACK
        public void UserCompletedAction(string action, JObject metadata, Action callback) {
            BranchServerActionCompleted request = new BranchServerActionCompleted(action, metadata);
            request.RequestType = RequestTypes.POST;

            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }

        internal void ShareLink(DataTransferManager dataTransferManager, CoreDispatcher dispatcher, BranchShareLinkBuilder builder) {
            //Cancel any existing sharing in progress.
            if (branchShareLinkManager != null) {
                branchShareLinkManager.CancelShareLinkDialog();
            }
            branchShareLinkManager = new BranchShareLinkManager();
            branchShareLinkManager.ShareLink(dataTransferManager, dispatcher, builder);
        }

        public void CancelShareLinkDialog() {
            if (branchShareLinkManager != null) {
                branchShareLinkManager.CancelShareLinkDialog();
            }
        }

        public void GetCreditHistory(BranchGetRewardHistoryCallback callback) {
            GetCreditHistory(string.Empty, string.Empty, 100, CreditHistoryOrder.KMostRecentFirst, callback);
        }

        public void GetCreditHistory(string bucket, BranchGetRewardHistoryCallback callback) {
            GetCreditHistory(bucket, string.Empty, 100, CreditHistoryOrder.KMostRecentFirst, callback);
        }

        public void GetCreditHistory(string afterID, int length, CreditHistoryOrder order, BranchGetRewardHistoryCallback callback) {
            GetCreditHistory(string.Empty, afterID, length, order, callback);
        }

        public void GetCreditHistory(string bucket, string afterId, int length, CreditHistoryOrder order, BranchGetRewardHistoryCallback callback) {
            BranchServerGetRewardHistory request = new BranchServerGetRewardHistory(bucket, afterId, length, order, callback);
            request.RequestType = RequestTypes.GET;

            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }

        public int GetCredits() {
            return LibraryAdapter.GetPrefHelper().GetCreditCount();
        }

        private bool HasUser() {
            return !string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetIdentityId());
        }

        private bool HasDeviceFingerPrint() {
            return !string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
        }

        private bool HasSession() {
            return !string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetSessionId());
        }

        internal string GenerateShortLinkInternal(BranchServerCreateUrl request) {
            return request.RunAsync().Result.ResponseAsText;
        }

        internal void GenerateShortLinkInternalWithCallback(BranchServerCreateUrl request) {
            BranchServerRequestQueue.AddRequest(request);
            BranchServerRequestQueue.RunQueue();
        }

        public void ResetUserSession() {
            initState = SessionState.Uninitialised;
        }

        private JObject AppendDebugParams(JObject originalParams) {
            try {
                if (originalParams != null && deeplinkDebugParams != null) {
                    if (deeplinkDebugParams.Count > 0) {
                        Debug.WriteLine("You're currently in deep link debug mode. Please comment out 'setDeepLinkDebugMode' to receive the deep link parameters from a real Branch link");
                    }
                    foreach(JProperty prop in deeplinkDebugParams.Properties()) {
                        originalParams.Add(prop.Name, prop);
                    }
                }
            } catch (Exception ignore) {
            }
            return originalParams;
        }

        private JObject ConvertParamsStringToDictionary(string paramString) {
            if (string.IsNullOrEmpty(paramString)) {
                return new JObject();
            } else {
                return JObject.Parse(paramString);
            }
        }
    }
}
