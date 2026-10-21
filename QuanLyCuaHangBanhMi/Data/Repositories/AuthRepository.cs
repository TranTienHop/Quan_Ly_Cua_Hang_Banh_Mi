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
                       vt.MaCode AS MaVaiTro, nv.HoTen, nv.ChucVu, nv.MaChiNhanh
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

        public static void UpdateLastLogin(int maTaiKhoan)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE dbo.TaiKhoan SET LanDangNhapCuoi = SYSDATETIME() WHERE MaTaiKhoan = @MaTaiKhoan",
                new[] { new SqlParameter("@MaTaiKhoan", maTaiKhoan) });
        }

        public static bool TestConnection(out string error) => DatabaseHelper.TestConnection(out error);
    }
}
