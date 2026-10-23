using System;
using System.Data;

namespace QuanLyCuaHangBanhMi.Models
{
    /// <summary>Dữ liệu đơn hàng cần lưu khi thanh toán POS.</summary>
    public sealed class CheckoutRequest
    {
        public int MaChiNhanh { get; init; }
        public string MaCodeChiNhanh { get; init; } = "";
        public int MaThuNgan { get; init; }
        public int? MaKhachHang { get; init; }
        public int? MaKhuyenMai { get; init; }
        public string NguonDon { get; init; } = "";
        public string LoaiDon { get; init; } = "";
        public string DiaChiGiao { get; init; } = "";
        public string HinhThucThanhToan { get; init; } = "";
        public string MaThamChieu { get; init; } = "";
        public decimal TienHang { get; init; }
        public decimal TienGiam { get; init; }
        public decimal PhiShip { get; init; }
        public decimal TongThanhToan { get; init; }
        public int DiemCong { get; init; }
        public int DiemTru { get; init; }
        public DataTable CartTable { get; init; } = new DataTable();
    }

    public sealed class CheckoutResult
    {
        public int MaHoaDon { get; init; }
        public string MaDon { get; init; } = "";
    }
}
