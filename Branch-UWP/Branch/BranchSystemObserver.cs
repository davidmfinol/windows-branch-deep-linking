using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.System.Profile;
using Windows.System.UserProfile;

namespace BranchSdk {
    //TODO: It should be extracted into a separate uwp library
    public class BranchSystemObserver : IBranchSystemObserver {
        private bool isRealHardwareId = true;

        public bool IsRealHardwareId {
            get {
                return isRealHardwareId;
            }
        }

        public string GetUniqueID(bool debug) {
            string windowsId = null;
            if (!debug && !Branch.I.IsSimulatingInstalls) {
                windowsId = AdvertisingManager.AdvertisingId;
            }
            if (string.IsNullOrEmpty(windowsId)) {
                windowsId = Guid.NewGuid().ToString();
                isRealHardwareId = false;
            }
            return windowsId;
        }

        public string GetLocalIp() {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return null;
            var hostname =
                NetworkInformation.GetHostNames()
                    .FirstOrDefault(
                        hn =>
                            hn.Type == HostNameType.Ipv4 &&
                            hn.IPInformation?.NetworkAdapter != null &&
                            hn.IPInformation.NetworkAdapter.NetworkAdapterId == icp.NetworkAdapter.NetworkAdapterId);

            // the ip address
            return hostname?.CanonicalName;
        }

        public string GetOS() {
            string deviceFamilyVersion = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong version = ulong.Parse(deviceFamilyVersion);
            ulong major = (version & 0xFFFF000000000000L) >> 48;
            return $"Windows {major}";
        }

        public string GetRawOSVersion() {
            return AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
        }

        public string GetOSVersion() {
            string sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
            ulong v = ulong.Parse(sv);
            ulong v1 = (v & 0xFFFF000000000000L) >> 48;
            ulong v2 = (v & 0x0000FFFF00000000L) >> 32;
            return $"{v1}.{v2}";
        }
    }
}
