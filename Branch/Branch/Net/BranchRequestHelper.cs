using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using BranchSdk.Net.Requests;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Net {
    public static class BranchRequestHelper {
        public static void MakeRestfulGetRequest(BranchServerRequest request) {
            string getParams = string.Empty;
            JObject parameters = request.PostData != null ? request.PostData : new JObject();
            getParams = ConvertJSONtoString(parameters);
            request.RequestParameters = getParams;
        }

        public static void AddCommonParams(JObject postData, string branchKey) {
            if(!postData.ContainsKey(BranchJsonKey.UserData.GetKey())) {
                postData.Add(BranchJsonKey.SDK.GetKey(), string.Format("{0}{1}", "windows", Utils.AssemblyUtils.Version.ToString(3)));
            }
            postData.Add(BranchJsonKey.BranchKey.GetKey(), LibraryAdapter.GetPrefHelper().GetBranchKey());
        }

        public static void UpdateRequestMetadata(JObject data) {
            try {
                JObject metadata = new JObject();
                foreach(JProperty prop in LibraryAdapter.GetPrefHelper().GetRequestMetadata().Properties()) {
                    metadata.Add(prop.Name, prop.Value);
                }

                JObject originalMetadata = null;
                if (data.ContainsKey(BranchJsonKey.Metadata.GetKey())) originalMetadata = data[BranchJsonKey.Metadata.GetKey()].Value<JObject>();

                if (originalMetadata != null) {
                    foreach (JProperty prop in originalMetadata.Properties()) {
                        metadata.Add(prop.Name, prop.Value);
                    }
                }
                data.Add(BranchJsonKey.Metadata.GetKey(), metadata);
            } catch (Exception ignore) {
                Debug.WriteLine("Could not merge metadata, ignoring user metadata.");
            }
        }

        private static string ConvertJSONtoString(JObject json) {
            StringBuilder result = new StringBuilder();
            if (json != null) {
                bool first = true;
                foreach(JProperty prop in json.Properties()) {
                    if (first) {
                        result.Append("?");
                        first = false;
                    } else {
                        result.Append("&");
                    }

                    if (json[prop.Name].Type == JTokenType.Array) {
                        result.Append(prop.Name).Append("=").Append(json[prop.Name].Value<JArray>().ToString());
                    } else if (json[prop.Name].Type == JTokenType.Object) {
                        result.Append(prop.Name).Append("=").Append(json[prop.Name].Value<JObject>().ToString());
                    } else {
                        result.Append(prop.Name).Append("=").Append(json[prop.Name].Value<string>());
                    }
                }
            }

            return result.ToString();
        }
    }
}
