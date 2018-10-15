using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Windows.Data.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Net.Requests
{
    public class BranchServerGetRewardHistory : BranchServerRequest
    {
        private Branch.BranchGetRewardHistoryCallback callback;

        public BranchServerGetRewardHistory(string bucket, string afterID, int length,
            Branch.CreditHistoryOrder order, Branch.BranchGetRewardHistoryCallback callback)
        {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.GetCreditHistory.GetPath();

            JsonObject post = new JsonObject();
            post.Add(BranchJsonKey.IdentityID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetIdentityId()));
            post.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId()));
            post.Add(BranchJsonKey.SessionID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetSessionId()));
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                post.Add(BranchJsonKey.LinkClickID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetLinkClickId()));
            }
            post.Add(BranchJsonKey.Length.GetKey(), JsonValue.CreateNumberValue(length));
            post.Add(BranchJsonKey.Direction.GetKey(), JsonValue.CreateNumberValue((int)order));

            if (!string.IsNullOrEmpty(bucket)) {
                post.Add(BranchJsonKey.Bucket.GetKey(), JsonValue.CreateStringValue(bucket));
            }

            if (!string.IsNullOrEmpty(afterID)) {
                post.Add(BranchJsonKey.BeginAfterID.GetKey(), JsonValue.CreateStringValue(afterID));
            }

            SetPost(post);
            BranchRequestHelper.MakeRestfulGetRequest(this);
        }

        public override void OnSuccess(string responseAsText)
        {
            Debug.WriteLine("GET REWARD HISTORY RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            if (callback != null) callback.Invoke(JsonArray.Parse(responseAsText), null);
        }

        public override void OnFailed(string errorMessage, int statusCode)
        {
            if (callback != null) callback.Invoke(null, new BranchError("Trouble retrieving user credit history. " + errorMessage, statusCode));
        }
    }
}
