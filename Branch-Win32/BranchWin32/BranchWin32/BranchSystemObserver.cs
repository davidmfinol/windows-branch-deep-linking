using BranchSdk.Utils;
using System;

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
            if (string.IsNullOrEmpty(windowsId)) {
                windowsId = CpuID.GetCPUID();
                isRealHardwareId = false;
            }
            return windowsId;
        }

        public string GetLocalIp() {
            return NetUtils.GetLocalIPAddress();
        }

        public string GetOS() {
            return "Windows";
        }

        public string GetRawOSVersion() {
            return WindowsUtils.GetOSVersion();
        }

        public string GetOSVersion() {
            return WindowsUtils.GetOSVersion();
        }
    }
}
