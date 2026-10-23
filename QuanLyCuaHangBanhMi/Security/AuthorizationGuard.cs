using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Security
{
    internal static class AuthorizationGuard
    {
        public static void Demand(string permissionCode)
        {
            if (!UserSession.HasPermission(permissionCode))
                throw new UnauthorizedAccessException($"Tài khoản không có quyền {permissionCode}.");
        }
    }
}
