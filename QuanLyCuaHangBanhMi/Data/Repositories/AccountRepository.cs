using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    /// <summary>Truy cập dữ liệu quản trị tài khoản trên schema QuanLyBanhMi.</summary>
    public static class AccountRepository
    {
        public static DataTable GetAll(string keyword, bool? isActive)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT tk.MaTaiKhoan, tk.TenDangNhap, nv.HoTen,
                       nv.MaCode AS MaCodeNhanVien, vt.TenVaiTro,
                       vt.MaCode AS MaCodeVaiTro, cn.TenChiNhanh,
                       tk.DangHoatDong, tk.LanDangNhapCuoi
                FROM dbo.TaiKhoan tk
                INNER JOIN dbo.NhanVien nv ON nv.MaNhanVien = tk.MaNhanVien
                INNER JOIN dbo.VaiTro vt ON vt.MaVaiTro = tk.MaVaiTro
                INNER JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = nv.MaChiNhanh
                WHERE (@Keyword = N''
                       OR tk.TenDangNhap LIKE N'%' + @Keyword + N'%'
                       OR nv.HoTen LIKE N'%' + @Keyword + N'%'
                       OR nv.MaCode LIKE N'%' + @Keyword + N'%'
                       OR vt.TenVaiTro LIKE N'%' + @Keyword + N'%'
                       OR vt.MaCode LIKE N'%' + @Keyword + N'%'
                       OR cn.TenChiNhanh LIKE N'%' + @Keyword + N'%')
                  AND (@IsActive IS NULL OR tk.DangHoatDong = @IsActive)
                ORDER BY tk.TenDangNhap",
                new[]
                {
                    DatabaseHelper.Param("@Keyword", SqlDbType.NVarChar, keyword, 100),
                    DatabaseHelper.Param("@IsActive", SqlDbType.Bit, isActive)
                });
        }

        public static DataTable GetEmployeesWithoutAccount()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT nv.MaNhanVien,
                       nv.MaCode + N' - ' + nv.HoTen + N' (' + cn.TenChiNhanh + N')' AS HienThi
                FROM dbo.NhanVien nv
                INNER JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = nv.MaChiNhanh
                LEFT JOIN dbo.TaiKhoan tk ON tk.MaNhanVien = nv.MaNhanVien
                WHERE tk.MaTaiKhoan IS NULL
                  AND nv.DangHoatDong = 1
                  AND cn.DangHoatDong = 1
                ORDER BY nv.HoTen, nv.MaCode");
        }

        public static DataTable GetRoles()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT MaVaiTro, MaCode + N' - ' + TenVaiTro AS HienThi
                FROM dbo.VaiTro
                ORDER BY MaVaiTro");
        }

        public static DataTable GetActiveBranches()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT MaChiNhanh, MaCode + N' - ' + TenChiNhanh AS HienThi
                FROM dbo.ChiNhanh
                WHERE DangHoatDong = 1
                ORDER BY TenChiNhanh, MaCode");
        }

        public static DataTable GetEmployeeForAccountCreation(int employeeId)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT nv.MaNhanVien, nv.MaChiNhanh, nv.DangHoatDong AS NhanVienDangHoatDong,
                       cn.DangHoatDong AS ChiNhanhDangHoatDong,
                       CASE WHEN tk.MaTaiKhoan IS NULL THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS DaCoTaiKhoan
                FROM dbo.NhanVien nv
                INNER JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = nv.MaChiNhanh
                LEFT JOIN dbo.TaiKhoan tk ON tk.MaNhanVien = nv.MaNhanVien
                WHERE nv.MaNhanVien = @EmployeeId",
                new[] { DatabaseHelper.Param("@EmployeeId", SqlDbType.Int, employeeId) });
        }

        public static bool RoleExists(int roleId)
        {
            object? result = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(1) FROM dbo.VaiTro WHERE MaVaiTro = @RoleId",
                new[] { DatabaseHelper.Param("@RoleId", SqlDbType.Int, roleId) });
            return Convert.ToInt32(result) > 0;
        }

        public static bool ActiveBranchExists(int branchId)
        {
            object? result = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(1) FROM dbo.ChiNhanh WHERE MaChiNhanh = @BranchId AND DangHoatDong = 1",
                new[] { DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId) });
            return Convert.ToInt32(result) > 0;
        }

        public static bool UsernameExists(string username)
        {
            object? result = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(1) FROM dbo.TaiKhoan WHERE TenDangNhap = @Username",
                new[] { DatabaseHelper.Param("@Username", SqlDbType.NVarChar, username, 50) });
            return Convert.ToInt32(result) > 0;
        }

        public static int Insert(int employeeId, int roleId, string username, string passwordHash, bool isActive)
        {
            try
            {
                object? result = DatabaseHelper.ExecuteScalar(@"
                    INSERT INTO dbo.TaiKhoan
                        (MaNhanVien, MaVaiTro, TenDangNhap, MatKhauBam, DangHoatDong)
                    OUTPUT INSERTED.MaTaiKhoan
                    VALUES
                        (@EmployeeId, @RoleId, @Username, @PasswordHash, @IsActive)",
                    new[]
                    {
                        DatabaseHelper.Param("@EmployeeId", SqlDbType.Int, employeeId),
                        DatabaseHelper.Param("@RoleId", SqlDbType.Int, roleId),
                        DatabaseHelper.Param("@Username", SqlDbType.NVarChar, username, 50),
                        DatabaseHelper.Param("@PasswordHash", SqlDbType.NVarChar, passwordHash, 255),
                        DatabaseHelper.Param("@IsActive", SqlDbType.Bit, isActive)
                    });

                if (result is null || result == DBNull.Value)
                    throw new InvalidOperationException("Không thể tạo tài khoản.");

                return Convert.ToInt32(result);
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                throw new InvalidOperationException(
                    "Tên đăng nhập hoặc nhân viên đã được gắn với một tài khoản khác.", ex);
            }
        }

        public static int ResetPassword(int accountId, string passwordHash)
        {
            return DatabaseHelper.ExecuteNonQuery(@"
                UPDATE dbo.TaiKhoan
                SET MatKhauBam = @PasswordHash
                WHERE MaTaiKhoan = @AccountId;

                IF COL_LENGTH(N'dbo.TaiKhoan', N'SoLanDangNhapThatBai') IS NOT NULL
                   AND COL_LENGTH(N'dbo.TaiKhoan', N'KhoaDen') IS NOT NULL
                BEGIN
                    EXEC sys.sp_executesql
                        N'UPDATE dbo.TaiKhoan
                          SET SoLanDangNhapThatBai = 0, KhoaDen = NULL
                          WHERE MaTaiKhoan = @Id',
                        N'@Id INT',
                        @Id = @AccountId;
                END",
                new[]
                {
                    DatabaseHelper.Param("@AccountId", SqlDbType.Int, accountId),
                    DatabaseHelper.Param("@PasswordHash", SqlDbType.NVarChar, passwordHash, 255)
                });
        }

        public static bool? ToggleStatus(int accountId)
        {
            object? result = DatabaseHelper.ExecuteScalar(@"
                UPDATE dbo.TaiKhoan
                SET DangHoatDong = CASE WHEN DangHoatDong = 1 THEN 0 ELSE 1 END
                OUTPUT INSERTED.DangHoatDong
                WHERE MaTaiKhoan = @AccountId",
                new[] { DatabaseHelper.Param("@AccountId", SqlDbType.Int, accountId) });

            return result is null || result == DBNull.Value ? null : Convert.ToBoolean(result);
        }
    }
}
