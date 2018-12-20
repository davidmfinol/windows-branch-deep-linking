using System;
using System.Collections.Generic;
using System.IO;

namespace BranchSdk {
    public static class BranchConfigManager {
        private static Dictionary<string, string> cachedFiles = new Dictionary<string, string>();

        public static void LoadAll() {
            SetupDataPath();
            DirectoryInfo directory = new DirectoryInfo(GetDataPath());
            FileInfo[] fileInfos = directory.GetFiles("*.txt");
            foreach(FileInfo fileInfo in fileInfos) {
                string text = File.ReadAllText(Path.Combine(GetDataPath(), fileInfo.Name));
                cachedFiles.Add(Path.GetFileNameWithoutExtension(fileInfo.Name), text);
                Console.WriteLine("Config loaded, name: {0}, content: {1}", fileInfo.Name, text);
            }
        }

        public static string GetLiveBranchKey() {
            if (!cachedFiles.ContainsKey("live_branch_key")) {
                Console.WriteLine("Please create \"live_branch_key\" txt file with live branch key inside in \"Configs\" folder");
                return string.Empty;
            }
            return cachedFiles["live_branch_key"];
        }

        public static string GetTestBranchKey() {
            if (!cachedFiles.ContainsKey("test_branch_key")) {
                Console.WriteLine("Please create \"test_branch_key\" txt file with test branch key inside in \"Configs\" folder");
                return string.Empty;
            }
            return cachedFiles["test_branch_key"];
        }

        private static string GetDataPath()
        {
            return Path.Combine(Environment.CurrentDirectory, "Configs");
        }

        private static void SetupDataPath()
        {
            if (!DataPathIsSetup()) {
                Directory.CreateDirectory(GetDataPath());
            }
        }

        private static bool DataPathIsSetup()
        {
            return Directory.Exists(GetDataPath());
        }
    }
}
