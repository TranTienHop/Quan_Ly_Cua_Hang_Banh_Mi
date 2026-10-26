using System.Data;
using System.Text.RegularExpressions;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class NccService
    {
        /// <summary>Danh sách NCC kèm số phiếu nhập và công nợ còn lại (chỉ tính phiếu đã nhập hàng).</summary>
        public static DataTable GetAll(string keyword, bool? hoatDong) => Db_QLKho.Query(@"
            SELECT n.MaNCC,
                   n.TenNCC,
                   n.SoDienThoai,
                   n.DiaChi,
                   ISNULL(p.SoPhieu, 0) AS SoPhieu,
                   ISNULL(p.ConNo, 0)   AS ConNo,
                   CASE WHEN n.DangHoatDong = 1 THEN N'Đang hợp tác' ELSE N'Ngừng hợp tác' END AS TrangThai,
                   n.DangHoatDong
            FROM dbo.NhaCungCap n
            LEFT JOIN (
                SELECT MaNCC,
                       COUNT(*) AS SoPhieu,
                       SUM(CASE WHEN TrangThai = N'DaNhap' THEN TongTien - DaThanhToan ELSE 0 END) AS ConNo
                FROM dbo.Phieu_NhapHang
                GROUP BY MaNCC
            ) p ON p.MaNCC = n.MaNCC
            WHERE (n.TenNCC LIKE @k OR n.SoDienThoai LIKE @k OR n.DiaChi LIKE @k)
              AND (@hd IS NULL OR n.DangHoatDong = @hd)
            ORDER BY n.TenNCC",
            Db_QLKho.P("@k", "%" + keyword + "%"),
            Db_QLKho.P("@hd", hoatDong));

        /// <summary>Dùng cho ComboBox khi lập phiếu nhập: chỉ NCC còn hợp tác.</summary>
        public static DataTable GetCombo() => Db_QLKho.Query(
            "SELECT MaNCC, TenNCC FROM dbo.NhaCungCap WHERE DangHoatDong = 1 ORDER BY TenNCC");

        /// <summary>
        /// Chuẩn hóa số điện thoại (bỏ khoảng trắng, dấu chấm, gạch ngang).
        /// Trả về null nếu để trống; nếu sai định dạng thì gán thông báo vào <paramref name="loi"/>.
        /// </summary>
        public static string? ChuanHoaSdt(string? s, out string? loi)
        {
            loi = null;
            if (string.IsNullOrWhiteSpace(s)) return null;

            var x = new string(s.Where(c => !char.IsWhiteSpace(c) && c != '.' && c != '-').ToArray());
            if (!Regex.IsMatch(x, @"^\+?\d{8,15}$"))
            {
                loi = "Số điện thoại gồm 8–15 chữ số (có thể bắt đầu bằng +).";
                return null;
            }
            return x;
        }

        /// <summary>Thêm mới (ma = null) hoặc cập nhật. Trả về mã NCC.</summary>
        public static int Save(int? ma, string ten, string? sdt, string? diaChi, bool hoatDong)
        {
            ten = (ten ?? "").Trim();
            if (ten.Length == 0)
                throw new InvalidOperationException("Tên nhà cung cấp không được để trống.");

            string? sdtChuan = ChuanHoaSdt(sdt, out string? loiSdt);
            if (loiSdt != null) throw new InvalidOperationException(loiSdt);

            string? dc = string.IsNullOrWhiteSpace(diaChi) ? null : diaChi.Trim();

            // Không có ràng buộc UNIQUE ở DB nên kiểm tra trùng tên tại đây
            int trung = Convert.ToInt32(Db_QLKho.Scalar(
                "SELECT COUNT(*) FROM dbo.NhaCungCap WHERE TenNCC = @t AND MaNCC <> @ma",
                Db_QLKho.P("@t", ten), Db_QLKho.P("@ma", ma ?? 0)));
            if (trung > 0)
                throw new InvalidOperationException($"Đã có nhà cung cấp tên \"{ten}\".");

            if (ma == null)
            {
                return Convert.ToInt32(Db_QLKho.Scalar(@"
                    INSERT dbo.NhaCungCap (TenNCC, SoDienThoai, DiaChi, DangHoatDong)
                    OUTPUT INSERTED.MaNCC
                    VALUES (@t, @s, @d, @h)",
                    Db_QLKho.P("@t", ten), Db_QLKho.P("@s", sdtChuan), Db_QLKho.P("@d", dc), Db_QLKho.P("@h", hoatDong)));
            }

            Db_QLKho.Exec(@"
                UPDATE dbo.NhaCungCap
                SET TenNCC = @t, SoDienThoai = @s, DiaChi = @d, DangHoatDong = @h
                WHERE MaNCC = @ma",
                Db_QLKho.P("@t", ten), Db_QLKho.P("@s", sdtChuan), Db_QLKho.P("@d", dc),
                Db_QLKho.P("@h", hoatDong), Db_QLKho.P("@ma", ma));
            return ma.Value;
        }
    }
}