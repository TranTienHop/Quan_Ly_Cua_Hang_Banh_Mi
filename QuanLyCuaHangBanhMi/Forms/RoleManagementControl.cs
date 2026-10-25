using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class RoleManagementControl : UserControl
    {
        private readonly System.Windows.Forms.Timer _searchTimer = new() { Interval = 300 };
        private readonly List<RoleMenuOption> _menuOptions = new();
        private bool _loadingSelection;

        public RoleManagementControl()
        {
            InitializeComponent();
            _searchTimer.Tick += (_, _) => { _searchTimer.Stop(); LoadRoles(); };
            txtSearch.TextChanged += (_, _) => { _searchTimer.Stop(); _searchTimer.Start(); };
            btnReload.Click += (_, _) => LoadData();
            btnAdd.Click += (_, _) => AddRole();
            btnEdit.Click += (_, _) => EditSelectedRole();
            btnSavePermissions.Click += (_, _) => SaveSelectedPermissions();
            dgvRoles.SelectionChanged += (_, _) => LoadSelectedPermissions();
            dgvRoles.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) EditSelectedRole(); };
        }

        public void LoadData()
        {
            if (DesignMode) return;
            try
            {
                LoadMenus();
                LoadRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được dữ liệu vai trò: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMenus()
        {
            _menuOptions.Clear();
            DataTable menus = RoleService.GetMenus();
            foreach (DataRow row in menus.Rows)
            {
                _menuOptions.Add(new RoleMenuOption(
                    Convert.ToInt32(row["MaMenu"]),
                    Convert.ToString(row["MaCode"]) ?? string.Empty,
                    Convert.ToString(row["TenHienThi"]) ?? string.Empty,
                    Convert.ToString(row["TenNhom"]) ?? string.Empty));
            }
        }

        private void LoadRoles()
        {
            int? selectedId = GetSelectedRoleId();
            _loadingSelection = true;
            DataTable roles = RoleService.GetAll(txtSearch.Text.Trim());
            dgvRoles.DataSource = roles;
            HideColumn("MaVaiTro");
            SetHeader("MaCode", "Mã vai trò");
            SetHeader("TenVaiTro", "Tên vai trò");
            SetHeader("MoTa", "Mô tả");
            SetHeader("SoQuyen", "Số quyền");
            SetHeader("SoTaiKhoan", "Tài khoản");
            FormatGrid();
            lblRecordCount.Text = $"{roles.Rows.Count:N0} vai trò";
            RestoreSelection(selectedId);
            _loadingSelection = false;
            LoadSelectedPermissions();
        }

        private void LoadSelectedPermissions()
        {
            if (_loadingSelection) return;
            clbPermissions.Items.Clear();
            if (!TryGetSelectedRole(out int roleId, out string code, out string name, out _))
            {
                lblSelectedRole.Text = "Chọn vai trò để xem quyền";
                btnSavePermissions.Enabled = false;
                return;
            }

            try
            {
                var assigned = RoleService.GetPermissionIds(roleId).ToHashSet();
                foreach (RoleMenuOption option in _menuOptions)
                    clbPermissions.Items.Add(option, assigned.Contains(option.Id));
                lblSelectedRole.Text = $"{name} ({code})";
                btnSavePermissions.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tải được phân quyền: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddRole()
        {
            using Form dialog = CreateRoleDialog("Thêm vai trò", true, null, null, null,
                Array.Empty<int>(), out TextBox txtCode, out TextBox txtName,
                out TextBox txtDescription, out CheckedListBox permissions);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                RoleService.Create(new RoleCreateModel
                {
                    Code = txtCode.Text.Trim(),
                    Name = txtName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    PermissionIds = GetCheckedPermissionIds(permissions)
                });
                LoadRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể thêm vai trò",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void EditSelectedRole()
        {
            if (!TryGetSelectedRole(out int roleId, out string code, out string name, out string description))
            {
                ShowRoleSelectionRequired();
                return;
            }
            IReadOnlyList<int> assigned;
            try
            {
                assigned = RoleService.GetPermissionIds(roleId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không tải được phân quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using Form dialog = CreateRoleDialog("Cập nhật vai trò", false, code, name, description,
                assigned, out _, out TextBox txtName, out TextBox txtDescription, out CheckedListBox permissions);
            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                RoleService.Update(new RoleUpdateModel
                {
                    RoleId = roleId,
                    Name = txtName.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    PermissionIds = GetCheckedPermissionIds(permissions)
                });
                LoadRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể cập nhật vai trò",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveSelectedPermissions()
        {
            if (!TryGetSelectedRole(out int roleId, out _, out string name, out string description))
            {
                ShowRoleSelectionRequired();
                return;
            }
            try
            {
                RoleService.Update(new RoleUpdateModel
                {
                    RoleId = roleId,
                    Name = name,
                    Description = description,
                    PermissionIds = GetCheckedPermissionIds(clbPermissions)
                });
                MessageBox.Show("Đã lưu phân quyền.", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRoles();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể lưu phân quyền",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool TryGetSelectedRole(out int roleId, out string code, out string name, out string description)
        {
            roleId = 0;
            code = name = description = string.Empty;
            if (dgvRoles.CurrentRow?.DataBoundItem is not DataRowView row)
                return false;

            roleId = Convert.ToInt32(row["MaVaiTro"]);
            code = Convert.ToString(row["MaCode"]) ?? string.Empty;
            name = Convert.ToString(row["TenVaiTro"]) ?? string.Empty;
            description = Convert.ToString(row["MoTa"]) ?? string.Empty;
            return true;
        }

        private void ShowRoleSelectionRequired()
        {
            lblSelectedRole.Text = "Vui lòng chọn một vai trò.";
            dgvRoles.Focus();
        }

        private int? GetSelectedRoleId() =>
            dgvRoles.CurrentRow?.DataBoundItem is DataRowView row
                ? Convert.ToInt32(row["MaVaiTro"])
                : null;

        private void RestoreSelection(int? roleId)
        {
            if (roleId == null || dgvRoles.Rows.Count == 0) return;
            foreach (DataGridViewRow row in dgvRoles.Rows)
            {
                if (row.DataBoundItem is DataRowView data && Convert.ToInt32(data["MaVaiTro"]) == roleId)
                {
                    row.Selected = true;
                    dgvRoles.CurrentCell = row.Cells.Cast<DataGridViewCell>().FirstOrDefault(c => c.Visible);
                    break;
                }
            }
        }

        private static IReadOnlyList<int> GetCheckedPermissionIds(CheckedListBox list) =>
            list.CheckedItems.Cast<RoleMenuOption>().Select(item => item.Id).ToArray();

        private Form CreateRoleDialog(string title, bool allowCode, string? code, string? name,
            string? description, IEnumerable<int> assignedIds, out TextBox txtCode, out TextBox txtName,
            out TextBox txtDescription, out CheckedListBox permissions)
        {
            var dialog = new Form
            {
                Text = title,
                ClientSize = new Size(620, 600),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterParent,
                Font = new Font("Segoe UI", 9.5F)
            };
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(18)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 125));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 82));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            dialog.Controls.Add(layout);

            txtCode = new TextBox { Dock = DockStyle.Fill, Text = code ?? string.Empty, MaxLength = 30, ReadOnly = !allowCode };
            txtName = new TextBox { Dock = DockStyle.Fill, Text = name ?? string.Empty, MaxLength = 100 };
            txtDescription = new TextBox { Dock = DockStyle.Fill, Text = description ?? string.Empty, MaxLength = 255, Multiline = true };
            permissions = new CheckedListBox { Dock = DockStyle.Fill, CheckOnClick = true, IntegralHeight = false };

            var assigned = assignedIds.ToHashSet();
            foreach (RoleMenuOption option in _menuOptions)
                permissions.Items.Add(option, assigned.Contains(option.Id));

            AddDialogControl(layout, 0, "Mã vai trò *", txtCode);
            AddDialogControl(layout, 1, "Tên vai trò *", txtName);
            AddDialogControl(layout, 2, "Mô tả", txtDescription);
            AddDialogControl(layout, 3, "Quyền chức năng", permissions);

            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var save = new Button { Text = "Lưu", DialogResult = DialogResult.OK, Width = 95, Height = 34 };
            var cancel = new Button { Text = "Hủy", DialogResult = DialogResult.Cancel, Width = 95, Height = 34 };
            buttons.Controls.Add(save);
            buttons.Controls.Add(cancel);
            layout.Controls.Add(buttons, 0, 4);
            layout.SetColumnSpan(buttons, 2);
            dialog.AcceptButton = save;
            dialog.CancelButton = cancel;
            return dialog;
        }

        private static void AddDialogControl(TableLayoutPanel layout, int row, string label, Control control)
        {
            layout.Controls.Add(new Label
            {
                Text = label,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            }, 0, row);
            control.Margin = new Padding(3, 7, 3, 7);
            layout.Controls.Add(control, 1, row);
        }

        private void HideColumn(string name)
        {
            DataGridViewColumn? column = dgvRoles.Columns[name];
            if (column != null) column.Visible = false;
        }

        private void SetHeader(string name, string text)
        {
            DataGridViewColumn? column = dgvRoles.Columns[name];
            if (column != null) column.HeaderText = text;
        }

        private void FormatGrid()
        {
            dgvRoles.EnableHeadersVisualStyles = false;
            dgvRoles.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dgvRoles.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRoles.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgvRoles.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }

        private sealed record RoleMenuOption(int Id, string Code, string Name, string Group)
        {
            public override string ToString() => $"{(string.IsNullOrWhiteSpace(Group) ? "Khác" : Group)} · {Name} ({Code})";
        }
    }
}
