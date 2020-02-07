using BranchSdk.Enum;
using Windows.Data.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Net.Requests {
    public class BranchServerLogEvent : BranchServerRequest {
        public override BranchApiVersions ApiVersion => BranchApiVersions.V2; 

        public BranchServerLogEvent(string requestPath, string eventName, bool isStandartEvent, List<BranchUniversalObject> buoList, JsonObject standartProperties, JsonObject topLevelProperties, JsonObject customProperties) {
            this.requestPath = requestPath;

            JsonObject post = new JsonObject();
            try {
                post.Add(BranchJsonKey.Name.GetKey(), JsonValue.CreateStringValue(eventName));
                if (customProperties.Count > 0) {
                    post.Add(BranchJsonKey.CustomData.GetKey(), customProperties);
                }

                if (standartProperties.Count > 0) {
                    post.Add(BranchJsonKey.EventData.GetKey(), standartProperties);
                }

                if (topLevelProperties.Count > 0) {
                    foreach (string key in topLevelProperties.Keys) {
                        post.Add(key, topLevelProperties[key]);
                    }
                }

                if (isStandartEvent && buoList.Count > 0) {
                    JsonArray contentItemsArray = new JsonArray();
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
