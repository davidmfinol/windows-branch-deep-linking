using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerRegisterInstall : BranchServerRequest {
        private BranchInitCallbackWrapper callback;

        public BranchServerRegisterInstall(BranchInitCallbackWrapper callback, string installID) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.RegisterInstall.GetPath();

            JObject installPost = new JObject();
            if (!string.IsNullOrEmpty(installID)) installPost.Add(BranchJsonKey.LinkClickID.GetKey(), installID);
            SetPost(installPost);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("RIGSTER INSTALL REQUEST RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            JObject responseAsJson = JObject.Parse(responseAsText);

            LibraryAdapter.GetPrefHelper().SetUserUrl(responseAsJson[BranchJsonKey.Link.GetKey()].Value<string>());

            if (responseAsJson.ContainsKey(BranchJsonKey.Data.GetKey())) {
                JObject dataObj = JObject.Parse(responseAsJson[BranchJsonKey.Data.GetKey()].Value<string>().Replace(@"\", ""));
                if (dataObj.ContainsKey(BranchJsonKey.Clicked_Branch_Link.GetKey()) &&
                    (dataObj[BranchJsonKey.Clicked_Branch_Link.GetKey()].Value<bool>() == true)) {
                    if (string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetInstallParams())) {
                        if (LibraryAdapter.GetPrefHelper().GetIsReferrable().Equals(1)) {
                            LibraryAdapter.GetPrefHelper().SetInstallParams(responseAsJson[BranchJsonKey.Data.GetKey()].Value<string>());
                        }
                    }
                }
            }

            if (responseAsJson.ContainsKey(BranchJsonKey.LinkClickID.GetKey())) {
                LibraryAdapter.GetPrefHelper().SetLinkClickId(responseAsJson[BranchJsonKey.LinkClickID.GetKey()].Value<string>());
            } else {
                LibraryAdapter.GetPrefHelper().SetLinkClickId(string.Empty);
            }

            if (responseAsJson.ContainsKey(BranchJsonKey.Data.GetKey())) {
                LibraryAdapter.GetPrefHelper().SetSessionParams(responseAsJson[BranchJsonKey.Data.GetKey()].Value<string>());
            } else {
                LibraryAdapter.GetPrefHelper().SetSessionParams(string.Empty);
            }

            LibraryAdapter.GetPrefHelper().SetSessionId(responseAsJson[BranchJsonKey.SessionID.GetKey()].Value<string>());
            LibraryAdapter.GetPrefHelper().SetIdentityId(responseAsJson[BranchJsonKey.IdentityID.GetKey()].Value<string>());
            LibraryAdapter.GetPrefHelper().SetDeviceFingerPrintId(responseAsJson[BranchJsonKey.DeviceFingerprintID.GetKey()].Value<string>());

            if (callback != null) callback.Invoke(JObject.Parse(responseAsText), string.Empty);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            Debug.WriteLine("RIGSTER INSTALL REQUEST RESPONSE >>>>>");
            base.OnFailed(errorMessage, statusCode);

            if (callback != null) callback.Invoke(null, errorMessage);
        }

        public override bool PrepareExecuteWithoutTracking() {
            if (PostData == null) return false;

            try {
                PostData.Remove(BranchJsonKey.DeviceFingerprintID.GetKey());
                PostData.Remove(BranchJsonKey.HardwareID.GetKey());
                PostData.Remove(BranchJsonKey.IsHardwareIDReal.GetKey());
                PostData.Remove(BranchJsonKey.LocalIP.GetKey());
                PostData.Remove(BranchJsonKey.Metadata.GetKey());
                PostData.Add(BranchJsonKey.TrackingDisable.GetKey(), true);
            } catch (Exception ignore) {
                return false;
            }
            return true;
        }
    }
}
