using System;
using System.Collections.Generic;
using System.Data;
using QuanLyCuaHangBanhMi.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Security;
using QuanLyCuaHangBanhMi.Services.Abstractions;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>Triển khai IAuthService — đăng nhập / session.</summary>
    public sealed class AuthServiceImpl : IAuthService
    {
        public LoginStatus TryLogin(string username, string password, out LoginResult? result)
        {
            result = null;
            bool lockoutEnabled = AuthRepository.HasLockoutSchema();
            DataTable dt = AuthRepository.FindByUsername(username.Trim(), lockoutEnabled);
            if (dt.Rows.Count == 0)
                return LoginStatus.InvalidCredentials;

            DataRow row = dt.Rows[0];
            int accountId = Convert.ToInt32(row["MaTaiKhoan"]);
            if (lockoutEnabled && AuthRepository.IsTemporarilyLocked(accountId))
                return LoginStatus.TemporarilyLocked;

            string passwordHash = row["MatKhauBam"].ToString() ?? string.Empty;
            if (!PasswordHasher.Verify(password, passwordHash))
            {
                if (lockoutEnabled && AuthRepository.RecordFailedLogin(accountId))
                    return LoginStatus.TemporarilyLocked;
                return LoginStatus.InvalidCredentials;
            }

            bool isActive = Convert.ToBoolean(row["DangHoatDong"]) && Convert.ToBoolean(row["NhanVienDangHoatDong"]);
            int roleId = Convert.ToInt32(row["MaVaiTroId"]);
            IReadOnlyCollection<string> permissionCodes = isActive
                ? LoadPermissionCodes(roleId)
                : Array.Empty<string>();
            result = new LoginResult
            {
                UserId = Convert.ToInt32(row["MaTaiKhoan"]),
                EmployeeId = Convert.ToInt32(row["MaNhanVien"]),
                MaChiNhanh = Convert.ToInt32(row["MaChiNhanh"]),
                RoleId = roleId,
                Username = row["TenDangNhap"].ToString() ?? "",
                FullName = row["HoTen"].ToString() ?? "",
                RoleName = RoleCodes.Normalize(row["MaVaiTro"].ToString()),
                Position = row["ChucVu"].ToString() ?? "",
                IsActive = isActive,
                PermissionCodes = permissionCodes
            };

            if (!isActive)
                return LoginStatus.Locked;

            if (lockoutEnabled)
                AuthRepository.ClearFailedLoginState(accountId);
            if (PasswordHasher.NeedsRehash(passwordHash))
                AuthRepository.UpdatePasswordHash(result.UserId, PasswordHasher.Hash(password));
            AuthRepository.UpdateLastLogin(result.UserId);
            ApplySession(result);
            return LoginStatus.Success;
        }

        public void ApplySession(LoginResult user)
        {
            UserSession.UserID = user.UserId;
            UserSession.EmployeeID = user.EmployeeId;
            UserSession.MaChiNhanh = user.MaChiNhanh;
            UserSession.RoleID = user.RoleId;
            UserSession.Username = user.Username;
            UserSession.FullName = user.FullName;
            UserSession.RoleName = user.RoleName;
            UserSession.Position = user.Position;
            UserSession.SetPermissions(user.PermissionCodes);
        }

        private static IReadOnlyCollection<string> LoadPermissionCodes(int roleId)
        {
            DataTable permissions = AuthRepository.GetPermissionCodes(roleId);
            var result = new List<string>(permissions.Rows.Count);
            foreach (DataRow row in permissions.Rows)
            {
                string? code = row["MaCode"].ToString();
                if (!string.IsNullOrWhiteSpace(code))
                    result.Add(code);
            }

            return result;
        }

        public bool TestConnection(out string error) => AuthRepository.TestConnection(out error);
    }
}
