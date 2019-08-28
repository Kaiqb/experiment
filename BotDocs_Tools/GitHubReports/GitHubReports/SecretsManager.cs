using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace GitHubReports
{
    /// <summary>A hand-rolled app settings manager.</summary>
    public static class SecretsManager
    {
        // Use a GUID to identify the settings file for this app.
        // TODO Move this to the CLI project, and accept it here as a parameter. That will require
        //      using an instance, instead of just the static methods.
        private const string SecretsFolder = "B4A3713C-6560-4FF9-A421-33B139EFBAF4";

        /// <summary>The absolute file path.</summary>
        private static string FilePath { get; }

        // TODO these strings should also move to the CLI project.

        /// <summary>ID for the user's GitHub user name.</summary>
        public static string GitHubUsername { get; } = nameof(GitHubUsername);

        /// <summary>ID for the user's GitHub user access token.</summary>
        public static string GitHubUserToken { get; } = nameof(GitHubUserToken);

        /// <summary>ID for the user's Cognitive Services Text Analytics subscription.</summary>
        public static string TextAnalyticsSubscription { get; } = nameof(TextAnalyticsSubscription);

        /// <summary>Contains the secrets, indexed by ID.</summary>
        private static Dictionary<string, string> _secrets;

        /// <summary>Indicates whether any values might have changed since the file was last read or
        /// saved.</summary>
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

        /// <summary>Reads the secrets from the file.</summary>
        private static void Read()
        {
            using (Stream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                _secrets = JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream).Result;
            }

            Dirty = false;
        }

        /// <summary>Saves the secrets to the file.</summary>
        public static void Save()
        {
            if (!Dirty) return;

            using (Stream stream = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                JsonSerializer.SerializeAsync<Dictionary<string, string>>(stream, _secrets).Wait();
            }

            Dirty = false;
        }

        /// <summary>Retrieves a secret by its ID.</summary>
        /// <param name="id">The ID of the secret.</param>
        /// <returns>The secret's value.</returns>
        /// <remarks>The dictionary can throw a key not found exception.</remarks>
        public static string Get(string id) => _secrets[id];

        /// <summary>Attempts to retrieve a secret by its ID.</summary>
        /// <param name="id">The ID of the secret.</param>
        /// <param name="value">On success, the secret's value.</param>
        /// <returns>True if the value was successfully retrieved; otherwise, false.</returns>
        public static bool TryGet(string id, out string value) => _secrets.TryGetValue(id, out value);

        /// <summary>Sets the value of a secret.</summary>
        /// <param name="id">The ID of the secret.</param>
        /// <param name="value">The secret's value.</param>
        /// <remarks>To persist any changes, separately call <see cref="Save"/>.</remarks>
        public static void Set(string id, string value)
        {
            if (_secrets.ContainsKey(id) && _secrets[id].Equals(value)) return;

            _secrets[id] = value;
            Dirty = true;
        }

        /// <summary>Indicates whether the dictionary contains a specific ID.</summary>
        /// <param name="id">The ID to check for.</param>
        /// <returns>True if the dictionary contains the key; otherwise, false.</returns>
        public static bool ContainsKey(string id) => _secrets.ContainsKey(id);

        /// <summary>Clears the secrets dictionary.</summary>
        /// <remarks>To persist any changes, separately call <see cref="Save"/>.</remarks>
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
