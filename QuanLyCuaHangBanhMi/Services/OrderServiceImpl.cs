using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services.Abstractions;
using QuanLyCuaHangBanhMi.Security;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>Triển khai IOrderService — nghiệp vụ POS.</summary>
    public sealed class OrderServiceImpl : IOrderService
    {
        public BranchInfo? GetBranchByEmployee(int maNhanVien)
        {
            var dt = OrderRepository.GetBranchByEmployee(maNhanVien);
            if (dt.Rows.Count == 0) return null;
            var row = dt.Rows[0];
            return new BranchInfo
            {
                MaChiNhanh = row.Field<int>("MaChiNhanh"),
                MaCode = row.Field<string>("MaCode") ?? "",
                TenChiNhanh = row.Field<string>("TenChiNhanh") ?? ""
            };
        }

        public string GetOpenShiftText(int maChiNhanh)
        {
            var dt = OrderRepository.GetOpenShift(maChiNhanh);
            return dt.Rows.Count > 0
                ? $"Ca #{dt.Rows[0]["MaCa"]} mở lúc {dt.Rows[0].Field<DateTime>("GioMo"):HH:mm}"
                : "Chưa mở ca";
        }

        public DataTable GetActivePromotions(int maChiNhanh)
            => PromotionRepository.GetActiveForBranch(maChiNhanh);

        public DataTable GetPosMenu(int maChiNhanh)
        {
            AuthorizationGuard.Demand(PermissionCodes.PosOrder);
            return OrderRepository.GetPosMenu(maChiNhanh);
        }

        public List<(string TenNguyenLieu, decimal SoLuong)> GetRecipe(int maSanPham)
        {
            var dt = OrderRepository.GetRecipe(maSanPham);
            return dt.AsEnumerable()
                .Select(r => (r.Field<string>("TenNguyenLieu") ?? "", r.Field<decimal>("SoLuongTrenMotPhan")))
                .ToList();
        }

        public DataTable FindCustomerByPhone(string phone)
        {
            AuthorizationGuard.Demand(PermissionCodes.PosCustomer);
            return CustomerRepository.FindByPhone(phone);
        }

        public CheckoutResult Checkout(CheckoutRequest request, IFormatProvider? numberFormat = null)
        {
            AuthorizationGuard.Demand(PermissionCodes.PosOrder);
            AuthorizationGuard.Demand(PermissionCodes.PosPayment);
            return OrderRepository.SaveCheckout(request, numberFormat ?? CultureInfo.GetCultureInfo("vi-VN"));
        }

        public DataTable GetOrdersByDate(int maChiNhanh, DateTime ngay)
        {
            AuthorizationGuard.Demand(PermissionCodes.PosSearch);
            return OrderHistoryRepository.GetOrdersByDate(maChiNhanh, ngay);
        }

        public DataTable GetOrderDetails(int maHoaDon)
        {
            AuthorizationGuard.Demand(PermissionCodes.PosSearch);
            return OrderHistoryRepository.GetOrderDetails(maHoaDon);
        }
    }
}
