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
    public class BranchServerActionCompleted : BranchServerRequest {
        public BranchServerActionCompleted(string action, JObject metadata) {
            this.requestPath = Enum.RequestPath.CompletedAction.GetPath();

            JObject post = new JObject();
            post.Add(BranchJsonKey.IdentityID.GetKey(), LibraryAdapter.GetPrefHelper().GetIdentityId());
            post.Add(BranchJsonKey.DeviceFingerprintID.GetKey(), LibraryAdapter.GetPrefHelper().GetDeviceFingerPrintId());
            post.Add(BranchJsonKey.SessionID.GetKey(), LibraryAdapter.GetPrefHelper().GetSessionId());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetPrefHelper().GetLinkClickId())) {
                post.Add(BranchJsonKey.LinkClickID.GetKey(), LibraryAdapter.GetPrefHelper().GetLinkClickId());
            }
            post.Add(BranchJsonKey.Event.GetKey(), action);
            if(metadata != null) {
                post.Add(BranchJsonKey.Metadata.GetKey(), metadata);
            }

            SetPost(post);

            if (string.IsNullOrEmpty(action) && action.ToLower().Equals("purchase")) {
                Debug.WriteLine("Warning: You are sending a purchase event with our non-dedicated purchase function. Please see function sendCommerceEvent");
            }
        }
    }
}
