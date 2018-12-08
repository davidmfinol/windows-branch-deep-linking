using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerRegisterOpen : BranchServerRequest {
        private BranchInitCallbackWrapper callback;

        public BranchServerRegisterOpen(BranchInitCallbackWrapper callback, string url) {
            this.callback = callback;
            this.requestPath = BranchEnumUtils.GetPath(Enum.RequestPath.RegisterOpen);

            Dictionary<string, object> openPost = new Dictionary<string, object>();
            openPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.DeviceFingerprintID), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            openPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.IdentityID), LibraryAdapter.GetPrefHelper().GetIdentityId());
            if(!string.IsNullOrEmpty(url)) openPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.WindowsAppWebLinkUrl), url);
            SetPost(openPost);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("RIGSTER OPEN REQUEST RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            Dictionary<string, object> responseAsJson = (Dictionary<string, object>)Json.Deserialize(responseAsText);

            if (responseAsJson.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.LinkClickID))) {
                LibraryAdapter.GetPrefHelper().SetLinkClickId((string)responseAsJson[BranchEnumUtils.GetKey(BranchJsonKey.LinkClickID)]);
            } else {
                LibraryAdapter.GetPrefHelper().SetLinkClickId(string.Empty);
            }

            if (responseAsJson.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Data))) {
                Dictionary<string, object> dataObj = (Dictionary<string, object>)Json.Deserialize(((string)responseAsJson[BranchEnumUtils.GetKey(BranchJsonKey.Data)]).Replace(@"\",""));
                if (dataObj.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Clicked_Branch_Link)) &&
                    ((bool)dataObj[BranchEnumUtils.GetKey(BranchJsonKey.Clicked_Branch_Link)] == true)) {
                    if (string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetInstallParams())) {
                        if (LibraryAdapter.GetPrefHelper().GetIsReferrable().Equals(1)) {
                            LibraryAdapter.GetPrefHelper().SetInstallParams((string)responseAsJson[BranchEnumUtils.GetKey(BranchJsonKey.Data)]);
                        }
                    }
                }
            }

            if (responseAsJson.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Data))) {
                LibraryAdapter.GetPrefHelper().SetSessionParams((string)responseAsJson[BranchEnumUtils.GetKey(BranchJsonKey.Data)]);
            } else {
                LibraryAdapter.GetPrefHelper().SetSessionParams(string.Empty);
            }

            LibraryAdapter.GetPrefHelper().SetSessionId((string)responseAsJson[BranchEnumUtils.GetKey(BranchJsonKey.SessionID)]);
            LibraryAdapter.GetPrefHelper().SetIdentityId((string)responseAsJson[BranchEnumUtils.GetKey(BranchJsonKey.IdentityID)]);
            LibraryAdapter.GetPrefHelper().SetDeviceFingerPrintId((string)responseAsJson[BranchEnumUtils.GetKey(BranchJsonKey.DeviceFingerprintID)]);

            if (callback != null) callback.Invoke(responseAsText, string.Empty);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            Debug.WriteLine("RIGSTER OPEN REQUEST RESPONSE >>>>>");
            base.OnFailed(errorMessage, statusCode);

            if (callback != null) callback.Invoke(null, errorMessage);
        }

        public override bool PrepareExecuteWithoutTracking() {
            if (PostData == null) return false;

            try {
                PostData.Remove(BranchEnumUtils.GetKey(BranchJsonKey.DeviceFingerprintID));
                PostData.Remove(BranchEnumUtils.GetKey(BranchJsonKey.HardwareID));
                PostData.Remove(BranchEnumUtils.GetKey(BranchJsonKey.IsHardwareIDReal));
                PostData.Remove(BranchEnumUtils.GetKey(BranchJsonKey.LocalIP));
                PostData.Remove(BranchEnumUtils.GetKey(BranchJsonKey.Metadata));
                PostData.Add(BranchEnumUtils.GetKey(BranchJsonKey.TrackingDisable), true);
            } catch (Exception ignore) {
                return false;
            }
            return true;
        }
    }
}
