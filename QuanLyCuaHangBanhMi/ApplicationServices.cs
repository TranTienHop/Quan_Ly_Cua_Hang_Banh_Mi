using System;
using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.Services.Abstractions;

namespace QuanLyCuaHangBanhMi
{
    /// <summary>Composition root tối thiểu; ứng dụng tiếp tục dùng ADO.NET repositories.</summary>
    internal sealed class ApplicationServices
    {
        private ApplicationServices(IAuthService authService)
        {
            AuthService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public IAuthService AuthService { get; }

        public static ApplicationServices CreateDefault() => new(new AuthServiceImpl());
    }
}
