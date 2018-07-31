using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerIdentifyUser : BranchServerRequest {
        private Branch.BranchIdentityUserCallback callback;

        public BranchServerIdentifyUser(Branch.BranchIdentityUserCallback callback, string userId) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.IdentifyUser.GetPath();

            JObject identityPost = new JObject();

            identityPost.Add(BranchJsonKey.IdentityID.GetKey(), LibraryAdapter.GetPrefHelper().GetIdentityId());
            identityPost.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            identityPost.Add(BranchJsonKey.SessionID.GetKey(), LibraryAdapter.GetPrefHelper().GetSessionId());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                identityPost.Add(BranchJsonKey.LinkClickID.GetKey(), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }
            identityPost.Add(BranchJsonKey.Identity.GetKey(), userId);

            SetPost(identityPost);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("IDENTITY USER RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            JObject resp = JObject.Parse(responseAsText);

            if (PostData != null && PostData.ContainsKey(BranchJsonKey.Identity.GetKey())) {
                LibraryAdapter.GetPrefHelper().SetIdentity(PostData[BranchJsonKey.Identity.GetKey()].Value<string>());
            }

            LibraryAdapter.GetPrefHelper().SetIdentityId(resp[BranchJsonKey.IdentityID.GetKey()].Value<string>());
            LibraryAdapter.GetPrefHelper().SetUserUrl(resp[BranchJsonKey.Link.GetKey()].Value<string>());

            if (resp.ContainsKey(BranchJsonKey.ReferringData.GetKey())) {
                string parameters = resp[BranchJsonKey.ReferringData.GetKey()].Value<string>();
                LibraryAdapter.GetPrefHelper().SetInstallParams(parameters);
            }

            if (callback != null) callback.Invoke(Branch.I.GetFirstParams(), null);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            Debug.WriteLine("IDENTITY USER RESPONSE >>>>>");
            base.OnFailed(errorMessage, statusCode);

            if (callback != null) callback.Invoke(null, new BranchError(errorMessage, statusCode));
        }
    }
}
