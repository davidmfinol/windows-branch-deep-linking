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
    public class BranchServerGetRewards : BranchServerRequest {
        private Branch.BranchGetRewardsCallback callback;

        public BranchServerGetRewards(Branch.BranchGetRewardsCallback callback) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.GetCredits.GetPath();

            JObject post = new JObject();
            post.Add(BranchJsonKey.Identity.GetKey(), LibraryAdapter.GetPrefHelper().GetIdentity());
            SetPost(post);

            BranchRequestHelper.MakeRestfulGetRequest(this);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("GET REWARDS RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            JObject resp = JObject.Parse(responseAsText);
            bool updateListener = false;

            foreach(JProperty prop in resp.Properties()) {
                int credits = resp[prop.Name].Value<int>();
                if (credits != LibraryAdapter.GetPrefHelper().GetCreditCount(prop.Name)) {
                    updateListener = true;
                }
                Debug.WriteLine(credits);
                LibraryAdapter.GetPrefHelper().SetCreditCount(prop.Name, credits);
            }

            if (callback != null) callback.Invoke(updateListener, null);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            if (callback != null) callback.Invoke(false, new BranchError("Trouble retrieving user credits. " + errorMessage, statusCode));
        }
    }
}
