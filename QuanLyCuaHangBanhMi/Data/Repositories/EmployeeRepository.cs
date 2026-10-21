using System.Data;

namespace QuanLyCuaHangBanhMi.Data.Repositories
{
    public static class EmployeeRepository
    {
        public static DataTable GetAllWithAccounts()
        {
            return DatabaseHelper.ExecuteQuery(@"
                SELECT e.EmployeeID, e.EmployeeCode AS [Mã NV], e.FullName AS [Họ và Tên],
                       e.Phone AS [SĐT], e.Email, e.Position AS [Chức Vụ],
                       u.Username AS [Tên Đăng Nhập], r.RoleName AS [Quyền Hạn],
                       e.Status AS [Trạng Thái]
                FROM dbo.Employees e
                LEFT JOIN dbo.Users u ON e.EmployeeID = u.EmployeeID
                LEFT JOIN dbo.Roles r ON u.RoleID = r.RoleID
                ORDER BY e.EmployeeID");
        }
    }
}
