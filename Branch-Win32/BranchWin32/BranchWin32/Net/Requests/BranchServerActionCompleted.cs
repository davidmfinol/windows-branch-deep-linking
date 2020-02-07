using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests
{
    public class BranchServerActionCompleted : BranchServerRequest
    {
        public BranchServerActionCompleted(string action, BranchCommerceEvent commerceEvent, Dictionary<string, object> metadata)
        {
            this.requestPath = BranchEnumUtils.GetPath(Enum.RequestPath.CompletedAction);

            Dictionary<string, object> post = new Dictionary<string, object>();
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.IdentityID), LibraryAdapter.GetPrefHelper().GetIdentityId());
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.DeviceFingerprintID), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.SessionID), LibraryAdapter.GetPrefHelper().GetSessionId());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.LinkClickID), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }
            post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Event), action);

            if (metadata != null) {
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Metadata), metadata);
            }

            if (commerceEvent != null) {
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.CommerceData), commerceEvent.GetCommerceJSONObject());
            }

            SetPost(post);

            if (string.IsNullOrEmpty(action) && action.ToLower().Equals(BranchEnumUtils.GetEventName(BranchStandartEvent.PURCHASE).ToLower()) && commerceEvent == null) {
                Debug.WriteLine("Warning: You are sending a purchase event with our non-dedicated purchase function. Please see function sendCommerceEvent");
            }
        }
    }
}
