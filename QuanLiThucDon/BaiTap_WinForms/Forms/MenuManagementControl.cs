using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using BaiTap_WinForms.Data;
using BaiTap_WinForms.Models;
using Microsoft.Data.SqlClient;

namespace BaiTap_WinForms.Forms
{
    public partial class MenuManagementControl : UserControl
    {
        private readonly CultureInfo _vnCulture = new CultureInfo("vi-VN");
        private readonly Color _menuMuted = Color.FromArgb(105, 119, 132);
        private readonly Color _menuOrange = Color.FromArgb(226, 112, 45);
        private static readonly Font _menuStatusFont = new("Segoe UI", 9F, FontStyle.Bold);
        private readonly System.Windows.Forms.Timer _menuSearchTimer = new() { Interval = 280 };
        private Action? _pendingMenuSearch;

        private int _selectedMenuCategoryId;
        private bool _selectedMenuCategoryActive;

        private int _selectedMenuProductId;

        private int _selectedMenuUnitId;

        private int _selectedMenuPriceProductId;
        private int _selectedMenuPriceBranchId;
        private bool _selectedMenuPriceAvailable;
        private bool _loadingMenuBranchOptions;

        public MenuManagementControl()
        {
            InitializeComponent();
            InitializeMenuPageDesign();
            InitializeMenuPages();
        }

        private void InitializeMenuPages()
        {
            components ??= new System.ComponentModel.Container();
            components.Add(_menuSearchTimer);
            _menuSearchTimer.Tick += (s, e) =>
            {
                _menuSearchTimer.Stop();
                _pendingMenuSearch?.Invoke();
            };

            btnMenuCategories.Click += (s, e) => SelectMenuSection(0);
            btnMenuProducts.Click += (s, e) => SelectMenuSection(1);
            btnMenuUnits.Click += (s, e) => SelectMenuSection(2);
            btnMenuPrices.Click += (s, e) => SelectMenuSection(3);
            tabMenuSections.SelectedIndexChanged += (s, e) => UpdateMenuNavigationStyle();

            InitializeMenuCategoriesPage();
            InitializeMenuProductsPage();
            InitializeMenuUnitsPage();
            InitializeMenuPricesPage();
            ConfigureResponsiveSplit(splitMenuCategories);
            ConfigureResponsiveSplit(splitMenuProducts);
            ConfigureResponsiveSplit(splitMenuUnits);
            ConfigureResponsiveSplit(splitMenuPrices);
            UpdateMenuNavigationStyle();
        }

        private static void ConfigureResponsiveSplit(SplitContainer split)
        {
            split.SizeChanged += (s, e) =>
            {
                int maximum = split.ClientSize.Width - split.Panel2MinSize - split.SplitterWidth;
                if (maximum < split.Panel1MinSize) return;
                int target = Math.Clamp((int)(split.ClientSize.Width * 0.61), split.Panel1MinSize, maximum);
                if (split.SplitterDistance != target)
                    split.SplitterDistance = target;
            };
        }

        private void SelectMenuSection(int index)
        {
            tabMenuSections.SelectedIndex = index;
            UpdateMenuNavigationStyle();
        }

        private void UpdateMenuNavigationStyle()
        {
            if (tabMenuSections == null) return;
            var buttons = new[] { btnMenuCategories, btnMenuProducts, btnMenuUnits, btnMenuPrices };
            for (int i = 0; i < buttons.Length; i++)
            {
                bool selected = i == tabMenuSections.SelectedIndex;
                buttons[i].BackColor = selected ? _menuOrange : Color.White;
                buttons[i].ForeColor = selected ? Color.White : _menuMuted;
                buttons[i].FlatAppearance.BorderColor = selected ? _menuOrange : Color.FromArgb(224, 231, 236);
            }
        }

        private void ConfigureMenuGrid(DataGridView grid)
        {
            grid.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0 || grid.Columns[e.ColumnIndex].Name != "TrangThai" || e.Value == null) return;
                bool isInactive = Convert.ToString(e.Value, CultureInfo.CurrentCulture)?.Contains("ngừng", StringComparison.OrdinalIgnoreCase) == true
                    || Convert.ToString(e.Value, CultureInfo.CurrentCulture)?.Contains("tạm", StringComparison.OrdinalIgnoreCase) == true;
                e.CellStyle.ForeColor = isInactive ? Color.FromArgb(166, 69, 63) : Color.FromArgb(42, 125, 88);
                e.CellStyle.Font = _menuStatusFont;
            };
        }

        private static void AddMenuTextColumn(DataGridView grid, string property, string title, float fillWeight = 100)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = property,
                DataPropertyName = property,
                HeaderText = title,
                FillWeight = fillWeight,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
        }

        private static void AddMenuCheckColumn(DataGridView grid, string property, string title, float fillWeight = 28)
        {
            grid.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = property,
                DataPropertyName = property,
                HeaderText = title,
                FillWeight = fillWeight,
                MinimumWidth = 72,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
        }

        private static void AddMenuStatusColumn(DataGridView grid, float fillWeight = 32)
        {
            grid.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                DataPropertyName = "TrangThai",
                HeaderText = "Trạng thái",
                FillWeight = fillWeight,
                MinimumWidth = 90,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.Automatic
            });
        }

        private static void SetMenuGridDataSource(DataGridView grid, DataTable data)
        {
            grid.DataSource = data;
            grid.ClearSelection();
            if (grid.Rows.Count > 0)
                grid.CurrentCell = null;
        }

        private void ScheduleMenuSearch(Action load, Action clearSelection)
        {
            clearSelection();
            _pendingMenuSearch = load;
            _menuSearchTimer.Stop();
            _menuSearchTimer.Start();
        }

        private void InitializeMenuCategoriesPage()
        {
            ConfigureMenuGrid(dgvMenuCategories);
            dgvMenuCategories.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaDanhMuc", DataPropertyName = "MaDanhMuc", Visible = false });
            dgvMenuCategories.Columns.Add(new DataGridViewTextBoxColumn { Name = "DangHoatDong", DataPropertyName = "DangHoatDong", Visible = false });
            AddMenuTextColumn(dgvMenuCategories, "TenDanhMuc", "Tên danh mục", 72);
            AddMenuStatusColumn(dgvMenuCategories, 28);

            txtMenuCategorySearch.MaxLength = 100;
            txtMenuCategorySearch.TextChanged += (s, e) => ScheduleMenuSearch(LoadMenuCategoryPage, () => ResetMenuCategoryForm(false));
            dgvMenuCategories.CellClick += (s, e) => { if (e.RowIndex >= 0) BindMenuCategory(); };
            btnMenuCategoryAdd.Click += (s, e) => SaveMenuCategory(true);
            btnMenuCategoryUpdate.Click += (s, e) => SaveMenuCategory(false);
            btnMenuCategoryToggle.Click += (s, e) => ToggleMenuCategory();
            btnMenuCategoryReset.Click += (s, e) => ResetMenuCategoryForm();
        }
        private void InitializeMenuProductsPage()
        {
            ConfigureMenuGrid(dgvMenuProducts);
            dgvMenuProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaSanPham", DataPropertyName = "MaSanPham", Visible = false });
            dgvMenuProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaDanhMuc", DataPropertyName = "MaDanhMuc", Visible = false });
            dgvMenuProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaDonViBan", DataPropertyName = "MaDonViBan", Visible = false });
            dgvMenuProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "BatBuocCongThuc", DataPropertyName = "BatBuocCongThuc", Visible = false });
            dgvMenuProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "DangHoatDong", DataPropertyName = "DangHoatDong", Visible = false });
            dgvMenuProducts.Columns.Add(new DataGridViewTextBoxColumn { Name = "LoaiSanPham", DataPropertyName = "LoaiSanPham", Visible = false });
            AddMenuTextColumn(dgvMenuProducts, "MaSKU", "SKU", 28);
            AddMenuTextColumn(dgvMenuProducts, "TenSanPham", "Tên món", 78);
            AddMenuTextColumn(dgvMenuProducts, "TenDanhMuc", "Danh mục", 55);
            AddMenuTextColumn(dgvMenuProducts, "TenDonVi", "ĐVT", 22);
            AddMenuTextColumn(dgvMenuProducts, "LoaiHienThi", "Loại", 40);
            AddMenuStatusColumn(dgvMenuProducts, 31);

            txtMenuProductSearch.MaxLength = 200;
            txtMenuProductSearch.TextChanged += (s, e) => ScheduleMenuSearch(LoadMenuProductPage, () => ResetMenuProductForm(false));
            dgvMenuProducts.CellClick += (s, e) => { if (e.RowIndex >= 0) BindMenuProduct(); };
            cboMenuProductType.SelectedValueChanged += (s, e) => UpdateMenuRecipeControl();
            btnMenuProductAdd.Click += (s, e) => SaveMenuProduct(true);
            btnMenuProductUpdate.Click += (s, e) => SaveMenuProduct(false);
            btnMenuProductDeactivate.Click += (s, e) => DeactivateMenuProduct();
            btnMenuProductReset.Click += (s, e) => ResetMenuProductForm();
        }
        private void InitializeMenuUnitsPage()
        {
            ConfigureMenuGrid(dgvMenuUnits);
            dgvMenuUnits.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaDonVi", DataPropertyName = "MaDonVi", Visible = false });
            dgvMenuUnits.Columns.Add(new DataGridViewTextBoxColumn { Name = "LoaiDonVi", DataPropertyName = "LoaiDonVi", Visible = false });
            AddMenuTextColumn(dgvMenuUnits, "TenDonVi", "Tên đơn vị", 70);
            AddMenuTextColumn(dgvMenuUnits, "LoaiDonViHienThi", "Nhóm đơn vị", 45);
            AddMenuTextColumn(dgvMenuUnits, "SoSanPham", "Sản phẩm dùng", 35);
            AddMenuTextColumn(dgvMenuUnits, "SoNguyenLieu", "Nguyên liệu dùng", 40);

            txtMenuUnitSearch.MaxLength = 50;
            cboMenuUnitType.Items.AddRange(new object[] { "KhoiLuong", "TheTich", "Dem" });
            dgvMenuUnits.CellClick += (s, e) => { if (e.RowIndex >= 0) BindMenuUnit(); };
            txtMenuUnitSearch.TextChanged += (s, e) => ScheduleMenuSearch(LoadMenuUnitPage, () => ResetMenuUnitForm(false));
            btnMenuUnitAdd.Click += (s, e) => SaveMenuUnit(true);
            btnMenuUnitUpdate.Click += (s, e) => SaveMenuUnit(false);
            btnMenuUnitDelete.Click += (s, e) => DeleteMenuUnit();
            btnMenuUnitReset.Click += (s, e) => ResetMenuUnitForm();
        }
        private void InitializeMenuPricesPage()
        {
            ConfigureMenuGrid(dgvMenuPrices);
            dgvMenuPrices.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaChiNhanh", DataPropertyName = "MaChiNhanh", Visible = false });
            dgvMenuPrices.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaSanPham", DataPropertyName = "MaSanPham", Visible = false });
            dgvMenuPrices.Columns.Add(new DataGridViewCheckBoxColumn { Name = "ConBan", DataPropertyName = "ConBan", Visible = false });
            AddMenuTextColumn(dgvMenuPrices, "MaCodeChiNhanh", "Mã CN", 25);
            AddMenuTextColumn(dgvMenuPrices, "TenChiNhanh", "Chi nhánh", 48);
            AddMenuTextColumn(dgvMenuPrices, "MaSKU", "SKU", 27);
            AddMenuTextColumn(dgvMenuPrices, "TenSanPham", "Tên món", 80);
            AddMenuTextColumn(dgvMenuPrices, "TenDonVi", "ĐVT", 20);
            AddMenuTextColumn(dgvMenuPrices, "GiaBan", "Giá bán", 42);
            dgvMenuPrices.Columns["GiaBan"]!.DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N0 'đ'",
                FormatProvider = _vnCulture,
                Alignment = DataGridViewContentAlignment.MiddleRight,
                Padding = new Padding(4, 0, 8, 0)
            };
            AddMenuStatusColumn(dgvMenuPrices, 29);

            cboMenuPriceBranch.SelectedValueChanged += (s, e) =>
            {
                if (!_loadingMenuBranchOptions)
                {
                    ResetMenuPriceForm();
                    LoadMenuPricePage();
                }
                UpdateMenuPriceEditorState();
            };
            txtMenuPriceSearch.TextChanged += (s, e) => ScheduleMenuSearch(LoadMenuPricePage, ResetMenuPriceForm);
            dgvMenuPrices.CellClick += (s, e) => { if (e.RowIndex >= 0) BindMenuPrice(); };
            btnMenuPriceAdd.Click += (s, e) => SaveMenuPrice(updateSelected: false);
            btnMenuPriceUpdate.Click += (s, e) => SaveMenuPrice(updateSelected: true);
            btnMenuPriceToggle.Click += (s, e) => ToggleMenuPriceAvailability();
            btnMenuPriceReset.Click += (s, e) => ResetMenuPriceForm();
        }
        public void LoadMenuData()
        {
            LoadMenuBranchOptions();
            LoadMenuCategoryPage();
            LoadMenuUnitPage();
            LoadMenuProductLookups();
            LoadMenuProductPage();
            LoadMenuPriceProductLookup();
            LoadMenuPricePage();
            UpdateMenuPriceEditorState();
            UpdateMenuBranchBadge();
        }

        private void LoadMenuBranchOptions()
        {
            try
            {
                if (!UserSession.IsAdmin && UserSession.MaChiNhanh <= 0)
                {
                    MessageBox.Show("Không xác định được chi nhánh của tài khoản. Vui lòng đăng nhập lại hoặc liên hệ quản trị viên.", "Thiếu thông tin chi nhánh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = @"
                    SELECT MaChiNhanh, MaCode, TenChiNhanh
                    FROM dbo.ChiNhanh
                    WHERE DangHoatDong = 1 AND (@IsAdmin = 1 OR MaChiNhanh = @MaChiNhanh)
                    ORDER BY MaCode";
                var parameters = new[]
                {
                    new SqlParameter("@IsAdmin", SqlDbType.Bit) { Value = UserSession.IsAdmin },
                    new SqlParameter("@MaChiNhanh", SqlDbType.Int) { Value = UserSession.MaChiNhanh }
                };
                DataTable branches = DatabaseHelper.ExecuteQuery(query, parameters);
                var options = new DataTable();
                options.Columns.Add("MaChiNhanh", typeof(int));
                options.Columns.Add("TenHienThi", typeof(string));
                if (UserSession.IsAdmin)
                    options.Rows.Add(-1, "Tất cả chi nhánh — chỉ xem");
                foreach (DataRow row in branches.Rows)
                    options.Rows.Add(Convert.ToInt32(row["MaChiNhanh"]), $"{row["MaCode"]} · {row["TenChiNhanh"]}");

                _loadingMenuBranchOptions = true;
                cboMenuPriceBranch.DataSource = options;
                cboMenuPriceBranch.DisplayMember = "TenHienThi";
                cboMenuPriceBranch.ValueMember = "MaChiNhanh";
                if (UserSession.IsAdmin)
                    cboMenuPriceBranch.SelectedValue = -1;
                else
                    cboMenuPriceBranch.SelectedValue = UserSession.MaChiNhanh;
                cboMenuPriceBranch.Enabled = UserSession.IsAdmin;
                _loadingMenuBranchOptions = false;
            }
            catch (Exception ex)
            {
                _loadingMenuBranchOptions = false;
                MessageBox.Show("Lỗi tải danh sách chi nhánh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateMenuBranchBadge()
        {
            if (lblMenuBranchBadge == null) return;
            if (UserSession.IsAdmin)
                lblMenuBranchBadge.Text = "QUẢN TRỊ TOÀN HỆ THỐNG";
            else if (cboMenuPriceBranch.SelectedItem is DataRowView branch)
                lblMenuBranchBadge.Text = $"CHI NHÁNH {branch["TenHienThi"]}";
            else
                lblMenuBranchBadge.Text = "CHI NHÁNH CỦA TÀI KHOẢN";
        }

        private void LoadMenuCategoryPage()
        {
            try
            {
                const string query = @"
                    SELECT MaDanhMuc, TenDanhMuc, DangHoatDong,
                           CASE WHEN DangHoatDong = 1 THEN N'Đang dùng' ELSE N'Ngừng dùng' END AS TrangThai
                    FROM dbo.DanhMuc
                    WHERE @TuKhoa = N'' OR TenDanhMuc LIKE N'%' + @TuKhoa + N'%'
                    ORDER BY DangHoatDong DESC, TenDanhMuc";
                DataTable table = DatabaseHelper.ExecuteQuery(query, new[]
                {
                    new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 100) { Value = txtMenuCategorySearch.Text.Trim() }
                });
                SetMenuGridDataSource(dgvMenuCategories, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMenuUnitPage()
        {
            try
            {
                const string query = @"
                    SELECT dv.MaDonVi, dv.TenDonVi, dv.LoaiDonVi,
                           CASE dv.LoaiDonVi
                               WHEN N'KhoiLuong' THEN N'Khối lượng'
                               WHEN N'TheTich' THEN N'Thể tích'
                               WHEN N'Dem' THEN N'Đếm'
                               ELSE dv.LoaiDonVi END AS LoaiDonViHienThi,
                           (SELECT COUNT(*) FROM dbo.SanPham sp WHERE sp.MaDonViBan = dv.MaDonVi) AS SoSanPham,
                           (SELECT COUNT(*) FROM dbo.NguyenLieu nl WHERE nl.MaDonViGoc = dv.MaDonVi) AS SoNguyenLieu
                    FROM dbo.DonVi dv
                    WHERE @TuKhoa = N'' OR dv.TenDonVi LIKE N'%' + @TuKhoa + N'%'
                    ORDER BY dv.LoaiDonVi, dv.TenDonVi";
                DataTable table = DatabaseHelper.ExecuteQuery(query, new[]
                {
                    new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 50) { Value = txtMenuUnitSearch.Text.Trim() }
                });
                SetMenuGridDataSource(dgvMenuUnits, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải đơn vị tính: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMenuProductLookups()
        {
            try
            {
                DataTable categories = DatabaseHelper.ExecuteQuery("SELECT MaDanhMuc, TenDanhMuc FROM dbo.DanhMuc ORDER BY TenDanhMuc");
                cboMenuProductCategory.DataSource = categories;
                cboMenuProductCategory.DisplayMember = "TenDanhMuc";
                cboMenuProductCategory.ValueMember = "MaDanhMuc";

                DataTable units = DatabaseHelper.ExecuteQuery("SELECT MaDonVi, TenDonVi FROM dbo.DonVi ORDER BY TenDonVi");
                cboMenuProductUnit.DataSource = units;
                cboMenuProductUnit.DisplayMember = "TenDonVi";
                cboMenuProductUnit.ValueMember = "MaDonVi";

                var productTypes = new DataTable();
                productTypes.Columns.Add("LoaiSanPham", typeof(string));
                productTypes.Columns.Add("TenHienThi", typeof(string));
                productTypes.Rows.Add("ThanhPham", "Thành phẩm");
                productTypes.Rows.Add("KhongTruKho", "Không trừ kho");
                productTypes.Rows.Add("Combo", "Combo");
                cboMenuProductType.DataSource = productTypes;
                cboMenuProductType.DisplayMember = "TenHienThi";
                cboMenuProductType.ValueMember = "LoaiSanPham";
                UpdateMenuRecipeControl();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục hoặc đơn vị cho sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMenuProductPage()
        {
            try
            {
                string query = @"
                    SELECT sp.MaSanPham, sp.MaDanhMuc, sp.MaDonViBan, sp.TenSanPham,
                           COALESCE(sp.MaSKU, N'') AS MaSKU, sp.LoaiSanPham, sp.BatBuocCongThuc,
                           sp.DangHoatDong, dm.TenDanhMuc, dv.TenDonVi,
                           CASE sp.LoaiSanPham
                               WHEN N'ThanhPham' THEN N'Thành phẩm'
                               WHEN N'KhongTruKho' THEN N'Không trừ kho'
                               WHEN N'Combo' THEN N'Combo'
                               ELSE sp.LoaiSanPham END AS LoaiHienThi,
                           CASE WHEN sp.DangHoatDong = 1 THEN N'Đang kinh doanh' ELSE N'Ngừng kinh doanh' END AS TrangThai
                    FROM dbo.SanPham sp
                    JOIN dbo.DanhMuc dm ON dm.MaDanhMuc = sp.MaDanhMuc
                    JOIN dbo.DonVi dv ON dv.MaDonVi = sp.MaDonViBan
                    WHERE (@IsAdmin = 1 OR EXISTS (
                              SELECT 1 FROM dbo.SanPham_ChiNhanh spcn
                              WHERE spcn.MaSanPham = sp.MaSanPham AND spcn.MaChiNhanh = @MaChiNhanh))
                      AND (@TuKhoa = N'' OR sp.TenSanPham LIKE N'%' + @TuKhoa + N'%' OR sp.MaSKU LIKE N'%' + @TuKhoa + N'%')
                    ORDER BY sp.DangHoatDong DESC, dm.TenDanhMuc, sp.TenSanPham";
                var parameters = new[]
                {
                    new SqlParameter("@IsAdmin", SqlDbType.Bit) { Value = UserSession.IsAdmin },
                    new SqlParameter("@MaChiNhanh", SqlDbType.Int) { Value = UserSession.MaChiNhanh },
                    new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = txtMenuProductSearch.Text.Trim() }
                };
                SetMenuGridDataSource(dgvMenuProducts, DatabaseHelper.ExecuteQuery(query, parameters));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMenuPriceProductLookup()
        {
            try
            {
                const string query = @"
                    SELECT sp.MaSanPham,
                    CASE WHEN sp.MaSKU IS NULL OR sp.MaSKU = N'' THEN sp.TenSanPham
                                ELSE sp.MaSKU + N' · ' + sp.TenSanPham END
                           + CASE WHEN sp.DangHoatDong = 0 THEN N' [Ngừng kinh doanh]' ELSE N'' END AS TenHienThi
                    FROM dbo.SanPham sp
                    WHERE @IsAdmin = 1 OR EXISTS (
                        SELECT 1 FROM dbo.SanPham_ChiNhanh spcn
                        WHERE spcn.MaSanPham = sp.MaSanPham AND spcn.MaChiNhanh = @MaChiNhanh)
                    ORDER BY sp.TenSanPham";
                DataTable products = DatabaseHelper.ExecuteQuery(query, new[]
                {
                    new SqlParameter("@IsAdmin", SqlDbType.Bit) { Value = UserSession.IsAdmin },
                    new SqlParameter("@MaChiNhanh", SqlDbType.Int) { Value = UserSession.MaChiNhanh }
                });
                cboMenuPriceProduct.DataSource = products;
                cboMenuPriceProduct.DisplayMember = "TenHienThi";
                cboMenuPriceProduct.ValueMember = "MaSanPham";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sản phẩm để thiết lập giá: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMenuPricePage()
        {
            try
            {
                int selectedBranchId = ReadBoundComboInteger(cboMenuPriceBranch, "MaChiNhanh", int.MinValue);
                if (selectedBranchId == int.MinValue) return;
                int branchId = UserSession.IsAdmin ? selectedBranchId : UserSession.MaChiNhanh;
                const string query = @"
                    SELECT spcn.MaChiNhanh, spcn.MaSanPham, cn.MaCode AS MaCodeChiNhanh, cn.TenChiNhanh,
                           COALESCE(sp.MaSKU, N'') AS MaSKU, sp.TenSanPham, dv.TenDonVi,
                           spcn.GiaBan, spcn.ConBan,
                           CASE WHEN spcn.ConBan = 1 THEN N'Đang bán' ELSE N'Ngừng bán' END AS TrangThai
                    FROM dbo.SanPham_ChiNhanh spcn
                    JOIN dbo.ChiNhanh cn ON cn.MaChiNhanh = spcn.MaChiNhanh
                    JOIN dbo.SanPham sp ON sp.MaSanPham = spcn.MaSanPham
                    JOIN dbo.DonVi dv ON dv.MaDonVi = sp.MaDonViBan
                    WHERE (@MaChiNhanh = -1 OR spcn.MaChiNhanh = @MaChiNhanh)
                      AND (@TuKhoa = N'' OR sp.TenSanPham LIKE N'%' + @TuKhoa + N'%'
                           OR sp.MaSKU LIKE N'%' + @TuKhoa + N'%' OR cn.MaCode LIKE N'%' + @TuKhoa + N'%')
                    ORDER BY cn.MaCode, spcn.ConBan DESC, sp.TenSanPham";
                var parameters = new[]
                {
                    new SqlParameter("@MaChiNhanh", SqlDbType.Int) { Value = branchId },
                    new SqlParameter("@TuKhoa", SqlDbType.NVarChar, 200) { Value = txtMenuPriceSearch.Text.Trim() }
                };
                SetMenuGridDataSource(dgvMenuPrices, DatabaseHelper.ExecuteQuery(query, parameters));
                UpdateMenuPriceEditorState();
                UpdateMenuBranchBadge();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải bảng giá chi nhánh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateMenuPriceEditorState()
        {
            if (numMenuPrice == null || cboMenuPriceBranch == null) return;
            int branchId = ReadBoundComboInteger(cboMenuPriceBranch, "MaChiNhanh", int.MinValue);
            bool branchSelected = branchId != int.MinValue;
            bool canEdit = branchSelected && branchId > 0;
            cboMenuPriceProduct.Enabled = canEdit && _selectedMenuPriceProductId == 0;
            numMenuPrice.Enabled = canEdit;
            chkMenuPriceAvailable.Enabled = canEdit;
        }

        private static int ReadBoundComboInteger(ComboBox combo, string valueColumn, int fallback)
        {
            object? value = ReadBoundComboValue(combo, valueColumn);
            return int.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), out int result)
                ? result
                : fallback;
        }

        private static string ReadBoundComboText(ComboBox combo, string valueColumn, string fallback = "")
        {
            object? value = ReadBoundComboValue(combo, valueColumn);
            return value == null || value == DBNull.Value
                ? fallback
                : Convert.ToString(value, CultureInfo.InvariantCulture) ?? fallback;
        }

        private static object? ReadBoundComboValue(ComboBox combo, string valueColumn)
        {
            object? value = combo.SelectedValue;
            if (value is DataRowView selectedValueRow)
                value = selectedValueRow.Row.Table.Columns.Contains(valueColumn)
                    ? selectedValueRow[valueColumn]
                    : null;
            else if (value is DataRow dataRow)
                value = dataRow.Table.Columns.Contains(valueColumn) ? dataRow[valueColumn] : null;

            if ((value == null || value == DBNull.Value) && combo.SelectedItem is DataRowView selectedItemRow)
                value = selectedItemRow.Row.Table.Columns.Contains(valueColumn)
                    ? selectedItemRow[valueColumn]
                    : null;

            return value;
        }

        private void BindMenuCategory()
        {
            if (dgvMenuCategories.CurrentRow == null || dgvMenuCategories.CurrentRow.Index < 0) return;
            var row = dgvMenuCategories.CurrentRow;
            _selectedMenuCategoryId = Convert.ToInt32(row.Cells["MaDanhMuc"].Value);
            _selectedMenuCategoryActive = Convert.ToBoolean(row.Cells["DangHoatDong"].Value);
            txtMenuCategoryName.Text = Convert.ToString(row.Cells["TenDanhMuc"].Value) ?? string.Empty;
            chkMenuCategoryActive.Checked = Convert.ToBoolean(row.Cells["DangHoatDong"].Value);
        }

        private void ResetMenuCategoryForm(bool focus = true)
        {
            _selectedMenuCategoryId = 0;
            _selectedMenuCategoryActive = false;
            txtMenuCategoryName.Clear();
            chkMenuCategoryActive.Checked = true;
            if (focus) txtMenuCategoryName.Focus();
        }

        private void SaveMenuCategory(bool isNew)
        {
            string name = txtMenuCategoryName.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên danh mục.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMenuCategoryName.Focus();
                return;
            }
            if (!isNew && _selectedMenuCategoryId == 0)
            {
                MessageBox.Show("Hãy chọn danh mục cần cập nhật trong danh sách.", "Chưa chọn danh mục", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = isNew
                    ? "INSERT INTO dbo.DanhMuc (TenDanhMuc, DangHoatDong) VALUES (@TenDanhMuc, @DangHoatDong)"
                    : "UPDATE dbo.DanhMuc SET TenDanhMuc = @TenDanhMuc, DangHoatDong = @DangHoatDong WHERE MaDanhMuc = @MaDanhMuc";
                var parameters = new[]
                {
                    new SqlParameter("@TenDanhMuc", SqlDbType.NVarChar, 100) { Value = name },
                    new SqlParameter("@DangHoatDong", SqlDbType.Bit) { Value = chkMenuCategoryActive.Checked },
                    new SqlParameter("@MaDanhMuc", SqlDbType.Int) { Value = _selectedMenuCategoryId }
                };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show(isNew ? "Đã thêm danh mục món." : "Đã cập nhật danh mục món.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMenuCategoryPage();
                LoadMenuProductLookups();
                ResetMenuCategoryForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu danh mục món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleMenuCategory()
        {
            if (_selectedMenuCategoryId == 0)
            {
                MessageBox.Show("Hãy chọn danh mục cần thay đổi trạng thái.", "Chưa chọn danh mục", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE dbo.DanhMuc SET DangHoatDong = @DangHoatDong WHERE MaDanhMuc = @MaDanhMuc",
                    new[]
                    {
                        new SqlParameter("@DangHoatDong", SqlDbType.Bit) { Value = !_selectedMenuCategoryActive },
                        new SqlParameter("@MaDanhMuc", SqlDbType.Int) { Value = _selectedMenuCategoryId }
                    });
                LoadMenuCategoryPage();
                LoadMenuProductLookups();
                ResetMenuCategoryForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thay đổi trạng thái danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindMenuProduct()
        {
            if (dgvMenuProducts.CurrentRow == null || dgvMenuProducts.CurrentRow.Index < 0) return;
            var row = dgvMenuProducts.CurrentRow;
            _selectedMenuProductId = Convert.ToInt32(row.Cells["MaSanPham"].Value);
            txtMenuProductSku.Text = Convert.ToString(row.Cells["MaSKU"].Value) ?? string.Empty;
            txtMenuProductName.Text = Convert.ToString(row.Cells["TenSanPham"].Value) ?? string.Empty;
            cboMenuProductCategory.SelectedValue = Convert.ToInt32(row.Cells["MaDanhMuc"].Value);
            cboMenuProductUnit.SelectedValue = Convert.ToInt32(row.Cells["MaDonViBan"].Value);
            cboMenuProductType.SelectedValue = Convert.ToString(row.Cells["LoaiSanPham"].Value) ?? "ThanhPham";
            chkMenuProductRecipe.Checked = Convert.ToBoolean(row.Cells["BatBuocCongThuc"].Value);
            chkMenuProductActive.Checked = Convert.ToBoolean(row.Cells["DangHoatDong"].Value);
            UpdateMenuRecipeControl();
        }

        private void UpdateMenuRecipeControl()
        {
            if (chkMenuProductRecipe == null || cboMenuProductType == null) return;
            bool isCombo = string.Equals(ReadBoundComboText(cboMenuProductType, "LoaiSanPham"), "Combo", StringComparison.Ordinal);
            if (isCombo) chkMenuProductRecipe.Checked = false;
            chkMenuProductRecipe.Enabled = !isCombo;
        }

        private void ResetMenuProductForm(bool focus = true)
        {
            _selectedMenuProductId = 0;
            txtMenuProductSku.Clear();
            txtMenuProductName.Clear();
            if (cboMenuProductCategory.Items.Count > 0) cboMenuProductCategory.SelectedIndex = 0;
            if (cboMenuProductUnit.Items.Count > 0) cboMenuProductUnit.SelectedIndex = 0;
            cboMenuProductType.SelectedValue = "ThanhPham";
            chkMenuProductRecipe.Checked = true;
            chkMenuProductActive.Checked = true;
            UpdateMenuRecipeControl();
            if (focus) txtMenuProductName.Focus();
        }

        private void SaveMenuProduct(bool isNew)
        {
            string name = txtMenuProductName.Text.Trim();
            string sku = txtMenuProductSku.Text.Trim();
            if (name.Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMenuProductName.Focus();
                return;
            }
            int categoryId = ReadBoundComboInteger(cboMenuProductCategory, "MaDanhMuc", 0);
            int unitId = ReadBoundComboInteger(cboMenuProductUnit, "MaDonVi", 0);
            string productType = ReadBoundComboText(cboMenuProductType, "LoaiSanPham");
            if (categoryId <= 0 || unitId <= 0 || string.IsNullOrWhiteSpace(productType))
            {
                MessageBox.Show("Cần có ít nhất một danh mục, một đơn vị và một loại sản phẩm trước khi lưu.", "Thiếu dữ liệu danh mục", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!isNew && _selectedMenuProductId == 0)
            {
                MessageBox.Show("Hãy chọn sản phẩm cần cập nhật trong danh sách.", "Chưa chọn sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = isNew
                    ? @"INSERT INTO dbo.SanPham (MaDanhMuc, MaDonViBan, TenSanPham, MaSKU, LoaiSanPham, BatBuocCongThuc, DangHoatDong)
                       VALUES (@MaDanhMuc, @MaDonViBan, @TenSanPham, @MaSKU, @LoaiSanPham, @BatBuocCongThuc, @DangHoatDong)"
                    : @"UPDATE dbo.SanPham
                       SET MaDanhMuc = @MaDanhMuc, MaDonViBan = @MaDonViBan, TenSanPham = @TenSanPham,
                           MaSKU = @MaSKU, LoaiSanPham = @LoaiSanPham, BatBuocCongThuc = @BatBuocCongThuc,
                           DangHoatDong = @DangHoatDong
                       WHERE MaSanPham = @MaSanPham";
                var parameters = new[]
                {
                    new SqlParameter("@MaDanhMuc", SqlDbType.Int) { Value = categoryId },
                    new SqlParameter("@MaDonViBan", SqlDbType.Int) { Value = unitId },
                    new SqlParameter("@TenSanPham", SqlDbType.NVarChar, 200) { Value = name },
                    new SqlParameter("@MaSKU", SqlDbType.NVarChar, 50) { Value = sku.Length == 0 ? DBNull.Value : sku },
                    new SqlParameter("@LoaiSanPham", SqlDbType.NVarChar, 30) { Value = productType },
                    new SqlParameter("@BatBuocCongThuc", SqlDbType.Bit) { Value = chkMenuProductRecipe.Checked },
                    new SqlParameter("@DangHoatDong", SqlDbType.Bit) { Value = chkMenuProductActive.Checked },
                    new SqlParameter("@MaSanPham", SqlDbType.Int) { Value = _selectedMenuProductId }
                };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show(isNew ? "Đã thêm sản phẩm vào thực đơn." : "Đã cập nhật sản phẩm.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMenuProductPage();
                LoadMenuPriceProductLookup();
                ResetMenuProductForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeactivateMenuProduct()
        {
            if (_selectedMenuProductId == 0)
            {
                MessageBox.Show("Hãy chọn sản phẩm cần ngừng kinh doanh.", "Chưa chọn sản phẩm", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Ngừng kinh doanh sản phẩm này trên toàn hệ thống? Dữ liệu giá và hóa đơn cũ vẫn được giữ.", "Xác nhận ngừng kinh doanh", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE dbo.SanPham SET DangHoatDong = 0 WHERE MaSanPham = @MaSanPham",
                    new[] { new SqlParameter("@MaSanPham", SqlDbType.Int) { Value = _selectedMenuProductId } });
                LoadMenuProductPage();
                LoadMenuPriceProductLookup();
                ResetMenuProductForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ngừng kinh doanh sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindMenuUnit()
        {
            if (dgvMenuUnits.CurrentRow == null || dgvMenuUnits.CurrentRow.Index < 0) return;
            var row = dgvMenuUnits.CurrentRow;
            _selectedMenuUnitId = Convert.ToInt32(row.Cells["MaDonVi"].Value);
            txtMenuUnitName.Text = Convert.ToString(row.Cells["TenDonVi"].Value) ?? string.Empty;
            cboMenuUnitType.SelectedItem = Convert.ToString(row.Cells["LoaiDonVi"].Value);
        }

        private void ResetMenuUnitForm(bool focus = true)
        {
            _selectedMenuUnitId = 0;
            txtMenuUnitName.Clear();
            if (cboMenuUnitType.Items.Count > 0) cboMenuUnitType.SelectedIndex = 0;
            if (focus) txtMenuUnitName.Focus();
        }

        private void SaveMenuUnit(bool isNew)
        {
            string name = txtMenuUnitName.Text.Trim();
            if (name.Length == 0 || cboMenuUnitType.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập tên và chọn loại đơn vị.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!isNew && _selectedMenuUnitId == 0)
            {
                MessageBox.Show("Hãy chọn đơn vị cần cập nhật trong danh sách.", "Chưa chọn đơn vị", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                string query = isNew
                    ? "INSERT INTO dbo.DonVi (TenDonVi, LoaiDonVi) VALUES (@TenDonVi, @LoaiDonVi)"
                    : "UPDATE dbo.DonVi SET TenDonVi = @TenDonVi, LoaiDonVi = @LoaiDonVi WHERE MaDonVi = @MaDonVi";
                var parameters = new[]
                {
                    new SqlParameter("@TenDonVi", SqlDbType.NVarChar, 50) { Value = name },
                    new SqlParameter("@LoaiDonVi", SqlDbType.NVarChar, 20) { Value = Convert.ToString(cboMenuUnitType.SelectedItem)! },
                    new SqlParameter("@MaDonVi", SqlDbType.Int) { Value = _selectedMenuUnitId }
                };
                DatabaseHelper.ExecuteNonQuery(query, parameters);
                MessageBox.Show(isNew ? "Đã thêm đơn vị tính." : "Đã cập nhật đơn vị tính.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadMenuUnitPage();
                LoadMenuProductLookups();
                ResetMenuUnitForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu đơn vị tính: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteMenuUnit()
        {
            if (_selectedMenuUnitId == 0)
            {
                MessageBox.Show("Hãy chọn đơn vị cần xóa.", "Chưa chọn đơn vị", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Xóa đơn vị đã chọn? Cơ sở dữ liệu sẽ từ chối nếu đơn vị đang được sản phẩm hoặc nguyên liệu sử dụng.", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "DELETE FROM dbo.DonVi WHERE MaDonVi = @MaDonVi",
                    new[] { new SqlParameter("@MaDonVi", SqlDbType.Int) { Value = _selectedMenuUnitId } });
                LoadMenuUnitPage();
                LoadMenuProductLookups();
                ResetMenuUnitForm();
                MessageBox.Show("Đã xóa đơn vị tính.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể xóa đơn vị đang được sản phẩm hoặc nguyên liệu sử dụng. Chi tiết: " + ex.Message, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BindMenuPrice()
        {
            if (dgvMenuPrices.CurrentRow == null || dgvMenuPrices.CurrentRow.Index < 0) return;
            var row = dgvMenuPrices.CurrentRow;
            int productId = Convert.ToInt32(row.Cells["MaSanPham"].Value);
            int branchId = Convert.ToInt32(row.Cells["MaChiNhanh"].Value);
            decimal price = Convert.ToDecimal(row.Cells["GiaBan"].Value);
            bool isAvailable = Convert.ToBoolean(row.Cells["ConBan"].Value);
            int currentlySelectedBranch = ReadBoundComboInteger(cboMenuPriceBranch, "MaChiNhanh", -1);
            bool branchChanged = UserSession.IsAdmin && currentlySelectedBranch != branchId;
            if (branchChanged)
            {
                _loadingMenuBranchOptions = true;
                try
                {
                    cboMenuPriceBranch.SelectedValue = branchId;
                }
                finally
                {
                    _loadingMenuBranchOptions = false;
                }
            }
            _selectedMenuPriceProductId = productId;
            _selectedMenuPriceBranchId = branchId;
            _selectedMenuPriceAvailable = isAvailable;
            cboMenuPriceProduct.SelectedValue = productId;
            numMenuPrice.Value = Math.Min(price, numMenuPrice.Maximum);
            chkMenuPriceAvailable.Checked = isAvailable;
            if (branchChanged)
            {
                LoadMenuPricePage();
                SelectMenuPriceRow(branchId, productId);
            }
            UpdateMenuPriceEditorState();
        }

        private void SelectMenuPriceRow(int branchId, int productId)
        {
            foreach (DataGridViewRow row in dgvMenuPrices.Rows)
            {
                if (Convert.ToInt32(row.Cells["MaChiNhanh"].Value) != branchId
                    || Convert.ToInt32(row.Cells["MaSanPham"].Value) != productId)
                    continue;

                row.Selected = true;
                dgvMenuPrices.CurrentCell = row.Cells["TenSanPham"];
                break;
            }
        }

        private void ResetMenuPriceForm()
        {
            _selectedMenuPriceProductId = 0;
            _selectedMenuPriceBranchId = 0;
            _selectedMenuPriceAvailable = false;
            numMenuPrice.Value = 0;
            chkMenuPriceAvailable.Checked = true;
            UpdateMenuPriceEditorState();
        }

        private void SaveMenuPrice(bool updateSelected)
        {
            if (updateSelected && (_selectedMenuPriceProductId == 0 || _selectedMenuPriceBranchId == 0))
            {
                MessageBox.Show("Chọn một dòng giá trong danh sách trước khi cập nhật.", "Chưa chọn giá", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int selectedBranchId = ReadBoundComboInteger(cboMenuPriceBranch, "MaChiNhanh", int.MinValue);
            int selectedProductId = ReadBoundComboInteger(cboMenuPriceProduct, "MaSanPham", 0);
            if (selectedBranchId == int.MinValue || (!updateSelected && selectedProductId <= 0))
            {
                MessageBox.Show("Hãy chọn chi nhánh và sản phẩm cần thiết lập giá.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int branchId = updateSelected
                ? _selectedMenuPriceBranchId
                : UserSession.IsAdmin ? selectedBranchId : UserSession.MaChiNhanh;
            if (branchId <= 0)
            {
                MessageBox.Show("Chọn một chi nhánh cụ thể trước khi lưu giá.", "Chưa chọn chi nhánh", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!UserSession.IsAdmin && branchId != UserSession.MaChiNhanh)
            {
                MessageBox.Show("Bạn chỉ được thay đổi giá tại chi nhánh được phân công.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int productId = updateSelected ? _selectedMenuPriceProductId : selectedProductId;
            try
            {
                string query = updateSelected
                    ? @"UPDATE dbo.SanPham_ChiNhanh
                       SET GiaBan = @GiaBan, ConBan = @ConBan
                       WHERE MaChiNhanh = @MaChiNhanh AND MaSanPham = @MaSanPham"
                    : @"IF EXISTS (SELECT 1 FROM dbo.SanPham_ChiNhanh WHERE MaChiNhanh = @MaChiNhanh AND MaSanPham = @MaSanPham)
                           UPDATE dbo.SanPham_ChiNhanh SET GiaBan = @GiaBan, ConBan = @ConBan
                           WHERE MaChiNhanh = @MaChiNhanh AND MaSanPham = @MaSanPham;
                       ELSE
                           INSERT INTO dbo.SanPham_ChiNhanh (MaChiNhanh, MaSanPham, GiaBan, ConBan)
                           VALUES (@MaChiNhanh, @MaSanPham, @GiaBan, @ConBan);";
                var parameters = new[]
                {
                    new SqlParameter("@MaChiNhanh", SqlDbType.Int) { Value = branchId },
                    new SqlParameter("@MaSanPham", SqlDbType.Int) { Value = productId },
                    new SqlParameter("@GiaBan", SqlDbType.Decimal) { Precision = 18, Scale = 2, Value = numMenuPrice.Value },
                    new SqlParameter("@ConBan", SqlDbType.Bit) { Value = chkMenuPriceAvailable.Checked }
                };
                int affectedRows = DatabaseHelper.ExecuteNonQuery(query, parameters);
                if (updateSelected && affectedRows == 0)
                {
                    MessageBox.Show("Dòng giá đã bị xóa hoặc thay đổi. Hãy tải lại danh sách rồi thử lại.", "Không tìm thấy giá", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LoadMenuPricePage();
                    return;
                }
                _selectedMenuPriceProductId = productId;
                _selectedMenuPriceBranchId = branchId;
                _selectedMenuPriceAvailable = chkMenuPriceAvailable.Checked;
                LoadMenuPricePage();
                SelectMenuPriceRow(branchId, productId);
                MessageBox.Show("Đã lưu giá bán cho chi nhánh.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu giá bán chi nhánh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleMenuPriceAvailability()
        {
            if (_selectedMenuPriceProductId == 0 || _selectedMenuPriceBranchId == 0)
            {
                MessageBox.Show("Hãy chọn dòng giá bán cần thay đổi trạng thái.", "Chưa chọn giá", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!UserSession.IsAdmin && _selectedMenuPriceBranchId != UserSession.MaChiNhanh)
            {
                MessageBox.Show("Bạn chỉ được thay đổi giá tại chi nhánh được phân công.", "Không có quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE dbo.SanPham_ChiNhanh SET ConBan = @ConBan WHERE MaChiNhanh = @MaChiNhanh AND MaSanPham = @MaSanPham",
                    new[]
                    {
                        new SqlParameter("@ConBan", SqlDbType.Bit) { Value = !_selectedMenuPriceAvailable },
                        new SqlParameter("@MaChiNhanh", SqlDbType.Int) { Value = _selectedMenuPriceBranchId },
                        new SqlParameter("@MaSanPham", SqlDbType.Int) { Value = _selectedMenuPriceProductId }
                    });
                _selectedMenuPriceAvailable = !_selectedMenuPriceAvailable;
                LoadMenuPricePage();
                ResetMenuPriceForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thay đổi trạng thái bán tại chi nhánh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMenuCategories_Click(object sender, EventArgs e)
        {

        }
    }
}
