using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Windows.Data.Json;
using System;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerLogout : BranchServerRequest {
        private Branch.BranchLogoutCallback callback;

        public BranchServerLogout(Branch.BranchLogoutCallback callback) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.Logout.GetPath();

            JsonObject post = new JsonObject();
            post.Add(BranchJsonKey.IdentityID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetIdentityId()));
            post.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId()));
            post.Add(BranchJsonKey.SessionID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetSessionId()));
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                post.Add(BranchJsonKey.LinkClickID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetLinkClickId()));
            }

            SetPost(post);
        }

        public override void OnSuccess(string responseAsText) {
            try {
                Debug.WriteLine("LOGOUT RESPONSE >>>>>");
                base.OnSuccess(responseAsText);

                JsonObject resp = JsonObject.Parse(responseAsText);

                LibraryAdapter.GetPrefHelper().SetSessionId(resp[BranchJsonKey.SessionID.GetKey()].GetString());
                LibraryAdapter.GetPrefHelper().SetIdentityId(resp[BranchJsonKey.IdentityID.GetKey()].GetString());
                LibraryAdapter.GetPrefHelper().SetUserUrl(resp[BranchJsonKey.Link.GetKey()].GetString());

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
