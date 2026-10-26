using System.Data;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class LookupService
    {
        public static DataTable ChiNhanh() => Db_QLKho.Query(
            @"SELECT MaChiNhanh, TenChiNhanh
              FROM dbo.ChiNhanh
              WHERE DangHoatDong = 1
              ORDER BY MaChiNhanh");

        /// <summary>Nguyên liệu đang dùng kèm đơn vị và tồn tại một chi nhánh (null thì tồn = 0).</summary>
        public static DataTable NguyenLieu(int? maChiNhanh) => Db_QLKho.Query(@"
            SELECT nl.MaNguyenLieu, nl.TenNguyenLieu, d.TenDonVi, ISNULL(tk.SoLuongTon, 0) AS SoLuongTon
            FROM dbo.NguyenLieu nl
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            LEFT JOIN dbo.TonKho_ChiNhanh tk
                   ON tk.MaNguyenLieu = nl.MaNguyenLieu AND tk.MaChiNhanh = @cn
            WHERE nl.DangHoatDong = 1
            ORDER BY nl.TenNguyenLieu",
            Db_QLKho.P("@cn", maChiNhanh));

        /// <summary>Nguyên liệu CÓ TỒN tại chi nhánh (kể cả nguyên liệu đã ngừng dùng) để lập phiếu hủy.</summary>
        public static DataTable NguyenLieuCoTon(int maChiNhanh) => Db_QLKho.Query(@"
            SELECT nl.MaNguyenLieu, nl.TenNguyenLieu, d.TenDonVi, tk.SoLuongTon
            FROM dbo.NguyenLieu nl
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            JOIN dbo.TonKho_ChiNhanh tk
                ON tk.MaNguyenLieu = nl.MaNguyenLieu AND tk.MaChiNhanh = @cn
            WHERE tk.SoLuongTon > 0
            ORDER BY nl.TenNguyenLieu",
            Db_QLKho.P("@cn", maChiNhanh));

        /// <summary>Mọi nguyên liệu (kể cả đã ngừng dùng) để lọc lịch sử.</summary>
        public static DataTable NguyenLieuTatCa() => Db_QLKho.Query(@"
            SELECT MaNguyenLieu,
                CASE WHEN DangHoatDong = 1 THEN TenNguyenLieu
                    ELSE TenNguyenLieu + N' (ngừng dùng)' END AS TenNguyenLieu
            FROM dbo.NguyenLieu
            ORDER BY TenNguyenLieu");
    }
}