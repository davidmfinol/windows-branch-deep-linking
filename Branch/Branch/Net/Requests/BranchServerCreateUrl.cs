using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerCreateUrl : BranchServerRequest {
        private Branch.BranchCreateLinkCallback callback;
        private JObject linkPost;

        public BranchServerCreateUrl(string alias, int type, int duration, List<string> tags, string channel, string feature, string stage, string campaign, JObject parameters, Branch.BranchCreateLinkCallback callback) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.GetURL.GetPath();

            linkPost = new JObject();
            linkPost.Add(BranchJsonKey.IdentityID.GetKey(), LibraryAdapter.GetPrefHelper().GetIdentityId());
            linkPost.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            linkPost.Add(BranchJsonKey.SessionID.GetKey(), LibraryAdapter.GetPrefHelper().GetSessionId());

            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                linkPost.Add(BranchJsonKey.LinkClickID.GetKey(), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }

            linkPost.Add(LinkParam.Type.GetKey(), type);
            linkPost.Add(LinkParam.Duration.GetKey(), duration);
            linkPost.Add(new JProperty(LinkParam.Tags.GetKey(), tags));
            linkPost.Add(LinkParam.Alias.GetKey(), alias);
            linkPost.Add(LinkParam.Channel.GetKey(), channel);
            linkPost.Add(LinkParam.Feature.GetKey(), feature);
            linkPost.Add(LinkParam.Stage.GetKey(), stage);
            linkPost.Add(LinkParam.Campaign.GetKey(), campaign);
            linkPost.Add(LinkParam.Data.GetKey(), parameters);

            SetPost(linkPost);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("CREATE URL REQUEST RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            JObject resp = JObject.Parse(responseAsText);

            if (callback != null) callback.Invoke(resp["url"].Value<string>(), null);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            Debug.WriteLine("CREATE URL REQUEST RESPONSE >>>>>");
            base.OnFailed(errorMessage, statusCode);

            if (callback != null) callback.Invoke(string.Empty, new BranchError(errorMessage, statusCode));
        }
    }
}
