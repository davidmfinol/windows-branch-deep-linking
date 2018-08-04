using BranchSdk.CrossPlatform;
using BranchSdk.Net;
using BranchSdk.Net.Requests;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

            if (type == Types.WithDictionary) {
                string serializedJson = jsonData["data"].ToString();
                jsonData = JObject.Parse(serializedJson.Replace(@"\", ""));
                dictionaryCallback.Invoke(jsonData.ToObject<Dictionary<string, object>>(), null);
            } else {
                //todo
            }
        }
    }

    public class Branch {
        #region Singleton

        private static Branch instance;
        public static Branch Instance {
            get {
                if (instance == null) {
                    instance = new Branch();
                    BranchDeviceInfo.Init();
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

        #endregion

        public const int LINK_TYPE_UNLIMITED_USE = 0;
        public const int LINK_TYPE_ONE_TIME_USE = 1;

        private JObject deeplinkDebugParams;

        public delegate void BranchLogoutCallback(bool loggedOut, BranchError error);
        public delegate void BranchIdentityUserCallback(JObject referringParams, BranchError error);
        public delegate void BranchInitCallbackWithDictionary(Dictionary<string, object> parameters, BranchError error);
        public delegate void BranchInitCallbackWithBUO(BranchUniversalObject universalObject, BranchLinkProperties linkProperties, BranchError error);
        public delegate void BranchCreateLinkCallback(string url, BranchError error);

        private enum SessionState {
            Initialised, Initialising, Uninitialised
        }

        private SessionState initState = SessionState.Uninitialised;

        public bool IsSimulatingInstalls;

        public BranchInitCallbackWrapper CachedInitCallback { get; private set; }
        public bool IsFirstSessionInited { get; private set; }

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
                Debug.WriteLine("BranchSDK", "Branch Warning: Please enter your branch_key in your project's res/values/strings.xml!");
                return;
            } else {
                Debug.WriteLine("BranchSDK", "Branch Warning: You are using your test app's Branch Key. Remember to change it to live Branch Key during deployment.");
            }

            CachedInitCallback = callback;

            RegisterAppInit(callback, linkUrl);
        }

        private void RegisterAppInit(BranchInitCallbackWrapper callback, string linkUrl = "") {
            callback.CommonCallback = () => { IsFirstSessionInited = true; };

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

        private bool HasUser() {
            return !string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetIdentityId());
        }

        private bool HasDeviceFingerPrint() {
            return !string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
        }

        private bool HasSession() {
            return !string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetSessionId());
        }

        public string GenerateShortLinkInternal(BranchServerCreateUrl request) {
            return request.RunAsync().Result.ResponseAsText;
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
