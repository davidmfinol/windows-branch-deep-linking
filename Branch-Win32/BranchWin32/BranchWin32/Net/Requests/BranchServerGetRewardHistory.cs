using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests
{
    public class BranchServerGetRewardHistory : BranchServerRequest
    {
        private Branch.BranchGetRewardHistoryCallback callback;

        public BranchServerGetRewardHistory(string bucket, string afterID, int length,
            Branch.CreditHistoryOrder order, Branch.BranchGetRewardHistoryCallback callback)
        {
            this.callback = callback;
            this.requestPath = BranchEnumUtils.GetPath(Enum.RequestPath.GetCreditHistory);

            Dictionary<string, object> post = new Dictionary<string, object>();
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.IdentityID), LibraryAdapter.GetPrefHelper().GetIdentityId());
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.DeviceFingerprintID), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.SessionID), LibraryAdapter.GetPrefHelper().GetSessionId());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.LinkClickID), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Length), length);
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Direction), (int)order);

            if (!string.IsNullOrEmpty(bucket)) {
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Bucket), bucket);
            }

            if (!string.IsNullOrEmpty(afterID)) {
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.BeginAfterID), afterID);
            }

            SetPost(post);
            BranchRequestHelper.MakeRestfulGetRequest(this);
        }

        public override void OnSuccess(string responseAsText)
        {
            Debug.WriteLine("GET REWARD HISTORY RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            if (callback != null) callback.Invoke((List<object>)Json.Deserialize(responseAsText), null);
        }

        public override void OnFailed(string errorMessage, int statusCode)
        {
            if (callback != null) callback.Invoke(null, new BranchError("Trouble retrieving user credit history. " + errorMessage, statusCode));
        }
    }
}
