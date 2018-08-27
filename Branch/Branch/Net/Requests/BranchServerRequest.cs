using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BranchSdk.Net.Requests {
    public class BranchServerRequest {
        public enum BranchApiVersions {
            V1,
            V2
        }

        public Dictionary<string, string> Parameters = new Dictionary<string, string>();

        public RequestTypes RequestType { get; set; }

        protected JObject postData;
        protected string requestPath;

        public string RequestPath {
            get {
                return requestPath;
            }
        }

        public string RequestParameters {
            get; set;
        }

        public JObject PostData {
            get {
                return postData;
            }
        }

        public virtual BranchApiVersions ApiVersion {
            get {
                return BranchApiVersions.V1;
            }
        }

        public virtual string RequestUrl() {
            return LibraryAdapter.GetPrefHelper().GetAPIBaseUrl() + requestPath + RequestParameters;
        }

        public void SetPost(JObject postData) {
            this.postData = postData;

            if (this.postData == null) this.postData = new JObject();

            if (ApiVersion == BranchApiVersions.V2) {
                JObject userData = new JObject();
                BranchDeviceInfo.UpdateRequestWithUserData(userData);
                this.postData.Add(BranchJsonKey.UserData.GetKey(), userData);
            } else {
                BranchDeviceInfo.UpdateRequestWithUserData(this.postData);
            }

            BranchRequestHelper.AddCommonParams(this.postData, LibraryAdapter.GetPrefHelper().GetBranchKey());
            BranchRequestHelper.UpdateRequestMetadata(this.postData);
        }

        public async Task<BranchRequestResponse> RunAsync() {
            return await BranchServerRequestQueue.HandleRequest(this);
        }

        public virtual void OnSuccess(string responseAsText) {
            Debug.WriteLine("Success request, response: " + responseAsText);
        }

        public virtual void OnFailed(string errorMessage, int statusCode) {
            Debug.WriteLine("Error request, error: " + errorMessage);
        }

        public virtual bool PrepareExecuteWithoutTracking() {
            return false;
        }

        protected void UpdateEnvironment(JObject post) {
            try {
                string environment = BranchJsonKey.InstantApp.GetKey();
                if (ApiVersion == BranchApiVersions.V2) {
                    JObject userData = post[BranchJsonKey.UserData.GetKey()].Value<JObject>();
                    if (userData != null) {
                        userData.Add(BranchJsonKey.Environment.GetKey(), environment);
                    }
                } else {
                    post.Add(BranchJsonKey.Environment.GetKey(), environment);
                }
            } catch (Exception ignore) {
            }
        }
    }
}

namespace BranchSdk.Net {
    public enum RequestTypes {
        GET, POST
    }
}
