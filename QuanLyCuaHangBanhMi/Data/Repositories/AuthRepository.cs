using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    /// <summary>Lớp Data — truy vấn tài khoản / đăng nhập.</summary>
    public static class AuthRepository
    {
        public static bool HasLockoutSchema()
        {
            object? result = DatabaseHelper.ExecuteScalar(@"
                SELECT CASE
                    WHEN COL_LENGTH(N'dbo.TaiKhoan', N'SoLanDangNhapThatBai') IS NOT NULL
                     AND COL_LENGTH(N'dbo.TaiKhoan', N'KhoaDen') IS NOT NULL
                    THEN 1 ELSE 0 END");
            return Convert.ToBoolean(result);
        }

        public static DataTable FindByUsername(string username, bool includeLockoutState = false)
        {
            string lockoutColumns = includeLockoutState
                ? "tk.SoLanDangNhapThatBai, tk.KhoaDen,"
                : string.Empty;
            string query = $@"
                SELECT tk.MaTaiKhoan, tk.MaNhanVien, tk.TenDangNhap, tk.MatKhauBam, tk.DangHoatDong,
                       {lockoutColumns}
                       nv.DangHoatDong AS NhanVienDangHoatDong,
                       vt.MaVaiTro AS MaVaiTroId, vt.MaCode AS MaVaiTro,
                       nv.HoTen, nv.ChucVu, nv.MaChiNhanh
                FROM dbo.TaiKhoan tk
                JOIN dbo.VaiTro vt ON tk.MaVaiTro = vt.MaVaiTro
                JOIN dbo.NhanVien nv ON tk.MaNhanVien = nv.MaNhanVien
                WHERE tk.TenDangNhap = @TenDangNhap";

            return DatabaseHelper.ExecuteQuery(query, new[]
            {
                new SqlParameter("@TenDangNhap", SqlDbType.NVarChar, 50) { Value = username }
            });
        }

        public static bool IsTemporarilyLocked(int accountId)
        {
            object? result = DatabaseHelper.ExecuteScalar(@"
                SELECT CASE WHEN KhoaDen > SYSDATETIME() THEN 1 ELSE 0 END
                FROM dbo.TaiKhoan
                WHERE MaTaiKhoan = @MaTaiKhoan",
                new[] { DatabaseHelper.Param("@MaTaiKhoan", SqlDbType.Int, accountId) });
            return result is not null && result != DBNull.Value && Convert.ToBoolean(result);
        }

        public static bool RecordFailedLogin(int accountId)
        {
            object? lockoutEnd = DatabaseHelper.ExecuteScalar(@"
                UPDATE dbo.TaiKhoan
                SET SoLanDangNhapThatBai =
                        CASE WHEN KhoaDen IS NOT NULL AND KhoaDen <= SYSDATETIME()
                             THEN 1
                             WHEN SoLanDangNhapThatBai >= 4 THEN 5
                             ELSE SoLanDangNhapThatBai + 1 END,
                    KhoaDen =
                        CASE WHEN KhoaDen > SYSDATETIME() THEN KhoaDen
                             WHEN KhoaDen IS NOT NULL THEN NULL
                             WHEN SoLanDangNhapThatBai >= 4
                                  THEN DATEADD(MINUTE, 15, SYSDATETIME())
                             ELSE NULL END
                OUTPUT CASE
                    WHEN INSERTED.KhoaDen > SYSDATETIME() THEN CAST(1 AS BIT)
                    ELSE CAST(0 AS BIT) END
                WHERE MaTaiKhoan = @MaTaiKhoan",
                new[] { DatabaseHelper.Param("@MaTaiKhoan", SqlDbType.Int, accountId) });

            return lockoutEnd is not null && lockoutEnd != DBNull.Value &&
                   Convert.ToBoolean(lockoutEnd);
        }

        public static void ClearFailedLoginState(int accountId)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE dbo.TaiKhoan
                SET SoLanDangNhapThatBai = 0, KhoaDen = NULL
                WHERE MaTaiKhoan = @MaTaiKhoan",
                new[] { DatabaseHelper.Param("@MaTaiKhoan", SqlDbType.Int, accountId) });
        }

        public static void UpdatePasswordHash(int accountId, string passwordHash)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE dbo.TaiKhoan
                SET MatKhauBam = @MatKhauBam
                WHERE MaTaiKhoan = @MaTaiKhoan",
                new[]
                {
                    DatabaseHelper.Param("@MaTaiKhoan", SqlDbType.Int, accountId),
                    DatabaseHelper.Param("@MatKhauBam", SqlDbType.NVarChar, passwordHash, 255)
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
