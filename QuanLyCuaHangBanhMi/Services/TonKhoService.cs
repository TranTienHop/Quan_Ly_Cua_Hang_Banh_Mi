using System.Data;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class TonKhoService
    {
        /// <summary>
        /// Tồn kho một chi nhánh. Nguyên liệu chưa có dòng tồn được coi là tồn 0.
        /// TrangThai: Het (tồn &lt;= 0) | SapHet (tồn &lt;= tối thiểu) | Con
        /// </summary>
        public static DataTable GetTon(int maChiNhanh, string keyword, bool chiSapHet) => Db_QLKho.Query(@"
            SELECT nl.MaNguyenLieu,
                   nl.TenNguyenLieu,
                   d.TenDonVi,
                   ISNULL(tk.SoLuongTon, 0)        AS SoLuongTon,
                   nl.TonToiThieuCanhBao           AS TonToiThieu,
                   CASE WHEN ISNULL(tk.SoLuongTon, 0) < nl.TonToiThieuCanhBao
                        THEN nl.TonToiThieuCanhBao - ISNULL(tk.SoLuongTon, 0) ELSE 0 END AS CanBoSung,
                   tk.NgayCapNhat,
                   CASE WHEN ISNULL(tk.SoLuongTon, 0) <= 0 THEN 'Het'
                        WHEN tk.SoLuongTon <= nl.TonToiThieuCanhBao THEN 'SapHet'
                        ELSE 'Con' END            AS TrangThai
            FROM dbo.NguyenLieu nl
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            LEFT JOIN dbo.TonKho_ChiNhanh tk
                   ON tk.MaNguyenLieu = nl.MaNguyenLieu AND tk.MaChiNhanh = @cn
            WHERE nl.DangHoatDong = 1
              AND nl.TenNguyenLieu LIKE @k
              AND (@sap = 0 OR ISNULL(tk.SoLuongTon, 0) <= nl.TonToiThieuCanhBao)
            ORDER BY CASE WHEN ISNULL(tk.SoLuongTon, 0) <= 0 THEN 0
                          WHEN tk.SoLuongTon <= nl.TonToiThieuCanhBao THEN 1 ELSE 2 END,
                     nl.TenNguyenLieu",
            Db_QLKho.P("@cn", maChiNhanh),
            Db_QLKho.P("@k", "%" + keyword + "%"),
            Db_QLKho.P("@sap", chiSapHet));
    }
}