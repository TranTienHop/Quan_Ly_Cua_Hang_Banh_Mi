using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services.Abstractions;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>
    /// Facade tĩnh — ủy thác sang <see cref="AuthServiceImpl"/> (IAuthService) để giữ tương thích UI cũ.
    /// </summary>
    public static class AuthService
    {
        private static readonly IAuthService Impl = new AuthServiceImpl();

        public enum LoginStatus
        {
            Success = 0,
            InvalidCredentials = 1,
            Locked = 2
        }

        public static LoginStatus TryLogin(string username, string password, out LoginResult? result)
        {
            var status = Impl.TryLogin(username, password, out result);
            return status switch
            {
                Abstractions.LoginStatus.Locked => LoginStatus.Locked,
                Abstractions.LoginStatus.InvalidCredentials => LoginStatus.InvalidCredentials,
                _ => LoginStatus.Success
            };
        }

        public static void ApplySession(LoginResult user) => Impl.ApplySession(user);

        public static bool TestConnection(out string error) => Impl.TestConnection(out error);
    }
}
