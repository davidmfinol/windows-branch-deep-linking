using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerRedeemRewards : BranchServerRequest {
        private Branch.BranchRedeemRewardsCallback callback;
        private int actualNumOfCreditsToRedeem = 0;

        public BranchServerRedeemRewards(string bucketName, int numOfCreditsToRedeem, Branch.BranchRedeemRewardsCallback callback) {
            this.callback = callback;
            this.requestPath = BranchEnumUtils.GetPath(Enum.RequestPath.RedeemRewards);

            int availableCredits = LibraryAdapter.GetPrefHelper().GetCreditCount(bucketName);
            actualNumOfCreditsToRedeem = numOfCreditsToRedeem;

            if(numOfCreditsToRedeem > availableCredits) {
                actualNumOfCreditsToRedeem = availableCredits;
                Debug.WriteLine("Branch Warning: You're trying to redeem more credits than are available. Have you updated loaded rewards");
            }

            Dictionary<string, object> post = new Dictionary<string, object>();
            if (actualNumOfCreditsToRedeem > 0) {
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.IdentityID), LibraryAdapter.GetPrefHelper().GetIdentityId());
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.DeviceFingerprintID), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.SessionID), LibraryAdapter.GetPrefHelper().GetSessionId());
                if(!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                    post.Add(BranchEnumUtils.GetKey(BranchJsonKey.LinkClickID), LibraryAdapter.GetPrefHelper().GetLinkClickId());
                }
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Bucket), bucketName);
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Amount), actualNumOfCreditsToRedeem);
            }
            SetPost(post);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("REDEEM REWARDS RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            bool isRedemptionSucceeded = false;
            Dictionary<string, object> post = PostData;
            if(post != null) {
                if(post.ContainsKey(BranchJsonKey.Bucket.ToString()) && post.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Amount))) {
                    int redeemdCredits = Convert.ToInt32(post[BranchEnumUtils.GetKey(BranchJsonKey.Amount)]);
                    string creditBucket = (string)post[BranchEnumUtils.GetKey(BranchJsonKey.Bucket)];
                    isRedemptionSucceeded = redeemdCredits > 0;

                    int updatedCreditCount = LibraryAdapter.GetPrefHelper().GetCreditCount(creditBucket) - redeemdCredits;
                    LibraryAdapter.GetPrefHelper().SetCreditCount(creditBucket, updatedCreditCount);
                }
            }

            if (callback != null) {
                BranchError branchError = isRedemptionSucceeded ? null : new BranchError("Trouble redeeming rewards.", BranchError.ERR_BRANCH_REDEEM_REWARD);
                callback.Invoke(isRedemptionSucceeded, branchError);
            }
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            if (callback != null) callback.Invoke(false, new BranchError("Trouble redeeming rewards. " + errorMessage, statusCode));
        }
    }
}
