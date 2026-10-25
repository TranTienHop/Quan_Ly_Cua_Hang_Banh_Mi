using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    /// <summary>Data access cho lịch phân ca, tách biệt nhật ký mở/đóng ca.</summary>
    public static class ShiftScheduleRepository
    {
        public static DataTable GetBranches(int? branchId)
        {
            EnsureMigrationApplied();
            return DatabaseHelper.ExecuteQuery(@"
                SELECT MaChiNhanh, MaCode + N' · ' + TenChiNhanh AS HienThi
                FROM dbo.ChiNhanh
                WHERE DangHoatDong = 1
                  AND (@BranchId IS NULL OR MaChiNhanh = @BranchId)
                ORDER BY TenChiNhanh, MaCode",
                new[] { DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId) });
        }

        public static DataTable GetEmployees(int branchId)
        {
            EnsureMigrationApplied();
            return DatabaseHelper.ExecuteQuery(@"
                SELECT MaNhanVien, MaCode + N' · ' + HoTen AS HienThi
                FROM dbo.NhanVien
                WHERE MaChiNhanh = @BranchId AND DangHoatDong = 1
                ORDER BY HoTen, MaCode",
                new[] { DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId) });
        }

        public static DataTable GetShiftTypes()
        {
            EnsureMigrationApplied();
            return DatabaseHelper.ExecuteQuery(@"
                SELECT MaCaLam, MaCode, TenCa, GioBatDau, GioKetThuc, DangHoatDong,
                       MaCode + N' · ' + TenCa +
                           CASE WHEN DangHoatDong = 1 THEN N'' ELSE N' (Ngừng)' END AS HienThi
                FROM dbo.DanhMuc_CaLam
                ORDER BY GioBatDau, TenCa");
        }

        public static int CreateShiftType(string code, string name, TimeSpan start, TimeSpan end)
        {
            EnsureMigrationApplied();
            try
            {
                object? result = DatabaseHelper.ExecuteScalar(@"
                    INSERT INTO dbo.DanhMuc_CaLam (MaCode, TenCa, GioBatDau, GioKetThuc)
                    OUTPUT INSERTED.MaCaLam
                    VALUES (@Code, @Name, @Start, @End)",
                    new[]
                    {
                        DatabaseHelper.Param("@Code", SqlDbType.NVarChar, code, 30),
                        DatabaseHelper.Param("@Name", SqlDbType.NVarChar, name, 100),
                        DatabaseHelper.Param("@Start", SqlDbType.Time, start),
                        DatabaseHelper.Param("@End", SqlDbType.Time, end)
                    });
                if (result is null || result == DBNull.Value)
                    throw new InvalidOperationException("Không thể tạo danh mục ca làm.");
                return Convert.ToInt32(result);
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                throw new InvalidOperationException("Mã ca làm đã tồn tại.", ex);
            }
        }

        public static void UpdateShiftType(int shiftId, string code, string name, TimeSpan start, TimeSpan end)
        {
            EnsureMigrationApplied();
            try
            {
                int affected = DatabaseHelper.ExecuteNonQuery(@"
                    UPDATE dbo.DanhMuc_CaLam
                    SET MaCode = @Code, TenCa = @Name,
                        GioBatDau = @Start, GioKetThuc = @End
                    WHERE MaCaLam = @ShiftId",
                    new[]
                    {
                        DatabaseHelper.Param("@ShiftId", SqlDbType.Int, shiftId),
                        DatabaseHelper.Param("@Code", SqlDbType.NVarChar, code, 30),
                        DatabaseHelper.Param("@Name", SqlDbType.NVarChar, name, 100),
                        DatabaseHelper.Param("@Start", SqlDbType.Time, start),
                        DatabaseHelper.Param("@End", SqlDbType.Time, end)
                    });
                if (affected == 0)
                    throw new InvalidOperationException("Ca làm không tồn tại.");
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                throw new InvalidOperationException("Mã ca làm đã được một ca khác sử dụng.", ex);
            }
        }

        public static bool ToggleShiftType(int shiftId)
        {
            EnsureMigrationApplied();
            object? result = DatabaseHelper.ExecuteScalar(@"
                UPDATE dbo.DanhMuc_CaLam
                SET DangHoatDong = CASE WHEN DangHoatDong = 1 THEN 0 ELSE 1 END
                OUTPUT INSERTED.DangHoatDong
                WHERE MaCaLam = @ShiftId",
                new[] { DatabaseHelper.Param("@ShiftId", SqlDbType.Int, shiftId) });
            return result is not null && result != DBNull.Value && Convert.ToBoolean(result);
        }

        public static DataTable GetSchedule(DateTime fromDate, DateTime toDate, int? branchId)
        {
            EnsureMigrationApplied();
            return DatabaseHelper.ExecuteQuery(@"
                SELECT lp.MaLich, lp.NgayLam, cn.TenChiNhanh,
                       nv.MaCode AS MaNhanVien, nv.HoTen,
                       ca.MaCode AS MaCa, ca.TenCa, ca.GioBatDau, ca.GioKetThuc,
                       lp.TrangThai, ISNULL(lp.GhiChu, N'') AS GhiChu
                FROM dbo.LichPhanCa lp
                INNER JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = lp.MaChiNhanh
                INNER JOIN dbo.NhanVien nv ON nv.MaNhanVien = lp.MaNhanVien
                INNER JOIN dbo.DanhMuc_CaLam ca ON ca.MaCaLam = lp.MaCaLam
                WHERE lp.NgayLam >= @FromDate AND lp.NgayLam <= @ToDate
                  AND (@BranchId IS NULL OR lp.MaChiNhanh = @BranchId)
                ORDER BY lp.NgayLam, ca.GioBatDau, nv.HoTen",
                new[]
                {
                    DatabaseHelper.Param("@FromDate", SqlDbType.Date, fromDate.Date),
                    DatabaseHelper.Param("@ToDate", SqlDbType.Date, toDate.Date),
                    DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId)
                });
        }

        public static int Assign(int employeeId, int shiftId, int branchId, DateTime workDate, string? note)
        {
            EnsureMigrationApplied();
            using SqlConnection connection = DatabaseHelper.GetConnection();
            connection.Open();
            using SqlTransaction transaction = connection.BeginTransaction(IsolationLevel.Serializable);

            try
            {
                DataTable employee = DatabaseHelper.ExecuteQuery(connection, transaction, @"
                    SELECT nv.MaChiNhanh
                    FROM dbo.NhanVien nv WITH (UPDLOCK, HOLDLOCK)
                    INNER JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = nv.MaChiNhanh
                    WHERE nv.MaNhanVien = @EmployeeId
                      AND nv.DangHoatDong = 1 AND cn.DangHoatDong = 1
                      AND nv.MaChiNhanh = @BranchId",
                    new[]
                    {
                        DatabaseHelper.Param("@EmployeeId", SqlDbType.Int, employeeId),
                        DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId)
                    });
                if (employee.Rows.Count == 0)
                    throw new InvalidOperationException("Nhân viên không hoạt động hoặc không thuộc chi nhánh đã chọn.");

                object? shiftActive = DatabaseHelper.ExecuteScalar(connection, transaction, @"
                    SELECT DangHoatDong
                    FROM dbo.DanhMuc_CaLam WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaCaLam = @ShiftId",
                    new[] { DatabaseHelper.Param("@ShiftId", SqlDbType.Int, shiftId) });
                if (shiftActive is null || shiftActive == DBNull.Value || !Convert.ToBoolean(shiftActive))
                    throw new InvalidOperationException("Ca làm không tồn tại hoặc đã ngừng sử dụng.");

                object? result = DatabaseHelper.ExecuteScalar(connection, transaction, @"
                    INSERT INTO dbo.LichPhanCa (MaCaLam, MaNhanVien, MaChiNhanh, NgayLam, GhiChu)
                    OUTPUT INSERTED.MaLich
                    VALUES (@ShiftId, @EmployeeId, @BranchId, @WorkDate, @Note)",
                    new[]
                    {
                        DatabaseHelper.Param("@ShiftId", SqlDbType.Int, shiftId),
                        DatabaseHelper.Param("@EmployeeId", SqlDbType.Int, employeeId),
                        DatabaseHelper.Param("@BranchId", SqlDbType.Int, branchId),
                        DatabaseHelper.Param("@WorkDate", SqlDbType.Date, workDate.Date),
                        DatabaseHelper.Param("@Note", SqlDbType.NVarChar, note, 255)
                    });

                if (result is null || result == DBNull.Value)
                    throw new InvalidOperationException("Không thể tạo lịch phân ca.");

                int assignmentId = Convert.ToInt32(result);
                transaction.Commit();
                return assignmentId;
            }
            catch (SqlException ex) when (ex.Number is 2601 or 2627)
            {
                transaction.Rollback();
                throw new InvalidOperationException("Nhân viên đã được xếp ca này trong ngày đã chọn.", ex);
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public static void Cancel(int assignmentId, int? restrictedBranchId)
        {
            EnsureMigrationApplied();
            int affected = DatabaseHelper.ExecuteNonQuery(@"
                UPDATE dbo.LichPhanCa
                SET TrangThai = N'Huy'
                WHERE MaLich = @AssignmentId
                  AND TrangThai = N'DaXep'
                  AND (@BranchId IS NULL OR MaChiNhanh = @BranchId)",
                new[]
                {
                    DatabaseHelper.Param("@AssignmentId", SqlDbType.Int, assignmentId),
                    DatabaseHelper.Param("@BranchId", SqlDbType.Int, restrictedBranchId)
                });
            if (affected == 0)
                throw new InvalidOperationException("Lịch không tồn tại, đã xử lý hoặc nằm ngoài chi nhánh được phép.");
        }

        private static void EnsureMigrationApplied()
        {
            object? result = DatabaseHelper.ExecuteScalar(@"
                SELECT CASE
                    WHEN OBJECT_ID(N'dbo.DanhMuc_CaLam', N'U') IS NOT NULL
                     AND OBJECT_ID(N'dbo.LichPhanCa', N'U') IS NOT NULL
                    THEN 1 ELSE 0 END");
            if (!Convert.ToBoolean(result))
            {
                throw new InvalidOperationException(
                    "Chưa có schema lịch phân ca. Hãy xem Database/03_Migration_TichHop_Login.sql; " +
                    "không tự chạy migration khi chưa được phê duyệt.");
            }
        }
    }
}
