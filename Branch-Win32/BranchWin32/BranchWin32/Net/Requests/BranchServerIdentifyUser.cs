using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests
{
    public class BranchServerIdentifyUser : BranchServerRequest
    {
        private Branch.BranchIdentityUserCallback callback;

        public BranchServerIdentifyUser(Branch.BranchIdentityUserCallback callback, string userId)
        {
            this.callback = callback;
            this.requestPath = BranchEnumUtils.GetPath(Enum.RequestPath.IdentifyUser);

            Dictionary<string, object> identityPost = new Dictionary<string, object>();

            identityPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.IdentityID), LibraryAdapter.GetPrefHelper().GetIdentityId());
            identityPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.DeviceFingerprintID), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            identityPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.SessionID), LibraryAdapter.GetPrefHelper().GetSessionId());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                identityPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.LinkClickID), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }
            identityPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.Identity), userId);

            SetPost(identityPost);
        }

        public override void OnSuccess(string responseAsText)
        {
            Debug.WriteLine("IDENTITY USER RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            Dictionary<string, object> resp = (Dictionary<string, object>)Json.Deserialize(responseAsText);

            if (PostData != null && PostData.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Identity))) {
                LibraryAdapter.GetPrefHelper().SetIdentity((string)PostData[BranchEnumUtils.GetKey(BranchJsonKey.Identity)]);
            }

            LibraryAdapter.GetPrefHelper().SetIdentityId((string)resp[BranchEnumUtils.GetKey(BranchJsonKey.IdentityID)]);
            LibraryAdapter.GetPrefHelper().SetUserUrl((string)resp[BranchEnumUtils.GetKey(BranchJsonKey.Link)]);

            if (resp.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.ReferringData))) {
                string parameters = (string)resp[BranchEnumUtils.GetKey(BranchJsonKey.ReferringData)];
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
