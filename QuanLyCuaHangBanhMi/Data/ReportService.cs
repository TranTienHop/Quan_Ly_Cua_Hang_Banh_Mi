using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Data
{
    /// <summary>
    /// Service xử lý toàn bộ truy vấn dữ liệu cho Module Báo Cáo & Thống Kê (Admin)
    /// Hỗ trợ CSDL QuanLyBanhMi (chính) và tương thích dự phòng với BanhMiShopDB.
    /// </summary>
    public static class ReportService
    {
        private static bool? _isQuanLyBanhMi = null;

        /// <summary>
        /// Kiểm tra CSDL hiện tại có phải là QuanLyBanhMi (chứa bảng dbo.HoaDon) hay không
        /// </summary>
        public static bool IsQuanLyBanhMi()
        {
            if (_isQuanLyBanhMi.HasValue) return _isQuanLyBanhMi.Value;
            try
            {
                object? result = DatabaseHelper.ExecuteScalar("SELECT OBJECT_ID('dbo.HoaDon', 'U')");
                _isQuanLyBanhMi = (result != null && result != DBNull.Value);
            }
            catch
            {
                _isQuanLyBanhMi = false;
            }
            return _isQuanLyBanhMi.Value;
        }

        #region ====================== 1. DANH MỤC & CHI NHÁNH ======================

        public static DataTable GetBranches()
        {
            var dt = new DataTable();
            dt.Columns.Add("MaChiNhanh", typeof(int));
            dt.Columns.Add("TenChiNhanh", typeof(string));

            // Thêm mục "Tất cả chi nhánh"
            dt.Rows.Add(0, "-- Tất cả chi nhánh --");

            try
            {
                if (IsQuanLyBanhMi())
                {
                    string sql = "SELECT MaChiNhanh, TenChiNhanh FROM dbo.ChiNhanh ORDER BY MaChiNhanh";
                    DataTable dbBranches = DatabaseHelper.ExecuteQuery(sql);
                    foreach (DataRow r in dbBranches.Rows)
                    {
                        dt.Rows.Add(Convert.ToInt32(r["MaChiNhanh"]), r["TenChiNhanh"].ToString());
                    }
                }
            }
            catch { }
            return dt;
        }

        public static DataTable GetCategories()
        {
            var dt = new DataTable();
            dt.Columns.Add("MaDanhMuc", typeof(int));
            dt.Columns.Add("TenDanhMuc", typeof(string));

            dt.Rows.Add(0, "-- Tất cả danh mục --");

            try
            {
                string sql = IsQuanLyBanhMi()
                    ? "SELECT MaDanhMuc, TenDanhMuc FROM dbo.DanhMuc WHERE DangHoatDong = 1 ORDER BY MaDanhMuc"
                    : "SELECT CategoryID AS MaDanhMuc, CategoryName AS TenDanhMuc FROM dbo.Categories WHERE IsActive = 1 ORDER BY CategoryID";

                DataTable dbCats = DatabaseHelper.ExecuteQuery(sql);
                foreach (DataRow r in dbCats.Rows)
                {
                    dt.Rows.Add(Convert.ToInt32(r["MaDanhMuc"]), r["TenDanhMuc"].ToString());
                }
            }
            catch { }
            return dt;
        }

        #endregion

        #region ====================== 2. BÁO CÁO DOANH THU ======================

        /// <summary>
        /// Lấy báo cáo doanh thu theo ngày hoặc chi tiết từng đơn hàng
        /// </summary>
        public static DataTable GetRevenueReport(DateTime fromDate, DateTime toDate, int branchId, bool detailed)
        {
            var prms = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date.AddDays(1).AddTicks(-1))
            };

            string branchFilter = "";
            if (branchId > 0)
            {
                branchFilter = " AND hd.MaChiNhanh = @MaChiNhanh";
                prms.Add(new SqlParameter("@MaChiNhanh", branchId));
            }

            if (IsQuanLyBanhMi())
            {
                if (!detailed)
                {
                    // Tổng hợp theo ngày
                    string sql = $@"
                        SELECT 
                            FORMAT(CAST(hd.NgayTao AS DATE), 'dd/MM/yyyy') AS [Ngày Bán],
                            cn.TenChiNhanh AS [Chi Nhánh],
                            COUNT(hd.MaHoaDon) AS [Số Đơn Hàng],
                            SUM(hd.TongTienHang) AS [Tổng Tiền Hàng (VNĐ)],
                            SUM(hd.TienGiam) AS [Giảm Giá (VNĐ)],
                            SUM(hd.PhiShip) AS [Phí Ship (VNĐ)],
                            SUM(hd.TongThanhToan) AS [Doanh Thu Thực (VNĐ)]
                        FROM dbo.HoaDon hd
                        JOIN dbo.ChiNhanh cn ON hd.MaChiNhanh = cn.MaChiNhanh
                        WHERE hd.TrangThaiThanhToan = 'DaTT'
                          AND hd.NgayTao >= @FromDate AND hd.NgayTao <= @ToDate
                          {branchFilter}
                        GROUP BY CAST(hd.NgayTao AS DATE), cn.TenChiNhanh
                        ORDER BY CAST(hd.NgayTao AS DATE) DESC, cn.TenChiNhanh ASC";

                    return DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
                }
                else
                {
                    // Chi tiết từng hóa đơn
                    string sql = $@"
                        SELECT 
                            hd.MaHoaDon AS [Mã HĐ],
                            ISNULL(hd.MaDonLocal, CONVERT(NVARCHAR(20), hd.MaHoaDon)) AS [Mã Đơn],
                            cn.TenChiNhanh AS [Chi Nhánh],
                            nv.HoTen AS [Thu Ngân],
                            FORMAT(hd.NgayTao, 'dd/MM/yyyy HH:mm') AS [Thời Gian],
                            hd.NguonDon AS [Kênh Bán],
                            hd.LoaiDon AS [Loại Đơn],
                            hd.TongTienHang AS [Tiền Hàng (VNĐ)],
                            hd.TienGiam AS [Giảm Giá (VNĐ)],
                            hd.PhiShip AS [Phí Ship (VNĐ)],
                            hd.TongThanhToan AS [Thực Thu (VNĐ)],
                            hd.TrangThaiThanhToan AS [Trạng Thái TT]
                        FROM dbo.HoaDon hd
                        JOIN dbo.ChiNhanh cn ON hd.MaChiNhanh = cn.MaChiNhanh
                        JOIN dbo.NhanVien nv ON hd.MaThuNgan = nv.MaNhanVien
                        WHERE hd.TrangThaiThanhToan = 'DaTT'
                          AND hd.NgayTao >= @FromDate AND hd.NgayTao <= @ToDate
                          {branchFilter}
                        ORDER BY hd.NgayTao DESC";

                    return DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
                }
            }
            else
            {
                // Fallback BanhMiShopDB
                string sql = @"
                    SELECT 
                        FORMAT(OrderDate, 'dd/MM/yyyy') AS [Ngày Bán],
                        'Tiệm Bánh Mì' AS [Chi Nhánh],
                        COUNT(OrderID) AS [Số Đơn Hàng],
                        SUM(TotalAmount) AS [Tổng Tiền Hàng (VNĐ)],
                        SUM(DiscountAmount) AS [Giảm Giá (VNĐ)],
                        0 AS [Phí Ship (VNĐ)],
                        SUM(FinalAmount) AS [Doanh Thu Thực (VNĐ)]
                    FROM dbo.Orders
                    WHERE Status = 'COMPLETED'
                      AND OrderDate >= @FromDate AND OrderDate <= @ToDate
                    GROUP BY FORMAT(OrderDate, 'dd/MM/yyyy')
                    ORDER BY [Ngày Bán] DESC";

                return DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
            }
        }

        public struct RevenueSummary
        {
            public decimal TotalRevenue;
            public int TotalOrders;
            public decimal TotalDiscount;
            public decimal AverageOrderValue;
        }

        public static RevenueSummary GetRevenueSummary(DateTime fromDate, DateTime toDate, int branchId)
        {
            var summary = new RevenueSummary();
            var prms = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date.AddDays(1).AddTicks(-1))
            };

            string branchFilter = "";
            if (branchId > 0)
            {
                branchFilter = " AND hd.MaChiNhanh = @MaChiNhanh";
                prms.Add(new SqlParameter("@MaChiNhanh", branchId));
            }

            try
            {
                if (IsQuanLyBanhMi())
                {
                    string sql = $@"
                        SELECT 
                            ISNULL(SUM(hd.TongThanhToan), 0) AS TotalRevenue,
                            COUNT(hd.MaHoaDon) AS TotalOrders,
                            ISNULL(SUM(hd.TienGiam), 0) AS TotalDiscount
                        FROM dbo.HoaDon hd
                        WHERE hd.TrangThaiThanhToan = 'DaTT'
                          AND hd.NgayTao >= @FromDate AND hd.NgayTao <= @ToDate
                          {branchFilter}";

                    DataTable dt = DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
                    if (dt.Rows.Count > 0)
                    {
                        summary.TotalRevenue = Convert.ToDecimal(dt.Rows[0]["TotalRevenue"]);
                        summary.TotalOrders = Convert.ToInt32(dt.Rows[0]["TotalOrders"]);
                        summary.TotalDiscount = Convert.ToDecimal(dt.Rows[0]["TotalDiscount"]);
                        summary.AverageOrderValue = summary.TotalOrders > 0
                            ? Math.Round(summary.TotalRevenue / summary.TotalOrders, 0)
                            : 0;
                    }
                }
                else
                {
                    string sql = @"
                        SELECT 
                            ISNULL(SUM(FinalAmount), 0) AS TotalRevenue,
                            COUNT(OrderID) AS TotalOrders,
                            ISNULL(SUM(DiscountAmount), 0) AS TotalDiscount
                        FROM dbo.Orders
                        WHERE Status = 'COMPLETED'
                          AND OrderDate >= @FromDate AND OrderDate <= @ToDate";

                    DataTable dt = DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
                    if (dt.Rows.Count > 0)
                    {
                        summary.TotalRevenue = Convert.ToDecimal(dt.Rows[0]["TotalRevenue"]);
                        summary.TotalOrders = Convert.ToInt32(dt.Rows[0]["TotalOrders"]);
                        summary.TotalDiscount = Convert.ToDecimal(dt.Rows[0]["TotalDiscount"]);
                        summary.AverageOrderValue = summary.TotalOrders > 0
                            ? Math.Round(summary.TotalRevenue / summary.TotalOrders, 0)
                            : 0;
                    }
                }
            }
            catch { }

            return summary;
        }

        #endregion

        #region ====================== 3. BÁO CÁO SẢN PHẨM BEST SELLER ======================

        public static DataTable GetBestSellerReport(DateTime fromDate, DateTime toDate, int branchId, int categoryId, int topN, bool sortByRevenue)
        {
            var prms = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date.AddDays(1).AddTicks(-1))
            };

            string topClause = topN > 0 ? $"TOP {topN}" : "";
            string branchFilter = "";
            if (branchId > 0)
            {
                branchFilter = " AND hd.MaChiNhanh = @MaChiNhanh";
                prms.Add(new SqlParameter("@MaChiNhanh", branchId));
            }

            string catFilter = "";
            if (categoryId > 0)
            {
                catFilter = " AND sp.MaDanhMuc = @MaDanhMuc";
                prms.Add(new SqlParameter("@MaDanhMuc", categoryId));
            }

            string orderColumn = sortByRevenue ? "[Doanh Thu (VNĐ)]" : "[Số Lượng Đã Bán]";

            if (IsQuanLyBanhMi())
            {
                string sql = $@"
                    SELECT {topClause}
                        ROW_NUMBER() OVER (ORDER BY {(sortByRevenue ? "SUM(ct.ThanhTien)" : "SUM(ct.SoLuong)")} DESC) AS [Hạng],
                        sp.MaSKU AS [Mã SKU],
                        sp.TenSanPham AS [Tên Sản Phẩm],
                        dm.TenDanhMuc AS [Danh Mục],
                        dv.TenDonVi AS [ĐVT],
                        SUM(ct.SoLuong) AS [Số Lượng Đã Bán],
                        SUM(ct.ThanhTien) AS [Doanh Thu (VNĐ)]
                    FROM dbo.ChiTiet_HoaDon ct
                    JOIN dbo.HoaDon hd ON ct.MaHoaDon = hd.MaHoaDon
                    JOIN dbo.SanPham sp ON ct.MaSanPham = sp.MaSanPham
                    JOIN dbo.DanhMuc dm ON sp.MaDanhMuc = dm.MaDanhMuc
                    JOIN dbo.DonVi dv ON sp.MaDonViBan = dv.MaDonVi
                    WHERE hd.TrangThaiThanhToan = 'DaTT'
                      AND hd.NgayTao >= @FromDate AND hd.NgayTao <= @ToDate
                      {branchFilter}
                      {catFilter}
                    GROUP BY sp.MaSKU, sp.TenSanPham, dm.TenDanhMuc, dv.TenDonVi
                    ORDER BY {orderColumn} DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(sql, prms.ToArray());

                // Tính thêm cột % Đóng góp doanh thu
                dt.Columns.Add("Tỷ Trọng (%)", typeof(string));
                decimal totalRev = 0;
                foreach (DataRow r in dt.Rows)
                {
                    totalRev += Convert.ToDecimal(r["Doanh Thu (VNĐ)"]);
                }

                if (totalRev > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        decimal itemRev = Convert.ToDecimal(r["Doanh Thu (VNĐ)"]);
                        double pct = (double)(itemRev / totalRev) * 100.0;
                        r["Tỷ Trọng (%)"] = pct.ToString("0.0") + "%";
                    }
                }

                return dt;
            }
            else
            {
                // Fallback BanhMiShopDB
                string sql = $@"
                    SELECT {topClause}
                        ROW_NUMBER() OVER (ORDER BY {(sortByRevenue ? "SUM(od.TotalAmount)" : "SUM(od.Quantity)")} DESC) AS [Hạng],
                        p.ProductCode AS [Mã SKU],
                        p.ProductName AS [Tên Sản Phẩm],
                        c.CategoryName AS [Danh Mục],
                        p.Unit AS [ĐVT],
                        SUM(od.Quantity) AS [Số Lượng Đã Bán],
                        SUM(od.TotalAmount) AS [Doanh Thu (VNĐ)]
                    FROM dbo.OrderDetails od
                    JOIN dbo.Orders o ON od.OrderID = o.OrderID
                    JOIN dbo.Products p ON od.ProductID = p.ProductID
                    JOIN dbo.Categories c ON p.CategoryID = c.CategoryID
                    WHERE o.Status = 'COMPLETED'
                      AND o.OrderDate >= @FromDate AND o.OrderDate <= @ToDate
                    GROUP BY p.ProductCode, p.ProductName, c.CategoryName, p.Unit
                    ORDER BY {orderColumn} DESC";

                DataTable dt = DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
                dt.Columns.Add("Tỷ Trọng (%)", typeof(string));
                decimal totalRev = 0;
                foreach (DataRow r in dt.Rows)
                {
                    totalRev += Convert.ToDecimal(r["Doanh Thu (VNĐ)"]);
                }
                if (totalRev > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        decimal itemRev = Convert.ToDecimal(r["Doanh Thu (VNĐ)"]);
                        double pct = (double)(itemRev / totalRev) * 100.0;
                        r["Tỷ Trọng (%)"] = pct.ToString("0.0") + "%";
                    }
                }
                return dt;
            }
        }

        #endregion

        #region ====================== 4. BÁO CÁO TỒN KHO & TIÊU HAO ======================

        public static DataTable GetInventoryConsumptionReport(int branchId, bool onlyLowStock, string searchKeyword, DateTime fromDate, DateTime toDate)
        {
            var prms = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date.AddDays(1).AddTicks(-1))
            };

            string branchJoinStock = "";
            string branchFilterTrans = "";
            if (branchId > 0)
            {
                branchJoinStock = " AND tk.MaChiNhanh = @MaChiNhanh";
                branchFilterTrans = " AND MaChiNhanh = @MaChiNhanh";
                prms.Add(new SqlParameter("@MaChiNhanh", branchId));
            }

            string searchCondition = "";
            if (!string.IsNullOrWhiteSpace(searchKeyword))
            {
                searchCondition = " AND nl.TenNguyenLieu LIKE @Search";
                prms.Add(new SqlParameter("@Search", "%" + searchKeyword.Trim() + "%"));
            }

            if (IsQuanLyBanhMi())
            {
                string sql = $@"
                    SELECT 
                        nl.MaNguyenLieu AS [Mã NL],
                        nl.TenNguyenLieu AS [Tên Nguyên Liệu],
                        dv.TenDonVi AS [ĐVT],
                        ISNULL(SUM(tk.SoLuongTon), 0) AS [Tồn Kho Hiện Tại],
                        nl.TonToiThieuCanhBao AS [Tồn Tối Thiểu],
                        CASE 
                            WHEN ISNULL(SUM(tk.SoLuongTon), 0) <= nl.TonToiThieuCanhBao THEN N'⚠️ Sắp Hết' 
                            ELSE N'An Toàn' 
                        END AS [Trạng Thái Cảnh Báo],
                        ISNULL(xuat.LuongTieuHao, 0) AS [Tiêu Hao Bán Hàng],
                        ISNULL(nhap.LuongNhap, 0) AS [Tổng Đã Nhập]
                    FROM dbo.NguyenLieu nl
                    JOIN dbo.DonVi dv ON nl.MaDonViGoc = dv.MaDonVi
                    LEFT JOIN dbo.TonKho_ChiNhanh tk ON nl.MaNguyenLieu = tk.MaNguyenLieu {branchJoinStock}
                    LEFT JOIN (
                        SELECT MaNguyenLieu, ABS(SUM(SoLuongThayDoi)) AS LuongTieuHao
                        FROM dbo.NhatKy_Kho
                        WHERE LoaiGiaoDich = 'XUAT_BAN'
                          AND NgayTao >= @FromDate AND NgayTao <= @ToDate
                          {branchFilterTrans}
                        GROUP BY MaNguyenLieu
                    ) xuat ON nl.MaNguyenLieu = xuat.MaNguyenLieu
                    LEFT JOIN (
                        SELECT MaNguyenLieu, SUM(SoLuongThayDoi) AS LuongNhap
                        FROM dbo.NhatKy_Kho
                        WHERE LoaiGiaoDich = 'NHAP'
                          AND NgayTao >= @FromDate AND NgayTao <= @ToDate
                          {branchFilterTrans}
                        GROUP BY MaNguyenLieu
                    ) nhap ON nl.MaNguyenLieu = nhap.MaNguyenLieu
                    WHERE nl.DangHoatDong = 1
                      {searchCondition}
                    GROUP BY nl.MaNguyenLieu, nl.TenNguyenLieu, dv.TenDonVi, nl.TonToiThieuCanhBao, xuat.LuongTieuHao, nhap.LuongNhap
                    {(onlyLowStock ? "HAVING ISNULL(SUM(tk.SoLuongTon), 0) <= nl.TonToiThieuCanhBao" : "")}
                    ORDER BY 
                        CASE WHEN ISNULL(SUM(tk.SoLuongTon), 0) <= nl.TonToiThieuCanhBao THEN 0 ELSE 1 END,
                        nl.TenNguyenLieu ASC";

                return DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
            }
            else
            {
                // Fallback BanhMiShopDB
                string sql = $@"
                    SELECT 
                        IngredientID AS [Mã NL],
                        IngredientName AS [Tên Nguyên Liệu],
                        Unit AS [ĐVT],
                        CurrentStock AS [Tồn Kho Hiện Tại],
                        MinimumStock AS [Tồn Tối Thiểu],
                        CASE 
                            WHEN CurrentStock <= MinimumStock THEN N'⚠️ Sắp Hết' 
                            ELSE N'An Toàn' 
                        END AS [Trạng Thái Cảnh Báo],
                        0 AS [Tiêu Hao Bán Hàng],
                        0 AS [Tổng Đã Nhập]
                    FROM dbo.Ingredients
                    WHERE IsActive = 1
                      {(onlyLowStock ? "AND CurrentStock <= MinimumStock" : "")}
                    ORDER BY CurrentStock ASC";

                return DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
            }
        }

        #endregion

        #region ====================== 5. DỮ LIỆU VẼ BIỂU ĐỒ ======================

        public class ChartPoint
        {
            public string Label { get; set; } = string.Empty;
            public decimal Value { get; set; }
            public string FormattedValue { get; set; } = string.Empty;
            public System.Drawing.Color Color { get; set; }
        }

        /// <summary>
        /// Dữ liệu biểu đồ doanh thu theo ngày
        /// </summary>
        public static List<ChartPoint> GetDailyRevenueChartData(DateTime fromDate, DateTime toDate, int branchId)
        {
            var list = new List<ChartPoint>();
            var prms = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date.AddDays(1).AddTicks(-1))
            };

            string branchFilter = "";
            if (branchId > 0)
            {
                branchFilter = " AND hd.MaChiNhanh = @MaChiNhanh";
                prms.Add(new SqlParameter("@MaChiNhanh", branchId));
            }

            try
            {
                if (IsQuanLyBanhMi())
                {
                    string sql = $@"
                        SELECT 
                            CAST(hd.NgayTao AS DATE) AS Ngay,
                            FORMAT(CAST(hd.NgayTao AS DATE), 'dd/MM') AS NgayLabel,
                            SUM(hd.TongThanhToan) AS DoanhThu
                        FROM dbo.HoaDon hd
                        WHERE hd.TrangThaiThanhToan = 'DaTT'
                          AND hd.NgayTao >= @FromDate AND hd.NgayTao <= @ToDate
                          {branchFilter}
                        GROUP BY CAST(hd.NgayTao AS DATE)
                        ORDER BY CAST(hd.NgayTao AS DATE) ASC";

                    DataTable dt = DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
                    foreach (DataRow r in dt.Rows)
                    {
                        decimal val = Convert.ToDecimal(r["DoanhThu"]);
                        list.Add(new ChartPoint
                        {
                            Label = r["NgayLabel"].ToString() ?? "",
                            Value = val,
                            FormattedValue = val.ToString("N0") + " đ",
                            Color = System.Drawing.Color.FromArgb(41, 128, 185)
                        });
                    }
                }
            }
            catch { }

            return list;
        }

        /// <summary>
        /// Dữ liệu biểu đồ cơ cấu doanh thu theo danh mục món (Pie chart)
        /// </summary>
        public static List<ChartPoint> GetCategoryRevenueChartData(DateTime fromDate, DateTime toDate, int branchId)
        {
            var list = new List<ChartPoint>();
            var prms = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date.AddDays(1).AddTicks(-1))
            };

            string branchFilter = "";
            if (branchId > 0)
            {
                branchFilter = " AND hd.MaChiNhanh = @MaChiNhanh";
                prms.Add(new SqlParameter("@MaChiNhanh", branchId));
            }

            try
            {
                if (IsQuanLyBanhMi())
                {
                    string sql = $@"
                        SELECT 
                            dm.TenDanhMuc,
                            SUM(ct.ThanhTien) AS DoanhThu
                        FROM dbo.ChiTiet_HoaDon ct
                        JOIN dbo.HoaDon hd ON ct.MaHoaDon = hd.MaHoaDon
                        JOIN dbo.SanPham sp ON ct.MaSanPham = sp.MaSanPham
                        JOIN dbo.DanhMuc dm ON sp.MaDanhMuc = dm.MaDanhMuc
                        WHERE hd.TrangThaiThanhToan = 'DaTT'
                          AND hd.NgayTao >= @FromDate AND hd.NgayTao <= @ToDate
                          {branchFilter}
                        GROUP BY dm.TenDanhMuc
                        ORDER BY DoanhThu DESC";

                    DataTable dt = DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
                    System.Drawing.Color[] palette = new[]
                    {
                        System.Drawing.Color.FromArgb(230, 126, 34),  // Orange
                        System.Drawing.Color.FromArgb(46, 204, 113),  // Emerald
                        System.Drawing.Color.FromArgb(52, 152, 219),  // Peter River
                        System.Drawing.Color.FromArgb(155, 89, 182),  // Amethyst
                        System.Drawing.Color.FromArgb(241, 196, 15),  // Sunflower
                        System.Drawing.Color.FromArgb(26, 188, 156),  // Turquoise
                        System.Drawing.Color.FromArgb(231, 76, 60),   // Alizarin
                        System.Drawing.Color.FromArgb(52, 73, 94),    // Wet Asphalt
                        System.Drawing.Color.FromArgb(149, 165, 166)  // Silver
                    };

                    int idx = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        decimal val = Convert.ToDecimal(r["DoanhThu"]);
                        list.Add(new ChartPoint
                        {
                            Label = r["TenDanhMuc"].ToString() ?? "",
                            Value = val,
                            FormattedValue = val.ToString("N0") + " đ",
                            Color = palette[idx % palette.Length]
                        });
                        idx++;
                    }
                }
            }
            catch { }

            return list;
        }

        /// <summary>
        /// Dữ liệu biểu đồ so sánh doanh thu các chi nhánh
        /// </summary>
        public static List<ChartPoint> GetBranchRevenueChartData(DateTime fromDate, DateTime toDate)
        {
            var list = new List<ChartPoint>();
            var prms = new List<SqlParameter>
            {
                new SqlParameter("@FromDate", fromDate.Date),
                new SqlParameter("@ToDate", toDate.Date.AddDays(1).AddTicks(-1))
            };

            try
            {
                if (IsQuanLyBanhMi())
                {
                    string sql = @"
                        SELECT 
                            cn.TenChiNhanh,
                            SUM(hd.TongThanhToan) AS DoanhThu
                        FROM dbo.HoaDon hd
                        JOIN dbo.ChiNhanh cn ON hd.MaChiNhanh = cn.MaChiNhanh
                        WHERE hd.TrangThaiThanhToan = 'DaTT'
                          AND hd.NgayTao >= @FromDate AND hd.NgayTao <= @ToDate
                        GROUP BY cn.TenChiNhanh
                        ORDER BY DoanhThu DESC";

                    DataTable dt = DatabaseHelper.ExecuteQuery(sql, prms.ToArray());
                    System.Drawing.Color[] palette = new[]
                    {
                        System.Drawing.Color.FromArgb(39, 174, 96),
                        System.Drawing.Color.FromArgb(41, 128, 185),
                        System.Drawing.Color.FromArgb(142, 68, 173)
                    };

                    int idx = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        decimal val = Convert.ToDecimal(r["DoanhThu"]);
                        list.Add(new ChartPoint
                        {
                            Label = r["TenChiNhanh"].ToString() ?? "",
                            Value = val,
                            FormattedValue = val.ToString("N0") + " đ",
                            Color = palette[idx % palette.Length]
                        });
                        idx++;
                    }
                }
            }
            catch { }

            return list;
        }

        /// <summary>
        /// Dữ liệu biểu đồ Top 10 món bán chạy
        /// </summary>
        public static List<ChartPoint> GetTopProductsChartData(DateTime fromDate, DateTime toDate, int branchId, int topN = 10)
        {
            var list = new List<ChartPoint>();
            DataTable dt = GetBestSellerReport(fromDate, toDate, branchId, 0, topN, false);

            System.Drawing.Color[] palette = new[]
            {
                System.Drawing.Color.FromArgb(211, 84, 0),
                System.Drawing.Color.FromArgb(230, 126, 34),
                System.Drawing.Color.FromArgb(243, 156, 18),
                System.Drawing.Color.FromArgb(39, 174, 96),
                System.Drawing.Color.FromArgb(46, 204, 113),
                System.Drawing.Color.FromArgb(41, 128, 185),
                System.Drawing.Color.FromArgb(52, 152, 219),
                System.Drawing.Color.FromArgb(142, 68, 173),
                System.Drawing.Color.FromArgb(155, 89, 182),
                System.Drawing.Color.FromArgb(52, 73, 94)
            };

            int idx = 0;
            foreach (DataRow r in dt.Rows)
            {
                decimal qty = Convert.ToDecimal(r["Số Lượng Đã Bán"]);
                list.Add(new ChartPoint
                {
                    Label = r["Tên Sản Phẩm"].ToString() ?? "",
                    Value = qty,
                    FormattedValue = qty.ToString("N0") + " " + (r["ĐVT"].ToString() ?? ""),
                    Color = palette[idx % palette.Length]
                });
                idx++;
            }

            return list;
        }

        #endregion
    }
}
