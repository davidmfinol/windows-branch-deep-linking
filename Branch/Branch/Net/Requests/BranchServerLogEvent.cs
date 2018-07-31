using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Net.Requests {
    public class BranchServerLogEvent : BranchServerRequest {
        public override BranchApiVersions ApiVersion => BranchApiVersions.V2; 

        public BranchServerLogEvent(string requestPath, string eventName, bool isStandartEvent, List<BranchUniversalObject> buoList, JObject standartProperties, JObject customProperties) {
            this.requestPath = requestPath;

            JObject post = new JObject();
            try {
                post.Add(BranchJsonKey.Name.GetKey(), eventName);
                if (customProperties.Count > 0) {
                    post.Add(BranchJsonKey.CustomData.GetKey(), customProperties);
                }

                if (standartProperties.Count > 0) {
                    post.Add(BranchJsonKey.EventData.GetKey(), standartProperties);
                }
                if (isStandartEvent && buoList.Count > 0) {
                    JArray contentItemsArray = new JArray();
                    post.Add(BranchJsonKey.ContentItems.GetKey(), contentItemsArray);
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
