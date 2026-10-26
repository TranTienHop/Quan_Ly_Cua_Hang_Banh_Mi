using System.Data;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class NhapKhoService
    {
        // =====================================================================
        //  LẬP PHIẾU (chưa ảnh hưởng kho)
        // =====================================================================

        /// <summary>
        /// Tạo phiếu nhập ở trạng thái ChoNhap. ct cần các cột: MaNguyenLieu, SoLuong, DonGiaNhap.
        /// Trả về mã phiếu.
        /// </summary>
        public static int TaoPhieu(int maCN, int maNCC, DateTime ngayDat, int nguoiLap, string? ghiChu, DataTable ct)
        {
            if (ct.Rows.Count == 0)
                throw new InvalidOperationException("Phiếu nhập chưa có dòng nào.");
            if (ngayDat.Date > DateTime.Today)
                throw new InvalidOperationException("Ngày đặt không được ở tương lai.");

            var ids = new HashSet<int>();
            foreach (DataRow r in ct.Rows)
            {
                if (r["MaNguyenLieu"] == DBNull.Value)
                    throw new InvalidOperationException("Có dòng chưa chọn nguyên liệu.");
                if (!ids.Add(Convert.ToInt32(r["MaNguyenLieu"])))
                    throw new InvalidOperationException("Một nguyên liệu chỉ được xuất hiện một lần trong phiếu.");
                if (Convert.ToDecimal(r["SoLuong"]) <= 0)
                    throw new InvalidOperationException("Số lượng phải lớn hơn 0.");
                if (Convert.ToDecimal(r["DonGiaNhap"]) < 0)
                    throw new InvalidOperationException("Đơn giá không được âm.");
            }

            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int ncc = Convert.ToInt32(Db_QLKho.Scalar(conn, tran,
                    "SELECT COUNT(*) FROM dbo.NhaCungCap WHERE MaNCC = @n AND DangHoatDong = 1",
                    Db_QLKho.P("@n", maNCC)));
                if (ncc == 0)
                    throw new InvalidOperationException("Nhà cung cấp không tồn tại hoặc đã ngừng hợp tác.");

                // ids là danh sách số nguyên do chương trình tạo nên ghép chuỗi ở đây là an toàn
                int dung = Convert.ToInt32(Db_QLKho.Scalar(conn, tran,
                    $"SELECT COUNT(*) FROM dbo.NguyenLieu WHERE DangHoatDong = 1 AND MaNguyenLieu IN ({string.Join(",", ids)})"));
                if (dung != ids.Count)
                    throw new InvalidOperationException("Có nguyên liệu đã ngừng sử dụng hoặc không còn tồn tại.");

                decimal tong = ct.AsEnumerable().Sum(r =>
                    Math.Round(Convert.ToDecimal(r["SoLuong"]) * Convert.ToDecimal(r["DonGiaNhap"]), 2));

                int ma = Convert.ToInt32(Db_QLKho.Scalar(conn, tran, @"
                    INSERT dbo.Phieu_NhapHang (MaChiNhanh, MaNCC, NgayDat, TongTien, NguoiLap, GhiChu)
                    OUTPUT INSERTED.MaPhieuNhap
                    VALUES (@cn, @ncc, @ngay, @tong, @nv, @gc)",
                    Db_QLKho.P("@cn", maCN), Db_QLKho.P("@ncc", maNCC), Db_QLKho.P("@ngay", ngayDat.Date),
                    Db_QLKho.P("@tong", tong), Db_QLKho.P("@nv", nguoiLap),
                    Db_QLKho.P("@gc", string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu.Trim())));

                foreach (DataRow r in ct.Rows)
                {
                    decimal sl = Convert.ToDecimal(r["SoLuong"]);
                    decimal gia = Convert.ToDecimal(r["DonGiaNhap"]);
                    Db_QLKho.Exec(conn, tran, @"
                        INSERT dbo.ChiTiet_NhapHang (MaPhieuNhap, MaNguyenLieu, SoLuong, DonGiaNhap, ThanhTien)
                        VALUES (@p, @nl, @sl, @gia, @tt)",
                        Db_QLKho.P("@p", ma), Db_QLKho.P("@nl", Convert.ToInt32(r["MaNguyenLieu"])),
                        Db_QLKho.P("@sl", sl), Db_QLKho.P("@gia", gia), Db_QLKho.P("@tt", Math.Round(sl * gia, 2)));
                }

                tran.Commit();
                return ma;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        /// <summary>Giá nhập gần nhất của nguyên liệu (ưu tiên cùng nhà cung cấp). Null nếu chưa từng nhập.</summary>
        public static decimal? GoiYGia(int maNguyenLieu, int? maNCC)
        {
            const string sql = @"
                SELECT TOP 1 ct.DonGiaNhap
                FROM dbo.ChiTiet_NhapHang ct
                JOIN dbo.Phieu_NhapHang p ON p.MaPhieuNhap = ct.MaPhieuNhap
                WHERE ct.MaNguyenLieu = @nl AND p.TrangThai = N'DaNhap' {0}
                ORDER BY p.NgayDat DESC, ct.MaChiTiet DESC";

            object? v = null;
            if (maNCC != null)
                v = Db_QLKho.Scalar(string.Format(sql, "AND p.MaNCC = @ncc"), Db_QLKho.P("@nl", maNguyenLieu), Db_QLKho.P("@ncc", maNCC));
            v ??= Db_QLKho.Scalar(string.Format(sql, ""), Db_QLKho.P("@nl", maNguyenLieu));
            return v == null ? null : Convert.ToDecimal(v);
        }

        // =====================================================================
        //  NHẬN HÀNG (cộng kho)
        // =====================================================================

        public static void NhanHang(int maPhieu, int maNhanVien)
        {
            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran,
                    @"SELECT MaChiNhanh, TrangThai FROM dbo.Phieu_NhapHang WITH (UPDLOCK, ROWLOCK)
                      WHERE MaPhieuNhap = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu nhập.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "ChoNhap")
                    throw new InvalidOperationException("Phiếu này đã được xử lý nên không thể nhận hàng lại.");
                int maCN = Convert.ToInt32(h.Rows[0]["MaChiNhanh"]);

                var ct = Db_QLKho.Query(conn, tran,
                    @"SELECT MaNguyenLieu, SoLuong FROM dbo.ChiTiet_NhapHang
                      WHERE MaPhieuNhap = @p ORDER BY MaChiTiet", Db_QLKho.P("@p", maPhieu));
                if (ct.Rows.Count == 0)
                    throw new InvalidOperationException("Phiếu không có dòng chi tiết.");

                foreach (DataRow r in ct.Rows)
                    SoKhoService.BienDong(conn, tran, maCN, Convert.ToInt32(r["MaNguyenLieu"]),
                        Convert.ToDecimal(r["SoLuong"]), "NHAP", "PHIEU_NHAP", maPhieu, maNhanVien);

                Db_QLKho.Exec(conn, tran,
                    @"UPDATE dbo.Phieu_NhapHang
                      SET TrangThai = N'DaNhap', NgayNhan = CAST(SYSDATETIME() AS DATE)
                      WHERE MaPhieuNhap = @p", Db_QLKho.P("@p", maPhieu));

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // =====================================================================
        //  HỦY PHIẾU / THANH TOÁN CÔNG NỢ
        // =====================================================================

        /// <summary>Hủy phiếu chưa nhập hàng và chưa thanh toán.</summary>
        public static void HuyPhieu(int maPhieu)
        {
            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran,
                    @"SELECT TrangThai, DaThanhToan FROM dbo.Phieu_NhapHang WITH (UPDLOCK, ROWLOCK)
                      WHERE MaPhieuNhap = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu nhập.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "ChoNhap")
                    throw new InvalidOperationException("Chỉ hủy được phiếu đang chờ nhập hàng.");
                if (Convert.ToDecimal(h.Rows[0]["DaThanhToan"]) > 0)
                    throw new InvalidOperationException("Phiếu đã có thanh toán nên không thể hủy.");

                Db_QLKho.Exec(conn, tran,
                    "UPDATE dbo.Phieu_NhapHang SET TrangThai = N'DaHuy' WHERE MaPhieuNhap = @p",
                    Db_QLKho.P("@p", maPhieu));
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        /// <summary>Ghi nhận một lần thanh toán công nợ cho phiếu đã nhập hàng.</summary>
        public static void ThanhToan(int maPhieu, decimal soTien)
        {
            if (soTien <= 0)
                throw new InvalidOperationException("Số tiền thanh toán phải lớn hơn 0.");

            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran,
                    @"SELECT TongTien, DaThanhToan, TrangThai FROM dbo.Phieu_NhapHang WITH (UPDLOCK, ROWLOCK)
                      WHERE MaPhieuNhap = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu nhập.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "DaNhap")
                    throw new InvalidOperationException("Chỉ thanh toán công nợ cho phiếu đã nhập hàng.");

                decimal tong = Convert.ToDecimal(h.Rows[0]["TongTien"]);
                decimal da = Convert.ToDecimal(h.Rows[0]["DaThanhToan"]);
                decimal moi = da + soTien;
                if (moi > tong)
                    throw new InvalidOperationException($"Số tiền vượt quá công nợ còn lại ({tong - da:N0} đ).");

                Db_QLKho.Exec(conn, tran,
                    @"UPDATE dbo.Phieu_NhapHang
                      SET DaThanhToan = @m, TrangThaiCongNo = @tt
                      WHERE MaPhieuNhap = @p",
                    Db_QLKho.P("@m", moi), Db_QLKho.P("@tt", moi == tong ? "DaTT" : "TraMotPhan"), Db_QLKho.P("@p", maPhieu));
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

        /// <summary>
        /// Danh sách phiếu của một chi nhánh. ConNo và TrangThaiCongNo chỉ có nghĩa với phiếu đã nhập hàng
        /// (phiếu chờ nhập / đã hủy: ConNo = 0, TrangThaiCongNo = NULL).
        /// </summary>
        public static DataTable DanhSach(int maCN, string? trangThai, bool chiConNo, string keyword) => Db_QLKho.Query(@"
            SELECT p.MaPhieuNhap,
                   'PN' + RIGHT('00000' + CAST(p.MaPhieuNhap AS VARCHAR(10)), 5) AS SoPhieu,
                   ncc.TenNCC,
                   p.NgayDat,
                   p.NgayNhan,
                   p.TongTien,
                   p.DaThanhToan,
                   CASE WHEN p.TrangThai = N'DaNhap' THEN p.TongTien - p.DaThanhToan ELSE 0 END AS ConNo,
                   p.TrangThai,
                   CASE WHEN p.TrangThai = N'DaNhap' THEN p.TrangThaiCongNo ELSE NULL END AS TrangThaiCongNo,
                   nv.HoTen AS NguoiLap,
                   p.GhiChu
            FROM dbo.Phieu_NhapHang p
            JOIN dbo.NhaCungCap ncc ON ncc.MaNCC = p.MaNCC
            LEFT JOIN dbo.NhanVien nv ON nv.MaNhanVien = p.NguoiLap
            WHERE p.MaChiNhanh = @cn
              AND (@tt IS NULL OR p.TrangThai = @tt)
              AND (@no = 0 OR (p.TrangThai = N'DaNhap' AND p.TongTien - p.DaThanhToan > 0))
              AND (ncc.TenNCC LIKE @k
                   OR 'PN' + RIGHT('00000' + CAST(p.MaPhieuNhap AS VARCHAR(10)), 5) LIKE @k)
            ORDER BY p.NgayDat DESC, p.MaPhieuNhap DESC",
            Db_QLKho.P("@cn", maCN), Db_QLKho.P("@tt", trangThai), Db_QLKho.P("@no", chiConNo),
            Db_QLKho.P("@k", "%" + keyword + "%"));

        public static DataTable GetChiTiet(int maPhieu) => Db_QLKho.Query(@"
            SELECT nl.TenNguyenLieu, d.TenDonVi, ct.SoLuong, ct.DonGiaNhap, ct.ThanhTien
            FROM dbo.ChiTiet_NhapHang ct
            JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            WHERE ct.MaPhieuNhap = @p
            ORDER BY ct.MaChiTiet",
            Db_QLKho.P("@p", maPhieu));
    }
}