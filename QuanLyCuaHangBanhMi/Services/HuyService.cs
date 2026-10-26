using System.Data;
using System.Text;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class HuyService
    {
        // =====================================================================
        //  LẬP PHIẾU (chưa ảnh hưởng kho)
        // =====================================================================

        /// <summary>
        /// Tạo phiếu hủy ở trạng thái Draft. ct cần các cột: MaNguyenLieu, SoLuong, GhiChu.
        /// Trả về mã phiếu.
        /// </summary>
        public static int TaoPhieu(int maCN, int nguoiLap, string lyDo, DataTable ct)
        {
            lyDo = (lyDo ?? "").Trim();
            if (lyDo.Length == 0)
                throw new InvalidOperationException("Hãy nhập lý do hủy.");
            if (lyDo.Length > 255)
                throw new InvalidOperationException("Lý do tối đa 255 ký tự.");
            if (ct.Rows.Count == 0)
                throw new InvalidOperationException("Phiếu hủy chưa có dòng nào.");

            var ids = new HashSet<int>();
            foreach (DataRow r in ct.Rows)
            {
                if (r["MaNguyenLieu"] == DBNull.Value)
                    throw new InvalidOperationException("Có dòng chưa chọn nguyên liệu.");
                if (!ids.Add(Convert.ToInt32(r["MaNguyenLieu"])))
                    throw new InvalidOperationException("Một nguyên liệu chỉ được xuất hiện một lần trong phiếu.");
                if (Convert.ToDecimal(r["SoLuong"]) <= 0)
                    throw new InvalidOperationException("Số lượng hủy phải lớn hơn 0.");
                if (Convert.ToString(r["GhiChu"])?.Length > 255)
                    throw new InvalidOperationException("Ghi chú của mỗi dòng tối đa 255 ký tự.");
            }

            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                int ma = Convert.ToInt32(Db_QLKho.Scalar(conn, tran, @"
                    INSERT dbo.Phieu_HuyHaoHut (MaChiNhanh, NguoiLap, NgayLap, LyDo)
                    OUTPUT INSERTED.MaPhieu
                    VALUES (@cn, @nv, SYSDATETIME(), @ly)",
                    Db_QLKho.P("@cn", maCN), Db_QLKho.P("@nv", nguoiLap), Db_QLKho.P("@ly", lyDo)));

                foreach (DataRow r in ct.Rows)
                {
                    string? gc = Convert.ToString(r["GhiChu"]);
                    Db_QLKho.Exec(conn, tran, @"
                        INSERT dbo.ChiTiet_HuyHaoHut (MaPhieu, MaNguyenLieu, SoLuong, GhiChu)
                        VALUES (@p, @nl, @sl, @gc)",
                        Db_QLKho.P("@p", ma), Db_QLKho.P("@nl", Convert.ToInt32(r["MaNguyenLieu"])),
                        Db_QLKho.P("@sl", Convert.ToDecimal(r["SoLuong"])),
                        Db_QLKho.P("@gc", string.IsNullOrWhiteSpace(gc) ? null : gc.Trim()));
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

        // =====================================================================
        //  DUYỆT (trừ kho) / TỪ CHỐI
        // =====================================================================

        public static void Duyet(int maPhieu, int maNguoiDuyet)
        {
            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran,
                    @"SELECT MaChiNhanh, TrangThai FROM dbo.Phieu_HuyHaoHut WITH (UPDLOCK, ROWLOCK)
                      WHERE MaPhieu = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu hủy.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "Draft")
                    throw new InvalidOperationException("Phiếu này đã được xử lý nên không thể duyệt lại.");
                int maCN = Convert.ToInt32(h.Rows[0]["MaChiNhanh"]);

                var ct = Db_QLKho.Query(conn, tran, @"
                    SELECT ct.MaNguyenLieu, nl.TenNguyenLieu, d.TenDonVi, ct.SoLuong, ct.GhiChu,
                           ISNULL(tk.SoLuongTon, 0) AS Ton
                    FROM dbo.ChiTiet_HuyHaoHut ct
                    JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
                    JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
                    LEFT JOIN dbo.TonKho_ChiNhanh tk
                           ON tk.MaNguyenLieu = ct.MaNguyenLieu AND tk.MaChiNhanh = @cn
                    WHERE ct.MaPhieu = @p
                    ORDER BY ct.MaChiTiet",
                    Db_QLKho.P("@p", maPhieu), Db_QLKho.P("@cn", maCN));
                if (ct.Rows.Count == 0)
                    throw new InvalidOperationException("Phiếu không có dòng chi tiết.");

                // Báo TẤT CẢ dòng thiếu một lần thay vì dừng ở dòng đầu tiên
                var thieu = new StringBuilder();
                foreach (DataRow r in ct.Rows)
                {
                    decimal can = Convert.ToDecimal(r["SoLuong"]), ton = Convert.ToDecimal(r["Ton"]);
                    if (can > ton)
                        thieu.AppendLine($"• {r["TenNguyenLieu"]}: cần {can:#,##0.###}, chỉ còn {ton:#,##0.###} {r["TenDonVi"]}");
                }
                if (thieu.Length > 0)
                    throw new InvalidOperationException(
                        "Không đủ tồn kho để duyệt phiếu, phiếu vẫn giữ trạng thái Nháp:\n\n" + thieu);

                foreach (DataRow r in ct.Rows)
                {
                    string? gc = Convert.ToString(r["GhiChu"]);
                    SoKhoService.BienDong(conn, tran, maCN, Convert.ToInt32(r["MaNguyenLieu"]),
                        -Convert.ToDecimal(r["SoLuong"]), "HUY", "HUY", maPhieu, maNguoiDuyet,
                        string.IsNullOrWhiteSpace(gc) ? null : gc);
                }

                Db_QLKho.Exec(conn, tran,
                    "UPDATE dbo.Phieu_HuyHaoHut SET TrangThai = N'DaDuyet' WHERE MaPhieu = @p",
                    Db_QLKho.P("@p", maPhieu));
                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public static void TuChoi(int maPhieu)
        {
            using var conn = Db_QLKho.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                var h = Db_QLKho.Query(conn, tran,
                    @"SELECT TrangThai FROM dbo.Phieu_HuyHaoHut WITH (UPDLOCK, ROWLOCK)
                      WHERE MaPhieu = @p", Db_QLKho.P("@p", maPhieu));
                if (h.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy phiếu hủy.");
                if (Convert.ToString(h.Rows[0]["TrangThai"]) != "Draft")
                    throw new InvalidOperationException("Chỉ từ chối được phiếu đang ở trạng thái Nháp.");

                Db_QLKho.Exec(conn, tran,
                    "UPDATE dbo.Phieu_HuyHaoHut SET TrangThai = N'TuChoi' WHERE MaPhieu = @p",
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

        /// <summary>Danh sách phiếu một chi nhánh; phiếu chờ duyệt (Draft) được xếp lên đầu.</summary>
        public static DataTable DanhSach(int maCN, string? trangThai, string keyword) => Db_QLKho.Query(@"
            SELECT p.MaPhieu,
                   'HH' + RIGHT('00000' + CAST(p.MaPhieu AS VARCHAR(10)), 5) AS SoPhieu,
                   p.NgayLap,
                   nv.HoTen AS NguoiLap,
                   p.LyDo,
                   (SELECT COUNT(*) FROM dbo.ChiTiet_HuyHaoHut c WHERE c.MaPhieu = p.MaPhieu) AS SoDong,
                   p.TrangThai
            FROM dbo.Phieu_HuyHaoHut p
            JOIN dbo.NhanVien nv ON nv.MaNhanVien = p.NguoiLap
            WHERE p.MaChiNhanh = @cn
              AND (@tt IS NULL OR p.TrangThai = @tt)
              AND (p.LyDo LIKE @k OR nv.HoTen LIKE @k
                   OR 'HH' + RIGHT('00000' + CAST(p.MaPhieu AS VARCHAR(10)), 5) LIKE @k)
            ORDER BY CASE WHEN p.TrangThai = N'Draft' THEN 0 ELSE 1 END, p.NgayLap DESC, p.MaPhieu DESC",
            Db_QLKho.P("@cn", maCN), Db_QLKho.P("@tt", trangThai), Db_QLKho.P("@k", "%" + keyword + "%"));

        /// <summary>Chi tiết phiếu kèm tồn HIỆN TẠI để người duyệt đối chiếu.</summary>
        public static DataTable GetChiTiet(int maPhieu) => Db_QLKho.Query(@"
            SELECT nl.TenNguyenLieu, d.TenDonVi, ct.SoLuong,
                   ISNULL(tk.SoLuongTon, 0) AS TonHienTai, ct.GhiChu
            FROM dbo.ChiTiet_HuyHaoHut ct
            JOIN dbo.Phieu_HuyHaoHut p ON p.MaPhieu = ct.MaPhieu
            JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
            JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
            LEFT JOIN dbo.TonKho_ChiNhanh tk
                   ON tk.MaNguyenLieu = ct.MaNguyenLieu AND tk.MaChiNhanh = p.MaChiNhanh
            WHERE ct.MaPhieu = @p
            ORDER BY ct.MaChiTiet",
            Db_QLKho.P("@p", maPhieu));
    }
}