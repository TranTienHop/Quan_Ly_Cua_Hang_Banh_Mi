using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    internal enum BranchToggleOutcome
    {
        Success,
        NotFound,
        CurrentBranch,
        OpenShift
    }

    internal readonly record struct BranchToggleResult(BranchToggleOutcome Outcome, bool NewStatus);

    /// <summary>Truy cập dữ liệu chi nhánh trên schema QuanLyBanhMi.</summary>
    public static class BranchRepository
    {
        public static DataTable GetAll(string keyword, bool? active, byte? type)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT cn.MaChiNhanh, cn.MaCode, cn.TenChiNhanh, cn.LoaiChiNhanh,
                       CASE cn.LoaiChiNhanh
                           WHEN 1 THEN N'Cửa hàng'
                           WHEN 2 THEN N'Lò/Kho tổng'
                       END AS TenLoai,
                       ISNULL(cn.DiaChi, N'') AS DiaChi,
                       ISNULL(cn.SoDienThoai, N'') AS SoDienThoai,
                       cn.DangHoatDong,
                       (SELECT COUNT(1)
                        FROM dbo.NhanVien nv
                        WHERE nv.MaChiNhanh = cn.MaChiNhanh) AS SoNhanVien,
                       (SELECT COUNT(1)
                        FROM dbo.NhatKy_CaLam ca
                        WHERE ca.MaChiNhanh = cn.MaChiNhanh
                          AND ca.TrangThai = N'DangMo') AS SoCaDangMo
                FROM dbo.ChiNhanh cn
                WHERE (@Keyword = N''
                       OR cn.MaCode LIKE N'%' + @Keyword + N'%'
                       OR cn.TenChiNhanh LIKE N'%' + @Keyword + N'%'
                       OR ISNULL(cn.DiaChi, N'') LIKE N'%' + @Keyword + N'%'
                       OR ISNULL(cn.SoDienThoai, N'') LIKE N'%' + @Keyword + N'%')
                  AND (@Active IS NULL OR cn.DangHoatDong = @Active)
                  AND (@Type IS NULL OR cn.LoaiChiNhanh = @Type)
                ORDER BY cn.MaChiNhanh",
                new[]
                {
                    DatabaseHelper.Param("@Keyword", SqlDbType.NVarChar, keyword, 100),
                    DatabaseHelper.Param("@Active", SqlDbType.Bit, active),
                    DatabaseHelper.Param("@Type", SqlDbType.TinyInt, type)
                });
        }

        public static bool CodeExists(string code)
        {
            object? result = DatabaseHelper.ExecuteScalar(
                "SELECT COUNT(1) FROM dbo.ChiNhanh WHERE MaCode = @Code",
                new[] { DatabaseHelper.Param("@Code", SqlDbType.NVarChar, code, 20) });
            return Convert.ToInt32(result) > 0;
        }

        public static int Create(
            string code,
            string name,
            byte type,
            string? address,
            string? phone,
            bool isActive)
        {
            try
            {
                object? result = DatabaseHelper.ExecuteScalar(@"
                    INSERT INTO dbo.ChiNhanh
                        (MaCode, TenChiNhanh, LoaiChiNhanh, DiaChi, SoDienThoai, DangHoatDong)
                    OUTPUT INSERTED.MaChiNhanh
                    VALUES
                        (@Code, @Name, @Type, @Address, @Phone, @IsActive)",
                    new[]
                    {
                        DatabaseHelper.Param("@Code", SqlDbType.NVarChar, code, 20),
                        DatabaseHelper.Param("@Name", SqlDbType.NVarChar, name, 200),
                        DatabaseHelper.Param("@Type", SqlDbType.TinyInt, type),
                        DatabaseHelper.Param("@Address", SqlDbType.NVarChar, address, 255),
                        DatabaseHelper.Param("@Phone", SqlDbType.NVarChar, phone, 20),
                        DatabaseHelper.Param("@IsActive", SqlDbType.Bit, isActive)
                    });

                if (result is null || result == DBNull.Value)
                    throw new InvalidOperationException("Không thể tạo chi nhánh.");

                return Convert.ToInt32(result);
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                throw new InvalidOperationException("Mã chi nhánh đã tồn tại.", ex);
            }
        }

        public static int Update(int branchId, string name, byte type, string? address, string? phone)
        {
            return DatabaseHelper.ExecuteNonQuery(@"
                UPDATE dbo.ChiNhanh
                SET TenChiNhanh = @Name,
                    LoaiChiNhanh = @Type,
                    DiaChi = @Address,
                    SoDienThoai = @Phone
                WHERE MaChiNhanh = @BranchId",
                new[]
                {
                    DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId),
                    DatabaseHelper.Param("@Name", SqlDbType.NVarChar, name, 200),
                    DatabaseHelper.Param("@Type", SqlDbType.TinyInt, type),
                    DatabaseHelper.Param("@Address", SqlDbType.NVarChar, address, 255),
                    DatabaseHelper.Param("@Phone", SqlDbType.NVarChar, phone, 20)
                });
        }

        internal static BranchToggleResult ToggleStatus(int branchId, int currentBranchId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();
            using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                DataTable branch = DatabaseHelper.ExecuteQuery(connection, transaction, @"
                    SELECT DangHoatDong
                    FROM dbo.ChiNhanh WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaChiNhanh = @BranchId",
                    new[] { DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId) });

                if (branch.Rows.Count == 0)
                {
                    transaction.Commit();
                    return new BranchToggleResult(BranchToggleOutcome.NotFound, false);
                }

                bool isActive = Convert.ToBoolean(branch.Rows[0]["DangHoatDong"]);
                if (isActive && branchId == currentBranchId)
                {
                    transaction.Commit();
                    return new BranchToggleResult(BranchToggleOutcome.CurrentBranch, true);
                }

                if (isActive)
                {
                    object? openShiftCount = DatabaseHelper.ExecuteScalar(connection, transaction, @"
                        SELECT COUNT(1)
                        FROM dbo.NhatKy_CaLam WITH (UPDLOCK, HOLDLOCK)
                        WHERE MaChiNhanh = @BranchId
                          AND TrangThai = N'DangMo'",
                        new[] { DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId) });

                    if (Convert.ToInt32(openShiftCount) > 0)
                    {
                        transaction.Commit();
                        return new BranchToggleResult(BranchToggleOutcome.OpenShift, true);
                    }
                }

                bool newStatus = !isActive;
                DatabaseHelper.ExecuteNonQuery(connection, transaction, @"
                    UPDATE dbo.ChiNhanh
                    SET DangHoatDong = @NewStatus
                    WHERE MaChiNhanh = @BranchId",
                    new[]
                    {
                        DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId),
                        DatabaseHelper.Param("@NewStatus", SqlDbType.Bit, newStatus)
                    });
                transaction.Commit();
                return new BranchToggleResult(BranchToggleOutcome.Success, newStatus);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
