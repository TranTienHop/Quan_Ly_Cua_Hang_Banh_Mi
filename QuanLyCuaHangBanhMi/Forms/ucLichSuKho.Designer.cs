namespace QuanLyCuaHangBanhMi.Forms
{
    partial class ucLichSuKho
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tlpRoot = new TableLayoutPanel();
            flowLoc = new FlowLayoutPanel();
            dgv = new DataGridView();
            lblTongKet = new Label();
            lblTuCap = new Label();
            dtpTu = new DateTimePicker();
            lblDenCap = new Label();
            dtpDen = new DateTimePicker();
            cboLoai = new ComboBox();
            cboNL = new ComboBox();
            btnXem = new Button();
            btnXuat = new Button();
            tlpRoot.SuspendLayout();
            flowLoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.ColumnCount = 1;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpRoot.Controls.Add(flowLoc, 0, 0);
            tlpRoot.Controls.Add(dgv, 0, 1);
            tlpRoot.Controls.Add(lblTongKet, 0, 2);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.Padding = new Padding(8);
            tlpRoot.RowCount = 3;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tlpRoot.Size = new Size(1100, 600);
            tlpRoot.TabIndex = 0;
            // 
            // flowLoc
            // 
            flowLoc.Controls.Add(lblTuCap);
            flowLoc.Controls.Add(dtpTu);
            flowLoc.Controls.Add(lblDenCap);
            flowLoc.Controls.Add(dtpDen);
            flowLoc.Controls.Add(cboLoai);
            flowLoc.Controls.Add(cboNL);
            flowLoc.Controls.Add(btnXem);
            flowLoc.Controls.Add(btnXuat);
            flowLoc.Dock = DockStyle.Fill;
            flowLoc.Location = new Point(11, 11);
            flowLoc.Name = "flowLoc";
            flowLoc.Size = new Size(1078, 78);
            flowLoc.TabIndex = 0;
            // 
            // dgv
            // 
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Dock = DockStyle.Fill;
            dgv.Location = new Point(11, 95);
            dgv.Name = "dgv";
            dgv.RowHeadersWidth = 51;
            dgv.Size = new Size(1078, 462);
            dgv.TabIndex = 1;
            // 
            // lblTongKet
            // 
            lblTongKet.Dock = DockStyle.Fill;
            lblTongKet.Location = new Point(11, 560);
            lblTongKet.Name = "lblTongKet";
            lblTongKet.Size = new Size(1078, 32);
            lblTongKet.TabIndex = 2;
            lblTongKet.Text = "label1";
            lblTongKet.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTuCap
            // 
            lblTuCap.AutoSize = true;
            lblTuCap.Location = new Point(3, 9);
            lblTuCap.Margin = new Padding(3, 9, 3, 3);
            lblTuCap.Name = "lblTuCap";
            lblTuCap.Size = new Size(65, 20);
            lblTuCap.TabIndex = 0;
            lblTuCap.Text = "Từ ngày:";
            // 
            // dtpTu
            // 
            dtpTu.Format = DateTimePickerFormat.Short;
            dtpTu.Location = new Point(74, 6);
            dtpTu.Margin = new Padding(3, 6, 14, 3);
            dtpTu.Name = "dtpTu";
            dtpTu.Size = new Size(120, 27);
            dtpTu.TabIndex = 1;
            // 
            // lblDenCap
            // 
            lblDenCap.AutoSize = true;
            lblDenCap.Location = new Point(211, 9);
            lblDenCap.Margin = new Padding(3, 9, 3, 3);
            lblDenCap.Name = "lblDenCap";
            lblDenCap.Size = new Size(75, 20);
            lblDenCap.TabIndex = 2;
            lblDenCap.Text = "Đến ngày:";
            // 
            // dtpDen
            // 
            dtpDen.Format = DateTimePickerFormat.Short;
            dtpDen.Location = new Point(292, 6);
            dtpDen.Margin = new Padding(3, 6, 14, 3);
            dtpDen.Name = "dtpDen";
            dtpDen.Size = new Size(120, 27);
            dtpDen.TabIndex = 3;
            // 
            // cboLoai
            // 
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoai.FormattingEnabled = true;
            cboLoai.Location = new Point(429, 6);
            cboLoai.Margin = new Padding(3, 6, 14, 3);
            cboLoai.Name = "cboLoai";
            cboLoai.Size = new Size(149, 28);
            cboLoai.TabIndex = 4;
            // 
            // cboNL
            // 
            cboNL.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNL.FormattingEnabled = true;
            cboNL.Location = new Point(595, 6);
            cboNL.Margin = new Padding(3, 6, 14, 3);
            cboNL.Name = "cboNL";
            cboNL.Size = new Size(270, 28);
            cboNL.TabIndex = 5;
            // 
            // btnXem
            // 
            btnXem.Location = new Point(882, 5);
            btnXem.Margin = new Padding(3, 5, 3, 3);
            btnXem.Name = "btnXem";
            btnXem.Size = new Size(89, 32);
            btnXem.TabIndex = 6;
            btnXem.Text = "Tìm";
            btnXem.UseVisualStyleBackColor = true;
            // 
            // btnXuat
            // 
            btnXuat.Location = new Point(977, 5);
            btnXuat.Margin = new Padding(3, 5, 3, 3);
            btnXuat.Name = "btnXuat";
            btnXuat.Size = new Size(89, 32);
            btnXuat.TabIndex = 7;
            btnXuat.Text = "Xuất CSV";
            btnXuat.UseVisualStyleBackColor = true;
            // 
            // ucLichSuKho
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tlpRoot);
            Name = "ucLichSuKho";
            Size = new Size(1100, 600);
            tlpRoot.ResumeLayout(false);
            flowLoc.ResumeLayout(false);
            flowLoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRoot;
        private FlowLayoutPanel flowLoc;
        private Label lblTuCap;
        private DateTimePicker dtpTu;
        private Label lblDenCap;
        private DataGridView dgv;
        private Label lblTongKet;
        private DateTimePicker dtpDen;
        private ComboBox cboLoai;
        private ComboBox cboNL;
        private Button btnXem;
        private Button btnXuat;
    }
}
