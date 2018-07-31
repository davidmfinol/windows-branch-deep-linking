using Windows.Storage;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using BranchSdk.CrossPlatform;

namespace BranchSdk {
    //TODO: It should be extracted into a separate uwp library
    public class BranchPrefHelper : IBranchPrefHelper {
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
            IsReferrable
        }

        private Dictionary<PrefKeyType, string> PrefKeyMap = new Dictionary<PrefKeyType, string>() {
            { PrefKeyType.SessionId, "branch_win_session_id" },
            { PrefKeyType.IdentityId, "branch_win_identity_id" },
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
        };

        public string GetSessionId() {
            return Load(PrefKeyType.SessionId, string.Empty).Result;
        }

        public string GetIdentity() {
            return Load(PrefKeyType.Identity, string.Empty).Result;
        }

        public string GetIdentityId() {
            return Load(PrefKeyType.IdentityId, string.Empty).Result;
        }

        public string GetBranchKey() {
            return Load(PrefKeyType.BranchKey, string.Empty).Result;
        }

        public string GetDeviceFingerPrintId() {
            return Load(PrefKeyType.DeviceFingerPrintId, string.Empty).Result;
        }

        public string GetUserUrl() {
            return Load(PrefKeyType.UserUrl, string.Empty).Result;
        }

        public string GetLinkClickId() {
            return Load(PrefKeyType.LinkClickedId, string.Empty).Result;
        }

        public string GetSessionParams() {
            return Load(PrefKeyType.SessionParams, string.Empty).Result;
        }

        public string GetInstallParams() {
            return Load(PrefKeyType.InstallParams, string.Empty).Result;
        }

        public int GetIsReferrable() {
            return int.Parse(Load(PrefKeyType.IsReferrable, "0").Result);
        }

        public void SetSessionId(string sessionId) {
            Save(PrefKeyType.SessionId, sessionId);
        }

        public void SetIdentity(string identity) {
            Save(PrefKeyType.Identity, identity);
        }

        public void SetIdentityId(string identityId) {
            Save(PrefKeyType.IdentityId, identityId);
        }

        public void SetBranchKey(string branchKey) {
            Save(PrefKeyType.BranchKey, branchKey);
        }

        public void SetDeviceFingerPrintId(string deviceFingerPrintId) {
            Save(PrefKeyType.DeviceFingerPrintId, deviceFingerPrintId);
        }

        public void SetUserUrl(string userUrl) {
            Save(PrefKeyType.UserUrl, userUrl);
        }

        public void SetLinkClickId(string linkClickId) {
            Save(PrefKeyType.LinkClickedId, linkClickId);
        }

        public void SetSessionParams(string sessionParams) {
            Save(PrefKeyType.SessionParams, sessionParams);
        }

        public void SetInstallParams(string parameters) {
            Save(PrefKeyType.InstallParams, parameters);
        }

        public void ClearIsReferrable() {
            Save(PrefKeyType.IsReferrable, "0");
        }

        public void SetIsReferrable() {
            Save(PrefKeyType.IsReferrable, "1");
        }

        public string GetAPIBaseUrl() {
            return "https://api.branch.io/";
        }

        private async Task<string> Load(PrefKeyType key, string defaultValue = "") {
            IStorageFile prefFile = null;
            try {
                prefFile = await ApplicationData.Current.LocalCacheFolder.GetFileAsync(string.Format("{0}{1}", PrefKeyMap[key], ".txt"));
            } catch(FileNotFoundException ignore) { }
            if (prefFile == null) {
                prefFile = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(string.Format("{0}{1}", PrefKeyMap[key], ".txt"), CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(prefFile, defaultValue);
            }
            return await FileIO.ReadTextAsync(prefFile);
        }

        private async void Save(PrefKeyType key, string value) {
            StorageFile prefFile = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(string.Format("{0}{1}", PrefKeyMap[key], ".txt"), CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(prefFile, value);
        }

        private async void Save(PrefKeyType key, string postfix, string value) {
            StorageFile prefFile = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(string.Format("{0}{1}{2}", PrefKeyMap[key], postfix, ".txt"), CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(prefFile, value);
        }

        public void ClearUserValues() {
            List<string> buckets = GetBuckets();
            foreach (string bucket in buckets) {
                SetCreditCount(bucket, 0);
            }
            SetBuckets(new List<string>());

            List<string> actions = GetActions();
            foreach (string action in actions) {
                SetActionTotalCount(action, 0);
                SetActionUniqueCount(action, 0);
            }
            SetActions(new List<string>());
        }

        private List<string> GetActions() {
            string actionList = Load(PrefKeyType.Actions, string.Empty).Result;
            if (string.IsNullOrEmpty(actionList)) {
                return new List<string>();
            } else {
                return DeserializeString(actionList);
            }
        }

        private void SetActions(List<string> actions) {
            if (actions.Count == 0) {
                Save(PrefKeyType.Actions, string.Empty);
            } else {
                Save(PrefKeyType.Actions, SerializeArrayList(actions));
            }
        }

        private List<String> GetBuckets() {
            string bucketList = Load(PrefKeyType.Buckets, string.Empty).Result;
            if (string.IsNullOrEmpty(bucketList)) {
                return new List<string>();
            } else {
                return DeserializeString(bucketList);
            }
        }

        private void SetBuckets(List<string> buckets) {
            if (buckets.Count == 0) {
                Save(PrefKeyType.Buckets, string.Empty);
            } else {
                Save(PrefKeyType.Buckets, SerializeArrayList(buckets));
            }
        }

        public void SetCreditCount(string bucket, int count) {
            List<string> buckets = GetBuckets();
            if (!buckets.Contains(bucket)) {
                buckets.Add(bucket);
                SetBuckets(buckets);
            }
            Save(PrefKeyType.CreditBase, bucket, count.ToString());
        }

        public void SetActionTotalCount(string action, int count) {
            List<string> actions = GetActions();
            if (!actions.Contains(action)) {
                actions.Add(action);
                SetActions(actions);
            }
            Save(PrefKeyType.TotalBase, action, count.ToString());
        }

        public void SetActionUniqueCount(string action, int count) {
            Save(PrefKeyType.UniqueBase, action, count.ToString());
        }

        private List<string> DeserializeString(string list) {
            List<string> strings = new List<string>();
            string[] stringArr = list.Split(new char[] { '"' });
            strings.AddRange(stringArr);
            return strings;
        }

        private string SerializeArrayList(List<string> strings) {
            string retString = "";
            foreach (string value in strings) {
                retString = retString + value + ",";
            }
            retString = retString.Substring(0, retString.Length - 1);
            return retString;
        }
    }
}
