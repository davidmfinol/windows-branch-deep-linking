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
    public class BranchServerGetRewards : BranchServerRequest {
        private Branch.BranchGetRewardsCallback callback;

        public BranchServerGetRewards(Branch.BranchGetRewardsCallback callback) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.GetCredits.GetPath();

            JsonObject post = new JsonObject();
            post.Add(BranchJsonKey.Identity.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetIdentity()));
            SetPost(post);

            BranchRequestHelper.MakeRestfulGetRequest(this);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("GET REWARDS RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            JsonObject resp = JsonObject.Parse(responseAsText);
            bool updateListener = false;

            foreach(string key in resp.Keys) {
                int credits = (int)resp[key].GetNumber();
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
