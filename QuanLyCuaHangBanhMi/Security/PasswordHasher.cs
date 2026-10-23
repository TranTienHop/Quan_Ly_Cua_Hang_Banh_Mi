using System;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Security
{
    /// <summary>
    /// BCrypt cho mật khẩu mới và xác thực SHA-256 cho dữ liệu QuanLyBanhMi cũ.
    /// Hash SHA hợp lệ sẽ được nâng lên BCrypt sau lần đăng nhập thành công.
    /// </summary>
    internal static class PasswordHasher
    {
        public static string Hash(string password)
        {
            ArgumentNullException.ThrowIfNull(password);
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool Verify(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(storedHash))
                return false;

            if (IsBcrypt(storedHash))
            {
                try
                {
                    return BCrypt.Net.BCrypt.Verify(password, storedHash);
                }
                catch (BCrypt.Net.SaltParseException)
                {
                    return false;
                }
            }

            string legacyHash = DatabaseHelper.HashPasswordSHA256(password);
            return string.Equals(legacyHash, storedHash.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        public static bool NeedsRehash(string storedHash) => !IsBcrypt(storedHash);

        private static bool IsBcrypt(string value)
        {
            return value.StartsWith("$2a$", StringComparison.Ordinal) ||
                   value.StartsWith("$2b$", StringComparison.Ordinal) ||
                   value.StartsWith("$2y$", StringComparison.Ordinal);
        }
    }
}
