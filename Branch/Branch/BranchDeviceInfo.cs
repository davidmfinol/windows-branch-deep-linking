using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

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

        public static void UpdateRequestWithUserData(JObject requestObj) {
            if (!string.IsNullOrEmpty(hardwareId)) {
                requestObj.Add(BranchJsonKey.HardwareID.GetKey(), hardwareId);
                requestObj.Add(BranchJsonKey.IsHardwareIDReal.GetKey(), isRealHardwareId);
            }
            if (!string.IsNullOrEmpty(os)) requestObj.Add(BranchJsonKey.OS.GetKey(), os);
            if (!string.IsNullOrEmpty(osVersion)) requestObj.Add(BranchJsonKey.OSVersion.GetKey(), osVersion);
            if (!string.IsNullOrEmpty(localIp)) requestObj.Add(BranchJsonKey.LocalIP.GetKey(), localIp);
            if (!string.IsNullOrEmpty(devIdentity)) requestObj.Add(BranchJsonKey.DeveloperIdentity.GetKey(), devIdentity);
        }

        public static void UpdateRequestWithDeviceParams(JObject requestObj) {
            if (!string.IsNullOrEmpty(hardwareId)) {
                requestObj.Add(BranchJsonKey.HardwareID.GetKey(), hardwareId);
                requestObj.Add(BranchJsonKey.IsHardwareIDReal.GetKey(), isRealHardwareId);
            }
            if (!string.IsNullOrEmpty(os)) requestObj.Add(BranchJsonKey.OS.GetKey(), os);
            if (!string.IsNullOrEmpty(osVersion)) requestObj.Add(BranchJsonKey.OSVersion.GetKey(), osVersion);
            if (!string.IsNullOrEmpty(localIp)) requestObj.Add(BranchJsonKey.LocalIP.GetKey(), localIp);
        }
    }
}
