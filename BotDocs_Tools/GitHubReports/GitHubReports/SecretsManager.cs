using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json.Serialization;
using System.Net.Http.Headers;

namespace GitHubReports
{
    /// <summary>A complete kludge to make up for the missing user application settings features.</summary>
    public static class SecretsManager
    {
        private const string SecretsFolder = "B4A3713C-6560-4FF9-A421-33B139EFBAF4";

        private static string FilePath { get; }

        public static string GitHubUsername { get; } = nameof(GitHubUsername);
        public static string GitHubUserToken { get; } = nameof(GitHubUserToken);
        public static string TextAnalyticsSubscription { get; } = nameof(TextAnalyticsSubscription);

        private static Dictionary<string, string> _secrets;

        private static bool Dirty = false;

        static SecretsManager()
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SecretsFolder);
            FilePath = Path.Combine(dir, "secrets.json");

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            if (!File.Exists(FilePath))
            {
                _secrets = new Dictionary<string, string>();
                Save();
            }
            else
            {
                Read();
            }
        }

        private static void Read()
        {
            using (Stream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                _secrets = JsonSerializer.ReadAsync<Dictionary<string, string>>(stream).Result;
            }
        }

        public static void Save()
        {
            if (!Dirty) return;
            using (Stream stream = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                JsonSerializer.WriteAsync(_secrets, stream).Wait();
            }
        }

        public static string Get(string id) => _secrets[id];

        public static bool TryGet(string id, out string value) => _secrets.TryGetValue(id, out value);

        public static void Set(string id, string value)
        {
            if (_secrets.ContainsKey(id) && _secrets[id].Equals(value)) return;

            _secrets[id] = value;
            Dirty = true;
        }

        public static bool ContainsKey(string id) => _secrets.ContainsKey(id);

        public static void Clear()
        {
            if (_secrets.Count > 0)
            {
                _secrets.Clear();
                Dirty = true;
            }
        }
    }
}
