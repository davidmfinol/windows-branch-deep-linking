using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerGetRewards : BranchServerRequest {
        private Branch.BranchGetRewardsCallback callback;

        public BranchServerGetRewards(Branch.BranchGetRewardsCallback callback) {
            this.callback = callback;
            this.requestPath = BranchEnumUtils.GetPath(Enum.RequestPath.GetCredits);

            Dictionary<string, object> post = new Dictionary<string, object>();
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Identity), LibraryAdapter.GetPrefHelper().GetIdentity());
            SetPost(post);

            BranchRequestHelper.MakeRestfulGetRequest(this);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("GET REWARDS RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            Dictionary<string, object> resp = (Dictionary<string, object>)Json.Deserialize(responseAsText);
            bool updateListener = false;

            foreach(string key in resp.Keys) {
                int credits = Convert.ToInt32(resp[key]);
                if (credits != LibraryAdapter.GetPrefHelper().GetCreditCount(key)) {
                    updateListener = true;
                }
                Debug.WriteLine(credits);
                LibraryAdapter.GetPrefHelper().SetCreditCount(key, credits);
            }

            if (callback != null) callback.Invoke(updateListener, null);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            if (callback != null) callback.Invoke(false, new BranchError("Trouble retrieving user credits." + errorMessage, statusCode));
        }
    }
}
