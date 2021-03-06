using System;
using System.IO;
using System.Collections.Generic;
using BranchSdk.Enum;
using System.Diagnostics;
using System.Text;
using BranchSdk.Utils;

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
        private Dictionary<string, object> requestMetadata;
        private bool trackingDisabled;
        private Dictionary<string, object> analyticsData;

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
            return GetCreditCount(BranchEnumUtils.GetKey(BranchJsonKey.DefaultBucket));
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
            if (requestMetadata == null) requestMetadata = new Dictionary<string, object>();
            return Json.Serialize(requestMetadata);
        }

        public bool GetTrackingDisable() {
            return trackingDisabled;
        }

        public string GetBranchAnalyticsData() {
            if (analyticsData == null) analyticsData = new Dictionary<string, object>();
            return Json.Serialize(analyticsData);
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
            if (requestMetadata == null) requestMetadata = new Dictionary<string, object>();

            if (requestMetadata.ContainsKey(key) && value == null) {
                requestMetadata.Remove(key);
            }

            requestMetadata.Add(key, value);
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
            analyticsData = new Dictionary<string, object>();
        }

        public void SaveBranchAnalyticsData(string analyticsDataRaw) {
            Dictionary<string, object> analyticsData = (Dictionary<string, object>)Json.Deserialize(analyticsDataRaw);

            string sessionID = GetSessionId();
            if (!string.IsNullOrEmpty(sessionID)) {
                if (analyticsData == null) {
                    analyticsData = (Dictionary<string, object>)Json.Deserialize(GetBranchAnalyticsData());
                }
                try {
                    List<object> viewDataArray;
                    if (analyticsData.ContainsKey(sessionID)) {
                        viewDataArray = (List<object>)analyticsData[sessionID];

                    } else {
                        viewDataArray = new List<object>();
                        analyticsData.Add(sessionID, viewDataArray);
                    }
                    viewDataArray.Add(analyticsData);
                } catch (Exception ignore) {
                }
            }
        }

        public void LoadAll() {
            sessionId = Load(PrefKeyType.SessionId, string.Empty);
            identity = Load(PrefKeyType.Identity, string.Empty);
            identity = Load(PrefKeyType.DeveloperIdentity, string.Empty);
            identityId = Load(PrefKeyType.IdentityId, string.Empty);
            branchKey = Load(PrefKeyType.BranchKey, string.Empty);
            deviceFingerPrintId = Load(PrefKeyType.DeviceFingerPrintId, string.Empty);
            userUrl = Load(PrefKeyType.UserUrl, string.Empty);
            linkClickId = Load(PrefKeyType.LinkClickedId, string.Empty);
            sessionParams = Load(PrefKeyType.SessionParams, string.Empty);
            installParams = Load(PrefKeyType.InstallParams, string.Empty);
            int.TryParse(Load(PrefKeyType.IsReferrable, "0"), out isReferrable);

            string creditCounts = Load(PrefKeyType.CreditBase, string.Empty);
            if (!string.IsNullOrEmpty(creditCounts)) {
                try {
                    this.creditCounts = ObjectUtils.DictObjectToDictInt((Dictionary<string, object>)Json.Deserialize(creditCounts));
                } catch (Exception e) {
                    Console.WriteLine(e.StackTrace);
                }
            }
            string actionsTotalBase = Load(PrefKeyType.TotalBase, string.Empty);
            if (!string.IsNullOrEmpty(actionsTotalBase)) {
                try {
                    this.actionsTotalBase = ObjectUtils.DictObjectToDictInt((Dictionary<string, object>)Json.Deserialize(actionsTotalBase));
                } catch (Exception e) {
                    Console.WriteLine(e.StackTrace);
                }
            }

            string actionsUniqueBase = Load(PrefKeyType.UniqueBase, string.Empty);
            if (!string.IsNullOrEmpty(actionsUniqueBase)) {
                try {
                    this.actionsUniqueBase = ObjectUtils.DictObjectToDictInt((Dictionary<string, object>)Json.Deserialize(actionsUniqueBase));
                } catch (Exception e) {
                    Console.WriteLine(e.StackTrace);
                }
            }

            int.TryParse(Load(PrefKeyType.NetworkTimeout, NETWORK_TIMEOUT), out networkTimeout);
            int.TryParse(Load(PrefKeyType.MaxRetries, MAX_RETRIES), out maxRetries);
            int.TryParse(Load(PrefKeyType.RetryInterval, RETRY_INTERVAL), out retryInterval);

            string requestMetadata = Load(PrefKeyType.RequestMetadata, string.Empty);
            if (!string.IsNullOrEmpty(requestMetadata)) {
                this.requestMetadata = (Dictionary<string, object>)Json.Deserialize(requestMetadata);
            } else {
                this.requestMetadata = new Dictionary<string, object>();
            }

            bool.TryParse(Load(PrefKeyType.TrackingDisabled, "false"), out trackingDisabled);
            
            string analyticsData = Load(PrefKeyType.AnalyticsData, string.Empty);
            if (!string.IsNullOrEmpty(analyticsData)) {
                this.analyticsData = (Dictionary<string, object>)Json.Deserialize(analyticsData);
            } else {
                this.analyticsData = new Dictionary<string, object>();
            }

            isLoaded = true;

            Console.WriteLine("All prefs has beens loaded");
        }

        public void SaveAll() {
            Save(PrefKeyType.SessionId, sessionId);
            Save(PrefKeyType.Identity, identity);
            Save(PrefKeyType.DeveloperIdentity, developerIdentity);
            Save(PrefKeyType.IdentityId, identityId);
            Save(PrefKeyType.BranchKey, branchKey);
            Save(PrefKeyType.DeviceFingerPrintId, deviceFingerPrintId);
            Save(PrefKeyType.UserUrl, userUrl);
            Save(PrefKeyType.LinkClickedId, linkClickId);
            Save(PrefKeyType.SessionParams, sessionParams);
            Save(PrefKeyType.InstallParams, installParams);
            Save(PrefKeyType.IsReferrable, isReferrable.ToString());
            Save(PrefKeyType.CreditBase, Json.Serialize(this.creditCounts));
            Save(PrefKeyType.TotalBase, Json.Serialize(this.actionsTotalBase));
            Save(PrefKeyType.UniqueBase, Json.Serialize(this.actionsUniqueBase));
            Save(PrefKeyType.NetworkTimeout, networkTimeout.ToString());
            Save(PrefKeyType.MaxRetries, maxRetries.ToString());
            Save(PrefKeyType.RetryInterval, retryInterval.ToString());
            Save(PrefKeyType.RequestMetadata, Json.Serialize(requestMetadata));
            Save(PrefKeyType.TrackingDisabled, trackingDisabled.ToString());
            Save(PrefKeyType.AnalyticsData, Json.Serialize(analyticsData));
        }

        public bool IsLoaded {
            get {
                return isLoaded;
            }
        }

        #region Base pref logic

        private string Load(PrefKeyType key, string defaultValue = "")
        {
            SetupDataPath();
            string path = Path.Combine(GetDataPath(), string.Format("{0}{1}", PrefKeyMap[key], ".txt"));

            if (!File.Exists(path)) {
                File.Create(path).Dispose();
            }

            return File.ReadAllText(path, Encoding.UTF8).Trim();
        }

        private string Load(PrefKeyType key, string postfix, string defaultValue = "") {
            SetupDataPath();
            string path = Path.Combine(GetDataPath(), string.Format("{0}{1}{2}", PrefKeyMap[key], postfix, ".txt"));

            if (!File.Exists(path)) {
                File.Create(path).Dispose();
            }

            return File.ReadAllText(path, Encoding.UTF8).Trim();
        }

        private void Save(PrefKeyType key, string value) {
            SetupDataPath();
            string path = Path.Combine(GetDataPath(), string.Format("{0}{1}", PrefKeyMap[key], ".txt"));

            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fs)) {
                sw.Write(value);
            }
        }

        private void Save(PrefKeyType key, string postfix, string value) {
            SetupDataPath();
            string path = Path.Combine(GetDataPath(), string.Format("{0}{1}{2}", PrefKeyMap[key], postfix, ".txt"));

            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fs)) {
                sw.Write(value);
            }
        }

        private string GetDataPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Branch");
        }

        private void SetupDataPath()
        {
            if (!DataPathIsSetup()) {
                Directory.CreateDirectory(GetDataPath());
            }
        }

        private bool DataPathIsSetup()
        {
            return Directory.Exists(GetDataPath());
        }

        #endregion
    }
}