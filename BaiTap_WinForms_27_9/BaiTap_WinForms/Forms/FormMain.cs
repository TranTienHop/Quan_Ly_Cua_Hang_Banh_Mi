using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using BaiTap_WinForms.Data;
using BaiTap_WinForms.Models;

namespace BaiTap_WinForms.Forms
{
    public partial class FormMain : Form
    {
        private readonly CultureInfo _vnCulture = new CultureInfo("vi-VN");

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            // Hiển thị thông tin đăng nhập
            lblUserInfo.Text = $"Xin chào: {UserSession.FullName} ({UserSession.RoleName})";

            // Phân quyền: nếu là STAFF thì ẩn các tab quản lý nâng cao (hoặc chỉ cho phép POS)
            if (!UserSession.IsAdmin)
            {
                btnNavEmployees.Visible = false;
                btnNavReports.Visible = false;
            }

            // Tải dữ liệu ban đầu
            LoadPOSCategories();
            LoadPOSProducts();
            LoadProductsTab();
            LoadIngredientsTab();
            LoadReportsTab();
            LoadEmployeesTab();

            // Mặc định chọn POS
            SwitchTab(0, btnNavPOS);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                UserSession.Clear();
                this.Hide();
                var loginForm = new FormLogin();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    this.FormMain_Load(sender, e);
                    this.Show();
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void btnNav_Click(object sender, EventArgs e)
        {
            if (sender == btnNavPOS) SwitchTab(0, btnNavPOS);
            else if (sender == btnNavProducts) SwitchTab(1, btnNavProducts);
            else if (sender == btnNavIngredients) SwitchTab(2, btnNavIngredients);
            else if (sender == btnNavReports) SwitchTab(3, btnNavReports);
            else if (sender == btnNavEmployees) SwitchTab(4, btnNavEmployees);
        }

        private void SwitchTab(int tabIndex, Button activeBtn)
        {
            tabMain.SelectedIndex = tabIndex;
            var buttons = new[] { btnNavPOS, btnNavProducts, btnNavIngredients, btnNavReports, btnNavEmployees };
            foreach (var btn in buttons)
            {
                if (btn == activeBtn)
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
        }

        #region ======================== 1. MODULE BÁN HÀNG (POS) ========================

        private ComboBox cboPOSCategory = null!;
        private TextBox txtPOSSearch = null!;
        private DataGridView dgvPOSMenu = null!;
        private DataGridView dgvPOSCart = null!;
        private Label lblPOSTotal = null!;
        private NumericUpDown numPOSDiscount = null!;
        private Label lblPOSFinal = null!;
        private ComboBox cboPOSPaymentMethod = null!;
        private TextBox txtPOSCustomerMoney = null!;
        private Label lblPOSChange = null!;
        private TextBox txtPOSNote = null!;
        private Button btnPOSCheckout = null!;
        private Button btnPOSClear = null!;

        private readonly DataTable _cartTable = new DataTable();

        private void InitializePOSTab()
        {
            tabPOS.BackColor = Color.FromArgb(245, 246, 250);

            // Cấu trúc cartTable
            _cartTable.Columns.Add("ProductID", typeof(int));
            _cartTable.Columns.Add("ProductCode", typeof(string));
            _cartTable.Columns.Add("ProductName", typeof(string));
            _cartTable.Columns.Add("UnitPrice", typeof(decimal));
            _cartTable.Columns.Add("Quantity", typeof(decimal));
            _cartTable.Columns.Add("TotalAmount", typeof(decimal));

            // Chia làm 2 cột: Trái (Thực đơn), Phải (Giỏ hàng & Thanh toán)
            var splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 640,
                IsSplitterFixed = false
            };
            tabPOS.Controls.Add(splitContainer);

            // ================= CỘT TRÁI: THỰC ĐƠN =================
            var panelMenu = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            splitContainer.Panel1.Controls.Add(panelMenu);

            var groupMenu = new GroupBox
            {
                Text = "📋 THỰC ĐƠN MÓN",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            panelMenu.Controls.Add(groupMenu);

            // Thanh tìm kiếm + lọc danh mục
            var pnlMenuFilter = new Panel { Dock = DockStyle.Top, Height = 45, Padding = new Padding(5) };
            groupMenu.Controls.Add(pnlMenuFilter);

            var lblCat = new Label { Text = "Danh mục:", Location = new Point(5, 12), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            cboPOSCategory = new ComboBox { Location = new Point(80, 8), Size = new Size(180, 28), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cboPOSCategory.SelectedIndexChanged += (s, e) => LoadPOSProducts();

            var lblSearch = new Label { Text = "Tìm món:", Location = new Point(280, 12), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtPOSSearch = new TextBox { Location = new Point(350, 8), Size = new Size(220, 28), Font = new Font("Segoe UI", 9.5F) };
            txtPOSSearch.TextChanged += (s, e) => LoadPOSProducts();

            pnlMenuFilter.Controls.AddRange(new Control[] { lblCat, cboPOSCategory, lblSearch, txtPOSSearch });

            // DataGridView Thực đơn
            dgvPOSMenu = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            dgvPOSMenu.CellDoubleClick += (s, e) => AddSelectedProductToCart();
            groupMenu.Controls.Add(dgvPOSMenu);
            dgvPOSMenu.BringToFront();

            var btnAddCart = new Button
            {
                Text = "➕ Thêm Vào Giỏ Hàng (Hoặc Double-click)",
                Dock = DockStyle.Bottom,
                Height = 40,
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAddCart.FlatAppearance.BorderSize = 0;
            btnAddCart.Click += (s, e) => AddSelectedProductToCart();
            groupMenu.Controls.Add(btnAddCart);

            // ================= CỘT PHẢI: GIỎ HÀNG & THANH TOÁN =================
            var panelCart = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10) };
            splitContainer.Panel2.Controls.Add(panelCart);

            var groupCart = new GroupBox
            {
                Text = "🛒 ĐƠN HÀNG HIỆN TẠI",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80)
            };
            panelCart.Controls.Add(groupCart);

            // DataGridView Giỏ hàng – dùng cột thủ công để tránh bị chồng
            dgvPOSCart = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                AutoGenerateColumns = false,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };

            // Định nghĩa cột thủ công
            dgvPOSCart.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProductID",   Visible = false, HeaderText = "ID" });
            dgvPOSCart.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProductCode", Visible = false, HeaderText = "Mã" });
            dgvPOSCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName", HeaderText = "Tên Món",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 120
            });
            dgvPOSCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UnitPrice", HeaderText = "Đơn Giá", Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvPOSCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Quantity", HeaderText = "SL", Width = 48,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvPOSCart.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TotalAmount", HeaderText = "Thành Tiền", Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0", Alignment = DataGridViewContentAlignment.MiddleRight },
                ReadOnly = true
            });

            dgvPOSCart.DataSource = _cartTable;
            dgvPOSCart.CellValueChanged += (s, e) => RecalculateCart();
            dgvPOSCart.RowsRemoved += (s, e) => RecalculateCart();

            // Panel thanh toán phía dưới giỏ hàng
            var pnlCheckout = new Panel { Dock = DockStyle.Bottom, Height = 280, Padding = new Padding(10), BackColor = Color.FromArgb(240, 243, 244) };

            int y = 10;
            var lblT1 = new Label { Text = "Tổng tiền hàng:", Location = new Point(15, y), AutoSize = true, Font = new Font("Segoe UI", 10F) };
            lblPOSTotal = new Label { Text = "0 đ", Location = new Point(160, y), AutoSize = true, Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80) };

            y += 30;
            var lblT2 = new Label { Text = "Giảm giá (VNĐ):", Location = new Point(15, y), AutoSize = true, Font = new Font("Segoe UI", 10F) };
            numPOSDiscount = new NumericUpDown { Location = new Point(160, y - 2), Size = new Size(160, 26), Maximum = 10000000, Increment = 1000, Font = new Font("Segoe UI", 9.5F) };
            numPOSDiscount.ValueChanged += (s, e) => RecalculateCart();

            y += 30;
            var lblT3 = new Label { Text = "KHÁCH PHẢI TRẢ:", Location = new Point(15, y), AutoSize = true, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold) };
            lblPOSFinal = new Label { Text = "0 đ", Location = new Point(160, y), AutoSize = true, Font = new Font("Segoe UI", 13F, FontStyle.Bold), ForeColor = Color.FromArgb(192, 57, 43) };

            y += 35;
            var lblT4 = new Label { Text = "Hình thức TT:", Location = new Point(15, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            cboPOSPaymentMethod = new ComboBox { Location = new Point(160, y - 2), Size = new Size(160, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };
            cboPOSPaymentMethod.Items.AddRange(new object[] { "CASH (Tiền mặt)", "BANK_TRANSFER (Chuyển khoản)" });
            cboPOSPaymentMethod.SelectedIndex = 0;

            y += 30;
            var lblT5 = new Label { Text = "Tiền khách đưa:", Location = new Point(15, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtPOSCustomerMoney = new TextBox { Location = new Point(160, y - 2), Size = new Size(160, 26), Font = new Font("Segoe UI", 9.5F) };
            txtPOSCustomerMoney.TextChanged += (s, e) => CalculateChangeMoney();

            y += 28;
            var lblT6 = new Label { Text = "Tiền thối lại:", Location = new Point(15, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            lblPOSChange = new Label { Text = "0 đ", Location = new Point(160, y), AutoSize = true, Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), ForeColor = Color.FromArgb(39, 174, 96) };

            y += 30;
            var lblT7 = new Label { Text = "Ghi chú:", Location = new Point(15, y), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            txtPOSNote = new TextBox { Location = new Point(80, y - 2), Size = new Size(240, 24), Font = new Font("Segoe UI", 9F) };

            y += 35;
            btnPOSCheckout = new Button
            {
                Text = "💰 THANH TOÁN (LƯU ĐƠN)",
                Location = new Point(15, y),
                Size = new Size(210, 42),
                BackColor = Color.FromArgb(211, 84, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnPOSCheckout.FlatAppearance.BorderSize = 0;
            btnPOSCheckout.Click += (s, e) => ProcessPOSCheckout();

            btnPOSClear = new Button
            {
                Text = "❌ Hủy Giỏ",
                Location = new Point(235, y),
                Size = new Size(95, 42),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnPOSClear.FlatAppearance.BorderSize = 0;
            btnPOSClear.Click += (s, e) => ClearPOSCart();

            pnlCheckout.Controls.AddRange(new Control[]
            {
                lblT1, lblPOSTotal, lblT2, numPOSDiscount, lblT3, lblPOSFinal,
                lblT4, cboPOSPaymentMethod, lblT5, txtPOSCustomerMoney, lblT6, lblPOSChange,
                lblT7, txtPOSNote, btnPOSCheckout, btnPOSClear
            });

            groupCart.Controls.Add(dgvPOSCart);
            groupCart.Controls.Add(pnlCheckout);
        }

        private void LoadPOSCategories()
        {
            try
            {
                string query = "SELECT CategoryID, CategoryName FROM dbo.Categories WHERE IsActive = 1 ORDER BY CategoryID";
                DataTable dt = DatabaseHelper.ExecuteQuery(query);
                var row = dt.NewRow();
                row["CategoryID"] = 0;
                row["CategoryName"] = "--- Tất cả danh mục ---";
                dt.Rows.InsertAt(row, 0);

                cboPOSCategory.DataSource = dt;
                cboPOSCategory.DisplayMember = "CategoryName";
                cboPOSCategory.ValueMember = "CategoryID";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPOSProducts()
        {
            try
            {
                int catId = 0;
                if (cboPOSCategory.SelectedValue != null && int.TryParse(cboPOSCategory.SelectedValue.ToString(), out int val))
                {
                    catId = val;
                }

                string search = txtPOSSearch.Text.Trim();

                string query = @"
                    SELECT p.ProductID, p.ProductCode AS [Mã Món], p.ProductName AS [Tên Món], 
                           c.CategoryName AS [Danh Mục], p.SellingPrice AS [Giá Bán (VNĐ)], p.Unit AS [ĐVT]
                    FROM dbo.Products p
                    JOIN dbo.Categories c ON p.CategoryID = c.CategoryID
                    WHERE p.IsActive = 1";

                var parameters = new List<SqlParameter>();

                if (catId > 0)
                {
                    query += " AND p.CategoryID = @CatID";
                    parameters.Add(new SqlParameter("@CatID", catId));
                }

                if (!string.IsNullOrEmpty(search))
                {
                    query += " AND (p.ProductName LIKE @Search OR p.ProductCode LIKE @Search)";
                    parameters.Add(new SqlParameter("@Search", "%" + search + "%"));
                }

                query += " ORDER BY p.ProductID";

                DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
                dgvPOSMenu.DataSource = dt;
                if (dgvPOSMenu.Columns["ProductID"] != null)
                    dgvPOSMenu.Columns["ProductID"].Visible = false;

                if (dgvPOSMenu.Columns["Giá Bán (VNĐ)"] != null)
                    dgvPOSMenu.Columns["Giá Bán (VNĐ)"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thực đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddSelectedProductToCart()
        {
            if (dgvPOSMenu.CurrentRow == null) return;

            int productId = Convert.ToInt32(dgvPOSMenu.CurrentRow.Cells["ProductID"].Value);
            string code = dgvPOSMenu.CurrentRow.Cells["Mã Món"].Value.ToString() ?? "";
            string name = dgvPOSMenu.CurrentRow.Cells["Tên Món"].Value.ToString() ?? "";
            decimal price = Convert.ToDecimal(dgvPOSMenu.CurrentRow.Cells["Giá Bán (VNĐ)"].Value);

            // Kiểm tra món đã có trong giỏ hàng chưa
            DataRow[] existing = _cartTable.Select("ProductID = " + productId);
            if (existing.Length > 0)
            {
                existing[0]["Quantity"] = Convert.ToDecimal(existing[0]["Quantity"]) + 1;
                existing[0]["TotalAmount"] = Convert.ToDecimal(existing[0]["Quantity"]) * price;
            }
            else
            {
                var newRow = _cartTable.NewRow();
                newRow["ProductID"] = productId;
                newRow["ProductCode"] = code;
                newRow["ProductName"] = name;
                newRow["UnitPrice"] = price;
                newRow["Quantity"] = 1;
                newRow["TotalAmount"] = price;
                _cartTable.Rows.Add(newRow);
            }

            RecalculateCart();
        }

        private decimal _totalOrderAmount = 0;
        private decimal _finalOrderAmount = 0;

        private void RecalculateCart()
        {
            _totalOrderAmount = 0;
            foreach (DataRow row in _cartTable.Rows)
            {
                decimal qty = Convert.ToDecimal(row["Quantity"]);
                decimal price = Convert.ToDecimal(row["UnitPrice"]);
                row["TotalAmount"] = qty * price;
                _totalOrderAmount += qty * price;
            }

            lblPOSTotal.Text = _totalOrderAmount.ToString("N0", _vnCulture) + " đ";

            decimal discount = numPOSDiscount.Value;
            _finalOrderAmount = Math.Max(0, _totalOrderAmount - discount);
            lblPOSFinal.Text = _finalOrderAmount.ToString("N0", _vnCulture) + " đ";

            CalculateChangeMoney();
        }

        private void CalculateChangeMoney()
        {
            if (decimal.TryParse(txtPOSCustomerMoney.Text.Replace(",", "").Replace(".", "").Trim(), out decimal customerMoney))
            {
                decimal change = customerMoney - _finalOrderAmount;
                if (change >= 0)
                {
                    lblPOSChange.Text = change.ToString("N0", _vnCulture) + " đ";
                    lblPOSChange.ForeColor = Color.FromArgb(39, 174, 96);
                }
                else
                {
                    lblPOSChange.Text = "Thiếu: " + Math.Abs(change).ToString("N0", _vnCulture) + " đ";
                    lblPOSChange.ForeColor = Color.FromArgb(192, 57, 43);
                }
            }
            else
            {
                lblPOSChange.Text = "0 đ";
            }
        }

        private void ClearPOSCart()
        {
            _cartTable.Rows.Clear();
            numPOSDiscount.Value = 0;
            txtPOSCustomerMoney.Clear();
            txtPOSNote.Clear();
            RecalculateCart();
        }

        private void ProcessPOSCheckout()
        {
            if (_cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Giỏ hàng đang trống! Vui lòng chọn món trước khi thanh toán.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string orderCode = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            decimal subTotal = _totalOrderAmount;
            decimal discount = numPOSDiscount.Value;
            decimal totalAmount = _finalOrderAmount;
            string paymentMethod = cboPOSPaymentMethod.SelectedIndex == 1 ? "BANK_TRANSFER" : "CASH";
            string note = txtPOSNote.Text.Trim();

            try
            {
                using var conn = DatabaseHelper.GetConnection();
                conn.Open();
                using var transaction = conn.BeginTransaction();

                // 1. Tạo đơn hàng Orders
                string insertOrderSql = @"
                    INSERT INTO dbo.Orders (OrderCode, EmployeeID, OrderDate, SubTotal, DiscountAmount, TotalAmount, PaymentStatus, OrderStatus, Note)
                    VALUES (@OrderCode, @EmployeeID, SYSDATETIME(), @SubTotal, @Discount, @TotalAmount, 'PAID', 'COMPLETED', @Note);
                    SELECT SCOPE_IDENTITY();";

                using var cmdOrder = new SqlCommand(insertOrderSql, conn, transaction);
                cmdOrder.Parameters.AddWithValue("@OrderCode", orderCode);
                cmdOrder.Parameters.AddWithValue("@EmployeeID", UserSession.EmployeeID);
                cmdOrder.Parameters.AddWithValue("@SubTotal", subTotal);
                cmdOrder.Parameters.AddWithValue("@Discount", discount);
                cmdOrder.Parameters.AddWithValue("@TotalAmount", totalAmount);
                cmdOrder.Parameters.AddWithValue("@Note", string.IsNullOrEmpty(note) ? (object)DBNull.Value : note);

                int orderId = Convert.ToInt32(cmdOrder.ExecuteScalar());

                // 2. Tạo OrderDetails
                foreach (DataRow row in _cartTable.Rows)
                {
                    string insertDetailSql = @"
                        INSERT INTO dbo.OrderDetails (OrderID, ProductID, Quantity, UnitPrice, DiscountAmount)
                        VALUES (@OrderID, @ProductID, @Quantity, @UnitPrice, 0);";

                    using var cmdDetail = new SqlCommand(insertDetailSql, conn, transaction);
                    cmdDetail.Parameters.AddWithValue("@OrderID", orderId);
                    cmdDetail.Parameters.AddWithValue("@ProductID", Convert.ToInt32(row["ProductID"]));
                    cmdDetail.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(row["Quantity"]));
                    cmdDetail.Parameters.AddWithValue("@UnitPrice", Convert.ToDecimal(row["UnitPrice"]));
                    cmdDetail.ExecuteNonQuery();
                }

                // 3. Tạo Payments
                string insertPaymentSql = @"
                    INSERT INTO dbo.Payments (OrderID, PaymentMethod, Amount, PaidAt, PaymentStatus)
                    VALUES (@OrderID, @Method, @Amount, SYSDATETIME(), 'SUCCESS');";

                using var cmdPay = new SqlCommand(insertPaymentSql, conn, transaction);
                cmdPay.Parameters.AddWithValue("@OrderID", orderId);
                cmdPay.Parameters.AddWithValue("@Method", paymentMethod);
                cmdPay.Parameters.AddWithValue("@Amount", totalAmount);
                cmdPay.ExecuteNonQuery();

                // Commit transaction tạo đơn
                transaction.Commit();

                // 4. Tự động gọi Stored Procedure trừ kho
                try
                {
                    DatabaseHelper.ExecuteNonQuery("dbo.sp_DeductStockForOrder", new SqlParameter[]
                    {
                        new SqlParameter("@OrderID", orderId)
                    }, CommandType.StoredProcedure);
                }
                catch (Exception exSp)
                {
                    Console.WriteLine("SP deduct error: " + exSp.Message);
                }

                string msg = "Thanh toán thành công!\n\nMã hóa đơn: " + orderCode + "\nTổng tiền: " + totalAmount.ToString("N0", _vnCulture) + " đ\nTồn kho nguyên liệu đã được tự động cập nhật!";
                MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearPOSCart();
                LoadIngredientsTab(); // Làm mới kho
                LoadReportsTab();     // Làm mới báo cáo
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thanh toán đơn hàng:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ======================== 2. MODULE QUẢN LÝ THỰC ĐƠN ========================

        private DataGridView dgvProducts = null!;
        private TextBox txtProdCode = null!;
        private TextBox txtProdName = null!;
        private ComboBox cboProdCategory = null!;
        private NumericUpDown numProdPrice = null!;
        private TextBox txtProdUnit = null!;
        private CheckBox chkProdActive = null!;
        private int _selectedProductID = 0;

        private void InitializeProductsTab()
        {
            tabProducts.BackColor = Color.FromArgb(245, 246, 250);

            var split = new SplitContainer { Dock = DockStyle.Fill, SplitterDistance = 780 };
            tabProducts.Controls.Add(split);

            // Trái: Bảng danh sách
            var grpList = new GroupBox
            {
                Text = "DANH SÁCH MÓN ĂN & NƯỚC UỐNG",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Padding = new Padding(10)
            };
            split.Panel1.Controls.Add(grpList);

            dgvProducts = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Font = new Font("Segoe UI", 9.5F)
            };
            dgvProducts.CellClick += (s, e) => BindProductToForm();
            grpList.Controls.Add(dgvProducts);

            // Phải: Form thông tin
            var grpForm = new GroupBox
            {
                Text = "THÔNG TIN SẢN PHẨM",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Padding = new Padding(15)
            };
            split.Panel2.Controls.Add(grpForm);

            int y = 30;
            var l1 = new Label { Text = "Mã sản phẩm:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtProdCode = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l2 = new Label { Text = "Tên món:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtProdName = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l3 = new Label { Text = "Danh mục:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            cboProdCategory = new ComboBox { Location = new Point(20, y + 22), Size = new Size(280, 26), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l4 = new Label { Text = "Giá bán (VNĐ):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            numProdPrice = new NumericUpDown { Location = new Point(20, y + 22), Size = new Size(280, 26), Maximum = 10000000, Increment = 1000, Font = new Font("Segoe UI", 9.5F) };

            y += 60;
            var l5 = new Label { Text = "Đơn vị tính (Ổ, Ly, Chai...):", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9.5F) };
            txtProdUnit = new TextBox { Location = new Point(20, y + 22), Size = new Size(280, 26), Font = new Font("Segoe UI", 9.5F), Text = "Ổ" };

            y += 55;
            chkProdActive = new CheckBox { Text = "Đang kinh doanh", Location = new Point(20, y), AutoSize = true, Checked = true, Font = new Font("Segoe UI", 9.5F) };

            y += 40;
            var btnAdd = new Button { Text = "➕ Thêm Mới", Location = new Point(20, y), Size = new Size(130, 36), BackColor = Color.FromArgb(39, 174, 96), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => SaveProduct(isNew: true);

            var btnEdit = new Button { Text = "✏️ Cập Nhật", Location = new Point(160, y), Size = new Size(140, 36), BackColor = Color.FromArgb(41, 128, 185), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Click += (s, e) => SaveProduct(isNew: false);

            y += 45;
            var btnDelete = new Button { Text = "🗑️ Xóa Món", Location = new Point(20, y), Size = new Size(130, 36), BackColor = Color.FromArgb(192, 57, 43), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += (s, e) => DeleteProduct();

            var btnReset = new Button { Text = "🔄 Làm Mới", Location = new Point(160, y), Size = new Size(140, 36), BackColor = Color.FromArgb(149, 165, 166), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9.5F) };
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.Click += (s, e) => ResetProductForm();

            grpForm.Controls.AddRange(new Control[]
            {
                l1, txtProdCode, l2, txtProdName, l3, cboProdCategory,
                l4, numProdPrice, l5, txtProdUnit, chkProdActive,
                btnAdd, btnEdit, btnDelete, btnReset
            });
        }

        private void LoadProductsTab()
        {
            try
            {
                // Danh mục
                string queryCat = "SELECT CategoryID, CategoryName FROM dbo.Categories WHERE IsActive = 1";
                DataTable dtCat = DatabaseHelper.ExecuteQuery(queryCat);
                cboProdCategory.DataSource = dtCat;
                cboProdCategory.DisplayMember = "CategoryName";
                cboProdCategory.ValueMember = "CategoryID";

                // Sản phẩm
                string queryProd = @"
                    SELECT p.ProductID, p.ProductCode AS [Mã Món], p.ProductName AS [Tên Món],
                           c.CategoryName AS [Danh Mục], p.SellingPrice AS [Giá Bán (VNĐ)], 
                           p.Unit AS [ĐVT], p.IsActive AS [Hoạt Động], p.CategoryID
                    FROM dbo.Products p
                    JOIN dbo.Categories c ON p.CategoryID = c.CategoryID
                    ORDER BY p.ProductID";

                DataTable dtProd = DatabaseHelper.ExecuteQuery(queryProd);
                dgvProducts.DataSource = dtProd;
                if (dgvProducts.Columns["ProductID"] != null) dgvProducts.Columns["ProductID"].Visible = false;
                if (dgvProducts.Columns["CategoryID"] != null) dgvProducts.Columns["CategoryID"].Visible = false;
                if (dgvProducts.Columns["Giá Bán (VNĐ)"] != null) dgvProducts.Columns["Giá Bán (VNĐ)"].DefaultCellStyle.Format = "N0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindProductToForm()
        {
            if (dgvProducts.CurrentRow == null) return;
            var row = dgvProducts.CurrentRow;
            _selectedProductID = Convert.ToInt32(row.Cells["ProductID"].Value);
            txtProdCode.Text = row.Cells["Mã Món"].Value.ToString();
            txtProdName.Text = row.Cells["Tên Món"].Value.ToString();
            cboProdCategory.SelectedValue = row.Cells["CategoryID"].Value;
            numProdPrice.Value = Convert.ToDecimal(row.Cells["Giá Bán (VNĐ)"].Value);
            txtProdUnit.Text = row.Cells["ĐVT"].Value.ToString();
            chkProdActive.Checked = Convert.ToBoolean(row.Cells["Hoạt Động"].Value);
        }

        private void ResetProductForm()
        {
            _selectedProductID = 0;
            txtProdCode.Clear();
            txtProdName.Clear();
            numProdPrice.Value = 0;
            txtProdUnit.Text = "Ổ";
            chkProdActive.Checked = true;
        }

        private void SaveProduct(bool isNew)
        {
            string code = txtProdCode.Text.Trim();
            string name = txtProdName.Text.Trim();
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập Mã món và Tên món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int catId = Convert.ToInt32(cboProdCategory.SelectedValue);
            decimal price = numProdPrice.Value;
            string unit = txtProdUnit.Text.Trim();
            bool isActive = chkProdActive.Checked;

            try
            {
                if (isNew)
                {
                    string sql = @"
                        INSERT INTO dbo.Products (ProductCode, ProductName, CategoryID, SellingPrice, Unit, IsActive)
                        VALUES (@Code, @Name, @CatID, @Price, @Unit, @IsActive)";
                    var p = new SqlParameter[]
                    {
                        new SqlParameter("@Code", code),
                        new SqlParameter("@Name", name),
                        new SqlParameter("@CatID", catId),
                        new SqlParameter("@Price", price),
                        new SqlParameter("@Unit", unit),
                        new SqlParameter("@IsActive", isActive)
                    };
                    DatabaseHelper.ExecuteNonQuery(sql, p);
                    MessageBox.Show("Thêm món mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (_selectedProductID == 0)
                    {
                        MessageBox.Show("Vui lòng chọn món cần cập nhật từ bảng danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string sql = @"
                        UPDATE dbo.Products
                        SET ProductCode = @Code, ProductName = @Name, CategoryID = @CatID, 
                            SellingPrice = @Price, Unit = @Unit, IsActive = @IsActive, UpdatedAt = SYSDATETIME()
                        WHERE ProductID = @ProductID";
                    var p = new SqlParameter[]
                    {
                        new SqlParameter("@ProductID", _selectedProductID),
                        new SqlParameter("@Code", code),
                        new SqlParameter("@Name", name),
                        new SqlParameter("@CatID", catId),
                        new SqlParameter("@Price", price),
                        new SqlParameter("@Unit", unit),
                        new SqlParameter("@IsActive", isActive)
                    };
                    DatabaseHelper.ExecuteNonQuery(sql, p);
                    MessageBox.Show("Cập nhật thông tin món thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadProductsTab();
                LoadPOSProducts();
                ResetProductForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteProduct()
        {
            if (_selectedProductID == 0)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc muốn xóa (ngừng kinh doanh) món này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                try
                {
                    string sql = "UPDATE dbo.Products SET IsActive = 0 WHERE ProductID = @ProductID";
                    DatabaseHelper.ExecuteNonQuery(sql, new SqlParameter[] { new SqlParameter("@ProductID", _selectedProductID) });
                    MessageBox.Show("Đã ngừng kinh doanh món này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProductsTab();
                    LoadPOSProducts();
                    ResetProductForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region ======================== 3. MODULE KHO NGUYÊN LIỆU ========================

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
                string query;
                if (chkLowStockOnly.Checked)
                {
                    query = @"
                        SELECT IngredientID, IngredientCode AS [Mã NL], IngredientName AS [Tên Nguyên Liệu],
                               Unit AS [ĐVT], CurrentStock AS [Tồn Kho], MinimumStock AS [Tồn Tối Thiểu], CostPrice AS [Giá Vốn]
                        FROM dbo.Ingredients
                        WHERE CurrentStock <= MinimumStock AND IsActive = 1
                        ORDER BY CurrentStock ASC";
                }
                else
                {
                    query = @"
                        SELECT IngredientID, IngredientCode AS [Mã NL], IngredientName AS [Tên Nguyên Liệu],
                               Unit AS [ĐVT], CurrentStock AS [Tồn Kho], MinimumStock AS [Tồn Tối Thiểu], CostPrice AS [Giá Vốn]
                        FROM dbo.Ingredients
                        WHERE IsActive = 1
                        ORDER BY IngredientID";
                }

                DataTable dt = DatabaseHelper.ExecuteQuery(query);
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
                if (_selectedIngredientID > 0)
                {
                    string sql = @"
                        UPDATE dbo.Ingredients
                        SET IngredientCode = @Code, IngredientName = @Name, Unit = @Unit,
                            CurrentStock = @Current, MinimumStock = @Min, CostPrice = @Cost
                        WHERE IngredientID = @ID";
                    var p = new SqlParameter[]
                    {
                        new SqlParameter("@ID", _selectedIngredientID),
                        new SqlParameter("@Code", code),
                        new SqlParameter("@Name", name),
                        new SqlParameter("@Unit", txtIngUnit.Text.Trim()),
                        new SqlParameter("@Current", numIngCurrentStock.Value),
                        new SqlParameter("@Min", numIngMinStock.Value),
                        new SqlParameter("@Cost", numIngCost.Value)
                    };
                    DatabaseHelper.ExecuteNonQuery(sql, p);
                    MessageBox.Show("Cập nhật nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    string sql = @"
                        INSERT INTO dbo.Ingredients (IngredientCode, IngredientName, Unit, CurrentStock, MinimumStock, CostPrice, IsActive)
                        VALUES (@Code, @Name, @Unit, @Current, @Min, @Cost, 1)";
                    var p = new SqlParameter[]
                    {
                        new SqlParameter("@Code", code),
                        new SqlParameter("@Name", name),
                        new SqlParameter("@Unit", txtIngUnit.Text.Trim()),
                        new SqlParameter("@Current", numIngCurrentStock.Value),
                        new SqlParameter("@Min", numIngMinStock.Value),
                        new SqlParameter("@Cost", numIngCost.Value)
                    };
                    DatabaseHelper.ExecuteNonQuery(sql, p);
                    MessageBox.Show("Thêm nguyên liệu mới thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

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
        private RadioButton rdoExpCsv = null!;
        private RadioButton rdoExpHtml = null!;
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

            btnSubNavRev = CreateSubNavButton("💰 Báo Cáo Doanh Thu", 0);
            btnSubNavBestSeller = CreateSubNavButton("🏆 Sản Phẩm Best Seller", 1);
            btnSubNavInventory = CreateSubNavButton("📦 Tồn Kho & Tiêu Hao", 2);
            btnSubNavChart = CreateSubNavButton("📊 Xuất Biểu Đồ Phân Tích", 3);
            btnSubNavExport = CreateSubNavButton("📑 Xuất File Báo Cáo", 4);

            pnlSubNav.Controls.AddRange(new Control[]
            {
                btnSubNavRev, btnSubNavBestSeller, btnSubNavInventory, btnSubNavChart, btnSubNavExport
            });

            // Container chứa 5 panels con
            pnlReportSubContainer = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 246, 250)
            };
            tabReports.Controls.Add(pnlReportSubContainer);
            pnlReportSubContainer.BringToFront();

            // Khởi tạo 5 view panels
            InitializeRevenueSubView();
            InitializeBestSellerSubView();
            InitializeInventorySubView();
            InitializeChartSubView();
            InitializeExportSubView();

            // Mặc định mở view Doanh thu
            SwitchReportSubTab(0);
        }

        private Button CreateSubNavButton(string text, int index)
        {
            var btn = new Button
            {
                Text = text,
                Dock = DockStyle.Left,
                Width = 220,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
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

            var buttons = new[] { btnSubNavRev, btnSubNavBestSeller, btnSubNavInventory, btnSubNavChart, btnSubNavExport };
            var panels = new[] { pnlSubRev, pnlSubBestSeller, pnlSubInventory, pnlSubChart, pnlSubExport };

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
                case 0: LoadRevenueReport(); break;
                case 1: LoadBestSellerReport(); break;
                case 2: LoadInventoryReport(); break;
                case 3: LoadChartReport(); break;
                case 4: LoadExportPreview(); break;
            }
        }

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

        private void ExportRevenueData(bool isHtml)
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

            bool ok = isHtml
                ? ReportExportHelper.ExportToHtml(dt, "Báo Cáo Doanh Thu Bán Hàng", period, branchName, kpis, out string p)
                : ReportExportHelper.ExportToCsv(dt, "Báo Cáo Doanh Thu Bán Hàng", period, branchName, kpis, out p);

            if (ok) ReportExportHelper.PromptOpenFile(p);
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

        private void ExportBestSellerData(bool isHtml)
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

            bool ok = isHtml
                ? ReportExportHelper.ExportToHtml(dt, "Báo Cáo Sản Phẩm Best Seller", period, branchName, kpis, out string p)
                : ReportExportHelper.ExportToCsv(dt, "Báo Cáo Sản Phẩm Best Seller", period, branchName, kpis, out p);

            if (ok) ReportExportHelper.PromptOpenFile(p);
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

        private void ExportInventoryData(bool isHtml)
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

            bool ok = isHtml
                ? ReportExportHelper.ExportToHtml(dt, "Báo Cáo Tồn Kho & Tiêu Hao Nguyên Liệu", period, branchName, kpis, out string p)
                : ReportExportHelper.ExportToCsv(dt, "Báo Cáo Tồn Kho & Tiêu Hao Nguyên Liệu", period, branchName, kpis, out p);

            if (ok) ReportExportHelper.PromptOpenFile(p);
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
            var grpType = new GroupBox { Text = "1. Chọn loại báo cáo", Location = new Point(15, 30), Size = new Size(340, 125), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            rdoExpRev = new RadioButton { Text = "💰 Báo cáo Doanh thu bán hàng", Location = new Point(15, 25), Size = new Size(300, 24), Checked = true, Font = new Font("Segoe UI", 9.5F) };
            rdoExpBestSeller = new RadioButton { Text = "🏆 Báo cáo Sản phẩm Best Seller", Location = new Point(15, 55), Size = new Size(300, 24), Font = new Font("Segoe UI", 9.5F) };
            rdoExpInv = new RadioButton { Text = "📦 Báo cáo Tồn kho & Tiêu hao nguyên liệu", Location = new Point(15, 85), Size = new Size(310, 24), Font = new Font("Segoe UI", 9.5F) };
            grpType.Controls.AddRange(new Control[] { rdoExpRev, rdoExpBestSeller, rdoExpInv });

            // 2. Phạm vi dữ liệu
            var grpRange = new GroupBox { Text = "2. Phạm vi dữ liệu", Location = new Point(15, 165), Size = new Size(340, 140), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            var lblF = new Label { Text = "Từ ngày:", Location = new Point(15, 28), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            dtpExpFrom = new DateTimePicker { Location = new Point(90, 25), Size = new Size(130, 24), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 1), Font = new Font("Segoe UI", 9F) };

            var lblT = new Label { Text = "Đến ngày:", Location = new Point(15, 63), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            dtpExpTo = new DateTimePicker { Location = new Point(90, 60), Size = new Size(130, 24), Format = DateTimePickerFormat.Short, Value = new DateTime(2026, 9, 30), Font = new Font("Segoe UI", 9F) };

            var lblB = new Label { Text = "Chi nhánh:", Location = new Point(15, 98), AutoSize = true, Font = new Font("Segoe UI", 9F) };
            cboExpBranch = new ComboBox { Location = new Point(90, 95), Size = new Size(235, 24), DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 9F) };
            grpRange.Controls.AddRange(new Control[] { lblF, dtpExpFrom, lblT, dtpExpTo, lblB, cboExpBranch });

            // 3. Định dạng xuất
            var grpFormat = new GroupBox { Text = "3. Định dạng tệp xuất", Location = new Point(15, 315), Size = new Size(340, 95), Font = new Font("Segoe UI", 9.5F, FontStyle.Bold) };
            rdoExpCsv = new RadioButton { Text = "📊 Tệp Microsoft Excel (.csv UTF-8 BOM)", Location = new Point(15, 26), Size = new Size(310, 24), Checked = true, Font = new Font("Segoe UI", 9.5F) };
            rdoExpHtml = new RadioButton { Text = "📄 Báo cáo HTML In ấn / Xuất PDF (.html)", Location = new Point(15, 56), Size = new Size(310, 24), Font = new Font("Segoe UI", 9.5F) };
            grpFormat.Controls.AddRange(new Control[] { rdoExpCsv, rdoExpHtml });

            // Buttons
            var btnPreview = new Button
            {
                Text = "🔍 Xem Trước Dữ Liệu (Preview)",
                Location = new Point(15, 425),
                Size = new Size(340, 38),
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnPreview.FlatAppearance.BorderSize = 0;
            btnPreview.Click += (s, e) => LoadExportPreview();

            var btnExecute = new Button
            {
                Text = "🚀 TIẾN HÀNH XUẤT FILE BÁO CÁO",
                Location = new Point(15, 475),
                Size = new Size(340, 50),
                BackColor = Color.FromArgb(230, 126, 34),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnExecute.FlatAppearance.BorderSize = 0;
            btnExecute.Click += (s, e) => ExecuteExportFile();

            grpConfig.Controls.AddRange(new Control[] { grpType, grpRange, grpFormat, btnPreview, btnExecute });

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

        private void ExecuteExportFile()
        {
            try
            {
                int branchId = 0;
                if (cboExpBranch.SelectedValue is int bId) branchId = bId;

                DateTime from = dtpExpFrom.Value;
                DateTime to = dtpExpTo.Value;
                string period = $"{from:dd/MM/yyyy} - {to:dd/MM/yyyy}";
                string branchName = cboExpBranch.Text;
                bool isHtml = rdoExpHtml.Checked;

                DataTable dt;
                string title;
                Dictionary<string, string> kpis = new Dictionary<string, string>();

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
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn để xuất file!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                bool ok = isHtml
                    ? ReportExportHelper.ExportToHtml(dt, title, period, branchName, kpis, out string savedPath)
                    : ReportExportHelper.ExportToCsv(dt, title, period, branchName, kpis, out savedPath);

                if (ok)
                {
                    ReportExportHelper.PromptOpenFile(savedPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                BindCombo(cboRevBranch, dtBranches, "TenChiNhanh", "MaChiNhanh");
                BindCombo(cboTopBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");
                BindCombo(cboInvBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");
                BindCombo(cboChartBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");
                BindCombo(cboExpBranch, dtBranches.Copy(), "TenChiNhanh", "MaChiNhanh");

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

        #region ======================== 5. MODULE NHÂN VIÊN ========================

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
                string query = @"
                    SELECT e.EmployeeID, e.EmployeeCode AS [Mã NV], e.FullName AS [Họ và Tên], 
                           e.Phone AS [SĐT], e.Email, e.Position AS [Chức Vụ], 
                           u.Username AS [Tên Đăng Nhập], r.RoleName AS [Quyền Hạn], 
                           e.Status AS [Trạng Thái]
                    FROM dbo.Employees e
                    LEFT JOIN dbo.Users u ON e.EmployeeID = u.EmployeeID
                    LEFT JOIN dbo.Roles r ON u.RoleID = r.RoleID
                    ORDER BY e.EmployeeID";

                DataTable dt = DatabaseHelper.ExecuteQuery(query);
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
