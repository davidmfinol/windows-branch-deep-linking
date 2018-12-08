using System;

namespace TestbedWindows {
    public static class DeepLinkHandler {
        public static event Action<string> OnAppDeepLinkEvent = delegate { };

        public static void OnAppDeepLink(string url) {
            OnAppDeepLinkEvent.Invoke(url);
        }
    }
}
