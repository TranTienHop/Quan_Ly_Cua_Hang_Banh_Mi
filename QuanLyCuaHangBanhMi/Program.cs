using System;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Data;
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

            try
            {
                DatabaseHelper.ConfigureConnectionString(DatabaseConfiguration.LoadConnectionString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể đọc cấu hình kết nối cơ sở dữ liệu.\n\nChi tiết: " + ex.Message,
                    "Lỗi cấu hình",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            var services = ApplicationServices.CreateDefault();

            // Đăng nhập → FormMain. BÁN HÀNG mở FormOrder. Đăng xuất → login.
            while (true)
            {
                using var loginForm = new FormLogin(services.AuthService);
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    UserSession.Clear();
                    break;
                }

                using var mainForm = new FormMain();
                Application.Run(mainForm);
                if (!mainForm.IsLoggedOut) break;

                UserSession.Clear();
            }
        }
    }
}
