namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormGhiChuMon
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlBody = new Panel();
            flpQuick = new FlowLayoutPanel();
            txtNote = new TextBox();
            lblTitle = new Label();
            pnlButtons = new FlowLayoutPanel();
            btnCancel = new Button();
            btnOk = new Button();
            pnlBody.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBody
            // 
            pnlBody.Controls.Add(flpQuick);
            pnlBody.Controls.Add(txtNote);
            pnlBody.Controls.Add(lblTitle);
            pnlBody.Controls.Add(pnlButtons);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 0);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(14, 8, 14, 10);
            pnlBody.Size = new Size(460, 270);
            pnlBody.TabIndex = 0;
            // 
            // flpQuick
            // 
            flpQuick.Dock = DockStyle.Fill;
            flpQuick.Location = new Point(14, 71);
            flpQuick.Name = "flpQuick";
            flpQuick.Padding = new Padding(0, 8, 0, 0);
            flpQuick.Size = new Size(432, 143);
            flpQuick.TabIndex = 1;
            // 
            // txtNote
            // 
            txtNote.Dock = DockStyle.Top;
            txtNote.Font = new Font("Segoe UI", 11F);
            txtNote.Location = new Point(14, 44);
            txtNote.MaxLength = 255;
            txtNote.Name = "txtNote";
            txtNote.PlaceholderText = "Nhập ghi chú hoặc bấm chọn nhanh bên dưới";
            txtNote.Size = new Size(432, 27);
            txtNote.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoEllipsis = true;
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitle.Location = new Point(14, 8);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(432, 36);
            lblTitle.TabIndex = 2;
            lblTitle.Text = "📝 Tên món";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.UseMnemonic = false;
            lblTitle.Click += lblTitle_Click;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnOk);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.FlowDirection = FlowDirection.RightToLeft;
            pnlButtons.Location = new Point(14, 214);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(0, 6, 0, 0);
            pnlButtons.Size = new Size(432, 46);
            pnlButtons.TabIndex = 2;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.FromArgb(236, 240, 241);
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(339, 9);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 34);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.FromArgb(242, 153, 0);
            btnOk.DialogResult = DialogResult.OK;
            btnOk.FlatAppearance.BorderSize = 0;
            btnOk.FlatStyle = FlatStyle.Flat;
            btnOk.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnOk.ForeColor = Color.White;
            btnOk.Location = new Point(203, 9);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(130, 34);
            btnOk.TabIndex = 0;
            btnOk.Text = "✔ Lưu ghi chú";
            btnOk.UseVisualStyleBackColor = false;
            // 
            // FormGhiChuMon
            // 
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = btnCancel;
            ClientSize = new Size(460, 270);
            Controls.Add(pnlBody);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormGhiChuMon";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Ghi chú món";
            Shown += FormGhiChuMon_Shown;
            pnlBody.ResumeLayout(false);
            pnlBody.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.FlowLayoutPanel flpQuick;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.FlowLayoutPanel pnlButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}
