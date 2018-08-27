using Newtonsoft.Json.Linq;

namespace BranchSdk {
    public static class BranchUtil {
        public static bool IsCustomDebugEnabled = false;

        public static JObject FormatLinkParam(JObject parameters) {
            return AddSource(parameters);
        }

        public static JObject AddSource(JObject parameters) {
            if (parameters == null) {
                parameters = new JObject();
            }
            parameters.Add("source", "windows"); //TODO: It's temp value
            return parameters;
        }
    }
}
