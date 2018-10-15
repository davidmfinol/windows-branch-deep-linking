using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Windows.Data.Json;
using System.Diagnostics;

namespace BranchSdk.Net.Requests
{
    public class BranchServerIdentifyUser : BranchServerRequest
    {
        private Branch.BranchIdentityUserCallback callback;

        public BranchServerIdentifyUser(Branch.BranchIdentityUserCallback callback, string userId)
        {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.IdentifyUser.GetPath();

            JsonObject identityPost = new JsonObject();

            identityPost.Add(BranchJsonKey.IdentityID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetIdentityId()));
            identityPost.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId()));
            identityPost.Add(BranchJsonKey.SessionID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetSessionId()));
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                identityPost.Add(BranchJsonKey.LinkClickID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetLinkClickId()));
            }
            identityPost.Add(BranchJsonKey.Identity.GetKey(), JsonValue.CreateStringValue(userId));

            SetPost(identityPost);
        }

        public override void OnSuccess(string responseAsText)
        {
            Debug.WriteLine("IDENTITY USER RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            JsonObject resp = JsonObject.Parse(responseAsText);

            if (PostData != null && PostData.ContainsKey(BranchJsonKey.Identity.GetKey())) {
                LibraryAdapter.GetPrefHelper().SetIdentity(PostData[BranchJsonKey.Identity.GetKey()].GetString());
            }

            LibraryAdapter.GetPrefHelper().SetIdentityId(resp[BranchJsonKey.IdentityID.GetKey()].GetString());
            LibraryAdapter.GetPrefHelper().SetUserUrl(resp[BranchJsonKey.Link.GetKey()].GetString());

            if (resp.ContainsKey(BranchJsonKey.ReferringData.GetKey())) {
                string parameters = resp[BranchJsonKey.ReferringData.GetKey()].GetString();
                LibraryAdapter.GetPrefHelper().SetInstallParams(parameters);
            }

            if (callback != null) callback.Invoke(Branch.I.GetFirstParams(), null);
        }

        public override void OnFailed(string errorMessage, int statusCode)
        {
            Debug.WriteLine("IDENTITY USER RESPONSE >>>>>");
            base.OnFailed(errorMessage, statusCode);

            if (callback != null) callback.Invoke(null, new BranchError(errorMessage, statusCode));
        }
    }
}
