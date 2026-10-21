namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormOrder
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            tlpHeader = new TableLayoutPanel();
            btnMenu = new Button();
            btnOrders = new Button();
            lblClock = new Label();
            btnSettings = new Button();
            mnuMain = new ContextMenuStrip(components);
            mnuOrders = new ContextMenuStrip(components);
            mnuSettings = new ContextMenuStrip(components);
            tlpBody = new TableLayoutPanel();
            pnlCategoryHost = new Panel();
            flpCategories = new FlowLayoutPanel();
            pnlMenuCard = new Panel();
            tlpMenu = new TableLayoutPanel();
            flpProducts = new FlowLayoutPanel();
            pnlOrderCard = new Panel();
            tlpOrder = new TableLayoutPanel();
            tlpCustomer = new TableLayoutPanel();
            lblCustomerCaption = new Label();
            txtCustomerPhone = new TextBox();
            btnFindCustomer = new Button();
            btnClearCustomer = new Button();
            lblCustomerInfo = new Label();
            flpOrderType = new FlowLayoutPanel();
            btnTypeMangDi = new Button();
            btnTypeTaiQuay = new Button();
            btnTypeGiaoHang = new Button();
            lblSource = new Label();
            cboSource = new ComboBox();
            tlpDelivery = new TableLayoutPanel();
            lblAddressCaption = new Label();
            txtAddress = new TextBox();
            lblShipFeeCaption = new Label();
            numShipFee = new NumericUpDown();
            dgvCart = new DataGridView();
            colName = new DataGridViewTextBoxColumn();
            colPrice = new DataGridViewTextBoxColumn();
            colMinus = new DataGridViewButtonColumn();
            colQty = new DataGridViewTextBoxColumn();
            colPlus = new DataGridViewButtonColumn();
            colDiscount = new DataGridViewTextBoxColumn();
            colTotal = new DataGridViewTextBoxColumn();
            colMore = new DataGridViewButtonColumn();
            tlpCheckout = new TableLayoutPanel();
            tlpPaymentOptions = new TableLayoutPanel();
            tlpPromo = new TableLayoutPanel();
            btnPromo = new Button();
            lblPoints = new Label();
            numRedeemPoints = new NumericUpDown();
            lblRedeemHint = new Label();
            tlpPay = new TableLayoutPanel();
            btnPayTienMat = new Button();
            btnPayChuyenKhoan = new Button();
            btnPayThe = new Button();
            btnPayVi = new Button();
            tlpMoney = new TableLayoutPanel();
            lblMoneyCaption = new Label();
            pnlMoney = new Panel();
            txtCustomerMoney = new TextBox();
            txtPaymentRef = new TextBox();
            lblChange = new Label();
            tlpQuick = new TableLayoutPanel();
            flpQuickCash = new FlowLayoutPanel();
            chkPrintReceipt = new CheckBox();
            tlpTotals = new TableLayoutPanel();
            tlpTotalLines = new TableLayoutPanel();
            lblSubtotalCaption = new Label();
            lblSubtotal = new Label();
            lblDiscountCaption = new Label();
            lblDiscount = new Label();
            lblShipCaption = new Label();
            lblShip = new Label();
            lblFinalCaption = new Label();
            lblFinal = new Label();
            tlpActions = new TableLayoutPanel();
            btnClearOrder = new Button();
            btnParkOrder = new Button();
            btnCheckout = new Button();
            mnuParked = new ContextMenuStrip(components);
            mnuCartRow = new ContextMenuStrip(components);
            ttpOrder = new ToolTip(components);
            tmrClock = new System.Windows.Forms.Timer(components);
            tlpHeader.SuspendLayout();
            tlpBody.SuspendLayout();
            pnlCategoryHost.SuspendLayout();
            pnlMenuCard.SuspendLayout();
            tlpMenu.SuspendLayout();
            pnlOrderCard.SuspendLayout();
            tlpOrder.SuspendLayout();
            tlpCustomer.SuspendLayout();
            flpOrderType.SuspendLayout();
            tlpDelivery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numShipFee).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCart).BeginInit();
            tlpCheckout.SuspendLayout();
            tlpPaymentOptions.SuspendLayout();
            tlpPromo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numRedeemPoints).BeginInit();
            tlpPay.SuspendLayout();
            tlpMoney.SuspendLayout();
            pnlMoney.SuspendLayout();
            tlpQuick.SuspendLayout();
            tlpTotals.SuspendLayout();
            tlpTotalLines.SuspendLayout();
            tlpActions.SuspendLayout();
            SuspendLayout();
            // 
            // tlpHeader
            // 
            tlpHeader.BackColor = Color.FromArgb(242, 153, 0);
            tlpHeader.ColumnCount = 5;
            tlpBody.SetColumnSpan(tlpHeader, 2);
            tlpHeader.ColumnStyles.Add(new ColumnStyle());
            tlpHeader.ColumnStyles.Add(new ColumnStyle());
            tlpHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpHeader.ColumnStyles.Add(new ColumnStyle());
            tlpHeader.ColumnStyles.Add(new ColumnStyle());
            tlpHeader.Controls.Add(btnMenu, 0, 0);
            tlpHeader.Controls.Add(btnOrders, 1, 0);
            tlpHeader.Controls.Add(lblClock, 3, 0);
            tlpHeader.Controls.Add(btnSettings, 4, 0);
            tlpHeader.Dock = DockStyle.Fill;
            tlpHeader.Location = new Point(0, 0);
            tlpHeader.Margin = new Padding(0);
            tlpHeader.Name = "tlpHeader";
            tlpHeader.Padding = new Padding(8, 0, 8, 0);
            tlpHeader.RowCount = 1;
            tlpHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpHeader.Size = new Size(768, 52);
            tlpHeader.TabIndex = 0;
            // 
            // btnMenu
            // 
            btnMenu.Anchor = AnchorStyles.Left;
            btnMenu.BackColor = Color.FromArgb(204, 122, 0);
            btnMenu.Cursor = Cursors.Hand;
            btnMenu.FlatAppearance.BorderSize = 0;
            btnMenu.FlatAppearance.MouseOverBackColor = Color.FromArgb(187, 112, 0);
            btnMenu.FlatStyle = FlatStyle.Flat;
            btnMenu.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            btnMenu.ForeColor = Color.White;
            btnMenu.Location = new Point(8, 8);
            btnMenu.Margin = new Padding(0, 0, 8, 0);
            btnMenu.Name = "btnMenu";
            btnMenu.Size = new Size(40, 36);
            btnMenu.TabIndex = 0;
            btnMenu.TabStop = false;
            btnMenu.Text = "☰";
            btnMenu.UseVisualStyleBackColor = false;
            btnMenu.Click += btnMenu_Click;
            // 
            // btnOrders
            // 
            btnOrders.Anchor = AnchorStyles.Left;
            btnOrders.AutoSize = true;
            btnOrders.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnOrders.BackColor = Color.FromArgb(204, 122, 0);
            btnOrders.Cursor = Cursors.Hand;
            btnOrders.FlatAppearance.BorderSize = 0;
            btnOrders.FlatAppearance.MouseOverBackColor = Color.FromArgb(187, 112, 0);
            btnOrders.FlatStyle = FlatStyle.Flat;
            btnOrders.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnOrders.ForeColor = Color.White;
            btnOrders.Location = new Point(56, 6);
            btnOrders.Margin = new Padding(0, 0, 4, 0);
            btnOrders.Name = "btnOrders";
            btnOrders.Padding = new Padding(12, 4, 12, 4);
            btnOrders.Size = new Size(119, 39);
            btnOrders.TabIndex = 1;
            btnOrders.TabStop = false;
            btnOrders.Text = "Đơn hàng";
            btnOrders.UseMnemonic = false;
            btnOrders.UseVisualStyleBackColor = false;
            btnOrders.Click += btnOrders_Click;
            // 
            // lblClock
            // 
            lblClock.Anchor = AnchorStyles.None;
            lblClock.AutoSize = true;
            lblClock.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblClock.ForeColor = Color.White;
            lblClock.Location = new Point(545, 13);
            lblClock.Margin = new Padding(8, 0, 12, 0);
            lblClock.Name = "lblClock";
            lblClock.Size = new Size(163, 25);
            lblClock.TabIndex = 2;
            lblClock.Text = "00:00  01/01/2026";
            lblClock.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSettings
            // 
            btnSettings.Anchor = AnchorStyles.Right;
            btnSettings.BackColor = Color.FromArgb(204, 122, 0);
            btnSettings.Cursor = Cursors.Hand;
            btnSettings.FlatAppearance.BorderSize = 0;
            btnSettings.FlatAppearance.MouseOverBackColor = Color.FromArgb(187, 112, 0);
            btnSettings.FlatStyle = FlatStyle.Flat;
            btnSettings.Font = new Font("Segoe UI Symbol", 14F);
            btnSettings.ForeColor = Color.White;
            btnSettings.Location = new Point(720, 8);
            btnSettings.Margin = new Padding(0);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(40, 36);
            btnSettings.TabIndex = 4;
            btnSettings.TabStop = false;
            btnSettings.Text = "⚙";
            btnSettings.UseVisualStyleBackColor = false;
            btnSettings.Click += btnSettings_Click;
            // 
            // mnuMain
            // 
            mnuMain.Font = new Font("Segoe UI", 10F);
            mnuMain.ImageScalingSize = new Size(20, 20);
            mnuMain.Name = "mnuMain";
            mnuMain.ShowImageMargin = false;
            mnuMain.Size = new Size(36, 4);
            // 
            // mnuOrders
            // 
            mnuOrders.Font = new Font("Segoe UI", 10F);
            mnuOrders.ImageScalingSize = new Size(20, 20);
            mnuOrders.Name = "mnuOrders";
            mnuOrders.ShowImageMargin = false;
            mnuOrders.Size = new Size(36, 4);
            // 
            // mnuSettings
            // 
            mnuSettings.Font = new Font("Segoe UI", 10F);
            mnuSettings.ImageScalingSize = new Size(20, 20);
            mnuSettings.Name = "mnuSettings";
            mnuSettings.ShowImageMargin = false;
            mnuSettings.Size = new Size(36, 4);
            // 
            // tlpBody
            // 
            tlpBody.BackColor = Color.FromArgb(244, 245, 247);
            tlpBody.ColumnCount = 3;
            tlpBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tlpBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tlpBody.Controls.Add(tlpHeader, 0, 0);
            tlpBody.Controls.Add(pnlCategoryHost, 0, 1);
            tlpBody.Controls.Add(pnlMenuCard, 1, 1);
            tlpBody.Controls.Add(pnlOrderCard, 2, 0);
            tlpBody.Dock = DockStyle.Fill;
            tlpBody.Location = new Point(0, 0);
            tlpBody.Margin = new Padding(0);
            tlpBody.Name = "tlpBody";
            tlpBody.RowCount = 2;
            tlpBody.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tlpBody.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpBody.Size = new Size(1280, 720);
            tlpBody.TabIndex = 0;
            // 
            // pnlCategoryHost
            // 
            pnlCategoryHost.BackColor = Color.FromArgb(248, 246, 242);
            pnlCategoryHost.Controls.Add(flpCategories);
            pnlCategoryHost.Dock = DockStyle.Fill;
            pnlCategoryHost.Location = new Point(0, 52);
            pnlCategoryHost.Margin = new Padding(0);
            pnlCategoryHost.Name = "pnlCategoryHost";
            pnlCategoryHost.Padding = new Padding(3, 4, 2, 4);
            pnlCategoryHost.Size = new Size(128, 668);
            pnlCategoryHost.TabIndex = 1;
            pnlCategoryHost.Resize += pnlCategoryHost_Resize;
            // 
            // flpCategories
            // 
            flpCategories.AutoScroll = true;
            flpCategories.BackColor = Color.FromArgb(248, 246, 242);
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.Location = new Point(0, 0);
            flpCategories.Margin = new Padding(0);
            flpCategories.Name = "flpCategories";
            flpCategories.Size = new Size(103, 400);
            flpCategories.TabIndex = 0;
            flpCategories.WrapContents = false;
            // 
            // pnlMenuCard
            // 
            pnlMenuCard.BackColor = Color.FromArgb(248, 246, 242);
            pnlMenuCard.Controls.Add(tlpMenu);
            pnlMenuCard.Dock = DockStyle.Fill;
            pnlMenuCard.Location = new Point(128, 52);
            pnlMenuCard.Margin = new Padding(0);
            pnlMenuCard.Name = "pnlMenuCard";
            pnlMenuCard.Padding = new Padding(8);
            pnlMenuCard.Size = new Size(640, 668);
            pnlMenuCard.TabIndex = 1;
            // 
            // tlpMenu
            // 
            tlpMenu.BackColor = Color.FromArgb(248, 246, 242);
            tlpMenu.ColumnCount = 1;
            tlpMenu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMenu.Controls.Add(flpProducts, 0, 0);
            tlpMenu.Dock = DockStyle.Fill;
            tlpMenu.Location = new Point(8, 8);
            tlpMenu.Name = "tlpMenu";
            tlpMenu.RowCount = 1;
            tlpMenu.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMenu.Size = new Size(624, 652);
            tlpMenu.TabIndex = 0;
            // 
            // flpProducts
            // 
            flpProducts.AutoScroll = true;
            flpProducts.BackColor = Color.FromArgb(248, 246, 242);
            flpProducts.Dock = DockStyle.Fill;
            flpProducts.Location = new Point(0, 0);
            flpProducts.Margin = new Padding(0);
            flpProducts.Name = "flpProducts";
            flpProducts.Size = new Size(624, 652);
            flpProducts.TabIndex = 0;
            flpProducts.Resize += flpProducts_Resize;
            // 
            // pnlOrderCard
            // 
            pnlOrderCard.BackColor = Color.White;
            pnlOrderCard.Controls.Add(tlpOrder);
            pnlOrderCard.Dock = DockStyle.Fill;
            pnlOrderCard.Location = new Point(768, 0);
            pnlOrderCard.Margin = new Padding(0);
            pnlOrderCard.Name = "pnlOrderCard";
            pnlOrderCard.Padding = new Padding(10, 8, 10, 8);
            tlpBody.SetRowSpan(pnlOrderCard, 2);
            pnlOrderCard.Size = new Size(512, 720);
            pnlOrderCard.TabIndex = 2;
            // 
            // tlpOrder
            // 
            tlpOrder.ColumnCount = 1;
            tlpOrder.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpOrder.Controls.Add(tlpCustomer, 0, 0);
            tlpOrder.Controls.Add(flpOrderType, 0, 1);
            tlpOrder.Controls.Add(tlpDelivery, 0, 2);
            tlpOrder.Controls.Add(dgvCart, 0, 3);
            tlpOrder.Controls.Add(tlpCheckout, 0, 5);
            tlpOrder.Dock = DockStyle.Fill;
            tlpOrder.Location = new Point(10, 8);
            tlpOrder.Name = "tlpOrder";
            tlpOrder.RowCount = 6;
            tlpOrder.RowStyles.Add(new RowStyle());
            tlpOrder.RowStyles.Add(new RowStyle());
            tlpOrder.RowStyles.Add(new RowStyle());
            tlpOrder.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tlpOrder.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpOrder.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tlpOrder.Size = new Size(492, 704);
            tlpOrder.TabIndex = 0;
            // 
            // tlpCustomer
            // 
            tlpCustomer.AutoSize = true;
            tlpCustomer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpCustomer.ColumnCount = 5;
            tlpCustomer.ColumnStyles.Add(new ColumnStyle());
            tlpCustomer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            tlpCustomer.ColumnStyles.Add(new ColumnStyle());
            tlpCustomer.ColumnStyles.Add(new ColumnStyle());
            tlpCustomer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCustomer.Controls.Add(lblCustomerCaption, 0, 0);
            tlpCustomer.Controls.Add(txtCustomerPhone, 1, 0);
            tlpCustomer.Controls.Add(btnFindCustomer, 2, 0);
            tlpCustomer.Controls.Add(btnClearCustomer, 3, 0);
            tlpCustomer.Controls.Add(lblCustomerInfo, 4, 0);
            tlpCustomer.Dock = DockStyle.Fill;
            tlpCustomer.Location = new Point(0, 0);
            tlpCustomer.Margin = new Padding(0);
            tlpCustomer.Name = "tlpCustomer";
            tlpCustomer.RowCount = 1;
            tlpCustomer.RowStyles.Add(new RowStyle());
            tlpCustomer.Size = new Size(492, 37);
            tlpCustomer.TabIndex = 0;
            // 
            // lblCustomerCaption
            // 
            lblCustomerCaption.Anchor = AnchorStyles.Left;
            lblCustomerCaption.AutoSize = true;
            lblCustomerCaption.ForeColor = Color.FromArgb(44, 62, 80);
            lblCustomerCaption.Location = new Point(0, 8);
            lblCustomerCaption.Margin = new Padding(0, 4, 8, 4);
            lblCustomerCaption.Name = "lblCustomerCaption";
            lblCustomerCaption.Size = new Size(117, 21);
            lblCustomerCaption.TabIndex = 0;
            lblCustomerCaption.Text = "👤 Khách hàng";
            // 
            // txtCustomerPhone
            // 
            txtCustomerPhone.Dock = DockStyle.Fill;
            txtCustomerPhone.Font = new Font("Segoe UI", 10.5F);
            txtCustomerPhone.Location = new Point(125, 3);
            txtCustomerPhone.Margin = new Padding(0, 3, 4, 3);
            txtCustomerPhone.MaxLength = 20;
            txtCustomerPhone.Name = "txtCustomerPhone";
            txtCustomerPhone.PlaceholderText = "Số điện thoại (F3)";
            txtCustomerPhone.Size = new Size(146, 31);
            txtCustomerPhone.TabIndex = 1;
            txtCustomerPhone.KeyDown += txtCustomerPhone_KeyDown;
            // 
            // btnFindCustomer
            // 
            btnFindCustomer.Anchor = AnchorStyles.Left;
            btnFindCustomer.BackColor = Color.FromArgb(242, 153, 0);
            btnFindCustomer.Cursor = Cursors.Hand;
            btnFindCustomer.FlatAppearance.BorderSize = 0;
            btnFindCustomer.FlatAppearance.MouseOverBackColor = Color.FromArgb(222, 140, 0);
            btnFindCustomer.FlatStyle = FlatStyle.Flat;
            btnFindCustomer.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnFindCustomer.ForeColor = Color.White;
            btnFindCustomer.Location = new Point(278, 3);
            btnFindCustomer.Name = "btnFindCustomer";
            btnFindCustomer.Size = new Size(52, 30);
            btnFindCustomer.TabIndex = 2;
            btnFindCustomer.TabStop = false;
            btnFindCustomer.Text = "Tìm";
            btnFindCustomer.UseVisualStyleBackColor = false;
            btnFindCustomer.Click += btnFindCustomer_Click;
            // 
            // btnClearCustomer
            // 
            btnClearCustomer.Anchor = AnchorStyles.Left;
            btnClearCustomer.BackColor = Color.FromArgb(236, 240, 241);
            btnClearCustomer.Cursor = Cursors.Hand;
            btnClearCustomer.FlatAppearance.BorderSize = 0;
            btnClearCustomer.FlatAppearance.MouseOverBackColor = Color.FromArgb(217, 220, 221);
            btnClearCustomer.FlatStyle = FlatStyle.Flat;
            btnClearCustomer.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnClearCustomer.ForeColor = Color.FromArgb(44, 62, 80);
            btnClearCustomer.Location = new Point(336, 3);
            btnClearCustomer.Name = "btnClearCustomer";
            btnClearCustomer.Size = new Size(32, 30);
            btnClearCustomer.TabIndex = 3;
            btnClearCustomer.TabStop = false;
            btnClearCustomer.Text = "✕";
            btnClearCustomer.UseVisualStyleBackColor = false;
            btnClearCustomer.Click += btnClearCustomer_Click;
            // 
            // lblCustomerInfo
            // 
            lblCustomerInfo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblCustomerInfo.AutoEllipsis = true;
            lblCustomerInfo.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblCustomerInfo.Location = new Point(377, 8);
            lblCustomerInfo.Margin = new Padding(6, 0, 0, 0);
            lblCustomerInfo.Name = "lblCustomerInfo";
            lblCustomerInfo.Size = new Size(115, 21);
            lblCustomerInfo.TabIndex = 4;
            lblCustomerInfo.TextAlign = ContentAlignment.MiddleLeft;
            lblCustomerInfo.UseMnemonic = false;
            // 
            // flpOrderType
            // 
            flpOrderType.AutoSize = true;
            flpOrderType.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpOrderType.Controls.Add(btnTypeMangDi);
            flpOrderType.Controls.Add(btnTypeTaiQuay);
            flpOrderType.Controls.Add(btnTypeGiaoHang);
            flpOrderType.Controls.Add(lblSource);
            flpOrderType.Controls.Add(cboSource);
            flpOrderType.Dock = DockStyle.Fill;
            flpOrderType.Location = new Point(0, 41);
            flpOrderType.Margin = new Padding(0, 4, 0, 0);
            flpOrderType.Name = "flpOrderType";
            flpOrderType.Size = new Size(492, 38);
            flpOrderType.TabIndex = 1;
            flpOrderType.WrapContents = false;
            // 
            // btnTypeMangDi
            // 
            btnTypeMangDi.BackColor = Color.White;
            btnTypeMangDi.Cursor = Cursors.Hand;
            btnTypeMangDi.FlatAppearance.BorderColor = Color.FromArgb(214, 219, 223);
            btnTypeMangDi.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 222);
            btnTypeMangDi.FlatStyle = FlatStyle.Flat;
            btnTypeMangDi.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnTypeMangDi.ForeColor = Color.FromArgb(44, 62, 80);
            btnTypeMangDi.Location = new Point(3, 3);
            btnTypeMangDi.Name = "btnTypeMangDi";
            btnTypeMangDi.Size = new Size(100, 32);
            btnTypeMangDi.TabIndex = 0;
            btnTypeMangDi.TabStop = false;
            btnTypeMangDi.Tag = "MangDi";
            btnTypeMangDi.Text = "🛍 Mang về";
            btnTypeMangDi.UseVisualStyleBackColor = false;
            btnTypeMangDi.Click += btnOrderType_Click;
            // 
            // btnTypeTaiQuay
            // 
            btnTypeTaiQuay.BackColor = Color.White;
            btnTypeTaiQuay.Cursor = Cursors.Hand;
            btnTypeTaiQuay.FlatAppearance.BorderColor = Color.FromArgb(214, 219, 223);
            btnTypeTaiQuay.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 222);
            btnTypeTaiQuay.FlatStyle = FlatStyle.Flat;
            btnTypeTaiQuay.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnTypeTaiQuay.ForeColor = Color.FromArgb(44, 62, 80);
            btnTypeTaiQuay.Location = new Point(109, 3);
            btnTypeTaiQuay.Name = "btnTypeTaiQuay";
            btnTypeTaiQuay.Size = new Size(100, 32);
            btnTypeTaiQuay.TabIndex = 1;
            btnTypeTaiQuay.TabStop = false;
            btnTypeTaiQuay.Tag = "TaiQuay";
            btnTypeTaiQuay.Text = "\U0001fa91 Tại bàn";
            btnTypeTaiQuay.UseVisualStyleBackColor = false;
            btnTypeTaiQuay.Click += btnOrderType_Click;
            // 
            // btnTypeGiaoHang
            // 
            btnTypeGiaoHang.BackColor = Color.White;
            btnTypeGiaoHang.Cursor = Cursors.Hand;
            btnTypeGiaoHang.FlatAppearance.BorderColor = Color.FromArgb(214, 219, 223);
            btnTypeGiaoHang.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 222);
            btnTypeGiaoHang.FlatStyle = FlatStyle.Flat;
            btnTypeGiaoHang.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnTypeGiaoHang.ForeColor = Color.FromArgb(44, 62, 80);
            btnTypeGiaoHang.Location = new Point(215, 3);
            btnTypeGiaoHang.Name = "btnTypeGiaoHang";
            btnTypeGiaoHang.Size = new Size(88, 32);
            btnTypeGiaoHang.TabIndex = 2;
            btnTypeGiaoHang.TabStop = false;
            btnTypeGiaoHang.Tag = "GiaoHang";
            btnTypeGiaoHang.Text = "\U0001f6f5 Giao";
            btnTypeGiaoHang.UseVisualStyleBackColor = false;
            btnTypeGiaoHang.Click += btnOrderType_Click;
            // 
            // lblSource
            // 
            lblSource.Anchor = AnchorStyles.None;
            lblSource.AutoSize = true;
            lblSource.ForeColor = Color.FromArgb(44, 62, 80);
            lblSource.Location = new Point(318, 12);
            lblSource.Margin = new Padding(12, 8, 4, 0);
            lblSource.Name = "lblSource";
            lblSource.Size = new Size(61, 21);
            lblSource.TabIndex = 3;
            lblSource.Text = "Nguồn:";
            // 
            // cboSource
            // 
            cboSource.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSource.Font = new Font("Segoe UI", 9.5F);
            cboSource.Location = new Point(383, 4);
            cboSource.Margin = new Padding(0, 4, 0, 0);
            cboSource.Name = "cboSource";
            cboSource.Size = new Size(110, 29);
            cboSource.TabIndex = 4;
            cboSource.SelectedIndexChanged += cboSource_SelectedIndexChanged;
            // 
            // tlpDelivery
            // 
            tlpDelivery.AutoSize = true;
            tlpDelivery.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpDelivery.ColumnCount = 4;
            tlpDelivery.ColumnStyles.Add(new ColumnStyle());
            tlpDelivery.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDelivery.ColumnStyles.Add(new ColumnStyle());
            tlpDelivery.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tlpDelivery.Controls.Add(lblAddressCaption, 0, 0);
            tlpDelivery.Controls.Add(txtAddress, 1, 0);
            tlpDelivery.Controls.Add(lblShipFeeCaption, 2, 0);
            tlpDelivery.Controls.Add(numShipFee, 3, 0);
            tlpDelivery.Dock = DockStyle.Fill;
            tlpDelivery.Location = new Point(0, 83);
            tlpDelivery.Margin = new Padding(0, 4, 0, 0);
            tlpDelivery.Name = "tlpDelivery";
            tlpDelivery.RowCount = 1;
            tlpDelivery.RowStyles.Add(new RowStyle());
            tlpDelivery.Size = new Size(492, 35);
            tlpDelivery.TabIndex = 2;
            tlpDelivery.Visible = false;
            // 
            // lblAddressCaption
            // 
            lblAddressCaption.Anchor = AnchorStyles.Left;
            lblAddressCaption.AutoSize = true;
            lblAddressCaption.ForeColor = Color.FromArgb(44, 62, 80);
            lblAddressCaption.Location = new Point(0, 7);
            lblAddressCaption.Margin = new Padding(0, 4, 8, 4);
            lblAddressCaption.Name = "lblAddressCaption";
            lblAddressCaption.Size = new Size(117, 21);
            lblAddressCaption.TabIndex = 0;
            lblAddressCaption.Text = "📍 Địa chỉ giao";
            // 
            // txtAddress
            // 
            txtAddress.Dock = DockStyle.Fill;
            txtAddress.Location = new Point(125, 3);
            txtAddress.Margin = new Padding(0, 3, 8, 3);
            txtAddress.MaxLength = 255;
            txtAddress.Name = "txtAddress";
            txtAddress.PlaceholderText = "Số nhà, đường, phường / quận";
            txtAddress.Size = new Size(186, 29);
            txtAddress.TabIndex = 1;
            // 
            // lblShipFeeCaption
            // 
            lblShipFeeCaption.Anchor = AnchorStyles.Left;
            lblShipFeeCaption.AutoSize = true;
            lblShipFeeCaption.ForeColor = Color.FromArgb(44, 62, 80);
            lblShipFeeCaption.Location = new Point(319, 7);
            lblShipFeeCaption.Margin = new Padding(0, 4, 8, 4);
            lblShipFeeCaption.Name = "lblShipFeeCaption";
            lblShipFeeCaption.Size = new Size(65, 21);
            lblShipFeeCaption.TabIndex = 2;
            lblShipFeeCaption.Text = "Phí ship";
            // 
            // numShipFee
            // 
            numShipFee.Dock = DockStyle.Fill;
            numShipFee.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numShipFee.Location = new Point(392, 3);
            numShipFee.Margin = new Padding(0, 3, 0, 3);
            numShipFee.Maximum = new decimal(new int[] { 500000, 0, 0, 0 });
            numShipFee.Name = "numShipFee";
            numShipFee.Size = new Size(100, 29);
            numShipFee.TabIndex = 3;
            numShipFee.TextAlign = HorizontalAlignment.Right;
            numShipFee.ThousandsSeparator = true;
            numShipFee.ValueChanged += numShipFee_ValueChanged;
            // 
            // dgvCart
            // 
            dgvCart.AllowUserToAddRows = false;
            dgvCart.AllowUserToDeleteRows = false;
            dgvCart.AllowUserToResizeColumns = false;
            dgvCart.AllowUserToResizeRows = false;
            dgvCart.BackgroundColor = Color.White;
            dgvCart.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle1.SelectionForeColor = Color.FromArgb(44, 62, 80);
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvCart.ColumnHeadersHeight = 30;
            dgvCart.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCart.Columns.AddRange(new DataGridViewColumn[] { colName, colPrice, colMinus, colQty, colPlus, colDiscount, colTotal, colMore });
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Window;
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle9.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle9.SelectionBackColor = Color.FromArgb(255, 226, 168);
            dataGridViewCellStyle9.SelectionForeColor = Color.Black;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.False;
            dgvCart.DefaultCellStyle = dataGridViewCellStyle9;
            dgvCart.Dock = DockStyle.Fill;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.Font = new Font("Segoe UI", 10F);
            dgvCart.GridColor = Color.FromArgb(236, 238, 240);
            dgvCart.Location = new Point(0, 124);
            dgvCart.Margin = new Padding(0, 6, 0, 4);
            dgvCart.MultiSelect = false;
            dgvCart.Name = "dgvCart";
            dgvCart.RowHeadersVisible = false;
            dgvCart.RowHeadersWidth = 51;
            dgvCart.RowTemplate.Height = 34;
            dgvCart.ScrollBars = ScrollBars.Vertical;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.Size = new Size(492, 315);
            dgvCart.TabIndex = 3;
            dgvCart.CellClick += dgvCart_CellClick;
            dgvCart.CellEndEdit += dgvCart_CellEndEdit;
            dgvCart.CellPainting += dgvCart_CellPainting;
            dgvCart.CellValidating += dgvCart_CellValidating;
            dgvCart.DataError += dgvCart_DataError;
            dgvCart.Paint += dgvCart_Paint;
            dgvCart.KeyDown += dgvCart_KeyDown;
            // 
            // colName
            // 
            colName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colName.DataPropertyName = "TenSanPham";
            colName.HeaderText = "Tên món";
            colName.MinimumWidth = 6;
            colName.Name = "colName";
            colName.ReadOnly = true;
            colName.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // colPrice
            // 
            colPrice.DataPropertyName = "DonGia";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            colPrice.DefaultCellStyle = dataGridViewCellStyle2;
            colPrice.HeaderText = "Đơn giá";
            colPrice.MinimumWidth = 6;
            colPrice.Name = "colPrice";
            colPrice.ReadOnly = true;
            colPrice.SortMode = DataGridViewColumnSortMode.NotSortable;
            colPrice.Width = 72;
            // 
            // colMinus
            // 
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(204, 122, 0);
            dataGridViewCellStyle3.Padding = new Padding(2, 5, 2, 5);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(204, 122, 0);
            colMinus.DefaultCellStyle = dataGridViewCellStyle3;
            colMinus.FlatStyle = FlatStyle.Flat;
            colMinus.HeaderText = "";
            colMinus.MinimumWidth = 6;
            colMinus.Name = "colMinus";
            colMinus.Text = "−";
            colMinus.UseColumnTextForButtonValue = true;
            colMinus.Width = 34;
            // 
            // colQty
            // 
            colQty.DataPropertyName = "SoLuong";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            colQty.DefaultCellStyle = dataGridViewCellStyle4;
            colQty.HeaderText = "SL";
            colQty.MinimumWidth = 6;
            colQty.Name = "colQty";
            colQty.SortMode = DataGridViewColumnSortMode.NotSortable;
            colQty.Width = 46;
            // 
            // colPlus
            // 
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(204, 122, 0);
            dataGridViewCellStyle5.Padding = new Padding(2, 5, 2, 5);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(204, 122, 0);
            colPlus.DefaultCellStyle = dataGridViewCellStyle5;
            colPlus.FlatStyle = FlatStyle.Flat;
            colPlus.HeaderText = "";
            colPlus.MinimumWidth = 6;
            colPlus.Name = "colPlus";
            colPlus.Text = "+";
            colPlus.UseColumnTextForButtonValue = true;
            colPlus.Width = 34;
            // 
            // colDiscount
            // 
            colDiscount.DataPropertyName = "GiamDong";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(192, 57, 43);
            dataGridViewCellStyle6.Format = "N0";
            colDiscount.DefaultCellStyle = dataGridViewCellStyle6;
            colDiscount.HeaderText = "Giảm giá";
            colDiscount.MinimumWidth = 6;
            colDiscount.Name = "colDiscount";
            colDiscount.ReadOnly = true;
            colDiscount.SortMode = DataGridViewColumnSortMode.NotSortable;
            colDiscount.Width = 72;
            // 
            // colTotal
            // 
            colTotal.DataPropertyName = "ThanhTien";
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle7.Format = "N0";
            colTotal.DefaultCellStyle = dataGridViewCellStyle7;
            colTotal.HeaderText = "Thành tiền";
            colTotal.MinimumWidth = 6;
            colTotal.Name = "colTotal";
            colTotal.ReadOnly = true;
            colTotal.SortMode = DataGridViewColumnSortMode.NotSortable;
            colTotal.Width = 86;
            // 
            // colMore
            // 
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dataGridViewCellStyle8.ForeColor = Color.FromArgb(204, 122, 0);
            dataGridViewCellStyle8.Padding = new Padding(2, 5, 2, 5);
            dataGridViewCellStyle8.SelectionBackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle8.SelectionForeColor = Color.FromArgb(204, 122, 0);
            colMore.DefaultCellStyle = dataGridViewCellStyle8;
            colMore.FlatStyle = FlatStyle.Flat;
            colMore.HeaderText = "";
            colMore.MinimumWidth = 6;
            colMore.Name = "colMore";
            colMore.Text = "⋯";
            colMore.UseColumnTextForButtonValue = true;
            colMore.Width = 34;
            // 
            // tlpCheckout
            // 
            tlpCheckout.ColumnCount = 2;
            tlpCheckout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
            tlpCheckout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            tlpCheckout.Controls.Add(tlpPaymentOptions, 0, 0);
            tlpCheckout.Controls.Add(tlpTotals, 1, 0);
            tlpCheckout.Dock = DockStyle.Fill;
            tlpCheckout.Location = new Point(0, 487);
            tlpCheckout.Margin = new Padding(0);
            tlpCheckout.Name = "tlpCheckout";
            tlpCheckout.RowCount = 1;
            tlpCheckout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCheckout.Size = new Size(492, 217);
            tlpCheckout.TabIndex = 4;
            // 
            // tlpPaymentOptions
            // 
            tlpPaymentOptions.ColumnCount = 1;
            tlpPaymentOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPaymentOptions.Controls.Add(tlpPromo, 0, 0);
            tlpPaymentOptions.Controls.Add(tlpPay, 0, 1);
            tlpPaymentOptions.Controls.Add(tlpMoney, 0, 2);
            tlpPaymentOptions.Controls.Add(tlpQuick, 0, 3);
            tlpPaymentOptions.Dock = DockStyle.Fill;
            tlpPaymentOptions.Location = new Point(0, 0);
            tlpPaymentOptions.Margin = new Padding(0, 0, 8, 0);
            tlpPaymentOptions.Name = "tlpPaymentOptions";
            tlpPaymentOptions.RowCount = 5;
            tlpPaymentOptions.RowStyles.Add(new RowStyle());
            tlpPaymentOptions.RowStyles.Add(new RowStyle());
            tlpPaymentOptions.RowStyles.Add(new RowStyle());
            tlpPaymentOptions.RowStyles.Add(new RowStyle());
            tlpPaymentOptions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPaymentOptions.Size = new Size(267, 217);
            tlpPaymentOptions.TabIndex = 0;
            // 
            // tlpPromo
            // 
            tlpPromo.AutoSize = true;
            tlpPromo.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPromo.ColumnCount = 4;
            tlpPromo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPromo.ColumnStyles.Add(new ColumnStyle());
            tlpPromo.ColumnStyles.Add(new ColumnStyle());
            tlpPromo.ColumnStyles.Add(new ColumnStyle());
            tlpPromo.Controls.Add(btnPromo, 0, 0);
            tlpPromo.Controls.Add(lblPoints, 1, 0);
            tlpPromo.Controls.Add(numRedeemPoints, 2, 0);
            tlpPromo.Controls.Add(lblRedeemHint, 3, 0);
            tlpPromo.Dock = DockStyle.Fill;
            tlpPromo.Location = new Point(0, 0);
            tlpPromo.Margin = new Padding(0);
            tlpPromo.Name = "tlpPromo";
            tlpPromo.RowCount = 1;
            tlpPromo.RowStyles.Add(new RowStyle());
            tlpPromo.Size = new Size(267, 34);
            tlpPromo.TabIndex = 0;
            // 
            // btnPromo
            // 
            btnPromo.AutoEllipsis = true;
            btnPromo.BackColor = Color.White;
            btnPromo.Cursor = Cursors.Hand;
            btnPromo.Dock = DockStyle.Fill;
            btnPromo.FlatAppearance.BorderColor = Color.FromArgb(214, 219, 223);
            btnPromo.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 222);
            btnPromo.FlatStyle = FlatStyle.Flat;
            btnPromo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPromo.ForeColor = Color.FromArgb(204, 122, 0);
            btnPromo.Location = new Point(0, 2);
            btnPromo.Margin = new Padding(0, 2, 8, 2);
            btnPromo.Name = "btnPromo";
            btnPromo.Size = new Size(126, 30);
            btnPromo.TabIndex = 0;
            btnPromo.TabStop = false;
            btnPromo.Text = "🎁 Khuyến mãi (F4)";
            btnPromo.TextAlign = ContentAlignment.MiddleLeft;
            btnPromo.UseMnemonic = false;
            btnPromo.UseVisualStyleBackColor = false;
            btnPromo.Click += btnPromo_Click;
            // 
            // lblPoints
            // 
            lblPoints.Anchor = AnchorStyles.Left;
            lblPoints.AutoSize = true;
            lblPoints.ForeColor = Color.FromArgb(44, 62, 80);
            lblPoints.Location = new Point(134, 6);
            lblPoints.Margin = new Padding(0, 0, 4, 0);
            lblPoints.Name = "lblPoints";
            lblPoints.Size = new Size(65, 21);
            lblPoints.TabIndex = 1;
            lblPoints.Text = "⭐ Điểm";
            // 
            // numRedeemPoints
            // 
            numRedeemPoints.Anchor = AnchorStyles.Left;
            numRedeemPoints.Enabled = false;
            numRedeemPoints.Location = new Point(203, 2);
            numRedeemPoints.Margin = new Padding(0, 2, 2, 2);
            numRedeemPoints.Maximum = new decimal(new int[] { 0, 0, 0, 0 });
            numRedeemPoints.Name = "numRedeemPoints";
            numRedeemPoints.Size = new Size(62, 29);
            numRedeemPoints.TabIndex = 2;
            numRedeemPoints.TextAlign = HorizontalAlignment.Right;
            numRedeemPoints.ThousandsSeparator = true;
            numRedeemPoints.ValueChanged += numRedeemPoints_ValueChanged;
            // 
            // lblRedeemHint
            // 
            lblRedeemHint.Anchor = AnchorStyles.Left;
            lblRedeemHint.AutoSize = true;
            lblRedeemHint.Font = new Font("Segoe UI", 8.5F);
            lblRedeemHint.ForeColor = Color.FromArgb(127, 140, 141);
            lblRedeemHint.Location = new Point(267, 7);
            lblRedeemHint.Margin = new Padding(0);
            lblRedeemHint.Name = "lblRedeemHint";
            lblRedeemHint.Size = new Size(0, 20);
            lblRedeemHint.TabIndex = 3;
            // 
            // tlpPay
            // 
            tlpPay.AutoSize = true;
            tlpPay.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpPay.ColumnCount = 4;
            tlpPay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24F));
            tlpPay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            tlpPay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16F));
            tlpPay.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27F));
            tlpPay.Controls.Add(btnPayTienMat, 0, 0);
            tlpPay.Controls.Add(btnPayChuyenKhoan, 1, 0);
            tlpPay.Controls.Add(btnPayThe, 2, 0);
            tlpPay.Controls.Add(btnPayVi, 3, 0);
            tlpPay.Dock = DockStyle.Fill;
            tlpPay.Location = new Point(0, 34);
            tlpPay.Margin = new Padding(0);
            tlpPay.Name = "tlpPay";
            tlpPay.RowCount = 1;
            tlpPay.RowStyles.Add(new RowStyle());
            tlpPay.Size = new Size(267, 34);
            tlpPay.TabIndex = 1;
            // 
            // btnPayTienMat
            // 
            btnPayTienMat.BackColor = Color.White;
            btnPayTienMat.Cursor = Cursors.Hand;
            btnPayTienMat.Dock = DockStyle.Fill;
            btnPayTienMat.FlatAppearance.BorderColor = Color.FromArgb(214, 219, 223);
            btnPayTienMat.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 222);
            btnPayTienMat.FlatStyle = FlatStyle.Flat;
            btnPayTienMat.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPayTienMat.ForeColor = Color.FromArgb(44, 62, 80);
            btnPayTienMat.Location = new Point(0, 2);
            btnPayTienMat.Margin = new Padding(0, 2, 3, 2);
            btnPayTienMat.Name = "btnPayTienMat";
            btnPayTienMat.Size = new Size(61, 30);
            btnPayTienMat.TabIndex = 0;
            btnPayTienMat.TabStop = false;
            btnPayTienMat.Tag = "TienMat";
            btnPayTienMat.Text = "Tiền mặt";
            btnPayTienMat.UseVisualStyleBackColor = false;
            btnPayTienMat.Click += btnPayment_Click;
            // 
            // btnPayChuyenKhoan
            // 
            btnPayChuyenKhoan.BackColor = Color.White;
            btnPayChuyenKhoan.Cursor = Cursors.Hand;
            btnPayChuyenKhoan.Dock = DockStyle.Fill;
            btnPayChuyenKhoan.FlatAppearance.BorderColor = Color.FromArgb(214, 219, 223);
            btnPayChuyenKhoan.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 222);
            btnPayChuyenKhoan.FlatStyle = FlatStyle.Flat;
            btnPayChuyenKhoan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPayChuyenKhoan.ForeColor = Color.FromArgb(44, 62, 80);
            btnPayChuyenKhoan.Location = new Point(64, 2);
            btnPayChuyenKhoan.Margin = new Padding(0, 2, 3, 2);
            btnPayChuyenKhoan.Name = "btnPayChuyenKhoan";
            btnPayChuyenKhoan.Size = new Size(85, 30);
            btnPayChuyenKhoan.TabIndex = 1;
            btnPayChuyenKhoan.TabStop = false;
            btnPayChuyenKhoan.Tag = "ChuyenKhoan";
            btnPayChuyenKhoan.Text = "Chuyển khoản";
            btnPayChuyenKhoan.UseVisualStyleBackColor = false;
            btnPayChuyenKhoan.Click += btnPayment_Click;
            // 
            // btnPayThe
            // 
            btnPayThe.BackColor = Color.White;
            btnPayThe.Cursor = Cursors.Hand;
            btnPayThe.Dock = DockStyle.Fill;
            btnPayThe.FlatAppearance.BorderColor = Color.FromArgb(214, 219, 223);
            btnPayThe.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 222);
            btnPayThe.FlatStyle = FlatStyle.Flat;
            btnPayThe.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPayThe.ForeColor = Color.FromArgb(44, 62, 80);
            btnPayThe.Location = new Point(152, 2);
            btnPayThe.Margin = new Padding(0, 2, 3, 2);
            btnPayThe.Name = "btnPayThe";
            btnPayThe.Size = new Size(39, 30);
            btnPayThe.TabIndex = 2;
            btnPayThe.TabStop = false;
            btnPayThe.Tag = "The";
            btnPayThe.Text = "Thẻ";
            btnPayThe.UseVisualStyleBackColor = false;
            btnPayThe.Click += btnPayment_Click;
            // 
            // btnPayVi
            // 
            btnPayVi.BackColor = Color.White;
            btnPayVi.Cursor = Cursors.Hand;
            btnPayVi.Dock = DockStyle.Fill;
            btnPayVi.FlatAppearance.BorderColor = Color.FromArgb(214, 219, 223);
            btnPayVi.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 244, 222);
            btnPayVi.FlatStyle = FlatStyle.Flat;
            btnPayVi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnPayVi.ForeColor = Color.FromArgb(44, 62, 80);
            btnPayVi.Location = new Point(194, 2);
            btnPayVi.Margin = new Padding(0, 2, 3, 2);
            btnPayVi.Name = "btnPayVi";
            btnPayVi.Size = new Size(70, 30);
            btnPayVi.TabIndex = 3;
            btnPayVi.TabStop = false;
            btnPayVi.Tag = "Vi";
            btnPayVi.Text = "Ví điện tử";
            btnPayVi.UseVisualStyleBackColor = false;
            btnPayVi.Click += btnPayment_Click;
            // 
            // tlpMoney
            // 
            tlpMoney.AutoSize = true;
            tlpMoney.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpMoney.ColumnCount = 3;
            tlpMoney.ColumnStyles.Add(new ColumnStyle());
            tlpMoney.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMoney.ColumnStyles.Add(new ColumnStyle());
            tlpMoney.Controls.Add(lblMoneyCaption, 0, 0);
            tlpMoney.Controls.Add(pnlMoney, 1, 0);
            tlpMoney.Controls.Add(lblChange, 2, 0);
            tlpMoney.Dock = DockStyle.Fill;
            tlpMoney.Location = new Point(0, 68);
            tlpMoney.Margin = new Padding(0);
            tlpMoney.Name = "tlpMoney";
            tlpMoney.RowCount = 1;
            tlpMoney.RowStyles.Add(new RowStyle());
            tlpMoney.Size = new Size(267, 33);
            tlpMoney.TabIndex = 2;
            // 
            // lblMoneyCaption
            // 
            lblMoneyCaption.Anchor = AnchorStyles.Left;
            lblMoneyCaption.AutoSize = true;
            lblMoneyCaption.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblMoneyCaption.ForeColor = Color.FromArgb(44, 62, 80);
            lblMoneyCaption.Location = new Point(0, 6);
            lblMoneyCaption.Margin = new Padding(0, 4, 8, 4);
            lblMoneyCaption.Name = "lblMoneyCaption";
            lblMoneyCaption.Size = new Size(91, 21);
            lblMoneyCaption.TabIndex = 0;
            lblMoneyCaption.Text = "Khách đưa";
            // 
            // pnlMoney
            // 
            pnlMoney.Controls.Add(txtCustomerMoney);
            pnlMoney.Controls.Add(txtPaymentRef);
            pnlMoney.Dock = DockStyle.Fill;
            pnlMoney.Location = new Point(99, 3);
            pnlMoney.Margin = new Padding(0, 3, 0, 3);
            pnlMoney.Name = "pnlMoney";
            pnlMoney.Size = new Size(70, 27);
            pnlMoney.TabIndex = 1;
            // 
            // txtCustomerMoney
            // 
            txtCustomerMoney.Dock = DockStyle.Fill;
            txtCustomerMoney.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            txtCustomerMoney.Location = new Point(0, 0);
            txtCustomerMoney.Margin = new Padding(0);
            txtCustomerMoney.MaxLength = 15;
            txtCustomerMoney.Name = "txtCustomerMoney";
            txtCustomerMoney.PlaceholderText = "Trống = đưa đủ";
            txtCustomerMoney.Size = new Size(70, 32);
            txtCustomerMoney.TabIndex = 0;
            txtCustomerMoney.TextAlign = HorizontalAlignment.Right;
            txtCustomerMoney.TextChanged += txtCustomerMoney_TextChanged;
            txtCustomerMoney.KeyDown += txtCustomerMoney_KeyDown;
            txtCustomerMoney.Leave += txtCustomerMoney_Leave;
            // 
            // txtPaymentRef
            // 
            txtPaymentRef.Dock = DockStyle.Fill;
            txtPaymentRef.Font = new Font("Segoe UI", 11F);
            txtPaymentRef.Location = new Point(0, 0);
            txtPaymentRef.Margin = new Padding(0);
            txtPaymentRef.MaxLength = 100;
            txtPaymentRef.Name = "txtPaymentRef";
            txtPaymentRef.PlaceholderText = "Mã giao dịch / 4 số cuối thẻ";
            txtPaymentRef.Size = new Size(70, 32);
            txtPaymentRef.TabIndex = 1;
            txtPaymentRef.Visible = false;
            // 
            // lblChange
            // 
            lblChange.Anchor = AnchorStyles.Right;
            lblChange.AutoSize = true;
            lblChange.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblChange.ForeColor = Color.FromArgb(39, 174, 96);
            lblChange.Location = new Point(177, 4);
            lblChange.Margin = new Padding(8, 0, 0, 0);
            lblChange.Name = "lblChange";
            lblChange.Size = new Size(90, 25);
            lblChange.TabIndex = 2;
            lblChange.Text = "Thừa 0 đ";
            // 
            // tlpQuick
            // 
            tlpQuick.AutoSize = true;
            tlpQuick.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpQuick.ColumnCount = 2;
            tlpQuick.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpQuick.ColumnStyles.Add(new ColumnStyle());
            tlpQuick.Controls.Add(flpQuickCash, 0, 0);
            tlpQuick.Controls.Add(chkPrintReceipt, 1, 0);
            tlpQuick.Dock = DockStyle.Fill;
            tlpQuick.Location = new Point(0, 101);
            tlpQuick.Margin = new Padding(0);
            tlpQuick.Name = "tlpQuick";
            tlpQuick.RowCount = 1;
            tlpQuick.RowStyles.Add(new RowStyle());
            tlpQuick.Size = new Size(267, 25);
            tlpQuick.TabIndex = 3;
            // 
            // flpQuickCash
            // 
            flpQuickCash.AutoSize = true;
            flpQuickCash.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpQuickCash.Dock = DockStyle.Fill;
            flpQuickCash.Location = new Point(0, 0);
            flpQuickCash.Margin = new Padding(0);
            flpQuickCash.Name = "flpQuickCash";
            flpQuickCash.Size = new Size(165, 25);
            flpQuickCash.TabIndex = 0;
            flpQuickCash.WrapContents = false;
            // 
            // chkPrintReceipt
            // 
            chkPrintReceipt.Anchor = AnchorStyles.Right;
            chkPrintReceipt.AutoSize = true;
            chkPrintReceipt.Checked = true;
            chkPrintReceipt.CheckState = CheckState.Checked;
            chkPrintReceipt.ForeColor = Color.FromArgb(44, 62, 80);
            chkPrintReceipt.Location = new Point(171, 0);
            chkPrintReceipt.Margin = new Padding(6, 0, 0, 0);
            chkPrintReceipt.Name = "chkPrintReceipt";
            chkPrintReceipt.Size = new Size(96, 25);
            chkPrintReceipt.TabIndex = 1;
            chkPrintReceipt.Text = "🖨 In bill";
            chkPrintReceipt.UseVisualStyleBackColor = true;
            // 
            // tlpTotals
            // 
            tlpTotals.BackColor = Color.FromArgb(255, 250, 240);
            tlpTotals.ColumnCount = 1;
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTotals.Controls.Add(tlpTotalLines, 0, 0);
            tlpTotals.Controls.Add(lblFinalCaption, 0, 1);
            tlpTotals.Controls.Add(lblFinal, 0, 2);
            tlpTotals.Dock = DockStyle.Fill;
            tlpTotals.Location = new Point(275, 0);
            tlpTotals.Margin = new Padding(0);
            tlpTotals.Name = "tlpTotals";
            tlpTotals.Padding = new Padding(10, 4, 10, 4);
            tlpTotals.RowCount = 4;
            tlpTotals.RowStyles.Add(new RowStyle());
            tlpTotals.RowStyles.Add(new RowStyle());
            tlpTotals.RowStyles.Add(new RowStyle());
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTotals.Size = new Size(217, 217);
            tlpTotals.TabIndex = 1;
            // 
            // tlpTotalLines
            // 
            tlpTotalLines.AutoSize = true;
            tlpTotalLines.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpTotalLines.ColumnCount = 2;
            tlpTotalLines.ColumnStyles.Add(new ColumnStyle());
            tlpTotalLines.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTotalLines.Controls.Add(lblSubtotalCaption, 0, 0);
            tlpTotalLines.Controls.Add(lblSubtotal, 1, 0);
            tlpTotalLines.Controls.Add(lblDiscountCaption, 0, 1);
            tlpTotalLines.Controls.Add(lblDiscount, 1, 1);
            tlpTotalLines.Controls.Add(lblShipCaption, 0, 2);
            tlpTotalLines.Controls.Add(lblShip, 1, 2);
            tlpTotalLines.Dock = DockStyle.Fill;
            tlpTotalLines.Location = new Point(10, 4);
            tlpTotalLines.Margin = new Padding(0);
            tlpTotalLines.Name = "tlpTotalLines";
            tlpTotalLines.RowCount = 3;
            tlpTotalLines.RowStyles.Add(new RowStyle());
            tlpTotalLines.RowStyles.Add(new RowStyle());
            tlpTotalLines.RowStyles.Add(new RowStyle());
            tlpTotalLines.Size = new Size(197, 87);
            tlpTotalLines.TabIndex = 0;
            // 
            // lblSubtotalCaption
            // 
            lblSubtotalCaption.Anchor = AnchorStyles.Left;
            lblSubtotalCaption.AutoSize = true;
            lblSubtotalCaption.ForeColor = Color.FromArgb(44, 62, 80);
            lblSubtotalCaption.Location = new Point(0, 4);
            lblSubtotalCaption.Margin = new Padding(0, 4, 8, 4);
            lblSubtotalCaption.Name = "lblSubtotalCaption";
            lblSubtotalCaption.Size = new Size(114, 21);
            lblSubtotalCaption.TabIndex = 0;
            lblSubtotalCaption.Text = "Tổng tiền hàng";
            // 
            // lblSubtotal
            // 
            lblSubtotal.Anchor = AnchorStyles.Right;
            lblSubtotal.AutoSize = true;
            lblSubtotal.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblSubtotal.ForeColor = Color.FromArgb(44, 62, 80);
            lblSubtotal.Location = new Point(161, 3);
            lblSubtotal.Margin = new Padding(4, 3, 0, 3);
            lblSubtotal.Name = "lblSubtotal";
            lblSubtotal.Size = new Size(36, 23);
            lblSubtotal.TabIndex = 1;
            lblSubtotal.Text = "0 đ";
            // 
            // lblDiscountCaption
            // 
            lblDiscountCaption.Anchor = AnchorStyles.Left;
            lblDiscountCaption.AutoSize = true;
            lblDiscountCaption.ForeColor = Color.FromArgb(44, 62, 80);
            lblDiscountCaption.Location = new Point(0, 33);
            lblDiscountCaption.Margin = new Padding(0, 4, 8, 4);
            lblDiscountCaption.Name = "lblDiscountCaption";
            lblDiscountCaption.Size = new Size(72, 21);
            lblDiscountCaption.TabIndex = 2;
            lblDiscountCaption.Text = "Giảm giá";
            // 
            // lblDiscount
            // 
            lblDiscount.Anchor = AnchorStyles.Right;
            lblDiscount.AutoSize = true;
            lblDiscount.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDiscount.ForeColor = Color.FromArgb(231, 76, 60);
            lblDiscount.Location = new Point(161, 32);
            lblDiscount.Margin = new Padding(4, 3, 0, 3);
            lblDiscount.Name = "lblDiscount";
            lblDiscount.Size = new Size(36, 23);
            lblDiscount.TabIndex = 3;
            lblDiscount.Text = "0 đ";
            // 
            // lblShipCaption
            // 
            lblShipCaption.Anchor = AnchorStyles.Left;
            lblShipCaption.AutoSize = true;
            lblShipCaption.ForeColor = Color.FromArgb(44, 62, 80);
            lblShipCaption.Location = new Point(0, 62);
            lblShipCaption.Margin = new Padding(0, 4, 8, 4);
            lblShipCaption.Name = "lblShipCaption";
            lblShipCaption.Size = new Size(65, 21);
            lblShipCaption.TabIndex = 4;
            lblShipCaption.Text = "Phí ship";
            // 
            // lblShip
            // 
            lblShip.Anchor = AnchorStyles.Right;
            lblShip.AutoSize = true;
            lblShip.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblShip.ForeColor = Color.FromArgb(44, 62, 80);
            lblShip.Location = new Point(161, 61);
            lblShip.Margin = new Padding(4, 3, 0, 3);
            lblShip.Name = "lblShip";
            lblShip.Size = new Size(36, 23);
            lblShip.TabIndex = 5;
            lblShip.Text = "0 đ";
            // 
            // lblFinalCaption
            // 
            lblFinalCaption.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblFinalCaption.AutoEllipsis = true;
            lblFinalCaption.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            lblFinalCaption.ForeColor = Color.FromArgb(44, 62, 80);
            lblFinalCaption.Location = new Point(10, 95);
            lblFinalCaption.Margin = new Padding(0, 4, 0, 0);
            lblFinalCaption.Name = "lblFinalCaption";
            lblFinalCaption.Size = new Size(197, 21);
            lblFinalCaption.TabIndex = 1;
            lblFinalCaption.Text = "KHÁCH PHẢI TRẢ";
            lblFinalCaption.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblFinal
            // 
            lblFinal.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            lblFinal.AutoEllipsis = true;
            lblFinal.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
            lblFinal.ForeColor = Color.FromArgb(231, 76, 60);
            lblFinal.Location = new Point(10, 116);
            lblFinal.Margin = new Padding(0);
            lblFinal.Name = "lblFinal";
            lblFinal.Size = new Size(197, 35);
            lblFinal.TabIndex = 2;
            lblFinal.Text = "0 đ";
            lblFinal.TextAlign = ContentAlignment.MiddleRight;
            lblFinal.UseMnemonic = false;
            // 
            // tlpActions
            // 
            tlpActions.ColumnCount = 3;
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tlpActions.Controls.Add(btnClearOrder, 0, 0);
            tlpActions.Controls.Add(btnParkOrder, 1, 0);
            tlpActions.Controls.Add(btnCheckout, 2, 0);
            tlpActions.Dock = DockStyle.Fill;
            tlpActions.Location = new Point(0, 658);
            tlpActions.Margin = new Padding(0, 6, 0, 0);
            tlpActions.Name = "tlpActions";
            tlpActions.RowCount = 1;
            tlpActions.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpActions.Size = new Size(492, 46);
            tlpActions.TabIndex = 5;
            // 
            // btnClearOrder
            // 
            btnClearOrder.BackColor = Color.FromArgb(236, 240, 241);
            btnClearOrder.Cursor = Cursors.Hand;
            btnClearOrder.Dock = DockStyle.Fill;
            btnClearOrder.FlatAppearance.BorderSize = 0;
            btnClearOrder.FlatAppearance.MouseOverBackColor = Color.FromArgb(217, 220, 221);
            btnClearOrder.FlatStyle = FlatStyle.Flat;
            btnClearOrder.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnClearOrder.ForeColor = Color.FromArgb(231, 76, 60);
            btnClearOrder.Location = new Point(3, 3);
            btnClearOrder.Name = "btnClearOrder";
            btnClearOrder.Size = new Size(92, 40);
            btnClearOrder.TabIndex = 0;
            btnClearOrder.TabStop = false;
            btnClearOrder.Text = "Xóa ĐH";
            btnClearOrder.UseVisualStyleBackColor = false;
            btnClearOrder.Click += btnClearOrder_Click;
            // 
            // btnParkOrder
            // 
            btnParkOrder.BackColor = Color.FromArgb(52, 152, 219);
            btnParkOrder.Cursor = Cursors.Hand;
            btnParkOrder.Dock = DockStyle.Fill;
            btnParkOrder.FlatAppearance.BorderSize = 0;
            btnParkOrder.FlatAppearance.MouseOverBackColor = Color.FromArgb(47, 139, 201);
            btnParkOrder.FlatStyle = FlatStyle.Flat;
            btnParkOrder.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            btnParkOrder.ForeColor = Color.White;
            btnParkOrder.Location = new Point(101, 3);
            btnParkOrder.Name = "btnParkOrder";
            btnParkOrder.Size = new Size(117, 40);
            btnParkOrder.TabIndex = 1;
            btnParkOrder.TabStop = false;
            btnParkOrder.Text = "Cất";
            btnParkOrder.UseVisualStyleBackColor = false;
            btnParkOrder.Click += btnParkOrder_Click;
            // 
            // btnCheckout
            // 
            btnCheckout.BackColor = Color.FromArgb(255, 196, 0);
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.Dock = DockStyle.Fill;
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 180, 0);
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            btnCheckout.ForeColor = Color.FromArgb(44, 62, 80);
            btnCheckout.Location = new Point(224, 3);
            btnCheckout.Name = "btnCheckout";
            btnCheckout.Size = new Size(265, 40);
            btnCheckout.TabIndex = 2;
            btnCheckout.TabStop = false;
            btnCheckout.Text = "💰 THANH TOÁN  (F9)";
            btnCheckout.UseVisualStyleBackColor = false;
            btnCheckout.Click += btnCheckout_Click;
            // 
            // mnuParked
            // 
            mnuParked.Font = new Font("Segoe UI", 10F);
            mnuParked.ImageScalingSize = new Size(20, 20);
            mnuParked.Name = "mnuParked";
            mnuParked.ShowImageMargin = false;
            mnuParked.Size = new Size(36, 4);
            // 
            // mnuCartRow
            // 
            mnuCartRow.Font = new Font("Segoe UI", 10F);
            mnuCartRow.ImageScalingSize = new Size(20, 20);
            mnuCartRow.Name = "mnuCartRow";
            mnuCartRow.ShowImageMargin = false;
            mnuCartRow.Size = new Size(36, 4);
            // 
            // ttpOrder
            // 
            ttpOrder.AutoPopDelay = 10000;
            ttpOrder.InitialDelay = 300;
            ttpOrder.ReshowDelay = 100;
            // 
            // tmrClock
            // 
            tmrClock.Interval = 1000;
            tmrClock.Tick += tmrClock_Tick;
            // 
            // FormOrder
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(244, 245, 247);
            ClientSize = new Size(1280, 720);
            Controls.Add(tlpBody);
            Font = new Font("Segoe UI", 9.5F);
            KeyPreview = true;
            MinimumSize = new Size(900, 560);
            Name = "FormOrder";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Bánh Mì Ngon — Order bán hàng";
            WindowState = FormWindowState.Maximized;
            FormClosing += FormOrder_FormClosing;
            Load += FormOrder_Load;
            tlpHeader.ResumeLayout(false);
            tlpHeader.PerformLayout();
            tlpBody.ResumeLayout(false);
            pnlCategoryHost.ResumeLayout(false);
            pnlMenuCard.ResumeLayout(false);
            tlpMenu.ResumeLayout(false);
            pnlOrderCard.ResumeLayout(false);
            tlpOrder.ResumeLayout(false);
            tlpOrder.PerformLayout();
            tlpCustomer.ResumeLayout(false);
            tlpCustomer.PerformLayout();
            flpOrderType.ResumeLayout(false);
            flpOrderType.PerformLayout();
            tlpDelivery.ResumeLayout(false);
            tlpDelivery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numShipFee).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCart).EndInit();
            tlpCheckout.ResumeLayout(false);
            tlpPaymentOptions.ResumeLayout(false);
            tlpPaymentOptions.PerformLayout();
            tlpPromo.ResumeLayout(false);
            tlpPromo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numRedeemPoints).EndInit();
            tlpPay.ResumeLayout(false);
            tlpMoney.ResumeLayout(false);
            tlpMoney.PerformLayout();
            pnlMoney.ResumeLayout(false);
            pnlMoney.PerformLayout();
            tlpQuick.ResumeLayout(false);
            tlpQuick.PerformLayout();
            tlpTotals.ResumeLayout(false);
            tlpTotals.PerformLayout();
            tlpTotalLines.ResumeLayout(false);
            tlpTotalLines.PerformLayout();
            tlpActions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpHeader;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnOrders;
        private System.Windows.Forms.Label lblClock;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.ContextMenuStrip mnuMain;
        private System.Windows.Forms.ContextMenuStrip mnuOrders;
        private System.Windows.Forms.ContextMenuStrip mnuSettings;
        private System.Windows.Forms.TableLayoutPanel tlpBody;
        private System.Windows.Forms.Panel pnlMenuCard;
        private System.Windows.Forms.TableLayoutPanel tlpMenu;
        private System.Windows.Forms.Panel pnlCategoryHost;
        private System.Windows.Forms.FlowLayoutPanel flpCategories;
        private System.Windows.Forms.FlowLayoutPanel flpProducts;
        private System.Windows.Forms.Panel pnlOrderCard;
        private System.Windows.Forms.TableLayoutPanel tlpOrder;
        private System.Windows.Forms.TableLayoutPanel tlpCustomer;
        private System.Windows.Forms.Label lblCustomerCaption;
        private System.Windows.Forms.TextBox txtCustomerPhone;
        private System.Windows.Forms.Button btnFindCustomer;
        private System.Windows.Forms.Button btnClearCustomer;
        private System.Windows.Forms.Label lblCustomerInfo;
        private System.Windows.Forms.FlowLayoutPanel flpOrderType;
        private System.Windows.Forms.Button btnTypeTaiQuay;
        private System.Windows.Forms.Button btnTypeMangDi;
        private System.Windows.Forms.Button btnTypeGiaoHang;
        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.ComboBox cboSource;
        private System.Windows.Forms.TableLayoutPanel tlpDelivery;
        private System.Windows.Forms.Label lblAddressCaption;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblShipFeeCaption;
        private System.Windows.Forms.NumericUpDown numShipFee;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrice;
        private System.Windows.Forms.DataGridViewButtonColumn colMinus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQty;
        private System.Windows.Forms.DataGridViewButtonColumn colPlus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDiscount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotal;
        private System.Windows.Forms.DataGridViewButtonColumn colMore;
        private System.Windows.Forms.TableLayoutPanel tlpCheckout;
        private System.Windows.Forms.TableLayoutPanel tlpPaymentOptions;
        private System.Windows.Forms.TableLayoutPanel tlpPromo;
        private System.Windows.Forms.Button btnPromo;
        private System.Windows.Forms.Label lblPoints;
        private System.Windows.Forms.NumericUpDown numRedeemPoints;
        private System.Windows.Forms.Label lblRedeemHint;
        private System.Windows.Forms.TableLayoutPanel tlpPay;
        private System.Windows.Forms.Button btnPayTienMat;
        private System.Windows.Forms.Button btnPayChuyenKhoan;
        private System.Windows.Forms.Button btnPayThe;
        private System.Windows.Forms.Button btnPayVi;
        private System.Windows.Forms.TableLayoutPanel tlpMoney;
        private System.Windows.Forms.Label lblMoneyCaption;
        private System.Windows.Forms.Panel pnlMoney;
        private System.Windows.Forms.TextBox txtCustomerMoney;
        private System.Windows.Forms.TextBox txtPaymentRef;
        private System.Windows.Forms.Label lblChange;
        private System.Windows.Forms.TableLayoutPanel tlpQuick;
        private System.Windows.Forms.FlowLayoutPanel flpQuickCash;
        private System.Windows.Forms.CheckBox chkPrintReceipt;
        private System.Windows.Forms.TableLayoutPanel tlpTotals;
        private System.Windows.Forms.TableLayoutPanel tlpTotalLines;
        private System.Windows.Forms.Label lblSubtotalCaption;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Label lblDiscountCaption;
        private System.Windows.Forms.Label lblDiscount;
        private System.Windows.Forms.Label lblShipCaption;
        private System.Windows.Forms.Label lblShip;
        private System.Windows.Forms.Label lblFinalCaption;
        private System.Windows.Forms.Label lblFinal;
        private System.Windows.Forms.TableLayoutPanel tlpActions;
        private System.Windows.Forms.Button btnClearOrder;
        private System.Windows.Forms.Button btnParkOrder;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.ContextMenuStrip mnuParked;
        private System.Windows.Forms.ContextMenuStrip mnuCartRow;
        private System.Windows.Forms.ToolTip ttpOrder;
        private System.Windows.Forms.Timer tmrClock;
    }
}
