using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    /// <summary>Truy cập dữ liệu vai trò và ánh xạ quyền menu.</summary>
    public static class RoleRepository
    {
        public static DataTable GetAll(string keyword)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT vt.MaVaiTro, vt.MaCode, vt.TenVaiTro,
                       ISNULL(vt.MoTa, N'') AS MoTa,
                       COUNT(DISTINCT vm.MaMenu) AS SoQuyen,
                       COUNT(DISTINCT tk.MaTaiKhoan) AS SoTaiKhoan
                FROM dbo.VaiTro vt
                LEFT JOIN dbo.VaiTro_Menu vm ON vm.MaVaiTro = vt.MaVaiTro
                LEFT JOIN dbo.TaiKhoan tk ON tk.MaVaiTro = vt.MaVaiTro
                WHERE @Keyword = N''
                   OR vt.MaCode LIKE N'%' + @Keyword + N'%'
                   OR vt.TenVaiTro LIKE N'%' + @Keyword + N'%'
                   OR ISNULL(vt.MoTa, N'') LIKE N'%' + @Keyword + N'%'
                GROUP BY vt.MaVaiTro, vt.MaCode, vt.TenVaiTro, vt.MoTa
                ORDER BY vt.MaVaiTro",
                new[] { DatabaseHelper.Param("@Keyword", SqlDbType.NVarChar, keyword, 100) });
        }

        public static DataTable GetMenus()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT m.MaMenu, m.MaCode, m.TenHienThi, m.Loai, m.MaMenuCha,
                       cha.MaCode AS MaCodeMenuCha,
                       COALESCE(cha.TenHienThi, m.TenHienThi) AS TenNhom,
                       m.ThuTu
                FROM dbo.MenuChucNang m
                LEFT JOIN dbo.MenuChucNang cha ON cha.MaMenu = m.MaMenuCha
                ORDER BY COALESCE(cha.ThuTu, m.ThuTu),
                         CASE WHEN m.MaMenuCha IS NULL THEN 0 ELSE 1 END,
                         m.ThuTu, m.TenHienThi");
        }

        public static IReadOnlyList<int> GetPermissionIds(int roleId)
        {
            DataTable table = DatabaseHelper.ExecuteQuery(@"
                SELECT MaMenu
                FROM dbo.VaiTro_Menu
                WHERE MaVaiTro = @RoleId
                ORDER BY MaMenu",
                new[] { DatabaseHelper.Param("@RoleId", SqlDbType.Int, roleId) });

            return table.AsEnumerable().Select(row => row.Field<int>("MaMenu")).ToArray();
        }

        public static bool RoleExists(int roleId)
        {
            object? result = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(1) FROM dbo.VaiTro WHERE MaVaiTro = @RoleId",
                new[] { DatabaseHelper.Param("@RoleId", SqlDbType.Int, roleId) });
            return Convert.ToInt32(result) > 0;
        }

        public static bool CodeExists(string code)
        {
            object? result = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(1) FROM dbo.VaiTro WHERE MaCode = @Code",
                new[] { DatabaseHelper.Param("@Code", SqlDbType.NVarChar, code, 30) });
            return Convert.ToInt32(result) > 0;
        }

        public static int Create(
            string code,
            string name,
            string? description,
            IReadOnlyCollection<int> permissionIds)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();
            using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                ValidatePermissionIds(connection, transaction, permissionIds);
                object? result = DatabaseHelper.ExecuteScalar(connection, transaction, @"
                    INSERT INTO dbo.VaiTro (MaCode, TenVaiTro, MoTa)
                    OUTPUT INSERTED.MaVaiTro
                    VALUES (@Code, @Name, @Description)",
                    new[]
                    {
                        DatabaseHelper.Param("@Code", SqlDbType.NVarChar, code, 30),
                        DatabaseHelper.Param("@Name", SqlDbType.NVarChar, name, 100),
                        DatabaseHelper.Param("@Description", SqlDbType.NVarChar, description, 255)
                    });

                if (result is null || result == DBNull.Value)
                    throw new InvalidOperationException("Không thể tạo vai trò.");

                int roleId = Convert.ToInt32(result);
                InsertPermissions(connection, transaction, roleId, permissionIds);
                transaction.Commit();
                return roleId;
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Mã vai trò đã tồn tại.", ex);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static void Update(
            int roleId,
            string name,
            string? description,
            IReadOnlyCollection<int> permissionIds)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();
            using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                ValidatePermissionIds(connection, transaction, permissionIds);
                int affected = DatabaseHelper.ExecuteNonQuery(connection, transaction, @"
                    UPDATE dbo.VaiTro
                    SET TenVaiTro = @Name, MoTa = @Description
                    WHERE MaVaiTro = @RoleId",
                    new[]
                    {
                        DatabaseHelper.Param("@RoleId", SqlDbType.Int, roleId),
                        DatabaseHelper.Param("@Name", SqlDbType.NVarChar, name, 100),
                        DatabaseHelper.Param("@Description", SqlDbType.NVarChar, description, 255)
                    });

                if (affected == 0)
                    throw new InvalidOperationException("Vai trò không tồn tại.");

                DatabaseHelper.ExecuteNonQuery(connection, transaction,
                    "DELETE FROM dbo.VaiTro_Menu WHERE MaVaiTro = @RoleId",
                    new[] { DatabaseHelper.Param("@RoleId", SqlDbType.Int, roleId) });
                InsertPermissions(connection, transaction, roleId, permissionIds);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private static void ValidatePermissionIds(
            SqlConnection connection,
            SqlTransaction transaction,
            IReadOnlyCollection<int> permissionIds)
        {
            if (permissionIds.Count == 0)
                return;

            string[] parameterNames = permissionIds
                .Select((_, index) => "@MenuId" + index)
                .ToArray();
            SqlParameter[] parameters = permissionIds
                .Select((id, index) => DatabaseHelper.Param(parameterNames[index], SqlDbType.Int, id))
                .ToArray();
            string query = $"SELECT COUNT(1) FROM dbo.MenuChucNang WHERE MaMenu IN ({string.Join(",", parameterNames)})";
            object? result = DatabaseHelper.ExecuteScalar(connection, transaction, query, parameters);

            if (Convert.ToInt32(result) != permissionIds.Count)
                throw new InvalidOperationException("Danh sách quyền chứa menu không tồn tại.");
        }

        private static void InsertPermissions(
            SqlConnection connection,
            SqlTransaction transaction,
            int roleId,
            IEnumerable<int> permissionIds)
        {
            foreach (int permissionId in permissionIds)
            {
                DatabaseHelper.ExecuteNonQuery(connection, transaction, @"
                    INSERT INTO dbo.VaiTro_Menu (MaVaiTro, MaMenu)
                    VALUES (@RoleId, @MenuId)",
                    new[]
                    {
                        DatabaseHelper.Param("@RoleId", SqlDbType.Int, roleId),
                        DatabaseHelper.Param("@MenuId", SqlDbType.Int, permissionId)
                    });
            }
        }
    }
}
