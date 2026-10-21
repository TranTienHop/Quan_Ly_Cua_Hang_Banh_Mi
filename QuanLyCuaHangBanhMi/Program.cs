using System;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Forms;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Đăng nhập → FormMain. BÁN HÀNG mở FormOrder. Đăng xuất → login.
            while (true)
            {
                using var loginForm = new FormLogin();
                if (loginForm.ShowDialog() != DialogResult.OK) break;

                using var mainForm = new FormMain();
                Application.Run(mainForm);
                if (!mainForm.IsLoggedOut) break;

                UserSession.Clear();
            }
        }
    }
}
