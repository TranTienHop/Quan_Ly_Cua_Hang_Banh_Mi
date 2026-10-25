using System;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class AdministrationDashboardControl : UserControl
    {
        public AdministrationDashboardControl()
        {
            InitializeComponent();
            lblUser.Text = $"{UserSession.FullName} · {UserSession.RoleName}";
            btnHome.Click += (_, _) => ShowHome();
            btnAccounts.Click += (_, _) => ShowSection("SYS_ACCOUNT", "Tài khoản", tabAccounts: true);
            btnRoles.Click += (_, _) => ShowSection("SYS_ROLE", "Vai trò & quyền", tabRoles: true);
            btnBranches.Click += (_, _) => ShowSection("SYS_BRANCH", "Chi nhánh", tabBranches: true);
            btnShifts.Click += (_, _) => ShowSection("SYS_SHIFT", "Ca làm", tabShifts: true);
            ShowHome();
        }

        public void ApplyPermissionsAndLoad()
        {
            btnAccounts.Visible = UserSession.HasPermission("SYS_ACCOUNT");
            btnRoles.Visible = UserSession.HasPermission("SYS_ROLE");
            btnBranches.Visible = UserSession.HasPermission("SYS_BRANCH");
            btnShifts.Visible = UserSession.HasPermission("SYS_SHIFT");
            administration.ApplyPermissionsAndLoad();
        }

        private void ShowHome()
        {
            administration.Visible = false;
            homePanel.Visible = true;
            lblBreadcrumb.Text = "Trang chủ  ›  Quản trị hệ thống";
            SetSelected(btnHome);
        }

        private void ShowSection(string permission, string title,
            bool tabAccounts = false, bool tabRoles = false,
            bool tabBranches = false, bool tabShifts = false)
        {
            if (!UserSession.HasPermission(permission))
            {
                MessageBox.Show("Bạn không có quyền truy cập chức năng này.",
                    "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            administration.NavigateToSection(tabAccounts, tabRoles, tabBranches, tabShifts);
            homePanel.Visible = false;
            administration.Visible = true;
            lblBreadcrumb.Text = $"Trang chủ  ›  {title}";
            SetSelected(tabAccounts ? btnAccounts :
                tabRoles ? btnRoles : tabBranches ? btnBranches : btnShifts);
        }

        private void SetSelected(Button selected)
        {
            foreach (Button button in new[] { btnHome, btnAccounts, btnRoles, btnBranches, btnShifts })
            {
                button.BackColor = ReferenceEquals(button, selected)
                    ? System.Drawing.Color.FromArgb(161, 82, 26)
                    : System.Drawing.Color.FromArgb(117, 58, 18);
            }
        }
    }
}
