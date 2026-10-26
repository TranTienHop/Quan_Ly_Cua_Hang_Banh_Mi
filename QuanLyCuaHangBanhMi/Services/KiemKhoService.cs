using System.Data;
using System.Text;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class KiemKhoService
    {
        /// <summary>gioiHanCN = null nghĩa là không giới hạn (ADMIN); ngược lại chỉ được thao tác cho chi nhánh đó.</summary>
        private static void KiemTraQuyen(int? gioiHanCN, int chiNhanhPhaiLa, string viec)
        {
            if (gioiHanCN != null && gioiHanCN != chiNhanhPhaiLa)
                throw new InvalidOperationException($"Bạn chỉ được {viec} cho chi nhánh của mình.");
        }

        // =====================================================================
        //  LẬP PHIẾU
        // =====================================================================

        /// <summary>Nguyên liệu có thể đưa vào đợt kiểm, kèm tồn hiện tại tại chi nhánh.</summary>
        public static DataTable NguyenLieuDeKiem(int maCN) => Db_QLKho.Query(@"
            SELECT nl.MaNguyenLieu, nl.TenNguyenLieu, d.TenDonVi, ISNULL(tk.SoLuongTon, 0) AS SoLuongTon
            FROM dbo.NguyenLieu nl
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            LEFT JOIN dbo.TonKho_ChiNhanh tk
                   ON tk.MaNguyenLieu = nl.MaNguyenLieu AND tk.MaChiNhanh = @cn
            WHERE nl.DangHoatDong = 1 OR ISNULL(tk.SoLuongTon, 0) > 0
            ORDER BY nl.TenNguyenLieu",
            Db_QLKho.P("@cn", maCN));

        /// <summary>Bắt đầu một đợt kiểm: chụp tồn hiện tại của các nguyên liệu đã chọn. Trả về mã phiếu.</summary>
        public static int TaoPhieu(int maCN, int maNhanVien, string? ghiChu, IEnumerable<int> maNguyenLieu, int? gioiHanCN)
        {
            KiemTraQuyen(gioiHanCN, maCN, "lập phiếu kiểm kho");
            var ids = maNguyenLieu.Distinct().ToList();
            if (ids.Count == 0)
                throw new InvalidOperationException("Hãy chọn ít nhất một nguyên liệu để kiểm.");
            if (!string.IsNullOrWhiteSpace(ghiChu) && ghiChu.Trim().Length > 255)
                throw new InvalidOperationException("Ghi chú tối đa 255 ký tự.");

            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int dangMo = Convert.ToInt32(Db_QLKho.Scalar(conn, tran, @"
                    SELECT COUNT(*) FROM dbo.Phieu_KiemKho WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaChiNhanh = @cn AND TrangThai = N'DangKiem'", Db_QLKho.P("@cn", maCN)));
                if (dangMo > 0)
                    throw new InvalidOperationException(
                        "Chi nhánh đang có một phiếu kiểm kho chưa hoàn thành. Hãy hoàn thành hoặc xóa phiếu đó trước.");

                int ma = Convert.ToInt32(Db_QLKho.Scalar(conn, tran, @"
                    INSERT dbo.Phieu_KiemKho (MaChiNhanh, NgayKiem, NguoiKiem, GhiChu)
                    OUTPUT INSERTED.MaPhieu
                    VALUES (@cn, SYSDATETIME(), @nv, @gc)",
                    Db_QLKho.P("@cn", maCN), Db_QLKho.P("@nv", maNhanVien),
                    Db_QLKho.P("@gc", string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu.Trim())));

                // ids là danh sách số nguyên do chương trình tạo nên ghép chuỗi ở đây là an toàn
                int n = Db_QLKho.Exec(conn, tran, $@"
                    INSERT dbo.ChiTiet_KiemKho (MaPhieu, MaNguyenLieu, SoLuongHeThong, SoLuongThucTe)
                    SELECT @p, nl.MaNguyenLieu, ISNULL(tk.SoLuongTon, 0), ISNULL(tk.SoLuongTon, 0)
                    FROM dbo.NguyenLieu nl
                    LEFT JOIN dbo.TonKho_ChiNhanh tk
                           ON tk.MaNguyenLieu = nl.MaNguyenLieu AND tk.MaChiNhanh = @cn
                    WHERE nl.MaNguyenLieu IN ({string.Join(",", ids)})",
                    Db_QLKho.P("@p", ma), Db_QLKho.P("@cn", maCN));
                if (n == 0)
                    throw new InvalidOperationException("Không có nguyên liệu hợp lệ để kiểm.");

                tran.Commit();
                return ma;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // =====================================================================
        //  NHẬP SỐ THỰC TẾ (lưu ngay từng ô)
        // =====================================================================

        public static void CapNhatThucTe(int maChiTiet, decimal thucTe)
        {
            if (thucTe < 0)
                throw new InvalidOperationException("Số thực tế không được âm.");

            int n = Db_QLKho.Exec(@"
                UPDATE ct SET SoLuongThucTe = @t
                FROM dbo.ChiTiet_KiemKho ct
                JOIN dbo.Phieu_KiemKho p ON p.MaPhieu = ct.MaPhieu
                WHERE ct.MaChiTiet = @c AND p.TrangThai = N'DangKiem'",
                Db_QLKho.P("@t", thucTe), Db_QLKho.P("@c", maChiTiet));
            if (n == 0)
                throw new InvalidOperationException("Phiếu đã hoàn thành (hoặc đã bị xóa) nên không sửa được.");
        }

        // =====================================================================
        //  HOÀN THÀNH (điều chỉnh kho theo chênh lệch) / XÓA PHIẾU
        // =====================================================================

        /// <summary>Hoàn thành phiếu: ghi KIEM_KE cho mỗi dòng lệch. Trả về số nguyên liệu đã điều chỉnh.</summary>
        public static int HoanThanh(int maPhieu, int maNhanVien, int? gioiHanCN)
        {
            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran, @"
                    SELECT MaChiNhanh, TrangThai FROM dbo.Phieu_KiemKho WITH (UPDLOCK, ROWLOCK)
                    WHERE MaPhieu = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu kiểm kho.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "DangKiem")
                    throw new InvalidOperationException("Phiếu này đã hoàn thành rồi.");
                int maCN = Convert.ToInt32(h.Rows[0]["MaChiNhanh"]);
                KiemTraQuyen(gioiHanCN, maCN, "hoàn thành phiếu kiểm kho");

                var ct = Db_QLKho.Query(conn, tran, @"
                    SELECT ct.MaNguyenLieu, nl.TenNguyenLieu, d.TenDonVi,
                           ct.SoLuongThucTe - ct.SoLuongHeThong AS ChenhLech,
                           ISNULL(tk.SoLuongTon, 0) AS Ton
                    FROM dbo.ChiTiet_KiemKho ct
                    JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
                    JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
                    LEFT JOIN dbo.TonKho_ChiNhanh tk
                           ON tk.MaNguyenLieu = ct.MaNguyenLieu AND tk.MaChiNhanh = @cn
                    WHERE ct.MaPhieu = @p AND ct.SoLuongThucTe <> ct.SoLuongHeThong
                    ORDER BY ct.MaChiTiet",
                    Db_QLKho.P("@p", maPhieu), Db_QLKho.P("@cn", maCN));

                // Mức giảm không được lớn hơn tồn hiện tại (hàng có thể đã xuất sau lúc bắt đầu kiểm)
                var thieu = new StringBuilder();
                foreach (DataRow r in ct.Rows)
                {
                    decimal lech = Convert.ToDecimal(r["ChenhLech"]), ton = Convert.ToDecimal(r["Ton"]);
                    if (lech < 0 && -lech > ton)
                        thieu.AppendLine($"• {r["TenNguyenLieu"]}: kiểm kê giảm {-lech:#,##0.###} nhưng tồn hiện tại chỉ còn {ton:#,##0.###} {r["TenDonVi"]}");
                }
                if (thieu.Length > 0)
                    throw new InvalidOperationException(
                        "Không thể điều chỉnh vì tồn hiện tại thấp hơn mức giảm theo kiểm kê " +
                        "(hàng đã xuất, hủy hoặc chuyển đi sau lúc bắt đầu kiểm). Phiếu vẫn giữ trạng thái Đang kiểm:\n\n" +
                        thieu + "\nHãy đếm lại các dòng này, hoặc xóa phiếu và kiểm lại.");

                string ghiChu = $"Điều chỉnh theo phiếu kiểm kho KK{maPhieu:D5}";
                foreach (DataRow r in ct.Rows)
                    SoKhoService.BienDong(conn, tran, maCN, Convert.ToInt32(r["MaNguyenLieu"]),
                        Convert.ToDecimal(r["ChenhLech"]), "KIEM_KE", "KIEM_KHO", maPhieu, maNhanVien, ghiChu);

                Db_QLKho.Exec(conn, tran,
                    "UPDATE dbo.Phieu_KiemKho SET TrangThai = N'HoanThanh' WHERE MaPhieu = @p",
                    Db_QLKho.P("@p", maPhieu));
                tran.Commit();
                return ct.Rows.Count;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        /// <summary>Xóa phiếu đang kiểm (chưa ảnh hưởng kho). Phiếu đã hoàn thành không xóa được.</summary>
        public static void XoaPhieu(int maPhieu, int? gioiHanCN)
        {
            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran, @"
                    SELECT MaChiNhanh, TrangThai FROM dbo.Phieu_KiemKho WITH (UPDLOCK, ROWLOCK)
                    WHERE MaPhieu = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu kiểm kho.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "DangKiem")
                    throw new InvalidOperationException("Chỉ xóa được phiếu đang kiểm; phiếu đã hoàn thành được giữ lại làm chứng từ.");
                KiemTraQuyen(gioiHanCN, Convert.ToInt32(h.Rows[0]["MaChiNhanh"]), "xóa phiếu kiểm kho");

                Db_QLKho.Exec(conn, tran, "DELETE dbo.ChiTiet_KiemKho WHERE MaPhieu = @p", Db_QLKho.P("@p", maPhieu));
                Db_QLKho.Exec(conn, tran, "DELETE dbo.Phieu_KiemKho WHERE MaPhieu = @p", Db_QLKho.P("@p", maPhieu));
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // =====================================================================
        //  TRA CỨU
        // =====================================================================

        /// <summary>Danh sách phiếu một chi nhánh; phiếu đang kiểm xếp lên đầu.</summary>
        public static DataTable DanhSach(int maCN) => Db_QLKho.Query(@"
            SELECT p.MaPhieu,
                   'KK' + RIGHT('00000' + CAST(p.MaPhieu AS VARCHAR(10)), 5) AS SoPhieu,
                   p.NgayKiem,
                   nv.HoTen AS NguoiKiem,
                   p.GhiChu,
                   p.TrangThai,
                   (SELECT COUNT(*) FROM dbo.ChiTiet_KiemKho c WHERE c.MaPhieu = p.MaPhieu) AS SoDong,
                   (SELECT COUNT(*) FROM dbo.ChiTiet_KiemKho c
                    WHERE c.MaPhieu = p.MaPhieu AND c.SoLuongThucTe <> c.SoLuongHeThong) AS SoLech
            FROM dbo.Phieu_KiemKho p
            JOIN dbo.NhanVien nv ON nv.MaNhanVien = p.NguoiKiem
            WHERE p.MaChiNhanh = @cn
            ORDER BY CASE WHEN p.TrangThai = N'DangKiem' THEN 0 ELSE 1 END, p.NgayKiem DESC, p.MaPhieu DESC",
            Db_QLKho.P("@cn", maCN));

        /// <summary>Chi tiết phiếu kèm tồn HIỆN TẠI để người kiểm thấy hàng có phát sinh trong lúc đếm.</summary>
        public static DataTable GetChiTiet(int maPhieu) => Db_QLKho.Query(@"
            SELECT ct.MaChiTiet,
                   ct.MaNguyenLieu,
                   nl.TenNguyenLieu,
                   d.TenDonVi,
                   ct.SoLuongHeThong,
                   ct.SoLuongThucTe,
                   ct.SoLuongThucTe - ct.SoLuongHeThong AS ChenhLech,
                   ISNULL(tk.SoLuongTon, 0) AS TonHienTai
            FROM dbo.ChiTiet_KiemKho ct
            JOIN dbo.Phieu_KiemKho p ON p.MaPhieu = ct.MaPhieu
            JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            LEFT JOIN dbo.TonKho_ChiNhanh tk
                   ON tk.MaNguyenLieu = ct.MaNguyenLieu AND tk.MaChiNhanh = p.MaChiNhanh
            WHERE ct.MaPhieu = @p
            ORDER BY nl.TenNguyenLieu",
            Db_QLKho.P("@p", maPhieu));
    }
}