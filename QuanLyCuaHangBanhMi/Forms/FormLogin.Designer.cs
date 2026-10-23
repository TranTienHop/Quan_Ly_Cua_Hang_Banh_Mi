namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer? components = null;
        private TableLayoutPanel rootLayout = null!;
        private Panel posterPanel = null!;
        private PictureBox picPoster = null!;
        private Label lblPosterFallback = null!;
        private Panel loginCard = null!;
        private Panel loginContentPanel = null!;
        private Label lblTitle = null!;
        private Label lblSubtitle = null!;
        private Label lblUsername = null!;
        private TextBox txtUsername = null!;
        private Label lblPassword = null!;
        private TextBox txtPassword = null!;
        private CheckBox chkShowPassword = null!;
        private Button btnLogin = null!;
        private Panel pnlDivider = null!;
        private Button btnExit = null!;
        private LinkLabel lnkTestConnection = null!;
        private Label lblConnectionStatus = null!;
        private Label lblRoleHint = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                picPoster?.Image?.Dispose();
                components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            rootLayout = new TableLayoutPanel();
            posterPanel = new Panel();
            picPoster = new PictureBox();
            lblPosterFallback = new Label();
            loginCard = new Panel();
            loginContentPanel = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            chkShowPassword = new CheckBox();
            btnLogin = new Button();
            pnlDivider = new Panel();
            btnExit = new Button();
            lnkTestConnection = new LinkLabel();
            lblConnectionStatus = new Label();
            lblRoleHint = new Label();
            rootLayout.SuspendLayout();
            posterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picPoster).BeginInit();
            loginCard.SuspendLayout();
            loginContentPanel.SuspendLayout();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.ColumnCount = 2;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            rootLayout.Controls.Add(posterPanel, 0, 0);
            rootLayout.Controls.Add(loginCard, 1, 0);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
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
            posterPanel.Controls.Add(lblPosterFallback);
            posterPanel.Dock = DockStyle.Fill;
            posterPanel.Location = new Point(24, 24);
            posterPanel.Margin = new Padding(0, 0, 14, 0);
            posterPanel.MinimumSize = new Size(200, 300);
            posterPanel.Name = "posterPanel";
            posterPanel.Size = new Size(416, 633);
            posterPanel.TabIndex = 0;
            // 
            // picPoster
            // 
            picPoster.BackColor = Color.FromArgb(68, 34, 14);
            picPoster.Dock = DockStyle.Fill;
            picPoster.Location = new Point(0, 0);
            picPoster.Name = "picPoster";
            picPoster.Size = new Size(416, 633);
            picPoster.SizeMode = PictureBoxSizeMode.Zoom;
            picPoster.TabIndex = 0;
            picPoster.TabStop = false;
            // 
            // lblPosterFallback
            // 
            lblPosterFallback.Dock = DockStyle.Fill;
            lblPosterFallback.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblPosterFallback.ForeColor = Color.White;
            lblPosterFallback.Location = new Point(0, 0);
            lblPosterFallback.Name = "lblPosterFallback";
            lblPosterFallback.Padding = new Padding(28);
            lblPosterFallback.Size = new Size(416, 633);
            lblPosterFallback.TabIndex = 1;
            lblPosterFallback.Text = "🥖\r\nBÁNH MÌ NGON\r\n\r\nHệ thống quản lý bán hàng và điều hành kinh doanh";
            lblPosterFallback.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // loginCard
            // 
            loginCard.BackColor = Color.White;
            loginCard.Controls.Add(loginContentPanel);
            loginCard.Dock = DockStyle.Fill;
            loginCard.Location = new Point(468, 24);
            loginCard.Margin = new Padding(14, 0, 0, 0);
            loginCard.MinimumSize = new Size(460, 300);
            loginCard.Name = "loginCard";
            loginCard.Size = new Size(558, 633);
            loginCard.TabIndex = 1;
            // 
            // loginContentPanel
            // 
            loginContentPanel.AutoScroll = true;
            loginContentPanel.Controls.Add(lblTitle);
            loginContentPanel.Controls.Add(lblSubtitle);
            loginContentPanel.Controls.Add(lblUsername);
            loginContentPanel.Controls.Add(txtUsername);
            loginContentPanel.Controls.Add(lblPassword);
            loginContentPanel.Controls.Add(txtPassword);
            loginContentPanel.Controls.Add(chkShowPassword);
            loginContentPanel.Controls.Add(btnLogin);
            loginContentPanel.Controls.Add(pnlDivider);
            loginContentPanel.Controls.Add(btnExit);
            loginContentPanel.Controls.Add(lnkTestConnection);
            loginContentPanel.Controls.Add(lblConnectionStatus);
            loginContentPanel.Controls.Add(lblRoleHint);
            loginContentPanel.Dock = DockStyle.Fill;
            loginContentPanel.Location = new Point(0, 0);
            loginContentPanel.Margin = new Padding(0);
            loginContentPanel.Name = "loginContentPanel";
            loginContentPanel.Size = new Size(558, 633);
            loginContentPanel.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(128, 62, 19);
            lblTitle.Location = new Point(40, 54);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(440, 46);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "ĐĂNG NHẬP HỆ THỐNG";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            lblSubtitle.Font = new Font("Segoe UI", 9.5F);
            lblSubtitle.ForeColor = Color.FromArgb(107, 114, 128);
            lblSubtitle.Location = new Point(40, 104);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(440, 30);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Nhập tài khoản được cấp để tiếp tục";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblUsername.ForeColor = Color.FromArgb(55, 65, 81);
            lblUsername.Location = new Point(64, 166);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(105, 17);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Tên đăng nhập";
            // 
            // txtUsername
            // 
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.Location = new Point(64, 190);
            txtUsername.MaxLength = 50;
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Nhập tên đăng nhập";
            txtUsername.Size = new Size(390, 27);
            txtUsername.TabIndex = 0;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(55, 65, 81);
            lblPassword.Location = new Point(64, 242);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(66, 17);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Mật khẩu";
            // 
            // txtPassword
            // 
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(64, 266);
            txtPassword.MaxLength = 255;
            txtPassword.Name = "txtPassword";
            txtPassword.PlaceholderText = "Nhập mật khẩu";
            txtPassword.Size = new Size(390, 27);
            txtPassword.TabIndex = 1;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // chkShowPassword
            // 
            chkShowPassword.AutoSize = true;
            chkShowPassword.Font = new Font("Segoe UI", 9F);
            chkShowPassword.ForeColor = Color.FromArgb(75, 85, 99);
            chkShowPassword.Location = new Point(64, 309);
            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Size = new Size(108, 19);
            chkShowPassword.TabIndex = 2;
            chkShowPassword.Text = "Hiện mật khẩu";
            chkShowPassword.UseVisualStyleBackColor = true;
            chkShowPassword.CheckedChanged += chkShowPassword_CheckedChanged;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(174, 88, 24);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(64, 348);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(390, 50);
            btnLogin.TabIndex = 3;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // pnlDivider
            // 
            pnlDivider.BackColor = Color.FromArgb(229, 231, 235);
            pnlDivider.Location = new Point(64, 427);
            pnlDivider.Name = "pnlDivider";
            pnlDivider.Size = new Size(390, 1);
            pnlDivider.TabIndex = 8;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.White;
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatAppearance.BorderColor = Color.FromArgb(209, 213, 219);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 10F);
            btnExit.ForeColor = Color.FromArgb(55, 65, 81);
            btnExit.Location = new Point(64, 452);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(390, 42);
            btnExit.TabIndex = 4;
            btnExit.Text = "Thoát";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // lnkTestConnection
            // 
            lnkTestConnection.AutoSize = true;
            lnkTestConnection.Font = new Font("Segoe UI", 9F);
            lnkTestConnection.LinkColor = Color.FromArgb(37, 99, 235);
            lnkTestConnection.Location = new Point(337, 513);
            lnkTestConnection.Name = "lnkTestConnection";
            lnkTestConnection.Size = new Size(117, 15);
            lnkTestConnection.TabIndex = 5;
            lnkTestConnection.TabStop = true;
            lnkTestConnection.Text = "Kiểm tra kết nối SQL";
            lnkTestConnection.LinkClicked += lnkTestConnection_LinkClicked;
            // 
            // lblConnectionStatus
            // 
            lblConnectionStatus.Font = new Font("Segoe UI", 8.5F);
            lblConnectionStatus.ForeColor = Color.FromArgb(107, 114, 128);
            lblConnectionStatus.Location = new Point(64, 511);
            lblConnectionStatus.Name = "lblConnectionStatus";
            lblConnectionStatus.Size = new Size(245, 20);
            lblConnectionStatus.TabIndex = 11;
            lblConnectionStatus.Text = "CSDL: QuanLyBanhMi";
            // 
            // lblRoleHint
            // 
            lblRoleHint.Font = new Font("Segoe UI", 8.75F);
            lblRoleHint.ForeColor = Color.FromArgb(132, 87, 52);
            lblRoleHint.Location = new Point(64, 552);
            lblRoleHint.Name = "lblRoleHint";
            lblRoleHint.Size = new Size(390, 42);
            lblRoleHint.TabIndex = 12;
            lblRoleHint.Text = "Vai trò, chi nhánh và quyền truy cập được tải tự động từ tài khoản sau khi xác thực.";
            lblRoleHint.TextAlign = ContentAlignment.MiddleCenter;
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
            Load += FormLogin_Load;
            rootLayout.ResumeLayout(false);
            posterPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picPoster).EndInit();
            loginCard.ResumeLayout(false);
            loginContentPanel.ResumeLayout(false);
            loginContentPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}
