using System;
using System.Data;
using QuanLyCuaHangBanhMi.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services.Abstractions;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>Triển khai IAuthService — đăng nhập / session.</summary>
    public sealed class AuthServiceImpl : IAuthService
    {
        public LoginStatus TryLogin(string username, string password, out LoginResult? result)
        {
            result = null;
            string hash = DatabaseHelper.HashPasswordSHA256(password);
            DataTable dt = AuthRepository.FindByCredentials(username, hash);
            if (dt.Rows.Count == 0)
                return LoginStatus.InvalidCredentials;

            DataRow row = dt.Rows[0];
            bool isActive = Convert.ToBoolean(row["DangHoatDong"]) && Convert.ToBoolean(row["NhanVienDangHoatDong"]);
            result = new LoginResult
            {
                UserId = Convert.ToInt32(row["MaTaiKhoan"]),
                EmployeeId = Convert.ToInt32(row["MaNhanVien"]),
                MaChiNhanh = Convert.ToInt32(row["MaChiNhanh"]),
                Username = row["TenDangNhap"].ToString() ?? "",
                FullName = row["HoTen"].ToString() ?? "",
                RoleName = row["MaVaiTro"].ToString() ?? "",
                Position = row["ChucVu"].ToString() ?? "",
                IsActive = isActive
            };

            if (!isActive)
                return LoginStatus.Locked;

            ApplySession(result);
            AuthRepository.UpdateLastLogin(result.UserId);
            return LoginStatus.Success;
        }

        public void ApplySession(LoginResult user)
        {
            UserSession.UserID = user.UserId;
            UserSession.EmployeeID = user.EmployeeId;
            UserSession.MaChiNhanh = user.MaChiNhanh;
            UserSession.Username = user.Username;
            UserSession.FullName = user.FullName;
            UserSession.RoleName = user.RoleName;
            UserSession.Position = user.Position;
        }

        public bool TestConnection(out string error) => AuthRepository.TestConnection(out error);
    }
}
