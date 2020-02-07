using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Windows.Data.Json;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerCreateUrl : BranchServerRequest {
        private Branch.BranchCreateLinkCallback callback;
        private JsonObject linkPost;

        public BranchServerCreateUrl(string alias, int type, int duration, List<string> tags, string channel, string feature, string stage, string campaign, JsonObject parameters, Branch.BranchCreateLinkCallback callback) {
            this.callback = callback;
            this.requestPath = Enum.RequestPath.GetURL.GetPath();

            linkPost = new JsonObject();
            linkPost.Add(BranchJsonKey.IdentityID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetIdentityId()));
            linkPost.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId()));
            linkPost.Add(BranchJsonKey.SessionID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetSessionId()));

            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                linkPost.Add(BranchJsonKey.LinkClickID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetLinkClickId()));
            }

            linkPost.Add(LinkParam.Type.GetKey(), JsonValue.CreateNumberValue(type));
            linkPost.Add(LinkParam.Duration.GetKey(), JsonValue.CreateNumberValue(duration));
            linkPost.Add(LinkParam.Tags.GetKey(), tags.SerializeListAsJson());
            if (!string.IsNullOrEmpty(alias)) linkPost.Add(LinkParam.Alias.GetKey(), JsonValue.CreateStringValue(alias));
            if (!string.IsNullOrEmpty(channel)) linkPost.Add(LinkParam.Channel.GetKey(), JsonValue.CreateStringValue(channel));
            if (!string.IsNullOrEmpty(feature)) linkPost.Add(LinkParam.Feature.GetKey(), JsonValue.CreateStringValue(feature));
            if (!string.IsNullOrEmpty(stage)) linkPost.Add(LinkParam.Stage.GetKey(), JsonValue.CreateStringValue(stage));
            if (!string.IsNullOrEmpty(campaign)) linkPost.Add(LinkParam.Campaign.GetKey(), JsonValue.CreateStringValue(campaign));
            if (parameters != null) linkPost.Add(LinkParam.Data.GetKey(), parameters);

            SetPost(linkPost);
        }

        public override void OnSuccess(string responseAsText) {
            Debug.WriteLine("CREATE URL REQUEST RESPONSE >>>>>");
            base.OnSuccess(responseAsText);

            JsonObject resp = JsonObject.Parse(responseAsText);

            if (callback != null) callback.Invoke(resp["url"].GetString(), null);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            Debug.WriteLine("CREATE URL REQUEST RESPONSE >>>>>");
            base.OnFailed(errorMessage, statusCode);

            if (callback != null) callback.Invoke(string.Empty, new BranchError(errorMessage, statusCode));
        }
    }
}
