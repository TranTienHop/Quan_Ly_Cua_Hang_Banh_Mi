using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class ReceiptService
    {
        public static (DataTable Header, DataTable Lines, DataTable Payments) LoadForPrint(int maHoaDon)
        {
            return (
                ReceiptRepository.GetHeader(maHoaDon),
                ReceiptRepository.GetLines(maHoaDon),
                ReceiptRepository.GetPayments(maHoaDon)
            );
        }
    }
}
