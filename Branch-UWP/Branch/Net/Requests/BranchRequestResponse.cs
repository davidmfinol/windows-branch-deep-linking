namespace BranchSdk.Net.Requests {
    public class BranchRequestResponse {
        public string ResponseAsText;
        public string ErrorAsText;

        public BranchRequestResponse(string responseAsText, string errorAsText) {
            ResponseAsText = responseAsText;
            ErrorAsText = errorAsText;
        }
    }
}
