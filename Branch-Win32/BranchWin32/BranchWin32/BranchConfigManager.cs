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
            return "key_live_mnzYrzoSgtqi3gAyzkVzqppkxFnNY1jn";
        }

        public static string GetTestBranchKey() {
            return string.Empty;
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
