namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null!;
        private TableLayoutPanel rootLayout = null!;
        private Panel posterPanel = null!;
        private PictureBox picPoster = null!;
        private Label lblBrand = null!;
        private Label lblBrandSub = null!;
        private Panel loginCard = null!;
        private TableLayoutPanel loginContentHost = null!;
        private Panel loginContentPanel = null!;
        private Label lblTitle = null!;
        private Label lblSubtitle = null!;
        private QuanLyCuaHangBanhMi.Controls.CustomInputBox txtUsername = null!;
        private QuanLyCuaHangBanhMi.Controls.CustomInputBox txtPassword = null!;
        private QuanLyCuaHangBanhMi.Controls.ModernCheckBox chkRemember = null!;
        private LinkLabel lnkForgotPassword = null!;
        private QuanLyCuaHangBanhMi.Controls.ModernButton btnLogin = null!;
        private QuanLyCuaHangBanhMi.Controls.DividerLabel divider = null!;
        private QuanLyCuaHangBanhMi.Controls.RoleCardButton btnRoleAdmin = null!;
        private QuanLyCuaHangBanhMi.Controls.RoleCardButton btnRoleManager = null!;
        private QuanLyCuaHangBanhMi.Controls.RoleCardButton btnRoleStaff = null!;
        private PictureBox picInfoIcon = null!;
        private Label lblInfo = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                picPoster?.Image?.Dispose();
                picInfoIcon?.Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            rootLayout = new TableLayoutPanel();
            posterPanel = new Panel();
            picPoster = new PictureBox();
            lblBrandSub = new Label();
            lblBrand = new Label();
            loginCard = new Panel();
            loginContentHost = new TableLayoutPanel();
            loginContentPanel = new Panel();
            picInfoIcon = new PictureBox();
            lblInfo = new Label();
            btnRoleStaff = new QuanLyCuaHangBanhMi.Controls.RoleCardButton();
            btnRoleManager = new QuanLyCuaHangBanhMi.Controls.RoleCardButton();
            btnRoleAdmin = new QuanLyCuaHangBanhMi.Controls.RoleCardButton();
            divider = new QuanLyCuaHangBanhMi.Controls.DividerLabel();
            btnLogin = new QuanLyCuaHangBanhMi.Controls.ModernButton();
            lnkForgotPassword = new LinkLabel();
            chkRemember = new QuanLyCuaHangBanhMi.Controls.ModernCheckBox();
            txtPassword = new QuanLyCuaHangBanhMi.Controls.CustomInputBox();
            txtUsername = new QuanLyCuaHangBanhMi.Controls.CustomInputBox();
            lblSubtitle = new Label();
            lblTitle = new Label();
            rootLayout.SuspendLayout();
            posterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPoster).BeginInit();
            loginCard.SuspendLayout();
            loginContentHost.SuspendLayout();
            loginContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picInfoIcon).BeginInit();
            SuspendLayout();
            //
            // rootLayout
            //
            rootLayout.AutoScroll = true;
            rootLayout.ColumnCount = 2;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            rootLayout.Controls.Add(posterPanel, 0, 0);
            rootLayout.Controls.Add(loginCard, 1, 0);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Margin = new Padding(0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(24);
            rootLayout.RowCount = 1;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Size = new Size(1050, 681);
            rootLayout.TabIndex = 0;
            //
            // posterPanel
            //
            posterPanel.BackColor = Color.FromArgb(68, 34, 14);
            posterPanel.Controls.Add(picPoster);
            posterPanel.Controls.Add(lblBrandSub);
            posterPanel.Controls.Add(lblBrand);
            posterPanel.Dock = DockStyle.Fill;
            posterPanel.Margin = new Padding(0, 0, 14, 0);
            posterPanel.MinimumSize = new Size(200, 300);
            posterPanel.Name = "posterPanel";
            posterPanel.TabIndex = 1;
            //
            // picPoster
            //
            picPoster.BackColor = Color.FromArgb(68, 34, 14);
            picPoster.Dock = DockStyle.Fill;
            picPoster.Name = "picPoster";
            picPoster.SizeMode = PictureBoxSizeMode.Zoom;
            picPoster.TabIndex = 0;
            picPoster.TabStop = false;
            //
            // lblBrandSub
            //
            lblBrandSub.AutoSize = true;
            lblBrandSub.BackColor = Color.Transparent;
            lblBrandSub.Font = new Font("Segoe UI", 10F);
            lblBrandSub.ForeColor = Color.FromArgb(255, 231, 204);
            lblBrandSub.Location = new Point(31, 80);
            lblBrandSub.Name = "lblBrandSub";
            lblBrandSub.Text = "Hệ thống quản lý cửa hàng";
            //
            // lblBrand
            //
            lblBrand.AutoSize = true;
            lblBrand.BackColor = Color.Transparent;
            lblBrand.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblBrand.ForeColor = Color.White;
            lblBrand.Location = new Point(28, 36);
            lblBrand.Name = "lblBrand";
            lblBrand.Text = "BÁNH MÌ NGON";
            //
            // loginCard
            //
            loginCard.BackColor = Color.White;
            loginCard.Controls.Add(loginContentHost);
            loginCard.Dock = DockStyle.Fill;
            loginCard.Margin = new Padding(14, 0, 0, 0);
            loginCard.MinimumSize = new Size(460, 300);
            loginCard.Name = "loginCard";
            loginCard.TabIndex = 0;
            //
            // loginContentHost
            //
            loginContentHost.ColumnCount = 1;
            loginContentHost.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            loginContentHost.Controls.Add(loginContentPanel, 0, 0);
            loginContentHost.Dock = DockStyle.Fill;
            loginContentHost.Margin = new Padding(0);
            loginContentHost.Name = "loginContentHost";
            loginContentHost.RowCount = 1;
            loginContentHost.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            loginContentHost.TabIndex = 0;
            //
            // loginContentPanel
            //
            loginContentPanel.AutoScroll = true;
            loginContentPanel.Controls.Add(picInfoIcon);
            loginContentPanel.Controls.Add(lblInfo);
            loginContentPanel.Controls.Add(btnRoleStaff);
            loginContentPanel.Controls.Add(btnRoleManager);
            loginContentPanel.Controls.Add(btnRoleAdmin);
            loginContentPanel.Controls.Add(divider);
            loginContentPanel.Controls.Add(btnLogin);
            loginContentPanel.Controls.Add(lnkForgotPassword);
            loginContentPanel.Controls.Add(chkRemember);
            loginContentPanel.Controls.Add(txtPassword);
            loginContentPanel.Controls.Add(txtUsername);
            loginContentPanel.Controls.Add(lblSubtitle);
            loginContentPanel.Controls.Add(lblTitle);
            loginContentPanel.Dock = DockStyle.Fill;
            loginContentPanel.Margin = new Padding(0);
            loginContentPanel.Name = "loginContentPanel";
            loginContentPanel.TabIndex = 0;
            //
            // title and subtitle
            //
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(128, 62, 19);
            lblTitle.Location = new Point(40, 62);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(440, 46);
            lblTitle.Text = "ĐĂNG NHẬP HỆ THỐNG";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Font = new Font("Segoe UI", 9.5F);
            lblSubtitle.ForeColor = Color.FromArgb(135, 120, 110);
            lblSubtitle.Location = new Point(40, 118);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(440, 30);
            lblSubtitle.Text = "Vui lòng nhập tài khoản và mật khẩu để tiếp tục";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            //
            // txtUsername
            //
            txtUsername.BackColor = Color.White;
            txtUsername.InputText = "";
            txtUsername.IsPassword = false;
            txtUsername.LeftIcon = null;
            txtUsername.Location = new Point(64, 184);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Tên đăng nhập";
            txtUsername.ShowPasswordToggle = false;
            txtUsername.Size = new Size(390, 52);
            txtUsername.TabIndex = 0;
            //
            // txtPassword
            //
            txtPassword.BackColor = Color.White;
            txtPassword.InputText = "";
            txtPassword.IsPassword = true;
            txtPassword.LeftIcon = null;
            txtPassword.Location = new Point(64, 248);
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Mật khẩu";
            txtPassword.ShowPasswordToggle = true;
            txtPassword.Size = new Size(390, 52);
            txtPassword.TabIndex = 1;
            //
            // remember and forgot
            //
            chkRemember.BackColor = Color.Transparent;
            chkRemember.BoxCheckedColor = Color.FromArgb(37, 99, 235);
            chkRemember.Checked = true;
            chkRemember.Font = new Font("Segoe UI", 9.5F);
            chkRemember.Location = new Point(64, 312);
            chkRemember.Name = "chkRemember";
            chkRemember.Size = new Size(170, 26);
            chkRemember.TabIndex = 2;
            lnkForgotPassword.AutoSize = true;
            lnkForgotPassword.Font = new Font("Segoe UI", 9F);
            lnkForgotPassword.LinkColor = Color.FromArgb(37, 99, 235);
            lnkForgotPassword.Location = new Point(338, 316);
            lnkForgotPassword.Name = "lnkForgotPassword";
            lnkForgotPassword.Text = "Quên mật khẩu?";
            lnkForgotPassword.TabIndex = 3;
            //
            // btnLogin
            //
            btnLogin.BackColor = Color.Transparent;
            btnLogin.BorderColor = Color.Transparent;
            btnLogin.BorderRadius = 8;
            btnLogin.BorderWidth = 0;
            btnLogin.ButtonIcon = null;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.HoverColor = Color.FromArgb(140, 70, 20);
            btnLogin.Location = new Point(64, 352);
            btnLogin.Name = "btnLogin";
            btnLogin.NormalColor = Color.FromArgb(174, 88, 24);
            btnLogin.PressedColor = Color.FromArgb(115, 53, 12);
            btnLogin.Size = new Size(390, 52);
            btnLogin.TabIndex = 4;
            btnLogin.Text = "Đăng nhập";
            //
            // divider and role cards
            //
            divider.BackColor = Color.Transparent;
            divider.LineColor = Color.FromArgb(229, 231, 235);
            divider.Location = new Point(64, 420);
            divider.Name = "divider";
            divider.Size = new Size(390, 26);
            divider.Text = "hoặc chọn vai trò";
            divider.TextColor = Color.FromArgb(156, 163, 175);
            btnRoleAdmin.Font = new Font("Segoe UI", 9.5F);
            btnRoleAdmin.IsSelected = true;
            btnRoleAdmin.Location = new Point(64, 460);
            btnRoleAdmin.Name = "btnRoleAdmin";
            btnRoleAdmin.RoleName = "Quản trị viên";
            btnRoleAdmin.Size = new Size(120, 80);
            btnRoleAdmin.TabIndex = 5;
            btnRoleManager.Font = new Font("Segoe UI", 9.5F);
            btnRoleManager.IsSelected = false;
            btnRoleManager.Location = new Point(198, 460);
            btnRoleManager.Name = "btnRoleManager";
            btnRoleManager.RoleName = "Quản lý";
            btnRoleManager.Size = new Size(120, 80);
            btnRoleManager.TabIndex = 6;
            btnRoleStaff.Font = new Font("Segoe UI", 9.5F);
            btnRoleStaff.IsSelected = false;
            btnRoleStaff.Location = new Point(332, 460);
            btnRoleStaff.Name = "btnRoleStaff";
            btnRoleStaff.RoleName = "Nhân viên";
            btnRoleStaff.Size = new Size(120, 80);
            btnRoleStaff.TabIndex = 7;
            //
            // info
            //
            picInfoIcon.Location = new Point(68, 576);
            picInfoIcon.Name = "picInfoIcon";
            picInfoIcon.Size = new Size(22, 22);
            picInfoIcon.SizeMode = PictureBoxSizeMode.Zoom;
            picInfoIcon.TabStop = false;
            lblInfo.Font = new Font("Segoe UI", 8.75F);
            lblInfo.ForeColor = Color.FromArgb(132, 87, 52);
            lblInfo.Location = new Point(100, 573);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(352, 38);
            lblInfo.Text = "Liên hệ quản trị viên nếu bạn không có tài khoản đăng nhập.";
            lblInfo.TextAlign = ContentAlignment.MiddleLeft;
            //
            // FormLogin
            //
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(250, 247, 242);
            ClientSize = new Size(1050, 681);
            Controls.Add(rootLayout);
            Font = new Font("Segoe UI", 9.5F);
            FormBorderStyle = FormBorderStyle.Sizable;
            MaximizeBox = true;
            MinimumSize = new Size(900, 650);
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập hệ thống – Bánh Mì Ngon";
            rootLayout.ResumeLayout(false);
            posterPanel.ResumeLayout(false);
            posterPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picPoster).EndInit();
            loginCard.ResumeLayout(false);
            loginContentHost.ResumeLayout(false);
            loginContentPanel.ResumeLayout(false);
            loginContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picInfoIcon).EndInit();
            ResumeLayout(false);
        }
    }
}
