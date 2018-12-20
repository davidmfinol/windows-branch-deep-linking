using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerLogout : BranchServerRequest {
        private Branch.BranchLogoutCallback callback;

        public BranchServerLogout(Branch.BranchLogoutCallback callback) {
            this.callback = callback;
            this.requestPath = BranchEnumUtils.GetPath(Enum.RequestPath.Logout);

            Dictionary<string, object> post = new Dictionary<string, object>();
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.IdentityID), LibraryAdapter.GetPrefHelper().GetIdentityId());
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.DeviceFingerprintID), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.SessionID), LibraryAdapter.GetPrefHelper().GetSessionId());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.LinkClickID), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }

            SetPost(post);
        }

        public override void OnSuccess(string responseAsText) {
            try {
                Debug.WriteLine("LOGOUT RESPONSE >>>>>");
                base.OnSuccess(responseAsText);

                Dictionary<string, object> resp = (Dictionary<string, object>)Json.Deserialize(responseAsText);

                LibraryAdapter.GetPrefHelper().SetSessionId((string)resp[BranchEnumUtils.GetKey(BranchJsonKey.SessionID)]);
                LibraryAdapter.GetPrefHelper().SetIdentityId((string)resp[BranchEnumUtils.GetKey(BranchJsonKey.IdentityID)]);
                LibraryAdapter.GetPrefHelper().SetUserUrl((string)resp[BranchEnumUtils.GetKey(BranchJsonKey.Link)]);

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
