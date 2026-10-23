namespace QuanLyCuaHangBanhMi.Forms
{
    partial class ShiftManagementControl
    {
        private System.ComponentModel.IContainer components = null!;
        private System.Windows.Forms.TableLayoutPanel rootLayout = null!;
        private System.Windows.Forms.Label lblTitle = null!;
        private System.Windows.Forms.FlowLayoutPanel toolbar = null!;
        private System.Windows.Forms.DateTimePicker dtpFrom = null!;
        private System.Windows.Forms.DateTimePicker dtpTo = null!;
        private System.Windows.Forms.ComboBox cboBranch = null!;
        private System.Windows.Forms.ComboBox cboStatus = null!;
        private System.Windows.Forms.Button btnOpen = null!;
        private System.Windows.Forms.Button btnClose = null!;
        private System.Windows.Forms.Button btnApprove = null!;
        private System.Windows.Forms.Button btnReload = null!;
        private System.Windows.Forms.DataGridView dgvShifts = null!;
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
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.cboBranch = new System.Windows.Forms.ComboBox();
            this.cboStatus = new System.Windows.Forms.ComboBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnApprove = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.dgvShifts = new System.Windows.Forms.DataGridView();
            this.lblRecordCount = new System.Windows.Forms.Label();
            this.rootLayout.SuspendLayout();
            this.toolbar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShifts)).BeginInit();
            this.SuspendLayout();

            this.rootLayout.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.rootLayout.ColumnCount = 1;
            this.rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.rootLayout.Controls.Add(this.lblTitle, 0, 0);
            this.rootLayout.Controls.Add(this.toolbar, 0, 1);
            this.rootLayout.Controls.Add(this.dgvShifts, 0, 2);
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
            this.lblTitle.Text = "Quản lý ca làm";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.toolbar.Controls.Add(this.dtpFrom);
            this.toolbar.Controls.Add(this.dtpTo);
            this.toolbar.Controls.Add(this.cboBranch);
            this.toolbar.Controls.Add(this.cboStatus);
            this.toolbar.Controls.Add(this.btnOpen);
            this.toolbar.Controls.Add(this.btnClose);
            this.toolbar.Controls.Add(this.btnApprove);
            this.toolbar.Controls.Add(this.btnReload);
            this.toolbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbar.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.toolbar.WrapContents = false;

            ConfigureDate(this.dtpFrom);
            ConfigureDate(this.dtpTo);
            ConfigureCombo(this.cboBranch, 180);
            ConfigureCombo(this.cboStatus, 130);
            ConfigureButton(this.btnOpen, "Mở ca", System.Drawing.Color.FromArgb(39, 174, 96), 85);
            ConfigureButton(this.btnClose, "Đóng ca", System.Drawing.Color.FromArgb(230, 126, 34), 85);
            ConfigureButton(this.btnApprove, "Duyệt ca", System.Drawing.Color.FromArgb(52, 152, 219), 90);
            ConfigureButton(this.btnReload, "Tải lại", System.Drawing.Color.FromArgb(108, 117, 125), 80);

            this.dgvShifts.AllowUserToAddRows = false;
            this.dgvShifts.AllowUserToDeleteRows = false;
            this.dgvShifts.AllowUserToResizeRows = false;
            this.dgvShifts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvShifts.BackgroundColor = System.Drawing.Color.White;
            this.dgvShifts.ColumnHeadersHeight = 38;
            this.dgvShifts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvShifts.MultiSelect = false;
            this.dgvShifts.ReadOnly = true;
            this.dgvShifts.RowHeadersVisible = false;
            this.dgvShifts.RowTemplate.Height = 34;
            this.dgvShifts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.lblRecordCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRecordCount.ForeColor = System.Drawing.Color.DimGray;
            this.lblRecordCount.Text = "0 ca làm";
            this.lblRecordCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(245, 246, 250);
            this.Controls.Add(this.rootLayout);
            this.Name = "ShiftManagementControl";
            this.Size = new System.Drawing.Size(1160, 590);
            this.rootLayout.ResumeLayout(false);
            this.toolbar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShifts)).EndInit();
            this.ResumeLayout(false);
        }

        private static void ConfigureDate(System.Windows.Forms.DateTimePicker picker)
        {
            picker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            picker.CustomFormat = "dd/MM/yyyy";
            picker.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            picker.Margin = new System.Windows.Forms.Padding(0, 3, 8, 3);
            picker.Size = new System.Drawing.Size(120, 30);
        }

        private static void ConfigureCombo(System.Windows.Forms.ComboBox combo, int width)
        {
            combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            combo.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            combo.Margin = new System.Windows.Forms.Padding(0, 3, 8, 3);
            combo.Size = new System.Drawing.Size(width, 30);
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
