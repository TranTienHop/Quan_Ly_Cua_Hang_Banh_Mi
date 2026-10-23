using System.Globalization;

namespace QuanLyCuaHangBanhMi.Domain
{
    /// <summary>
    /// Hợp đồng nghiệp vụ cho món bán trên POS (map từ cột LoaiSanPham trong DB).
    /// </summary>
    public interface ISanPhamBanHang
    {
        int MaSanPham { get; }
        string TenSanPham { get; }
        string LoaiSanPham { get; }
        string TenDanhMuc { get; }
        decimal GiaBan { get; }
        decimal? CoTheBan { get; }

        /// <summary>Icon hiển thị trên ProductTile.</summary>
        string IconEmoji { get; }

        /// <summary>Combo không cộng vào nền khuyến mãi dòng món thường.</summary>
        bool TinhVaoNenKhuyenMai { get; }

        /// <summary>Thành phẩm có công thức → mở form tùy chỉnh.</summary>
        bool ChoPhepTuyChinh { get; }

        bool HetHang { get; }

        string FormatTonKho(CultureInfo culture);
        string FormatGia(CultureInfo culture);
    }
}
