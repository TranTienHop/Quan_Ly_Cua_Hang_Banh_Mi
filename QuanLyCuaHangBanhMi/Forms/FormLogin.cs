using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Controls;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.Services.Abstractions;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormLogin : Form
    {
        private readonly IAuthService _authService;
        private string _selectedRoleCode = RoleCodes.Admin;
        private static string _rememberedUsername = string.Empty;

        // Giữ constructor không tham số để Visual Studio Designer vẫn mở được form.
        public FormLogin() : this(new AuthServiceImpl())
        {
        }

        public FormLogin(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            InitializeComponent();
            AcceptButton = btnLogin;
            DoubleBuffered = true;

            if (!DesignTimeHelper.IsDesignTime(this))
            {
                LoadAssets();
                loginContentPanel.Resize += (_, _) => ApplyResponsiveLoginLayout();
                ApplyResponsiveLoginLayout();
            }
            SetupEventHandlers();
            txtUsername.InputText = _rememberedUsername;
        }

        private void LoadAssets()
        {
            string iconDir = Path.Combine(AppContext.BaseDirectory, "Assets", "Icons");
            string imageDir = Path.Combine(AppContext.BaseDirectory, "Assets", "Images");

            TrySetImage(Path.Combine(imageDir, "poster_rounded.png"), image => picPoster.Image = image);
            TrySetImage(Path.Combine(iconDir, "icon_user_clean.png"), image => txtUsername.LeftIcon = image);
            TrySetImage(Path.Combine(iconDir, "icon_lock_clean.png"), image => txtPassword.LeftIcon = image);
            TrySetImage(Path.Combine(iconDir, "icon_login_clean.png"), image => btnLogin.ButtonIcon = image);
            TrySetImage(Path.Combine(iconDir, "icon_shield_clean.png"), image => btnRoleAdmin.RoleIcon = image);
            TrySetImage(Path.Combine(iconDir, "icon_users_clean.png"), image => btnRoleManager.RoleIcon = image);
            TrySetImage(Path.Combine(iconDir, "icon_store_clean.png"), image => btnRoleStaff.RoleIcon = image);
            TrySetImage(Path.Combine(iconDir, "icon_info_clean.png"), image => picInfoIcon.Image = image);

            string eyeOpen = Path.Combine(iconDir, "icon_eye_clean.png");
            string eyeSlash = Path.Combine(iconDir, "icon_eyeslash_clean.png");
            if (File.Exists(eyeOpen) && File.Exists(eyeSlash))
                txtPassword.SetEyeIcons(Image.FromFile(eyeOpen), Image.FromFile(eyeSlash));

            string appIcon = Path.Combine(iconDir, "app.ico");
            if (File.Exists(appIcon))
                Icon = new Icon(appIcon);
        }

        private static void TrySetImage(string path, Action<Image> setter)
        {
            if (!File.Exists(path)) return;
            try { setter(Image.FromFile(path)); }
            catch { }
        }

        private void SetupEventHandlers()
        {
            btnRoleAdmin.SelectedChanged += (_, _) => SelectRole(btnRoleAdmin, RoleCodes.Admin);
            btnRoleManager.SelectedChanged += (_, _) => SelectRole(btnRoleManager, RoleCodes.Manager);
            btnRoleStaff.SelectedChanged += (_, _) => SelectRole(btnRoleStaff, RoleCodes.Staff);
            btnLogin.Click += btnLogin_Click;
            lnkForgotPassword.Click += (_, _) => ShowPasswordRecoveryInformation();
        }

        private void SelectRole(RoleCardButton selected, string roleCode)
        {
            if (!selected.IsSelected) return;
            btnRoleAdmin.IsSelected = ReferenceEquals(selected, btnRoleAdmin);
            btnRoleManager.IsSelected = ReferenceEquals(selected, btnRoleManager);
            btnRoleStaff.IsSelected = ReferenceEquals(selected, btnRoleStaff);
            _selectedRoleCode = roleCode;
        }

        private void ShowPasswordRecoveryInformation()
        {
            MessageBox.Show(
                "Chức năng tự đặt lại mật khẩu chưa có bảng token/yêu cầu khôi phục trong QuanLyBanhMi. " +
                "Vui lòng liên hệ quản trị viên để xác minh và đặt lại mật khẩu trong Quản lý tài khoản.",
                "Quên mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            txtUsername.SetBounds(fieldLeft, txtUsername.Top, fieldWidth, txtUsername.Height);
            txtPassword.SetBounds(fieldLeft, txtPassword.Top, fieldWidth, txtPassword.Height);
            chkRemember.Left = fieldLeft;
            lnkForgotPassword.Left = fieldLeft + fieldWidth - lnkForgotPassword.Width;
            btnLogin.SetBounds(fieldLeft, btnLogin.Top, fieldWidth, btnLogin.Height);
            divider.SetBounds(fieldLeft, divider.Top, fieldWidth, divider.Height);

            const int roleWidth = 120;
            const int gap = 14;
            int rolesLeft = Math.Max(24, (panelWidth - (roleWidth * 3 + gap * 2)) / 2);
            btnRoleAdmin.Left = rolesLeft;
            btnRoleManager.Left = rolesLeft + roleWidth + gap;
            btnRoleStaff.Left = rolesLeft + (roleWidth + gap) * 2;
            picInfoIcon.Left = fieldLeft + 4;
            lblInfo.Left = picInfoIcon.Right + 10;
            lblInfo.Width = Math.Max(200, fieldLeft + fieldWidth - lblInfo.Left);
        }

        private void btnLogin_Click(object? sender, EventArgs e)
        {
            string username = txtUsername.InputText.Trim();
            string password = txtPassword.InputText;

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

                    case LoginStatus.TemporarilyLocked:
                        MessageBox.Show(
                            "Tài khoản đang bị tạm khóa do đăng nhập sai nhiều lần. Vui lòng thử lại sau.",
                            "Tài khoản tạm khóa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        txtPassword.InputText = string.Empty;
                        return;

                    case LoginStatus.InvalidCredentials:
                        MessageBox.Show(
                            "Tên đăng nhập hoặc mật khẩu không chính xác.",
                            "Đăng nhập thất bại",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        txtPassword.InputText = string.Empty;
                        txtPassword.Focus();
                        return;

                    case LoginStatus.Success:
                        if (!UserSession.RoleName.Equals(_selectedRoleCode, StringComparison.OrdinalIgnoreCase))
                        {
                            UserSession.Clear();
                            MessageBox.Show("Tài khoản không thuộc vai trò đã chọn.", "Vai trò không phù hợp",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtPassword.InputText = string.Empty;
                            return;
                        }
                        _rememberedUsername = chkRemember.Checked ? username : string.Empty;
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
            chkRemember.Enabled = !busy;
            btnRoleAdmin.Enabled = !busy;
            btnRoleManager.Enabled = !busy;
            btnRoleStaff.Enabled = !busy;
            btnLogin.Enabled = !busy;
            btnLogin.Text = busy ? "Đang xác thực..." : "Đăng nhập";
            UseWaitCursor = busy;
        }

        private static void ShowWarning(string message)
        {
            MessageBox.Show(message, "Thông tin chưa đầy đủ",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using var pen = new Pen(Color.FromArgb(22, 161, 82, 26), 1.2f);
            using var brush = new SolidBrush(Color.FromArgb(18, 161, 82, 26));
            DrawWheatStalk(e.Graphics, pen, brush, Width - 18, Height - 30, -55, 6);
            DrawWheatStalk(e.Graphics, pen, brush, 12, Height - 50, 45, 5);
        }

        private static void DrawWheatStalk(Graphics graphics, Pen pen, Brush brush,
            float x, float y, float angle, int grains)
        {
            var state = graphics.Save();
            graphics.TranslateTransform(x, y);
            graphics.RotateTransform(angle);
            graphics.DrawLine(pen, 0, 0, 0, -80);
            for (int i = 0; i < grains; i++)
            {
                float grainY = -15 - i * 11;
                graphics.FillEllipse(brush, -7, grainY - 3, 7, 12);
                graphics.FillEllipse(brush, 0, grainY - 3, 7, 12);
            }
            graphics.Restore(state);
        }
    }
}
