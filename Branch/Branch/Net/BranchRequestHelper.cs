using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using BranchSdk.Net.Requests;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            postData.Add(BranchJsonKey.BranchKey.GetKey(), LibraryAdapter.GetPrefHelper().GetBranchKey());
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

                    result.Append(prop.Name).Append("=").Append(json[prop.Name].Value<string>());
                }
            }

            return result.ToString();
        }
    }
}
