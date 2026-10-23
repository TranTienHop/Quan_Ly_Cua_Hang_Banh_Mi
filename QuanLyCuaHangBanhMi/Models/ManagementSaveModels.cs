namespace QuanLyCuaHangBanhMi.Models
{
    public sealed class CustomerSaveModel
    {
        public int CustomerId { get; init; }
        public int MaHang { get; init; }
        public string HoTen { get; init; } = string.Empty;
        public string SoDienThoai { get; init; } = string.Empty;
        public int Diem { get; init; }
        public string? Email { get; init; }
        public string? GhiChu { get; init; }
        public bool DangHoatDong { get; init; }
    }

    public sealed class PromoSaveModel
    {
        public int PromoId { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string LoaiGiam { get; init; } = string.Empty;
        public decimal GiaTri { get; init; }
        public decimal? GiamToiDa { get; init; }
        public decimal DonToiThieu { get; init; }
        public int? MaChiNhanh { get; init; }
        public DateTime NgayBatDau { get; init; }
        public DateTime NgayKetThuc { get; init; }
        public bool DangHoatDong { get; init; }
    }

    public sealed class IngredientSaveModel
    {
        public int IngredientId { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Unit { get; init; } = string.Empty;
        public decimal CurrentStock { get; init; }
        public decimal MinimumStock { get; init; }
        public decimal CostPrice { get; init; }
    }
}
