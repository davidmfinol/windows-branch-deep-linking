using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using BranchSdk.Utils;
using System.Diagnostics;
using Windows.Data.Json;

namespace BranchSdk {
    public static class BranchDeviceInfo {
        private static string os;
        private static string osVersion;
        private static string localIp;
        private static string devIdentity;
        private static string hardwareId;
        private static bool isRealHardwareId;

        public static bool IsInit { get; private set; }

        public static void Init() {
            IsInit = true;

            os = LibraryAdapter.GetSystemObserver().GetOS();
            osVersion = LibraryAdapter.GetSystemObserver().GetOSVersion();
            localIp = LibraryAdapter.GetSystemObserver().GetLocalIp();
            devIdentity = LibraryAdapter.GetPrefHelper().GetDeveloperIdentity();
            hardwareId = LibraryAdapter.GetSystemObserver().GetUniqueID(false);
            isRealHardwareId = LibraryAdapter.GetSystemObserver().IsRealHardwareId;
        }

        public static void UpdateRequestWithV2Params(JsonObject requestObj) {
            if (!string.IsNullOrEmpty(hardwareId)) {
                requestObj.Add(BranchJsonKey.HardwareID.GetKey(), JsonValue.CreateStringValue(hardwareId));
                requestObj.Add(BranchJsonKey.IsHardwareIDReal.GetKey(), JsonValue.CreateBooleanValue(isRealHardwareId));
            }

            requestObj.Add(BranchJsonKey.PluginType.GetKey(), JsonValue.CreateStringValue(BranchUtil.GetPluginType()));
            requestObj.Add(BranchJsonKey.PluginVersion.GetKey(), JsonValue.CreateStringValue(BranchUtil.GetPluginVersion()));

            System.Version version = AssemblyUtils.Version;
            string sdkVersion = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);

            requestObj.Add(BranchJsonKey.SDK.GetKey(), JsonValue.CreateStringValue("c-sharp"));
            requestObj.Add(BranchJsonKey.SdkVersion.GetKey(), JsonValue.CreateStringValue(sdkVersion));

            if (!string.IsNullOrEmpty(os)) requestObj.Add(BranchJsonKey.OS.GetKey(), JsonValue.CreateStringValue(os));
            if (!string.IsNullOrEmpty(osVersion)) requestObj.Add(BranchJsonKey.OSVersion.GetKey(), JsonValue.CreateStringValue(osVersion));
            if (!string.IsNullOrEmpty(localIp)) requestObj.Add(BranchJsonKey.LocalIP.GetKey(), JsonValue.CreateStringValue(localIp));
            if (!string.IsNullOrEmpty(devIdentity)) requestObj.Add(BranchJsonKey.DeveloperIdentity.GetKey(), JsonValue.CreateStringValue(devIdentity));
        }

        public static void UpdateRequestWithV1Params(JsonObject requestObj) {
            if (!string.IsNullOrEmpty(hardwareId)) {
                requestObj.Add(BranchJsonKey.HardwareID.GetKey(), JsonValue.CreateStringValue(hardwareId));
                requestObj.Add(BranchJsonKey.IsHardwareIDReal.GetKey(), JsonValue.CreateBooleanValue(isRealHardwareId));
            }

            requestObj.Add(BranchJsonKey.PluginType.GetKey(), JsonValue.CreateStringValue(BranchUtil.GetPluginType()));
            requestObj.Add(BranchJsonKey.PluginVersion.GetKey(), JsonValue.CreateStringValue(BranchUtil.GetPluginVersion()));

            System.Version version = AssemblyUtils.Version;
            string sdkVersion = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build);

            requestObj.Add(BranchJsonKey.SDK.GetKey(), JsonValue.CreateStringValue("c-sharp"));
            requestObj.Add(BranchJsonKey.SdkVersion.GetKey(), JsonValue.CreateStringValue(sdkVersion));

            if (!string.IsNullOrEmpty(os)) requestObj.Add(BranchJsonKey.OS.GetKey(), JsonValue.CreateStringValue(os));
            if (!string.IsNullOrEmpty(osVersion)) requestObj.Add(BranchJsonKey.OSVersion.GetKey(), JsonValue.CreateStringValue(osVersion));
            if (!string.IsNullOrEmpty(localIp)) requestObj.Add(BranchJsonKey.LocalIP.GetKey(), JsonValue.CreateStringValue(localIp));
        }
    }
}
