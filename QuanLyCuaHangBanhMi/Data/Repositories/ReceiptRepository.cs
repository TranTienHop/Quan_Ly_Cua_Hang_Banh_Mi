using System.Data;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    public static class ReceiptRepository
    {
        public static DataTable GetHeader(int maHoaDon)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT hd.MaDonLocal, hd.NgayTao, hd.NguonDon, hd.LoaiDon, hd.TongTienHang, hd.TienGiam, hd.PhiShip,
                       hd.TongThanhToan, hd.DiemCong, hd.DiemTru, hd.TrangThaiDon,
                       cn.TenChiNhanh, cn.DiaChi, cn.SoDienThoai AS SdtChiNhanh,
                       nv.HoTen AS ThuNgan, kh.HoTen AS TenKhach, kh.SoDienThoai AS SdtKhach, km.TenKhuyenMai,
                       gh.DiaChi AS DiaChiGiao
                FROM dbo.HoaDon hd
                JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = hd.MaChiNhanh
                JOIN dbo.NhanVien nv ON nv.MaNhanVien = hd.MaThuNgan
                LEFT JOIN dbo.KhachHang kh ON kh.MaKhachHang = hd.MaKhachHang
                LEFT JOIN dbo.KhuyenMai km ON km.MaKhuyenMai = hd.MaKhuyenMai
                LEFT JOIN dbo.NhatKy_GiaoHang gh ON gh.MaHoaDon = hd.MaHoaDon
                WHERE hd.MaHoaDon = @MaHoaDon",
                new[] { DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon) });
        }

        public static DataTable GetLines(int maHoaDon)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT TenSanPhamChup, SoLuong, DonGia, ThanhTien, GhiChu
                FROM dbo.ChiTiet_HoaDon WHERE MaHoaDon = @MaHoaDon ORDER BY MaChiTiet",
                new[] { DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon) });
        }

        public static DataTable GetPayments(int maHoaDon)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT HinhThuc, SoTien, MaThamChieu
                FROM dbo.ThanhToan_HoaDon WHERE MaHoaDon = @MaHoaDon ORDER BY MaThanhToan",
                new[] { DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon) });
        }
    }
}
