using System;
using System.Data;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    public static class CustomerRepository
    {
        public static DataTable GetMembershipTiers()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT MaHang, TenHang + N' (−' + CAST(PhanTramGiam AS NVARCHAR(10)) + N'%)' AS TenHienThi
                FROM dbo.CauHinh_HangThanhVien
                ORDER BY DiemToiThieu");
        }

        public static DataTable Search(string keyword)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT kh.MaKhachHang, kh.HoTen AS [Họ tên], kh.SoDienThoai AS [SĐT],
                       h.TenHang AS [Hạng], kh.DiemHienTai AS [Điểm],
                       ISNULL(kh.Email, N'') AS [Email], ISNULL(kh.TheNhan, N'') AS [Ghi chú],
                       kh.DangHoatDong AS [Hoạt động], kh.MaHang
                FROM dbo.KhachHang kh
                JOIN dbo.CauHinh_HangThanhVien h ON h.MaHang = kh.MaHang
                WHERE (@Kw = N'' OR kh.HoTen LIKE N'%' + @Kw + N'%' OR kh.SoDienThoai LIKE N'%' + @Kw + N'%')
                ORDER BY kh.DiemHienTai DESC, kh.HoTen",
                new[] { DatabaseHelper.Param("@Kw", SqlDbType.NVarChar, keyword, 100) });
        }

        public static DataTable FindByPhone(string phone)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT kh.MaKhachHang, kh.HoTen, kh.SoDienThoai, kh.DiemHienTai, kh.DangHoatDong, h.TenHang, h.PhanTramGiam
                FROM dbo.KhachHang kh
                JOIN dbo.CauHinh_HangThanhVien h ON h.MaHang = kh.MaHang
                WHERE kh.SoDienThoai = @SoDienThoai",
                new[] { DatabaseHelper.Param("@SoDienThoai", SqlDbType.NVarChar, phone, 20) });
        }

        public static void Insert(CustomerSaveModel m)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                INSERT INTO dbo.KhachHang (MaHang, HoTen, SoDienThoai, DiemHienTai, Email, TheNhan, DangHoatDong)
                VALUES (@MaHang, @HoTen, @SDT, @Diem, @Email, @TheNhan, @Active)",
                new[]
                {
                    DatabaseHelper.Param("@MaHang", SqlDbType.Int, m.MaHang),
                    DatabaseHelper.Param("@HoTen", SqlDbType.NVarChar, m.HoTen, 150),
                    DatabaseHelper.Param("@SDT", SqlDbType.NVarChar, m.SoDienThoai, 20),
                    DatabaseHelper.Param("@Diem", SqlDbType.Int, m.Diem),
                    DatabaseHelper.Param("@Email", SqlDbType.NVarChar, (object?)m.Email ?? DBNull.Value, 150),
                    DatabaseHelper.Param("@TheNhan", SqlDbType.NVarChar, (object?)m.GhiChu ?? DBNull.Value, 255),
                    DatabaseHelper.Param("@Active", SqlDbType.Bit, m.DangHoatDong)
                });
        }

        public static void Update(CustomerSaveModel m)
        {
            DatabaseHelper.ExecuteNonQuery(@"
                UPDATE dbo.KhachHang
                SET MaHang = @MaHang, HoTen = @HoTen, SoDienThoai = @SDT, DiemHienTai = @Diem,
                    Email = @Email, TheNhan = @TheNhan, DangHoatDong = @Active
                WHERE MaKhachHang = @ID",
                new[]
                {
                    DatabaseHelper.Param("@ID", SqlDbType.Int, m.CustomerId),
                    DatabaseHelper.Param("@MaHang", SqlDbType.Int, m.MaHang),
                    DatabaseHelper.Param("@HoTen", SqlDbType.NVarChar, m.HoTen, 150),
                    DatabaseHelper.Param("@SDT", SqlDbType.NVarChar, m.SoDienThoai, 20),
                    DatabaseHelper.Param("@Diem", SqlDbType.Int, m.Diem),
                    DatabaseHelper.Param("@Email", SqlDbType.NVarChar, (object?)m.Email ?? DBNull.Value, 150),
                    DatabaseHelper.Param("@TheNhan", SqlDbType.NVarChar, (object?)m.GhiChu ?? DBNull.Value, 255),
                    DatabaseHelper.Param("@Active", SqlDbType.Bit, m.DangHoatDong)
                });
        }

        public static void Deactivate(int customerId)
        {
            DatabaseHelper.ExecuteNonQuery(
                "UPDATE dbo.KhachHang SET DangHoatDong = 0 WHERE MaKhachHang = @ID",
                new[] { DatabaseHelper.Param("@ID", SqlDbType.Int, customerId) });
        }
    }
}
