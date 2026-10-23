using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class ShiftManagementControl : UserControl
    {
        private bool _initialized;

        public ShiftManagementControl()
        {
            InitializeComponent();
            dtpFrom.Value = DateTime.Today.AddDays(-30);
            dtpTo.Value = DateTime.Today;
            cboStatus.Items.AddRange(new object[] { "Tất cả trạng thái", "Đang mở", "Đã đóng", "Đã duyệt" });
            cboStatus.SelectedIndex = 0;
            dtpFrom.ValueChanged += (_, _) => ReloadIfInitialized();
            dtpTo.ValueChanged += (_, _) => ReloadIfInitialized();
            cboBranch.SelectedIndexChanged += (_, _) => ReloadIfInitialized();
            cboStatus.SelectedIndexChanged += (_, _) => ReloadIfInitialized();
            btnReload.Click += (_, _) => LoadData();
            btnOpen.Click += (_, _) => OpenShift();
            btnClose.Click += (_, _) => CloseSelectedShift();
            btnApprove.Click += (_, _) => ApproveSelectedShift();
        }

        public void LoadData()
        {
            if (DesignMode) return;
            try
            {
                if (!_initialized)
                {
                    BindBranches();
                    _initialized = true;
                }

                int selectedBranch = ReadSelectedInt(cboBranch);
                string? status = cboStatus.SelectedIndex switch
                {
                    1 => "DangMo",
                    2 => "DaDong",
                    3 => "DaDuyet",
                    _ => null
                };
                DataTable shifts = ShiftService.GetAll(dtpFrom.Value.Date, dtpTo.Value.Date,
                    selectedBranch > 0 ? selectedBranch : null, status);
                dgvShifts.DataSource = shifts;
                HideColumn("MaChiNhanh");
                HideColumn("MaNhanVien");
                SetHeader("MaCa", "Mã ca");
                SetHeader("TenChiNhanh", "Chi nhánh");
                SetHeader("MaCodeNhanVien", "Mã NV");
                SetHeader("HoTen", "Nhân viên");
                SetHeader("GioMo", "Giờ mở");
                SetHeader("GioDong", "Giờ đóng");
                SetHeader("TienDauCa", "Đầu ca");
                SetHeader("TienHeThong", "Hệ thống");
                SetHeader("TienThucDem", "Thực đếm");
                SetHeader("ChenhLech", "Chênh lệch");
                SetHeader("TrangThai", "Trạng thái");
                SetHeader("GhiChu", "Ghi chú");
                HideColumn("TienCuoiCa");
                FormatGrid();
                lblRecordCount.Text = $"{shifts.Rows.Count:N0} ca làm";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được danh sách ca làm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindBranches()
        {
            DataTable source = ShiftService.GetBranches();
            DataTable choices = source.Clone();
            choices.Rows.Add(0, "Tất cả chi nhánh");
            foreach (DataRow row in source.Rows)
                choices.ImportRow(row);
            cboBranch.DataSource = choices;
            cboBranch.DisplayMember = "HienThi";
            cboBranch.ValueMember = "MaChiNhanh";
        }

        private void OpenShift()
        {
            try
            {
                DataTable branches = ShiftService.GetBranches();
                using Form dialog = CreateOpenDialog(branches, out ComboBox branch,
                    out ComboBox employee, out NumericUpDown openingCash, out TextBox note);
                void LoadEmployees()
                {
                    int branchId = ReadSelectedInt(branch);
                    employee.DataSource = branchId > 0 ? ShiftService.GetEmployees(branchId) : null;
                    employee.DisplayMember = "HienThi";
                    employee.ValueMember = "MaNhanVien";
                }
                branch.SelectedIndexChanged += (_, _) => LoadEmployees();
                LoadEmployees();
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                ShiftService.Open(new ShiftOpenModel
                {
                    BranchId = ReadSelectedInt(branch),
                    EmployeeId = ReadSelectedInt(employee),
                    OpeningCash = openingCash.Value,
                    Note = note.Text.Trim()
                });
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể mở ca",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CloseSelectedShift()
        {
            if (!TryGetSelectedShift(out int shiftId, out string status)) return;
            if (!string.Equals(status, "DangMo", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Chỉ ca đang mở mới có thể đóng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using Form dialog = CreateCloseDialog(out NumericUpDown countedCash, out TextBox note);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;
            try
            {
                ShiftService.Close(new ShiftCloseModel
                {
                    ShiftId = shiftId,
                    CountedCash = countedCash.Value,
                    Note = note.Text.Trim()
                });
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể đóng ca",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ApproveSelectedShift()
        {
            if (!TryGetSelectedShift(out int shiftId, out string status)) return;
            if (!string.Equals(status, "DaDong", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Chỉ ca đã đóng mới có thể duyệt.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Xác nhận duyệt ca đã chọn?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                ShiftService.Approve(shiftId);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể duyệt ca",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool TryGetSelectedShift(out int shiftId, out string status)
        {
            shiftId = 0;
            status = string.Empty;
            if (dgvShifts.CurrentRow?.DataBoundItem is not DataRowView row)
            {
                MessageBox.Show("Vui lòng chọn một ca làm.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            shiftId = Convert.ToInt32(row["MaCa"]);
            status = Convert.ToString(row["TrangThai"]) ?? string.Empty;
            return true;
        }

        private static Form CreateOpenDialog(DataTable branches, out ComboBox branch,
            out ComboBox employee, out NumericUpDown openingCash, out TextBox note)
        {
            Form dialog = CreateDialog("Mở ca làm", 540, 360);
            TableLayoutPanel layout = CreateDialogLayout();
            dialog.Controls.Add(layout);
            branch = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList, DataSource = branches, DisplayMember = "HienThi", ValueMember = "MaChiNhanh" };
            employee = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            openingCash = CreateMoneyInput();
            note = new TextBox { Dock = DockStyle.Fill, MaxLength = 255, Multiline = true };
            AddDialogRow(layout, 0, "Chi nhánh *", branch, 45);
            AddDialogRow(layout, 1, "Nhân viên *", employee, 45);
            AddDialogRow(layout, 2, "Tiền đầu ca *", openingCash, 45);
            AddDialogRow(layout, 3, "Ghi chú", note, 80);
            AddDialogButtons(dialog, layout, 4);
            return dialog;
        }

        private static Form CreateCloseDialog(out NumericUpDown countedCash, out TextBox note)
        {
            Form dialog = CreateDialog("Đóng ca làm", 520, 265);
            TableLayoutPanel layout = CreateDialogLayout();
            dialog.Controls.Add(layout);
            countedCash = CreateMoneyInput();
            note = new TextBox { Dock = DockStyle.Fill, MaxLength = 255, Multiline = true };
            AddDialogRow(layout, 0, "Tiền thực đếm *", countedCash, 45);
            AddDialogRow(layout, 1, "Ghi chú", note, 80);
            AddDialogButtons(dialog, layout, 2);
            return dialog;
        }

        private static NumericUpDown CreateMoneyInput() => new()
        {
            Dock = DockStyle.Fill,
            DecimalPlaces = 0,
            ThousandsSeparator = true,
            Maximum = 999999999999999m,
            Minimum = 0m,
            Increment = 10000m
        };

        private static Form CreateDialog(string title, int width, int height) => new()
        {
            Text = title, ClientSize = new Size(width, height), FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false, MinimizeBox = false, StartPosition = FormStartPosition.CenterParent,
            Font = new Font("Segoe UI", 9.5F)
        };

        private static TableLayoutPanel CreateDialogLayout()
        {
            var layout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(18) };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            return layout;
        }

        private static void AddDialogRow(TableLayoutPanel layout, int row, string label, Control control, int height)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
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
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            layout.Controls.Add(buttons, 0, row);
            layout.SetColumnSpan(buttons, 2);
            dialog.AcceptButton = save;
            dialog.CancelButton = cancel;
        }

        private void ReloadIfInitialized()
        {
            if (_initialized) LoadData();
        }

        private static int ReadSelectedInt(ComboBox combo) =>
            combo.SelectedValue == null || combo.SelectedValue == DBNull.Value
                ? 0
                : Convert.ToInt32(combo.SelectedValue);

        private void HideColumn(string name)
        {
            DataGridViewColumn? column = dgvShifts.Columns[name];
            if (column != null) column.Visible = false;
        }

        private void SetHeader(string name, string text)
        {
            DataGridViewColumn? column = dgvShifts.Columns[name];
            if (column != null) column.HeaderText = text;
        }

        private void FormatGrid()
        {
            dgvShifts.EnableHeadersVisualStyles = false;
            dgvShifts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvShifts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvShifts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvShifts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            foreach (string name in new[] { "TienDauCa", "TienHeThong", "TienThucDem", "ChenhLech" })
            {
                DataGridViewColumn? column = dgvShifts.Columns[name];
                if (column != null) column.DefaultCellStyle.Format = "N0";
            }
            foreach (string name in new[] { "GioMo", "GioDong" })
            {
                DataGridViewColumn? column = dgvShifts.Columns[name];
                if (column != null) column.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
        }
    }
}
