namespace QuanLyCuaHangBanhMi.Forms
{
    partial class RoleManagementControl
    {
        private System.ComponentModel.IContainer components = null!;
        private System.Windows.Forms.TableLayoutPanel rootLayout = null!;
        private System.Windows.Forms.Label lblTitle = null!;
        private System.Windows.Forms.FlowLayoutPanel toolbar = null!;
        private System.Windows.Forms.TextBox txtSearch = null!;
        private System.Windows.Forms.Button btnAdd = null!;
        private System.Windows.Forms.Button btnEdit = null!;
        private System.Windows.Forms.Button btnReload = null!;
        private System.Windows.Forms.SplitContainer splitMain = null!;
        private System.Windows.Forms.DataGridView dgvRoles = null!;
        private System.Windows.Forms.TableLayoutPanel permissionLayout = null!;
        private System.Windows.Forms.Label lblSelectedRole = null!;
        private System.Windows.Forms.CheckedListBox clbPermissions = null!;
        private System.Windows.Forms.Button btnSavePermissions = null!;
        private System.Windows.Forms.Label lblRecordCount = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.rootLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.toolbar = new System.Windows.Forms.FlowLayoutPanel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.dgvRoles = new System.Windows.Forms.DataGridView();
            this.permissionLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblSelectedRole = new System.Windows.Forms.Label();
            this.clbPermissions = new System.Windows.Forms.CheckedListBox();
            this.btnSavePermissions = new System.Windows.Forms.Button();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.rootLayout.SuspendLayout();
            this.toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).BeginInit();
            this.permissionLayout.SuspendLayout();
            this.SuspendLayout();

            this.rootLayout.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.rootLayout.ColumnCount = 1;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.lblTitle, 0, 0);
            this.rootLayout.Controls.Add(this.toolbar, 0, 1);
            this.rootLayout.Controls.Add(this.splitMain, 0, 2);
            this.rootLayout.Controls.Add(this.lblRecordCount, 0, 3);
            this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayout.Padding = new System.Windows.Forms.Padding(18);
            this.rootLayout.RowCount = 4;
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));

            this.lblTitle.AutoSize = true;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTitle.Text = "Quản lý vai trò và phân quyền";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.toolbar.Controls.Add(this.txtSearch);
            this.toolbar.Controls.Add(this.btnAdd);
            this.toolbar.Controls.Add(this.btnEdit);
            this.toolbar.Controls.Add(this.btnReload);
            this.toolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.toolbar.WrapContents = false;

            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 3, 14, 3);
            this.txtSearch.PlaceholderText = "Mã hoặc tên vai trò...";
            this.txtSearch.Size = new System.Drawing.Size(300, 30);
            ConfigureButton(this.btnAdd, "Thêm vai trò", System.Drawing.Color.FromArgb(39, 174, 96), 125);
            ConfigureButton(this.btnEdit, "Sửa thông tin", System.Drawing.Color.FromArgb(52, 152, 219), 125);
            ConfigureButton(this.btnReload, "Tải lại", System.Drawing.Color.FromArgb(108, 117, 125), 90);

            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitMain.Panel1.Controls.Add(this.dgvRoles);
            this.splitMain.Panel2.Controls.Add(this.permissionLayout);
            this.splitMain.SplitterDistance = 660;

            this.dgvRoles.AllowUserToAddRows = false;
            this.dgvRoles.AllowUserToDeleteRows = false;
            this.dgvRoles.AllowUserToResizeRows = false;
            this.dgvRoles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRoles.BackgroundColor = System.Drawing.Color.White;
            this.dgvRoles.ColumnHeadersHeight = 38;
            this.dgvRoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRoles.MultiSelect = false;
            this.dgvRoles.ReadOnly = true;
            this.dgvRoles.RowHeadersVisible = false;
            this.dgvRoles.RowTemplate.Height = 34;
            this.dgvRoles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.permissionLayout.BackColor = System.Drawing.Color.White;
            this.permissionLayout.ColumnCount = 1;
            this.permissionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.permissionLayout.Controls.Add(this.lblSelectedRole, 0, 0);
            this.permissionLayout.Controls.Add(this.clbPermissions, 0, 1);
            this.permissionLayout.Controls.Add(this.btnSavePermissions, 0, 2);
            this.permissionLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.permissionLayout.Padding = new System.Windows.Forms.Padding(12);
            this.permissionLayout.RowCount = 3;
            this.permissionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.permissionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.permissionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));

            this.lblSelectedRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSelectedRole.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSelectedRole.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblSelectedRole.Text = "Chọn vai trò để xem quyền";
            this.lblSelectedRole.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.clbPermissions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clbPermissions.CheckOnClick = true;
            this.clbPermissions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbPermissions.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.clbPermissions.IntegralHeight = false;

            this.btnSavePermissions.BackColor = System.Drawing.Color.FromArgb(230, 126, 34);
            this.btnSavePermissions.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSavePermissions.FlatAppearance.BorderSize = 0;
            this.btnSavePermissions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSavePermissions.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSavePermissions.ForeColor = System.Drawing.Color.White;
            this.btnSavePermissions.Size = new System.Drawing.Size(160, 38);
            this.btnSavePermissions.Text = "Lưu phân quyền";
            this.btnSavePermissions.UseVisualStyleBackColor = false;

            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblRecordCount.Text = "0 vai trò";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.Controls.Add(this.rootLayout);
            this.Name = "RoleManagementControl";
            this.Size = new System.Drawing.Size(1160, 590);
            this.rootLayout.ResumeLayout(false);
            this.rootLayout.PerformLayout();
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoles)).EndInit();
            this.permissionLayout.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private static void ConfigureButton(System.Windows.Forms.Button button, string text, System.Drawing.Color color, int width)
        {
            button.BackColor = color;
            button.Cursor = System.Windows.Forms.Cursors.Hand;
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            button.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            button.ForeColor = System.Drawing.Color.White;
            button.Margin = new System.Windows.Forms.Padding(0, 2, 8, 2);
            button.Size = new System.Drawing.Size(width, 34);
            button.Text = text;
            button.UseVisualStyleBackColor = false;
        }
    }
}
