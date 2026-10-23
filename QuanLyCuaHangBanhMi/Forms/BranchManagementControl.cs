using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class BranchManagementControl : UserControl
    {
        private readonly System.Windows.Forms.Timer _searchTimer = new() { Interval = 300 };

        public BranchManagementControl()
        {
            InitializeComponent();
            cboType.Items.AddRange(new object[] { "Tất cả loại", "Cửa hàng", "Lò / kho tổng" });
            cboStatus.Items.AddRange(new object[] { "Tất cả trạng thái", "Đang hoạt động", "Đã khóa" });
            cboType.SelectedIndex = 0;
            cboStatus.SelectedIndex = 0;
            _searchTimer.Tick += (_, _) => { _searchTimer.Stop(); LoadData(); };
            txtSearch.TextChanged += (_, _) => { _searchTimer.Stop(); _searchTimer.Start(); };
            cboType.SelectedIndexChanged += (_, _) => LoadData();
            cboStatus.SelectedIndexChanged += (_, _) => LoadData();
            btnReload.Click += (_, _) => LoadData();
            btnAdd.Click += (_, _) => AddBranch();
            btnEdit.Click += (_, _) => EditSelectedBranch();
            btnToggleStatus.Click += (_, _) => ToggleSelectedStatus();
            dgvBranches.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) EditSelectedBranch(); };
        }

        public void LoadData()
        {
            if (DesignMode) return;
            try
            {
                byte? type = cboType.SelectedIndex switch { 1 => (byte)1, 2 => (byte)2, _ => null };
                bool? active = cboStatus.SelectedIndex switch { 1 => true, 2 => false, _ => null };
                DataTable branches = BranchService.GetAll(txtSearch.Text.Trim(), active, type);
                dgvBranches.DataSource = branches;
                HideColumn("MaChiNhanh");
                HideColumn("LoaiChiNhanh");
                SetHeader("MaCode", "Mã CN");
                SetHeader("TenChiNhanh", "Tên chi nhánh");
                SetHeader("TenLoai", "Loại");
                SetHeader("DiaChi", "Địa chỉ");
                SetHeader("SoDienThoai", "Điện thoại");
                SetHeader("DangHoatDong", "Hoạt động");
                SetHeader("SoNhanVien", "Nhân viên");
                SetHeader("SoCaDangMo", "Ca đang mở");
                FormatGrid();
                lblRecordCount.Text = $"{branches.Rows.Count:N0} chi nhánh";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách chi nhánh: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddBranch()
        {
            using Form dialog = CreateBranchDialog("Thêm chi nhánh", true, null,
                out TextBox txtCode, out TextBox txtName, out ComboBox cboBranchType,
                out TextBox txtAddress, out TextBox txtPhone, out CheckBox chkActive);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                BranchService.Create(new BranchCreateModel
                {
                    Code = txtCode.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Type = Convert.ToByte(cboBranchType.SelectedValue),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    IsActive = chkActive.Checked
                });
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể thêm chi nhánh",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EditSelectedBranch()
        {
            if (!TryGetSelected(out DataRowView row)) return;
            using Form dialog = CreateBranchDialog("Cập nhật chi nhánh", false, row,
                out _, out TextBox txtName, out ComboBox cboBranchType,
                out TextBox txtAddress, out TextBox txtPhone, out _);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                BranchService.Update(new BranchUpdateModel
                {
                    BranchId = Convert.ToInt32(row["MaChiNhanh"]),
                    Name = txtName.Text.Trim(),
                    Type = Convert.ToByte(cboBranchType.SelectedValue),
                    Address = txtAddress.Text.Trim(),
                    Phone = txtPhone.Text.Trim()
                });
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể cập nhật chi nhánh",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ToggleSelectedStatus()
        {
            if (!TryGetSelected(out DataRowView row)) return;
            int branchId = Convert.ToInt32(row["MaChiNhanh"]);
            string name = Convert.ToString(row["TenChiNhanh"]) ?? string.Empty;
            bool active = Convert.ToBoolean(row["DangHoatDong"]);
            string action = active ? "khóa" : "mở khóa";
            if (MessageBox.Show($"Bạn có chắc muốn {action} chi nhánh '{name}'?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                BranchService.ToggleStatus(branchId);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể thay đổi trạng thái",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool TryGetSelected(out DataRowView row)
        {
            if (dgvBranches.CurrentRow?.DataBoundItem is DataRowView selected)
            {
                row = selected;
                return true;
            }
            row = null!;
            MessageBox.Show("Vui lòng chọn một chi nhánh.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        private static Form CreateBranchDialog(string title, bool allowCode, DataRowView? row,
            out TextBox txtCode, out TextBox txtName, out ComboBox cboBranchType,
            out TextBox txtAddress, out TextBox txtPhone, out CheckBox chkActive)
        {
            var dialog = new Form
            {
                Text = title, ClientSize = new Size(560, 430), FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false, MinimizeBox = false, StartPosition = FormStartPosition.CenterParent,
                Font = new Font("Segoe UI", 9.5F)
            };
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(18) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            dialog.Controls.Add(layout);

            txtCode = new TextBox { Dock = DockStyle.Fill, MaxLength = 20, ReadOnly = !allowCode, Text = row == null ? "" : Convert.ToString(row["MaCode"]) };
            txtName = new TextBox { Dock = DockStyle.Fill, MaxLength = 200, Text = row == null ? "" : Convert.ToString(row["TenChiNhanh"]) };
            cboBranchType = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cboBranchType.DataSource = new[] { new { Id = (byte)1, Name = "Cửa hàng" }, new { Id = (byte)2, Name = "Lò / kho tổng" } };
            cboBranchType.ValueMember = "Id";
            cboBranchType.DisplayMember = "Name";
            cboBranchType.SelectedValue = row == null ? (byte)1 : Convert.ToByte(row["LoaiChiNhanh"]);
            txtAddress = new TextBox { Dock = DockStyle.Fill, MaxLength = 255, Multiline = true, Text = row == null ? "" : Convert.ToString(row["DiaChi"]) };
            txtPhone = new TextBox { Dock = DockStyle.Fill, MaxLength = 20, Text = row == null ? "" : Convert.ToString(row["SoDienThoai"]) };
            chkActive = new CheckBox { Text = "Đang hoạt động", AutoSize = true, Checked = row == null || Convert.ToBoolean(row["DangHoatDong"]), Enabled = allowCode };
            AddRow(layout, 0, "Mã chi nhánh *", txtCode, 45);
            AddRow(layout, 1, "Tên chi nhánh *", txtName, 45);
            AddRow(layout, 2, "Loại *", cboBranchType, 45);
            AddRow(layout, 3, "Địa chỉ", txtAddress, 85);
            AddRow(layout, 4, "Điện thoại", txtPhone, 45);
            AddRow(layout, 5, "Trạng thái", chkActive, 45);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var save = new Button { Text = "Lưu", DialogResult = DialogResult.OK, Width = 95, Height = 34 };
            var cancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 95, Height = 34 };
            buttons.Controls.Add(save);
            buttons.Controls.Add(cancel);
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.Controls.Add(buttons, 0, 6);
            layout.SetColumnSpan(buttons, 2);
            dialog.AcceptButton = save;
            dialog.CancelButton = cancel;
            return dialog;
        }

        private static void AddRow(TableLayoutPanel layout, int row, string label, Control control, int height)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
            layout.Controls.Add(new Label { Text = label, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, row);
            control.Margin = new Padding(3, 7, 3, 7);
            layout.Controls.Add(control, 1, row);
        }

        private void HideColumn(string name)
        {
            DataGridViewColumn? column = dgvBranches.Columns[name];
            if (column != null) column.Visible = false;
        }

        private void SetHeader(string name, string text)
        {
            DataGridViewColumn? column = dgvBranches.Columns[name];
            if (column != null) column.HeaderText = text;
        }

        private void FormatGrid()
        {
            dgvBranches.EnableHeadersVisualStyles = false;
            dgvBranches.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvBranches.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBranches.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvBranches.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }
    }
}
