using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BranchSdk.Net.Requests
{
    public class BranchServerRequest
    {
        public enum BranchApiVersions
        {
            V1,
            V2
        }

        public Dictionary<string, string> Parameters = new Dictionary<string, string>();

        public RequestTypes RequestType { get; set; }

        protected Dictionary<string, object> postData;
        protected string requestPath;

        public string RequestPath {
            get {
                return requestPath;
            }
        }

        public string RequestParameters {
            get; set;
        }

        public Dictionary<string, object> PostData {
            get {
                return postData;
            }
        }

        public virtual BranchApiVersions ApiVersion {
            get {
                return BranchApiVersions.V1;
            }
        }

        public virtual string RequestUrl()
        {
            //api base url need take from pref helper
            return "https://api.branch.io/" + requestPath + RequestParameters;
        }

        public void SetPost(Dictionary<string, object> postData)
        {
            this.postData = postData;

            if (this.postData == null) this.postData = new Dictionary<string, object>();

            if (ApiVersion == BranchApiVersions.V2) {
                Dictionary<string, object> userData = new Dictionary<string, object>();
                BranchDeviceInfo.UpdateRequestWithUserData(userData);
                this.postData.Add(BranchEnumUtils.GetKey(BranchJsonKey.UserData), userData);
            } else {
                BranchDeviceInfo.UpdateRequestWithDeviceParams(this.postData);
            }

            BranchRequestHelper.AddCommonParams(this.postData, LibraryAdapter.GetPrefHelper().GetBranchKey());
            BranchRequestHelper.UpdateRequestMetadata(this.postData);
        }

        public void RunAsync(Action<BranchRequestResponse> callback)
        {
            BranchServerRequestQueue.HandleRequest(this, callback);
        }

        public virtual void OnSuccess(string responseAsText)
        {
            //Console.WriteLine("Success request, response: " + responseAsText);
        }

        public virtual void OnFailed(string errorMessage, int statusCode)
        {
            //Console.WriteLine("Error request, error: " + errorMessage);
        }

        public virtual bool PrepareExecuteWithoutTracking()
        {
            return false;
        }

        protected void UpdateEnvironment(Dictionary<string, object> post)
        {
            try {
                string environment = BranchEnumUtils.GetKey(BranchJsonKey.InstantApp);
                if (ApiVersion == BranchApiVersions.V2) {
                    Dictionary<string, object> userData = (Dictionary<string, object>)post[BranchEnumUtils.GetKey(BranchJsonKey.UserData)];
                    if (userData != null) {
                        userData.Add(BranchEnumUtils.GetKey(BranchJsonKey.Environment), environment);
                    }
                } else {
                    post.Add(BranchEnumUtils.GetKey(BranchJsonKey.Environment), environment);
                }
            } catch (Exception ignore) {
            }
        }
    }
}

namespace BranchSdk.Net
{
    public enum RequestTypes
    {
        GET, POST
    }
}
