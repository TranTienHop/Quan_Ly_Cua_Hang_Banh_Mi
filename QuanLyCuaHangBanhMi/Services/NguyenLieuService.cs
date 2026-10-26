using System.Data;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class NguyenLieuService
    {
        // =====================================================================
        //  DANH MỤC NGUYÊN LIỆU
        // =====================================================================

        /// <summary>
        /// Danh sách nguyên liệu kèm tồn tại chi nhánh đang đăng nhập, tổng tồn mọi chi nhánh
        /// và số món đang dùng nguyên liệu này trong công thức.
        /// </summary>
        public static DataTable GetAll(string keyword, bool? hoatDong, int maChiNhanh) => Db_QLKho.Query(@"
            SELECT nl.MaNguyenLieu,
                   CASE WHEN nl.MaNguyenLieu < 1000
                        THEN 'NL' + RIGHT('000' + CAST(nl.MaNguyenLieu AS VARCHAR(10)), 3)
                        ELSE 'NL' + CAST(nl.MaNguyenLieu AS VARCHAR(10)) END AS MaHienThi,
                   nl.TenNguyenLieu,
                   d.TenDonVi,
                   nl.MaDonViGoc,
                   nl.TonToiThieuCanhBao,
                   ISNULL(tcn.SoLuongTon, 0) AS TonTaiCN,
                   ISNULL(tt.TongTon, 0)     AS TongTon,
                   ISNULL(m.SoMon, 0)        AS SoMon,
                   CASE WHEN nl.DangHoatDong = 1 THEN N'Đang dùng' ELSE N'Ngừng dùng' END AS TrangThai,
                   nl.DangHoatDong
            FROM dbo.NguyenLieu nl
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            LEFT JOIN dbo.TonKho_ChiNhanh tcn
                   ON tcn.MaNguyenLieu = nl.MaNguyenLieu AND tcn.MaChiNhanh = @cn
            LEFT JOIN (SELECT MaNguyenLieu, SUM(SoLuongTon) AS TongTon
                       FROM dbo.TonKho_ChiNhanh GROUP BY MaNguyenLieu) tt
                   ON tt.MaNguyenLieu = nl.MaNguyenLieu
            LEFT JOIN (SELECT MaNguyenLieu, COUNT(*) AS SoMon
                       FROM dbo.CongThuc GROUP BY MaNguyenLieu) m
                   ON m.MaNguyenLieu = nl.MaNguyenLieu
            WHERE nl.TenNguyenLieu LIKE @k
              AND (@hd IS NULL OR nl.DangHoatDong = @hd)
            ORDER BY nl.TenNguyenLieu",
            Db_QLKho.P("@cn", maChiNhanh),
            Db_QLKho.P("@k", "%" + keyword + "%"),
            Db_QLKho.P("@hd", hoatDong));

        public static DataTable GetDonVi() => Db_QLKho.Query(
            "SELECT MaDonVi, TenDonVi FROM dbo.DonVi ORDER BY TenDonVi");

        /// <summary>Nguyên liệu đang dùng (kèm đơn vị) cho ComboBox thêm dòng công thức.</summary>
        public static DataTable GetCombo() => Db_QLKho.Query(@"
            SELECT nl.MaNguyenLieu, nl.TenNguyenLieu, d.TenDonVi
            FROM dbo.NguyenLieu nl
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            WHERE nl.DangHoatDong = 1
            ORDER BY nl.TenNguyenLieu");

        /// <summary>Nguyên liệu đã có dữ liệu gắn với nó (công thức, tồn kho, nhật ký, phiếu nhập)?</summary>
        public static bool CoPhatSinh(int ma) => Convert.ToInt32(Db_QLKho.Scalar(@"
            SELECT CASE WHEN EXISTS (SELECT 1 FROM dbo.CongThuc        WHERE MaNguyenLieu = @ma)
                          OR EXISTS (SELECT 1 FROM dbo.TonKho_ChiNhanh WHERE MaNguyenLieu = @ma)
                          OR EXISTS (SELECT 1 FROM dbo.NhatKy_Kho      WHERE MaNguyenLieu = @ma)
                          OR EXISTS (SELECT 1 FROM dbo.ChiTiet_NhapHang WHERE MaNguyenLieu = @ma)
                        THEN 1 ELSE 0 END",
            Db_QLKho.P("@ma", ma))) == 1;

        /// <summary>Số món ĐANG BÁN có nguyên liệu này trong công thức.</summary>
        public static int DemMonDangBan(int ma) => Convert.ToInt32(Db_QLKho.Scalar(@"
            SELECT COUNT(*)
            FROM dbo.CongThuc ct
            JOIN dbo.SanPham sp ON sp.MaSanPham = ct.MaSanPham
            WHERE ct.MaNguyenLieu = @ma AND sp.DangHoatDong = 1",
            Db_QLKho.P("@ma", ma)));

        /// <summary>Các món dùng nguyên liệu này, kèm định lượng.</summary>
        public static DataTable GetMonDung(int ma) => Db_QLKho.Query(@"
            SELECT sp.MaSKU, sp.TenSanPham, ct.SoLuongTrenMotPhan
            FROM dbo.CongThuc ct
            JOIN dbo.SanPham sp ON sp.MaSanPham = ct.MaSanPham
            WHERE ct.MaNguyenLieu = @ma
            ORDER BY sp.TenSanPham",
            Db_QLKho.P("@ma", ma));

        /// <summary>Thêm mới (ma = null) hoặc cập nhật. Trả về mã nguyên liệu.</summary>
        public static int Save(int? ma, string ten, int maDonVi, decimal tonToiThieu, bool hoatDong)
        {
            ten = (ten ?? "").Trim();
            if (ten.Length == 0)
                throw new InvalidOperationException("Tên nguyên liệu không được để trống.");
            if (ten.Length > 200)
                throw new InvalidOperationException("Tên nguyên liệu tối đa 200 ký tự.");
            if (tonToiThieu < 0)
                throw new InvalidOperationException("Tồn tối thiểu phải lớn hơn hoặc bằng 0.");

            int trung = Convert.ToInt32(Db_QLKho.Scalar(
                "SELECT COUNT(*) FROM dbo.NguyenLieu WHERE TenNguyenLieu = @t AND MaNguyenLieu <> @ma",
                Db_QLKho.P("@t", ten), Db_QLKho.P("@ma", ma ?? 0)));
            if (trung > 0)
                throw new InvalidOperationException($"Đã có nguyên liệu tên \"{ten}\".");

            if (ma == null)
            {
                return Convert.ToInt32(Db_QLKho.Scalar(@"
                    INSERT dbo.NguyenLieu (TenNguyenLieu, MaDonViGoc, TonToiThieuCanhBao, DangHoatDong)
                    OUTPUT INSERTED.MaNguyenLieu
                    VALUES (@t, @dv, @min, @hd)",
                    Db_QLKho.P("@t", ten), Db_QLKho.P("@dv", maDonVi), Db_QLKho.P("@min", tonToiThieu), Db_QLKho.P("@hd", hoatDong)));
            }

            object? cu = Db_QLKho.Scalar("SELECT MaDonViGoc FROM dbo.NguyenLieu WHERE MaNguyenLieu = @ma", Db_QLKho.P("@ma", ma));
            if (cu == null)
                throw new InvalidOperationException("Không tìm thấy nguyên liệu (có thể đã bị xóa).");

            // Lớp bảo vệ thật nằm ở service; form chỉ khóa ô cho dễ nhìn
            if (Convert.ToInt32(cu) != maDonVi && CoPhatSinh(ma.Value))
                throw new InvalidOperationException(
                    "Nguyên liệu đã có tồn kho, công thức hoặc phiếu nên không thể đổi đơn vị gốc " +
                    "(sẽ làm sai toàn bộ số liệu cũ).");

            Db_QLKho.Exec(@"
                UPDATE dbo.NguyenLieu
                SET TenNguyenLieu = @t, MaDonViGoc = @dv, TonToiThieuCanhBao = @min, DangHoatDong = @hd
                WHERE MaNguyenLieu = @ma",
                Db_QLKho.P("@t", ten), Db_QLKho.P("@dv", maDonVi), Db_QLKho.P("@min", tonToiThieu),
                Db_QLKho.P("@hd", hoatDong), Db_QLKho.P("@ma", ma));
            return ma.Value;
        }

        // =====================================================================
        //  CÔNG THỨC MÓN
        // =====================================================================

        /// <summary>Các món thành phẩm đang bán, kèm số nguyên liệu trong công thức.</summary>
        public static DataTable GetSanPham(string keyword) => Db_QLKho.Query(@"
            SELECT sp.MaSanPham, sp.MaSKU, sp.TenSanPham, dm.TenDanhMuc,
                   (SELECT COUNT(*) FROM dbo.CongThuc ct WHERE ct.MaSanPham = sp.MaSanPham) AS SoNguyenLieu
            FROM dbo.SanPham sp
            JOIN dbo.DanhMuc dm ON dm.MaDanhMuc = sp.MaDanhMuc
            WHERE sp.LoaiSanPham = N'ThanhPham' AND sp.DangHoatDong = 1
              AND (sp.TenSanPham LIKE @k OR sp.MaSKU LIKE @k)
            ORDER BY sp.MaSKU, sp.TenSanPham",
            Db_QLKho.P("@k", "%" + keyword + "%"));

        /// <summary>Thứ tự cột: MaNguyenLieu, TenNguyenLieu, TenDonVi, SoLuongTrenMotPhan (form dựa vào thứ tự này).</summary>
        public static DataTable GetCongThuc(int maSanPham) => Db_QLKho.Query(@"
            SELECT ct.MaNguyenLieu, nl.TenNguyenLieu, d.TenDonVi, ct.SoLuongTrenMotPhan
            FROM dbo.CongThuc ct
            JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            WHERE ct.MaSanPham = @sp
            ORDER BY nl.TenNguyenLieu",
            Db_QLKho.P("@sp", maSanPham));

        /// <summary>Ghi lại toàn bộ công thức của một món (xóa cũ, ghi mới) trong một transaction.</summary>
        public static void SaveCongThuc(int maSanPham, IEnumerable<(int MaNguyenLieu, decimal SoLuong)> dong)
        {
            var list = dong.ToList();
            if (list.GroupBy(x => x.MaNguyenLieu).Any(g => g.Count() > 1))
                throw new InvalidOperationException("Một nguyên liệu chỉ được xuất hiện một lần trong công thức.");
            if (list.Any(x => x.SoLuong <= 0))
                throw new InvalidOperationException("Định lượng phải lớn hơn 0.");

            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var sp = Db_QLKho.Query(conn, tran,
                    "SELECT LoaiSanPham, BatBuocCongThuc FROM dbo.SanPham WHERE MaSanPham = @sp",
                    Db_QLKho.P("@sp", maSanPham));
                if (sp.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy món.");
                if (!string.Equals(Convert.ToString(sp.Rows[0]["LoaiSanPham"]), "ThanhPham", StringComparison.OrdinalIgnoreCase))
                    throw new InvalidOperationException(
                        "Chỉ món thành phẩm mới có công thức (combo trừ kho qua công thức của các món thành phần).");
                if (list.Count == 0 && Convert.ToBoolean(sp.Rows[0]["BatBuocCongThuc"]))
                    throw new InvalidOperationException(
                        "Món này bắt buộc có công thức, hãy giữ lại ít nhất một nguyên liệu.");

                Db_QLKho.Exec(conn, tran, "DELETE dbo.CongThuc WHERE MaSanPham = @sp", Db_QLKho.P("@sp", maSanPham));
                foreach (var (maNL, sl) in list)
                    Db_QLKho.Exec(conn, tran,
                        @"INSERT dbo.CongThuc (MaSanPham, MaNguyenLieu, SoLuongTrenMotPhan)
                          VALUES (@sp, @nl, @sl)",
                        Db_QLKho.P("@sp", maSanPham), Db_QLKho.P("@nl", maNL), Db_QLKho.P("@sl", sl));

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }
    }
}