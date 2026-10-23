using System;
using System.Collections.Generic;

namespace QuanLyCuaHangBanhMi.Models
{
    public static class UserSession
    {
        public static int UserID { get; set; }
        public static int EmployeeID { get; set; }
        public static int MaChiNhanh { get; set; }
        public static int RoleID { get; set; }
        public static string Username { get; set; } = string.Empty;
        public static string FullName { get; set; } = string.Empty;
        public static string RoleName { get; set; } = string.Empty;
        public static string Position { get; set; } = string.Empty;

        public static bool IsAdmin => RoleName.Equals("ADMIN", StringComparison.OrdinalIgnoreCase);

        public static bool IsManagerOrAdmin =>
            IsAdmin || RoleName.Equals("MANAGER", StringComparison.OrdinalIgnoreCase);

        private static readonly HashSet<string> Permissions =
            new(StringComparer.OrdinalIgnoreCase);

        public static IReadOnlySet<string> PermissionCodes => Permissions;

        public static bool HasPermission(string code)
        {
            if (IsAdmin)
                return true;

            return !string.IsNullOrWhiteSpace(code) && Permissions.Contains(code.Trim());
        }

        internal static void SetPermissions(IEnumerable<string>? permissionCodes)
        {
            Permissions.Clear();
            if (permissionCodes is null)
                return;

            foreach (string code in permissionCodes)
            {
                if (!string.IsNullOrWhiteSpace(code))
                    Permissions.Add(code.Trim());
            }
        }

        public static void Clear()
        {
            UserID = 0;
            EmployeeID = 0;
            MaChiNhanh = 0;
            RoleID = 0;
            Username = string.Empty;
            FullName = string.Empty;
            RoleName = string.Empty;
            Position = string.Empty;
            Permissions.Clear();
        }
    }
}
