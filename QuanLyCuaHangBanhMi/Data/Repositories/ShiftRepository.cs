using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    /// <summary>Truy cập dữ liệu nhật ký mở, đóng và duyệt ca.</summary>
    public static class ShiftRepository
    {
        public static DataTable GetAll(
            DateTime fromInclusive,
            DateTime toExclusive,
            int? branchId,
            string? status)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT ca.MaCa, ca.MaChiNhanh, cn.TenChiNhanh,
                       ca.MaNhanVien, nv.MaCode AS MaCodeNhanVien, nv.HoTen,
                       ca.GioMo, ca.GioDong, ca.TienDauCa, ca.TienCuoiCa,
                       ca.TienHeThong, ca.TienThucDem,
                       CASE
                           WHEN ca.TienThucDem IS NULL OR ca.TienHeThong IS NULL THEN NULL
                           ELSE ca.TienThucDem - ca.TienHeThong
                       END AS ChenhLech,
                       ca.TrangThai, ISNULL(ca.GhiChu, N'') AS GhiChu
                FROM dbo.NhatKy_CaLam ca
                INNER JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = ca.MaChiNhanh
                INNER JOIN dbo.NhanVien nv ON nv.MaNhanVien = ca.MaNhanVien
                WHERE ca.GioMo >= @FromInclusive
                  AND ca.GioMo < @ToExclusive
                  AND (@BranchId IS NULL OR ca.MaChiNhanh = @BranchId)
                  AND (@Status IS NULL OR ca.TrangThai = @Status)
                ORDER BY ca.GioMo DESC, ca.MaCa DESC",
                new[]
                {
                    DatabaseHelper.Param("@FromInclusive", SqlDbType.DateTime2, fromInclusive),
                    DatabaseHelper.Param("@ToExclusive", SqlDbType.DateTime2, toExclusive),
                    DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId),
                    DatabaseHelper.Param("@Status", SqlDbType.NVarChar, status, 20)
                });
        }

        public static DataTable GetBranches(int? restrictedBranchId)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT MaChiNhanh, MaCode + N' · ' + TenChiNhanh AS HienThi
                FROM dbo.ChiNhanh
                WHERE DangHoatDong = 1
                  AND (@BranchId IS NULL OR MaChiNhanh = @BranchId)
                ORDER BY TenChiNhanh, MaCode",
                new[] { DatabaseHelper.Param("@BranchId", SqlDbType.Int, restrictedBranchId) });
        }

        public static DataTable GetEmployees(int branchId)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT nv.MaNhanVien, nv.MaCode + N' · ' + nv.HoTen AS HienThi
                FROM dbo.NhanVien nv
                INNER JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = nv.MaChiNhanh
                WHERE nv.MaChiNhanh = @BranchId
                  AND nv.DangHoatDong = 1
                  AND cn.DangHoatDong = 1
                ORDER BY nv.HoTen, nv.MaCode",
                new[] { DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId) });
        }

        public static int Open(int branchId, int employeeId, decimal openingCash, string? note)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();
            using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                object? branchActive = DatabaseHelper.ExecuteScalar(connection, transaction, @"
                    SELECT DangHoatDong
                    FROM dbo.ChiNhanh WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaChiNhanh = @BranchId",
                    new[] { DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId) });
                if (branchActive is null || branchActive == DBNull.Value)
                    throw new InvalidOperationException("Chi nhánh không tồn tại.");
                if (!Convert.ToBoolean(branchActive))
                    throw new InvalidOperationException("Không thể mở ca tại chi nhánh đã ngừng hoạt động.");

                DataTable employee = DatabaseHelper.ExecuteQuery(connection, transaction, @"
                    SELECT MaChiNhanh, DangHoatDong
                    FROM dbo.NhanVien WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaNhanVien = @EmployeeId",
                    new[] { DatabaseHelper.Param("@EmployeeId", SqlDbType.Int, employeeId) });
                if (employee.Rows.Count == 0)
                    throw new InvalidOperationException("Nhân viên không tồn tại.");
                if (!Convert.ToBoolean(employee.Rows[0]["DangHoatDong"]))
                    throw new InvalidOperationException("Không thể mở ca cho nhân viên đã ngừng hoạt động.");
                if (Convert.ToInt32(employee.Rows[0]["MaChiNhanh"]) != branchId)
                    throw new InvalidOperationException("Nhân viên không thuộc chi nhánh đã chọn.");

                object? openShift = DatabaseHelper.ExecuteScalar(connection, transaction, @"
                    SELECT TOP (1) MaCa
                    FROM dbo.NhatKy_CaLam WITH (XLOCK, HOLDLOCK)
                    WHERE MaChiNhanh = @BranchId
                      AND TrangThai = N'DangMo'",
                    new[] { DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId) });
                if (openShift is not null && openShift != DBNull.Value)
                    throw new InvalidOperationException("Chi nhánh đã có một ca đang mở.");

                object? result = DatabaseHelper.ExecuteScalar(connection, transaction, @"
                    INSERT INTO dbo.NhatKy_CaLam
                        (MaChiNhanh, MaNhanVien, GioMo, TienDauCa, TrangThai, GhiChu)
                    OUTPUT INSERTED.MaCa
                    VALUES
                        (@BranchId, @EmployeeId, SYSDATETIME(), @OpeningCash, N'DangMo', @Note)",
                    new[]
                    {
                        DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId),
                        DatabaseHelper.Param("@EmployeeId", SqlDbType.Int, employeeId),
                        MoneyParam("@OpeningCash", openingCash),
                        DatabaseHelper.Param("@Note", SqlDbType.NVarChar, note, 255)
                    });

                if (result is null || result == DBNull.Value)
                    throw new InvalidOperationException("Không thể mở ca.");

                int shiftId = Convert.ToInt32(result);
                transaction.Commit();
                return shiftId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static void Close(int shiftId, decimal countedCash, string? note, int? restrictedBranchId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();
            using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                DataTable shift = DatabaseHelper.ExecuteQuery(connection, transaction, @"
                    SELECT MaChiNhanh, TienDauCa, TrangThai
                    FROM dbo.NhatKy_CaLam WITH (XLOCK, HOLDLOCK)
                    WHERE MaCa = @ShiftId",
                    new[] { DatabaseHelper.Param("@ShiftId", SqlDbType.Int, shiftId) });
                if (shift.Rows.Count == 0)
                    throw new InvalidOperationException("Ca làm không tồn tại.");

                int branchId = Convert.ToInt32(shift.Rows[0]["MaChiNhanh"]);
                if (restrictedBranchId.HasValue && branchId != restrictedBranchId.Value)
                    throw new UnauthorizedAccessException("Bạn chỉ được quản lý ca tại chi nhánh hiện tại.");
                if (!string.Equals(Convert.ToString(shift.Rows[0]["TrangThai"]), "DangMo", StringComparison.Ordinal))
                    throw new InvalidOperationException("Chỉ ca đang mở mới có thể đóng.");

                object? cashPayments = DatabaseHelper.ExecuteScalar(connection, transaction, @"
                    SELECT ISNULL(SUM(tt.SoTien), 0)
                    FROM dbo.ThanhToan_HoaDon tt WITH (HOLDLOCK)
                    INNER JOIN dbo.HoaDon hd WITH (HOLDLOCK) ON hd.MaHoaDon = tt.MaHoaDon
                    WHERE hd.MaCa = @ShiftId
                      AND tt.HinhThuc = N'TienMat'",
                    new[] { DatabaseHelper.Param("@ShiftId", SqlDbType.Int, shiftId) });
                decimal systemCash = Convert.ToDecimal(shift.Rows[0]["TienDauCa"])
                    + Convert.ToDecimal(cashPayments);

                DatabaseHelper.ExecuteNonQuery(connection, transaction, @"
                    UPDATE dbo.NhatKy_CaLam
                    SET GioDong = SYSDATETIME(),
                        TienHeThong = @SystemCash,
                        TienCuoiCa = @CountedCash,
                        TienThucDem = @CountedCash,
                        TrangThai = N'DaDong',
                        GhiChu = COALESCE(@Note, GhiChu)
                    WHERE MaCa = @ShiftId",
                    new[]
                    {
                        DatabaseHelper.Param("@ShiftId", SqlDbType.Int, shiftId),
                        MoneyParam("@SystemCash", systemCash),
                        MoneyParam("@CountedCash", countedCash),
                        DatabaseHelper.Param("@Note", SqlDbType.NVarChar, note, 255)
                    });
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static void Approve(int shiftId, int? restrictedBranchId)
        {
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();
            using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                DataTable shift = DatabaseHelper.ExecuteQuery(connection, transaction, @"
                    SELECT MaChiNhanh, TrangThai
                    FROM dbo.NhatKy_CaLam WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaCa = @ShiftId",
                    new[] { DatabaseHelper.Param("@ShiftId", SqlDbType.Int, shiftId) });
                if (shift.Rows.Count == 0)
                    throw new InvalidOperationException("Ca làm không tồn tại.");

                int branchId = Convert.ToInt32(shift.Rows[0]["MaChiNhanh"]);
                if (restrictedBranchId.HasValue && branchId != restrictedBranchId.Value)
                    throw new UnauthorizedAccessException("Bạn chỉ được quản lý ca tại chi nhánh hiện tại.");
                if (!string.Equals(Convert.ToString(shift.Rows[0]["TrangThai"]), "DaDong", StringComparison.Ordinal))
                    throw new InvalidOperationException("Chỉ ca đã đóng mới có thể duyệt.");

                DatabaseHelper.ExecuteNonQuery(connection, transaction, @"
                    UPDATE dbo.NhatKy_CaLam
                    SET TrangThai = N'DaDuyet'
                    WHERE MaCa = @ShiftId",
                    new[] { DatabaseHelper.Param("@ShiftId", SqlDbType.Int, shiftId) });
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private static SqlParameter MoneyParam(string name, decimal value) => new(name, SqlDbType.Decimal)
        {
            Precision = 18,
            Scale = 2,
            Value = value
        };
    }
}
