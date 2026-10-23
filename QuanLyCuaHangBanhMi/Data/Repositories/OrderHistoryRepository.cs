using System;
using System.Data;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    public static class OrderHistoryRepository
    {
        public static DataTable GetOrdersByDate(int maChiNhanh, DateTime ngay)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT hd.MaHoaDon, hd.MaDonLocal, hd.NgayTao, hd.NguonDon, hd.LoaiDon, hd.TienGiam, hd.PhiShip,
                       hd.TongThanhToan, hd.TrangThaiDon, kh.HoTen AS TenKhach, nv.HoTen AS ThuNgan,
                       (SELECT STRING_AGG(tt.HinhThuc, N', ') FROM dbo.ThanhToan_HoaDon tt WHERE tt.MaHoaDon = hd.MaHoaDon) AS HinhThuc
                FROM dbo.HoaDon hd
                JOIN dbo.NhanVien nv ON nv.MaNhanVien = hd.MaThuNgan
                LEFT JOIN dbo.KhachHang kh ON kh.MaKhachHang = hd.MaKhachHang
                WHERE hd.MaChiNhanh = @MaChiNhanh
                  AND hd.NgayTao >= @Ngay AND hd.NgayTao < DATEADD(DAY, 1, @Ngay)
                ORDER BY hd.NgayTao DESC",
                new[]
                {
                    DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, maChiNhanh),
                    DatabaseHelper.Param("@Ngay", SqlDbType.Date, ngay.Date)
                });
        }

        public static DataTable GetOrderDetails(int maHoaDon)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT TenSanPhamChup, SoLuong, DonGia, ThanhTien, GhiChu
                FROM dbo.ChiTiet_HoaDon WHERE MaHoaDon = @MaHoaDon ORDER BY MaChiTiet",
                new[] { DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon) });
        }
    }
}
