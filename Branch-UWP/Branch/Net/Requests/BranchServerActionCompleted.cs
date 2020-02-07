using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Windows.Data.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Net.Requests
{
    public class BranchServerActionCompleted : BranchServerRequest
    {
        public BranchServerActionCompleted(string action, BranchCommerceEvent commerceEvent, JsonObject metadata)
        {
            this.requestPath = Enum.RequestPath.CompletedAction.GetPath();

            JsonObject post = new JsonObject();
            post.Add(BranchJsonKey.IdentityID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetIdentityId()));
            post.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId()));
            post.Add(BranchJsonKey.SessionID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetSessionId()));
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                post.Add(BranchJsonKey.LinkClickID.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetLinkClickId()));
            }
            post.Add(BranchJsonKey.Event.GetKey(), JsonValue.CreateStringValue(action));

            if (metadata != null) {
                post.Add(BranchJsonKey.Metadata.GetKey(), metadata);
            }

            if (commerceEvent != null) {
                post.Add(BranchJsonKey.CommerceData.GetKey(), commerceEvent.GetCommerceJSONObject());
            }

            SetPost(post);

            if (string.IsNullOrEmpty(action) && action.ToLower().Equals(BranchStandartEvent.PURCHASE.GetEventName().ToLower()) && commerceEvent == null) {
                Debug.WriteLine("Warning: You are sending a purchase event with our non-dedicated purchase function. Please see function sendCommerceEvent");
            }
        }
    }
}
