using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormNhaCungCap
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            flowLoc = new FlowLayoutPanel();
            txtTim = new TextBox();
            cboLoc = new ComboBox();
            dgv = new DataGridView();
            lblTongKet = new Label();
            grpChiTiet = new GroupBox();
            tlpForm = new TableLayoutPanel();
            lblMa = new Label();
            lblTen = new Label();
            lblSdt = new Label();
            lblDiaChi = new Label();
            lblTT = new Label();
            lblGD = new Label();
            txtMa = new TextBox();
            txtTen = new TextBox();
            txtSDT = new TextBox();
            txtDiaChi = new TextBox();
            chkHoatDong = new CheckBox();
            lblThongKe = new Label();
            flowNut = new FlowLayoutPanel();
            btnMoi = new Button();
            btnLuu = new Button();
            ((System.ComponentModel.ISupportInitialize)Err).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            flowLoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            grpChiTiet.SuspendLayout();
            tlpForm.SuspendLayout();
            flowNut.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            tableLayoutPanel1.Controls.Add(flowLoc, 0, 0);
            tableLayoutPanel1.Controls.Add(dgv, 0, 1);
            tableLayoutPanel1.Controls.Add(lblTongKet, 0, 2);
            tableLayoutPanel1.Controls.Add(grpChiTiet, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.Padding = new Padding(12);
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.Size = new Size(1082, 603);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLoc
            // 
            flowLoc.Controls.Add(txtTim);
            flowLoc.Controls.Add(cboLoc);
            flowLoc.Dock = DockStyle.Fill;
            flowLoc.Location = new Point(15, 15);
            flowLoc.Name = "flowLoc";
            flowLoc.Size = new Size(652, 46);
            flowLoc.TabIndex = 0;
            flowLoc.WrapContents = false;
            // 
            // txtTim
            // 
            txtTim.Location = new Point(3, 6);
            txtTim.Margin = new Padding(3, 6, 18, 3);
            txtTim.Name = "txtTim";
            txtTim.PlaceholderText = "Tìm theo tên, SĐT, địa chỉ,...";
            txtTim.Size = new Size(280, 30);
            txtTim.TabIndex = 0;
            // 
            // cboLoc
            // 
            cboLoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoc.FormattingEnabled = true;
            cboLoc.Location = new Point(304, 6);
            cboLoc.Margin = new Padding(3, 6, 3, 3);
            cboLoc.Name = "cboLoc";
            cboLoc.Size = new Size(170, 31);
            cboLoc.TabIndex = 1;
            // 
            // dgv
            // 
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Dock = DockStyle.Fill;
            dgv.Location = new Point(15, 67);
            dgv.Name = "dgv";
            dgv.RowHeadersWidth = 51;
            dgv.Size = new Size(652, 489);
            dgv.TabIndex = 2;
            // 
            // lblTongKet
            // 
            lblTongKet.Dock = DockStyle.Fill;
            lblTongKet.Location = new Point(15, 559);
            lblTongKet.Name = "lblTongKet";
            lblTongKet.Size = new Size(652, 32);
            lblTongKet.TabIndex = 1;
            lblTongKet.Text = "label1";
            lblTongKet.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpChiTiet
            // 
            grpChiTiet.Controls.Add(tlpForm);
            grpChiTiet.Dock = DockStyle.Fill;
            grpChiTiet.Location = new Point(682, 12);
            grpChiTiet.Margin = new Padding(12, 0, 0, 0);
            grpChiTiet.Name = "grpChiTiet";
            tableLayoutPanel1.SetRowSpan(grpChiTiet, 3);
            grpChiTiet.Size = new Size(388, 579);
            grpChiTiet.TabIndex = 3;
            grpChiTiet.TabStop = false;
            grpChiTiet.Text = "Thông tin nhà cung cấp";
            // 
            // tlpForm
            // 
            tlpForm.ColumnCount = 2;
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpForm.Controls.Add(lblMa, 0, 0);
            tlpForm.Controls.Add(lblTen, 0, 1);
            tlpForm.Controls.Add(lblSdt, 0, 2);
            tlpForm.Controls.Add(lblDiaChi, 0, 3);
            tlpForm.Controls.Add(lblTT, 0, 4);
            tlpForm.Controls.Add(lblGD, 0, 5);
            tlpForm.Controls.Add(txtMa, 1, 0);
            tlpForm.Controls.Add(txtTen, 1, 1);
            tlpForm.Controls.Add(txtSDT, 1, 2);
            tlpForm.Controls.Add(txtDiaChi, 1, 3);
            tlpForm.Controls.Add(chkHoatDong, 1, 4);
            tlpForm.Controls.Add(lblThongKe, 1, 5);
            tlpForm.Controls.Add(flowNut, 0, 7);
            tlpForm.Dock = DockStyle.Fill;
            tlpForm.Location = new Point(3, 26);
            tlpForm.Name = "tlpForm";
            tlpForm.Padding = new Padding(6);
            tlpForm.RowCount = 8;
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpForm.Size = new Size(382, 550);
            tlpForm.TabIndex = 0;
            // 
            // lblMa
            // 
            lblMa.Dock = DockStyle.Fill;
            lblMa.Location = new Point(9, 6);
            lblMa.Name = "lblMa";
            lblMa.Size = new Size(94, 40);
            lblMa.TabIndex = 1;
            lblMa.Text = "Mã NCC";
            lblMa.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTen
            // 
            lblTen.Dock = DockStyle.Fill;
            lblTen.Location = new Point(9, 46);
            lblTen.Name = "lblTen";
            lblTen.Size = new Size(94, 40);
            lblTen.TabIndex = 3;
            lblTen.Text = "Tên *";
            lblTen.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSdt
            // 
            lblSdt.Dock = DockStyle.Fill;
            lblSdt.Location = new Point(9, 86);
            lblSdt.Name = "lblSdt";
            lblSdt.Size = new Size(94, 40);
            lblSdt.TabIndex = 4;
            lblSdt.Text = "Điện thoại";
            lblSdt.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiaChi
            // 
            lblDiaChi.Dock = DockStyle.Fill;
            lblDiaChi.Location = new Point(9, 126);
            lblDiaChi.Name = "lblDiaChi";
            lblDiaChi.Size = new Size(94, 40);
            lblDiaChi.TabIndex = 2;
            lblDiaChi.Text = "Địa chỉ ";
            lblDiaChi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTT
            // 
            lblTT.Dock = DockStyle.Fill;
            lblTT.Location = new Point(9, 166);
            lblTT.Name = "lblTT";
            lblTT.Size = new Size(94, 40);
            lblTT.TabIndex = 4;
            lblTT.Text = "Trạng thái";
            lblTT.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGD
            // 
            lblGD.Dock = DockStyle.Fill;
            lblGD.Location = new Point(9, 206);
            lblGD.Name = "lblGD";
            lblGD.Size = new Size(94, 70);
            lblGD.TabIndex = 5;
            lblGD.Text = "Giao dịch";
            lblGD.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMa
            // 
            txtMa.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtMa.Location = new Point(109, 11);
            txtMa.Name = "txtMa";
            txtMa.ReadOnly = true;
            txtMa.Size = new Size(264, 30);
            txtMa.TabIndex = 2;
            // 
            // txtTen
            // 
            txtTen.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTen.Location = new Point(109, 51);
            txtTen.Name = "txtTen";
            txtTen.Size = new Size(264, 30);
            txtTen.TabIndex = 3;
            // 
            // txtSDT
            // 
            txtSDT.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSDT.Location = new Point(109, 91);
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new Size(264, 30);
            txtSDT.TabIndex = 4;
            // 
            // txtDiaChi
            // 
            txtDiaChi.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDiaChi.Location = new Point(109, 131);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.PlaceholderText = "Quận/Huyện, Tỉnh/TP (vd: Quận 1, TP.HCM)";
            txtDiaChi.Size = new Size(264, 30);
            txtDiaChi.TabIndex = 5;
            // 
            // chkHoatDong
            // 
            chkHoatDong.Anchor = AnchorStyles.Left;
            chkHoatDong.AutoSize = true;
            chkHoatDong.Checked = true;
            chkHoatDong.CheckState = CheckState.Checked;
            chkHoatDong.Location = new Point(109, 172);
            chkHoatDong.Name = "chkHoatDong";
            chkHoatDong.Size = new Size(136, 27);
            chkHoatDong.TabIndex = 6;
            chkHoatDong.Text = "Đang hợp tác";
            chkHoatDong.UseVisualStyleBackColor = true;
            // 
            // lblThongKe
            // 
            lblThongKe.Dock = DockStyle.Fill;
            lblThongKe.Location = new Point(109, 206);
            lblThongKe.Name = "lblThongKe";
            lblThongKe.Size = new Size(264, 70);
            lblThongKe.TabIndex = 11;
            lblThongKe.Text = "label1";
            lblThongKe.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowNut
            // 
            tlpForm.SetColumnSpan(flowNut, 2);
            flowNut.Controls.Add(btnMoi);
            flowNut.Controls.Add(btnLuu);
            flowNut.Dock = DockStyle.Fill;
            flowNut.Location = new Point(9, 491);
            flowNut.Name = "flowNut";
            flowNut.Size = new Size(364, 50);
            flowNut.TabIndex = 12;
            // 
            // btnMoi
            // 
            btnMoi.Location = new Point(3, 3);
            btnMoi.Name = "btnMoi";
            btnMoi.Size = new Size(130, 36);
            btnMoi.TabIndex = 7;
            btnMoi.Text = "+ Thêm mới";
            btnMoi.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(139, 3);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(130, 36);
            btnLuu.TabIndex = 8;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // FrmNhaCungCap
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1082, 603);
            Controls.Add(tableLayoutPanel1);
            MinimumSize = new Size(950, 520);
            Name = "FrmNhaCungCap";
            Text = "Nhà cung cấp";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)Err).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            flowLoc.ResumeLayout(false);
            flowLoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            grpChiTiet.ResumeLayout(false);
            tlpForm.ResumeLayout(false);
            tlpForm.PerformLayout();
            flowNut.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLoc;
        private DataGridView dgv;
        private Label lblTongKet;
        private GroupBox grpChiTiet;
        private TextBox txtTim;
        private ComboBox cboLoc;
        private TableLayoutPanel tlpForm;
        private Label lblMa;
        private Label lblTen;
        private Label lblSdt;
        private Label lblDiaChi;
        private Label lblTT;
        private Label lblGD;
        private TextBox txtMa;
        private TextBox txtTen;
        private TextBox txtSDT;
        private TextBox txtDiaChi;
        private CheckBox chkHoatDong;
        private Label lblThongKe;
        private FlowLayoutPanel flowNut;
        private Button btnMoi;
        private Button btnLuu;
    }
}