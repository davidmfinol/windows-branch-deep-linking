using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Net.Requests {
    public class BranchServerGetRewardHistory : BranchServerRequest {
        private Branch.BranchGetRewardHistoryCallback callback;

        public BranchServerGetRewardHistory(string bucket, string afterID, int length,
            Branch.CreditHistoryOrder order, Branch.BranchGetRewardHistoryCallback callback) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.GetCreditHistory.GetPath();

            JObject post = new JObject();
            post.Add(BranchJsonKey.IdentityID.GetKey(), LibraryAdapter.GetPrefHelper().GetIdentityId());
            post.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            post.Add(BranchJsonKey.SessionID.GetKey(), LibraryAdapter.GetPrefHelper().GetSessionId());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                post.Add(BranchJsonKey.LinkClickID.GetKey(), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }
            post.Add(BranchJsonKey.Length.GetKey(), length);
            post.Add(BranchJsonKey.Direction.GetKey(), (int)order);

            if (!string.IsNullOrEmpty(bucket)) {
                post.Add(BranchJsonKey.Bucket.GetKey(), bucket);
            }

            if (!string.IsNullOrEmpty(afterID)) {
                post.Add(BranchJsonKey.BeginAfterID.GetKey(), afterID);
            }

            SetPost(post);
            BranchRequestHelper.MakeRestfulGetRequest(this);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("GET REWARD HISTORY RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            if (callback != null) callback.Invoke(JArray.Parse(responseAsText), null);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            if (callback != null) callback.Invoke(null, new BranchError("Trouble retrieving user credit history. " + errorMessage, statusCode));
        }
    }
}
