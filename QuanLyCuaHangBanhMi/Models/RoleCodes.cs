using System;

namespace QuanLyCuaHangBanhMi.Models
{
    /// <summary>Roles seeded in Database/02_DuLieuMau.sql.</summary>
    public static class RoleCodes
    {
        public const string Admin = "ADMIN";
        public const string Manager = "MANAGER";
        public const string Staff = "STAFF";

        public static bool IsStandard(string? roleCode)
        {
            return roleCode is not null &&
                   (roleCode.Equals(Admin, StringComparison.OrdinalIgnoreCase) ||
                    roleCode.Equals(Manager, StringComparison.OrdinalIgnoreCase) ||
                    roleCode.Equals(Staff, StringComparison.OrdinalIgnoreCase));
        }

        public static string Normalize(string? roleCode)
        {
            return string.IsNullOrWhiteSpace(roleCode)
                ? string.Empty
                : roleCode.Trim().ToUpperInvariant();
        }
    }
}
