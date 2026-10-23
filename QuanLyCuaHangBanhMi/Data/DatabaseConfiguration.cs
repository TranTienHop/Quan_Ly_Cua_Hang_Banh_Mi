using System;
using System.IO;
using System.Text.Json;

namespace QuanLyCuaHangBanhMi.Data
{
    internal static class DatabaseConfiguration
    {
        public static string LoadConnectionString()
        {
            string? environmentConnection = Environment.GetEnvironmentVariable("BANHMI_DB_CONNECTION");
            if (!string.IsNullOrWhiteSpace(environmentConnection))
                return environmentConnection;

            foreach (string directory in GetConfigurationDirectories())
            {
                string path = Path.Combine(directory, "appsettings.json");
                if (!File.Exists(path))
                    continue;

                try
                {
                    using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
                    if (document.RootElement.TryGetProperty("ConnectionStrings", out JsonElement connectionStrings) &&
                        connectionStrings.TryGetProperty("QuanLyBanhMi", out JsonElement value) &&
                        value.ValueKind == JsonValueKind.String &&
                        !string.IsNullOrWhiteSpace(value.GetString()))
                    {
                        return value.GetString()!;
                    }
                }
                catch (JsonException)
                {
                }
                catch (IOException)
                {
                }
            }

            return DatabaseHelper.DefaultConnectionString;
        }

        private static string[] GetConfigurationDirectories()
        {
            string baseDirectory = AppContext.BaseDirectory;
            string currentDirectory = Directory.GetCurrentDirectory();
            return string.Equals(baseDirectory, currentDirectory, StringComparison.OrdinalIgnoreCase)
                ? new[] { baseDirectory }
                : new[] { baseDirectory, currentDirectory };
        }
    }
}
