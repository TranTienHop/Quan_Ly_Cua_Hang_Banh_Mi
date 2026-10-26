using System.Data;
using System.Text;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class DieuChuyenService
    {
        /// <summary>gioiHanCN = null nghĩa là không giới hạn (ADMIN); ngược lại chỉ được thao tác cho chi nhánh đó.</summary>
        private static void KiemTraQuyen(int? gioiHanCN, int chiNhanhPhaiLa, string viec)
        {
            if (gioiHanCN != null && gioiHanCN != chiNhanhPhaiLa)
                throw new InvalidOperationException($"Bạn chỉ được {viec} cho chi nhánh của mình.");
        }

        // =====================================================================
        //  LẬP PHIẾU (chưa ảnh hưởng kho)
        // =====================================================================

        /// <summary>Tạo phiếu ở trạng thái ChoGui. ct cần các cột: MaNguyenLieu, SoLuongGui. Trả về mã phiếu.</summary>
        public static int TaoPhieu(int khoGui, int khoNhan, int nguoiLap, string? ghiChu, DataTable ct, int? gioiHanCN)
        {
            if (khoGui == khoNhan)
                throw new InvalidOperationException("Kho gửi và kho nhận phải khác nhau.");
            KiemTraQuyen(gioiHanCN, khoGui, "lập phiếu điều chuyển");
            if (ct.Rows.Count == 0)
                throw new InvalidOperationException("Phiếu điều chuyển chưa có dòng nào.");
            if (!string.IsNullOrWhiteSpace(ghiChu) && ghiChu.Trim().Length > 255)
                throw new InvalidOperationException("Ghi chú tối đa 255 ký tự.");

            var ids = new HashSet<int>();
            foreach (DataRow r in ct.Rows)
            {
                if (r["MaNguyenLieu"] == DBNull.Value)
                    throw new InvalidOperationException("Có dòng chưa chọn nguyên liệu.");
                if (!ids.Add(Convert.ToInt32(r["MaNguyenLieu"])))
                    throw new InvalidOperationException("Một nguyên liệu chỉ được xuất hiện một lần trong phiếu.");
                if (Convert.ToDecimal(r["SoLuongGui"]) <= 0)
                    throw new InvalidOperationException("Số lượng gửi phải lớn hơn 0.");
            }

            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int cn = Convert.ToInt32(Db_QLKho.Scalar(conn, tran,
                    "SELECT COUNT(*) FROM dbo.ChiNhanh WHERE MaChiNhanh IN (@g, @n) AND DangHoatDong = 1",
                    Db_QLKho.P("@g", khoGui), Db_QLKho.P("@n", khoNhan)));
                if (cn != 2)
                    throw new InvalidOperationException("Kho gửi hoặc kho nhận không tồn tại hoặc đã ngừng hoạt động.");

                // ids là danh sách số nguyên do chương trình tạo nên ghép chuỗi ở đây là an toàn
                int dung = Convert.ToInt32(Db_QLKho.Scalar(conn, tran,
                    $"SELECT COUNT(*) FROM dbo.NguyenLieu WHERE MaNguyenLieu IN ({string.Join(",", ids)})"));
                if (dung != ids.Count)
                    throw new InvalidOperationException("Có nguyên liệu không còn tồn tại.");

                int ma = Convert.ToInt32(Db_QLKho.Scalar(conn, tran, @"
                    INSERT dbo.Phieu_DieuChuyen (MaKhoGui, MaKhoNhan, NgayGui, NguoiGui, GhiChu)
                    OUTPUT INSERTED.MaPhieu
                    VALUES (@g, @n, SYSDATETIME(), @nv, @gc)",
                    Db_QLKho.P("@g", khoGui), Db_QLKho.P("@n", khoNhan), Db_QLKho.P("@nv", nguoiLap),
                    Db_QLKho.P("@gc", string.IsNullOrWhiteSpace(ghiChu) ? null : ghiChu.Trim())));

                foreach (DataRow r in ct.Rows)
                    Db_QLKho.Exec(conn, tran, @"
                        INSERT dbo.ChiTiet_DieuChuyen (MaPhieu, MaNguyenLieu, SoLuongGui)
                        VALUES (@p, @nl, @sl)",
                        Db_QLKho.P("@p", ma), Db_QLKho.P("@nl", Convert.ToInt32(r["MaNguyenLieu"])),
                        Db_QLKho.P("@sl", Convert.ToDecimal(r["SoLuongGui"])));

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
        //  GỬI HÀNG (trừ kho gửi)
        // =====================================================================

        public static void GuiHang(int maPhieu, int maNhanVien, int? gioiHanCN)
        {
            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran, @"
                    SELECT p.MaKhoGui, p.TrangThai, nhan.MaCode AS MaCodeNhan, nhan.TenChiNhanh AS TenNhan
                    FROM dbo.Phieu_DieuChuyen p WITH (UPDLOCK, ROWLOCK)
                    JOIN dbo.ChiNhanh nhan ON nhan.MaChiNhanh = p.MaKhoNhan
                    WHERE p.MaPhieu = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu điều chuyển.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "ChoGui")
                    throw new InvalidOperationException("Phiếu này không còn ở trạng thái chờ gửi.");
                int khoGui = Convert.ToInt32(h.Rows[0]["MaKhoGui"]);
                KiemTraQuyen(gioiHanCN, khoGui, "gửi hàng");
                string ghiChu = $"Chuyển sang {h.Rows[0]["MaCodeNhan"]} - {h.Rows[0]["TenNhan"]}";

                var ct = Db_QLKho.Query(conn, tran, @"
                    SELECT ct.MaNguyenLieu, nl.TenNguyenLieu, d.TenDonVi, ct.SoLuongGui,
                           ISNULL(tk.SoLuongTon, 0) AS Ton
                    FROM dbo.ChiTiet_DieuChuyen ct
                    JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
                    JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
                    LEFT JOIN dbo.TonKho_ChiNhanh tk
                           ON tk.MaNguyenLieu = ct.MaNguyenLieu AND tk.MaChiNhanh = @cn
                    WHERE ct.MaPhieu = @p
                    ORDER BY ct.MaChiTiet",
                    Db_QLKho.P("@p", maPhieu), Db_QLKho.P("@cn", khoGui));
                if (ct.Rows.Count == 0)
                    throw new InvalidOperationException("Phiếu không có dòng chi tiết.");

                // Báo TẤT CẢ dòng thiếu một lần thay vì dừng ở dòng đầu tiên
                var thieu = new StringBuilder();
                foreach (DataRow r in ct.Rows)
                {
                    decimal can = Convert.ToDecimal(r["SoLuongGui"]), ton = Convert.ToDecimal(r["Ton"]);
                    if (can > ton)
                        thieu.AppendLine($"• {r["TenNguyenLieu"]}: cần gửi {can:#,##0.###}, kho chỉ còn {ton:#,##0.###} {r["TenDonVi"]}");
                }
                if (thieu.Length > 0)
                    throw new InvalidOperationException(
                        "Kho gửi không đủ tồn để gửi hàng, phiếu vẫn giữ trạng thái Chờ gửi:\n\n" + thieu);

                foreach (DataRow r in ct.Rows)
                    SoKhoService.BienDong(conn, tran, khoGui, Convert.ToInt32(r["MaNguyenLieu"]),
                        -Convert.ToDecimal(r["SoLuongGui"]), "CHUYEN_DI", "DIEU_CHUYEN", maPhieu, maNhanVien, ghiChu);

                Db_QLKho.Exec(conn, tran, @"
                    UPDATE dbo.Phieu_DieuChuyen
                    SET TrangThai = N'DangChuyen', NgayGui = SYSDATETIME(), NguoiGui = @nv
                    WHERE MaPhieu = @p",
                    Db_QLKho.P("@nv", maNhanVien), Db_QLKho.P("@p", maPhieu));
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // =====================================================================
        //  NHẬN HÀNG (cộng kho nhận theo số thực nhận)
        // =====================================================================

        /// <param name="thucNhan">key = MaChiTiet, value = số lượng thực nhận (0 đến số đã gửi)</param>
        public static void NhanHang(int maPhieu, int maNhanVien, Dictionary<int, decimal> thucNhan, int? gioiHanCN)
        {
            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran, @"
                    SELECT p.MaKhoNhan, p.TrangThai, gui.MaCode AS MaCodeGui, gui.TenChiNhanh AS TenGui
                    FROM dbo.Phieu_DieuChuyen p WITH (UPDLOCK, ROWLOCK)
                    JOIN dbo.ChiNhanh gui ON gui.MaChiNhanh = p.MaKhoGui
                    WHERE p.MaPhieu = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu điều chuyển.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "DangChuyen")
                    throw new InvalidOperationException("Phiếu chưa được gửi hoặc đã được nhận rồi.");
                int khoNhan = Convert.ToInt32(h.Rows[0]["MaKhoNhan"]);
                KiemTraQuyen(gioiHanCN, khoNhan, "nhận hàng");
                string ghiChuGoc = $"Nhận từ {h.Rows[0]["MaCodeGui"]} - {h.Rows[0]["TenGui"]}";

                var ct = Db_QLKho.Query(conn, tran, @"
                    SELECT MaChiTiet, MaNguyenLieu, SoLuongGui
                    FROM dbo.ChiTiet_DieuChuyen WHERE MaPhieu = @p ORDER BY MaChiTiet",
                    Db_QLKho.P("@p", maPhieu));

                foreach (DataRow r in ct.Rows)
                {
                    int maCT = Convert.ToInt32(r["MaChiTiet"]);
                    decimal gui = Convert.ToDecimal(r["SoLuongGui"]);
                    decimal nhan = thucNhan.TryGetValue(maCT, out var v) ? v : gui;
                    if (nhan < 0 || nhan > gui)
                        throw new InvalidOperationException(
                            "Số lượng thực nhận phải từ 0 đến số lượng đã gửi.");

                    if (nhan > 0)
                    {
                        string gc = nhan < gui
                            ? $"{ghiChuGoc}; hao hụt vận chuyển {gui - nhan:#,##0.###}"
                            : ghiChuGoc;
                        SoKhoService.BienDong(conn, tran, khoNhan, Convert.ToInt32(r["MaNguyenLieu"]),
                            nhan, "CHUYEN_DEN", "DIEU_CHUYEN", maPhieu, maNhanVien, gc);
                    }

                    Db_QLKho.Exec(conn, tran,
                        "UPDATE dbo.ChiTiet_DieuChuyen SET SoLuongNhanThucTe = @n WHERE MaChiTiet = @c",
                        Db_QLKho.P("@n", nhan), Db_QLKho.P("@c", maCT));
                }

                Db_QLKho.Exec(conn, tran, @"
                    UPDATE dbo.Phieu_DieuChuyen
                    SET TrangThai = N'DaNhan', NgayNhan = SYSDATETIME(), NguoiNhan = @nv
                    WHERE MaPhieu = @p",
                    Db_QLKho.P("@nv", maNhanVien), Db_QLKho.P("@p", maPhieu));
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        // =====================================================================
        //  HỦY PHIẾU (chỉ khi chưa gửi)
        // =====================================================================

        public static void HuyPhieu(int maPhieu, int? gioiHanCN)
        {
            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran, @"
                    SELECT MaKhoGui, TrangThai FROM dbo.Phieu_DieuChuyen WITH (UPDLOCK, ROWLOCK)
                    WHERE MaPhieu = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu điều chuyển.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "ChoGui")
                    throw new InvalidOperationException("Chỉ hủy được phiếu đang chờ gửi (hàng đã gửi thì không hủy được).");
                KiemTraQuyen(gioiHanCN, Convert.ToInt32(h.Rows[0]["MaKhoGui"]), "hủy phiếu");

                Db_QLKho.Exec(conn, tran,
                    "UPDATE dbo.Phieu_DieuChuyen SET TrangThai = N'DaHuy' WHERE MaPhieu = @p",
                    Db_QLKho.P("@p", maPhieu));
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
        /// Danh sách phiếu. maCN = null (ADMIN) thì xem tất cả; ngược lại chỉ phiếu có chi nhánh đó là kho gửi hoặc kho nhận.
        /// chiCanXuLy: chỉ phiếu chờ gửi mà chi nhánh là kho gửi, hoặc đang chuyển mà chi nhánh là kho nhận.
        /// Phiếu cần xử lý được xếp lên đầu.
        /// </summary>
        public static DataTable DanhSach(int? maCN, string? trangThai, bool chiCanXuLy, string keyword) => Db_QLKho.Query(@"
            SELECT p.MaPhieu,
                   'DC' + RIGHT('00000' + CAST(p.MaPhieu AS VARCHAR(10)), 5) AS SoPhieu,
                   g.TenChiNhanh AS KhoGui,
                   n.TenChiNhanh AS KhoNhan,
                   p.MaKhoGui, p.MaKhoNhan,
                   p.NgayGui, p.NgayNhan, p.TrangThai,
                   nvg.HoTen AS NguoiGui,
                   nvn.HoTen AS NguoiNhan,
                   p.GhiChu
            FROM dbo.Phieu_DieuChuyen p
            JOIN dbo.ChiNhanh g ON g.MaChiNhanh = p.MaKhoGui
            JOIN dbo.ChiNhanh n ON n.MaChiNhanh = p.MaKhoNhan
            LEFT JOIN dbo.NhanVien nvg ON nvg.MaNhanVien = p.NguoiGui
            LEFT JOIN dbo.NhanVien nvn ON nvn.MaNhanVien = p.NguoiNhan
            WHERE (@cn IS NULL OR p.MaKhoGui = @cn OR p.MaKhoNhan = @cn)
              AND (@tt IS NULL OR p.TrangThai = @tt)
              AND (@xl = 0
                   OR (p.TrangThai = N'ChoGui'     AND (@cn IS NULL OR p.MaKhoGui  = @cn))
                   OR (p.TrangThai = N'DangChuyen' AND (@cn IS NULL OR p.MaKhoNhan = @cn)))
              AND (g.TenChiNhanh LIKE @k OR n.TenChiNhanh LIKE @k
                   OR nvg.HoTen LIKE @k OR nvn.HoTen LIKE @k
                   OR 'DC' + RIGHT('00000' + CAST(p.MaPhieu AS VARCHAR(10)), 5) LIKE @k)
            ORDER BY CASE WHEN p.TrangThai IN (N'ChoGui', N'DangChuyen') THEN 0 ELSE 1 END,
                     p.NgayGui DESC, p.MaPhieu DESC",
            Db_QLKho.P("@cn", maCN), Db_QLKho.P("@tt", trangThai), Db_QLKho.P("@xl", chiCanXuLy),
            Db_QLKho.P("@k", "%" + keyword + "%"));

        /// <summary>Chi tiết phiếu kèm tồn HIỆN TẠI của kho gửi (để đối chiếu khi chờ gửi) và hao hụt (khi đã nhận).</summary>
        public static DataTable GetChiTiet(int maPhieu) => Db_QLKho.Query(@"
            SELECT ct.MaChiTiet,
                   nl.TenNguyenLieu,
                   d.TenDonVi,
                   ct.SoLuongGui,
                   ct.SoLuongNhanThucTe,
                   ct.SoLuongGui - ct.SoLuongNhanThucTe AS HaoHut,
                   ISNULL(tk.SoLuongTon, 0) AS TonKhoGui
            FROM dbo.ChiTiet_DieuChuyen ct
            JOIN dbo.Phieu_DieuChuyen p ON p.MaPhieu = ct.MaPhieu
            JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            LEFT JOIN dbo.TonKho_ChiNhanh tk
                   ON tk.MaNguyenLieu = ct.MaNguyenLieu AND tk.MaChiNhanh = p.MaKhoGui
            WHERE ct.MaPhieu = @p
            ORDER BY ct.MaChiTiet",
            Db_QLKho.P("@p", maPhieu));
    }
}