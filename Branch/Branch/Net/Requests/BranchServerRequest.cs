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
            return LibraryAdapter.GetPrefHelper().GetAPIBaseUrl() + requestPath;
        }

        public void SetPost(JObject postData) {
            this.postData = postData;

            if (ApiVersion == BranchApiVersions.V2) {
                JObject userData = new JObject();
                BranchDeviceInfo.UpdateRequestWithUserData(userData);
                this.postData.Add(BranchJsonKey.UserData.GetKey(), userData);
            } else {
                BranchDeviceInfo.UpdateRequestWithUserData(postData);
            }

            AddCommonParams(postData);

            //Debug.WriteLine(this.postData.ToString());
        }

        public void AddCommonParams(JObject postData) {
            postData.Add(BranchJsonKey.BranchKey.GetKey(), LibraryAdapter.GetPrefHelper().GetBranchKey());
        }

        public async Task<BranchRequestResponse> RunAsync() {
            try {
                HttpClient httpClient = new HttpClient();
                var headers = httpClient.DefaultRequestHeaders;

                if (RequestType == RequestTypes.GET) {
                    Uri requestUri = new Uri(BranchServerRequestQueue.GetUriWithParameters(RequestUrl(), Parameters));
                    HttpResponseMessage httpResponse = new HttpResponseMessage();
                    string httpResponseBody = "";

                    httpResponse = await httpClient.GetAsync(requestUri);
                    httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                    if (httpResponse.IsSuccessStatusCode) {
                        string responseAsText = await httpResponse.Content.ReadAsStringAsync();
                        return new BranchRequestResponse(responseAsText, string.Empty);
                    } else {
                        string rawError = await httpResponse.Content.ReadAsStringAsync();
                        Debug.WriteLine("Request Error: " + rawError);
                        return new BranchRequestResponse(string.Empty, rawError);
                    }
                } else if (RequestType == RequestTypes.POST) {
                    Uri requestUri = new Uri(RequestUrl());
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("utf-8"));

                    HttpContent content = new StringContent(PostData.ToString(), Encoding.UTF8, "application/json");
                    HttpResponseMessage httpResponse = await httpClient.PostAsync(requestUri, content);

                    Debug.WriteLine("Post data: " + PostData);

                    if (httpResponse.IsSuccessStatusCode) {
                        string responseAsText = await httpResponse.Content.ReadAsStringAsync();
                        return new BranchRequestResponse(responseAsText, string.Empty);
                    } else {
                        string rawError = await httpResponse.Content.ReadAsStringAsync();
                        Debug.WriteLine("Request Error: " + rawError);
                        return new BranchRequestResponse(string.Empty, rawError);
                    }
                }
            } catch (Exception e) {
                Debug.WriteLine("Error: " + e.Message + " - " + e.StackTrace);
            }
            return null;
        }

        public virtual void OnSuccess(string responseAsText) {
            Debug.WriteLine("Success request, response: " + responseAsText);
        }

        public virtual void OnFailed(string errorMessage, int statusCode) {
            Debug.WriteLine("Error request, error: " + errorMessage);
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
