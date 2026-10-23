using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    /// <summary>Lớp Data — truy vấn tài khoản / đăng nhập.</summary>
    public static class AuthRepository
    {
        public static DataTable FindByCredentials(string username, string passwordHash)
        {
            const string query = @"
                SELECT tk.MaTaiKhoan, tk.MaNhanVien, tk.TenDangNhap, tk.DangHoatDong,
                       nv.DangHoatDong AS NhanVienDangHoatDong,
                       vt.MaVaiTro AS MaVaiTroId, vt.MaCode AS MaVaiTro,
                       nv.HoTen, nv.ChucVu, nv.MaChiNhanh
                FROM dbo.TaiKhoan tk
                JOIN dbo.VaiTro vt ON tk.MaVaiTro = vt.MaVaiTro
                JOIN dbo.NhanVien nv ON tk.MaNhanVien = nv.MaNhanVien
                WHERE tk.TenDangNhap = @TenDangNhap AND tk.MatKhauBam = @MatKhauBam";

            return DatabaseHelper.ExecuteQuery(query, new[]
            {
                new SqlParameter("@TenDangNhap", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@MatKhauBam", SqlDbType.NVarChar, 255) { Value = passwordHash }
            });
        }

        public static DataTable GetPermissionCodes(int roleId)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT DISTINCT m.MaCode
                FROM dbo.VaiTro_Menu vm
                INNER JOIN dbo.MenuChucNang m ON m.MaMenu = vm.MaMenu
                WHERE vm.MaVaiTro = @RoleId
                ORDER BY m.MaCode",
                new[] { DatabaseHelper.Param("@RoleId", SqlDbType.Int, roleId) });
        }

        public static void UpdateLastLogin(int maTaiKhoan)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE dbo.TaiKhoan SET LanDangNhapCuoi = SYSDATETIME() WHERE MaTaiKhoan = @MaTaiKhoan",
                new[] { new SqlParameter("@MaTaiKhoan", maTaiKhoan) });
        }

        public static bool TestConnection(out string error) => DatabaseHelper.TestConnection(out error);
    }
}
