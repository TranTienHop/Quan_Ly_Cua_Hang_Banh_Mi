using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.Services.Abstractions;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormLogin : Form
    {
        private readonly IAuthService _authService = new AuthServiceImpl();

        public FormLogin()
        {
            InitializeComponent();
            AcceptButton = btnLogin;
            CancelButton = btnExit;

            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                loginContentPanel.Resize += (_, _) => ApplyResponsiveLoginLayout();
                ApplyResponsiveLoginLayout();
            }
        }

        private void FormLogin_Load(object? sender, EventArgs e)
        {
            LoadPoster();
            txtUsername.Focus();
        }

        private void LoadPoster()
        {
            string posterPath = Path.Combine(
                AppContext.BaseDirectory,
                "Assets",
                "Images",
                "login_poster.png");

            if (!File.Exists(posterPath))
                return;

            try
            {
                using var source = Image.FromFile(posterPath);
                picPoster.Image = new Bitmap(source);
            }
            catch
            {
                // Phần hình ảnh không được phép làm gián đoạn luồng đăng nhập.
                picPoster.Image = null;
            }
        }

        private void ApplyResponsiveLoginLayout()
        {
            int panelWidth = loginContentPanel.ClientSize.Width;
            if (panelWidth <= 0)
                return;

            int fieldWidth = Math.Min(390, Math.Max(300, panelWidth - 64));
            int fieldLeft = Math.Max(32, (panelWidth - fieldWidth) / 2);
            int titleWidth = Math.Min(440, Math.Max(300, panelWidth - 48));
            int titleLeft = Math.Max(24, (panelWidth - titleWidth) / 2);

            lblTitle.SetBounds(titleLeft, lblTitle.Top, titleWidth, lblTitle.Height);
            lblSubtitle.SetBounds(titleLeft, lblSubtitle.Top, titleWidth, lblSubtitle.Height);
            lblUsername.Left = fieldLeft;
            txtUsername.SetBounds(fieldLeft, txtUsername.Top, fieldWidth, txtUsername.Height);
            lblPassword.Left = fieldLeft;
            txtPassword.SetBounds(fieldLeft, txtPassword.Top, fieldWidth, txtPassword.Height);
            chkShowPassword.Left = fieldLeft;
            btnLogin.SetBounds(fieldLeft, btnLogin.Top, fieldWidth, btnLogin.Height);
            btnExit.SetBounds(fieldLeft, btnExit.Top, fieldWidth, btnExit.Height);
            pnlDivider.SetBounds(fieldLeft, pnlDivider.Top, fieldWidth, pnlDivider.Height);
            lblRoleHint.SetBounds(fieldLeft, lblRoleHint.Top, fieldWidth, lblRoleHint.Height);
            lnkTestConnection.Left = fieldLeft + fieldWidth - lnkTestConnection.Width;
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                ShowWarning("Vui lòng nhập tên đăng nhập.");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowWarning("Vui lòng nhập mật khẩu.");
                txtPassword.Focus();
                return;
            }

            SetLoginBusy(true);
            try
            {
                LoginStatus status = _authService.TryLogin(username, password, out _);
                switch (status)
                {
                    case LoginStatus.Locked:
                        MessageBox.Show(
                            "Tài khoản hoặc nhân viên đã ngừng hoạt động. Vui lòng liên hệ quản trị viên.",
                            "Tài khoản bị khóa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;

                    case LoginStatus.InvalidCredentials:
                        MessageBox.Show(
                            "Tên đăng nhập hoặc mật khẩu không chính xác.",
                            "Đăng nhập thất bại",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtPassword.Clear();
                        txtPassword.Focus();
                        return;

                    case LoginStatus.Success:
                        DialogResult = DialogResult.OK;
                        Close();
                        return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không thể đăng nhập vào hệ thống.\n\nChi tiết: " + ex.Message,
                    "Lỗi kết nối cơ sở dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed)
                    SetLoginBusy(false);
            }
        }

        private void SetLoginBusy(bool busy)
        {
            txtUsername.Enabled = !busy;
            txtPassword.Enabled = !busy;
            chkShowPassword.Enabled = !busy;
            btnLogin.Enabled = !busy;
            btnLogin.Text = busy ? "Đang xác thực..." : "Đăng nhập";
            UseWaitCursor = busy;
        }

        private static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Thông tin chưa đầy đủ",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void chkShowPassword_CheckedChanged(object? sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }

        private void lnkTestConnection_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
        {
            lblConnectionStatus.Text = "Đang kiểm tra kết nối...";
            lblConnectionStatus.ForeColor = Color.FromArgb(107, 114, 128);
            Refresh();

            if (_authService.TestConnection(out string error))
            {
                lblConnectionStatus.Text = "Đã kết nối CSDL QuanLyBanhMi";
                lblConnectionStatus.ForeColor = Color.FromArgb(22, 101, 52);
            }
            else
            {
                lblConnectionStatus.Text = "Không thể kết nối CSDL";
                lblConnectionStatus.ForeColor = Color.FromArgb(185, 28, 28);
                MessageBox.Show(error, "Lỗi kết nối",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
