namespace QuanLyCuaHangBanhMi.Models
{
    public sealed class ProductSaveModel
    {
        public int ProductId { get; init; }
        public string Code { get; init; } = "";
        public string Name { get; init; } = "";
        public int CategoryId { get; init; }
        public decimal Price { get; init; }
        public string Unit { get; init; } = "";
        public bool IsActive { get; init; }
    }

    public sealed class CustomerSaveModel
    {
        public int CustomerId { get; init; }
        public int MaHang { get; init; }
        public string HoTen { get; init; } = "";
        public string SoDienThoai { get; init; } = "";
        public int Diem { get; init; }
        public string? Email { get; init; }
        public string? GhiChu { get; init; }
        public bool DangHoatDong { get; init; }
    }

    public sealed class PromoSaveModel
    {
        public int PromoId { get; init; }
        public string Code { get; init; } = "";
        public string Name { get; init; } = "";
        public string LoaiGiam { get; init; } = "";
        public decimal GiaTri { get; init; }
        public decimal? GiamToiDa { get; init; }
        public decimal DonToiThieu { get; init; }
        public int? MaChiNhanh { get; init; }
        public System.DateTime NgayBatDau { get; init; }
        public System.DateTime NgayKetThuc { get; init; }
        public bool DangHoatDong { get; init; }
    }

    public sealed class IngredientSaveModel
    {
        public int IngredientId { get; init; }
        public string Code { get; init; } = "";
        public string Name { get; init; } = "";
        public string Unit { get; init; } = "";
        public decimal CurrentStock { get; init; }
        public decimal MinimumStock { get; init; }
        public decimal CostPrice { get; init; }
    }
}
