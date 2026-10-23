using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Security;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class ReceiptService
    {
        public static (DataTable Header, DataTable Lines, DataTable Payments) LoadForPrint(int maHoaDon)
        {
            AuthorizationGuard.Demand(PermissionCodes.PosPrint);
            return (
                ReceiptRepository.GetHeader(maHoaDon),
                ReceiptRepository.GetLines(maHoaDon),
                ReceiptRepository.GetPayments(maHoaDon)
            );
        }
    }
}
