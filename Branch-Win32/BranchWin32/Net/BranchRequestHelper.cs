using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using BranchSdk.Net.Requests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BranchSdk.Net {
    public static class BranchRequestHelper {
        public static void MakeRestfulGetRequest(BranchServerRequest request) {
            string getParams = string.Empty;
            Dictionary<string, object> parameters = request.PostData != null ? request.PostData : new Dictionary<string, object>();
            getParams = ConvertJSONtoString(parameters);
            request.RequestParameters = getParams;
        }

        public static void AddCommonParams(Dictionary<string, object> postData, string branchKey) {
            postData.Add(BranchEnumUtils.GetKey(BranchJsonKey.BranchKey), LibraryAdapter.GetPrefHelper().GetBranchKey());
        }

        public static void UpdateRequestMetadata(Dictionary<string, object> data) {
            try {
                Dictionary<string, object> metadata = new Dictionary<string, object>();
                Dictionary<string, object> requestMetadata = (Dictionary<string, object>)Json.Deserialize(LibraryAdapter.GetPrefHelper().GetRequestMetadata());

                foreach(string key in requestMetadata.Keys) {
                    metadata.Add(key, requestMetadata[key]);
                }

                Dictionary<string, object> originalMetadata = null;
                if (data.ContainsKey(BranchEnumUtils.GetKey(BranchJsonKey.Metadata))) originalMetadata = (Dictionary<string, object>)data[BranchEnumUtils.GetKey(BranchJsonKey.Metadata)];

                if (originalMetadata != null) {
                    foreach (string key in originalMetadata.Keys) {
                        metadata.Add(key, originalMetadata[key]);
                    }
                    data.Remove(BranchEnumUtils.GetKey(BranchJsonKey.Metadata));
                }

                data.Add(BranchEnumUtils.GetKey(BranchJsonKey.Metadata), metadata);
            } catch (Exception ignore) {
                Console.WriteLine("Could not merge metadata, ignoring user metadata.");
            }
        }

        private static string ConvertJSONtoString(Dictionary<string, object> json) {
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
