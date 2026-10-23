using System;
using System.Collections.Generic;
using System.Data;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services.Abstractions
{
    public interface IOrderService
    {
        BranchInfo? GetBranchByEmployee(int maNhanVien);
        string GetOpenShiftText(int maChiNhanh);
        DataTable GetActivePromotions(int maChiNhanh);
        DataTable GetPosMenu(int maChiNhanh);
        List<(string TenNguyenLieu, decimal SoLuong)> GetRecipe(int maSanPham);
        DataTable FindCustomerByPhone(string phone);
        CheckoutResult Checkout(CheckoutRequest request, IFormatProvider? numberFormat = null);
        DataTable GetOrdersByDate(int maChiNhanh, DateTime ngay);
        DataTable GetOrderDetails(int maHoaDon);
    }
}
