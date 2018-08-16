﻿using Windows.Storage;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System.Diagnostics;
using Newtonsoft.Json;

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

        private string sessionId;
        private string identity;
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

        public void SetSessionId(string sessionId) {
            this.sessionId = sessionId;
        }

        public void SetIdentity(string identity) {
            this.identity = identity;
        }

        public void SetIdentityId(string identityId) {
            this.identityId = identityId;
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

        public void ClearUserValues() {
            creditCounts = new Dictionary<string, int>();
            actionsTotalBase = new Dictionary<string, int>();
            actionsUniqueBase = new Dictionary<string, int>();
        }

        public async Task LoadAll() {
            sessionId = await Load(PrefKeyType.SessionId, string.Empty);
            identity = await Load(PrefKeyType.Identity, string.Empty);
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

            isLoaded = true;

            Debug.WriteLine("All prefs has beens loaded");
        }

        public async Task SaveAll() {
            await Save(PrefKeyType.SessionId, sessionId);
            await Save(PrefKeyType.Identity, identity);
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
