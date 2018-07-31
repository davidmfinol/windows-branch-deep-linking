using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerLogout : BranchServerRequest {
        private Branch.BranchLogoutCallback callback;

        public BranchServerLogout(Branch.BranchLogoutCallback callback) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.Logout.GetPath();

            JObject post = new JObject();
            post.Add(BranchJsonKey.IdentityID.GetKey(), LibraryAdapter.GetPrefHelper().GetIdentityId());
            post.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            post.Add(BranchJsonKey.SessionID.GetKey(), LibraryAdapter.GetPrefHelper().GetSessionId());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                post.Add(BranchJsonKey.LinkClickID.GetKey(), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }

            SetPost(post);
        }

        public override void OnSuccess(string responseAsText) {
            try {
                Debug.WriteLine("LOGOUT RESPONSE >>>>>");
                base.OnSuccess(responseAsText);

                JObject resp = JObject.Parse(responseAsText);

                LibraryAdapter.GetPrefHelper().SetSessionId(resp[BranchJsonKey.SessionID.GetKey()].Value<string>());
                LibraryAdapter.GetPrefHelper().SetIdentityId(resp[BranchJsonKey.IdentityID.GetKey()].Value<string>());
                LibraryAdapter.GetPrefHelper().SetUserUrl(resp[BranchJsonKey.Link.GetKey()].Value<string>());

                LibraryAdapter.GetPrefHelper().SetInstallParams(string.Empty);
                LibraryAdapter.GetPrefHelper().SetSessionParams(string.Empty);
                LibraryAdapter.GetPrefHelper().SetIdentity(string.Empty);
                LibraryAdapter.GetPrefHelper().ClearUserValues();
            } catch (Exception e) {
                Debug.WriteLine(e.StackTrace);
            } finally {
                if (callback != null) callback.Invoke(true, null);
            }
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            if (callback != null) callback.Invoke(true, new BranchError("Logout error. " + errorMessage, statusCode));
        }
    }
}
