using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Data;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormMain : Form
    {
        private readonly CultureInfo _vnCulture = new CultureInfo("vi-VN");
        private readonly int _startTab;

        /// <summary>true khi người dùng đăng xuất (Program quay lại màn hình đăng nhập).</summary>
        public bool IsLoggedOut { get; private set; }

        /// <param name="startTab">0=Tổng quan, 1=Thực đơn, 2=Khách, 3=KM, 4=Kho, 5=Báo cáo, 6=Nhân viên</param>
        public FormMain(int startTab = 0)
        {
            _startTab = Math.Clamp(startTab, 0, 6);
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            lblUserInfo.Text = $"Xin chào: {UserSession.FullName} ({UserSession.RoleName})";

            if (!UserSession.IsAdmin)
            {
                btnNavEmployees.Visible = false;
            }

            if (!UserSession.IsManagerOrAdmin)
            {
                btnNavReports.Visible = false;
            }

            // STAFF chỉ xem/sửa khách; quản lý chương trình KM dành cho MANAGER/ADMIN
            if (!UserSession.IsManagerOrAdmin)
                btnNavPromos.Visible = false;

            LoadDashboardTab();
            LoadProductsTab();
            LoadCustomersTab();
            LoadPromosTab();
            LoadIngredientsTab();
            LoadReportsTab();
            LoadEmployeesTab();

            // Sau đăng nhập luôn vào Dashboard; chỉ nhảy tab khác khi được yêu cầu rõ ràng
            NavigateToTab(_startTab);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            IsLoggedOut = true;
            Close();
        }

        private void btnHamburger_Click(object? sender, EventArgs e)
        {
            // Mọi trang (kể cả Dashboard): ☰ vẫn đóng/mở sidebar được.
            panelSidebar.Visible = !panelSidebar.Visible;
            PerformLayout();
        }

        private void btnNav_Click(object sender, EventArgs e)
        {
            if (sender == btnNavPOS)
            {
                panelSidebar.Visible = false;
                OpenOrderScreen();
                return;
            }

            if (sender == btnNavDashboard) NavigateToTab(0);
            else if (sender == btnNavProducts) NavigateToTab(1);
            else if (sender == btnNavCustomers) NavigateToTab(2);
            else if (sender == btnNavPromos) NavigateToTab(3);
            else if (sender == btnNavIngredients) NavigateToTab(4);
            else if (sender == btnNavReports) NavigateToTab(5);
            else if (sender == btnNavEmployees) NavigateToTab(6);
        }

        /// <summary>0=Tổng quan, 1=Thực đơn, 2=Khách, 3=KM, 4=Kho, 5=Báo cáo, 6=Nhân viên</summary>
        public void NavigateToTab(int tabIndex)
        {
            var navButtons = new[]
            {
                btnNavDashboard, btnNavProducts, btnNavCustomers, btnNavPromos,
                btnNavIngredients, btnNavReports, btnNavEmployees
            };
            int tab = Math.Clamp(tabIndex, 0, navButtons.Length - 1);
            if (!UserSession.IsAdmin && tab == 6) tab = 0;
            if (!UserSession.IsManagerOrAdmin && tab == 5) tab = 0;
            if (!UserSession.IsManagerOrAdmin && tab == 3) tab = 0;
            if (!navButtons[tab].Visible) tab = 0;
            SwitchTab(tab, navButtons[tab]);
            if (tab == 0)
            {
                LoadDashboardTab();
                EnsureDashboardSidebarOpen();
            }
            else
            {
                // Module khác: đóng sidebar (mở lại bằng ☰)
                panelSidebar.Visible = false;
                PerformLayout();
            }
        }

        /// <summary>Trang Tổng quan luôn giữ sidebar mở.</summary>
        private void EnsureDashboardSidebarOpen()
        {
            if (!panelSidebar.Visible)
            {
                panelSidebar.Visible = true;
                PerformLayout();
            }
        }

        private void OpenOrderScreen()
        {
            using var orderForm = new FormOrder();
            Hide();
            orderForm.ShowDialog(this);

            if (orderForm.IsLoggedOut)
            {
                IsLoggedOut = true;
                Close();
                return;
            }

            Show();
            Activate();

            if (orderForm.RequestedAdminTab is int tab)
                NavigateToTab(tab);
            else
                NavigateToTab(0);

            LoadDashboardTab();
            LoadProductsTab();
            LoadCustomersTab();
            LoadPromosTab();
            LoadIngredientsTab();
            LoadReportsTab();
            LoadEmployeesTab();
        }

        private void SwitchTab(int tabIndex, Button activeBtn)
        {
            tabMain.SelectedIndex = tabIndex;
            SetNavButtonStyle(btnNavPOS, false);

            var buttons = new[]
            {
                btnNavDashboard, btnNavProducts, btnNavCustomers, btnNavPromos,
                btnNavIngredients, btnNavReports, btnNavEmployees
            };
            foreach (var btn in buttons)
                SetNavButtonStyle(btn, btn == activeBtn);
        }

        private static void SetNavButtonStyle(Button btn, bool active)
        {
            if (active)
            {
                btn.BackColor = Color.FromArgb(230, 126, 34);
                btn.ForeColor = Color.White;
                btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            }
            else
            {
                btn.BackColor = Color.FromArgb(44, 62, 80);
                btn.ForeColor = Color.Gainsboro;
                btn.Font = new Font("Segoe UI", 11F, FontStyle.Regular);
            }
        }

        #region ======================== 0. DASHBOARD TỔNG QUAN ========================

        private Label lblDashWelcome = null!;
        private Label lblDashClock = null!;
        private Label lblDashRevenue = null!;
        private Label lblDashOrders = null!;
        private Label lblDashDiscount = null!;
        private Label lblDashAov = null!;
        private Label lblDashLowStock = null!;
        private DataGridView dgvDashTopProducts = null!;
        private readonly System.Windows.Forms.Timer _dashClockTimer = new() { Interval = 1000 };

        private void InitializeDashboardTab()
        {
            // Nền xám nhạt, góc vuông, không đổ bóng
            Color pageBg = Color.FromArgb(244, 246, 248);
            Color cardBorder = Color.FromArgb(220, 225, 230);
            tabDashboard.BackColor = pageBg;
            tabDashboard.Padding = new Padding(0);
            tabDashboard.Controls.Clear();

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                BackColor = pageBg,
                Padding = new Padding(20, 16, 20, 16)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 78));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 108));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 168));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            tabDashboard.Controls.Add(root);

            // ----- Header: chào + đồng hồ + Làm mới -----
            var pnlHeader = new Panel { Dock = DockStyle.Fill, BackColor = pageBg };
            root.Controls.Add(pnlHeader, 0, 0);

            lblDashWelcome = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(33, 37, 41),
                Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold),
                Location = new Point(0, 4),
                Text = "Xin chào!"
            };
            lblDashClock = new Label
            {
                AutoSize = true,
                ForeColor = Color.FromArgb(108, 117, 125),
                Font = new Font("Segoe UI", 10F),
                Location = new Point(2, 44),
                Text = ""
            };
            var btnRefreshDash = new Button
            {
                Text = "⟳  Làm mới",
                Size = new Size(118, 36),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(pnlHeader.Width - 118, 18),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(55, 65, 75),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand
            };
            btnRefreshDash.FlatAppearance.BorderColor = cardBorder;
            btnRefreshDash.FlatAppearance.BorderSize = 1;
            btnRefreshDash.Click += (s, e) => LoadDashboardTab();
            pnlHeader.Resize += (s, e) => btnRefreshDash.Left = Math.Max(280, pnlHeader.Width - btnRefreshDash.Width);
            pnlHeader.Controls.AddRange(new Control[] { lblDashWelcome, lblDashClock, btnRefreshDash });

            // ----- 5 KPI cards (trắng + icon màu, góc vuông) -----
            var tblKpis = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = pageBg,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            for (int i = 0; i < 5; i++)
                tblKpis.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            root.Controls.Add(tblKpis, 0, 1);

            lblDashRevenue = CreateDashKpiCard(tblKpis, 0, "💰", "DOANH THU HÔM NAY", "0 đ",
                Color.FromArgb(39, 174, 96));
            lblDashOrders = CreateDashKpiCard(tblKpis, 1, "🧾", "ĐƠN HOÀN THÀNH", "0",
                Color.FromArgb(52, 152, 219));
            lblDashDiscount = CreateDashKpiCard(tblKpis, 2, "🏷", "GIẢM GIÁ", "0 đ",
                Color.FromArgb(241, 196, 15));
            lblDashAov = CreateDashKpiCard(tblKpis, 3, "📈", "TB / ĐƠN", "0 đ",
                Color.FromArgb(155, 89, 182));
            lblDashLowStock = CreateDashKpiCard(tblKpis, 4, "⚠", "NL SẮP HẾT", "0",
                Color.FromArgb(231, 76, 60));

            // ----- Truy cập nhanh -----
            var pnlQuick = new Panel { Dock = DockStyle.Fill, BackColor = pageBg, Padding = new Padding(0, 8, 0, 0) };
            root.Controls.Add(pnlQuick, 0, 2);

            var lblQuick = new Label
            {
                Text = "TRUY CẬP NHANH",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            pnlQuick.Controls.Add(lblQuick);

            var tblQuick = new TableLayoutPanel
            {
                Location = new Point(0, 28),
                Height = 120,
                ColumnCount = 5,
                RowCount = 1,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = pageBg
            };
            for (int i = 0; i < 5; i++)
                tblQuick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20f));
            pnlQuick.Controls.Add(tblQuick);
            pnlQuick.Resize += (s, e) => tblQuick.Width = Math.Max(400, pnlQuick.ClientSize.Width);

            void AddQuick(int col, string icon, string caption, Color bg, Action action)
            {
                var b = CreateDashShortcut(icon, caption, bg, action);
                b.Margin = new Padding(col == 0 ? 0 : 4, 0, col == 4 ? 0 : 4, 0);
                tblQuick.Controls.Add(b, col, 0);
            }

            AddQuick(0, "🛒", "Bán hàng", Color.FromArgb(230, 126, 34), () =>
            {
                panelSidebar.Visible = false;
                OpenOrderScreen();
            });
            AddQuick(1, "🥪", "Thực đơn", Color.FromArgb(52, 73, 94), () => NavigateToTab(1));
            AddQuick(2, "👤", "Khách hàng", Color.FromArgb(52, 152, 219), () => NavigateToTab(2));
            AddQuick(3, "🥬", "Kho NL", Color.FromArgb(39, 174, 96), () => NavigateToTab(4));
            if (UserSession.IsManagerOrAdmin)
                AddQuick(4, "📊", "Báo cáo", Color.FromArgb(142, 68, 173), () => NavigateToTab(5));
            else
                AddQuick(4, "🎁", "Khuyến mãi", Color.FromArgb(142, 68, 173), () => NavigateToTab(3));

            // ----- Bảng top món -----
            var pnlTop = new Panel { Dock = DockStyle.Fill, BackColor = pageBg, Padding = new Padding(0, 4, 0, 0) };
            root.Controls.Add(pnlTop, 0, 3);

            var lblTop = new Label
            {
                Text = "TOP MÓN BÁN CHẠY HÔM NAY",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            pnlTop.Controls.Add(lblTop);

            var pnlGridHost = new Panel
            {
                Location = new Point(0, 28),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlTop.Controls.Add(pnlGridHost);
            pnlTop.Resize += (s, e) =>
            {
                pnlGridHost.Width = Math.Max(200, pnlTop.ClientSize.Width);
                pnlGridHost.Height = Math.Max(80, pnlTop.ClientSize.Height - 28);
            };

            dgvDashTopProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 233, 237),
                Font = new Font("Segoe UI", 9.5F),
                RowHeadersVisible = false,
                ColumnHeadersHeight = 36,
                RowTemplate = { Height = 32 }
            };
            dgvDashTopProducts.EnableHeadersVisualStyles = false;
            dgvDashTopProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvDashTopProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(236, 239, 241);
            dgvDashTopProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(55, 65, 75);
            dgvDashTopProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            dgvDashTopProducts.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 239, 241);
            dgvDashTopProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 240, 254);
            dgvDashTopProducts.DefaultCellStyle.SelectionForeColor = Color.FromArgb(33, 37, 41);
            dgvDashTopProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            pnlGridHost.Controls.Add(dgvDashTopProducts);

            _dashClockTimer.Tick -= DashClockTick;
            _dashClockTimer.Tick += DashClockTick;
            _dashClockTimer.Start();
        }

        private void DashClockTick(object? sender, EventArgs e)
        {
            if (lblDashClock == null) return;
            string role = string.IsNullOrWhiteSpace(UserSession.RoleName) ? "" : " · " + UserSession.RoleName;
            lblDashClock.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy · HH:mm:ss", _vnCulture) + role;
        }

        /// <summary>Thẻ KPI trắng, icon màu bên trái — góc vuông, viền mỏng, không bóng.</summary>
        private static Label CreateDashKpiCard(
            TableLayoutPanel parent, int col, string icon, string title, string value, Color accent)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(col == 0 ? 0 : 6, 0, col == 4 ? 0 : 6, 0),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(0)
            };

            var pnlIcon = new Panel
            {
                Size = new Size(44, 44),
                Location = new Point(12, 24),
                BackColor = accent
            };
            var lblIcon = new Label
            {
                Text = icon,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI Emoji", 14F),
                ForeColor = Color.White
            };
            pnlIcon.Controls.Add(lblIcon);

            var lblT = new Label
            {
                Text = title,
                AutoSize = true,
                ForeColor = Color.FromArgb(108, 117, 125),
                Font = new Font("Segoe UI", 8F, FontStyle.Bold),
                Location = new Point(66, 18)
            };
            var lblV = new Label
            {
                Text = value,
                AutoSize = true,
                ForeColor = accent,
                Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold),
                Location = new Point(66, 42)
            };

            card.Controls.AddRange(new Control[] { pnlIcon, lblT, lblV });
            // FixedSingle vẽ viền 1px; để khớp màu border nhẹ hơn thì paint lại cạnh nếu cần — dùng mặc định.
            parent.Controls.Add(card, col, 0);
            return lblV;
        }

        private static Button CreateDashShortcut(string icon, string caption, Color bg, Action onClick)
        {
            var btn = new Button
            {
                Text = icon + Environment.NewLine + caption,
                Dock = DockStyle.Fill,
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bg, 0.08f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bg, 0.15f);
            btn.Click += (s, e) => onClick();
            return btn;
        }

        private void LoadDashboardTab()
        {
            if (lblDashWelcome == null) return;

            try
            {
                lblDashWelcome.Text = $"Xin chào, {UserSession.FullName}!";
                DashClockTick(null, EventArgs.Empty);

                DateTime today = DateTime.Today;
                int branchId = UserSession.IsAdmin ? 0 : UserSession.MaChiNhanh;
                var summary = ReportService.GetRevenueSummary(today, today, branchId);

                lblDashRevenue.Text = summary.TotalRevenue.ToString("N0", _vnCulture) + " đ";
                lblDashOrders.Text = summary.TotalOrders.ToString("N0", _vnCulture);
                lblDashDiscount.Text = summary.TotalDiscount.ToString("N0", _vnCulture) + " đ";
                lblDashAov.Text = summary.AverageOrderValue.ToString("N0", _vnCulture) + " đ";

                int lowStock = 0;
                try
                {
                    var inv = ReportService.GetInventoryConsumptionReport(branchId, true, "", today.AddDays(-7), today);
                    lowStock = inv.Rows.Count;
                }
                catch { /* bỏ qua nếu chưa có dữ liệu kho */ }
                lblDashLowStock.Text = lowStock.ToString("N0", _vnCulture);

                DataTable top = ReportService.GetBestSellerReport(today, today, branchId, 0, 8, true);
                dgvDashTopProducts.DataSource = top;
                foreach (DataGridViewColumn col in dgvDashTopProducts.Columns)
                {
                    if (col.ValueType == typeof(decimal) || col.HeaderText.Contains("VNĐ") || col.HeaderText.Contains("Thu")
                        || col.HeaderText.Contains("%") || col.HeaderText.Contains("Trọng"))
                        col.DefaultCellStyle.Format = "N0";
                    if (col.HeaderText.Contains("%") || col.HeaderText.Contains("Trọng"))
                        col.DefaultCellStyle.Format = "N1";
                }
            }
            catch (Exception ex)
            {
                lblDashRevenue.Text = "—";
                lblDashOrders.Text = "—";
                lblDashDiscount.Text = "—";
                lblDashAov.Text = "—";
                lblDashLowStock.Text = "—";
                MessageBox.Show("Không tải được dữ liệu tổng quan: " + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region ======================== 1. MODULE QUẢN LÝ THỰC ĐƠN ========================

        // UI + logic thực đơn nằm trong MenuManagementControl (từ QuanLiThucDon).
        private MenuManagementControl ucMenuManagement = null!;

        private void InitializeProductsTab()
        {
            tabProducts.BackColor = Color.FromArgb(245, 246, 250);
            tabProducts.Controls.Clear();
            ucMenuManagement = new MenuManagementControl { Dock = DockStyle.Fill, Name = "ucMenuManagement" };
            tabProducts.Controls.Add(ucMenuManagement);
        }

        private void LoadProductsTab()
        {
            try
            {
                ucMenuManagement?.LoadMenuData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thực đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ======================== 1b. MODULE KHÁCH HÀNG ========================

        private DataGridView dgvCustomers = null!;
        private TextBox txtCustSearch = null!;
        private TextBox txtCustName = null!;
        private TextBox txtCustPhone = null!;
        private TextBox txtCustEmail = null!;
        private TextBox txtCustNote = null!;
        private ComboBox cboCustTier = null!;
        private NumericUpDown numCustPoints = null!;
        private CheckBox chkCustActive = null!;
        private int _selectedCustomerId = 0;

        private void InitializeCustomersTab()
        {
            tabCustomers.BackColor = Color.FromArgb(245, 246, 250);

            var split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 780 };
            tabCustomers.Controls.Add(split);

            var grpList = new GroupBox
            {
                Text = "👤 DANH SÁCH KHÁCH HÀNG THÂN THIẾT",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Padding = new Padding(10)
            };
            split.Panel1.Controls.Add(grpList);

            var pnlSearch = new Panel { Dock = DockStyle.Top, Height = 44 };
            var lblSearch = new Label
            {
                Text = "Tìm SĐT / tên:",
                Location = new Point(10, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F)
            };
            txtCustSearch = new TextBox
            {
                Location = new Point(120, 8),
                Size = new Size(280, 28),
                Font = new Font("Segoe UI", 9.5F),
                PlaceholderText = "Nhập để lọc..."
            };
            txtCustSearch.TextChanged += (s, e) => LoadCustomersTab();
            pnlSearch.Controls.AddRange(new Control[] { lblSearch, txtCustSearch });
            grpList.Controls.Add(pnlSearch);

            dgvCustomers = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            dgvCustomers.CellClick += (s, e) => BindCustomerToForm();
            grpList.Controls.Add(dgvCustomers);
            dgvCustomers.BringToFront();

            var grpForm = new GroupBox
            {
                Text = "THÔNG TIN KHÁCH HÀNG",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Padding = new Padding(15)
            };
            split.Panel2.Controls.Add(grpForm);

            int y = 30;
            var l1 = new Label { Text = "Họ tên:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtCustName = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l2 = new Label { Text = "Số điện thoại:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtCustPhone = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F), MaxLength = 20 };

            y += 60;
            var l3 = new Label { Text = "Hạng thành viên:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            cboCustTier = new ComboBox
            {
                Location = new Point(20, y + 22),
                Size = new Size(280, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };

            y += 60;
            var l4 = new Label { Text = "Điểm hiện tại:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            numCustPoints = new NumericUpDown
            {
                Location = new Point(20, y + 22),
                Size = new Size(280, 26),
                Maximum = 1_000_000,
                Font = new Font("Segoe UI", 9.5F),
                ThousandsSeparator = true
            };

            y += 60;
            var l5 = new Label { Text = "Email:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtCustEmail = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F), MaxLength = 150 };

            y += 60;
            var l6 = new Label { Text = "Ghi chú / thẻ nhận:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtCustNote = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F), MaxLength = 255 };

            y += 55;
            chkCustActive = new CheckBox
            {
                Text = "Đang hoạt động",
                Location = new Point(20, y),
                AutoSize = true,
                Checked = true,
                Font = new Font("Segoe UI", 9.5F)
            };

            y += 40;
            var btnAdd = MakeActionButton("➕ Thêm Mới", new Point(20, y), new Size(130, 36), Color.FromArgb(39, 174, 96));
            btnAdd.Click += (s, e) => SaveCustomer(isNew: true);
            var btnEdit = MakeActionButton("✏️ Cập Nhật", new Point(160, y), new Size(140, 36), Color.FromArgb(41, 128, 185));
            btnEdit.Click += (s, e) => SaveCustomer(isNew: false);

            y += 45;
            var btnDelete = MakeActionButton("🗑️ Ngưng KH", new Point(20, y), new Size(130, 36), Color.FromArgb(192, 57, 43));
            btnDelete.Click += (s, e) => DeactivateCustomer();
            var btnReset = MakeActionButton("🔄 Làm Mới", new Point(160, y), new Size(140, 36), Color.FromArgb(149, 165, 166));
            btnReset.Click += (s, e) => ResetCustomerForm();

            grpForm.Controls.AddRange(new Control[]
            {
                l1, txtCustName, l2, txtCustPhone, l3, cboCustTier,
                l4, numCustPoints, l5, txtCustEmail, l6, txtCustNote, chkCustActive,
                btnAdd, btnEdit, btnDelete, btnReset
            });
        }

        private void LoadCustomersTab()
        {
            try
            {
                if (cboCustTier.DataSource == null)
                {
                    var dtTier = CustomerService.GetMembershipTiers();
                    cboCustTier.DataSource = dtTier;
                    cboCustTier.DisplayMember = "TenHienThi";
                    cboCustTier.ValueMember = "MaHang";
                }

                string keyword = (txtCustSearch?.Text ?? "").Trim();
                var dt = CustomerService.Search(keyword);
                dgvCustomers.DataSource = dt;
                if (dgvCustomers.Columns["MaKhachHang"] != null) dgvCustomers.Columns["MaKhachHang"].Visible = false;
                if (dgvCustomers.Columns["MaHang"] != null) dgvCustomers.Columns["MaHang"].Visible = false;
                if (dgvCustomers.Columns["Điểm"] != null) dgvCustomers.Columns["Điểm"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindCustomerToForm()
        {
            if (dgvCustomers.CurrentRow == null) return;
            var row = dgvCustomers.CurrentRow;
            _selectedCustomerId = Convert.ToInt32(row.Cells["MaKhachHang"].Value);
            txtCustName.Text = Convert.ToString(row.Cells["Họ tên"].Value) ?? "";
            txtCustPhone.Text = Convert.ToString(row.Cells["SĐT"].Value) ?? "";
            cboCustTier.SelectedValue = row.Cells["MaHang"].Value;
            numCustPoints.Value = Convert.ToDecimal(row.Cells["Điểm"].Value);
            txtCustEmail.Text = Convert.ToString(row.Cells["Email"].Value) ?? "";
            txtCustNote.Text = Convert.ToString(row.Cells["Ghi chú"].Value) ?? "";
            chkCustActive.Checked = Convert.ToBoolean(row.Cells["Hoạt động"].Value);
        }

        private void ResetCustomerForm()
        {
            _selectedCustomerId = 0;
            txtCustName.Clear();
            txtCustPhone.Clear();
            txtCustEmail.Clear();
            txtCustNote.Clear();
            numCustPoints.Value = 0;
            chkCustActive.Checked = true;
            if (cboCustTier.Items.Count > 0) cboCustTier.SelectedIndex = 0;
        }

        private void SaveCustomer(bool isNew)
        {
            string name = txtCustName.Text.Trim();
            string phone = new string(txtCustPhone.Text.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(name) || phone.Length < 9)
            {
                MessageBox.Show("Vui lòng nhập họ tên và SĐT hợp lệ (ít nhất 9 số)!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cboCustTier.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn hạng thành viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var model = new CustomerSaveModel
            {
                CustomerId = _selectedCustomerId,
                MaHang = Convert.ToInt32(cboCustTier.SelectedValue),
                HoTen = name,
                SoDienThoai = phone,
                Diem = (int)numCustPoints.Value,
                Email = string.IsNullOrWhiteSpace(txtCustEmail.Text) ? null : txtCustEmail.Text.Trim(),
                GhiChu = string.IsNullOrWhiteSpace(txtCustNote.Text) ? null : txtCustNote.Text.Trim(),
                DangHoatDong = chkCustActive.Checked
            };

            try
            {
                if (isNew)
                {
                    CustomerService.Create(model);
                    MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (_selectedCustomerId == 0)
                    {
                        MessageBox.Show("Vui lòng chọn khách cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    CustomerService.Update(model);
                    MessageBox.Show("Cập nhật khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadCustomersTab();
                ResetCustomerForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeactivateCustomer()
        {
            if (_selectedCustomerId == 0)
            {
                MessageBox.Show("Vui lòng chọn khách cần ngưng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Ngưng hoạt động khách hàng này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                CustomerService.Deactivate(_selectedCustomerId);
                MessageBox.Show("Đã ngưng khách hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomersTab();
                ResetCustomerForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ======================== 1c. MODULE CHƯƠNG TRÌNH KHUYẾN MÃI ========================

        private DataGridView dgvPromos = null!;
        private TextBox txtPromoCode = null!;
        private TextBox txtPromoName = null!;
        private ComboBox cboPromoType = null!;
        private ComboBox cboPromoBranch = null!;
        private NumericUpDown numPromoValue = null!;
        private NumericUpDown numPromoMax = null!;
        private NumericUpDown numPromoMinOrder = null!;
        private DateTimePicker dtpPromoStart = null!;
        private DateTimePicker dtpPromoEnd = null!;
        private CheckBox chkPromoActive = null!;
        private CheckBox chkPromoNoMax = null!;
        private int _selectedPromoId = 0;

        private void InitializePromosTab()
        {
            tabPromos.BackColor = Color.FromArgb(245, 246, 250);

            var split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 760 };
            tabPromos.Controls.Add(split);

            var grpList = new GroupBox
            {
                Text = "🎁 DANH SÁCH CHƯƠNG TRÌNH KHUYẾN MÃI",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Padding = new Padding(10)
            };
            split.Panel1.Controls.Add(grpList);

            dgvPromos = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                AllowUserToAddRows = false,
                RowHeadersVisible = false
            };
            dgvPromos.CellClick += (s, e) => BindPromoToForm();
            grpList.Controls.Add(dgvPromos);

            var grpForm = new GroupBox
            {
                Text = "THÔNG TIN CHƯƠNG TRÌNH",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Padding = new Padding(8)
            };
            split.Panel2.Controls.Add(grpForm);

            var pnlPromoForm = new Panel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(8) };
            grpForm.Controls.Add(pnlPromoForm);

            int y = 28;
            var l1 = new Label { Text = "Mã KM:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtPromoCode = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F), MaxLength = 30 };

            y += 56;
            var l2 = new Label { Text = "Tên chương trình:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtPromoName = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F), MaxLength = 200 };

            y += 56;
            var l3 = new Label { Text = "Loại giảm:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            cboPromoType = new ComboBox
            {
                Location = new Point(20, y + 22),
                Size = new Size(280, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };
            cboPromoType.Items.AddRange(new object[] { "PhanTram — %", "SoTien — Số tiền" });
            cboPromoType.SelectedIndex = 0;
            cboPromoType.SelectedIndexChanged += (s, e) => UpdatePromoValueLimits();

            y += 56;
            var l4 = new Label { Text = "Giá trị giảm:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            numPromoValue = new NumericUpDown
            {
                Location = new Point(20, y + 22),
                Size = new Size(280, 26),
                Maximum = 10_000_000,
                DecimalPlaces = 2,
                Font = new Font("Segoe UI", 9.5F)
            };

            y += 56;
            var l5 = new Label { Text = "Giảm tối đa (chỉ %):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            numPromoMax = new NumericUpDown
            {
                Location = new Point(20, y + 22),
                Size = new Size(180, 26),
                Maximum = 10_000_000,
                DecimalPlaces = 0,
                Increment = 1000,
                Font = new Font("Segoe UI", 9.5F)
            };
            chkPromoNoMax = new CheckBox
            {
                Text = "Không trần",
                Location = new Point(210, y + 24),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };
            chkPromoNoMax.CheckedChanged += (s, e) =>
            {
                numPromoMax.Enabled = !chkPromoNoMax.Checked;
                if (chkPromoNoMax.Checked) numPromoMax.Value = 0;
            };

            y += 56;
            var l6 = new Label { Text = "Đơn tối thiểu (đ):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            numPromoMinOrder = new NumericUpDown
            {
                Location = new Point(20, y + 22),
                Size = new Size(280, 26),
                Maximum = 50_000_000,
                DecimalPlaces = 0,
                Increment = 1000,
                Font = new Font("Segoe UI", 9.5F)
            };

            y += 56;
            var l7 = new Label { Text = "Chi nhánh áp dụng:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            cboPromoBranch = new ComboBox
            {
                Location = new Point(20, y + 22),
                Size = new Size(280, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9.5F)
            };

            y += 56;
            var l8 = new Label { Text = "Ngày bắt đầu:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            dtpPromoStart = new DateTimePicker
            {
                Location = new Point(20, y + 22),
                Size = new Size(280, 26),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                ShowUpDown = true,
                Font = new Font("Segoe UI", 9.5F)
            };

            y += 56;
            var l9 = new Label { Text = "Ngày kết thúc:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            dtpPromoEnd = new DateTimePicker
            {
                Location = new Point(20, y + 22),
                Size = new Size(280, 26),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy HH:mm",
                ShowUpDown = true,
                Font = new Font("Segoe UI", 9.5F)
            };

            y += 52;
            chkPromoActive = new CheckBox
            {
                Text = "Đang hoạt động",
                Location = new Point(20, y),
                AutoSize = true,
                Checked = true,
                Font = new Font("Segoe UI", 9.5F)
            };

            y += 36;
            var btnAdd = MakeActionButton("➕ Thêm Mới", new Point(20, y), new Size(130, 36), Color.FromArgb(39, 174, 96));
            btnAdd.Click += (s, e) => SavePromo(isNew: true);
            var btnEdit = MakeActionButton("✏️ Cập Nhật", new Point(160, y), new Size(140, 36), Color.FromArgb(41, 128, 185));
            btnEdit.Click += (s, e) => SavePromo(isNew: false);

            y += 45;
            var btnDelete = MakeActionButton("🗑️ Tắt KM", new Point(20, y), new Size(130, 36), Color.FromArgb(192, 57, 43));
            btnDelete.Click += (s, e) => DeactivatePromo();
            var btnReset = MakeActionButton("🔄 Làm Mới", new Point(160, y), new Size(140, 36), Color.FromArgb(149, 165, 166));
            btnReset.Click += (s, e) => ResetPromoForm();

            pnlPromoForm.Controls.AddRange(new Control[]
            {
                l1, txtPromoCode, l2, txtPromoName, l3, cboPromoType, l4, numPromoValue,
                l5, numPromoMax, chkPromoNoMax, l6, numPromoMinOrder, l7, cboPromoBranch,
                l8, dtpPromoStart, l9, dtpPromoEnd, chkPromoActive,
                btnAdd, btnEdit, btnDelete, btnReset
            });

            UpdatePromoValueLimits();
        }

        private void UpdatePromoValueLimits()
        {
            bool percent = cboPromoType.SelectedIndex == 0;
            if (percent)
            {
                numPromoValue.Maximum = 100;
                numPromoValue.DecimalPlaces = 2;
                if (numPromoValue.Value > 100) numPromoValue.Value = 100;
                numPromoMax.Enabled = !chkPromoNoMax.Checked;
                chkPromoNoMax.Enabled = true;
            }
            else
            {
                numPromoValue.Maximum = 10_000_000;
                numPromoValue.DecimalPlaces = 0;
                chkPromoNoMax.Checked = true;
                chkPromoNoMax.Enabled = false;
                numPromoMax.Enabled = false;
            }
        }

        private void LoadPromosTab()
        {
            try
            {
                if (cboPromoBranch.DataSource == null)
                {
                    var dtBranch = PromotionService.GetActiveBranches();
                    // 0 = mọi chi nhánh (MaChiNhanh NULL trong DB)
                    var combo = new DataTable();
                    combo.Columns.Add("MaChiNhanh", typeof(int));
                    combo.Columns.Add("TenChiNhanh", typeof(string));
                    combo.Rows.Add(0, "Tất cả chi nhánh");
                    foreach (DataRow r in dtBranch.Rows)
                        combo.Rows.Add(Convert.ToInt32(r["MaChiNhanh"]), Convert.ToString(r["TenChiNhanh"]));
                    cboPromoBranch.DataSource = combo;
                    cboPromoBranch.DisplayMember = "TenChiNhanh";
                    cboPromoBranch.ValueMember = "MaChiNhanh";
                }

                var dt = PromotionService.GetAll();
                dgvPromos.DataSource = dt;
                foreach (string col in new[] { "MaKhuyenMai", "LoaiGiam", "MaChiNhanh" })
                    if (dgvPromos.Columns[col] != null) dgvPromos.Columns[col].Visible = false;
                foreach (string col in new[] { "Giá trị", "Trần", "Đơn TT" })
                    if (dgvPromos.Columns[col] != null) dgvPromos.Columns[col].DefaultCellStyle.Format = "N0";
                foreach (string col in new[] { "Bắt đầu", "Kết thúc" })
                    if (dgvPromos.Columns[col] != null) dgvPromos.Columns[col].DefaultCellStyle.Format = "dd/MM/yy HH:mm";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindPromoToForm()
        {
            if (dgvPromos.CurrentRow == null) return;
            var row = dgvPromos.CurrentRow;
            _selectedPromoId = Convert.ToInt32(row.Cells["MaKhuyenMai"].Value);
            txtPromoCode.Text = Convert.ToString(row.Cells["Mã"].Value) ?? "";
            txtPromoName.Text = Convert.ToString(row.Cells["Tên KM"].Value) ?? "";
            string loai = Convert.ToString(row.Cells["LoaiGiam"].Value) ?? "PhanTram";
            cboPromoType.SelectedIndex = loai == "SoTien" ? 1 : 0;
            UpdatePromoValueLimits();
            numPromoValue.Value = Convert.ToDecimal(row.Cells["Giá trị"].Value);

            object? max = row.Cells["Trần"].Value;
            if (max == null || max == DBNull.Value)
            {
                chkPromoNoMax.Checked = true;
                numPromoMax.Value = 0;
            }
            else
            {
                chkPromoNoMax.Checked = false;
                numPromoMax.Value = Convert.ToDecimal(max);
            }

            numPromoMinOrder.Value = Convert.ToDecimal(row.Cells["Đơn TT"].Value);
            object? branch = row.Cells["MaChiNhanh"].Value;
            cboPromoBranch.SelectedValue = (branch == null || branch == DBNull.Value)
                ? 0
                : Convert.ToInt32(branch);

            dtpPromoStart.Value = Convert.ToDateTime(row.Cells["Bắt đầu"].Value);
            dtpPromoEnd.Value = Convert.ToDateTime(row.Cells["Kết thúc"].Value);
            chkPromoActive.Checked = Convert.ToBoolean(row.Cells["HĐ"].Value);
        }

        private void ResetPromoForm()
        {
            _selectedPromoId = 0;
            txtPromoCode.Clear();
            txtPromoName.Clear();
            cboPromoType.SelectedIndex = 0;
            numPromoValue.Value = 10;
            chkPromoNoMax.Checked = false;
            numPromoMax.Value = 30000;
            numPromoMinOrder.Value = 50000;
            if (cboPromoBranch.Items.Count > 0) cboPromoBranch.SelectedIndex = 0;
            dtpPromoStart.Value = DateTime.Today.AddHours(6);
            dtpPromoEnd.Value = DateTime.Today.AddMonths(1).AddHours(21);
            chkPromoActive.Checked = true;
            UpdatePromoValueLimits();
        }

        private void SavePromo(bool isNew)
        {
            string code = txtPromoCode.Text.Trim().ToUpperInvariant();
            string name = txtPromoName.Text.Trim();
            if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Vui lòng nhập mã và tên chương trình!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dtpPromoEnd.Value <= dtpPromoStart.Value)
            {
                MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (numPromoValue.Value <= 0)
            {
                MessageBox.Show("Giá trị giảm phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string loai = cboPromoType.SelectedIndex == 0 ? "PhanTram" : "SoTien";
            decimal? giamToiDa = (loai == "PhanTram" && !chkPromoNoMax.Checked && numPromoMax.Value > 0)
                ? numPromoMax.Value
                : null;

            int branchId = cboPromoBranch.SelectedValue == null ? 0 : Convert.ToInt32(cboPromoBranch.SelectedValue);
            var model = new PromoSaveModel
            {
                PromoId = _selectedPromoId,
                Code = code,
                Name = name,
                LoaiGiam = loai,
                GiaTri = numPromoValue.Value,
                GiamToiDa = giamToiDa,
                DonToiThieu = numPromoMinOrder.Value,
                MaChiNhanh = branchId > 0 ? branchId : null,
                NgayBatDau = dtpPromoStart.Value,
                NgayKetThuc = dtpPromoEnd.Value,
                DangHoatDong = chkPromoActive.Checked
            };

            try
            {
                if (isNew)
                {
                    PromotionService.Create(model);
                    MessageBox.Show("Thêm chương trình KM thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (_selectedPromoId == 0)
                    {
                        MessageBox.Show("Vui lòng chọn chương trình cần cập nhật!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    PromotionService.Update(model);
                    MessageBox.Show("Cập nhật chương trình KM thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadPromosTab();
                ResetPromoForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeactivatePromo()
        {
            if (_selectedPromoId == 0)
            {
                MessageBox.Show("Vui lòng chọn chương trình cần tắt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Tắt chương trình khuyến mãi này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                PromotionService.Deactivate(_selectedPromoId);
                MessageBox.Show("Đã tắt chương trình KM!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadPromosTab();
                ResetPromoForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static Button MakeActionButton(string text, Point location, Size size, Color backColor)
        {
            var btn = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        #endregion

        #region ======================== 2. MODULE KHO NGUYÊN LIỆU ========================

        private DataGridView dgvIngredients = null!;
        private CheckBox chkLowStockOnly = null!;
        private TextBox txtIngCode = null!;
        private TextBox txtIngName = null!;
        private TextBox txtIngUnit = null!;
        private NumericUpDown numIngCurrentStock = null!;
        private NumericUpDown numIngMinStock = null!;
        private NumericUpDown numIngCost = null!;
        private int _selectedIngredientID = 0;

        private void InitializeIngredientsTab()
        {
            tabIngredients.BackColor = Color.FromArgb(245, 246, 250);

            var split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 780 };
            tabIngredients.Controls.Add(split);

            // Bảng danh sách
            var grpList = new GroupBox
            {
                Text = "📦 DANH SÁCH NGUYÊN LIỆU TRONG KHO",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Padding = new Padding(10)
            };
            split.Panel1.Controls.Add(grpList);

            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 40 };
            chkLowStockOnly = new CheckBox
            {
                Text = "⚠️ Chỉ hiện nguyên liệu sắp hết (Cảnh báo tồn kho)",
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 57, 43),
                Location = new Point(10, 8),
                AutoSize = true
            };
            chkLowStockOnly.CheckedChanged += (s, e) => LoadIngredientsTab();
            pnlFilter.Controls.Add(chkLowStockOnly);
            grpList.Controls.Add(pnlFilter);

            dgvIngredients = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            dgvIngredients.CellClick += (s, e) => BindIngredientToForm();
            grpList.Controls.Add(dgvIngredients);
            dgvIngredients.BringToFront();

            // Form cập nhật
            var grpForm = new GroupBox
            {
                Text = "QUẢN LÝ NGUYÊN LIỆU",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Padding = new Padding(15)
            };
            split.Panel2.Controls.Add(grpForm);

            int y = 30;
            var l1 = new Label { Text = "Mã nguyên liệu:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtIngCode = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l2 = new Label { Text = "Tên nguyên liệu:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtIngName = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l3 = new Label { Text = "Đơn vị (Kg, Quả, Ổ, Hộp...):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtIngUnit = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l4 = new Label { Text = "Số lượng tồn kho:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            numIngCurrentStock = new NumericUpDown { Location = new Point(20, y + 22), Size = new Size(280, 26), Maximum = 100000, DecimalPlaces = 2, Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l5 = new Label { Text = "Tồn kho tối thiểu:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            numIngMinStock = new NumericUpDown { Location = new Point(20, y + 22), Size = new Size(280, 26), Maximum = 100000, DecimalPlaces = 2, Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l6 = new Label { Text = "Giá vốn (VNĐ):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            numIngCost = new NumericUpDown { Location = new Point(20, y + 22), Size = new Size(280, 26), Maximum = 10000000, Increment = 1000, Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var btnSaveIng = new Button { Text = "💾 Lưu / Cập Nhật", Location = new Point(20, y), Size = new Size(135, 36), BackColor = Color.FromArgb(41, 128, 185), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            btnSaveIng.FlatAppearance.BorderSize = 0;
            btnSaveIng.Click += (s, e) => SaveIngredient();

            var btnAddStock = new Button { Text = "📥 Nhập Thêm", Location = new Point(165, y), Size = new Size(135, 36), BackColor = Color.FromArgb(39, 174, 96), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            btnAddStock.FlatAppearance.BorderSize = 0;
            btnAddStock.Click += (s, e) => QuickAddStock();

            grpForm.Controls.AddRange(new Control[]
            {
                l1, txtIngCode, l2, txtIngName, l3, txtIngUnit,
                l4, numIngCurrentStock, l5, numIngMinStock, l6, numIngCost,
                btnSaveIng, btnAddStock
            });
        }

        private void LoadIngredientsTab()
        {
            try
            {
                DataTable dt = IngredientService.GetAll(chkLowStockOnly.Checked);
                dgvIngredients.DataSource = dt;
                if (dgvIngredients.Columns["IngredientID"] != null)
                    dgvIngredients.Columns["IngredientID"].Visible = false;

                if (dgvIngredients.Columns["Giá Vốn"] != null)
                    dgvIngredients.Columns["Giá Vốn"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải kho nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindIngredientToForm()
        {
            if (dgvIngredients.CurrentRow == null) return;
            var row = dgvIngredients.CurrentRow;
            _selectedIngredientID = Convert.ToInt32(row.Cells["IngredientID"].Value);
            txtIngCode.Text = row.Cells["Mã NL"].Value.ToString();
            txtIngName.Text = row.Cells["Tên Nguyên Liệu"].Value.ToString();
            txtIngUnit.Text = row.Cells["ĐVT"].Value.ToString();
            numIngCurrentStock.Value = Convert.ToDecimal(row.Cells["Tồn Kho"].Value);
            numIngMinStock.Value = Convert.ToDecimal(row.Cells["Tồn Tối Thiểu"].Value);
            numIngCost.Value = Convert.ToDecimal(row.Cells["Giá Vốn"].Value);
        }

        private void SaveIngredient()
        {
            string code = txtIngCode.Text.Trim();
            string name = txtIngName.Text.Trim();
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập Mã và Tên nguyên liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                IngredientService.Save(new IngredientSaveModel
                {
                    IngredientId = _selectedIngredientID,
                    Code = code,
                    Name = name,
                    Unit = txtIngUnit.Text.Trim(),
                    CurrentStock = numIngCurrentStock.Value,
                    MinimumStock = numIngMinStock.Value,
                    CostPrice = numIngCost.Value
                });
                MessageBox.Show(
                    _selectedIngredientID > 0 ? "Cập nhật nguyên liệu thành công!" : "Thêm nguyên liệu mới thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadIngredientsTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu nguyên liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void QuickAddStock()
        {
            if (_selectedIngredientID == 0)
            {
                MessageBox.Show("Vui lòng chọn nguyên liệu cần nhập thêm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            numIngCurrentStock.Value += 10;
            SaveIngredient();
        }

        #endregion

        #region ======================== 4. MODULE BÁO CÁO & THỐNG KÊ (ADMIN) ========================

        // Sub-navigation buttons
        private Button btnSubNavRev = null!;
        private Button btnSubNavBestSeller = null!;
        private Button btnSubNavInventory = null!;
        private Button btnSubNavChart = null!;
        private Button btnSubNavExport = null!;
        private Panel pnlReportSubContainer = null!;

        // 5 View Panels
        private Panel pnlSubRev = null!;
        private Panel pnlSubBestSeller = null!;
        private Panel pnlSubInventory = null!;
        private Panel pnlSubChart = null!;
        private Panel pnlSubExport = null!;

        // 1. Controls Báo Cáo Doanh Thu
        private DateTimePicker dtpRevFrom = null!;
        private DateTimePicker dtpRevTo = null!;
        private ComboBox cboRevBranch = null!;
        private ComboBox cboRevMode = null!;
        private Label lblRevTotalAmount = null!;
        private Label lblRevTotalOrders = null!;
        private Label lblRevTotalDiscount = null!;
        private Label lblRevAov = null!;
        private DataGridView dgvRevenue = null!;

        // 2. Controls Sản Phẩm Best Seller
        private DateTimePicker dtpTopFrom = null!;
        private DateTimePicker dtpTopTo = null!;
        private ComboBox cboTopBranch = null!;
        private ComboBox cboTopCategory = null!;
        private ComboBox cboTopLimit = null!;
        private ComboBox cboTopSortBy = null!;
        private Label lblTop1Name = null!;
        private Label lblTop1RevName = null!;
        private Label lblTopTotalSold = null!;
        private DataGridView dgvBestSeller = null!;

        // 3. Controls Tồn Kho & Tiêu Hao
        private ComboBox cboInvBranch = null!;
        private ComboBox cboInvStatus = null!;
        private TextBox txtInvSearch = null!;
        private DateTimePicker dtpInvFrom = null!;
        private DateTimePicker dtpInvTo = null!;
        private Label lblInvTotalItems = null!;
        private Label lblInvLowStockCount = null!;
        private Label lblInvTotalConsumed = null!;
        private Label lblInvTotalImported = null!;
        private DataGridView dgvInventory = null!;

        // 0. Controls Dashboard Tổng Quan Báo Cáo
        private Panel pnlSubDashboard = null!;
        private Button btnSubNavDashboard = null!;
        private ComboBox cboRptDashPeriodPreset = null!;
        private DateTimePicker dtpRptDashFrom = null!;
        private DateTimePicker dtpRptDashTo = null!;
        private ComboBox cboRptDashBranch = null!;
        private Label lblRptDashTotalRevenue = null!;
        private Label lblRptDashTotalOrders = null!;
        private Label lblRptDashTotalItems = null!;
        private Label lblRptDashAov = null!;
        private Label lblRptDashTotalDiscount = null!;
        private ReportChartView chartRptDashRevenueTrend = null!;
        private ReportChartView chartRptDashBranch = null!;
        private ReportChartView chartRptDashTopDishes = null!;
        private ReportChartView chartRptDashCategory = null!;

        // 4. Controls Biểu Đồ Phân Tích
        private ComboBox cboChartType = null!;
        private DateTimePicker dtpChartFrom = null!;
        private DateTimePicker dtpChartTo = null!;
        private ComboBox cboChartBranch = null!;
        private ReportChartView chartAnalyticsView = null!;

        // 5. Controls Xuất File Báo Cáo
        private RadioButton rdoExpRev = null!;
        private RadioButton rdoExpBestSeller = null!;
        private RadioButton rdoExpInv = null!;
        private DateTimePicker dtpExpFrom = null!;
        private DateTimePicker dtpExpTo = null!;
        private ComboBox cboExpBranch = null!;
        private RadioButton rdoExpExcel = null!;
        private RadioButton rdoExpPdf = null!;
        private RadioButton rdoExpCsv = null!;
        private RadioButton rdoExpHtml = null!;
        private ProgressBar prgExport = null!;
        private Label lblExpStatus = null!;
        private DataGridView dgvExpPreview = null!;

        private int _currentReportSubIndex = 0;

        private void InitializeReportsTab()
        {
            tabReports.BackColor = Color.FromArgb(245, 246, 250);

            // ================= SUB-NAVIGATION BAR =================
            var pnlSubNav = new Panel
            {
                Dock = DockStyle.Top,
                Height = 52,
                BackColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(10, 5, 10, 5)
            };
            tabReports.Controls.Add(pnlSubNav);

            btnSubNavDashboard = CreateSubNavButton("📈 Dashboard Tổng Quan", 0, 205);
            btnSubNavRev = CreateSubNavButton("💰 Báo Cáo Doanh Thu", 1, 195);
            btnSubNavBestSeller = CreateSubNavButton("🏆 Sản Phẩm Best Seller", 2, 200);
            btnSubNavInventory = CreateSubNavButton("📦 Tồn Kho & Tiêu Hao", 3, 195);
            btnSubNavChart = CreateSubNavButton("📊 Biểu Đồ Phân Tích", 4, 185);
            btnSubNavExport = CreateSubNavButton("📑 Xuất File Báo Cáo", 5, 180);

            pnlSubNav.Controls.AddRange(new Control[]
            {
                btnSubNavDashboard, btnSubNavRev, btnSubNavBestSeller, btnSubNavInventory, btnSubNavChart, btnSubNavExport
            });

            // Container chứa 6 panels con
            pnlReportSubContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 246, 250)
            };
            tabReports.Controls.Add(pnlReportSubContainer);
            pnlReportSubContainer.BringToFront();

            // Khởi tạo 6 view panels
            InitializeDashboardSubView();
            InitializeRevenueSubView();
            InitializeBestSellerSubView();
            InitializeInventorySubView();
            InitializeChartSubView();
            InitializeExportSubView();

            // Mặc định mở view Dashboard Tổng Quan
            SwitchReportSubTab(0);
        }

        private Button CreateSubNavButton(string text, int index, int width = 200)
        {
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Left,
                Width = width,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = Color.Gainsboro,
                BackColor = Color.FromArgb(44, 62, 80),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => SwitchReportSubTab(index);
            return btn;
        }

        private void SwitchReportSubTab(int index)
        {
            _currentReportSubIndex = index;

            var buttons = new[] { btnSubNavDashboard, btnSubNavRev, btnSubNavBestSeller, btnSubNavInventory, btnSubNavChart, btnSubNavExport };
            var panels = new[] { pnlSubDashboard, pnlSubRev, pnlSubBestSeller, pnlSubInventory, pnlSubChart, pnlSubExport };

            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == index)
                {
                    buttons[i].BackColor = Color.FromArgb(230, 126, 34);
                    buttons[i].ForeColor = Color.White;
                    panels[i].BringToFront();
                    panels[i].Visible = true;
                }
                else
                {
                    buttons[i].BackColor = Color.FromArgb(44, 62, 80);
                    buttons[i].ForeColor = Color.Gainsboro;
                    panels[i].Visible = false;
                }
            }

            // Nạp dữ liệu khi chuyển tab
            switch (index)
            {
                case 0: LoadDashboardReport(); break;
                case 1: LoadRevenueReport(); break;
                case 2: LoadBestSellerReport(); break;
                case 3: LoadInventoryReport(); break;
                case 4: LoadChartReport(); break;
                case 5: LoadExportPreview(); break;
            }
        }

        #region --------------- 4.0 VIEW: DASHBOARD TỔNG QUAN BÁO CÁO ---------------

        private void InitializeDashboardSubView()
        {
            pnlSubDashboard = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 250) };
            pnlReportSubContainer.Controls.Add(pnlSubDashboard);

            // 1. Filter Bar
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(12, 10, 12, 10) };
            pnlSubDashboard.Controls.Add(pnlFilter);

            var lblPreset = new Label { Text = "Kỳ báo cáo:", Location = new Point(12, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboRptDashPeriodPreset = new ComboBox { Location = new Point(95, 13), Size = new Size(185, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cboRptDashPeriodPreset.Items.AddRange(new object[]
            {
                "⚡ Dữ liệu mẫu (Tháng 9/2026)",
                "📅 Hôm nay",
                "📅 7 ngày gần nhất",
                "📅 Tháng này",
                "📅 Tháng trước",
                "⚙️ Tùy chọn khoảng ngày"
            });
            cboRptDashPeriodPreset.SelectedIndex = 0;

            var lblFrom = new Label { Text = "Từ:", Location = new Point(290, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            dtpRptDashFrom = new DateTimePicker { Location = new Point(320, 13), Size = new Size(110, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 1), Font = new Font("Segoe UI", 9.5F) };

            var lblTo = new Label { Text = "Đến:", Location = new Point(438, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            dtpRptDashTo = new DateTimePicker { Location = new Point(475, 13), Size = new Size(110, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 30), Font = new Font("Segoe UI", 9.5F) };

            var lblBranch = new Label { Text = "Chi nhánh:", Location = new Point(595, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboRptDashBranch = new ComboBox { Location = new Point(670, 13), Size = new Size(165, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };

            cboRptDashPeriodPreset.SelectedIndexChanged += (s, e) =>
            {
                DateTime today = DateTime.Today;
                switch (cboRptDashPeriodPreset.SelectedIndex)
                {
                    case 0: // Tháng 9/2026
                        dtpRptDashFrom.Value = new DateTime(2026, 9, 1);
                        dtpRptDashTo.Value = new DateTime(2026, 9, 30);
                        break;
                    case 1: // Hôm nay
                        dtpRptDashFrom.Value = today;
                        dtpRptDashTo.Value = today;
                        break;
                    case 2: // 7 ngày qua
                        dtpRptDashFrom.Value = today.AddDays(-6);
                        dtpRptDashTo.Value = today;
                        break;
                    case 3: // Tháng này
                        dtpRptDashFrom.Value = new DateTime(today.Year, today.Month, 1);
                        dtpRptDashTo.Value = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
                        break;
                    case 4: // Tháng trước
                        DateTime prev = today.AddMonths(-1);
                        dtpRptDashFrom.Value = new DateTime(prev.Year, prev.Month, 1);
                        dtpRptDashTo.Value = new DateTime(prev.Year, prev.Month, DateTime.DaysInMonth(prev.Year, prev.Month));
                        break;
                }
                LoadDashboardReport();
            };

            var btnFilter = CreateActionButton("🔍 Tra Cứu", Color.FromArgb(41, 128, 185), new Point(845, 11), 95);
            btnFilter.Click += (s, e) => LoadDashboardReport();

            var btnExportExcel = CreateActionButton("📊 Xuất Excel", Color.FromArgb(39, 174, 96), new Point(948, 11), 105);
            btnExportExcel.Click += (s, e) => ExportDashboardExcel();

            var btnExportPdf = CreateActionButton("📕 Xuất PDF", Color.FromArgb(230, 126, 34), new Point(1060, 11), 95);
            btnExportPdf.Click += (s, e) => ExportDashboardPdf();

            pnlFilter.Controls.AddRange(new Control[]
            {
                lblPreset, cboRptDashPeriodPreset, lblFrom, dtpRptDashFrom, lblTo, dtpRptDashTo,
                lblBranch, cboRptDashBranch, btnFilter, btnExportExcel, btnExportPdf
            });

            // 2. KPI Cards Panel
            var pnlCards = new Panel { Dock = DockStyle.Top, Height = 95, BackColor = Color.FromArgb(245, 246, 250), Padding = new Padding(12, 10, 12, 10) };
            pnlSubDashboard.Controls.Add(pnlCards);

            CreateKpiCard(pnlCards, "TỔNG THỰC THU", out lblRptDashTotalRevenue, Color.FromArgb(39, 174, 96), 12, 10, 240, 75);
            CreateKpiCard(pnlCards, "TỔNG ĐƠN HÀNG", out lblRptDashTotalOrders, Color.FromArgb(41, 128, 185), 258, 10, 210, 75);
            CreateKpiCard(pnlCards, "SỐ LƯỢNG MÓN BÁN", out lblRptDashTotalItems, Color.FromArgb(22, 160, 133), 474, 10, 210, 75);
            CreateKpiCard(pnlCards, "GIÁ TRỊ ĐƠN TRUNG BÌNH (AOV)", out lblRptDashAov, Color.FromArgb(142, 68, 173), 690, 10, 240, 75);
            CreateKpiCard(pnlCards, "TỔNG TIỀN GIẢM GIÁ", out lblRptDashTotalDiscount, Color.FromArgb(230, 126, 34), 936, 10, 220, 75);

            // 3. Vùng chứa 4 Biểu Đồ Thống Kê (2x2 Grid)
            var tableCharts = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                RowCount = 2,
                ColumnCount = 2,
                Padding = new Padding(12, 4, 12, 12),
                BackColor = Color.FromArgb(245, 246, 250)
            };
            tableCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableCharts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableCharts.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            chartRptDashRevenueTrend = new ReportChartView { Dock = DockStyle.Fill, Margin = new Padding(4) };
            chartRptDashBranch = new ReportChartView { Dock = DockStyle.Fill, Margin = new Padding(4) };
            chartRptDashTopDishes = new ReportChartView { Dock = DockStyle.Fill, Margin = new Padding(4) };
            chartRptDashCategory = new ReportChartView { Dock = DockStyle.Fill, Margin = new Padding(4) };

            tableCharts.Controls.Add(chartRptDashRevenueTrend, 0, 0);
            tableCharts.Controls.Add(chartRptDashBranch, 1, 0);
            tableCharts.Controls.Add(chartRptDashTopDishes, 0, 1);
            tableCharts.Controls.Add(chartRptDashCategory, 1, 1);

            pnlSubDashboard.Controls.Add(tableCharts);
            tableCharts.BringToFront();
        }

        private void LoadDashboardReport()
        {
            try
            {
                int branchId = 0;
                if (cboRptDashBranch.SelectedValue is int bId) branchId = bId;

                DateTime from = dtpRptDashFrom.Value;
                DateTime to = dtpRptDashTo.Value;

                // 1. Chỉ số KPI
                var summary = ReportService.GetDashboardSummary(from, to, branchId);
                lblRptDashTotalRevenue.Text = summary.TotalRevenue.ToString("N0", _vnCulture) + " đ";
                lblRptDashTotalOrders.Text = summary.TotalOrders.ToString("N0", _vnCulture) + " đơn";
                lblRptDashTotalItems.Text = summary.TotalItemsSold.ToString("N0", _vnCulture) + " món";
                lblRptDashAov.Text = summary.AverageOrderValue.ToString("N0", _vnCulture) + " đ/đơn";
                lblRptDashTotalDiscount.Text = summary.TotalDiscount.ToString("N0", _vnCulture) + " đ";

                string periodStr = $"Thời gian: {from:dd/MM/yyyy} - {to:dd/MM/yyyy}";
                string branchText = cboRptDashBranch.Text;

                // 2. Biểu đồ doanh thu theo ngày
                chartRptDashRevenueTrend.Title = "📈 DOANH THU THEO NGÀY";
                chartRptDashRevenueTrend.Subtitle = $"{periodStr} | {branchText}";
                chartRptDashRevenueTrend.ChartType = ReportChartType.Bar;
                chartRptDashRevenueTrend.DataPoints = ReportService.GetDailyRevenueChartData(from, to, branchId);
                chartRptDashRevenueTrend.Invalidate();

                // 3. Biểu đồ doanh thu theo chi nhánh
                chartRptDashBranch.Title = "🏪 DOANH THU THEO CHI NHÁNH";
                chartRptDashBranch.Subtitle = periodStr;
                chartRptDashBranch.ChartType = ReportChartType.Bar;
                chartRptDashBranch.DataPoints = ReportService.GetBranchRevenueChartData(from, to);
                chartRptDashBranch.Invalidate();

                // 4. Biểu đồ món bán chạy
                chartRptDashTopDishes.Title = "🏆 TOP 10 MÓN BÁN CHẠY NHẤT";
                chartRptDashTopDishes.Subtitle = $"{periodStr} | {branchText}";
                chartRptDashTopDishes.ChartType = ReportChartType.HorizontalBar;
                chartRptDashTopDishes.DataPoints = ReportService.GetTopProductsChartData(from, to, branchId, 10);
                chartRptDashTopDishes.Invalidate();

                // 5. Biểu đồ cơ cấu doanh thu theo danh mục
                chartRptDashCategory.Title = "🥧 CƠ CẤU DOANH THU THEO DANH MỤC";
                chartRptDashCategory.Subtitle = $"{periodStr} | {branchText}";
                chartRptDashCategory.ChartType = ReportChartType.Pie;
                chartRptDashCategory.DataPoints = ReportService.GetCategoryRevenueChartData(from, to, branchId);
                chartRptDashCategory.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật Dashboard: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ExportDashboardExcel()
        {
            try
            {
                int branchId = 0;
                if (cboRptDashBranch.SelectedValue is int bId) branchId = bId;

                DateTime from = dtpRptDashFrom.Value;
                DateTime to = dtpRptDashTo.Value;
                string period = $"{from:dd/MM/yyyy} - {to:dd/MM/yyyy}";
                string branchText = cboRptDashBranch.Text;

                DataTable dt = ReportService.GetRevenueReport(from, to, branchId, false);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var kpis = new Dictionary<string, string>
                {
                    { "Tổng Thực Thu", lblRptDashTotalRevenue.Text },
                    { "Tổng Đơn Hàng", lblRptDashTotalOrders.Text },
                    { "Tổng Món Đã Bán", lblRptDashTotalItems.Text },
                    { "Giá Trị Đơn Trung Bình", lblRptDashAov.Text },
                    { "Tổng Tiền Giảm Giá", lblRptDashTotalDiscount.Text }
                };

                await ReportExportHelper.ExportToExcelAsync(
                    dt, "Báo Cáo Tổng Quan Dashboard Doanh Thu", period, branchText, kpis, null, null,
                    savedPath => ReportExportHelper.PromptOpenFile(savedPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel Dashboard: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ExportDashboardPdf()
        {
            try
            {
                int branchId = 0;
                if (cboRptDashBranch.SelectedValue is int bId) branchId = bId;

                DateTime from = dtpRptDashFrom.Value;
                DateTime to = dtpRptDashTo.Value;
                string period = $"{from:dd/MM/yyyy} - {to:dd/MM/yyyy}";
                string branchText = cboRptDashBranch.Text;

                DataTable dt = ReportService.GetRevenueReport(from, to, branchId, false);
                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var kpis = new Dictionary<string, string>
                {
                    { "Tổng Thực Thu", lblRptDashTotalRevenue.Text },
                    { "Tổng Đơn Hàng", lblRptDashTotalOrders.Text },
                    { "Tổng Món Đã Bán", lblRptDashTotalItems.Text },
                    { "Giá Trị Đơn Trung Bình", lblRptDashAov.Text },
                    { "Tổng Tiền Giảm Giá", lblRptDashTotalDiscount.Text }
                };

                await ReportExportHelper.ExportToPdfAsync(
                    dt, "Báo Cáo Tổng Quan Dashboard Doanh Thu", period, branchText, kpis, null, null,
                    savedPath => ReportExportHelper.PromptOpenFile(savedPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất PDF Dashboard: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region --------------- 4.1 VIEW: BÁO CÁO DOANH THU ---------------

        private void InitializeRevenueSubView()
        {
            pnlSubRev = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 250) };
            pnlReportSubContainer.Controls.Add(pnlSubRev);

            // 1. Filter Bar
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(12, 10, 12, 10) };
            pnlSubRev.Controls.Add(pnlFilter);

            var lblFrom = new Label { Text = "Từ ngày:", Location = new Point(12, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            dtpRevFrom = new DateTimePicker { Location = new Point(78, 13), Size = new Size(115, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 1), Font = new Font("Segoe UI", 9.5F) };

            var lblTo = new Label { Text = "Đến ngày:", Location = new Point(200, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            dtpRevTo = new DateTimePicker { Location = new Point(275, 13), Size = new Size(115, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 30), Font = new Font("Segoe UI", 9.5F) };

            var lblBranch = new Label { Text = "Chi nhánh:", Location = new Point(400, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboRevBranch = new ComboBox { Location = new Point(475, 13), Size = new Size(175, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };

            var lblMode = new Label { Text = "Xem theo:", Location = new Point(660, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboRevMode = new ComboBox { Location = new Point(735, 13), Size = new Size(160, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cboRevMode.Items.AddRange(new object[] { "📅 Tổng hợp theo ngày", "🧾 Chi tiết từng hóa đơn" });
            cboRevMode.SelectedIndex = 0;
            cboRevMode.SelectedIndexChanged += (s, e) => LoadRevenueReport();

            var btnFilter = CreateActionButton("🔍 Tra Cứu", Color.FromArgb(41, 128, 185), new Point(905, 11), 95);
            btnFilter.Click += (s, e) => LoadRevenueReport();

            var btnQuickMonth = CreateActionButton("⚡ Cả Tháng 9", Color.FromArgb(52, 73, 94), new Point(1005, 11), 105);
            btnQuickMonth.Click += (s, e) =>
            {
                dtpRevFrom.Value = new DateTime(2026, 9, 1);
                dtpRevTo.Value = new DateTime(2026, 9, 30);
                LoadRevenueReport();
            };

            var btnExportCsv = CreateActionButton("📊 Xuất Excel", Color.FromArgb(39, 174, 96), new Point(1115, 11), 100);
            btnExportCsv.Click += (s, e) => ExportRevenueData(false);

            var btnExportHtml = CreateActionButton("🖨️ In / PDF", Color.FromArgb(230, 126, 34), new Point(1220, 11), 95);
            btnExportHtml.Click += (s, e) => ExportRevenueData(true);

            pnlFilter.Controls.AddRange(new Control[]
            {
                lblFrom, dtpRevFrom, lblTo, dtpRevTo, lblBranch, cboRevBranch,
                lblMode, cboRevMode, btnFilter, btnQuickMonth, btnExportCsv, btnExportHtml
            });

            // 2. KPI Cards Panel
            var pnlCards = new Panel { Dock = DockStyle.Top, Height = 95, BackColor = Color.FromArgb(245, 246, 250), Padding = new Padding(12, 10, 12, 10) };
            pnlSubRev.Controls.Add(pnlCards);

            CreateKpiCard(pnlCards, "TỔNG THỰC THU", out lblRevTotalAmount, Color.FromArgb(39, 174, 96), 12, 10, 290, 75);
            CreateKpiCard(pnlCards, "TỔNG ĐƠN HOÀN THÀNH", out lblRevTotalOrders, Color.FromArgb(41, 128, 185), 312, 10, 240, 75);
            CreateKpiCard(pnlCards, "TỔNG TIỀN GIẢM GIÁ", out lblRevTotalDiscount, Color.FromArgb(230, 126, 34), 562, 10, 240, 75);
            CreateKpiCard(pnlCards, "GIÁ TRỊ ĐƠN TRUNG BÌNH (AOV)", out lblRevAov, Color.FromArgb(142, 68, 173), 812, 10, 280, 75);

            // 3. DataGridView Doanh thu
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 0, 12, 12) };
            pnlSubRev.Controls.Add(pnlGrid);
            pnlGrid.BringToFront();

            dgvRevenue = CreateStandardGrid();
            pnlGrid.Controls.Add(dgvRevenue);
        }

        private void LoadRevenueReport()
        {
            try
            {
                int branchId = 0;
                if (cboRevBranch.SelectedValue is int bId) branchId = bId;

                DateTime from = dtpRevFrom.Value;
                DateTime to = dtpRevTo.Value;
                bool detailed = (cboRevMode.SelectedIndex == 1);

                // Nạp KPI
                var summary = ReportService.GetRevenueSummary(from, to, branchId);
                lblRevTotalAmount.Text = summary.TotalRevenue.ToString("N0", _vnCulture) + " đ";
                lblRevTotalOrders.Text = summary.TotalOrders.ToString("N0", _vnCulture) + " đơn";
                lblRevTotalDiscount.Text = summary.TotalDiscount.ToString("N0", _vnCulture) + " đ";
                lblRevAov.Text = summary.AverageOrderValue.ToString("N0", _vnCulture) + " đ/đơn";

                // Nạp Grid
                DataTable dt = ReportService.GetRevenueReport(from, to, branchId, detailed);
                dgvRevenue.DataSource = dt;

                // Format tiền
                string[] moneyCols = { "Tổng Tiền Hàng (VNĐ)", "Giảm Giá (VNĐ)", "Phí Ship (VNĐ)", "Doanh Thu Thực (VNĐ)", "Tiền Hàng (VNĐ)", "Thực Thu (VNĐ)" };
                foreach (var c in moneyCols)
                {
                    if (dgvRevenue.Columns[c] != null)
                        dgvRevenue.Columns[c].DefaultCellStyle = new DataGridViewCellStyle
                        {
                            Format = "N0",
                            Alignment = DataGridViewContentAlignment.MiddleRight
                        };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo doanh thu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ExportRevenueData(bool isPdf)
        {
            if (dgvRevenue.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string period = $"{dtpRevFrom.Value:dd/MM/yyyy} - {dtpRevTo.Value:dd/MM/yyyy}";
            string branchName = cboRevBranch.Text;
            var kpis = new Dictionary<string, string>
            {
                { "Tổng Thực Thu", lblRevTotalAmount.Text },
                { "Tổng Đơn Hoàn Thành", lblRevTotalOrders.Text },
                { "Tổng Giảm Giá", lblRevTotalDiscount.Text },
                { "Giá Trị Đơn Trung Bình", lblRevAov.Text }
            };

            if (isPdf)
            {
                await ReportExportHelper.ExportToPdfAsync(
                    dt, "Báo Cáo Doanh Thu Bán Hàng", period, branchName, kpis, null, null,
                    savedPath => ReportExportHelper.PromptOpenFile(savedPath));
            }
            else
            {
                await ReportExportHelper.ExportToExcelAsync(
                    dt, "Báo Cáo Doanh Thu Bán Hàng", period, branchName, kpis, null, null,
                    savedPath => ReportExportHelper.PromptOpenFile(savedPath));
            }
        }

        #endregion

        #region --------------- 4.2 VIEW: SẢN PHẨM BEST SELLER ---------------

        private void InitializeBestSellerSubView()
        {
            pnlSubBestSeller = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 250) };
            pnlReportSubContainer.Controls.Add(pnlSubBestSeller);

            // Filter Bar
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(12, 10, 12, 10) };
            pnlSubBestSeller.Controls.Add(pnlFilter);

            var lblFrom = new Label { Text = "Từ:", Location = new Point(12, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            dtpTopFrom = new DateTimePicker { Location = new Point(45, 13), Size = new Size(110, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 1), Font = new Font("Segoe UI", 9.5F) };

            var lblTo = new Label { Text = "Đến:", Location = new Point(160, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            dtpTopTo = new DateTimePicker { Location = new Point(200, 13), Size = new Size(110, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 30), Font = new Font("Segoe UI", 9.5F) };

            var lblBranch = new Label { Text = "Chi nhánh:", Location = new Point(318, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboTopBranch = new ComboBox { Location = new Point(390, 13), Size = new Size(155, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };

            var lblCat = new Label { Text = "Danh mục:", Location = new Point(552, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboTopCategory = new ComboBox { Location = new Point(625, 13), Size = new Size(155, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };

            var lblLimit = new Label { Text = "Hiển thị:", Location = new Point(788, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboTopLimit = new ComboBox { Location = new Point(848, 13), Size = new Size(85, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cboTopLimit.Items.AddRange(new object[] { "Top 5", "Top 10", "Top 20", "Tất cả" });
            cboTopLimit.SelectedIndex = 1; // Top 10

            var lblSort = new Label { Text = "Xếp theo:", Location = new Point(940, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboTopSortBy = new ComboBox { Location = new Point(1008, 13), Size = new Size(115, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cboTopSortBy.Items.AddRange(new object[] { "Số lượng bán", "Doanh thu" });
            cboTopSortBy.SelectedIndex = 0;

            var btnFilter = CreateActionButton("🔍 Lọc", Color.FromArgb(41, 128, 185), new Point(1130, 11), 75);
            btnFilter.Click += (s, e) => LoadBestSellerReport();

            var btnExportCsv = CreateActionButton("📊 Excel", Color.FromArgb(39, 174, 96), new Point(1210, 11), 80);
            btnExportCsv.Click += (s, e) => ExportBestSellerData(false);

            pnlFilter.Controls.AddRange(new Control[]
            {
                lblFrom, dtpTopFrom, lblTo, dtpTopTo, lblBranch, cboTopBranch,
                lblCat, cboTopCategory, lblLimit, cboTopLimit, lblSort, cboTopSortBy,
                btnFilter, btnExportCsv
            });

            // KPI Cards Panel
            var pnlCards = new Panel { Dock = DockStyle.Top, Height = 95, BackColor = Color.FromArgb(245, 246, 250), Padding = new Padding(12, 10, 12, 10) };
            pnlSubBestSeller.Controls.Add(pnlCards);

            CreateKpiCard(pnlCards, "🥇 SẢN PHẨM BÁN CHẠY NHẤT", out lblTop1Name, Color.FromArgb(211, 84, 0), 12, 10, 360, 75);
            CreateKpiCard(pnlCards, "💎 DOANH THU CAO NHẤT", out lblTop1RevName, Color.FromArgb(39, 174, 96), 382, 10, 360, 75);
            CreateKpiCard(pnlCards, "🥪 TỔNG SẢN PHẨM BÁN RA", out lblTopTotalSold, Color.FromArgb(41, 128, 185), 752, 10, 280, 75);

            // DataGridView
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 0, 12, 12) };
            pnlSubBestSeller.Controls.Add(pnlGrid);
            pnlGrid.BringToFront();

            dgvBestSeller = CreateStandardGrid();
            pnlGrid.Controls.Add(dgvBestSeller);
        }

        private void LoadBestSellerReport()
        {
            try
            {
                int branchId = 0;
                if (cboTopBranch.SelectedValue is int bId) branchId = bId;

                int catId = 0;
                if (cboTopCategory.SelectedValue is int cId) catId = cId;

                int topN = cboTopLimit.SelectedIndex == 0 ? 5 :
                           cboTopLimit.SelectedIndex == 1 ? 10 :
                           cboTopLimit.SelectedIndex == 2 ? 20 : 0;

                bool sortByRev = (cboTopSortBy.SelectedIndex == 1);

                DataTable dt = ReportService.GetBestSellerReport(dtpTopFrom.Value, dtpTopTo.Value, branchId, catId, topN, sortByRev);
                dgvBestSeller.DataSource = dt;

                // Định dạng cột
                if (dgvBestSeller.Columns["Hạng"] != null)
                {
                    dgvBestSeller.Columns["Hạng"].Width = 60;
                    dgvBestSeller.Columns["Hạng"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                if (dgvBestSeller.Columns["Số Lượng Đã Bán"] != null)
                {
                    dgvBestSeller.Columns["Số Lượng Đã Bán"].DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "N0",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    };
                }
                if (dgvBestSeller.Columns["Doanh Thu (VNĐ)"] != null)
                {
                    dgvBestSeller.Columns["Doanh Thu (VNĐ)"].DefaultCellStyle = new DataGridViewCellStyle
                    {
                        Format = "N0",
                        Alignment = DataGridViewContentAlignment.MiddleRight
                    };
                }
                if (dgvBestSeller.Columns["Tỷ Trọng (%)"] != null)
                {
                    dgvBestSeller.Columns["Tỷ Trọng (%)"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvBestSeller.Columns["Tỷ Trọng (%)"].Width = 110;
                }

                // Cập nhật KPI
                decimal totalQty = 0;
                foreach (DataRow r in dt.Rows)
                {
                    totalQty += Convert.ToDecimal(r["Số Lượng Đã Bán"]);
                }
                lblTopTotalSold.Text = totalQty.ToString("N0", _vnCulture) + " sản phẩm";

                if (dt.Rows.Count > 0)
                {
                    string top1Name = dt.Rows[0]["Tên Sản Phẩm"].ToString() ?? "";
                    string top1Qty = Convert.ToDecimal(dt.Rows[0]["Số Lượng Đã Bán"]).ToString("N0", _vnCulture) + " " + dt.Rows[0]["ĐVT"];
                    lblTop1Name.Text = $"{top1Name} ({top1Qty})";

                    // Tìm món có doanh thu cao nhất
                    DataRow[] sortedByRev = dt.Select("", "[Doanh Thu (VNĐ)] DESC");
                    if (sortedByRev.Length > 0)
                    {
                        string revName = sortedByRev[0]["Tên Sản Phẩm"].ToString() ?? "";
                        string revAmount = Convert.ToDecimal(sortedByRev[0]["Doanh Thu (VNĐ)"]).ToString("N0", _vnCulture) + " đ";
                        lblTop1RevName.Text = $"{revName} ({revAmount})";
                    }
                }
                else
                {
                    lblTop1Name.Text = "Chưa có dữ liệu";
                    lblTop1RevName.Text = "Chưa có dữ liệu";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo sản phẩm bán chạy: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ExportBestSellerData(bool isPdf)
        {
            if (dgvBestSeller.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string period = $"{dtpTopFrom.Value:dd/MM/yyyy} - {dtpTopTo.Value:dd/MM/yyyy}";
            string branchName = cboTopBranch.Text;
            var kpis = new Dictionary<string, string>
            {
                { "Món Bán Chạy Nhất", lblTop1Name.Text },
                { "Doanh Thu Cao Nhất", lblTop1RevName.Text },
                { "Tổng Sản Phẩm Bán Ra", lblTopTotalSold.Text }
            };

            if (isPdf)
            {
                await ReportExportHelper.ExportToPdfAsync(
                    dt, "Báo Cáo Sản Phẩm Best Seller", period, branchName, kpis, null, null,
                    savedPath => ReportExportHelper.PromptOpenFile(savedPath));
            }
            else
            {
                await ReportExportHelper.ExportToExcelAsync(
                    dt, "Báo Cáo Sản Phẩm Best Seller", period, branchName, kpis, null, null,
                    savedPath => ReportExportHelper.PromptOpenFile(savedPath));
            }
        }

        #endregion

        #region --------------- 4.3 VIEW: TỒN KHO & TIÊU HAO NGUYÊN LIỆU ---------------

        private void InitializeInventorySubView()
        {
            pnlSubInventory = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 250) };
            pnlReportSubContainer.Controls.Add(pnlSubInventory);

            // Filter Bar
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(12, 10, 12, 10) };
            pnlSubInventory.Controls.Add(pnlFilter);

            var lblBranch = new Label { Text = "Chi nhánh:", Location = new Point(12, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboInvBranch = new ComboBox { Location = new Point(88, 13), Size = new Size(160, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };

            var lblStatus = new Label { Text = "Tình trạng tồn:", Location = new Point(255, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboInvStatus = new ComboBox { Location = new Point(352, 13), Size = new Size(185, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cboInvStatus.Items.AddRange(new object[] { "-- Tất cả nguyên liệu --", "⚠️ Sắp hết hàng (Dưới định mức)", "An toàn" });
            cboInvStatus.SelectedIndex = 0;
            cboInvStatus.SelectedIndexChanged += (s, e) => LoadInventoryReport();

            var lblSearch = new Label { Text = "Tìm kiếm:", Location = new Point(545, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            txtInvSearch = new TextBox { Location = new Point(610, 13), Size = new Size(140, 26), Font = new Font("Segoe UI", 9.5F), PlaceholderText = "Tên nguyên liệu..." };
            txtInvSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadInventoryReport(); };

            var lblPeriod = new Label { Text = "Kỳ tiêu hao:", Location = new Point(758, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            dtpInvFrom = new DateTimePicker { Location = new Point(838, 13), Size = new Size(105, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 1), Font = new Font("Segoe UI", 9.5F) };
            dtpInvTo = new DateTimePicker { Location = new Point(948, 13), Size = new Size(105, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 30), Font = new Font("Segoe UI", 9.5F) };

            var btnFilter = CreateActionButton("🔍 Tra Cứu", Color.FromArgb(41, 128, 185), new Point(1060, 11), 90);
            btnFilter.Click += (s, e) => LoadInventoryReport();

            var btnExportCsv = CreateActionButton("📊 Excel", Color.FromArgb(39, 174, 96), new Point(1155, 11), 80);
            btnExportCsv.Click += (s, e) => ExportInventoryData(false);

            pnlFilter.Controls.AddRange(new Control[]
            {
                lblBranch, cboInvBranch, lblStatus, cboInvStatus, lblSearch, txtInvSearch,
                lblPeriod, dtpInvFrom, dtpInvTo, btnFilter, btnExportCsv
            });

            // KPI Cards Panel
            var pnlCards = new Panel { Dock = DockStyle.Top, Height = 95, BackColor = Color.FromArgb(245, 246, 250), Padding = new Padding(12, 10, 12, 10) };
            pnlSubInventory.Controls.Add(pnlCards);

            CreateKpiCard(pnlCards, "📦 TỔNG MẶT HÀNG NL", out lblInvTotalItems, Color.FromArgb(41, 128, 185), 12, 10, 240, 75);
            CreateKpiCard(pnlCards, "⚠️ CẢNH BÁO SẮP HẾT", out lblInvLowStockCount, Color.FromArgb(231, 76, 60), 262, 10, 240, 75);
            CreateKpiCard(pnlCards, "📉 TIÊU HAO BÁN HÀNG", out lblInvTotalConsumed, Color.FromArgb(230, 126, 34), 512, 10, 280, 75);
            CreateKpiCard(pnlCards, "📥 TỔNG ĐÃ NHẬP KHO", out lblInvTotalImported, Color.FromArgb(39, 174, 96), 802, 10, 280, 75);

            // DataGridView
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12, 0, 12, 12) };
            pnlSubInventory.Controls.Add(pnlGrid);
            pnlGrid.BringToFront();

            dgvInventory = CreateStandardGrid();
            dgvInventory.CellFormatting += (s, e) =>
            {
                if (e.RowIndex >= 0 && dgvInventory.Columns[e.ColumnIndex].Name == "Trạng Thái Cảnh Báo")
                {
                    string val = e.Value?.ToString() ?? "";
                    if (val.Contains("Sắp Hết"))
                    {
                        e.CellStyle!.ForeColor = Color.Red;
                        e.CellStyle!.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle!.ForeColor = Color.FromArgb(39, 174, 96);
                    }
                }
            };
            pnlGrid.Controls.Add(dgvInventory);
        }

        private void LoadInventoryReport()
        {
            try
            {
                int branchId = 0;
                if (cboInvBranch.SelectedValue is int bId) branchId = bId;

                bool onlyLow = (cboInvStatus.SelectedIndex == 1);
                string search = txtInvSearch.Text;

                DataTable dt = ReportService.GetInventoryConsumptionReport(branchId, onlyLow, search, dtpInvFrom.Value, dtpInvTo.Value);
                dgvInventory.DataSource = dt;

                // Format số
                string[] numCols = { "Tồn Kho Hiện Tại", "Tồn Tối Thiểu", "Tiêu Hao Bán Hàng", "Tổng Đã Nhập" };
                foreach (var c in numCols)
                {
                    if (dgvInventory.Columns[c] != null)
                        dgvInventory.Columns[c].DefaultCellStyle = new DataGridViewCellStyle
                        {
                            Format = "N1",
                            Alignment = DataGridViewContentAlignment.MiddleRight
                        };
                }

                // Cập nhật KPI
                int lowCount = 0;
                decimal totalConsumed = 0;
                decimal totalImported = 0;
                foreach (DataRow r in dt.Rows)
                {
                    string alert = r["Trạng Thái Cảnh Báo"].ToString() ?? "";
                    if (alert.Contains("Sắp Hết")) lowCount++;
                    totalConsumed += Convert.ToDecimal(r["Tiêu Hao Bán Hàng"]);
                    totalImported += Convert.ToDecimal(r["Tổng Đã Nhập"]);
                }

                lblInvTotalItems.Text = dt.Rows.Count.ToString("N0", _vnCulture) + " mặt hàng";
                lblInvLowStockCount.Text = lowCount.ToString("N0", _vnCulture) + " mặt hàng";
                lblInvTotalConsumed.Text = totalConsumed.ToString("N0", _vnCulture) + " (đơn vị gốc)";
                lblInvTotalImported.Text = totalImported.ToString("N0", _vnCulture) + " (đơn vị gốc)";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo tồn kho & tiêu hao: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ExportInventoryData(bool isPdf)
        {
            if (dgvInventory.DataSource is not DataTable dt || dt.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string period = $"{dtpInvFrom.Value:dd/MM/yyyy} - {dtpInvTo.Value:dd/MM/yyyy}";
            string branchName = cboInvBranch.Text;
            var kpis = new Dictionary<string, string>
            {
                { "Tổng Mặt Hàng NL", lblInvTotalItems.Text },
                { "Cảnh Báo Sắp Hết", lblInvLowStockCount.Text },
                { "Tổng Lượng Tiêu Hao Bán Hàng", lblInvTotalConsumed.Text },
                { "Tổng Lượng Đã Nhập Kho", lblInvTotalImported.Text }
            };

            if (isPdf)
            {
                await ReportExportHelper.ExportToPdfAsync(
                    dt, "Báo Cáo Tồn Kho & Tiêu Hao Nguyên Liệu", period, branchName, kpis, null, null,
                    savedPath => ReportExportHelper.PromptOpenFile(savedPath));
            }
            else
            {
                await ReportExportHelper.ExportToExcelAsync(
                    dt, "Báo Cáo Tồn Kho & Tiêu Hao Nguyên Liệu", period, branchName, kpis, null, null,
                    savedPath => ReportExportHelper.PromptOpenFile(savedPath));
            }
        }

        #endregion

        #region --------------- 4.4 VIEW: XUẤT BIỂU ĐỒ PHÂN TÍCH ---------------

        private void InitializeChartSubView()
        {
            pnlSubChart = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 250) };
            pnlReportSubContainer.Controls.Add(pnlSubChart);

            // Filter Bar
            var pnlFilter = new Panel { Dock = DockStyle.Top, Height = 56, BackColor = Color.White, Padding = new Padding(12, 10, 12, 10) };
            pnlSubChart.Controls.Add(pnlFilter);

            var lblType = new Label { Text = "Loại biểu đồ:", Location = new Point(12, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboChartType = new ComboBox { Location = new Point(102, 13), Size = new Size(310, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cboChartType.Items.AddRange(new object[]
            {
                "📈 Doanh thu theo ngày (Biểu đồ cột)",
                "📉 Xu hướng doanh thu theo ngày (Biểu đồ đường)",
                "🏆 Top 10 Món bán chạy nhất (Biểu đồ ngang)",
                "🥧 Cơ cấu doanh thu theo Danh mục (Biểu đồ Donut)",
                "🏪 So sánh doanh thu theo Chi nhánh (Biểu đồ cột)"
            });
            cboChartType.SelectedIndex = 0;
            cboChartType.SelectedIndexChanged += (s, e) => LoadChartReport();

            var lblFrom = new Label { Text = "Từ:", Location = new Point(425, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            dtpChartFrom = new DateTimePicker { Location = new Point(455, 13), Size = new Size(110, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 1), Font = new Font("Segoe UI", 9.5F) };

            var lblTo = new Label { Text = "Đến:", Location = new Point(575, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            dtpChartTo = new DateTimePicker { Location = new Point(615, 13), Size = new Size(110, 26), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 30), Font = new Font("Segoe UI", 9.5F) };

            var lblBranch = new Label { Text = "Chi nhánh:", Location = new Point(735, 16), AutoSize = true, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            cboChartBranch = new ComboBox { Location = new Point(810, 13), Size = new Size(160, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };

            var btnDraw = CreateActionButton("📊 Vẽ Lại", Color.FromArgb(41, 128, 185), new Point(980, 11), 85);
            btnDraw.Click += (s, e) => LoadChartReport();

            var btnSaveImage = CreateActionButton("💾 Lưu Ảnh PNG", Color.FromArgb(39, 174, 96), new Point(1070, 11), 125);
            btnSaveImage.Click += (s, e) => SaveChartImage();

            var btnCopyClip = CreateActionButton("📋 Sao Chép Ảnh", Color.FromArgb(52, 73, 94), new Point(1200, 11), 120);
            btnCopyClip.Click += (s, e) =>
            {
                chartAnalyticsView.CopyToClipboard();
                MessageBox.Show("Đã sao chép ảnh biểu đồ vào Clipboard! Bạn có thể dán (Ctrl+V) vào Word, Excel, Zalo...", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            pnlFilter.Controls.AddRange(new Control[]
            {
                lblType, cboChartType, lblFrom, dtpChartFrom, lblTo, dtpChartTo,
                lblBranch, cboChartBranch, btnDraw, btnSaveImage, btnCopyClip
            });

            // Vùng hiển thị biểu đồ
            var pnlCanvas = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
            pnlSubChart.Controls.Add(pnlCanvas);
            pnlCanvas.BringToFront();

            chartAnalyticsView = new ReportChartView { Dock = DockStyle.Fill };
            pnlCanvas.Controls.Add(chartAnalyticsView);
        }

        private void LoadChartReport()
        {
            try
            {
                int branchId = 0;
                if (cboChartBranch.SelectedValue is int bId) branchId = bId;

                DateTime from = dtpChartFrom.Value;
                DateTime to = dtpChartTo.Value;
                int chartIdx = cboChartType.SelectedIndex;

                string periodStr = $"Thời gian: {from:dd/MM/yyyy} - {to:dd/MM/yyyy}";

                switch (chartIdx)
                {
                    case 0: // Cột doanh thu
                        chartAnalyticsView.Title = "📈 BIỂU ĐỒ DOANH THU THEO NGÀY (CỘT)";
                        chartAnalyticsView.Subtitle = periodStr + $" | Chi nhánh: {cboChartBranch.Text}";
                        chartAnalyticsView.ChartType = ReportChartType.Bar;
                        chartAnalyticsView.DataPoints = ReportService.GetDailyRevenueChartData(from, to, branchId);
                        break;

                    case 1: // Đường doanh thu
                        chartAnalyticsView.Title = "📉 XU HƯỚNG DOANH THU THEO NGÀY (ĐƯỜNG)";
                        chartAnalyticsView.Subtitle = periodStr + $" | Chi nhánh: {cboChartBranch.Text}";
                        chartAnalyticsView.ChartType = ReportChartType.Line;
                        chartAnalyticsView.DataPoints = ReportService.GetDailyRevenueChartData(from, to, branchId);
                        break;

                    case 2: // Top 10 Best Seller
                        chartAnalyticsView.Title = "🏆 TOP 10 SẢN PHẨM BÁN CHẠY NHẤT (SỐ LƯỢNG BÁN)";
                        chartAnalyticsView.Subtitle = periodStr + $" | Chi nhánh: {cboChartBranch.Text}";
                        chartAnalyticsView.ChartType = ReportChartType.HorizontalBar;
                        chartAnalyticsView.DataPoints = ReportService.GetTopProductsChartData(from, to, branchId, 10);
                        break;

                    case 3: // Donut cơ cấu danh mục
                        chartAnalyticsView.Title = "🥧 CƠ CẤU DOANH THU THEO DANH MỤC SẢN PHẨM";
                        chartAnalyticsView.Subtitle = periodStr + $" | Chi nhánh: {cboChartBranch.Text}";
                        chartAnalyticsView.ChartType = ReportChartType.Pie;
                        chartAnalyticsView.DataPoints = ReportService.GetCategoryRevenueChartData(from, to, branchId);
                        break;

                    case 4: // Cột so sánh chi nhánh
                        chartAnalyticsView.Title = "🏪 SO SÁNH DOANH THU GIỮA CÁC CHI NHÁNH";
                        chartAnalyticsView.Subtitle = periodStr;
                        chartAnalyticsView.ChartType = ReportChartType.Bar;
                        chartAnalyticsView.DataPoints = ReportService.GetBranchRevenueChartData(from, to);
                        break;
                }

                chartAnalyticsView.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị biểu đồ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveChartImage()
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "Hình ảnh PNG (*.png)|*.png|Tất cả tệp (*.*)|*.*",
                FileName = $"BieuDo_{DateTime.Now:yyyyMMdd_HHmmss}.png",
                Title = "Lưu Biểu Đồ Ra Hình Ảnh"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    chartAnalyticsView.SaveAsImage(sfd.FileName);
                    ReportExportHelper.PromptOpenFile(sfd.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu ảnh biểu đồ: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region --------------- 4.5 VIEW: XUẤT FILE BÁO CÁO ---------------

        private void InitializeExportSubView()
        {
            pnlSubExport = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 250), Padding = new Padding(15) };
            pnlReportSubContainer.Controls.Add(pnlSubExport);

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 380,
                IsSplitterFixed = false
            };
            pnlSubExport.Controls.Add(split);

            // ================= BÊN TRÁI: CẤU HÌNH XUẤT =================
            var grpConfig = new GroupBox
            {
                Text = "⚙️ THIẾT LẬP THÔNG SỐ XUẤT FILE",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(15)
            };
            split.Panel1.Controls.Add(grpConfig);

            // 1. Chọn loại báo cáo
            var grpType = new GroupBox { Text = "1. Chọn loại báo cáo", Location = new Point(15, 25), Size = new Size(340, 115), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            rdoExpRev = new RadioButton { Text = "💰 Báo cáo Doanh thu bán hàng", Location = new Point(15, 24), Size = new Size(300, 24), Checked = true, Font = new Font("Segoe UI", 9.5F) };
            rdoExpBestSeller = new RadioButton { Text = "🏆 Báo cáo Sản phẩm Best Seller", Location = new Point(15, 52), Size = new Size(300, 24), Font = new Font("Segoe UI", 9.5F) };
            rdoExpInv = new RadioButton { Text = "📦 Báo cáo Tồn kho & Tiêu hao nguyên liệu", Location = new Point(15, 80), Size = new Size(310, 24), Font = new Font("Segoe UI", 9.5F) };
            grpType.Controls.AddRange(new Control[] { rdoExpRev, rdoExpBestSeller, rdoExpInv });

            // 2. Phạm vi dữ liệu
            var grpRange = new GroupBox { Text = "2. Phạm vi dữ liệu", Location = new Point(15, 145), Size = new Size(340, 135), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            var lblF = new Label { Text = "Từ ngày:", Location = new Point(15, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            dtpExpFrom = new DateTimePicker { Location = new Point(90, 25), Size = new Size(130, 24), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 1), Font = new Font("Segoe UI", 9F) };

            var lblT = new Label { Text = "Đến ngày:", Location = new Point(15, 63), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            dtpExpTo = new DateTimePicker { Location = new Point(90, 60), Size = new Size(130, 24), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 30), Font = new Font("Segoe UI", 9F) };

            var lblB = new Label { Text = "Chi nhánh:", Location = new Point(15, 98), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            cboExpBranch = new ComboBox { Location = new Point(90, 95), Size = new Size(235, 24), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            grpRange.Controls.AddRange(new Control[] { lblF, dtpExpFrom, lblT, dtpExpTo, lblB, cboExpBranch });

            // 3. Định dạng xuất
            var grpFormat = new GroupBox { Text = "3. Định dạng tệp xuất", Location = new Point(15, 285), Size = new Size(340, 130), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            rdoExpExcel = new RadioButton { Text = "📊 Tệp Microsoft Excel (*.xlsx)", Location = new Point(15, 22), Size = new Size(310, 24), Checked = true, Font = new Font("Segoe UI", 9.5F) };
            rdoExpPdf = new RadioButton { Text = "📕 Tệp Adobe PDF (*.pdf)", Location = new Point(15, 48), Size = new Size(310, 24), Font = new Font("Segoe UI", 9.5F) };
            rdoExpCsv = new RadioButton { Text = "📄 Tệp CSV chuẩn (*.csv UTF-8)", Location = new Point(15, 74), Size = new Size(310, 24), Font = new Font("Segoe UI", 9.5F) };
            rdoExpHtml = new RadioButton { Text = "🌐 Báo cáo HTML (*.html)", Location = new Point(15, 100), Size = new Size(310, 24), Font = new Font("Segoe UI", 9.5F) };
            grpFormat.Controls.AddRange(new Control[] { rdoExpExcel, rdoExpPdf, rdoExpCsv, rdoExpHtml });

            // Buttons
            var btnPreview = new Button
            {
                Text = "🔍 Xem Trước Dữ Liệu (Preview)",
                Location = new Point(15, 425),
                Size = new Size(340, 35),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPreview.FlatAppearance.BorderSize = 0;
            btnPreview.Click += (s, e) => LoadExportPreview();

            var btnPrintPreview = new Button
            {
                Text = "🖨️ Xem Trước Bản In & In (Print Preview)",
                Location = new Point(15, 466),
                Size = new Size(340, 35),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPrintPreview.FlatAppearance.BorderSize = 0;
            btnPrintPreview.Click += (s, e) => ExecutePrintPreview();

            var btnExecute = new Button
            {
                Text = "🚀 TIẾN HÀNH XUẤT FILE BÁO CÁO",
                Location = new Point(15, 508),
                Size = new Size(340, 48),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExecute.FlatAppearance.BorderSize = 0;
            btnExecute.Click += (s, e) => ExecuteExportFile(btnExecute);

            prgExport = new ProgressBar
            {
                Location = new Point(15, 564),
                Size = new Size(340, 22),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Style = ProgressBarStyle.Continuous
            };

            grpConfig.Controls.AddRange(new Control[] { grpType, grpRange, grpFormat, btnPreview, btnPrintPreview, btnExecute, prgExport });

            // ================= BÊN PHẢI: XEM TRƯỚC (PREVIEW) =================
            var grpPreview = new GroupBox
            {
                Text = "📋 XEM TRƯỚC BÁO CÁO (PREVIEW)",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Padding = new Padding(12)
            };
            split.Panel2.Controls.Add(grpPreview);

            lblExpStatus = new Label
            {
                Text = "Sẵn sàng xuất báo cáo...",
                Dock = DockStyle.Top,
                Height = 28,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(41, 128, 185)
            };
            grpPreview.Controls.Add(lblExpStatus);

            dgvExpPreview = CreateStandardGrid();
            grpPreview.Controls.Add(dgvExpPreview);
            dgvExpPreview.BringToFront();

            // Event đổi radio button
            rdoExpRev.CheckedChanged += (s, e) => { if (rdoExpRev.Checked) LoadExportPreview(); };
            rdoExpBestSeller.CheckedChanged += (s, e) => { if (rdoExpBestSeller.Checked) LoadExportPreview(); };
            rdoExpInv.CheckedChanged += (s, e) => { if (rdoExpInv.Checked) LoadExportPreview(); };
        }

        private void LoadExportPreview()
        {
            try
            {
                int branchId = 0;
                if (cboExpBranch.SelectedValue is int bId) branchId = bId;

                DateTime from = dtpExpFrom.Value;
                DateTime to = dtpExpTo.Value;
                DataTable dt;
                string reportName;

                if (rdoExpRev.Checked)
                {
                    reportName = "Báo cáo Doanh thu";
                    dt = ReportService.GetRevenueReport(from, to, branchId, false);
                }
                else if (rdoExpBestSeller.Checked)
                {
                    reportName = "Báo cáo Sản phẩm Best Seller";
                    dt = ReportService.GetBestSellerReport(from, to, branchId, 0, 0, false);
                }
                else
                {
                    reportName = "Báo cáo Tồn kho & Tiêu hao";
                    dt = ReportService.GetInventoryConsumptionReport(branchId, false, "", from, to);
                }

                dgvExpPreview.DataSource = dt;
                lblExpStatus.Text = $"Đang hiển thị xem trước {reportName} ({dt.Rows.Count} dòng dữ liệu).";
            }
            catch (Exception ex)
            {
                lblExpStatus.Text = "Lỗi khi tải dữ liệu xem trước: " + ex.Message;
            }
        }

        private void ExecutePrintPreview()
        {
            try
            {
                int branchId = 0;
                if (cboExpBranch.SelectedValue is int bId) branchId = bId;

                DateTime from = dtpExpFrom.Value;
                DateTime to = dtpExpTo.Value;
                string period = $"{from:dd/MM/yyyy} - {to:dd/MM/yyyy}";
                string branchName = cboExpBranch.Text;

                DataTable dt;
                string title;
                var kpis = new Dictionary<string, string>();

                if (rdoExpRev.Checked)
                {
                    title = "Báo Cáo Doanh Thu Bán Hàng";
                    dt = ReportService.GetRevenueReport(from, to, branchId, false);
                    var sum = ReportService.GetRevenueSummary(from, to, branchId);
                    kpis["Tổng Thực Thu"] = sum.TotalRevenue.ToString("N0", _vnCulture) + " đ";
                    kpis["Tổng Đơn Hàng"] = sum.TotalOrders.ToString("N0", _vnCulture) + " đơn";
                    kpis["Tổng Giảm Giá"] = sum.TotalDiscount.ToString("N0", _vnCulture) + " đ";
                    kpis["Giá Trị Đơn Trung Bình"] = sum.AverageOrderValue.ToString("N0", _vnCulture) + " đ";
                }
                else if (rdoExpBestSeller.Checked)
                {
                    title = "Báo Cáo Sản Phẩm Bán Chạy (Best Seller)";
                    dt = ReportService.GetBestSellerReport(from, to, branchId, 0, 0, false);
                    decimal totalSold = 0;
                    foreach (DataRow r in dt.Rows) totalSold += Convert.ToDecimal(r["Số Lượng Đã Bán"]);
                    kpis["Tổng Sản Phẩm Bán Ra"] = totalSold.ToString("N0", _vnCulture);
                    if (dt.Rows.Count > 0)
                        kpis["Món Bán Chạy Nhất"] = dt.Rows[0]["Tên Sản Phẩm"].ToString() ?? "";
                }
                else
                {
                    title = "Báo Cáo Tồn Kho & Tiêu Hao Nguyên Liệu";
                    dt = ReportService.GetInventoryConsumptionReport(branchId, false, "", from, to);
                    int lowCount = 0;
                    decimal totalConsumed = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["Trạng Thái Cảnh Báo"].ToString()!.Contains("Sắp Hết")) lowCount++;
                        totalConsumed += Convert.ToDecimal(r["Tiêu Hao Bán Hàng"]);
                    }
                    kpis["Tổng Mặt Hàng"] = dt.Rows.Count.ToString();
                    kpis["Cảnh Báo Sắp Hết"] = lowCount.ToString() + " mặt hàng";
                    kpis["Tổng Lượng Tiêu Hao"] = totalConsumed.ToString("N0", _vnCulture);
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn để in!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                ReportExportHelper.ShowPrintPreview(this, dt, title, period, branchName, kpis);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xem trước bản in: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ExecuteExportFile(Button? triggerButton = null)
        {
            try
            {
                int branchId = 0;
                if (cboExpBranch.SelectedValue is int bId) branchId = bId;

                DateTime from = dtpExpFrom.Value;
                DateTime to = dtpExpTo.Value;
                string period = $"{from:dd/MM/yyyy} - {to:dd/MM/yyyy}";
                string branchName = cboExpBranch.Text;

                DataTable dt;
                string title;
                var kpis = new Dictionary<string, string>();

                lblExpStatus.Text = "Đang chuẩn bị dữ liệu báo cáo...";
                prgExport.Value = 10;

                if (rdoExpRev.Checked)
                {
                    title = "Báo Cáo Doanh Thu Bán Hàng";
                    dt = ReportService.GetRevenueReport(from, to, branchId, false);
                    var sum = ReportService.GetRevenueSummary(from, to, branchId);
                    kpis["Tổng Thực Thu"] = sum.TotalRevenue.ToString("N0", _vnCulture) + " đ";
                    kpis["Tổng Đơn Hàng"] = sum.TotalOrders.ToString("N0", _vnCulture) + " đơn";
                    kpis["Tổng Giảm Giá"] = sum.TotalDiscount.ToString("N0", _vnCulture) + " đ";
                    kpis["Giá Trị Đơn Trung Bình"] = sum.AverageOrderValue.ToString("N0", _vnCulture) + " đ";
                }
                else if (rdoExpBestSeller.Checked)
                {
                    title = "Báo Cáo Sản Phẩm Bán Chạy (Best Seller)";
                    dt = ReportService.GetBestSellerReport(from, to, branchId, 0, 0, false);
                    decimal totalSold = 0;
                    foreach (DataRow r in dt.Rows) totalSold += Convert.ToDecimal(r["Số Lượng Đã Bán"]);
                    kpis["Tổng Sản Phẩm Bán Ra"] = totalSold.ToString("N0", _vnCulture);
                    if (dt.Rows.Count > 0)
                        kpis["Món Bán Chạy Nhất"] = dt.Rows[0]["Tên Sản Phẩm"].ToString() ?? "";
                }
                else
                {
                    title = "Báo Cáo Tồn Kho & Tiêu Hao Nguyên Liệu";
                    dt = ReportService.GetInventoryConsumptionReport(branchId, false, "", from, to);
                    int lowCount = 0;
                    decimal totalConsumed = 0;
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["Trạng Thái Cảnh Báo"].ToString()!.Contains("Sắp Hết")) lowCount++;
                        totalConsumed += Convert.ToDecimal(r["Tiêu Hao Bán Hàng"]);
                    }
                    kpis["Tổng Mặt Hàng"] = dt.Rows.Count.ToString();
                    kpis["Cảnh Báo Sắp Hết"] = lowCount.ToString() + " mặt hàng";
                    kpis["Tổng Lượng Tiêu Hao"] = totalConsumed.ToString("N0", _vnCulture);
                }

                if (dt.Rows.Count == 0)
                {
                    prgExport.Value = 0;
                    lblExpStatus.Text = "Không có dữ liệu trong khoảng thời gian đã chọn.";
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn để xuất file!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dgvExpPreview.DataSource = dt;

                if (triggerButton != null) triggerButton.Enabled = false;
                Cursor = Cursors.WaitCursor;

                var progress = new Progress<int>(p =>
                {
                    prgExport.Value = Math.Clamp(p, 0, 100);
                });

                var statusProgress = new Progress<string>(msg =>
                {
                    lblExpStatus.Text = msg;
                });

                if (rdoExpExcel.Checked)
                {
                    await ReportExportHelper.ExportToExcelAsync(
                        dt, title, period, branchName, kpis, progress, statusProgress,
                        savedPath => ReportExportHelper.PromptOpenFile(savedPath));
                }
                else if (rdoExpPdf.Checked)
                {
                    await ReportExportHelper.ExportToPdfAsync(
                        dt, title, period, branchName, kpis, progress, statusProgress,
                        savedPath => ReportExportHelper.PromptOpenFile(savedPath));
                }
                else if (rdoExpCsv.Checked)
                {
                    if (ReportExportHelper.ExportToCsv(dt, title, period, branchName, kpis, out string savedPath))
                    {
                        prgExport.Value = 100;
                        lblExpStatus.Text = "Xuất file CSV thành công!";
                        ReportExportHelper.PromptOpenFile(savedPath);
                    }
                }
                else
                {
                    if (ReportExportHelper.ExportToHtml(dt, title, period, branchName, kpis, out string savedPath))
                    {
                        prgExport.Value = 100;
                        lblExpStatus.Text = "Xuất file HTML thành công!";
                        ReportExportHelper.PromptOpenFile(savedPath);
                    }
                }
            }
            catch (Exception ex)
            {
                lblExpStatus.Text = "Lỗi xuất file: " + ex.Message;
                MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (triggerButton != null) triggerButton.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region --------------- 4.6 HELPER METHODS CHO REPORT ---------------

        private void LoadReportsTab()
        {
            try
            {
                // Nạp danh sách chi nhánh và danh mục vào các ComboBox
                DataTable dtBranches = ReportService.GetBranches();
                BindCombo(cboRptDashBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");
                BindCombo(cboRevBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");
                BindCombo(cboTopBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");
                BindCombo(cboInvBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");
                BindCombo(cboChartBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");
                BindCombo(cboExpBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");

                // Nếu MANAGER có chi nhánh cụ thể (> 0) thì mặc định chọn chi nhánh của mình
                if (!UserSession.IsAdmin && UserSession.MaChiNhanh > 0)
                {
                    cboRptDashBranch.SelectedValue = UserSession.MaChiNhanh;
                    cboRevBranch.SelectedValue = UserSession.MaChiNhanh;
                    cboTopBranch.SelectedValue = UserSession.MaChiNhanh;
                    cboInvBranch.SelectedValue = UserSession.MaChiNhanh;
                    cboChartBranch.SelectedValue = UserSession.MaChiNhanh;
                    cboExpBranch.SelectedValue = UserSession.MaChiNhanh;
                }

                DataTable dtCats = ReportService.GetCategories();
                BindCombo(cboTopCategory, dtCats, "TenDanhMuc", "MaDanhMuc");

                // Nạp view hiện tại
                SwitchReportSubTab(_currentReportSubIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo Báo Cáo & Thống Kê: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void BindCombo(ComboBox cbo, DataTable dt, string displayMember, string valueMember)
        {
            cbo.DataSource = dt;
            cbo.DisplayMember = displayMember;
            cbo.ValueMember = valueMember;
            if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
        }

        private static void CreateKpiCard(Panel parent, string title, out Label valLabel, Color bg, int x, int y, int w, int h)
        {
            var card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = bg
            };

            var lblT = new Label
            {
                Text = title,
                Location = new Point(14, 10),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                AutoSize = true
            };

            valLabel = new Label
            {
                Text = "0",
                Location = new Point(14, 34),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                AutoSize = true
            };

            card.Controls.AddRange(new Control[] { lblT, valLabel });
            parent.Controls.Add(card);
        }

        private static Button CreateActionButton(string text, Color bg, Point loc, int width)
        {
            var btn = new Button
            {
                Text = text,
                Location = loc,
                Size = new Size(width, 32),
                BackColor = bg,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private static DataGridView CreateStandardGrid()
        {
            var dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9.5F),
                RowHeadersWidth = 30,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            return dgv;
        }

        #endregion

        #endregion

        #region ======================== 4. MODULE NHÂN VIÊN ========================

        private DataGridView dgvEmployees = null!;

        private void InitializeEmployeesTab()
        {
            tabEmployees.BackColor = Color.FromArgb(245, 246, 250);

            var grp = new GroupBox
            {
                Text = "👥 DANH SÁCH NHÂN VIÊN & TÀI KHOẢN HỆ THỐNG",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Padding = new Padding(15)
            };
            tabEmployees.Controls.Add(grp);

            dgvEmployees = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            grp.Controls.Add(dgvEmployees);
        }

        private void LoadEmployeesTab()
        {
            try
            {
                DataTable dt = EmployeeService.GetAllWithAccounts();
                dgvEmployees.DataSource = dt;
                if (dgvEmployees.Columns["EmployeeID"] != null)
                    dgvEmployees.Columns["EmployeeID"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
