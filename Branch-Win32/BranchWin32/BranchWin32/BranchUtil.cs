using BranchSdk.Utils;
using System.Collections.Generic;

namespace BranchSdk
{
    public static class BranchUtil
    {
        public static bool IsCustomDebugEnabled = false;

        public static Dictionary<string, object> FormatLinkParam(Dictionary<string, object> parameters)
        {
            return AddSource(parameters);
        }

        public static Dictionary<string, object> AddSource(Dictionary<string, object> parameters)
        {
            if (parameters == null) {
                parameters = new Dictionary<string, object>();
            }
            parameters.Add("source", "windows"); //TODO: It's temp value
            return parameters;
        }

        public static string GetPluginVersion()
        {
            return AssemblyUtils.Version.ToString();
        }

        public static string GetPluginType()
        {
            return "C-Sharp";
        }
    }
}
