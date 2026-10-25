using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Diagnostics
{
    internal static partial class ApplicationLogger
    {
        private static readonly object SyncRoot = new();

        public static void Error(string context, Exception exception)
        {
            ArgumentNullException.ThrowIfNull(exception);
            Write("ERROR", context, exception);
        }

        public static void Warning(string context, Exception exception)
        {
            ArgumentNullException.ThrowIfNull(exception);
            Write("WARN", context, exception);
        }

        private static void Write(string level, string context, Exception exception)
        {
            try
            {
                string directory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "QuanLyCuaHangBanhMi", "Logs");
                Directory.CreateDirectory(directory);

                string path = Path.Combine(directory, $"application-{DateTime.Now:yyyyMMdd}.log");
                string session = UserSession.UserID > 0
                    ? $"UserId={UserSession.UserID}; BranchId={UserSession.MaChiNhanh}; Role={UserSession.RoleName}"
                    : "UserId=anonymous";
                string entry =
                    $"{DateTimeOffset.Now:O} [{level}] {Sanitize(context)} | {session}{Environment.NewLine}" +
                    FormatException(exception) + Environment.NewLine;

                lock (SyncRoot)
                    File.AppendAllText(path, entry, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            }
            catch
            {
                // Logging must never replace the original application/database error.
            }
        }

        private static string FormatException(Exception exception)
        {
            var builder = new StringBuilder();
            int depth = 0;
            for (Exception? current = exception; current is not null && depth < 5; current = current.InnerException)
            {
                if (depth > 0) builder.Append("Caused by: ");
                builder.Append(current.GetType().FullName)
                    .Append(": ")
                    .AppendLine(Sanitize(current.Message));
                if (!string.IsNullOrWhiteSpace(current.StackTrace))
                    builder.AppendLine(Sanitize(current.StackTrace));
                depth++;
            }
            return builder.ToString().TrimEnd();
        }

        private static string Sanitize(string? value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return SecretPattern().Replace(value, "$1=<redacted>");
        }

        [GeneratedRegex("(?i)(password|pwd|user id|uid)\\s*=\\s*[^;\\r\\n]*")]
        private static partial Regex SecretPattern();
    }
}
