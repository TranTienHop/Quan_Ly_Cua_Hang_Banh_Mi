using System;
using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.Data.SqlClient;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    /// <summary>Lớp Data — POS: chi nhánh, ca, menu, công thức, lưu hóa đơn (transaction).</summary>
    public static class OrderRepository
    {
        public static DataTable GetBranchByEmployee(int maNhanVien)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT cn.MaChiNhanh, cn.MaCode, cn.TenChiNhanh
                FROM dbo.NhanVien nv
                JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = nv.MaChiNhanh
                WHERE nv.MaNhanVien = @MaNhanVien",
                new[] { DatabaseHelper.Param("@MaNhanVien", SqlDbType.Int, maNhanVien) });
        }

        public static DataTable GetOpenShift(int maChiNhanh)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT TOP (1) ca.MaCa, ca.GioMo, nv.HoTen
                FROM dbo.NhatKy_CaLam ca
                JOIN dbo.NhanVien nv ON nv.MaNhanVien = ca.MaNhanVien
                WHERE ca.MaChiNhanh = @MaChiNhanh AND ca.TrangThai = N'DangMo'
                ORDER BY ca.GioMo DESC",
                new[] { DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, maChiNhanh) });
        }

        public static DataTable GetPosMenu(int maChiNhanh)
        {
            return DatabaseHelper.ExecuteQuery(@"
                WITH TP AS (
                    SELECT ct.MaSanPham, MIN(FLOOR(ISNULL(tk.SoLuongTon, 0) / ct.SoLuongTrenMotPhan)) AS CoTheBan
                    FROM dbo.CongThuc ct
                    LEFT JOIN dbo.TonKho_ChiNhanh tk ON tk.MaNguyenLieu = ct.MaNguyenLieu AND tk.MaChiNhanh = @MaChiNhanh
                    GROUP BY ct.MaSanPham
                )
                SELECT sp.MaSanPham, sp.MaSKU, sp.TenSanPham, sp.LoaiSanPham, sp.MaDanhMuc, dm.TenDanhMuc, spcn.GiaBan,
                       CASE
                           WHEN sp.LoaiSanPham = N'KhongTruKho' THEN NULL
                           WHEN sp.LoaiSanPham = N'Combo' THEN
                               (SELECT MIN(FLOOR(ISNULL(tp2.CoTheBan, 0) / cb.SoLuong))
                                FROM dbo.Combo_ChiTiet cb
                                LEFT JOIN TP tp2 ON tp2.MaSanPham = cb.MaSanPham
                                WHERE cb.MaCombo = sp.MaSanPham)
                           WHEN tp.CoTheBan IS NULL AND sp.BatBuocCongThuc = 1 THEN 0
                           ELSE tp.CoTheBan
                       END AS CoTheBan
                FROM dbo.SanPham sp
                JOIN dbo.SanPham_ChiNhanh spcn ON spcn.MaSanPham = sp.MaSanPham
                                               AND spcn.MaChiNhanh = @MaChiNhanh AND spcn.ConBan = 1
                JOIN dbo.DanhMuc dm ON dm.MaDanhMuc = sp.MaDanhMuc AND dm.DangHoatDong = 1
                LEFT JOIN TP tp ON tp.MaSanPham = sp.MaSanPham
                WHERE sp.DangHoatDong = 1
                ORDER BY sp.MaDanhMuc, sp.MaSKU, sp.MaSanPham",
                new[] { DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, maChiNhanh) });
        }

        public static DataTable GetRecipe(int maSanPham)
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT nl.TenNguyenLieu, ct.SoLuongTrenMotPhan
                FROM dbo.CongThuc ct
                JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
                WHERE ct.MaSanPham = @MaSanPham
                ORDER BY nl.TenNguyenLieu",
                new[] { DatabaseHelper.Param("@MaSanPham", SqlDbType.Int, maSanPham) });
        }

        public static CheckoutResult SaveCheckout(CheckoutRequest req, IFormatProvider numberFormat)
        {
            using var conn = DatabaseHelper.GetConnection();
            conn.Open();
            using var tran = conn.BeginTransaction();
            try
            {
                DateTime now = Convert.ToDateTime(DatabaseHelper.ExecuteScalar(conn, tran,
                    "SELECT CAST(SYSDATETIME() AS DATETIME2(0))"));

                string prefix = $"{req.MaCodeChiNhanh}-{now:yyMMdd}-";
                object? lastCode = DatabaseHelper.ExecuteScalar(conn, tran, @"
                    SELECT MAX(MaDonLocal) FROM dbo.HoaDon WITH (UPDLOCK, HOLDLOCK)
                    WHERE MaChiNhanh = @MaChiNhanh AND MaDonLocal LIKE @Prefix", new[]
                {
                    DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, req.MaChiNhanh),
                    DatabaseHelper.Param("@Prefix", SqlDbType.NVarChar, prefix + "%", 50)
                });
                int nextNo = 1;
                if (lastCode is string s && int.TryParse(s.Substring(prefix.Length), out int lastNo))
                    nextNo = lastNo + 1;
                string maDon = prefix + nextNo.ToString("000");

                object? maCa = DatabaseHelper.ExecuteScalar(conn, tran, @"
                    SELECT TOP (1) MaCa FROM dbo.NhatKy_CaLam
                    WHERE MaChiNhanh = @MaChiNhanh AND TrangThai = N'DangMo'
                    ORDER BY GioMo DESC",
                    new[] { DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, req.MaChiNhanh) });

                if (req.MaKhuyenMai.HasValue)
                {
                    object? valid = DatabaseHelper.ExecuteScalar(conn, tran, @"
                        SELECT COUNT(*) FROM dbo.KhuyenMai
                        WHERE MaKhuyenMai = @MaKhuyenMai AND DangHoatDong = 1
                          AND (MaChiNhanh IS NULL OR MaChiNhanh = @MaChiNhanh)
                          AND @Now BETWEEN NgayBatDau AND NgayKetThuc", new[]
                    {
                        DatabaseHelper.Param("@MaKhuyenMai", SqlDbType.Int, req.MaKhuyenMai.Value),
                        DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, req.MaChiNhanh),
                        DatabaseHelper.Param("@Now", SqlDbType.DateTime2, now)
                    });
                    if (Convert.ToInt32(valid) == 0)
                        throw new InvalidOperationException(
                            "Chương trình khuyến mãi đã hết hạn hoặc bị tắt. Vui lòng chọn lại.");
                }

                int maHoaDon = Convert.ToInt32(DatabaseHelper.ExecuteScalar(conn, tran, @"
                    INSERT INTO dbo.HoaDon (MaDonLocal, MaChiNhanh, MaThuNgan, MaKhachHang, MaCa, MaKhuyenMai, NguonDon, LoaiDon,
                        TongTienHang, TienGiam, PhiShip, TongThanhToan, DiemCong, DiemTru,
                        TrangThaiDon, TrangThaiThanhToan, NgayTao, NgayThanhToan)
                    OUTPUT INSERTED.MaHoaDon
                    VALUES (@MaDonLocal, @MaChiNhanh, @MaThuNgan, @MaKhachHang, @MaCa, @MaKhuyenMai, @NguonDon, @LoaiDon,
                        @TongTienHang, @TienGiam, @PhiShip, @TongThanhToan, @DiemCong, @DiemTru,
                        N'HoanThanh', N'DaTT', @Now, @Now)", new[]
                {
                    DatabaseHelper.Param("@MaDonLocal", SqlDbType.NVarChar, maDon, 50),
                    DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, req.MaChiNhanh),
                    DatabaseHelper.Param("@MaThuNgan", SqlDbType.Int, req.MaThuNgan),
                    DatabaseHelper.Param("@MaKhachHang", SqlDbType.Int, req.MaKhachHang),
                    DatabaseHelper.Param("@MaCa", SqlDbType.Int, maCa == null || maCa == DBNull.Value ? null : Convert.ToInt32(maCa)),
                    DatabaseHelper.Param("@MaKhuyenMai", SqlDbType.Int, req.MaKhuyenMai),
                    DatabaseHelper.Param("@NguonDon", SqlDbType.NVarChar, req.NguonDon, 30),
                    DatabaseHelper.Param("@LoaiDon", SqlDbType.NVarChar, req.LoaiDon, 20),
                    DatabaseHelper.Param("@TongTienHang", SqlDbType.Decimal, req.TienHang),
                    DatabaseHelper.Param("@TienGiam", SqlDbType.Decimal, req.TienGiam),
                    DatabaseHelper.Param("@PhiShip", SqlDbType.Decimal, req.PhiShip),
                    DatabaseHelper.Param("@TongThanhToan", SqlDbType.Decimal, req.TongThanhToan),
                    DatabaseHelper.Param("@DiemCong", SqlDbType.Int, req.DiemCong),
                    DatabaseHelper.Param("@DiemTru", SqlDbType.Int, req.DiemTru),
                    DatabaseHelper.Param("@Now", SqlDbType.DateTime2, now)
                }));

                foreach (DataRow row in req.CartTable.Rows)
                {
                    decimal giamDong = row.Table.Columns.Contains("GiamDong") && !row.IsNull("GiamDong")
                        ? row.Field<decimal>("GiamDong") : 0m;
                    DatabaseHelper.ExecuteNonQuery(conn, tran, @"
                        INSERT INTO dbo.ChiTiet_HoaDon (MaHoaDon, MaSanPham, TenSanPhamChup, SoLuong, DonGia, GiamDong, ThanhTien, GhiChu)
                        VALUES (@MaHoaDon, @MaSanPham, @TenSanPham, @SoLuong, @DonGia, @GiamDong, @ThanhTien, @GhiChu)", new[]
                    {
                        DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon),
                        DatabaseHelper.Param("@MaSanPham", SqlDbType.Int, row.Field<int>("MaSanPham")),
                        DatabaseHelper.Param("@TenSanPham", SqlDbType.NVarChar, row.Field<string>("TenSanPham"), 200),
                        DatabaseHelper.Param("@SoLuong", SqlDbType.Decimal, (decimal)row.Field<int>("SoLuong")),
                        DatabaseHelper.Param("@DonGia", SqlDbType.Decimal, row.Field<decimal>("DonGia")),
                        DatabaseHelper.Param("@GiamDong", SqlDbType.Decimal, giamDong),
                        DatabaseHelper.Param("@ThanhTien", SqlDbType.Decimal, row.Field<decimal>("ThanhTien")),
                        DatabaseHelper.Param("@GhiChu", SqlDbType.NVarChar,
                            string.IsNullOrWhiteSpace(row.Field<string>("GhiChu"))
                                ? null
                                : row.Field<string>("GhiChu")!.Trim(), 255)
                    });
                }

                DeductStock(conn, tran, maHoaDon, maDon, now, req.MaChiNhanh, req.MaThuNgan, numberFormat);

                if (req.TongThanhToan > 0)
                {
                    DatabaseHelper.ExecuteNonQuery(conn, tran, @"
                        INSERT INTO dbo.ThanhToan_HoaDon (MaHoaDon, HinhThuc, SoTien, ThoiGian, MaThamChieu, NguoiThu)
                        VALUES (@MaHoaDon, @HinhThuc, @SoTien, @Now, @MaThamChieu, @NguoiThu)", new[]
                    {
                        DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon),
                        DatabaseHelper.Param("@HinhThuc", SqlDbType.NVarChar, req.HinhThucThanhToan, 30),
                        DatabaseHelper.Param("@SoTien", SqlDbType.Decimal, req.TongThanhToan),
                        DatabaseHelper.Param("@Now", SqlDbType.DateTime2, now),
                        DatabaseHelper.Param("@MaThamChieu", SqlDbType.NVarChar,
                            req.MaThamChieu.Length == 0 ? null : req.MaThamChieu, 100),
                        DatabaseHelper.Param("@NguoiThu", SqlDbType.Int, req.MaThuNgan)
                    });
                }

                if (req.LoaiDon == "GiaoHang")
                {
                    string loaiShip = req.NguonDon switch
                    {
                        "Grab" => "Grab",
                        "ShopeeFood" => "Shopee",
                        _ => "NoiBo"
                    };
                    DatabaseHelper.ExecuteNonQuery(conn, tran, @"
                        INSERT INTO dbo.NhatKy_GiaoHang (MaHoaDon, LoaiShip, DiaChi)
                        VALUES (@MaHoaDon, @LoaiShip, @DiaChi)", new[]
                    {
                        DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon),
                        DatabaseHelper.Param("@LoaiShip", SqlDbType.NVarChar, loaiShip, 30),
                        DatabaseHelper.Param("@DiaChi", SqlDbType.NVarChar, req.DiaChiGiao, 255)
                    });
                }

                if (req.MaKhachHang.HasValue)
                    UpdateLoyaltyPoints(conn, tran, maHoaDon, now, req.MaKhachHang.Value, req.DiemCong, req.DiemTru);

                tran.Commit();
                return new CheckoutResult { MaHoaDon = maHoaDon, MaDon = maDon };
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        private static void DeductStock(
            SqlConnection conn, SqlTransaction tran,
            int maHoaDon, string maDon, DateTime now,
            int maChiNhanh, int maNhanVien, IFormatProvider numberFormat)
        {
            object? missingRecipe = DatabaseHelper.ExecuteScalar(conn, tran, @"
                SELECT TOP (1) sp.TenSanPham
                FROM dbo.ChiTiet_HoaDon d
                LEFT JOIN dbo.Combo_ChiTiet cb ON cb.MaCombo = d.MaSanPham
                JOIN dbo.SanPham sp ON sp.MaSanPham = ISNULL(cb.MaSanPham, d.MaSanPham)
                WHERE d.MaHoaDon = @MaHoaDon AND sp.LoaiSanPham = N'ThanhPham' AND sp.BatBuocCongThuc = 1
                  AND NOT EXISTS (SELECT 1 FROM dbo.CongThuc ct WHERE ct.MaSanPham = sp.MaSanPham)",
                new[] { DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon) });
            if (missingRecipe is string tenMon)
                throw new InvalidOperationException(
                    $"Món \"{tenMon}\" bắt buộc có công thức nhưng chưa khai báo, không thể trừ kho.");

            var shortages = DatabaseHelper.ExecuteQuery(conn, tran, @"
                DECLARE @Can TABLE (MaNguyenLieu INT PRIMARY KEY, SoLuong DECIMAL(18,3) NOT NULL);
                DECLARE @Ra  TABLE (MaNguyenLieu INT PRIMARY KEY, SoLuong DECIMAL(18,3) NOT NULL, TonSau DECIMAL(18,3) NOT NULL);

                INSERT INTO @Can (MaNguyenLieu, SoLuong)
                SELECT ct.MaNguyenLieu, SUM(d.SoLuong * ISNULL(cb.SoLuong, 1) * ct.SoLuongTrenMotPhan)
                FROM dbo.ChiTiet_HoaDon d
                LEFT JOIN dbo.Combo_ChiTiet cb ON cb.MaCombo = d.MaSanPham
                JOIN dbo.SanPham sp  ON sp.MaSanPham = ISNULL(cb.MaSanPham, d.MaSanPham)
                JOIN dbo.CongThuc ct ON ct.MaSanPham = sp.MaSanPham
                WHERE d.MaHoaDon = @MaHoaDon AND sp.LoaiSanPham = N'ThanhPham'
                GROUP BY ct.MaNguyenLieu;

                IF EXISTS (SELECT 1 FROM @Can c
                           LEFT JOIN dbo.TonKho_ChiNhanh tk WITH (UPDLOCK, HOLDLOCK)
                                  ON tk.MaChiNhanh = @MaChiNhanh AND tk.MaNguyenLieu = c.MaNguyenLieu
                           WHERE ISNULL(tk.SoLuongTon, 0) < c.SoLuong)
                BEGIN
                    SELECT nl.TenNguyenLieu, dv.TenDonVi, c.SoLuong AS CanDung, ISNULL(tk.SoLuongTon, 0) AS TonHienTai
                    FROM @Can c
                    JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = c.MaNguyenLieu
                    JOIN dbo.DonVi dv      ON dv.MaDonVi = nl.MaDonViGoc
                    LEFT JOIN dbo.TonKho_ChiNhanh tk ON tk.MaChiNhanh = @MaChiNhanh AND tk.MaNguyenLieu = c.MaNguyenLieu
                    WHERE ISNULL(tk.SoLuongTon, 0) < c.SoLuong
                    ORDER BY nl.TenNguyenLieu;
                    RETURN;
                END

                UPDATE tk
                SET tk.SoLuongTon = tk.SoLuongTon - c.SoLuong, tk.NgayCapNhat = @Now
                OUTPUT INSERTED.MaNguyenLieu, c.SoLuong, INSERTED.SoLuongTon INTO @Ra (MaNguyenLieu, SoLuong, TonSau)
                FROM dbo.TonKho_ChiNhanh tk
                JOIN @Can c ON c.MaNguyenLieu = tk.MaNguyenLieu
                WHERE tk.MaChiNhanh = @MaChiNhanh;

                INSERT INTO dbo.NhatKy_Kho (MaChiNhanh, MaNguyenLieu, LoaiGiaoDich, SoLuongThayDoi, TonSau,
                                            MaNhanVien, NgayTao, LoaiThamChieu, MaThamChieu, GhiChu)
                SELECT @MaChiNhanh, r.MaNguyenLieu, N'XUAT_BAN', -r.SoLuong, r.TonSau,
                       @MaNhanVien, @Now, N'HOADON', @MaHoaDon, @MaDonLocal
                FROM @Ra r;", new[]
            {
                DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon),
                DatabaseHelper.Param("@MaChiNhanh", SqlDbType.Int, maChiNhanh),
                DatabaseHelper.Param("@MaNhanVien", SqlDbType.Int, maNhanVien),
                DatabaseHelper.Param("@Now", SqlDbType.DateTime2, now),
                DatabaseHelper.Param("@MaDonLocal", SqlDbType.NVarChar, maDon, 255)
            });

            if (shortages.Rows.Count > 0)
            {
                var sb = new StringBuilder("Không đủ nguyên liệu tại chi nhánh để làm đơn này:\n\n");
                foreach (DataRow r in shortages.Rows)
                {
                    sb.AppendLine(
                        $"• {r["TenNguyenLieu"]}: cần {Convert.ToDecimal(r["CanDung"]).ToString("#,##0.###", numberFormat)} {r["TenDonVi"]}, "
                        + $"còn {Convert.ToDecimal(r["TonHienTai"]).ToString("#,##0.###", numberFormat)} {r["TenDonVi"]}");
                }
                sb.Append("\nHóa đơn chưa được lưu. Vui lòng bớt món hoặc báo quản lý nhập thêm hàng.");
                throw new InvalidOperationException(sb.ToString());
            }
        }

        private static void UpdateLoyaltyPoints(
            SqlConnection conn, SqlTransaction tran,
            int maHoaDon, DateTime now, int maKhachHang, int diemCong, int diemTru)
        {
            object? current = DatabaseHelper.ExecuteScalar(conn, tran,
                "SELECT DiemHienTai FROM dbo.KhachHang WITH (UPDLOCK) WHERE MaKhachHang = @MaKhachHang AND DangHoatDong = 1",
                new[] { DatabaseHelper.Param("@MaKhachHang", SqlDbType.Int, maKhachHang) });
            if (current == null || current == DBNull.Value)
                throw new InvalidOperationException("Thẻ khách hàng đã ngưng sử dụng.");

            int diem = Convert.ToInt32(current);
            if (diemTru > diem)
                throw new InvalidOperationException(
                    $"Khách chỉ còn {diem:N0} điểm, không đủ để đổi {diemTru:N0} điểm.");

            void InsertPointTransaction(int soDiem, string lyDo)
            {
                diem += soDiem;
                DatabaseHelper.ExecuteNonQuery(conn, tran, @"
                    INSERT INTO dbo.GiaoDich_TichDiem (MaKhachHang, MaHoaDon, SoDiemThayDoi, DiemSauGD, LyDo, NgayTao)
                    VALUES (@MaKhachHang, @MaHoaDon, @SoDiem, @DiemSau, @LyDo, @Now)", new[]
                {
                    DatabaseHelper.Param("@MaKhachHang", SqlDbType.Int, maKhachHang),
                    DatabaseHelper.Param("@MaHoaDon", SqlDbType.Int, maHoaDon),
                    DatabaseHelper.Param("@SoDiem", SqlDbType.Int, soDiem),
                    DatabaseHelper.Param("@DiemSau", SqlDbType.Int, diem),
                    DatabaseHelper.Param("@LyDo", SqlDbType.NVarChar, lyDo, 30),
                    DatabaseHelper.Param("@Now", SqlDbType.DateTime2, now)
                });
            }

            if (diemTru > 0) InsertPointTransaction(-diemTru, "Tieu");
            if (diemCong > 0) InsertPointTransaction(diemCong, "Tich");
            if (diemTru == 0 && diemCong == 0) return;

            DatabaseHelper.ExecuteNonQuery(conn, tran, @"
                UPDATE dbo.KhachHang
                SET DiemHienTai = @Diem,
                    MaHang = ISNULL((SELECT TOP (1) h.MaHang FROM dbo.CauHinh_HangThanhVien h
                                     WHERE h.DiemToiThieu <= @Diem ORDER BY h.DiemToiThieu DESC), MaHang)
                WHERE MaKhachHang = @MaKhachHang", new[]
            {
                DatabaseHelper.Param("@Diem", SqlDbType.Int, diem),
                DatabaseHelper.Param("@MaKhachHang", SqlDbType.Int, maKhachHang)
            });
        }
    }
}
