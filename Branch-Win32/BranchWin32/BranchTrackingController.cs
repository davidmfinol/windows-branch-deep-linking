using BranchSdk.CrossPlatform;
using BranchSdk.Net;

namespace BranchSdk {
    public static class BranchTrackingController {
        public static bool TrackingDisabled { get; private set; }
        public static bool IsInit { get; private set; }

        public static void Init() {
            IsInit = true;

            UpdateTrackingState();
        }

        public static void DisableTracking(bool disableTracking) {
            if (TrackingDisabled != disableTracking) {
                TrackingDisabled = disableTracking;
                if (disableTracking) {
                    OnTrackingDisabled();
                } else {
                    OnTrackingEnabled();
                }
                LibraryAdapter.GetPrefHelper().SetTrackingDisable(disableTracking);
            }
        }

        public static void UpdateTrackingState() {
            TrackingDisabled = LibraryAdapter.GetPrefHelper().GetTrackingDisable();
        }

        private static void OnTrackingDisabled() {
            // Clear all pending requests
            BranchServerRequestQueue.ClearPendingRequests();

            // Clear any tracking specific preference items
            LibraryAdapter.GetPrefHelper().ClearBranchAnalyticsData();
            LibraryAdapter.GetPrefHelper().SetSessionId(string.Empty);
            LibraryAdapter.GetPrefHelper().SetLinkClickId(string.Empty);
            LibraryAdapter.GetPrefHelper().SetInstallParams(string.Empty);
            LibraryAdapter.GetPrefHelper().SetSessionParams(string.Empty);
        }

        private static void OnTrackingEnabled() {
            //Branch.I.RegisterAppInit(null);
        }
    }
}
