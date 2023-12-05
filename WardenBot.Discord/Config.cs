using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WardenBot.Discord
{
    public static class Config
    {
        private static bool _loadedEnv = false;

        private static string Required([CallerMemberName] string fieldName = "")
        {
            return Environment.GetEnvironmentVariable(fieldName)
                ?? throw new InvalidOperationException($"The environment variable {fieldName} does not exist!");
        }

        [return: NotNullIfNotNull(nameof(defaultValue))]
        private static string? Optional(string? defaultValue = null, [CallerMemberName] string fieldName = "")
        {
            return Environment.GetEnvironmentVariable(fieldName)
                ?? defaultValue;
        }

        public static bool Validate(out List<string> missingVariables)
        {
            LoadEnv();

            missingVariables = new();

            PropertyInfo[] propertyInfos = typeof(Config)
                .GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                bool isRequired = propertyInfo.GetCustomAttribute<RequiredAttribute>() is not null;
                if (!isRequired) { continue; }

                bool exists = !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(propertyInfo.Name));
                if (exists) { continue; }

                missingVariables.Add(propertyInfo.Name);
            }

            return !missingVariables.Any();
        }

        public static void LoadEnv()
        {
            if (_loadedEnv) { return; }

            string? filename = Environment.GetEnvironmentVariable("LOAD_ENV");
            if (string.IsNullOrWhiteSpace(filename)) { return; }

            using StreamReader streamReader = new(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read));

            string? line = null;
            while (!streamReader.EndOfStream)
            {
                line = streamReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) { continue; }

                string[] parts = line.Split('=', 2);
                if (parts.Length != 2) { continue; }

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }

            _loadedEnv = true;
        }



        [Required]
        public static string CLIENT_ID => Required();

        [Required]
        public static string DISCORD_TOKEN => Required();

        [Required]
        public static string OAUTH_URI => Required();

        [Required]
        public static string OAUTH_SCOPE => Required();

        [Required]
        public static string OAUTH_PERMISSIONS => Required();

        public static string GenerateOAuthUri(string? guildId = null)
        {
            string regularUri = $"{Config.OAUTH_URI}?client_id={Config.CLIENT_ID}&scope={Config.OAUTH_SCOPE}&permissions={Config.OAUTH_PERMISSIONS}";

            if (!string.IsNullOrWhiteSpace(guildId))
            {
                return $"{regularUri}&guild_id={guildId}&disable_guild_select=true";
            }

            return regularUri;

        }
    }
}
