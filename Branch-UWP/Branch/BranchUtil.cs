using Windows.Data.Json;

namespace BranchSdk
{
    public static class BranchUtil
    {
        public static bool IsCustomDebugEnabled = false;

        public static JsonObject FormatLinkParam(JsonObject parameters)
        {
            return AddSource(parameters);
        }

        public static JsonObject AddSource(JsonObject parameters)
        {
            if (parameters == null) {
                parameters = new JsonObject();
            }
            parameters.Add("source", JsonValue.CreateStringValue("windows")); //TODO: It's temp value
            return parameters;
        }
    }
}
