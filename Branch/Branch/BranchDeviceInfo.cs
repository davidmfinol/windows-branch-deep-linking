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
            devIdentity = LibraryAdapter.GetPrefHelper().GetIdentity();
            hardwareId = LibraryAdapter.GetSystemObserver().GetUniqueID(false);
            isRealHardwareId = LibraryAdapter.GetSystemObserver().IsRealHardwareId;
        }

        public static void UpdateRequestWithUserData(JsonObject requestObj) {
            if (!string.IsNullOrEmpty(hardwareId)) {
                requestObj.Add(BranchJsonKey.HardwareID.GetKey(), JsonValue.CreateStringValue(hardwareId));
                requestObj.Add(BranchJsonKey.IsHardwareIDReal.GetKey(), JsonValue.CreateBooleanValue(isRealHardwareId));
            }
            if (!string.IsNullOrEmpty(os)) requestObj.Add(BranchJsonKey.OS.GetKey(), JsonValue.CreateStringValue(os));
            if (!string.IsNullOrEmpty(osVersion)) requestObj.Add(BranchJsonKey.OSVersion.GetKey(), JsonValue.CreateStringValue(osVersion));
            if (!string.IsNullOrEmpty(localIp)) requestObj.Add(BranchJsonKey.LocalIP.GetKey(), JsonValue.CreateStringValue(localIp));
            if (!string.IsNullOrEmpty(devIdentity)) requestObj.Add(BranchJsonKey.DeveloperIdentity.GetKey(), JsonValue.CreateStringValue(devIdentity));
            requestObj.Add(BranchJsonKey.SDK.GetKey(), JsonValue.CreateStringValue("windows"));
            requestObj.Add(BranchJsonKey.SdkVersion.GetKey(), JsonValue.CreateStringValue(AssemblyUtils.Version.ToString(3)));
        }

        public static void UpdateRequestWithDeviceParams(JsonObject requestObj) {
            if (!string.IsNullOrEmpty(hardwareId)) {
                requestObj.Add(BranchJsonKey.HardwareID.GetKey(), JsonValue.CreateStringValue(hardwareId));
                requestObj.Add(BranchJsonKey.IsHardwareIDReal.GetKey(), JsonValue.CreateBooleanValue(isRealHardwareId));
            }
            if (!string.IsNullOrEmpty(os)) requestObj.Add(BranchJsonKey.OS.GetKey(), JsonValue.CreateStringValue(os));
            if (!string.IsNullOrEmpty(osVersion)) requestObj.Add(BranchJsonKey.OSVersion.GetKey(), JsonValue.CreateStringValue(osVersion));
            if (!string.IsNullOrEmpty(localIp)) requestObj.Add(BranchJsonKey.LocalIP.GetKey(), JsonValue.CreateStringValue(localIp));
        }
    }
}
