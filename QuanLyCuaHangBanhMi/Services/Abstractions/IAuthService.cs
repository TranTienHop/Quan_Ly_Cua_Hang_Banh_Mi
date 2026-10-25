using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services.Abstractions
{
    public enum LoginStatus
    {
        Success,
        InvalidCredentials,
        Locked,
        TemporarilyLocked
    }

    public interface IAuthService
    {
        LoginStatus TryLogin(string username, string password, out LoginResult? result);
        void ApplySession(LoginResult user);
        bool TestConnection(out string error);
    }
}
