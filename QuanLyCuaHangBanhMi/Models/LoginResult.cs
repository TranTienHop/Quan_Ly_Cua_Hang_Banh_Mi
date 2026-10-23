using System;
using System.Collections.Generic;

namespace QuanLyCuaHangBanhMi.Models
{
    /// <summary>Kết quả xác thực đăng nhập (Business → Presentation).</summary>
    public sealed class LoginResult
    {
        public int UserId { get; init; }
        public int EmployeeId { get; init; }
        public int MaChiNhanh { get; init; }
        public int RoleId { get; init; }
        public string Username { get; init; } = "";
        public string FullName { get; init; } = "";
        public string RoleName { get; init; } = "";
        public string Position { get; init; } = "";
        public bool IsActive { get; init; }
        public IReadOnlyCollection<string> PermissionCodes { get; init; } = Array.Empty<string>();
    }
}
