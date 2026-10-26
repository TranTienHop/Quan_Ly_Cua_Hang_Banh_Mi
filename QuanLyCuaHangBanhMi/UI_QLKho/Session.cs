using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.UI_QLKho
{
    public static class Session
    {
        public static int MaNhanVien => UserSession.EmployeeID > 0 ? UserSession.EmployeeID : 1;
        public static int MaChiNhanh => UserSession.MaChiNhanh > 0 ? UserSession.MaChiNhanh : 1;
        public static string TenNhanVien => !string.IsNullOrEmpty(UserSession.FullName) ? UserSession.FullName : "Admin";
        public static string TenChiNhanh => "Chi nhánh chính";
        public static bool LaQuanLy => UserSession.IsManagerOrAdmin;   // duyệt hủy, hoàn thành kiểm kho
        public static bool LaAdmin => UserSession.IsAdmin;     // đổi chi nhánh
    }
}