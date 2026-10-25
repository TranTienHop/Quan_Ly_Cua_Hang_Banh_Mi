namespace QuanLyCuaHangBanhMi.Forms
{
    partial class AdministrationDashboardControl
    {
        private System.ComponentModel.IContainer components = null!;
        private System.Windows.Forms.TableLayoutPanel rootLayout = null!;
        private System.Windows.Forms.Panel sidebar = null!;
        private System.Windows.Forms.Panel brandPanel = null!;
        private System.Windows.Forms.Label brandLabel = null!;
        private System.Windows.Forms.Button btnHome = null!;
        private System.Windows.Forms.Button btnAccounts = null!;
        private System.Windows.Forms.Button btnRoles = null!;
        private System.Windows.Forms.Button btnBranches = null!;
        private System.Windows.Forms.Button btnShifts = null!;
        private System.Windows.Forms.TableLayoutPanel contentLayout = null!;
        private System.Windows.Forms.Panel topbar = null!;
        private System.Windows.Forms.Label lblBreadcrumb = null!;
        private System.Windows.Forms.Label lblUser = null!;
        private System.Windows.Forms.Panel contentHost = null!;
        private System.Windows.Forms.Panel homePanel = null!;
        private System.Windows.Forms.Label lblWelcome = null!;
        private System.Windows.Forms.Label lblSubtitle = null!;
        private AdministrationControl administration = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            rootLayout = new System.Windows.Forms.TableLayoutPanel();
            sidebar = new System.Windows.Forms.Panel();
            brandPanel = new System.Windows.Forms.Panel();
            brandLabel = new System.Windows.Forms.Label();
            btnHome = CreateNavigationButton("Tổng quan");
            btnAccounts = CreateNavigationButton("Quản lý tài khoản");
            btnRoles = CreateNavigationButton("Phân quyền vai trò");
            btnBranches = CreateNavigationButton("Quản lý chi nhánh");
            btnShifts = CreateNavigationButton("Quản lý ca làm");
            contentLayout = new System.Windows.Forms.TableLayoutPanel();
            topbar = new System.Windows.Forms.Panel();
            lblBreadcrumb = new System.Windows.Forms.Label();
            lblUser = new System.Windows.Forms.Label();
            contentHost = new System.Windows.Forms.Panel();
            homePanel = new System.Windows.Forms.Panel();
            lblWelcome = new System.Windows.Forms.Label();
            lblSubtitle = new System.Windows.Forms.Label();
            administration = new AdministrationControl();
            rootLayout.SuspendLayout();
            sidebar.SuspendLayout();
            brandPanel.SuspendLayout();
            contentLayout.SuspendLayout();
            topbar.SuspendLayout();
            contentHost.SuspendLayout();
            homePanel.SuspendLayout();
            SuspendLayout();
            //
            // rootLayout
            //
            rootLayout.ColumnCount = 2;
            rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            rootLayout.Controls.Add(sidebar, 0, 0);
            rootLayout.Controls.Add(contentLayout, 1, 0);
            rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            rootLayout.RowCount = 1;
            rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //
            // sidebar
            //
            sidebar.BackColor = System.Drawing.Color.FromArgb(117, 58, 18);
            sidebar.Controls.Add(btnShifts);
            sidebar.Controls.Add(btnBranches);
            sidebar.Controls.Add(btnRoles);
            sidebar.Controls.Add(btnAccounts);
            sidebar.Controls.Add(btnHome);
            sidebar.Controls.Add(brandPanel);
            sidebar.Dock = System.Windows.Forms.DockStyle.Fill;
            //
            // brandPanel
            //
            brandPanel.Controls.Add(brandLabel);
            brandPanel.Dock = System.Windows.Forms.DockStyle.Top;
            brandPanel.Height = 96;
            brandLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            brandLabel.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            brandLabel.ForeColor = System.Drawing.Color.White;
            brandLabel.Text = "BÁNH MÌ\\nQUẢN TRỊ";
            brandLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // navigation buttons
            //
            foreach (System.Windows.Forms.Button button in new[] { btnHome, btnAccounts, btnRoles, btnBranches, btnShifts })
            {
                button.Dock = System.Windows.Forms.DockStyle.Top;
                button.Height = 48;
                button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.BackColor = System.Drawing.Color.FromArgb(117, 58, 18);
                button.ForeColor = System.Drawing.Color.White;
                button.Font = new System.Drawing.Font("Segoe UI", 9.5F);
                button.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                button.Padding = new System.Windows.Forms.Padding(18, 0, 4, 0);
                button.Cursor = System.Windows.Forms.Cursors.Hand;
            }
            //
            // contentLayout
            //
            contentLayout.ColumnCount = 1;
            contentLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            contentLayout.Controls.Add(topbar, 0, 0);
            contentLayout.Controls.Add(contentHost, 0, 1);
            contentLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            contentLayout.RowCount = 2;
            contentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            contentLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            //
            // topbar
            //
            topbar.BackColor = System.Drawing.Color.White;
            topbar.Controls.Add(lblBreadcrumb);
            topbar.Controls.Add(lblUser);
            topbar.Dock = System.Windows.Forms.DockStyle.Fill;
            lblBreadcrumb.AutoSize = true;
            lblBreadcrumb.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblBreadcrumb.Location = new System.Drawing.Point(20, 20);
            lblBreadcrumb.Text = "Trang chủ  ›  Quản trị hệ thống";
            lblUser.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            lblUser.AutoSize = true;
            lblUser.Location = new System.Drawing.Point(750, 21);
            lblUser.Text = "Quản trị viên";
            //
            // contentHost
            //
            contentHost.BackColor = System.Drawing.Color.FromArgb(248, 245, 240);
            contentHost.Controls.Add(administration);
            contentHost.Controls.Add(homePanel);
            contentHost.Dock = System.Windows.Forms.DockStyle.Fill;
            contentHost.Padding = new System.Windows.Forms.Padding(18);
            //
            // homePanel
            //
            homePanel.BackColor = System.Drawing.Color.White;
            homePanel.Controls.Add(lblSubtitle);
            homePanel.Controls.Add(lblWelcome);
            homePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            lblWelcome.Location = new System.Drawing.Point(32, 38);
            lblWelcome.Text = "Quản trị hệ thống";
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            lblSubtitle.Location = new System.Drawing.Point(36, 92);
            lblSubtitle.Text = "Chọn một chức năng ở thanh điều hướng để bắt đầu.";
            //
            // administration
            //
            administration.Dock = System.Windows.Forms.DockStyle.Fill;
            administration.Visible = false;
            //
            // AdministrationDashboardControl
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(rootLayout);
            Name = "AdministrationDashboardControl";
            Size = new System.Drawing.Size(1180, 650);
            rootLayout.ResumeLayout(false);
            sidebar.ResumeLayout(false);
            brandPanel.ResumeLayout(false);
            contentLayout.ResumeLayout(false);
            topbar.ResumeLayout(false);
            topbar.PerformLayout();
            contentHost.ResumeLayout(false);
            homePanel.ResumeLayout(false);
            homePanel.PerformLayout();
            ResumeLayout(false);
        }

        private static System.Windows.Forms.Button CreateNavigationButton(string text) =>
            new() { Text = text };
    }
}
