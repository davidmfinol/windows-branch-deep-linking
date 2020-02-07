using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace BranchSdk {
    public static class BranchConfigManager {
        private static Dictionary<string, string> cachedFiles = new Dictionary<string, string>();

        public static async Task LoadAll() {
            StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder assets = await appInstalledFolder.GetFolderAsync("Assets");
            StorageFolder configs = null;

            try {
                configs = await assets.GetFolderAsync("Configs");
            } catch(Exception e) {
                Debug.WriteLine("Please create \"Configs\" folder in \"Assets\" folder and put in it branch key files (\"live_branch_key\" and \"test_branch_key\")");
                return;
            }

            var files = await configs.GetFilesAsync();

            foreach (StorageFile file in files) {
                string text = await FileIO.ReadTextAsync(file);
                cachedFiles.Add(file.DisplayName, text);
                Debug.WriteLine("Config loaded, name: {0}, content: {1}", file.Name, text);
            }
        }

        public static string GetLiveBranchKey() {
            if (!cachedFiles.ContainsKey("live_branch_key")) {
                Debug.WriteLine("Please create \"live_branch_key\" txt file with live branch key inside in \"Configs\" folder in \"Assets\" folder");
                return string.Empty;
            }
            return cachedFiles["live_branch_key"];
        }

        public static string GetTestBranchKey() {
            if (!cachedFiles.ContainsKey("test_branch_key")) {
                Debug.WriteLine("Please create \"test_branch_key\" txt file with test branch key inside in \"Configs\" folder in \"Assets\" folder");
                return string.Empty;
            }
            return cachedFiles["test_branch_key"];
        }
    }
}
