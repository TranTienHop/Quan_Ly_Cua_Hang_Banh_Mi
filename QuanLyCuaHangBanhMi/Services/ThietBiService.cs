using System.Data;
using System.Text.RegularExpressions;
using QuanLyCuaHangBanhMi.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Services
{
    public record ThietBiDto(int? Ma, string MaCode, int MaChiNhanh, string Ten, string Loai,
                             int SoLuong, DateTime? NgayMua, decimal? DonGia, string TinhTrang, string? GhiChu);

    public static class ThietBiService
    {
        // Mã và nhãn hiển thị, khớp với các ràng buộc CHECK của bảng ThietBi
        public static readonly (string Code, string Text)[] Loai =
        {
            ("BepNuong", "Bếp nướng"), ("BaoQuan", "Bảo quản lạnh"), ("PhaChe", "Pha chế"),
            ("QuayThuNgan", "Quầy thu ngân"), ("NoiThat", "Nội thất")
        };

        public static readonly (string Code, string Text)[] TinhTrang =
        {
            ("DangDung", "Đang dùng"), ("BaoTri", "Bảo trì"), ("HuHong", "Hư hỏng"), ("ThanhLy", "Đã thanh lý")
        };

        /// <summary>Dựng DataTable (Code, Text) cho ComboBox. tatCa != null thì thêm dòng đầu có Code rỗng.</summary>
        public static DataTable Pairs((string Code, string Text)[] items, string? tatCa = null)
        {
            var dt = new DataTable();
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("Text", typeof(string));
            if (tatCa != null) dt.Rows.Add("", tatCa);
            foreach (var (c, t) in items) dt.Rows.Add(c, t);
            return dt;
        }

        public static string NhanLoai(string code) => Loai.FirstOrDefault(x => x.Code == code).Text ?? code;

        /// <summary>gioiHanCN = null nghĩa là không giới hạn (ADMIN); ngược lại chỉ được thao tác cho chi nhánh đó.</summary>
        private static void KiemTraQuyen(int? gioiHanCN, int chiNhanhPhaiLa)
        {
            if (gioiHanCN != null && gioiHanCN != chiNhanhPhaiLa)
                throw new InvalidOperationException("Bạn chỉ được quản lý thiết bị của chi nhánh mình.");
        }

        // =====================================================================
        //  TRA CỨU
        // =====================================================================

        /// <summary>maCN = null thì lấy mọi chi nhánh; loai và tinhTrang rỗng nghĩa là không lọc.</summary>
        public static DataTable GetAll(int? maCN, string loai, string tinhTrang, string keyword) => Db_QLKho.Query(@"
            SELECT tb.MaThietBi,
                   tb.MaCode,
                   cn.TenChiNhanh,
                   tb.MaChiNhanh,
                   tb.TenThietBi,
                   tb.LoaiThietBi,
                   tb.SoLuong,
                   tb.NgayMua,
                   tb.DonGiaMua,
                   CASE WHEN tb.DonGiaMua IS NULL THEN NULL ELSE tb.DonGiaMua * tb.SoLuong END AS GiaTri,
                   tb.TinhTrang,
                   tb.GhiChu
            FROM dbo.ThietBi tb
            JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = tb.MaChiNhanh
            WHERE (@cn IS NULL OR tb.MaChiNhanh = @cn)
              AND (@loai = '' OR tb.LoaiThietBi = @loai)
              AND (@tt = '' OR tb.TinhTrang = @tt)
              AND (tb.TenThietBi LIKE @k OR tb.MaCode LIKE @k)
            ORDER BY cn.MaCode, tb.MaCode",
            Db_QLKho.P("@cn", maCN), Db_QLKho.P("@loai", loai), Db_QLKho.P("@tt", tinhTrang),
            Db_QLKho.P("@k", "%" + keyword + "%"));

        /// <summary>Mã gợi ý dạng TB-{mã chi nhánh}-{số}: lấy số lớn nhất đang có của chi nhánh rồi cộng 1.</summary>
        public static string GoiYMa(int maCN)
        {
            var cn = Db_QLKho.Query("SELECT MaCode FROM dbo.ChiNhanh WHERE MaChiNhanh = @cn", Db_QLKho.P("@cn", maCN));
            if (cn.Rows.Count == 0) return "";
            string maChiNhanh = Convert.ToString(cn.Rows[0]["MaCode"]) ?? "";

            var codes = Db_QLKho.Query("SELECT MaCode FROM dbo.ThietBi WHERE MaChiNhanh = @cn", Db_QLKho.P("@cn", maCN));
            int max = 0;
            foreach (DataRow r in codes.Rows)
            {
                string c = Convert.ToString(r["MaCode"]) ?? "";
                int i = c.LastIndexOf('-');
                if (i >= 0 && int.TryParse(c[(i + 1)..], out int n)) max = Math.Max(max, n);
            }
            return $"TB-{maChiNhanh}-{max + 1:D2}";
        }

        // =====================================================================
        //  THÊM / SỬA
        // =====================================================================

        /// <summary>
        /// Thêm mới (Ma = null) hoặc cập nhật. Khi cập nhật, mã thiết bị và chi nhánh KHÔNG bị đổi.
        /// Trả về mã thiết bị (khóa chính).
        /// </summary>
        public static int Save(ThietBiDto t, int? gioiHanCN)
        {
            string ten = (t.Ten ?? "").Trim();
            if (ten.Length == 0)
                throw new InvalidOperationException("Tên thiết bị không được để trống.");
            if (ten.Length > 200)
                throw new InvalidOperationException("Tên thiết bị tối đa 200 ký tự.");
            if (!Loai.Any(x => x.Code == t.Loai))
                throw new InvalidOperationException("Loại thiết bị không hợp lệ.");
            if (!TinhTrang.Any(x => x.Code == t.TinhTrang))
                throw new InvalidOperationException("Tình trạng không hợp lệ.");
            if (t.SoLuong <= 0)
                throw new InvalidOperationException("Số lượng phải lớn hơn 0.");
            if (t.DonGia != null && t.DonGia < 0)
                throw new InvalidOperationException("Đơn giá mua không được âm.");
            if (t.NgayMua != null && t.NgayMua.Value.Date > DateTime.Today)
                throw new InvalidOperationException("Ngày mua không được ở tương lai.");
            if (!string.IsNullOrWhiteSpace(t.GhiChu) && t.GhiChu.Trim().Length > 255)
                throw new InvalidOperationException("Ghi chú tối đa 255 ký tự.");

            string? gc = string.IsNullOrWhiteSpace(t.GhiChu) ? null : t.GhiChu.Trim();
            var nm = t.NgayMua?.Date;

            try
            {
                if (t.Ma == null)
                {
                    string ma = (t.MaCode ?? "").Trim().ToUpperInvariant();
                    if (!Regex.IsMatch(ma, @"^[A-Z0-9][A-Z0-9\-]{2,19}$"))
                        throw new InvalidOperationException(
                            "Mã thiết bị gồm chữ, số và dấu gạch ngang, dài 3 đến 20 ký tự, không có khoảng trắng (ví dụ TB-CN01-16).");
                    KiemTraQuyen(gioiHanCN, t.MaChiNhanh);

                    return Convert.ToInt32(Db_QLKho.Scalar(@"
                        INSERT dbo.ThietBi (MaCode, MaChiNhanh, TenThietBi, LoaiThietBi, SoLuong, NgayMua, DonGiaMua, TinhTrang, GhiChu)
                        OUTPUT INSERTED.MaThietBi
                        VALUES (@mc, @cn, @ten, @loai, @sl, @nm, @dg, @tt, @gc)",
                        Db_QLKho.P("@mc", ma), Db_QLKho.P("@cn", t.MaChiNhanh), Db_QLKho.P("@ten", ten), Db_QLKho.P("@loai", t.Loai),
                        Db_QLKho.P("@sl", t.SoLuong), Db_QLKho.P("@nm", nm), Db_QLKho.P("@dg", t.DonGia),
                        Db_QLKho.P("@tt", t.TinhTrang), Db_QLKho.P("@gc", gc)));
                }

                var cu = Db_QLKho.Query("SELECT MaChiNhanh FROM dbo.ThietBi WHERE MaThietBi = @ma", Db_QLKho.P("@ma", t.Ma));
                if (cu.Rows.Count == 0)
                    throw new InvalidOperationException("Không tìm thấy thiết bị (có thể đã bị xóa).");
                KiemTraQuyen(gioiHanCN, Convert.ToInt32(cu.Rows[0]["MaChiNhanh"]));

                Db_QLKho.Exec(@"
                    UPDATE dbo.ThietBi
                    SET TenThietBi = @ten, LoaiThietBi = @loai, SoLuong = @sl,
                        NgayMua = @nm, DonGiaMua = @dg, TinhTrang = @tt, GhiChu = @gc
                    WHERE MaThietBi = @ma",
                    Db_QLKho.P("@ten", ten), Db_QLKho.P("@loai", t.Loai), Db_QLKho.P("@sl", t.SoLuong),
                    Db_QLKho.P("@nm", nm), Db_QLKho.P("@dg", t.DonGia), Db_QLKho.P("@tt", t.TinhTrang),
                    Db_QLKho.P("@gc", gc), Db_QLKho.P("@ma", t.Ma));
                return t.Ma.Value;
            }
            catch (SqlException ex) when (ex.Number is 2627 or 2601)       // vi phạm UNIQUE của MaCode
            {
                throw new InvalidOperationException("Mã thiết bị đã tồn tại, hãy dùng mã khác (bấm 'Gợi ý').");
            }
        }
    }
}