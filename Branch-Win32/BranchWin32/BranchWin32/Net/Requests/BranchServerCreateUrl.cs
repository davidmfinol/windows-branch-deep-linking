using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerCreateUrl : BranchServerRequest {
        private Branch.BranchCreateLinkCallback callback;
        private Dictionary<string, object> linkPost;

        public BranchServerCreateUrl(string alias, int type, int duration, List<string> tags, string channel, string feature, string stage, string campaign, Dictionary<string, object> parameters, Branch.BranchCreateLinkCallback callback) {
            this.callback = callback;
            this.requestPath = BranchEnumUtils.GetPath(Enum.RequestPath.GetURL);

            linkPost = new Dictionary<string, object>();
            linkPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.IdentityID), LibraryAdapter.GetPrefHelper().GetIdentityId());
            linkPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.DeviceFingerprintID), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            linkPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.SessionID), LibraryAdapter.GetPrefHelper().GetSessionId());

            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                linkPost.Add(BranchEnumUtils.GetKey(BranchJsonKey.LinkClickID), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }

            linkPost.Add(BranchEnumUtils.GetKey(LinkParam.Type), type);
            linkPost.Add(BranchEnumUtils.GetKey(LinkParam.Duration), duration);
            linkPost.Add(BranchEnumUtils.GetKey(LinkParam.Tags), Json.Serialize(tags));
            if (!string.IsNullOrEmpty(alias)) linkPost.Add(BranchEnumUtils.GetKey(LinkParam.Alias), alias);
            if (!string.IsNullOrEmpty(channel)) linkPost.Add(BranchEnumUtils.GetKey(LinkParam.Channel), channel);
            if (!string.IsNullOrEmpty(feature)) linkPost.Add(BranchEnumUtils.GetKey(LinkParam.Feature), feature);
            if (!string.IsNullOrEmpty(stage)) linkPost.Add(BranchEnumUtils.GetKey(LinkParam.Stage), stage);
            if (!string.IsNullOrEmpty(campaign)) linkPost.Add(BranchEnumUtils.GetKey(LinkParam.Campaign), campaign);
            if (parameters != null) linkPost.Add(BranchEnumUtils.GetKey(LinkParam.Data), parameters);

            SetPost(linkPost);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("CREATE URL REQUEST RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            Dictionary<string, object> resp = (Dictionary<string, object>)Json.Deserialize(responseAsText);

            if (callback != null) callback.Invoke((string)resp["url"], null);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            Debug.WriteLine("CREATE URL REQUEST RESPONSE >>>>>");
            base.OnFailed(errorMessage, statusCode);

            if (callback != null) callback.Invoke(string.Empty, new BranchError(errorMessage, statusCode));
        }
    }
}
