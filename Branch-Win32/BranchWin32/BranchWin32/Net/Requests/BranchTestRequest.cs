using System;
using System.Diagnostics;

namespace BranchSdk.Net.Requests {
    public class BranchTestRequest : BranchServerRequest {
        private Action<string> callback;
        string url = "";

        public BranchTestRequest(string url, Action<string> callback) {
            this.url = url;
            this.callback = callback;
        }

        public override string RequestUrl() {
            return url;
        }

        public override void OnSuccess(string responseAsText) {
            base.OnSuccess(responseAsText);
            if (callback != null) callback.Invoke(responseAsText);
        }

        public override void OnFailed(string errorMessage, int statusCode) {
            base.OnFailed(errorMessage, statusCode);
            if (callback != null) callback.Invoke(errorMessage);
        }
    }
}
