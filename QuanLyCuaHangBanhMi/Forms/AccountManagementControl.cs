using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class AccountManagementControl : UserControl
    {
        private readonly System.Windows.Forms.Timer _searchTimer = new() { Interval = 300 };

        public AccountManagementControl()
        {
            InitializeComponent();
            cboStatus.Items.AddRange(new object[] { "Tất cả trạng thái", "Đang hoạt động", "Đã khóa" });
            cboStatus.SelectedIndex = 0;

            _searchTimer.Tick += (_, _) => { _searchTimer.Stop(); LoadData(); };
            txtSearch.TextChanged += (_, _) => { _searchTimer.Stop(); _searchTimer.Start(); };
            cboStatus.SelectedIndexChanged += (_, _) => LoadData();
            btnReload.Click += (_, _) => LoadData();
            btnAdd.Click += (_, _) => CreateAccount();
            btnResetPassword.Click += (_, _) => ResetSelectedPassword();
            btnToggleStatus.Click += (_, _) => ToggleSelectedStatus();
            dgvAccounts.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) ResetSelectedPassword(); };
        }

        public void LoadData()
        {
            if (DesignMode) return;
            try
            {
                bool? active = cboStatus.SelectedIndex switch { 1 => true, 2 => false, _ => null };
                DataTable accounts = AccountService.GetAll(txtSearch.Text.Trim(), active);
                dgvAccounts.DataSource = accounts;
                HideColumn("MaTaiKhoan");
                HideColumn("MaNhanVien");
                HideColumn("MaVaiTro");
                HideColumn("MaChiNhanh");
                FormatColumns();
                lblRecordCount.Text = $"{accounts.Rows.Count:N0} tài khoản";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách tài khoản: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateAccount()
        {
            try
            {
                DataTable employees = AccountService.GetEmployeesWithoutAccount();
                if (employees.Rows.Count == 0)
                {
                    MessageBox.Show("Không còn nhân viên hoạt động nào chưa có tài khoản.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataTable roles = AccountService.GetRoles();
                using Form dialog = CreateAccountDialog(employees, roles,
                    out ComboBox cboEmployee, out ComboBox cboRole,
                    out TextBox txtUsername, out TextBox txtPassword,
                    out TextBox txtConfirm, out CheckBox chkActive);

                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                if (cboEmployee.SelectedValue == null || cboRole.SelectedValue == null)
                    throw new InvalidOperationException("Vui lòng chọn nhân viên và vai trò.");
                if (txtPassword.Text != txtConfirm.Text)
                    throw new InvalidOperationException("Mật khẩu xác nhận không khớp.");

                AccountService.Create(new AccountCreateModel
                {
                    EmployeeId = Convert.ToInt32(cboEmployee.SelectedValue),
                    RoleId = Convert.ToInt32(cboRole.SelectedValue),
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Text,
                    IsActive = chkActive.Checked
                });

                MessageBox.Show("Đã tạo tài khoản thành công.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể tạo tài khoản",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ResetSelectedPassword()
        {
            if (!TryGetSelectedAccount(out int accountId, out string username, out _)) return;
            using Form dialog = CreatePasswordDialog(username, out TextBox txtPassword, out TextBox txtConfirm);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                AccountService.ResetPassword(accountId, txtPassword.Text);
                MessageBox.Show($"Đã đặt lại mật khẩu cho tài khoản '{username}'.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể đặt lại mật khẩu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ToggleSelectedStatus()
        {
            if (!TryGetSelectedAccount(out int accountId, out string username, out bool isActive)) return;
            string action = isActive ? "khóa" : "mở khóa";
            if (MessageBox.Show($"Bạn có chắc muốn {action} tài khoản '{username}'?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                AccountService.ToggleStatus(accountId);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể thay đổi trạng thái",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool TryGetSelectedAccount(out int accountId, out string username, out bool isActive)
        {
            accountId = 0;
            username = string.Empty;
            isActive = false;
            if (dgvAccounts.CurrentRow?.DataBoundItem is not DataRowView row)
            {
                MessageBox.Show("Vui lòng chọn một tài khoản trong danh sách.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            accountId = Convert.ToInt32(row["MaTaiKhoan"]);
            username = Convert.ToString(row["TenDangNhap"]) ?? string.Empty;
            isActive = Convert.ToBoolean(row["DangHoatDong"]);
            return true;
        }

        private void HideColumn(string name)
        {
            DataGridViewColumn? column = dgvAccounts.Columns[name];
            if (column != null) column.Visible = false;
        }

        private void FormatColumns()
        {
            dgvAccounts.EnableHeadersVisualStyles = false;
            dgvAccounts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvAccounts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvAccounts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvAccounts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            SetHeader("TenDangNhap", "Tên đăng nhập");
            SetHeader("MaCodeNhanVien", "Mã NV");
            SetHeader("HoTen", "Nhân viên");
            SetHeader("TenVaiTro", "Vai trò");
            SetHeader("TenChiNhanh", "Chi nhánh");
            SetHeader("DangHoatDong", "Hoạt động");
            SetHeader("LanDangNhapCuoi", "Đăng nhập cuối");
        }

        private void SetHeader(string columnName, string header)
        {
            DataGridViewColumn? column = dgvAccounts.Columns[columnName];
            if (column != null) column.HeaderText = header;
        }

        private static Form CreateAccountDialog(DataTable employees, DataTable roles,
            out ComboBox cboEmployee, out ComboBox cboRole, out TextBox txtUsername,
            out TextBox txtPassword, out TextBox txtConfirm, out CheckBox chkActive)
        {
            var dialog = CreateDialog("Thêm tài khoản", 510, 390);
            var layout = CreateDialogLayout();
            dialog.Controls.Add(layout);
            cboEmployee = CreateCombo(employees, "HienThi", "MaNhanVien");
            cboRole = CreateCombo(roles, "HienThi", "MaVaiTro");
            txtUsername = new TextBox { Dock = DockStyle.Fill, MaxLength = 50 };
            txtPassword = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true, MaxLength = 100 };
            txtConfirm = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true, MaxLength = 100 };
            chkActive = new CheckBox { Text = "Cho phép đăng nhập", Checked = true, AutoSize = true };
            AddDialogRow(layout, 0, "Nhân viên *", cboEmployee);
            AddDialogRow(layout, 1, "Vai trò *", cboRole);
            AddDialogRow(layout, 2, "Tên đăng nhập *", txtUsername);
            AddDialogRow(layout, 3, "Mật khẩu *", txtPassword);
            AddDialogRow(layout, 4, "Xác nhận *", txtConfirm);
            AddDialogRow(layout, 5, "Trạng thái", chkActive);
            AddDialogButtons(dialog, layout, 6);
            return dialog;
        }

        private static Form CreatePasswordDialog(string username, out TextBox txtPassword, out TextBox txtConfirm)
        {
            var dialog = CreateDialog("Đặt lại mật khẩu", 470, 245);
            var layout = CreateDialogLayout();
            dialog.Controls.Add(layout);
            txtPassword = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true, MaxLength = 100 };
            txtConfirm = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = true, MaxLength = 100 };
            AddDialogRow(layout, 0, "Tài khoản", new Label { Text = username, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) });
            AddDialogRow(layout, 1, "Mật khẩu mới *", txtPassword);
            AddDialogRow(layout, 2, "Xác nhận *", txtConfirm);
            AddDialogButtons(dialog, layout, 3);
            return dialog;
        }

        private static Form CreateDialog(string title, int width, int height) => new()
        {
            Text = title, ClientSize = new Size(width, height), FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false, MinimizeBox = false, StartPosition = FormStartPosition.CenterParent,
            Font = new Font("Segoe UI", 9.5F)
        };

        private static TableLayoutPanel CreateDialogLayout()
        {
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(18), AutoScroll = true };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            return layout;
        }

        private static ComboBox CreateCombo(DataTable data, string displayMember, string valueMember) => new()
        {
            Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList,
            DataSource = data, DisplayMember = displayMember, ValueMember = valueMember
        };

        private static void AddDialogRow(TableLayoutPanel layout, int row, string label, Control control)
        {
            layout.RowCount = Math.Max(layout.RowCount, row + 1);
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
            layout.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            control.Margin = new Padding(3, 7, 3, 7);
            layout.Controls.Add(control, 1, row);
        }

        private static void AddDialogButtons(Form dialog, TableLayoutPanel layout, int row)
        {
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var save = new Button { Text = "Lưu", DialogResult = DialogResult.OK, Width = 95, Height = 34 };
            var cancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 95, Height = 34 };
            buttons.Controls.Add(save);
            buttons.Controls.Add(cancel);
            layout.RowCount = Math.Max(layout.RowCount, row + 1);
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.Controls.Add(buttons, 0, row);
            layout.SetColumnSpan(buttons, 2);
            dialog.AcceptButton = save;
            dialog.CancelButton = cancel;
        }
    }
}
