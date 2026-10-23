using System;
using System.Collections.Generic;
using System.Data;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services.Abstractions;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>
    /// Facade tĩnh — ủy thác sang <see cref="OrderServiceImpl"/> (IOrderService).
    /// </summary>
    public static class OrderService
    {
        private static readonly IOrderService Impl = new OrderServiceImpl();

        public static BranchInfo? GetBranchByEmployee(int maNhanVien) => Impl.GetBranchByEmployee(maNhanVien);
        public static string GetOpenShiftText(int maChiNhanh) => Impl.GetOpenShiftText(maChiNhanh);
        public static DataTable GetActivePromotions(int maChiNhanh) => Impl.GetActivePromotions(maChiNhanh);
        public static DataTable GetPosMenu(int maChiNhanh) => Impl.GetPosMenu(maChiNhanh);
        public static List<(string TenNguyenLieu, decimal SoLuong)> GetRecipe(int maSanPham) => Impl.GetRecipe(maSanPham);
        public static DataTable FindCustomerByPhone(string phone) => Impl.FindCustomerByPhone(phone);
        public static CheckoutResult Checkout(CheckoutRequest request, IFormatProvider? numberFormat = null)
            => Impl.Checkout(request, numberFormat);
        public static DataTable GetOrdersByDate(int maChiNhanh, DateTime ngay) => Impl.GetOrdersByDate(maChiNhanh, ngay);
        public static DataTable GetOrderDetails(int maHoaDon) => Impl.GetOrderDetails(maHoaDon);
    }
}
