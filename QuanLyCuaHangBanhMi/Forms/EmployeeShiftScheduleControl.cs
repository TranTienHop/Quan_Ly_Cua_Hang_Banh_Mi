using System;
using System.Data;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Services;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class EmployeeShiftScheduleControl : UserControl
    {
        private bool _loading;

        public EmployeeShiftScheduleControl()
        {
            InitializeComponent();
            cboBranch.SelectedIndexChanged += (_, _) =>
            {
                if (_loading) return;
                LoadEmployees();
                LoadSchedule();
            };
            dtpFrom.ValueChanged += (_, _) => LoadSchedule();
            dtpTo.ValueChanged += (_, _) => LoadSchedule();
            btnReload.Click += (_, _) => LoadData();
            btnAssign.Click += (_, _) => AssignShift();
            btnAddShift.Click += (_, _) => CreateShiftType();
            btnEditShift.Click += (_, _) => EditShiftType();
            btnToggleShift.Click += (_, _) => ToggleShiftType();
            btnCancel.Click += (_, _) => CancelSelectedAssignment();
        }

        public void LoadData()
        {
            if (DesignMode) return;

            try
            {
                _loading = true;
                cboBranch.DataSource = ShiftScheduleService.GetBranches();
                cboBranch.DisplayMember = "HienThi";
                cboBranch.ValueMember = "MaChiNhanh";
                _loading = false;

                if (cboBranch.Items.Count == 0)
                    throw new InvalidOperationException("Không có chi nhánh hoạt động để xếp lịch.");

                LoadShiftTypes();
                LoadEmployees();
                LoadSchedule();
            }
            catch (Exception ex)
            {
                _loading = false;
                MessageBox.Show(ex.Message, "Không thể tải lịch phân ca",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadShiftTypes()
        {
            DataTable shiftTypes = ShiftScheduleService.GetShiftTypes();
            cboShift.DataSource = shiftTypes;
            cboShift.DisplayMember = "HienThi";
            cboShift.ValueMember = "MaCaLam";
            bool hasShiftTypes = shiftTypes.Rows.Count > 0;
            btnAssign.Enabled = hasShiftTypes;
            btnEditShift.Enabled = hasShiftTypes;
            btnToggleShift.Enabled = hasShiftTypes;
        }

        private void LoadEmployees()
        {
            if (cboBranch.SelectedValue is null || cboBranch.SelectedValue is DataRowView)
                return;

            try
            {
                int branchId = Convert.ToInt32(cboBranch.SelectedValue);
                cboEmployee.DataSource = ShiftScheduleService.GetEmployees(branchId);
                cboEmployee.DisplayMember = "HienThi";
                cboEmployee.ValueMember = "MaNhanVien";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể tải nhân viên",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSchedule()
        {
            if (_loading || cboBranch.SelectedValue is null || cboBranch.SelectedValue is DataRowView)
                return;

            try
            {
                int branchId = Convert.ToInt32(cboBranch.SelectedValue);
                DataTable schedule = ShiftScheduleService.GetSchedule(
                    dtpFrom.Value.Date, dtpTo.Value.Date, branchId);
                dgvSchedule.DataSource = schedule;
                HideColumn("MaLich");
                lblCount.Text = cboShift.Items.Count == 0
                    ? "Chưa có danh mục ca làm; hãy tạo ca trước khi phân công."
                    : $"{schedule.Rows.Count:N0} lịch phân ca";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể tải lịch phân ca",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AssignShift()
        {
            if (cboBranch.SelectedValue is null || cboEmployee.SelectedValue is null ||
                cboShift.SelectedValue is null)
            {
                MessageBox.Show("Vui lòng chọn chi nhánh, nhân viên và ca làm.",
                    "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (cboShift.SelectedItem is not DataRowView shift ||
                    !Convert.ToBoolean(shift["DangHoatDong"]))
                    throw new InvalidOperationException("Chỉ có thể phân công ca đang hoạt động.");

                ShiftScheduleService.Assign(
                    Convert.ToInt32(cboEmployee.SelectedValue),
                    Convert.ToInt32(cboShift.SelectedValue),
                    Convert.ToInt32(cboBranch.SelectedValue),
                    dtpWorkDate.Value.Date,
                    txtNote.Text);
                txtNote.Clear();
                if (dtpWorkDate.Value.Date < dtpFrom.Value.Date ||
                    dtpWorkDate.Value.Date > dtpTo.Value.Date)
                {
                    _loading = true;
                    dtpFrom.Value = dtpWorkDate.Value.Date;
                    dtpTo.Value = dtpWorkDate.Value.Date;
                    _loading = false;
                }
                LoadSchedule();
                MessageBox.Show("Đã phân công ca làm.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể phân công ca",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CreateShiftType()
        {
            using Form dialog = CreateShiftTypeDialog(null, out TextBox code, out TextBox name,
                out DateTimePicker start, out DateTimePicker end);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                ShiftScheduleService.CreateShiftType(code.Text, name.Text,
                    start.Value.TimeOfDay, end.Value.TimeOfDay);
                LoadShiftTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể tạo ca làm",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EditShiftType()
        {
            if (cboShift.SelectedItem is not DataRowView selected)
            {
                MessageBox.Show("Vui lòng chọn ca làm cần sửa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using Form dialog = CreateShiftTypeDialog(selected, out TextBox code, out TextBox name,
                out DateTimePicker start, out DateTimePicker end);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                ShiftScheduleService.UpdateShiftType(Convert.ToInt32(selected["MaCaLam"]),
                    code.Text, name.Text, start.Value.TimeOfDay, end.Value.TimeOfDay);
                LoadShiftTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể cập nhật ca làm",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ToggleShiftType()
        {
            if (cboShift.SelectedItem is not DataRowView selected)
            {
                MessageBox.Show("Vui lòng chọn ca làm cần đổi trạng thái.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            bool active = Convert.ToBoolean(selected["DangHoatDong"]);
            string action = active ? "ngừng sử dụng" : "kích hoạt";
            if (MessageBox.Show($"Bạn có chắc muốn {action} ca '{selected["TenCa"]}'?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                ShiftScheduleService.ToggleShiftType(Convert.ToInt32(selected["MaCaLam"]));
                LoadShiftTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể đổi trạng thái ca",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static Form CreateShiftTypeDialog(DataRowView? selected, out TextBox code,
            out TextBox name, out DateTimePicker start, out DateTimePicker end)
        {
            var dialog = new Form
            {
                Text = selected is null ? "Thêm ca làm" : "Sửa ca làm",
                ClientSize = new System.Drawing.Size(460, 280),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterParent,
                Font = new System.Drawing.Font("Segoe UI", 9.5F)
            };
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Padding = new Padding(16)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 5; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));

            code = new TextBox { Dock = DockStyle.Fill, MaxLength = 30 };
            name = new TextBox { Dock = DockStyle.Fill, MaxLength = 100 };
            start = CreateTimePicker();
            end = CreateTimePicker();
            if (selected is not null)
            {
                code.Text = Convert.ToString(selected["MaCode"]);
                name.Text = Convert.ToString(selected["TenCa"]);
                start.Value = DateTime.Today.Add((TimeSpan)selected["GioBatDau"]);
                end.Value = DateTime.Today.Add((TimeSpan)selected["GioKetThuc"]);
            }

            AddRow(layout, 0, "Mã ca *", code);
            AddRow(layout, 1, "Tên ca *", name);
            AddRow(layout, 2, "Giờ bắt đầu *", start);
            AddRow(layout, 3, "Giờ kết thúc *", end);

            var buttons = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft
            };
            var save = new Button { Text = "Lưu", DialogResult = DialogResult.OK, AutoSize = true };
            var cancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, AutoSize = true };
            buttons.Controls.Add(save);
            buttons.Controls.Add(cancel);
            layout.Controls.Add(buttons, 1, 4);
            dialog.AcceptButton = save;
            dialog.CancelButton = cancel;
            dialog.Controls.Add(layout);
            return dialog;
        }

        private static DateTimePicker CreateTimePicker() => new()
        {
            Dock = DockStyle.Left,
            Format = DateTimePickerFormat.Time,
            ShowUpDown = true,
            Value = DateTime.Today.AddHours(8)
        };

        private static void AddRow(TableLayoutPanel layout, int row, string text, Control control)
        {
            layout.Controls.Add(new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            }, 0, row);
            control.Margin = new Padding(3, 7, 3, 7);
            layout.Controls.Add(control, 1, row);
        }

        private void CancelSelectedAssignment()
        {
            if (dgvSchedule.CurrentRow?.DataBoundItem is not DataRowView row)
            {
                MessageBox.Show("Vui lòng chọn lịch cần hủy.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!string.Equals(Convert.ToString(row["TrangThai"]), "DaXep", StringComparison.Ordinal))
            {
                MessageBox.Show("Chỉ lịch đang ở trạng thái Đã xếp mới được hủy.",
                    "Không thể hủy", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn hủy lịch phân ca đã chọn?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                ShiftScheduleService.Cancel(Convert.ToInt32(row["MaLich"]));
                LoadSchedule();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể hủy lịch",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void HideColumn(string name)
        {
            if (dgvSchedule.Columns[name] is DataGridViewColumn column)
                column.Visible = false;
        }
    }
}
