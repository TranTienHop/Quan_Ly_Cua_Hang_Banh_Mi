namespace QuanLyCuaHangBanhMi.Forms
{
    partial class EmployeeShiftScheduleControl
    {
        private System.ComponentModel.IContainer components = null!;
        private System.Windows.Forms.TableLayoutPanel rootLayout = null!;
        private System.Windows.Forms.FlowLayoutPanel filters = null!;
        private System.Windows.Forms.ComboBox cboBranch = null!;
        private System.Windows.Forms.DateTimePicker dtpFrom = null!;
        private System.Windows.Forms.DateTimePicker dtpTo = null!;
        private System.Windows.Forms.Button btnReload = null!;
        private System.Windows.Forms.FlowLayoutPanel assignmentPanel = null!;
        private System.Windows.Forms.FlowLayoutPanel catalogPanel = null!;
        private System.Windows.Forms.ComboBox cboEmployee = null!;
        private System.Windows.Forms.ComboBox cboShift = null!;
        private System.Windows.Forms.DateTimePicker dtpWorkDate = null!;
        private System.Windows.Forms.TextBox txtNote = null!;
        private System.Windows.Forms.Button btnAssign = null!;
        private System.Windows.Forms.Button btnCancel = null!;
        private System.Windows.Forms.Button btnAddShift = null!;
        private System.Windows.Forms.Button btnEditShift = null!;
        private System.Windows.Forms.Button btnToggleShift = null!;
        private System.Windows.Forms.DataGridView dgvSchedule = null!;
        private System.Windows.Forms.Label lblCount = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            rootLayout = new System.Windows.Forms.TableLayoutPanel();
            filters = new System.Windows.Forms.FlowLayoutPanel();
            cboBranch = new System.Windows.Forms.ComboBox();
            dtpFrom = new System.Windows.Forms.DateTimePicker();
            dtpTo = new System.Windows.Forms.DateTimePicker();
            btnReload = new System.Windows.Forms.Button();
            assignmentPanel = new System.Windows.Forms.FlowLayoutPanel();
            catalogPanel = new System.Windows.Forms.FlowLayoutPanel();
            cboEmployee = new System.Windows.Forms.ComboBox();
            cboShift = new System.Windows.Forms.ComboBox();
            dtpWorkDate = new System.Windows.Forms.DateTimePicker();
            txtNote = new System.Windows.Forms.TextBox();
            btnAssign = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            btnAddShift = new System.Windows.Forms.Button();
            btnEditShift = new System.Windows.Forms.Button();
            btnToggleShift = new System.Windows.Forms.Button();
            dgvSchedule = new System.Windows.Forms.DataGridView();
            lblCount = new System.Windows.Forms.Label();
            rootLayout.SuspendLayout();
            filters.SuspendLayout();
            assignmentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSchedule).BeginInit();
            SuspendLayout();
            //
            // rootLayout
            //
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            rootLayout.Controls.Add(filters, 0, 0);
            rootLayout.Controls.Add(assignmentPanel, 0, 1);
            rootLayout.Controls.Add(catalogPanel, 0, 2);
            rootLayout.Controls.Add(dgvSchedule, 0, 3);
            rootLayout.Controls.Add(lblCount, 0, 4);
            rootLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            rootLayout.RowCount = 5;
            rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            //
            // filters
            //
            filters.Controls.Add(new System.Windows.Forms.Label { Text = "Chi nhánh", AutoSize = true, Margin = new System.Windows.Forms.Padding(3, 10, 3, 3) });
            filters.Controls.Add(cboBranch);
            filters.Controls.Add(new System.Windows.Forms.Label { Text = "Từ ngày", AutoSize = true, Margin = new System.Windows.Forms.Padding(12, 10, 3, 3) });
            filters.Controls.Add(dtpFrom);
            filters.Controls.Add(new System.Windows.Forms.Label { Text = "Đến ngày", AutoSize = true, Margin = new System.Windows.Forms.Padding(12, 10, 3, 3) });
            filters.Controls.Add(dtpTo);
            filters.Controls.Add(btnReload);
            filters.Dock = System.Windows.Forms.DockStyle.Fill;
            filters.WrapContents = false;
            //
            // cboBranch
            //
            cboBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboBranch.Width = 220;
            //
            // date filters
            //
            dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpFrom.Value = System.DateTime.Today;
            dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpTo.Value = System.DateTime.Today.AddDays(7);
            //
            // btnReload
            //
            btnReload.Text = "Tải lại";
            btnReload.AutoSize = true;
            btnReload.Margin = new System.Windows.Forms.Padding(12, 3, 3, 3);
            //
            // assignmentPanel
            //
            assignmentPanel.Controls.Add(new System.Windows.Forms.Label { Text = "Nhân viên", AutoSize = true, Margin = new System.Windows.Forms.Padding(3, 10, 3, 3) });
            assignmentPanel.Controls.Add(cboEmployee);
            assignmentPanel.Controls.Add(new System.Windows.Forms.Label { Text = "Ca", AutoSize = true, Margin = new System.Windows.Forms.Padding(12, 10, 3, 3) });
            assignmentPanel.Controls.Add(cboShift);
            assignmentPanel.Controls.Add(new System.Windows.Forms.Label { Text = "Ngày làm", AutoSize = true, Margin = new System.Windows.Forms.Padding(12, 10, 3, 3) });
            assignmentPanel.Controls.Add(dtpWorkDate);
            assignmentPanel.Controls.Add(txtNote);
            assignmentPanel.Controls.Add(btnAssign);
            assignmentPanel.Controls.Add(btnCancel);
            assignmentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            assignmentPanel.WrapContents = false;
            //
            // assignment controls
            //
            cboEmployee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboEmployee.Width = 220;
            cboShift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboShift.Width = 180;
            dtpWorkDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            dtpWorkDate.Value = System.DateTime.Today;
            txtNote.PlaceholderText = "Ghi chú (tùy chọn)";
            txtNote.Width = 180;
            txtNote.Margin = new System.Windows.Forms.Padding(12, 6, 3, 3);
            btnAssign.Text = "Phân ca";
            btnAssign.AutoSize = true;
            btnCancel.Text = "Hủy lịch đã chọn";
            btnCancel.AutoSize = true;
            //
            // catalogPanel
            //
            catalogPanel.Controls.Add(new System.Windows.Forms.Label
            {
                Text = "Danh mục ca:",
                AutoSize = true,
                Margin = new System.Windows.Forms.Padding(3, 10, 10, 3)
            });
            catalogPanel.Controls.Add(btnAddShift);
            catalogPanel.Controls.Add(btnEditShift);
            catalogPanel.Controls.Add(btnToggleShift);
            catalogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            catalogPanel.WrapContents = false;
            btnAddShift.Text = "Thêm ca";
            btnAddShift.AutoSize = true;
            btnEditShift.Text = "Sửa ca đã chọn";
            btnEditShift.AutoSize = true;
            btnToggleShift.Text = "Kích hoạt / ngừng ca";
            btnToggleShift.AutoSize = true;
            //
            // dgvSchedule
            //
            dgvSchedule.AllowUserToAddRows = false;
            dgvSchedule.AllowUserToDeleteRows = false;
            dgvSchedule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvSchedule.BackgroundColor = System.Drawing.Color.White;
            dgvSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvSchedule.ReadOnly = true;
            dgvSchedule.RowHeadersVisible = false;
            dgvSchedule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            //
            // lblCount
            //
            lblCount.Dock = System.Windows.Forms.DockStyle.Fill;
            lblCount.Text = "0 lịch phân ca";
            lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // EmployeeShiftScheduleControl
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(rootLayout);
            Name = "EmployeeShiftScheduleControl";
            Size = new System.Drawing.Size(1150, 580);
            rootLayout.ResumeLayout(false);
            filters.ResumeLayout(false);
            filters.PerformLayout();
            assignmentPanel.ResumeLayout(false);
            assignmentPanel.PerformLayout();
            catalogPanel.ResumeLayout(false);
            catalogPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvSchedule).EndInit();
            ResumeLayout(false);
        }
    }
}
