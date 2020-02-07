using Windows.Storage;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System.Diagnostics;
using Windows.Data.Json;
using System.Runtime.InteropServices;

namespace BranchSdk {
    //TODO: It should be extracted into a separate uwp library
    public class BranchPrefHelper : IBranchPrefHelper
    {
        private enum PrefKeyType {
            SessionId,
            IdentityId,
            BranchKey,
            DeviceFingerPrintId,
            UserUrl,
            LinkClickedId,
            SessionParams,
            Identity,
            InstallParams,
            CreditBase,
            Buckets,
            Actions,
            TotalBase,
            UniqueBase,
            IsReferrable,
            NetworkTimeout,
            MaxRetries,
            RetryInterval,
            RequestMetadata,
            TrackingDisabled,
            AnalyticsData,
            DeveloperIdentity
        }

        private Dictionary<PrefKeyType, string> PrefKeyMap = new Dictionary<PrefKeyType, string>() {
            { PrefKeyType.SessionId, "branch_win_session_id" },
            { PrefKeyType.IdentityId, "branch_win_identity_id" },
            { PrefKeyType.DeveloperIdentity, "branch_win_developer_identity" },
            { PrefKeyType.BranchKey, "branch_win_branch_key" },
            { PrefKeyType.DeviceFingerPrintId, "branch_device_finger_print_id_key" },
            { PrefKeyType.UserUrl, "branch_user_url_key" },
            { PrefKeyType.SessionParams, "branch_session_params_key" },
            { PrefKeyType.LinkClickedId, "branch_link_clicked_id_key" },
            { PrefKeyType.Identity, "branch_identity_key" },
            { PrefKeyType.InstallParams, "branch_install_params_key" },
            { PrefKeyType.CreditBase, "branch_credit_key_base_" },
            { PrefKeyType.Buckets, "branch_buckets" },
            { PrefKeyType.Actions, "branch_actions" },
            { PrefKeyType.TotalBase, "branch_total_base_" },
            { PrefKeyType.UniqueBase, "branch_balance_base_" },
            { PrefKeyType.IsReferrable, "branch_is_referrable" },
            { PrefKeyType.NetworkTimeout, "branch_network_timeout" },
            { PrefKeyType.MaxRetries, "branch_max_retries" },
            { PrefKeyType.RetryInterval, "branch_retry_interval" },
            { PrefKeyType.RequestMetadata, "branch_request_metadata" },
            { PrefKeyType.TrackingDisabled, "branch_tracking_disabled" },
            { PrefKeyType.AnalyticsData, "branch_analytics_data" },
        };

        private const string RETRY_INTERVAL = "1000";
        private const string MAX_RETRIES = "3"; 
        private const string NETWORK_TIMEOUT = "5500"; 

        private string sessionId;
        private string identity;
        private string developerIdentity;
        private string identityId;
        private string branchKey;
        private string deviceFingerPrintId;
        private string userUrl;
        private string linkClickId;
        private string sessionParams;
        private string installParams;
        private int isReferrable;
        private Dictionary<string, int> creditCounts = new Dictionary<string, int>();
        private Dictionary<string, int> actionsTotalBase = new Dictionary<string, int>();
        private Dictionary<string, int> actionsUniqueBase = new Dictionary<string, int>();
        private int networkTimeout;
        private int maxRetries;
        private int retryInterval;
        private JsonObject requestMetadata;
        private bool trackingDisabled;
        private JsonObject analyticsData;

        private bool isLoaded = false;

        public string GetAPIBaseUrl() {
            return "https://api.branch.io/";
        }

        public string GetSessionId() {
            return sessionId;
        }

        public string GetIdentity() {
            return identity;
        } 
       
        public string GetIdentityId() {
            return identityId;
        }

        public string GetDeveloperIdentity()
        {
            return developerIdentity;
        }

        public string GetBranchKey() {
            return branchKey;
        }

        public string GetDeviceFingerPrintId() {
            return deviceFingerPrintId;
        }

        public string GetUserUrl() {
            return userUrl;
        }

        public string GetLinkClickId() {
            return linkClickId;
        }

        public string GetSessionParams() {
            return sessionParams;
        }

        public string GetInstallParams() {
            return installParams;
        }

        public int GetIsReferrable() {
            return isReferrable;
        }

        public int GetCreditCount() {
            return GetCreditCount(BranchJsonKey.DefaultBucket.GetKey());
        }

        public int GetCreditCount(string bucket) {
            if (creditCounts.ContainsKey(bucket)) {
                return creditCounts[bucket];
            }
            return 0;
        }

        public int GetNetworkTimeout() {
            return networkTimeout;
        }

        public int GetMaxRetries() {
            return maxRetries;
        }

        public int GetRetryInterval() {
            return retryInterval;
        }

        public string GetRequestMetadata() {
            if (requestMetadata == null) requestMetadata = new JsonObject();
            return requestMetadata.ToString();
        }

        public bool GetTrackingDisable() {
            return trackingDisabled;
        }

        public string GetBranchAnalyticsData() {
            if (analyticsData == null) analyticsData = new JsonObject();
            return analyticsData.ToString();
        }

        public void SetSessionId(string sessionId) {
            this.sessionId = sessionId;
        }

        public void SetIdentity(string identity) {
            this.identity = identity;
        }

        public void SetIdentityId(string identityId) {
            this.identityId = identityId;
        }

        public void SetDeveloperIdentity(string developerIdentity)
        {
            this.developerIdentity = developerIdentity;
        }

        public void SetBranchKey(string branchKey) {
            this.branchKey = branchKey;
        }

        public void SetDeviceFingerPrintId(string deviceFingerPrintId) {
            this.deviceFingerPrintId = deviceFingerPrintId;
        }

        public void SetUserUrl(string userUrl) {
            this.userUrl = userUrl;
        }

        public void SetLinkClickId(string linkClickId) {
            this.linkClickId = linkClickId;
        }

        public void SetSessionParams(string sessionParams) {
            this.sessionParams = sessionParams;
        }

        public void SetInstallParams(string installParams) {
            this.installParams = installParams;
        }

        public void ClearIsReferrable() {
            this.isReferrable = 0;
        }

        public void SetIsReferrable() {
            this.isReferrable = 1;
        }

        public void SetCreditCount(string bucket, int count) {
            if (creditCounts.ContainsKey(bucket)) {
                creditCounts[bucket] = count;
            } else {
                creditCounts.Add(bucket, count);
            }
        }

        public void SetActionTotalCount(string action, int count) {
            if (actionsTotalBase.ContainsKey(action)) {
                actionsTotalBase[action] = count;
            } else {
                actionsTotalBase.Add(action, count);
            }
        }

        public void SetActionUniqueCount(string action, int count) {
            if (actionsUniqueBase.ContainsKey(action)) {
                actionsUniqueBase[action] = count;
            } else {
                actionsUniqueBase.Add(action, count);
            }
        }

        public void SetNetworkTimeout(int networkTimeout) {
            this.networkTimeout = networkTimeout;
        }

        public void SetMaxRetries(int maxRetries) {
            this.maxRetries = maxRetries;
        }

        public void SetRetryInterval(int retryInterval) {
            this.retryInterval = retryInterval;
        }

        public void SetRequestMetadata(string key, string value) {
            if (string.IsNullOrEmpty(key)) return;
            if (requestMetadata == null) requestMetadata = new JsonObject();

            if (requestMetadata.ContainsKey(key) && value == null) {
                requestMetadata.Remove(key);
            }

            requestMetadata.Add(key, JsonValue.CreateStringValue(value));
        }

        public void ClearUserValues() {
            creditCounts = new Dictionary<string, int>();
            actionsTotalBase = new Dictionary<string, int>();
            actionsUniqueBase = new Dictionary<string, int>();
        }

        public void SetTrackingDisable(bool value) {
            trackingDisabled = value;
        }

        public void ClearBranchAnalyticsData() {
            analyticsData = new JsonObject();
        }

        public void SaveBranchAnalyticsData(string analyticsDataRaw) {
            JsonObject analyticsData = JsonObject.Parse(analyticsDataRaw);

            string sessionID = GetSessionId();
            if (!string.IsNullOrEmpty(sessionID)) {
                if (analyticsData == null) {
                    analyticsData = JsonObject.Parse(GetBranchAnalyticsData());
                }
                try {
                    JsonArray viewDataArray;
                    if (analyticsData.ContainsKey(sessionID)) {
                        viewDataArray = analyticsData[sessionID].GetArray();

                    } else {
                        viewDataArray = new JsonArray();
                        analyticsData.Add(sessionID, viewDataArray);
                    }
                    viewDataArray.Add(analyticsData);
                } catch (Exception ignore) {
                }
            }
        }

        public async Task LoadAll() {
            sessionId = await Load(PrefKeyType.SessionId, string.Empty);
            identity = await Load(PrefKeyType.Identity, string.Empty);
            identity = await Load(PrefKeyType.DeveloperIdentity, string.Empty);
            identityId = await Load(PrefKeyType.IdentityId, string.Empty);
            branchKey = await Load(PrefKeyType.BranchKey, string.Empty);
            deviceFingerPrintId = await Load(PrefKeyType.DeviceFingerPrintId, string.Empty);
            userUrl = await Load(PrefKeyType.UserUrl, string.Empty);
            linkClickId = await Load(PrefKeyType.LinkClickedId, string.Empty);
            sessionParams = await Load(PrefKeyType.SessionParams, string.Empty);
            installParams = await Load(PrefKeyType.InstallParams, string.Empty);
            int.TryParse(await Load(PrefKeyType.IsReferrable, "0"), out isReferrable);

            string creditCounts = await Load(PrefKeyType.CreditBase, string.Empty);
            if (!string.IsNullOrEmpty(creditCounts)) {
                try {
                    this.creditCounts = JsonConvert.DeserializeObject<Dictionary<string, int>>(creditCounts);
                } catch (Exception e) {
                    Debug.WriteLine(e.StackTrace);
                }
            }
            string actionsTotalBase = await Load(PrefKeyType.TotalBase, string.Empty);
            if (!string.IsNullOrEmpty(actionsTotalBase)) {
                try {
                    this.actionsTotalBase = JsonConvert.DeserializeObject<Dictionary<string, int>>(actionsTotalBase);
                } catch (Exception e) {
                    Debug.WriteLine(e.StackTrace);
                }
            }

            string actionsUniqueBase = await Load(PrefKeyType.UniqueBase, string.Empty);
            if (!string.IsNullOrEmpty(actionsUniqueBase)) {
                try {
                    this.actionsUniqueBase = JsonConvert.DeserializeObject<Dictionary<string, int>>(actionsUniqueBase);
                } catch (Exception e) {
                    Debug.WriteLine(e.StackTrace);
                }
            }

            int.TryParse(await Load(PrefKeyType.NetworkTimeout, NETWORK_TIMEOUT), out networkTimeout);
            int.TryParse(await Load(PrefKeyType.MaxRetries, MAX_RETRIES), out maxRetries);
            int.TryParse(await Load(PrefKeyType.RetryInterval, RETRY_INTERVAL), out retryInterval);

            string requestMetadata = await Load(PrefKeyType.RequestMetadata, string.Empty);
            if (!string.IsNullOrEmpty(requestMetadata)) {
                this.requestMetadata = JsonObject.Parse(requestMetadata);
            } else {
                this.requestMetadata = new JsonObject();
            }

            bool.TryParse(await Load(PrefKeyType.TrackingDisabled, "false"), out trackingDisabled);
            
            string analyticsData = await Load(PrefKeyType.AnalyticsData, string.Empty);
            if (!string.IsNullOrEmpty(analyticsData)) {
                this.analyticsData = JsonObject.Parse(analyticsData);
            } else {
                this.analyticsData = new JsonObject();
            }

            isLoaded = true;

            Debug.WriteLine("All prefs has beens loaded");
        }

        public async Task SaveAll() {
            await Save(PrefKeyType.SessionId, sessionId);
            await Save(PrefKeyType.Identity, identity);
            await Save(PrefKeyType.DeveloperIdentity, developerIdentity);
            await Save(PrefKeyType.IdentityId, identityId);
            await Save(PrefKeyType.BranchKey, branchKey);
            await Save(PrefKeyType.DeviceFingerPrintId, deviceFingerPrintId);
            await Save(PrefKeyType.UserUrl, userUrl);
            await Save(PrefKeyType.LinkClickedId, linkClickId);
            await Save(PrefKeyType.SessionParams, sessionParams);
            await Save(PrefKeyType.InstallParams, installParams);
            await Save(PrefKeyType.IsReferrable, isReferrable.ToString());
            await Save(PrefKeyType.CreditBase, JsonConvert.SerializeObject(this.creditCounts));
            await Save(PrefKeyType.TotalBase, JsonConvert.SerializeObject(this.actionsTotalBase));
            await Save(PrefKeyType.UniqueBase, JsonConvert.SerializeObject(this.actionsUniqueBase));
            await Save(PrefKeyType.NetworkTimeout, networkTimeout.ToString());
            await Save(PrefKeyType.MaxRetries, maxRetries.ToString());
            await Save(PrefKeyType.RetryInterval, retryInterval.ToString());
            await Save(PrefKeyType.RequestMetadata, requestMetadata.ToString());
            await Save(PrefKeyType.TrackingDisabled, trackingDisabled.ToString());
            await Save(PrefKeyType.AnalyticsData, analyticsData.ToString());
        }

        public bool IsLoaded {
            get {
                return isLoaded;
            }
        }

        #region Base pref logic

        private async Task<string> Load(PrefKeyType key, string defaultValue = "") {
            IStorageFile prefFile = null;
            try {
                prefFile = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(string.Format("{0}{1}", PrefKeyMap[key], ".txt"));
            } catch (FileNotFoundException ignore) { }
            if (prefFile == null) {
                prefFile = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(string.Format("{0}{1}", PrefKeyMap[key], ".txt"), CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(prefFile, defaultValue);
            }
            return await FileIO.ReadTextAsync(prefFile);
        }

        private async Task<string> Load(PrefKeyType key, string postfix, string defaultValue = "") {
            IStorageFile prefFile = null;
            try {
                prefFile = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(string.Format("{0}{1}{2}", PrefKeyMap[key], postfix, ".txt"));
            } catch (FileNotFoundException ignore) { }
            if (prefFile == null) {
                prefFile = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(string.Format("{0}{1}{2}", PrefKeyMap[key], postfix, ".txt"), CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(prefFile, defaultValue);
            }
            return await FileIO.ReadTextAsync(prefFile);
        }

        private async Task Save(PrefKeyType key, string value) {
            StorageFile prefFile = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(string.Format("{0}{1}", PrefKeyMap[key], ".txt"), CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(prefFile, value);
        }

        private async Task Save(PrefKeyType key, string postfix, string value) {
            StorageFile prefFile = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(string.Format("{0}{1}{2}", PrefKeyMap[key], postfix, ".txt"), CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(prefFile, value);
        }

        #endregion
    }
}
