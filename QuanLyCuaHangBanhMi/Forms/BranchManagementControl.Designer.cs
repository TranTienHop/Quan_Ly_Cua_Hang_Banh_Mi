namespace QuanLyCuaHangBanhMi.Forms
{
    partial class BranchManagementControl
    {
        private System.ComponentModel.IContainer components = null!;
        private System.Windows.Forms.TableLayoutPanel rootLayout = null!;
        private System.Windows.Forms.Label lblTitle = null!;
        private System.Windows.Forms.FlowLayoutPanel toolbar = null!;
        private System.Windows.Forms.TextBox txtSearch = null!;
        private System.Windows.Forms.ComboBox cboType = null!;
        private System.Windows.Forms.ComboBox cboStatus = null!;
        private System.Windows.Forms.Button btnAdd = null!;
        private System.Windows.Forms.Button btnEdit = null!;
        private System.Windows.Forms.Button btnToggleStatus = null!;
        private System.Windows.Forms.Button btnReload = null!;
        private System.Windows.Forms.DataGridView dgvBranches = null!;
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
            this.cboType = new System.Windows.Forms.ComboBox();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnToggleStatus = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.dgvBranches = new System.Windows.Forms.DataGridView();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.rootLayout.SuspendLayout();
            this.toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBranches)).BeginInit();
            this.SuspendLayout();

            this.rootLayout.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.rootLayout.ColumnCount = 1;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.lblTitle, 0, 0);
            this.rootLayout.Controls.Add(this.toolbar, 0, 1);
            this.rootLayout.Controls.Add(this.dgvBranches, 0, 2);
            this.rootLayout.Controls.Add(this.lblRecordCount, 0, 3);
            this.rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rootLayout.Padding = new System.Windows.Forms.Padding(18);
            this.rootLayout.RowCount = 4;
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));

            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblTitle.Text = "Quản lý chi nhánh";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.toolbar.Controls.Add(this.txtSearch);
            this.toolbar.Controls.Add(this.cboType);
            this.toolbar.Controls.Add(this.cboStatus);
            this.toolbar.Controls.Add(this.btnAdd);
            this.toolbar.Controls.Add(this.btnEdit);
            this.toolbar.Controls.Add(this.btnToggleStatus);
            this.toolbar.Controls.Add(this.btnReload);
            this.toolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.toolbar.WrapContents = false;

            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 3, 8, 3);
            this.txtSearch.PlaceholderText = "Mã, tên, địa chỉ...";
            this.txtSearch.Size = new System.Drawing.Size(220, 30);
            ConfigureCombo(this.cboType, 145);
            ConfigureCombo(this.cboStatus, 145);
            ConfigureButton(this.btnAdd, "Thêm chi nhánh", System.Drawing.Color.FromArgb(39, 174, 96), 125);
            ConfigureButton(this.btnEdit, "Sửa", System.Drawing.Color.FromArgb(52, 152, 219), 80);
            ConfigureButton(this.btnToggleStatus, "Khóa / Mở", System.Drawing.Color.FromArgb(230, 126, 34), 105);
            ConfigureButton(this.btnReload, "Tải lại", System.Drawing.Color.FromArgb(108, 117, 125), 85);

            this.dgvBranches.AllowUserToAddRows = false;
            this.dgvBranches.AllowUserToDeleteRows = false;
            this.dgvBranches.AllowUserToResizeRows = false;
            this.dgvBranches.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBranches.BackgroundColor = System.Drawing.Color.White;
            this.dgvBranches.ColumnHeadersHeight = 38;
            this.dgvBranches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBranches.MultiSelect = false;
            this.dgvBranches.ReadOnly = true;
            this.dgvBranches.RowHeadersVisible = false;
            this.dgvBranches.RowTemplate.Height = 34;
            this.dgvBranches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblRecordCount.Text = "0 chi nhánh";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.Controls.Add(this.rootLayout);
            this.Name = "BranchManagementControl";
            this.Size = new System.Drawing.Size(1160, 590);
            this.rootLayout.ResumeLayout(false);
            this.toolbar.ResumeLayout(false);
            this.toolbar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBranches)).EndInit();
            this.ResumeLayout(false);
        }

        private static void ConfigureCombo(System.Windows.Forms.ComboBox combo, int width)
        {
            combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            combo.Font = new System.Drawing.Font("Segoe UI", 10F);
            combo.Margin = new System.Windows.Forms.Padding(0, 3, 8, 3);
            combo.Size = new System.Drawing.Size(width, 31);
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
