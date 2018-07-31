using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerRegisterOpen : BranchServerRequest {
        private BranchInitCallbackWrapper callback;

        public BranchServerRegisterOpen(BranchInitCallbackWrapper callback, string url) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.RegisterOpen.GetPath();

            JObject openPost = new JObject();
            openPost.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            openPost.Add(BranchJsonKey.IdentityID.GetKey(), LibraryAdapter.GetPrefHelper().GetIdentityId());
            if(!string.IsNullOrEmpty(url)) openPost.Add(BranchJsonKey.WindowsAppWebLinkUrl.GetKey(), url);
            SetPost(openPost);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("RIGSTER OPEN REQUEST RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            JObject responseAsJson = JObject.Parse(responseAsText);

            if (responseAsJson.ContainsKey(BranchJsonKey.LinkClickID.GetKey())) {
                LibraryAdapter.GetPrefHelper().SetLinkClickId(responseAsJson[BranchJsonKey.LinkClickID.GetKey()].Value<string>());
            } else {
                LibraryAdapter.GetPrefHelper().SetLinkClickId(string.Empty);
            }

            if (responseAsJson.ContainsKey(BranchJsonKey.Data.GetKey())) {
                JObject dataObj = JObject.Parse(responseAsJson[BranchJsonKey.Data.GetKey()].Value<string>().Replace(@"\",""));
                if (dataObj.ContainsKey(BranchJsonKey.Clicked_Branch_Link.GetKey()) &&
                    (dataObj[BranchJsonKey.Clicked_Branch_Link.GetKey()].Value<bool>() == true)) {
                    if (string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetInstallParams())) {
                        if (LibraryAdapter.GetPrefHelper().GetIsReferrable().Equals(1)) {
                            LibraryAdapter.GetPrefHelper().SetInstallParams(responseAsJson[BranchJsonKey.Data.GetKey()].Value<string>());
                        }
                    }
                }
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
            Debug.WriteLine("RIGSTER OPEN REQUEST RESPONSE >>>>>");
            base.OnFailed(errorMessage, statusCode);

            if (callback != null) callback.Invoke(null, errorMessage);
        }
    }
}
