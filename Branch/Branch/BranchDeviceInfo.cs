using BranchSdk.CrossPlatform;
using BranchSdk.Enum;
using Newtonsoft.Json.Linq;

namespace BranchSdk {
    public static class BranchDeviceInfo {
        public static void UpdateRequestWithUserData(JObject requestObj) {
            if (!string.IsNullOrEmpty(LibraryAdapter.GetSystemObserver().GetOS())) requestObj.Add(BranchJsonKey.OS.GetKey(), LibraryAdapter.GetSystemObserver().GetOS());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetSystemObserver().GetOSVersion())) requestObj.Add(BranchJsonKey.OSVersion.GetKey(), LibraryAdapter.GetSystemObserver().GetOSVersion());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetSystemObserver().GetLocalIp())) requestObj.Add(BranchJsonKey.LocalIP.GetKey(), LibraryAdapter.GetSystemObserver().GetLocalIp());

            string devIdentity = LibraryAdapter.GetPrefHelper().GetIdentity();
            if(!string.IsNullOrEmpty(devIdentity)) {
                requestObj.Add(BranchJsonKey.DeveloperIdentity.GetKey(), devIdentity);
            }
        }

        public static void UpdateRequestWithDeviceParams(JObject requestObj) {
            if (!string.IsNullOrEmpty(LibraryAdapter.GetSystemObserver().GetOS())) requestObj.Add(BranchJsonKey.OS.GetKey(), LibraryAdapter.GetSystemObserver().GetOS());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetSystemObserver().GetOSVersion())) requestObj.Add(BranchJsonKey.OSVersion.GetKey(), LibraryAdapter.GetSystemObserver().GetOSVersion());
            if (!string.IsNullOrEmpty(LibraryAdapter.GetSystemObserver().GetLocalIp())) requestObj.Add(BranchJsonKey.LocalIP.GetKey(), LibraryAdapter.GetSystemObserver().GetLocalIp());
        }
    }
}
