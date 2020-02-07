using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using BranchSdk.Net.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace BranchSdk.Net {
    public static class BranchRequestHelper {
        public static void MakeRestfulGetRequest(BranchServerRequest request) {
            string getParams = string.Empty;
            JsonObject parameters = request.PostData != null ? request.PostData : new JsonObject();
            getParams = ConvertJSONtoString(parameters);
            request.RequestParameters = getParams;
        }

        public static void AddCommonParams(JsonObject postData, string branchKey) {
            postData.Add(BranchJsonKey.BranchKey.GetKey(), JsonValue.CreateStringValue(LibraryAdapter.GetPrefHelper().GetBranchKey()));
        }

        public static void UpdateRequestMetadata(JsonObject data) {
            try {
                JsonObject metadata = new JsonObject();
                JsonObject requestMetadata = JsonObject.Parse(LibraryAdapter.GetPrefHelper().GetRequestMetadata());

                foreach(string key in requestMetadata.Keys) {
                    metadata.Add(key, requestMetadata[key]);
                }

                JsonObject originalMetadata = null;
                if (data.ContainsKey(BranchJsonKey.Metadata.GetKey())) originalMetadata = data[BranchJsonKey.Metadata.GetKey()].GetObject();

                if (originalMetadata != null) {
                    foreach (string key in originalMetadata.Keys) {
                        metadata.Add(key, originalMetadata[key]);
                    }
                }
                data.Add(BranchJsonKey.Metadata.GetKey(), metadata);
            } catch (Exception ignore) {
                Debug.WriteLine("Could not merge metadata, ignoring user metadata.");
            }
        }

        private static string ConvertJSONtoString(JsonObject json) {
            StringBuilder result = new StringBuilder();
            if (json != null) {
                bool first = true;
                foreach(string key in json.Keys) {
                    if (first) {
                        result.Append("?");
                        first = false;
                    } else {
                        result.Append("&");
                    }

                    string val = json[key].ToString();
                    if (val.Length > 0 && val[0] == '\"') val = val.Remove(0, 1);
                    if (val.Length > 0 && val[val.Length - 1] == '\"') val = val.Remove(val.Length - 1, 1);

                    result.Append(key).Append("=").Append(val);
                }
            }

            return result.ToString();
        }
    }
}
