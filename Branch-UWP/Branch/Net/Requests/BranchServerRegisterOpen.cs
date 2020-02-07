using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Windows.Data.Json;
using System;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerRegisterOpen : BranchServerRequest {
        private BranchInitCallbackWrapper callback;

        public BranchServerRegisterOpen(BranchInitCallbackWrapper callback, string url) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.RegisterOpen.GetPath();

            JsonObject openPost = new JsonObject();
            openPost.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId()));
            openPost.Add(BranchJsonKey.IdentityID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetIdentityId()));
            if(!string.IsNullOrEmpty(url)) openPost.Add(BranchJsonKey.WindowsAppWebLinkUrl.GetKey(), JsonValue.CreateStringValue(url));
            SetPost(openPost);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("RIGSTER OPEN REQUEST RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            JsonObject responseAsJson = JsonObject.Parse(responseAsText);

            if (responseAsJson.ContainsKey(BranchJsonKey.LinkClickID.GetKey())) {
                LibraryAdapter.GetPrefHelper().SetLinkClickId(responseAsJson[BranchJsonKey.LinkClickID.GetKey()].GetString());
            } else {
                LibraryAdapter.GetPrefHelper().SetLinkClickId(string.Empty);
            }

            if (responseAsJson.ContainsKey(BranchJsonKey.Data.GetKey())) {
                JsonObject dataObj = JsonObject.Parse(responseAsJson[BranchJsonKey.Data.GetKey()].GetString().Replace(@"\",""));
                if (dataObj.ContainsKey(BranchJsonKey.Clicked_Branch_Link.GetKey()) &&
                    (dataObj[BranchJsonKey.Clicked_Branch_Link.GetKey()].GetBoolean() == true)) {
                    if (string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetInstallParams())) {
                        if (LibraryAdapter.GetPrefHelper().GetIsReferrable().Equals(1)) {
                            LibraryAdapter.GetPrefHelper().SetInstallParams(responseAsJson[BranchJsonKey.Data.GetKey()].GetString());
                        }
                    }
                }
            }

            if (responseAsJson.ContainsKey(BranchJsonKey.Data.GetKey())) {
                LibraryAdapter.GetPrefHelper().SetSessionParams(responseAsJson[BranchJsonKey.Data.GetKey()].GetString());
            } else {
                LibraryAdapter.GetPrefHelper().SetSessionParams(string.Empty);
            }

            LibraryAdapter.GetPrefHelper().SetSessionId(responseAsJson[BranchJsonKey.SessionID.GetKey()].GetString());
            LibraryAdapter.GetPrefHelper().SetIdentityId(responseAsJson[BranchJsonKey.IdentityID.GetKey()].GetString());
            LibraryAdapter.GetPrefHelper().SetDeviceFingerPrintId(responseAsJson[BranchJsonKey.DeviceFingerprintID.GetKey()].GetString());

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
                PostData.Remove(BranchJsonKey.DeviceFingerprintID.GetKey());
                PostData.Remove(BranchJsonKey.HardwareID.GetKey());
                PostData.Remove(BranchJsonKey.IsHardwareIDReal.GetKey());
                PostData.Remove(BranchJsonKey.LocalIP.GetKey());
                PostData.Remove(BranchJsonKey.Metadata.GetKey());
                PostData.Add(BranchJsonKey.TrackingDisable.GetKey(), JsonValue.CreateBooleanValue(true));
            } catch (Exception ignore) {
                return false;
            }
            return true;
        }
    }
}
