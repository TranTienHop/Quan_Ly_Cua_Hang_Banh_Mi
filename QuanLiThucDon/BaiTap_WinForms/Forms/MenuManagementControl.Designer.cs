namespace BaiTap_WinForms.Forms
{
    partial class MenuManagementControl
    {
        private System.ComponentModel.IContainer components = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuRoot = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuHeader = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuNavigation = null!;
        private System.Windows.Forms.Label lblMenuTitle = null!;
        private System.Windows.Forms.Label lblMenuSubtitle = null!;
        private System.Windows.Forms.Label lblMenuBranchBadge = null!;
        private System.Windows.Forms.Button btnMenuCategories = null!;
        private System.Windows.Forms.Button btnMenuProducts = null!;
        private System.Windows.Forms.Button btnMenuUnits = null!;
        private System.Windows.Forms.Button btnMenuPrices = null!;
        private System.Windows.Forms.TabControl tabMenuSections = null!;
        private System.Windows.Forms.TabPage tabMenuCategories = null!;
        private System.Windows.Forms.TabPage tabMenuProducts = null!;
        private System.Windows.Forms.TabPage tabMenuUnits = null!;
        private System.Windows.Forms.TabPage tabMenuPrices = null!;
        private System.Windows.Forms.SplitContainer splitMenuCategories = null!;
        private System.Windows.Forms.SplitContainer splitMenuProducts = null!;
        private System.Windows.Forms.SplitContainer splitMenuUnits = null!;
        private System.Windows.Forms.SplitContainer splitMenuPrices = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuCategoryList = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuProductList = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuUnitList = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuPriceList = null!;
        private System.Windows.Forms.Panel pnlMenuCategorySearch = null!;
        private System.Windows.Forms.Panel pnlMenuProductSearch = null!;
        private System.Windows.Forms.Panel pnlMenuUnitSearch = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuPriceFilters = null!;
        private System.Windows.Forms.FlowLayoutPanel pnlMenuPriceBranchFilter = null!;
        private System.Windows.Forms.DataGridView dgvMenuCategories = null!;
        private System.Windows.Forms.DataGridView dgvMenuProducts = null!;
        private System.Windows.Forms.DataGridView dgvMenuUnits = null!;
        private System.Windows.Forms.DataGridView dgvMenuPrices = null!;
        private System.Windows.Forms.TextBox txtMenuCategorySearch = null!;
        private System.Windows.Forms.TextBox txtMenuProductSearch = null!;
        private System.Windows.Forms.TextBox txtMenuUnitSearch = null!;
        private System.Windows.Forms.TextBox txtMenuPriceSearch = null!;
        private System.Windows.Forms.Panel pnlMenuCategoryEditor = null!;
        private System.Windows.Forms.Panel pnlMenuProductEditor = null!;
        private System.Windows.Forms.Panel pnlMenuUnitEditor = null!;
        private System.Windows.Forms.Panel pnlMenuPriceEditor = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuCategoryEditor = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuProductEditor = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuUnitEditor = null!;
        private System.Windows.Forms.TableLayoutPanel tlpMenuPriceEditor = null!;
        private System.Windows.Forms.FlowLayoutPanel pnlMenuCategoryActions = null!;
        private System.Windows.Forms.FlowLayoutPanel pnlMenuProductActions = null!;
        private System.Windows.Forms.FlowLayoutPanel pnlMenuUnitActions = null!;
        private System.Windows.Forms.FlowLayoutPanel pnlMenuPriceActions = null!;
        private System.Windows.Forms.Label lblMenuCategoryEditorTitle = null!;
        private System.Windows.Forms.Label lblMenuProductEditorTitle = null!;
        private System.Windows.Forms.Label lblMenuUnitEditorTitle = null!;
        private System.Windows.Forms.Label lblMenuPriceEditorTitle = null!;
        private System.Windows.Forms.Label lblMenuCategoryName = null!;
        private System.Windows.Forms.Label lblMenuProductName = null!;
        private System.Windows.Forms.Label lblMenuProductSku = null!;
        private System.Windows.Forms.Label lblMenuProductCategory = null!;
        private System.Windows.Forms.Label lblMenuProductUnit = null!;
        private System.Windows.Forms.Label lblMenuProductType = null!;
        private System.Windows.Forms.Label lblMenuUnitName = null!;
        private System.Windows.Forms.Label lblMenuUnitType = null!;
        private System.Windows.Forms.Label lblMenuUnitHint = null!;
        private System.Windows.Forms.Label lblMenuPriceProduct = null!;
        private System.Windows.Forms.Label lblMenuPriceValue = null!;
        private System.Windows.Forms.Label lblMenuPriceHint = null!;
        private System.Windows.Forms.TextBox txtMenuCategoryName = null!;
        private System.Windows.Forms.CheckBox chkMenuCategoryActive = null!;
        private System.Windows.Forms.TextBox txtMenuProductSku = null!;
        private System.Windows.Forms.TextBox txtMenuProductName = null!;
        private System.Windows.Forms.ComboBox cboMenuProductCategory = null!;
        private System.Windows.Forms.ComboBox cboMenuProductUnit = null!;
        private System.Windows.Forms.ComboBox cboMenuProductType = null!;
        private System.Windows.Forms.CheckBox chkMenuProductRecipe = null!;
        private System.Windows.Forms.CheckBox chkMenuProductActive = null!;
        private System.Windows.Forms.TextBox txtMenuUnitName = null!;
        private System.Windows.Forms.ComboBox cboMenuUnitType = null!;
        private System.Windows.Forms.Label lblMenuPriceBranch = null!;
        private System.Windows.Forms.ComboBox cboMenuPriceBranch = null!;
        private System.Windows.Forms.ComboBox cboMenuPriceProduct = null!;
        private System.Windows.Forms.NumericUpDown numMenuPrice = null!;
        private System.Windows.Forms.CheckBox chkMenuPriceAvailable = null!;
        private System.Windows.Forms.Button btnMenuCategoryAdd = null!;
        private System.Windows.Forms.Button btnMenuCategoryUpdate = null!;
        private System.Windows.Forms.Button btnMenuCategoryToggle = null!;
        private System.Windows.Forms.Button btnMenuCategoryReset = null!;
        private System.Windows.Forms.Button btnMenuProductAdd = null!;
        private System.Windows.Forms.Button btnMenuProductUpdate = null!;
        private System.Windows.Forms.Button btnMenuProductDeactivate = null!;
        private System.Windows.Forms.Button btnMenuProductReset = null!;
        private System.Windows.Forms.Button btnMenuUnitAdd = null!;
        private System.Windows.Forms.Button btnMenuUnitUpdate = null!;
        private System.Windows.Forms.Button btnMenuUnitDelete = null!;
        private System.Windows.Forms.Button btnMenuUnitReset = null!;
        private System.Windows.Forms.Button btnMenuPriceAdd = null!;
        private System.Windows.Forms.Button btnMenuPriceUpdate = null!;
        private System.Windows.Forms.Button btnMenuPriceToggle = null!;
        private System.Windows.Forms.Button btnMenuPriceReset = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tlpMenuRoot = new TableLayoutPanel();
            tlpMenuHeader = new TableLayoutPanel();
            lblMenuTitle = new Label();
            lblMenuSubtitle = new Label();
            lblMenuBranchBadge = new Label();
            tlpMenuNavigation = new TableLayoutPanel();
            btnMenuCategories = new Button();
            btnMenuProducts = new Button();
            btnMenuUnits = new Button();
            btnMenuPrices = new Button();
            tabMenuSections = new TabControl();
            tabMenuCategories = new TabPage();
            tabMenuProducts = new TabPage();
            tabMenuUnits = new TabPage();
            tabMenuPrices = new TabPage();
            splitMenuCategories = new SplitContainer();
            splitMenuProducts = new SplitContainer();
            splitMenuUnits = new SplitContainer();
            splitMenuPrices = new SplitContainer();
            tlpMenuCategoryList = new TableLayoutPanel();
            tlpMenuProductList = new TableLayoutPanel();
            tlpMenuUnitList = new TableLayoutPanel();
            tlpMenuPriceList = new TableLayoutPanel();
            pnlMenuCategorySearch = new Panel();
            pnlMenuProductSearch = new Panel();
            pnlMenuUnitSearch = new Panel();
            tlpMenuPriceFilters = new TableLayoutPanel();
            pnlMenuPriceBranchFilter = new FlowLayoutPanel();
            dgvMenuCategories = new DataGridView();
            dgvMenuProducts = new DataGridView();
            dgvMenuUnits = new DataGridView();
            dgvMenuPrices = new DataGridView();
            txtMenuCategorySearch = new TextBox();
            txtMenuProductSearch = new TextBox();
            txtMenuUnitSearch = new TextBox();
            txtMenuPriceSearch = new TextBox();
            pnlMenuCategoryEditor = new Panel();
            pnlMenuProductEditor = new Panel();
            pnlMenuUnitEditor = new Panel();
            pnlMenuPriceEditor = new Panel();
            tlpMenuCategoryEditor = new TableLayoutPanel();
            tlpMenuProductEditor = new TableLayoutPanel();
            tlpMenuUnitEditor = new TableLayoutPanel();
            tlpMenuPriceEditor = new TableLayoutPanel();
            pnlMenuCategoryActions = new FlowLayoutPanel();
            pnlMenuProductActions = new FlowLayoutPanel();
            pnlMenuUnitActions = new FlowLayoutPanel();
            pnlMenuPriceActions = new FlowLayoutPanel();
            lblMenuCategoryEditorTitle = new Label();
            lblMenuProductEditorTitle = new Label();
            lblMenuUnitEditorTitle = new Label();
            lblMenuPriceEditorTitle = new Label();
            lblMenuCategoryName = new Label();
            lblMenuProductName = new Label();
            lblMenuProductSku = new Label();
            lblMenuProductCategory = new Label();
            lblMenuProductUnit = new Label();
            lblMenuProductType = new Label();
            lblMenuUnitName = new Label();
            lblMenuUnitType = new Label();
            lblMenuUnitHint = new Label();
            lblMenuPriceProduct = new Label();
            lblMenuPriceValue = new Label();
            lblMenuPriceHint = new Label();
            txtMenuCategoryName = new TextBox();
            chkMenuCategoryActive = new CheckBox();
            txtMenuProductSku = new TextBox();
            txtMenuProductName = new TextBox();
            cboMenuProductCategory = new ComboBox();
            cboMenuProductUnit = new ComboBox();
            cboMenuProductType = new ComboBox();
            chkMenuProductRecipe = new CheckBox();
            chkMenuProductActive = new CheckBox();
            txtMenuUnitName = new TextBox();
            cboMenuUnitType = new ComboBox();
            lblMenuPriceBranch = new Label();
            cboMenuPriceBranch = new ComboBox();
            cboMenuPriceProduct = new ComboBox();
            numMenuPrice = new NumericUpDown();
            chkMenuPriceAvailable = new CheckBox();
            btnMenuCategoryAdd = new Button();
            btnMenuCategoryUpdate = new Button();
            btnMenuCategoryToggle = new Button();
            btnMenuCategoryReset = new Button();
            btnMenuProductAdd = new Button();
            btnMenuProductUpdate = new Button();
            btnMenuProductDeactivate = new Button();
            btnMenuProductReset = new Button();
            btnMenuUnitAdd = new Button();
            btnMenuUnitUpdate = new Button();
            btnMenuUnitDelete = new Button();
            btnMenuUnitReset = new Button();
            btnMenuPriceAdd = new Button();
            btnMenuPriceUpdate = new Button();
            btnMenuPriceToggle = new Button();
            btnMenuPriceReset = new Button();
            tlpMenuRoot.SuspendLayout();
            tlpMenuHeader.SuspendLayout();
            tlpMenuNavigation.SuspendLayout();
            tabMenuSections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMenuCategories).BeginInit();
            splitMenuCategories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMenuProducts).BeginInit();
            splitMenuProducts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMenuUnits).BeginInit();
            splitMenuUnits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMenuPrices).BeginInit();
            splitMenuPrices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMenuCategories).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMenuProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMenuUnits).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMenuPrices).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numMenuPrice).BeginInit();
            SuspendLayout();
            // 
            // tlpMenuRoot
            // 
            tlpMenuRoot.ColumnCount = 1;
            tlpMenuRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpMenuRoot.Controls.Add(tlpMenuHeader, 0, 0);
            tlpMenuRoot.Controls.Add(tlpMenuNavigation, 0, 1);
            tlpMenuRoot.Controls.Add(tabMenuSections, 0, 2);
            tlpMenuRoot.Dock = DockStyle.Fill;
            tlpMenuRoot.Location = new Point(0, 0);
            tlpMenuRoot.Margin = new Padding(0);
            tlpMenuRoot.Name = "tlpMenuRoot";
            tlpMenuRoot.RowCount = 3;
            tlpMenuRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 156F));
            tlpMenuRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 87F));
            tlpMenuRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMenuRoot.Size = new Size(1800, 1140);
            tlpMenuRoot.TabIndex = 0;
            // 
            // tlpMenuHeader
            // 
            tlpMenuHeader.BackColor = Color.White;
            tlpMenuHeader.ColumnCount = 2;
            tlpMenuHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tlpMenuHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tlpMenuHeader.Controls.Add(lblMenuTitle, 0, 0);
            tlpMenuHeader.Controls.Add(lblMenuSubtitle, 0, 1);
            tlpMenuHeader.Controls.Add(lblMenuBranchBadge, 1, 0);
            tlpMenuHeader.Dock = DockStyle.Fill;
            tlpMenuHeader.Location = new Point(4, 4);
            tlpMenuHeader.Margin = new Padding(4, 4, 4, 4);
            tlpMenuHeader.Name = "tlpMenuHeader";
            tlpMenuHeader.Padding = new Padding(30, 12, 30, 12);
            tlpMenuHeader.RowCount = 2;
            tlpMenuHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 58F));
            tlpMenuHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 42F));
            tlpMenuHeader.Size = new Size(1792, 148);
            tlpMenuHeader.TabIndex = 0;
            // 
            // lblMenuTitle
            // 
            lblMenuTitle.AutoEllipsis = true;
            lblMenuTitle.Dock = DockStyle.Fill;
            lblMenuTitle.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
            lblMenuTitle.ForeColor = Color.FromArgb(34, 48, 62);
            lblMenuTitle.Location = new Point(30, 12);
            lblMenuTitle.Margin = new Padding(0);
            lblMenuTitle.Name = "lblMenuTitle";
            lblMenuTitle.Size = new Size(1212, 71);
            lblMenuTitle.TabIndex = 0;
            lblMenuTitle.Text = "QUẢN LÝ THỰC ĐƠN";
            lblMenuTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMenuSubtitle
            // 
            lblMenuSubtitle.AutoEllipsis = true;
            lblMenuSubtitle.Dock = DockStyle.Fill;
            lblMenuSubtitle.Font = new Font("Segoe UI", 9.5F);
            lblMenuSubtitle.ForeColor = Color.FromArgb(105, 119, 132);
            lblMenuSubtitle.Location = new Point(33, 83);
            lblMenuSubtitle.Margin = new Padding(3, 0, 0, 0);
            lblMenuSubtitle.Name = "lblMenuSubtitle";
            lblMenuSubtitle.Size = new Size(1209, 53);
            lblMenuSubtitle.TabIndex = 1;
            lblMenuSubtitle.Text = "Danh mục, món ăn, đơn vị tính và giá bán theo từng chi nhánh";
            lblMenuSubtitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMenuBranchBadge
            // 
            lblMenuBranchBadge.AutoEllipsis = true;
            lblMenuBranchBadge.BackColor = Color.FromArgb(255, 241, 231);
            lblMenuBranchBadge.Dock = DockStyle.Fill;
            lblMenuBranchBadge.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMenuBranchBadge.ForeColor = Color.FromArgb(166, 75, 29);
            lblMenuBranchBadge.Location = new Point(1260, 24);
            lblMenuBranchBadge.Margin = new Padding(18, 12, 0, 12);
            lblMenuBranchBadge.Name = "lblMenuBranchBadge";
            lblMenuBranchBadge.Padding = new Padding(12, 0, 12, 0);
            tlpMenuHeader.SetRowSpan(lblMenuBranchBadge, 2);
            lblMenuBranchBadge.Size = new Size(502, 100);
            lblMenuBranchBadge.TabIndex = 2;
            lblMenuBranchBadge.Text = "Đang tải phạm vi chi nhánh...";
            lblMenuBranchBadge.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tlpMenuNavigation
            // 
            tlpMenuNavigation.BackColor = Color.FromArgb(244, 247, 249);
            tlpMenuNavigation.ColumnCount = 4;
            tlpMenuNavigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpMenuNavigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpMenuNavigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpMenuNavigation.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpMenuNavigation.Controls.Add(btnMenuCategories, 0, 0);
            tlpMenuNavigation.Controls.Add(btnMenuProducts, 1, 0);
            tlpMenuNavigation.Controls.Add(btnMenuUnits, 2, 0);
            tlpMenuNavigation.Controls.Add(btnMenuPrices, 3, 0);
            tlpMenuNavigation.Dock = DockStyle.Fill;
            tlpMenuNavigation.Location = new Point(4, 160);
            tlpMenuNavigation.Margin = new Padding(4, 4, 4, 4);
            tlpMenuNavigation.Name = "tlpMenuNavigation";
            tlpMenuNavigation.Padding = new Padding(24, 12, 24, 6);
            tlpMenuNavigation.RowCount = 1;
            tlpMenuNavigation.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpMenuNavigation.Size = new Size(1792, 79);
            tlpMenuNavigation.TabIndex = 1;
            // 
            // btnMenuCategories
            // 
            btnMenuCategories.BackColor = Color.White;
            btnMenuCategories.Cursor = Cursors.Hand;
            btnMenuCategories.Dock = DockStyle.Fill;
            btnMenuCategories.FlatAppearance.BorderColor = Color.FromArgb(224, 231, 236);
            btnMenuCategories.FlatStyle = FlatStyle.Flat;
            btnMenuCategories.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMenuCategories.ForeColor = Color.FromArgb(105, 119, 132);
            btnMenuCategories.Location = new Point(24, 12);
            btnMenuCategories.Margin = new Padding(0, 0, 12, 0);
            btnMenuCategories.Name = "btnMenuCategories";
            btnMenuCategories.Padding = new Padding(12, 0, 6, 0);
            btnMenuCategories.Size = new Size(424, 61);
            btnMenuCategories.TabIndex = 0;
            btnMenuCategories.Text = "01   Danh mục";
            btnMenuCategories.UseVisualStyleBackColor = false;
            btnMenuCategories.Click += btnMenuCategories_Click;
            // 
            // btnMenuProducts
            // 
            btnMenuProducts.BackColor = Color.White;
            btnMenuProducts.Cursor = Cursors.Hand;
            btnMenuProducts.Dock = DockStyle.Fill;
            btnMenuProducts.FlatAppearance.BorderColor = Color.FromArgb(224, 231, 236);
            btnMenuProducts.FlatStyle = FlatStyle.Flat;
            btnMenuProducts.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMenuProducts.ForeColor = Color.FromArgb(105, 119, 132);
            btnMenuProducts.Location = new Point(460, 12);
            btnMenuProducts.Margin = new Padding(0, 0, 12, 0);
            btnMenuProducts.Name = "btnMenuProducts";
            btnMenuProducts.Padding = new Padding(12, 0, 6, 0);
            btnMenuProducts.Size = new Size(424, 61);
            btnMenuProducts.TabIndex = 1;
            btnMenuProducts.Text = "02   Sản phẩm";
            btnMenuProducts.UseVisualStyleBackColor = false;
            // 
            // btnMenuUnits
            // 
            btnMenuUnits.BackColor = Color.White;
            btnMenuUnits.Cursor = Cursors.Hand;
            btnMenuUnits.Dock = DockStyle.Fill;
            btnMenuUnits.FlatAppearance.BorderColor = Color.FromArgb(224, 231, 236);
            btnMenuUnits.FlatStyle = FlatStyle.Flat;
            btnMenuUnits.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMenuUnits.ForeColor = Color.FromArgb(105, 119, 132);
            btnMenuUnits.Location = new Point(896, 12);
            btnMenuUnits.Margin = new Padding(0, 0, 12, 0);
            btnMenuUnits.Name = "btnMenuUnits";
            btnMenuUnits.Padding = new Padding(12, 0, 6, 0);
            btnMenuUnits.Size = new Size(424, 61);
            btnMenuUnits.TabIndex = 2;
            btnMenuUnits.Text = "03   Đơn vị tính";
            btnMenuUnits.UseVisualStyleBackColor = false;
            // 
            // btnMenuPrices
            // 
            btnMenuPrices.BackColor = Color.White;
            btnMenuPrices.Cursor = Cursors.Hand;
            btnMenuPrices.Dock = DockStyle.Fill;
            btnMenuPrices.FlatAppearance.BorderColor = Color.FromArgb(224, 231, 236);
            btnMenuPrices.FlatStyle = FlatStyle.Flat;
            btnMenuPrices.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnMenuPrices.ForeColor = Color.FromArgb(105, 119, 132);
            btnMenuPrices.Location = new Point(1332, 12);
            btnMenuPrices.Margin = new Padding(0, 0, 12, 0);
            btnMenuPrices.Name = "btnMenuPrices";
            btnMenuPrices.Padding = new Padding(12, 0, 6, 0);
            btnMenuPrices.Size = new Size(424, 61);
            btnMenuPrices.TabIndex = 3;
            btnMenuPrices.Text = "04   Giá theo chi nhánh";
            btnMenuPrices.UseVisualStyleBackColor = false;
            // 
            // tabMenuSections
            // 
            tabMenuSections.Appearance = TabAppearance.FlatButtons;
            tabMenuSections.Controls.Add(tabMenuCategories);
            tabMenuSections.Controls.Add(tabMenuProducts);
            tabMenuSections.Controls.Add(tabMenuUnits);
            tabMenuSections.Controls.Add(tabMenuPrices);
            tabMenuSections.Dock = DockStyle.Fill;
            tabMenuSections.ItemSize = new Size(0, 1);
            tabMenuSections.Location = new Point(4, 247);
            tabMenuSections.Margin = new Padding(4, 4, 4, 4);
            tabMenuSections.Name = "tabMenuSections";
            tabMenuSections.Padding = new Point(0, 0);
            tabMenuSections.SelectedIndex = 0;
            tabMenuSections.Size = new Size(1792, 889);
            tabMenuSections.SizeMode = TabSizeMode.Fixed;
            tabMenuSections.TabIndex = 2;
            // 
            // tabMenuCategories
            // 
            tabMenuCategories.BackColor = Color.FromArgb(244, 247, 249);
            tabMenuCategories.Location = new Point(4, 5);
            tabMenuCategories.Margin = new Padding(4, 4, 4, 4);
            tabMenuCategories.Name = "tabMenuCategories";
            tabMenuCategories.Padding = new Padding(18, 18, 18, 18);
            tabMenuCategories.Size = new Size(1784, 880);
            tabMenuCategories.TabIndex = 0;
            tabMenuCategories.Text = "Danh mục món";
            // 
            // tabMenuProducts
            // 
            tabMenuProducts.BackColor = Color.FromArgb(244, 247, 249);
            tabMenuProducts.Location = new Point(4, 5);
            tabMenuProducts.Margin = new Padding(4, 4, 4, 4);
            tabMenuProducts.Name = "tabMenuProducts";
            tabMenuProducts.Padding = new Padding(18, 18, 18, 18);
            tabMenuProducts.Size = new Size(283, 0);
            tabMenuProducts.TabIndex = 1;
            tabMenuProducts.Text = "Sản phẩm";
            // 
            // tabMenuUnits
            // 
            tabMenuUnits.BackColor = Color.FromArgb(244, 247, 249);
            tabMenuUnits.Location = new Point(4, 5);
            tabMenuUnits.Margin = new Padding(4, 4, 4, 4);
            tabMenuUnits.Name = "tabMenuUnits";
            tabMenuUnits.Padding = new Padding(18, 18, 18, 18);
            tabMenuUnits.Size = new Size(283, 0);
            tabMenuUnits.TabIndex = 2;
            tabMenuUnits.Text = "Đơn vị tính";
            // 
            // tabMenuPrices
            // 
            tabMenuPrices.BackColor = Color.FromArgb(244, 247, 249);
            tabMenuPrices.Location = new Point(4, 5);
            tabMenuPrices.Margin = new Padding(4, 4, 4, 4);
            tabMenuPrices.Name = "tabMenuPrices";
            tabMenuPrices.Padding = new Padding(18, 18, 18, 18);
            tabMenuPrices.Size = new Size(283, 0);
            tabMenuPrices.TabIndex = 3;
            tabMenuPrices.Text = "Giá bán chi nhánh";
            // 
            // splitMenuCategories
            // 
            splitMenuCategories.Location = new Point(0, 0);
            splitMenuCategories.Name = "splitMenuCategories";
            splitMenuCategories.Size = new Size(150, 100);
            splitMenuCategories.TabIndex = 0;
            // 
            // splitMenuProducts
            // 
            splitMenuProducts.Location = new Point(0, 0);
            splitMenuProducts.Name = "splitMenuProducts";
            splitMenuProducts.Size = new Size(150, 100);
            splitMenuProducts.TabIndex = 0;
            // 
            // splitMenuUnits
            // 
            splitMenuUnits.Location = new Point(0, 0);
            splitMenuUnits.Name = "splitMenuUnits";
            splitMenuUnits.Size = new Size(150, 100);
            splitMenuUnits.TabIndex = 0;
            // 
            // splitMenuPrices
            // 
            splitMenuPrices.Location = new Point(0, 0);
            splitMenuPrices.Name = "splitMenuPrices";
            splitMenuPrices.Size = new Size(150, 100);
            splitMenuPrices.TabIndex = 0;
            // 
            // tlpMenuCategoryList
            // 
            tlpMenuCategoryList.Location = new Point(0, 0);
            tlpMenuCategoryList.Name = "tlpMenuCategoryList";
            tlpMenuCategoryList.Size = new Size(200, 100);
            tlpMenuCategoryList.TabIndex = 0;
            // 
            // tlpMenuProductList
            // 
            tlpMenuProductList.Location = new Point(0, 0);
            tlpMenuProductList.Name = "tlpMenuProductList";
            tlpMenuProductList.Size = new Size(200, 100);
            tlpMenuProductList.TabIndex = 0;
            // 
            // tlpMenuUnitList
            // 
            tlpMenuUnitList.Location = new Point(0, 0);
            tlpMenuUnitList.Name = "tlpMenuUnitList";
            tlpMenuUnitList.Size = new Size(200, 100);
            tlpMenuUnitList.TabIndex = 0;
            // 
            // tlpMenuPriceList
            // 
            tlpMenuPriceList.Location = new Point(0, 0);
            tlpMenuPriceList.Name = "tlpMenuPriceList";
            tlpMenuPriceList.Size = new Size(200, 100);
            tlpMenuPriceList.TabIndex = 0;
            // 
            // pnlMenuCategorySearch
            // 
            pnlMenuCategorySearch.Location = new Point(0, 0);
            pnlMenuCategorySearch.Name = "pnlMenuCategorySearch";
            pnlMenuCategorySearch.Size = new Size(200, 100);
            pnlMenuCategorySearch.TabIndex = 0;
            // 
            // pnlMenuProductSearch
            // 
            pnlMenuProductSearch.Location = new Point(0, 0);
            pnlMenuProductSearch.Name = "pnlMenuProductSearch";
            pnlMenuProductSearch.Size = new Size(200, 100);
            pnlMenuProductSearch.TabIndex = 0;
            // 
            // pnlMenuUnitSearch
            // 
            pnlMenuUnitSearch.Location = new Point(0, 0);
            pnlMenuUnitSearch.Name = "pnlMenuUnitSearch";
            pnlMenuUnitSearch.Size = new Size(200, 100);
            pnlMenuUnitSearch.TabIndex = 0;
            // 
            // tlpMenuPriceFilters
            // 
            tlpMenuPriceFilters.Location = new Point(0, 0);
            tlpMenuPriceFilters.Name = "tlpMenuPriceFilters";
            tlpMenuPriceFilters.Size = new Size(200, 100);
            tlpMenuPriceFilters.TabIndex = 0;
            // 
            // pnlMenuPriceBranchFilter
            // 
            pnlMenuPriceBranchFilter.Location = new Point(0, 0);
            pnlMenuPriceBranchFilter.Name = "pnlMenuPriceBranchFilter";
            pnlMenuPriceBranchFilter.Size = new Size(200, 100);
            pnlMenuPriceBranchFilter.TabIndex = 0;
            // 
            // dgvMenuCategories
            // 
            dgvMenuCategories.ColumnHeadersHeight = 34;
            dgvMenuCategories.Location = new Point(0, 0);
            dgvMenuCategories.Name = "dgvMenuCategories";
            dgvMenuCategories.RowHeadersWidth = 62;
            dgvMenuCategories.Size = new Size(240, 150);
            dgvMenuCategories.TabIndex = 0;
            // 
            // dgvMenuProducts
            // 
            dgvMenuProducts.ColumnHeadersHeight = 34;
            dgvMenuProducts.Location = new Point(0, 0);
            dgvMenuProducts.Name = "dgvMenuProducts";
            dgvMenuProducts.RowHeadersWidth = 62;
            dgvMenuProducts.Size = new Size(240, 150);
            dgvMenuProducts.TabIndex = 0;
            // 
            // dgvMenuUnits
            // 
            dgvMenuUnits.ColumnHeadersHeight = 34;
            dgvMenuUnits.Location = new Point(0, 0);
            dgvMenuUnits.Name = "dgvMenuUnits";
            dgvMenuUnits.RowHeadersWidth = 62;
            dgvMenuUnits.Size = new Size(240, 150);
            dgvMenuUnits.TabIndex = 0;
            // 
            // dgvMenuPrices
            // 
            dgvMenuPrices.ColumnHeadersHeight = 34;
            dgvMenuPrices.Location = new Point(0, 0);
            dgvMenuPrices.Name = "dgvMenuPrices";
            dgvMenuPrices.RowHeadersWidth = 62;
            dgvMenuPrices.Size = new Size(240, 150);
            dgvMenuPrices.TabIndex = 0;
            // 
            // txtMenuCategorySearch
            // 
            txtMenuCategorySearch.Location = new Point(0, 0);
            txtMenuCategorySearch.Name = "txtMenuCategorySearch";
            txtMenuCategorySearch.Size = new Size(100, 31);
            txtMenuCategorySearch.TabIndex = 0;
            // 
            // txtMenuProductSearch
            // 
            txtMenuProductSearch.Location = new Point(0, 0);
            txtMenuProductSearch.Name = "txtMenuProductSearch";
            txtMenuProductSearch.Size = new Size(100, 31);
            txtMenuProductSearch.TabIndex = 0;
            // 
            // txtMenuUnitSearch
            // 
            txtMenuUnitSearch.Location = new Point(0, 0);
            txtMenuUnitSearch.Name = "txtMenuUnitSearch";
            txtMenuUnitSearch.Size = new Size(100, 31);
            txtMenuUnitSearch.TabIndex = 0;
            // 
            // txtMenuPriceSearch
            // 
            txtMenuPriceSearch.Location = new Point(0, 0);
            txtMenuPriceSearch.Name = "txtMenuPriceSearch";
            txtMenuPriceSearch.Size = new Size(100, 31);
            txtMenuPriceSearch.TabIndex = 0;
            // 
            // pnlMenuCategoryEditor
            // 
            pnlMenuCategoryEditor.Location = new Point(0, 0);
            pnlMenuCategoryEditor.Name = "pnlMenuCategoryEditor";
            pnlMenuCategoryEditor.Size = new Size(200, 100);
            pnlMenuCategoryEditor.TabIndex = 0;
            // 
            // pnlMenuProductEditor
            // 
            pnlMenuProductEditor.Location = new Point(0, 0);
            pnlMenuProductEditor.Name = "pnlMenuProductEditor";
            pnlMenuProductEditor.Size = new Size(200, 100);
            pnlMenuProductEditor.TabIndex = 0;
            // 
            // pnlMenuUnitEditor
            // 
            pnlMenuUnitEditor.Location = new Point(0, 0);
            pnlMenuUnitEditor.Name = "pnlMenuUnitEditor";
            pnlMenuUnitEditor.Size = new Size(200, 100);
            pnlMenuUnitEditor.TabIndex = 0;
            // 
            // pnlMenuPriceEditor
            // 
            pnlMenuPriceEditor.Location = new Point(0, 0);
            pnlMenuPriceEditor.Name = "pnlMenuPriceEditor";
            pnlMenuPriceEditor.Size = new Size(200, 100);
            pnlMenuPriceEditor.TabIndex = 0;
            // 
            // tlpMenuCategoryEditor
            // 
            tlpMenuCategoryEditor.Location = new Point(0, 0);
            tlpMenuCategoryEditor.Name = "tlpMenuCategoryEditor";
            tlpMenuCategoryEditor.Size = new Size(200, 100);
            tlpMenuCategoryEditor.TabIndex = 0;
            // 
            // tlpMenuProductEditor
            // 
            tlpMenuProductEditor.Location = new Point(0, 0);
            tlpMenuProductEditor.Name = "tlpMenuProductEditor";
            tlpMenuProductEditor.Size = new Size(200, 100);
            tlpMenuProductEditor.TabIndex = 0;
            // 
            // tlpMenuUnitEditor
            // 
            tlpMenuUnitEditor.Location = new Point(0, 0);
            tlpMenuUnitEditor.Name = "tlpMenuUnitEditor";
            tlpMenuUnitEditor.Size = new Size(200, 100);
            tlpMenuUnitEditor.TabIndex = 0;
            // 
            // tlpMenuPriceEditor
            // 
            tlpMenuPriceEditor.Location = new Point(0, 0);
            tlpMenuPriceEditor.Name = "tlpMenuPriceEditor";
            tlpMenuPriceEditor.Size = new Size(200, 100);
            tlpMenuPriceEditor.TabIndex = 0;
            // 
            // pnlMenuCategoryActions
            // 
            pnlMenuCategoryActions.Location = new Point(0, 0);
            pnlMenuCategoryActions.Name = "pnlMenuCategoryActions";
            pnlMenuCategoryActions.Size = new Size(200, 100);
            pnlMenuCategoryActions.TabIndex = 0;
            // 
            // pnlMenuProductActions
            // 
            pnlMenuProductActions.Location = new Point(0, 0);
            pnlMenuProductActions.Name = "pnlMenuProductActions";
            pnlMenuProductActions.Size = new Size(200, 100);
            pnlMenuProductActions.TabIndex = 0;
            // 
            // pnlMenuUnitActions
            // 
            pnlMenuUnitActions.Location = new Point(0, 0);
            pnlMenuUnitActions.Name = "pnlMenuUnitActions";
            pnlMenuUnitActions.Size = new Size(200, 100);
            pnlMenuUnitActions.TabIndex = 0;
            // 
            // pnlMenuPriceActions
            // 
            pnlMenuPriceActions.Location = new Point(0, 0);
            pnlMenuPriceActions.Name = "pnlMenuPriceActions";
            pnlMenuPriceActions.Size = new Size(200, 100);
            pnlMenuPriceActions.TabIndex = 0;
            // 
            // lblMenuCategoryEditorTitle
            // 
            lblMenuCategoryEditorTitle.Location = new Point(0, 0);
            lblMenuCategoryEditorTitle.Name = "lblMenuCategoryEditorTitle";
            lblMenuCategoryEditorTitle.Size = new Size(100, 23);
            lblMenuCategoryEditorTitle.TabIndex = 0;
            // 
            // lblMenuProductEditorTitle
            // 
            lblMenuProductEditorTitle.Location = new Point(0, 0);
            lblMenuProductEditorTitle.Name = "lblMenuProductEditorTitle";
            lblMenuProductEditorTitle.Size = new Size(100, 23);
            lblMenuProductEditorTitle.TabIndex = 0;
            // 
            // lblMenuUnitEditorTitle
            // 
            lblMenuUnitEditorTitle.Location = new Point(0, 0);
            lblMenuUnitEditorTitle.Name = "lblMenuUnitEditorTitle";
            lblMenuUnitEditorTitle.Size = new Size(100, 23);
            lblMenuUnitEditorTitle.TabIndex = 0;
            // 
            // lblMenuPriceEditorTitle
            // 
            lblMenuPriceEditorTitle.Location = new Point(0, 0);
            lblMenuPriceEditorTitle.Name = "lblMenuPriceEditorTitle";
            lblMenuPriceEditorTitle.Size = new Size(100, 23);
            lblMenuPriceEditorTitle.TabIndex = 0;
            // 
            // lblMenuCategoryName
            // 
            lblMenuCategoryName.Location = new Point(0, 0);
            lblMenuCategoryName.Name = "lblMenuCategoryName";
            lblMenuCategoryName.Size = new Size(100, 23);
            lblMenuCategoryName.TabIndex = 0;
            // 
            // lblMenuProductName
            // 
            lblMenuProductName.Location = new Point(0, 0);
            lblMenuProductName.Name = "lblMenuProductName";
            lblMenuProductName.Size = new Size(100, 23);
            lblMenuProductName.TabIndex = 0;
            // 
            // lblMenuProductSku
            // 
            lblMenuProductSku.Location = new Point(0, 0);
            lblMenuProductSku.Name = "lblMenuProductSku";
            lblMenuProductSku.Size = new Size(100, 23);
            lblMenuProductSku.TabIndex = 0;
            // 
            // lblMenuProductCategory
            // 
            lblMenuProductCategory.Location = new Point(0, 0);
            lblMenuProductCategory.Name = "lblMenuProductCategory";
            lblMenuProductCategory.Size = new Size(100, 23);
            lblMenuProductCategory.TabIndex = 0;
            // 
            // lblMenuProductUnit
            // 
            lblMenuProductUnit.Location = new Point(0, 0);
            lblMenuProductUnit.Name = "lblMenuProductUnit";
            lblMenuProductUnit.Size = new Size(100, 23);
            lblMenuProductUnit.TabIndex = 0;
            // 
            // lblMenuProductType
            // 
            lblMenuProductType.Location = new Point(0, 0);
            lblMenuProductType.Name = "lblMenuProductType";
            lblMenuProductType.Size = new Size(100, 23);
            lblMenuProductType.TabIndex = 0;
            // 
            // lblMenuUnitName
            // 
            lblMenuUnitName.Location = new Point(0, 0);
            lblMenuUnitName.Name = "lblMenuUnitName";
            lblMenuUnitName.Size = new Size(100, 23);
            lblMenuUnitName.TabIndex = 0;
            // 
            // lblMenuUnitType
            // 
            lblMenuUnitType.Location = new Point(0, 0);
            lblMenuUnitType.Name = "lblMenuUnitType";
            lblMenuUnitType.Size = new Size(100, 23);
            lblMenuUnitType.TabIndex = 0;
            // 
            // lblMenuUnitHint
            // 
            lblMenuUnitHint.Location = new Point(0, 0);
            lblMenuUnitHint.Name = "lblMenuUnitHint";
            lblMenuUnitHint.Size = new Size(100, 23);
            lblMenuUnitHint.TabIndex = 0;
            // 
            // lblMenuPriceProduct
            // 
            lblMenuPriceProduct.Location = new Point(0, 0);
            lblMenuPriceProduct.Name = "lblMenuPriceProduct";
            lblMenuPriceProduct.Size = new Size(100, 23);
            lblMenuPriceProduct.TabIndex = 0;
            // 
            // lblMenuPriceValue
            // 
            lblMenuPriceValue.Location = new Point(0, 0);
            lblMenuPriceValue.Name = "lblMenuPriceValue";
            lblMenuPriceValue.Size = new Size(100, 23);
            lblMenuPriceValue.TabIndex = 0;
            // 
            // lblMenuPriceHint
            // 
            lblMenuPriceHint.Location = new Point(0, 0);
            lblMenuPriceHint.Name = "lblMenuPriceHint";
            lblMenuPriceHint.Size = new Size(100, 23);
            lblMenuPriceHint.TabIndex = 0;
            // 
            // txtMenuCategoryName
            // 
            txtMenuCategoryName.Location = new Point(0, 0);
            txtMenuCategoryName.Name = "txtMenuCategoryName";
            txtMenuCategoryName.Size = new Size(100, 31);
            txtMenuCategoryName.TabIndex = 0;
            // 
            // chkMenuCategoryActive
            // 
            chkMenuCategoryActive.Location = new Point(0, 0);
            chkMenuCategoryActive.Name = "chkMenuCategoryActive";
            chkMenuCategoryActive.Size = new Size(104, 24);
            chkMenuCategoryActive.TabIndex = 0;
            // 
            // txtMenuProductSku
            // 
            txtMenuProductSku.Location = new Point(0, 0);
            txtMenuProductSku.Name = "txtMenuProductSku";
            txtMenuProductSku.Size = new Size(100, 31);
            txtMenuProductSku.TabIndex = 0;
            // 
            // txtMenuProductName
            // 
            txtMenuProductName.Location = new Point(0, 0);
            txtMenuProductName.Name = "txtMenuProductName";
            txtMenuProductName.Size = new Size(100, 31);
            txtMenuProductName.TabIndex = 0;
            // 
            // cboMenuProductCategory
            // 
            cboMenuProductCategory.Location = new Point(0, 0);
            cboMenuProductCategory.Name = "cboMenuProductCategory";
            cboMenuProductCategory.Size = new Size(121, 33);
            cboMenuProductCategory.TabIndex = 0;
            // 
            // cboMenuProductUnit
            // 
            cboMenuProductUnit.Location = new Point(0, 0);
            cboMenuProductUnit.Name = "cboMenuProductUnit";
            cboMenuProductUnit.Size = new Size(121, 33);
            cboMenuProductUnit.TabIndex = 0;
            // 
            // cboMenuProductType
            // 
            cboMenuProductType.Location = new Point(0, 0);
            cboMenuProductType.Name = "cboMenuProductType";
            cboMenuProductType.Size = new Size(121, 33);
            cboMenuProductType.TabIndex = 0;
            // 
            // chkMenuProductRecipe
            // 
            chkMenuProductRecipe.Location = new Point(0, 0);
            chkMenuProductRecipe.Name = "chkMenuProductRecipe";
            chkMenuProductRecipe.Size = new Size(104, 24);
            chkMenuProductRecipe.TabIndex = 0;
            // 
            // chkMenuProductActive
            // 
            chkMenuProductActive.Location = new Point(0, 0);
            chkMenuProductActive.Name = "chkMenuProductActive";
            chkMenuProductActive.Size = new Size(104, 24);
            chkMenuProductActive.TabIndex = 0;
            // 
            // txtMenuUnitName
            // 
            txtMenuUnitName.Location = new Point(0, 0);
            txtMenuUnitName.Name = "txtMenuUnitName";
            txtMenuUnitName.Size = new Size(100, 31);
            txtMenuUnitName.TabIndex = 0;
            // 
            // cboMenuUnitType
            // 
            cboMenuUnitType.Location = new Point(0, 0);
            cboMenuUnitType.Name = "cboMenuUnitType";
            cboMenuUnitType.Size = new Size(121, 33);
            cboMenuUnitType.TabIndex = 0;
            // 
            // lblMenuPriceBranch
            // 
            lblMenuPriceBranch.Location = new Point(0, 0);
            lblMenuPriceBranch.Name = "lblMenuPriceBranch";
            lblMenuPriceBranch.Size = new Size(100, 23);
            lblMenuPriceBranch.TabIndex = 0;
            // 
            // cboMenuPriceBranch
            // 
            cboMenuPriceBranch.Location = new Point(0, 0);
            cboMenuPriceBranch.Name = "cboMenuPriceBranch";
            cboMenuPriceBranch.Size = new Size(121, 33);
            cboMenuPriceBranch.TabIndex = 0;
            // 
            // cboMenuPriceProduct
            // 
            cboMenuPriceProduct.Location = new Point(0, 0);
            cboMenuPriceProduct.Name = "cboMenuPriceProduct";
            cboMenuPriceProduct.Size = new Size(121, 33);
            cboMenuPriceProduct.TabIndex = 0;
            // 
            // numMenuPrice
            // 
            numMenuPrice.Location = new Point(0, 0);
            numMenuPrice.Name = "numMenuPrice";
            numMenuPrice.Size = new Size(120, 31);
            numMenuPrice.TabIndex = 0;
            // 
            // chkMenuPriceAvailable
            // 
            chkMenuPriceAvailable.Location = new Point(0, 0);
            chkMenuPriceAvailable.Name = "chkMenuPriceAvailable";
            chkMenuPriceAvailable.Size = new Size(104, 24);
            chkMenuPriceAvailable.TabIndex = 0;
            // 
            // btnMenuCategoryAdd
            // 
            btnMenuCategoryAdd.Location = new Point(0, 0);
            btnMenuCategoryAdd.Name = "btnMenuCategoryAdd";
            btnMenuCategoryAdd.Size = new Size(75, 23);
            btnMenuCategoryAdd.TabIndex = 0;
            // 
            // btnMenuCategoryUpdate
            // 
            btnMenuCategoryUpdate.Location = new Point(0, 0);
            btnMenuCategoryUpdate.Name = "btnMenuCategoryUpdate";
            btnMenuCategoryUpdate.Size = new Size(75, 23);
            btnMenuCategoryUpdate.TabIndex = 0;
            // 
            // btnMenuCategoryToggle
            // 
            btnMenuCategoryToggle.Location = new Point(0, 0);
            btnMenuCategoryToggle.Name = "btnMenuCategoryToggle";
            btnMenuCategoryToggle.Size = new Size(75, 23);
            btnMenuCategoryToggle.TabIndex = 0;
            // 
            // btnMenuCategoryReset
            // 
            btnMenuCategoryReset.Location = new Point(0, 0);
            btnMenuCategoryReset.Name = "btnMenuCategoryReset";
            btnMenuCategoryReset.Size = new Size(75, 23);
            btnMenuCategoryReset.TabIndex = 0;
            // 
            // btnMenuProductAdd
            // 
            btnMenuProductAdd.Location = new Point(0, 0);
            btnMenuProductAdd.Name = "btnMenuProductAdd";
            btnMenuProductAdd.Size = new Size(75, 23);
            btnMenuProductAdd.TabIndex = 0;
            // 
            // btnMenuProductUpdate
            // 
            btnMenuProductUpdate.Location = new Point(0, 0);
            btnMenuProductUpdate.Name = "btnMenuProductUpdate";
            btnMenuProductUpdate.Size = new Size(75, 23);
            btnMenuProductUpdate.TabIndex = 0;
            // 
            // btnMenuProductDeactivate
            // 
            btnMenuProductDeactivate.Location = new Point(0, 0);
            btnMenuProductDeactivate.Name = "btnMenuProductDeactivate";
            btnMenuProductDeactivate.Size = new Size(75, 23);
            btnMenuProductDeactivate.TabIndex = 0;
            // 
            // btnMenuProductReset
            // 
            btnMenuProductReset.Location = new Point(0, 0);
            btnMenuProductReset.Name = "btnMenuProductReset";
            btnMenuProductReset.Size = new Size(75, 23);
            btnMenuProductReset.TabIndex = 0;
            // 
            // btnMenuUnitAdd
            // 
            btnMenuUnitAdd.Location = new Point(0, 0);
            btnMenuUnitAdd.Name = "btnMenuUnitAdd";
            btnMenuUnitAdd.Size = new Size(75, 23);
            btnMenuUnitAdd.TabIndex = 0;
            // 
            // btnMenuUnitUpdate
            // 
            btnMenuUnitUpdate.Location = new Point(0, 0);
            btnMenuUnitUpdate.Name = "btnMenuUnitUpdate";
            btnMenuUnitUpdate.Size = new Size(75, 23);
            btnMenuUnitUpdate.TabIndex = 0;
            // 
            // btnMenuUnitDelete
            // 
            btnMenuUnitDelete.Location = new Point(0, 0);
            btnMenuUnitDelete.Name = "btnMenuUnitDelete";
            btnMenuUnitDelete.Size = new Size(75, 23);
            btnMenuUnitDelete.TabIndex = 0;
            // 
            // btnMenuUnitReset
            // 
            btnMenuUnitReset.Location = new Point(0, 0);
            btnMenuUnitReset.Name = "btnMenuUnitReset";
            btnMenuUnitReset.Size = new Size(75, 23);
            btnMenuUnitReset.TabIndex = 0;
            // 
            // btnMenuPriceAdd
            // 
            btnMenuPriceAdd.Location = new Point(0, 0);
            btnMenuPriceAdd.Name = "btnMenuPriceAdd";
            btnMenuPriceAdd.Size = new Size(75, 23);
            btnMenuPriceAdd.TabIndex = 0;
            // 
            // btnMenuPriceUpdate
            // 
            btnMenuPriceUpdate.Location = new Point(0, 0);
            btnMenuPriceUpdate.Name = "btnMenuPriceUpdate";
            btnMenuPriceUpdate.Size = new Size(75, 23);
            btnMenuPriceUpdate.TabIndex = 0;
            // 
            // btnMenuPriceToggle
            // 
            btnMenuPriceToggle.Location = new Point(0, 0);
            btnMenuPriceToggle.Name = "btnMenuPriceToggle";
            btnMenuPriceToggle.Size = new Size(75, 23);
            btnMenuPriceToggle.TabIndex = 0;
            // 
            // btnMenuPriceReset
            // 
            btnMenuPriceReset.Location = new Point(0, 0);
            btnMenuPriceReset.Name = "btnMenuPriceReset";
            btnMenuPriceReset.Size = new Size(75, 23);
            btnMenuPriceReset.TabIndex = 0;
            // 
            // MenuManagementControl
            // 
            AutoScaleDimensions = new SizeF(144F, 144F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.FromArgb(244, 247, 249);
            Controls.Add(tlpMenuRoot);
            Margin = new Padding(4, 4, 4, 4);
            MinimumSize = new Size(1350, 780);
            Name = "MenuManagementControl";
            Size = new Size(1800, 1140);
            tlpMenuRoot.ResumeLayout(false);
            tlpMenuHeader.ResumeLayout(false);
            tlpMenuNavigation.ResumeLayout(false);
            tabMenuSections.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMenuCategories).EndInit();
            splitMenuCategories.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMenuProducts).EndInit();
            splitMenuProducts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMenuUnits).EndInit();
            splitMenuUnits.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMenuPrices).EndInit();
            splitMenuPrices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvMenuCategories).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMenuProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMenuUnits).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMenuPrices).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMenuPrice).EndInit();
            ResumeLayout(false);
        }

        private void InitializeMenuPageDesign()
        {
            SetMenuControlNames(
                (splitMenuCategories, "splitMenuCategories"), (splitMenuProducts, "splitMenuProducts"),
                (splitMenuUnits, "splitMenuUnits"), (splitMenuPrices, "splitMenuPrices"),
                (tlpMenuCategoryList, "tlpMenuCategoryList"), (tlpMenuProductList, "tlpMenuProductList"),
                (tlpMenuUnitList, "tlpMenuUnitList"), (tlpMenuPriceList, "tlpMenuPriceList"),
                (pnlMenuCategorySearch, "pnlMenuCategorySearch"), (pnlMenuProductSearch, "pnlMenuProductSearch"),
                (pnlMenuUnitSearch, "pnlMenuUnitSearch"), (tlpMenuPriceFilters, "tlpMenuPriceFilters"),
                (pnlMenuPriceBranchFilter, "pnlMenuPriceBranchFilter"),
                (pnlMenuCategoryEditor, "pnlMenuCategoryEditor"), (pnlMenuProductEditor, "pnlMenuProductEditor"),
                (pnlMenuUnitEditor, "pnlMenuUnitEditor"), (pnlMenuPriceEditor, "pnlMenuPriceEditor"),
                (tlpMenuCategoryEditor, "tlpMenuCategoryEditor"), (tlpMenuProductEditor, "tlpMenuProductEditor"),
                (tlpMenuUnitEditor, "tlpMenuUnitEditor"), (tlpMenuPriceEditor, "tlpMenuPriceEditor"),
                (pnlMenuCategoryActions, "pnlMenuCategoryActions"), (pnlMenuProductActions, "pnlMenuProductActions"),
                (pnlMenuUnitActions, "pnlMenuUnitActions"), (pnlMenuPriceActions, "pnlMenuPriceActions"),
                (dgvMenuCategories, "dgvMenuCategories"), (dgvMenuProducts, "dgvMenuProducts"),
                (dgvMenuUnits, "dgvMenuUnits"), (dgvMenuPrices, "dgvMenuPrices"),
                (txtMenuCategorySearch, "txtMenuCategorySearch"), (txtMenuProductSearch, "txtMenuProductSearch"),
                (txtMenuUnitSearch, "txtMenuUnitSearch"), (txtMenuPriceSearch, "txtMenuPriceSearch"),
                (txtMenuCategoryName, "txtMenuCategoryName"), (txtMenuProductName, "txtMenuProductName"),
                (txtMenuProductSku, "txtMenuProductSku"), (cboMenuProductCategory, "cboMenuProductCategory"),
                (cboMenuProductUnit, "cboMenuProductUnit"), (cboMenuProductType, "cboMenuProductType"),
                (chkMenuProductRecipe, "chkMenuProductRecipe"), (chkMenuProductActive, "chkMenuProductActive"),
                (txtMenuUnitName, "txtMenuUnitName"), (cboMenuUnitType, "cboMenuUnitType"),
                (cboMenuPriceBranch, "cboMenuPriceBranch"), (cboMenuPriceProduct, "cboMenuPriceProduct"),
                (numMenuPrice, "numMenuPrice"), (chkMenuPriceAvailable, "chkMenuPriceAvailable"),
                (chkMenuCategoryActive, "chkMenuCategoryActive"),
                (lblMenuCategoryEditorTitle, "lblMenuCategoryEditorTitle"), (lblMenuProductEditorTitle, "lblMenuProductEditorTitle"),
                (lblMenuUnitEditorTitle, "lblMenuUnitEditorTitle"), (lblMenuPriceEditorTitle, "lblMenuPriceEditorTitle"),
                (lblMenuCategoryName, "lblMenuCategoryName"), (lblMenuProductName, "lblMenuProductName"),
                (lblMenuProductSku, "lblMenuProductSku"), (lblMenuProductCategory, "lblMenuProductCategory"),
                (lblMenuProductUnit, "lblMenuProductUnit"), (lblMenuProductType, "lblMenuProductType"),
                (lblMenuUnitName, "lblMenuUnitName"), (lblMenuUnitType, "lblMenuUnitType"),
                (lblMenuUnitHint, "lblMenuUnitHint"), (lblMenuPriceBranch, "lblMenuPriceBranch"),
                (lblMenuPriceProduct, "lblMenuPriceProduct"), (lblMenuPriceValue, "lblMenuPriceValue"),
                (lblMenuPriceHint, "lblMenuPriceHint"),
                (btnMenuCategoryAdd, "btnMenuCategoryAdd"), (btnMenuCategoryUpdate, "btnMenuCategoryUpdate"),
                (btnMenuCategoryToggle, "btnMenuCategoryToggle"), (btnMenuCategoryReset, "btnMenuCategoryReset"),
                (btnMenuProductAdd, "btnMenuProductAdd"), (btnMenuProductUpdate, "btnMenuProductUpdate"),
                (btnMenuProductDeactivate, "btnMenuProductDeactivate"), (btnMenuProductReset, "btnMenuProductReset"),
                (btnMenuUnitAdd, "btnMenuUnitAdd"), (btnMenuUnitUpdate, "btnMenuUnitUpdate"),
                (btnMenuUnitDelete, "btnMenuUnitDelete"), (btnMenuUnitReset, "btnMenuUnitReset"),
                (btnMenuPriceAdd, "btnMenuPriceAdd"), (btnMenuPriceUpdate, "btnMenuPriceUpdate"),
                (btnMenuPriceToggle, "btnMenuPriceToggle"), (btnMenuPriceReset, "btnMenuPriceReset"));

            ConfigureMenuSplit(splitMenuCategories, tabMenuCategories, tlpMenuCategoryList, pnlMenuCategoryEditor);
            ConfigureMenuSplit(splitMenuProducts, tabMenuProducts, tlpMenuProductList, pnlMenuProductEditor);
            ConfigureMenuSplit(splitMenuUnits, tabMenuUnits, tlpMenuUnitList, pnlMenuUnitEditor);
            ConfigureMenuSplit(splitMenuPrices, tabMenuPrices, tlpMenuPriceList, pnlMenuPriceEditor);

            ConfigureSearchPanel(pnlMenuCategorySearch, txtMenuCategorySearch, "Tìm tên danh mục...");
            ConfigureSearchPanel(pnlMenuProductSearch, txtMenuProductSearch, "Tìm tên món hoặc SKU...");
            ConfigureSearchPanel(pnlMenuUnitSearch, txtMenuUnitSearch, "Tìm tên đơn vị...");
            ConfigureMenuList(tlpMenuCategoryList, pnlMenuCategorySearch, dgvMenuCategories, 56);
            ConfigureMenuList(tlpMenuProductList, pnlMenuProductSearch, dgvMenuProducts, 56);
            ConfigureMenuList(tlpMenuUnitList, pnlMenuUnitSearch, dgvMenuUnits, 56);

            tlpMenuPriceFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            tlpMenuPriceFilters.ColumnCount = 1;
            tlpMenuPriceFilters.RowCount = 2;
            tlpMenuPriceFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMenuPriceFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            tlpMenuPriceFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tlpMenuPriceFilters.Controls.Add(pnlMenuPriceBranchFilter, 0, 0);
            tlpMenuPriceFilters.Controls.Add(txtMenuPriceSearch, 0, 1);
            pnlMenuPriceBranchFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            pnlMenuPriceBranchFilter.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            pnlMenuPriceBranchFilter.WrapContents = false;
            pnlMenuPriceBranchFilter.Padding = new System.Windows.Forms.Padding(10, 2, 8, 0);
            lblMenuPriceBranch.AutoSize = true;
            lblMenuPriceBranch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            lblMenuPriceBranch.ForeColor = System.Drawing.Color.FromArgb(34, 48, 62);
            lblMenuPriceBranch.Margin = new System.Windows.Forms.Padding(0, 6, 8, 0);
            lblMenuPriceBranch.Text = "Chi nhánh";
            cboMenuPriceBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMenuPriceBranch.Margin = new System.Windows.Forms.Padding(0, 1, 0, 0);
            cboMenuPriceBranch.Width = 220;
            pnlMenuPriceBranchFilter.Controls.Add(lblMenuPriceBranch);
            pnlMenuPriceBranchFilter.Controls.Add(cboMenuPriceBranch);
            txtMenuPriceSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            txtMenuPriceSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            txtMenuPriceSearch.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            txtMenuPriceSearch.MaxLength = 200;
            txtMenuPriceSearch.PlaceholderText = "Tìm món, SKU hoặc mã chi nhánh...";
            ConfigureMenuList(tlpMenuPriceList, tlpMenuPriceFilters, dgvMenuPrices, 82);

            ConfigureDataGrid(dgvMenuCategories, "dgvMenuCategories");
            ConfigureDataGrid(dgvMenuProducts, "dgvMenuProducts");
            ConfigureDataGrid(dgvMenuUnits, "dgvMenuUnits");
            ConfigureDataGrid(dgvMenuPrices, "dgvMenuPrices");

            ConfigureLabel(lblMenuCategoryEditorTitle, "THÔNG TIN DANH MỤC", true);
            ConfigureLabel(lblMenuProductEditorTitle, "THÔNG TIN SẢN PHẨM", true);
            ConfigureLabel(lblMenuUnitEditorTitle, "THÔNG TIN ĐƠN VỊ", true);
            ConfigureLabel(lblMenuPriceEditorTitle, "GIÁ BÁN TẠI CHI NHÁNH", true);
            ConfigureLabel(lblMenuCategoryName, "Tên danh mục", false);
            ConfigureLabel(lblMenuProductName, "Tên món", false);
            ConfigureLabel(lblMenuProductSku, "Mã SKU (không bắt buộc)", false);
            ConfigureLabel(lblMenuProductCategory, "Danh mục", false);
            ConfigureLabel(lblMenuProductUnit, "Đơn vị bán", false);
            ConfigureLabel(lblMenuProductType, "Loại sản phẩm", false);
            ConfigureLabel(lblMenuUnitName, "Tên đơn vị", false);
            ConfigureLabel(lblMenuUnitType, "Loại đơn vị", false);
            ConfigureLabel(lblMenuUnitHint, "Loại đơn vị theo dữ liệu chuẩn: KhoiLuong, TheTich hoặc Dem.", false);
            lblMenuUnitHint.ForeColor = System.Drawing.Color.FromArgb(105, 119, 132);
            lblMenuUnitHint.AutoSize = false;
            lblMenuUnitHint.Height = 48;
            lblMenuUnitHint.Dock = System.Windows.Forms.DockStyle.Fill;
            ConfigureLabel(lblMenuPriceProduct, "Sản phẩm", false);
            ConfigureLabel(lblMenuPriceValue, "Giá bán (VNĐ)", false);
            ConfigureLabel(lblMenuPriceHint, "Giá được lưu riêng cho từng chi nhánh. Chọn một chi nhánh để thêm hoặc cập nhật giá.", false);
            lblMenuPriceHint.ForeColor = System.Drawing.Color.FromArgb(105, 119, 132);
            lblMenuPriceHint.AutoSize = false;
            lblMenuPriceHint.Height = 54;
            lblMenuPriceHint.Dock = System.Windows.Forms.DockStyle.Fill;

            txtMenuCategoryName.MaxLength = 100;
            chkMenuCategoryActive.AutoSize = true;
            chkMenuCategoryActive.Checked = true;
            chkMenuCategoryActive.ForeColor = System.Drawing.Color.FromArgb(34, 48, 62);
            chkMenuCategoryActive.Text = "Danh mục đang hoạt động";
            txtMenuProductSku.MaxLength = 50;
            txtMenuProductName.MaxLength = 200;
            cboMenuProductCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMenuProductUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMenuProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            chkMenuProductRecipe.AutoSize = true;
            chkMenuProductRecipe.Checked = true;
            chkMenuProductRecipe.ForeColor = System.Drawing.Color.FromArgb(34, 48, 62);
            chkMenuProductRecipe.Text = "Bắt buộc có công thức";
            chkMenuProductActive.AutoSize = true;
            chkMenuProductActive.Checked = true;
            chkMenuProductActive.ForeColor = System.Drawing.Color.FromArgb(34, 48, 62);
            chkMenuProductActive.Text = "Đang kinh doanh";
            txtMenuUnitName.MaxLength = 50;
            cboMenuUnitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboMenuPriceProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            numMenuPrice.DecimalPlaces = 2;
            numMenuPrice.Maximum = 9999999999999999M;
            numMenuPrice.Minimum = 0M;
            numMenuPrice.Increment = 1000M;
            numMenuPrice.ThousandsSeparator = true;
            chkMenuPriceAvailable.AutoSize = true;
            chkMenuPriceAvailable.Checked = true;
            chkMenuPriceAvailable.ForeColor = System.Drawing.Color.FromArgb(34, 48, 62);
            chkMenuPriceAvailable.Text = "Đang bán tại chi nhánh";

            ConfigureMenuButton(btnMenuCategoryAdd, "＋  Thêm mới", System.Drawing.Color.FromArgb(42, 145, 103));
            ConfigureMenuButton(btnMenuCategoryUpdate, "Lưu cập nhật", System.Drawing.Color.FromArgb(54, 119, 177));
            ConfigureMenuButton(btnMenuCategoryToggle, "Ngừng / Mở lại", System.Drawing.Color.FromArgb(190, 72, 65));
            ConfigureMenuButton(btnMenuCategoryReset, "Làm mới form", System.Drawing.Color.FromArgb(111, 127, 141));
            ConfigureMenuButton(btnMenuProductAdd, "＋  Thêm món", System.Drawing.Color.FromArgb(42, 145, 103));
            ConfigureMenuButton(btnMenuProductUpdate, "Lưu cập nhật", System.Drawing.Color.FromArgb(54, 119, 177));
            ConfigureMenuButton(btnMenuProductDeactivate, "Ngừng kinh doanh", System.Drawing.Color.FromArgb(190, 72, 65));
            ConfigureMenuButton(btnMenuProductReset, "Làm mới form", System.Drawing.Color.FromArgb(111, 127, 141));
            ConfigureMenuButton(btnMenuUnitAdd, "＋  Thêm mới", System.Drawing.Color.FromArgb(42, 145, 103));
            ConfigureMenuButton(btnMenuUnitUpdate, "Lưu cập nhật", System.Drawing.Color.FromArgb(54, 119, 177));
            ConfigureMenuButton(btnMenuUnitDelete, "Xóa đơn vị", System.Drawing.Color.FromArgb(190, 72, 65));
            ConfigureMenuButton(btnMenuUnitReset, "Làm mới form", System.Drawing.Color.FromArgb(111, 127, 141));
            ConfigureMenuButton(btnMenuPriceAdd, "＋  Thêm / lưu giá", System.Drawing.Color.FromArgb(42, 145, 103));
            ConfigureMenuButton(btnMenuPriceUpdate, "Cập nhật giá", System.Drawing.Color.FromArgb(54, 119, 177));
            ConfigureMenuButton(btnMenuPriceToggle, "Ngừng / Mở bán", System.Drawing.Color.FromArgb(190, 72, 65));
            ConfigureMenuButton(btnMenuPriceReset, "Làm mới form", System.Drawing.Color.FromArgb(111, 127, 141));

            ConfigureActionPanel(pnlMenuCategoryActions, btnMenuCategoryAdd, btnMenuCategoryUpdate, btnMenuCategoryToggle, btnMenuCategoryReset);
            ConfigureActionPanel(pnlMenuProductActions, btnMenuProductAdd, btnMenuProductUpdate, btnMenuProductDeactivate, btnMenuProductReset);
            ConfigureActionPanel(pnlMenuUnitActions, btnMenuUnitAdd, btnMenuUnitUpdate, btnMenuUnitDelete, btnMenuUnitReset);
            ConfigureActionPanel(pnlMenuPriceActions, btnMenuPriceAdd, btnMenuPriceUpdate, btnMenuPriceToggle, btnMenuPriceReset);

            ConfigureEditor(pnlMenuCategoryEditor, tlpMenuCategoryEditor, lblMenuCategoryEditorTitle,
                CreateMenuFieldRow(lblMenuCategoryName, txtMenuCategoryName), chkMenuCategoryActive, pnlMenuCategoryActions);
            ConfigureEditor(pnlMenuProductEditor, tlpMenuProductEditor, lblMenuProductEditorTitle,
                CreateMenuFieldRow(lblMenuProductName, txtMenuProductName), CreateMenuFieldRow(lblMenuProductSku, txtMenuProductSku),
                CreateMenuFieldRow(lblMenuProductCategory, cboMenuProductCategory), CreateMenuFieldRow(lblMenuProductUnit, cboMenuProductUnit),
                CreateMenuFieldRow(lblMenuProductType, cboMenuProductType), chkMenuProductRecipe, chkMenuProductActive, pnlMenuProductActions);
            ConfigureEditor(pnlMenuUnitEditor, tlpMenuUnitEditor, lblMenuUnitEditorTitle,
                CreateMenuFieldRow(lblMenuUnitName, txtMenuUnitName), CreateMenuFieldRow(lblMenuUnitType, cboMenuUnitType),
                lblMenuUnitHint, pnlMenuUnitActions);
            ConfigureEditor(pnlMenuPriceEditor, tlpMenuPriceEditor, lblMenuPriceEditorTitle,
                CreateMenuFieldRow(lblMenuPriceProduct, cboMenuPriceProduct), CreateMenuFieldRow(lblMenuPriceValue, numMenuPrice),
                chkMenuPriceAvailable, lblMenuPriceHint, pnlMenuPriceActions);
        }

        private static void ConfigureMenuSplit(System.Windows.Forms.SplitContainer split, System.Windows.Forms.TabPage page,
            System.Windows.Forms.Control list, System.Windows.Forms.Control editor)
        {
            split.Dock = System.Windows.Forms.DockStyle.Fill;
            split.Size = new System.Drawing.Size(900, 600);
            split.IsSplitterFixed = false;
            split.Panel1MinSize = 390;
            split.Panel2MinSize = 330;
            split.SplitterDistance = 540;
            split.SplitterWidth = 8;
            split.BackColor = System.Drawing.Color.FromArgb(244, 247, 249);
            split.Panel1.Controls.Add(list);
            split.Panel2.Padding = new System.Windows.Forms.Padding(4, 0, 0, 0);
            split.Panel2.Controls.Add(editor);
            page.Controls.Add(split);
        }

        private static void ConfigureMenuList(System.Windows.Forms.TableLayoutPanel layout, System.Windows.Forms.Control search,
            System.Windows.Forms.DataGridView grid, float searchHeight)
        {
            layout.Dock = System.Windows.Forms.DockStyle.Fill;
            layout.ColumnCount = 1;
            layout.RowCount = 2;
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, searchHeight));
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.BackColor = System.Drawing.Color.White;
            layout.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            layout.Controls.Add(search, 0, 0);
            layout.Controls.Add(grid, 0, 1);
        }

        private static void ConfigureSearchPanel(System.Windows.Forms.Panel panel, System.Windows.Forms.TextBox search, string placeholder)
        {
            panel.BackColor = System.Drawing.Color.White;
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            panel.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            search.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            search.Dock = System.Windows.Forms.DockStyle.Fill;
            search.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            search.PlaceholderText = placeholder;
            panel.Controls.Add(search);
        }

        private static void ConfigureDataGrid(System.Windows.Forms.DataGridView grid, string name)
        {
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToResizeRows = false;
            grid.AutoGenerateColumns = false;
            grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            grid.BackgroundColor = System.Drawing.Color.White;
            grid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 40;
            grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.Dock = System.Windows.Forms.DockStyle.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            grid.GridColor = System.Drawing.Color.FromArgb(232, 237, 241);
            grid.MultiSelect = false;
            grid.Name = name;
            grid.ReadOnly = true;
            grid.RowHeadersVisible = false;
            grid.RowTemplate.Height = 35;
            grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            grid.ColumnHeadersDefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(239, 243, 246),
                ForeColor = System.Drawing.Color.FromArgb(54, 68, 81),
                Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold),
                SelectionBackColor = System.Drawing.Color.FromArgb(239, 243, 246),
                SelectionForeColor = System.Drawing.Color.FromArgb(54, 68, 81),
                Padding = new System.Windows.Forms.Padding(8, 0, 4, 0)
            };
            grid.DefaultCellStyle = new System.Windows.Forms.DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.FromArgb(45, 57, 69),
                SelectionBackColor = System.Drawing.Color.FromArgb(255, 237, 224),
                SelectionForeColor = System.Drawing.Color.FromArgb(45, 57, 69),
                Padding = new System.Windows.Forms.Padding(8, 0, 4, 0)
            };
            grid.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 252, 253);
        }

        private static void ConfigureLabel(System.Windows.Forms.Label label, string text, bool heading)
        {
            label.AutoSize = !heading;
            label.Dock = heading ? System.Windows.Forms.DockStyle.Fill : System.Windows.Forms.DockStyle.Top;
            label.Font = new System.Drawing.Font("Segoe UI", heading ? 10F : 8.8F,
                heading ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Bold);
            label.ForeColor = System.Drawing.Color.FromArgb(86, 101, 115);
            label.Margin = new System.Windows.Forms.Padding(0, 0, 0, heading ? 8 : 0);
            label.Text = text;
            label.TextAlign = heading ? System.Drawing.ContentAlignment.MiddleLeft : System.Drawing.ContentAlignment.MiddleLeft;
            if (heading)
                label.ForeColor = System.Drawing.Color.FromArgb(34, 48, 62);
        }

        private static System.Windows.Forms.Panel CreateMenuFieldRow(System.Windows.Forms.Label caption, System.Windows.Forms.Control input)
        {
            var row = new System.Windows.Forms.Panel
            {
                Dock = System.Windows.Forms.DockStyle.Fill,
                Height = 66,
                Margin = new System.Windows.Forms.Padding(0, 0, 0, 7)
            };
            caption.Height = 23;
            caption.Dock = System.Windows.Forms.DockStyle.Top;
            input.Dock = System.Windows.Forms.DockStyle.Fill;
            input.Margin = new System.Windows.Forms.Padding(0);
            row.Controls.Add(input);
            row.Controls.Add(caption);
            return row;
        }

        private static void ConfigureEditor(System.Windows.Forms.Panel panel, System.Windows.Forms.TableLayoutPanel layout,
            System.Windows.Forms.Label title, params System.Windows.Forms.Control[] rows)
        {
            panel.AutoScroll = true;
            panel.BackColor = System.Drawing.Color.White;
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            layout.AutoSize = true;
            layout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            layout.ColumnCount = 1;
            layout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            layout.Dock = System.Windows.Forms.DockStyle.Top;
            layout.Padding = new System.Windows.Forms.Padding(16, 8, 16, 14);
            layout.RowCount = rows.Length + 1;
            layout.RowStyles.Clear();
            layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            title.Height = 42;
            layout.Controls.Add(title, 0, 0);
            for (int i = 0; i < rows.Length; i++)
            {
                var row = rows[i];
                row.Dock = System.Windows.Forms.DockStyle.Fill;
                float rowHeight = row switch
                {
                    System.Windows.Forms.FlowLayoutPanel => 102F,
                    System.Windows.Forms.Panel => 73F,
                    System.Windows.Forms.CheckBox => 38F,
                    System.Windows.Forms.Label => 60F,
                    _ => 42F
                };
                layout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, rowHeight));
                if (row is System.Windows.Forms.CheckBox checkBox)
                {
                    checkBox.Margin = new System.Windows.Forms.Padding(0, 6, 0, 4);
                    checkBox.Font = new System.Drawing.Font("Segoe UI", 9.5F);
                }
                layout.Controls.Add(row, 0, i + 1);
            }
            panel.Controls.Add(layout);
        }

        private static void ConfigureActionPanel(System.Windows.Forms.FlowLayoutPanel panel, params System.Windows.Forms.Button[] buttons)
        {
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            panel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            panel.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            panel.WrapContents = true;
            foreach (var button in buttons)
                panel.Controls.Add(button);
        }

        private static void ConfigureMenuButton(System.Windows.Forms.Button button, string text, System.Drawing.Color color)
        {
            button.BackColor = color;
            button.Cursor = System.Windows.Forms.Cursors.Hand;
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(
                Math.Min(color.R + 14, 255), Math.Min(color.G + 14, 255), Math.Min(color.B + 14, 255));
            button.Font = new System.Drawing.Font("Segoe UI", 8.8F, System.Drawing.FontStyle.Bold);
            button.ForeColor = System.Drawing.Color.White;
            button.Margin = new System.Windows.Forms.Padding(0, 0, 8, 8);
            button.Size = new System.Drawing.Size(148, 39);
            button.Text = text;
            button.UseVisualStyleBackColor = false;
        }

        private static void SetMenuControlNames(params (System.Windows.Forms.Control Control, string Name)[] controls)
        {
            foreach (var (control, name) in controls)
                control.Name = name;
        }
    }
}
