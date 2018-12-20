using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using BranchSdk.Utils;
using System.Collections.Generic;

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

        public static void UpdateRequestWithUserData(Dictionary<string, object> requestObj) {
            if (!string.IsNullOrEmpty(hardwareId)) {
                requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.HardwareID), hardwareId);
                requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.IsHardwareIDReal), isRealHardwareId);
            }
            if (!string.IsNullOrEmpty(os)) requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.OS), os);
            if (!string.IsNullOrEmpty(osVersion)) requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.OSVersion), osVersion);
            if (!string.IsNullOrEmpty(localIp)) requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.LocalIP), localIp);
            if (!string.IsNullOrEmpty(devIdentity)) requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.DeveloperIdentity), devIdentity);
            requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.SDK), "windows");
            requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.SdkVersion), AssemblyUtils.Version.ToString(3));
        }

        public static void UpdateRequestWithDeviceParams(Dictionary<string, object> requestObj) {
            if (!string.IsNullOrEmpty(hardwareId)) {
                requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.HardwareID), hardwareId);
                requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.IsHardwareIDReal), isRealHardwareId);
            }
            if (!string.IsNullOrEmpty(os)) requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.OS), os);
            if (!string.IsNullOrEmpty(osVersion)) requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.OSVersion), osVersion);
            if (!string.IsNullOrEmpty(localIp)) requestObj.Add(BranchEnumUtils.GetKey(BranchJsonKey.LocalIP), localIp);
        }
    }
}
