using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchServerLogEvent : BranchServerRequest {
        public override BranchApiVersions ApiVersion => BranchApiVersions.V2; 

        public BranchServerLogEvent(string requestPath, string eventName, bool isStandartEvent, List<BranchUniversalObject> buoList, Dictionary<string, object> standartProperties, Dictionary<string, object> customProperties) {
            this.requestPath = requestPath;

            Dictionary<string, object> post = new Dictionary<string, object>();
            try {
                post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Name), eventName);
                if (customProperties.Count > 0) {
                    post.Add(BranchEnumUtils.GetKey(BranchJsonKey.CustomData), customProperties);
                }

                if (standartProperties.Count > 0) {
                    post.Add(BranchEnumUtils.GetKey(BranchJsonKey.EventData), standartProperties);
                }
                if (isStandartEvent && buoList.Count > 0) {
                    List<object> contentItemsArray = new List<object>();
                    post.Add(BranchEnumUtils.GetKey(BranchJsonKey.ContentItems), contentItemsArray);
                    foreach (BranchUniversalObject buo in buoList) {
                        contentItemsArray.Add(buo.ConvertToJson());
                    }
                }
                SetPost(post);
            } catch (Exception e) {
                Debug.WriteLine(e.StackTrace);
            }
            UpdateEnvironment(post);
        }
    }
}
