using System.Data;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    public static class ReportRepository
    {
        public static DataTable GetDailyRevenue()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT SaleDate AS [Ngày Bán], TotalOrders AS [Số Đơn Hàng], TotalRevenue AS [Doanh Thu (VNĐ)]
                FROM dbo.v_DailyRevenue
                ORDER BY SaleDate DESC");
        }

        public static DataTable GetProductSales()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT ProductCode AS [Mã Món], ProductName AS [Tên Món],
                       TotalQuantitySold AS [Số Lượng Bán], TotalRevenue AS [Tổng Thu (VNĐ)]
                FROM dbo.v_ProductSales
                ORDER BY TotalQuantitySold DESC");
        }
    }
}
