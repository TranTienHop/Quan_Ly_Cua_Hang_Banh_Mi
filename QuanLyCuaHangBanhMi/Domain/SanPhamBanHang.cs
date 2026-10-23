using System;
using System.Globalization;
using System.Text;

namespace QuanLyCuaHangBanhMi.Domain
{
    /// <summary>
    /// Lớp trừu tượng nghiệp vụ bán hàng — các loại món (ThanhPham / Combo / KhongTruKho) kế thừa và override hành vi.
    /// </summary>
    public abstract class SanPhamBanHang : ISanPhamBanHang
    {
        protected SanPhamBanHang(
            int maSanPham,
            string tenSanPham,
            string loaiSanPham,
            string tenDanhMuc,
            decimal giaBan,
            decimal? coTheBan)
        {
            MaSanPham = maSanPham;
            TenSanPham = tenSanPham;
            LoaiSanPham = loaiSanPham;
            TenDanhMuc = tenDanhMuc;
            GiaBan = giaBan;
            CoTheBan = coTheBan;
        }

        public int MaSanPham { get; }
        public string TenSanPham { get; }
        public string LoaiSanPham { get; }
        public string TenDanhMuc { get; }
        public decimal GiaBan { get; }
        public decimal? CoTheBan { get; }

        public abstract string IconEmoji { get; }
        public abstract bool TinhVaoNenKhuyenMai { get; }
        public abstract bool ChoPhepTuyChinh { get; }

        public virtual bool HetHang => CoTheBan.HasValue && CoTheBan.Value <= 0;

        public virtual string FormatGia(CultureInfo culture)
            => GiaBan.ToString("N0", culture) + "đ";

        public virtual string FormatTonKho(CultureInfo culture)
        {
            if (!CoTheBan.HasValue) return "";
            return HetHang ? "Hết hàng" : "Còn " + CoTheBan.Value.ToString("N0", culture);
        }

        protected static string BoDau(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            string normalized = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);
            foreach (char c in normalized)
            {
                var uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
        }
    }

    /// <summary>Món thành phẩm (bánh mì…) — trừ kho theo công thức, cho tùy chỉnh.</summary>
    public sealed class ThanhPhamItem : SanPhamBanHang
    {
        public ThanhPhamItem(int ma, string ten, string danhMuc, decimal gia, decimal? coTheBan)
            : base(ma, ten, "ThanhPham", danhMuc, gia, coTheBan) { }

        public override bool TinhVaoNenKhuyenMai => true;
        public override bool ChoPhepTuyChinh => true;

        public override string IconEmoji
        {
            get
            {
                string ten = BoDau(TenSanPham);
                string dm = BoDau(TenDanhMuc);
                if (dm.Contains("ngot") || ten.Contains("kem") || ten.Contains("socola")) return "🧁";
                return "🥖";
            }
        }
    }

    /// <summary>Combo — không cộng nền KM từng dòng như món lẻ.</summary>
    public sealed class ComboItem : SanPhamBanHang
    {
        public ComboItem(int ma, string ten, string danhMuc, decimal gia, decimal? coTheBan)
            : base(ma, ten, "Combo", danhMuc, gia, coTheBan) { }

        public override bool TinhVaoNenKhuyenMai => false;
        public override bool ChoPhepTuyChinh => true;
        public override string IconEmoji => "🍱";
    }

    /// <summary>Không trừ kho (nước đóng chai, một số đồ uống…).</summary>
    public sealed class KhongTruKhoItem : SanPhamBanHang
    {
        public KhongTruKhoItem(int ma, string ten, string danhMuc, decimal gia, decimal? coTheBan)
            : base(ma, ten, "KhongTruKho", danhMuc, gia, coTheBan) { }

        public override bool TinhVaoNenKhuyenMai => true;
        public override bool ChoPhepTuyChinh => false;

        public override string IconEmoji
        {
            get
            {
                string ten = BoDau(TenSanPham);
                string dm = BoDau(TenDanhMuc);
                if (ten.Contains("ca phe")) return "☕";
                if (dm.Contains("dong chai") || ten.Contains("aquafina") || ten.Contains("coca") || ten.Contains("pepsi"))
                    return "🥤";
                if (dm.Contains("nuoc") || dm.Contains("pha che") || ten.StartsWith("tra ") || ten.Contains("sinh to"))
                    return "🧋";
                return "🧃";
            }
        }
    }
}
