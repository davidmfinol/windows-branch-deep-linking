using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Windows.Data.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Net.Requests {
    public class BranchServerRedeemRewards : BranchServerRequest {
        private Branch.BranchRedeemRewardsCallback callback;
        private int actualNumOfCreditsToRedeem = 0;

        public BranchServerRedeemRewards(string bucketName, int numOfCreditsToRedeem, Branch.BranchRedeemRewardsCallback callback) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.RedeemRewards.GetPath();

            int availableCredits = LibraryAdapter.GetPrefHelper().GetCreditCount(bucketName);
            actualNumOfCreditsToRedeem = numOfCreditsToRedeem;

            if(numOfCreditsToRedeem > availableCredits) {
                actualNumOfCreditsToRedeem = availableCredits;
                Debug.WriteLine("Branch Warning: You're trying to redeem more credits than are available. Have you updated loaded rewards");
            }

            JsonObject post = new JsonObject();
            if (actualNumOfCreditsToRedeem > 0) {
                post.Add(BranchJsonKey.IdentityID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetIdentityId()));
                post.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId()));
                post.Add(BranchJsonKey.SessionID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetSessionId()));
                if(!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                    post.Add(BranchJsonKey.LinkClickID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetLinkClickId()));
                }
                post.Add(BranchJsonKey.Bucket.GetKey(), JsonValue.CreateStringValue(bucketName));
                post.Add(BranchJsonKey.Amount.GetKey(), JsonValue.CreateNumberValue(actualNumOfCreditsToRedeem));
            }
            SetPost(post);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("REDEEM REWARDS RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            bool isRedemptionSucceeded = false;
            JsonObject post = PostData;
            if(post != null) {
                if(post.ContainsKey(BranchJsonKey.Bucket.GetKey()) && post.ContainsKey(BranchJsonKey.Amount.GetKey())) {
                    int redeemdCredits = (int)post[BranchJsonKey.Amount.GetKey()].GetNumber();
                    string creditBucket = post[BranchJsonKey.Bucket.GetKey()].GetString();
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
