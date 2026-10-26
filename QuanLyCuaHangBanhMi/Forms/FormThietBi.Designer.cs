using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormThietBi
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
            tlpRoot = new TableLayoutPanel();
            flowLoc = new FlowLayoutPanel();
            txtTim = new TextBox();
            cboLocCN = new ComboBox();
            cboLocLoai = new ComboBox();
            cboLocTT = new ComboBox();
            dgv = new DataGridView();
            lblTongKet = new Label();
            grpChiTiet = new GroupBox();
            tlpForm = new TableLayoutPanel();
            lblMa = new Label();
            lblCN = new Label();
            lblTen = new Label();
            lblLoai = new Label();
            lblSL = new Label();
            lblNgay = new Label();
            lblGia = new Label();
            lblTT = new Label();
            lblGC = new Label();
            lblGT = new Label();
            flowMa = new FlowLayoutPanel();
            txtMaCode = new TextBox();
            btnGoiY = new Button();
            cboChiNhanh = new ComboBox();
            txtTen = new TextBox();
            cboLoai = new ComboBox();
            numSoLuong = new NumericUpDown();
            dtpNgayMua = new DateTimePicker();
            numDonGia = new NumericUpDown();
            cboTinhTrang = new ComboBox();
            txtGhiChu = new TextBox();
            lblGiaTri = new Label();
            flowNut = new FlowLayoutPanel();
            btnMoi = new Button();
            btnLuu = new Button();
            ((System.ComponentModel.ISupportInitialize)Err).BeginInit();
            tlpRoot.SuspendLayout();
            flowLoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
            grpChiTiet.SuspendLayout();
            tlpForm.SuspendLayout();
            flowMa.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSoLuong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDonGia).BeginInit();
            flowNut.SuspendLayout();
            SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.ColumnCount = 2;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 484F));
            tlpRoot.Controls.Add(flowLoc, 0, 0);
            tlpRoot.Controls.Add(dgv, 0, 1);
            tlpRoot.Controls.Add(lblTongKet, 0, 2);
            tlpRoot.Controls.Add(grpChiTiet, 1, 0);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.Padding = new Padding(14);
            tlpRoot.RowCount = 3;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 106F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 37F));
            tlpRoot.Size = new Size(1386, 774);
            tlpRoot.TabIndex = 0;
            // 
            // flowLoc
            // 
            flowLoc.Controls.Add(txtTim);
            flowLoc.Controls.Add(cboLocCN);
            flowLoc.Controls.Add(cboLocLoai);
            flowLoc.Controls.Add(cboLocTT);
            flowLoc.Dock = DockStyle.Fill;
            flowLoc.Location = new Point(17, 17);
            flowLoc.Name = "flowLoc";
            flowLoc.Size = new Size(868, 100);
            flowLoc.TabIndex = 0;
            // 
            // txtTim
            // 
            txtTim.Location = new Point(3, 7);
            txtTim.Margin = new Padding(3, 7, 14, 3);
            txtTim.Name = "txtTim";
            txtTim.Size = new Size(265, 30);
            txtTim.TabIndex = 0;
            txtTim.Text = "Tìm theo tên hoặc mã thiết bị...";
            // 
            // cboLocCN
            // 
            cboLocCN.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocCN.FormattingEnabled = true;
            cboLocCN.Location = new Point(285, 7);
            cboLocCN.Margin = new Padding(3, 7, 14, 3);
            cboLocCN.Name = "cboLocCN";
            cboLocCN.Size = new Size(180, 31);
            cboLocCN.TabIndex = 1;
            // 
            // cboLocLoai
            // 
            cboLocLoai.FormattingEnabled = true;
            cboLocLoai.Location = new Point(482, 7);
            cboLocLoai.Margin = new Padding(3, 7, 14, 3);
            cboLocLoai.Name = "cboLocLoai";
            cboLocLoai.Size = new Size(180, 31);
            cboLocLoai.TabIndex = 2;
            // 
            // cboLocTT
            // 
            cboLocTT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocTT.FormattingEnabled = true;
            cboLocTT.Location = new Point(679, 7);
            cboLocTT.Margin = new Padding(3, 7, 3, 3);
            cboLocTT.Name = "cboLocTT";
            cboLocTT.Size = new Size(180, 31);
            cboLocTT.TabIndex = 3;
            // 
            // dgv
            // 
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Dock = DockStyle.Fill;
            dgv.Location = new Point(17, 123);
            dgv.Name = "dgv";
            dgv.RowHeadersWidth = 51;
            dgv.Size = new Size(868, 597);
            dgv.TabIndex = 1;
            // 
            // lblTongKet
            // 
            lblTongKet.Dock = DockStyle.Fill;
            lblTongKet.Location = new Point(17, 723);
            lblTongKet.Name = "lblTongKet";
            lblTongKet.Size = new Size(868, 37);
            lblTongKet.TabIndex = 2;
            lblTongKet.Text = "label1";
            lblTongKet.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpChiTiet
            // 
            grpChiTiet.Controls.Add(tlpForm);
            grpChiTiet.Dock = DockStyle.Fill;
            grpChiTiet.Location = new Point(902, 14);
            grpChiTiet.Margin = new Padding(14, 0, 0, 0);
            grpChiTiet.Name = "grpChiTiet";
            tlpRoot.SetRowSpan(grpChiTiet, 3);
            grpChiTiet.Size = new Size(470, 746);
            grpChiTiet.TabIndex = 3;
            grpChiTiet.TabStop = false;
            grpChiTiet.Text = "Thông tin thiết bị";
            // 
            // tlpForm
            // 
            tlpForm.ColumnCount = 2;
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135F));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpForm.Controls.Add(lblMa, 0, 0);
            tlpForm.Controls.Add(lblCN, 0, 1);
            tlpForm.Controls.Add(lblTen, 0, 2);
            tlpForm.Controls.Add(lblLoai, 0, 3);
            tlpForm.Controls.Add(lblSL, 0, 4);
            tlpForm.Controls.Add(lblNgay, 0, 5);
            tlpForm.Controls.Add(lblGia, 0, 6);
            tlpForm.Controls.Add(lblTT, 0, 7);
            tlpForm.Controls.Add(lblGC, 0, 8);
            tlpForm.Controls.Add(lblGT, 0, 9);
            tlpForm.Controls.Add(flowMa, 1, 0);
            tlpForm.Controls.Add(cboChiNhanh, 1, 1);
            tlpForm.Controls.Add(txtTen, 1, 2);
            tlpForm.Controls.Add(cboLoai, 1, 3);
            tlpForm.Controls.Add(numSoLuong, 1, 4);
            tlpForm.Controls.Add(dtpNgayMua, 1, 5);
            tlpForm.Controls.Add(numDonGia, 1, 6);
            tlpForm.Controls.Add(cboTinhTrang, 1, 7);
            tlpForm.Controls.Add(txtGhiChu, 1, 8);
            tlpForm.Controls.Add(lblGiaTri, 1, 9);
            tlpForm.Controls.Add(flowNut, 0, 11);
            tlpForm.Dock = DockStyle.Fill;
            tlpForm.Location = new Point(3, 26);
            tlpForm.Margin = new Padding(3, 5, 3, 3);
            tlpForm.Name = "tlpForm";
            tlpForm.Padding = new Padding(7);
            tlpForm.RowCount = 12;
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 97F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tlpForm.Size = new Size(464, 717);
            tlpForm.TabIndex = 0;
            // 
            // lblMa
            // 
            lblMa.Dock = DockStyle.Fill;
            lblMa.Location = new Point(10, 7);
            lblMa.Name = "lblMa";
            lblMa.Size = new Size(129, 46);
            lblMa.TabIndex = 0;
            lblMa.Text = "Mã thiết bị *";
            lblMa.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCN
            // 
            lblCN.Dock = DockStyle.Fill;
            lblCN.Location = new Point(10, 53);
            lblCN.Name = "lblCN";
            lblCN.Size = new Size(129, 46);
            lblCN.TabIndex = 2;
            lblCN.Text = "Chi nhánh *";
            lblCN.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTen
            // 
            lblTen.Dock = DockStyle.Fill;
            lblTen.Location = new Point(10, 99);
            lblTen.Name = "lblTen";
            lblTen.Size = new Size(129, 46);
            lblTen.TabIndex = 4;
            lblTen.Text = "Tên thiết bị *";
            lblTen.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblLoai
            // 
            lblLoai.Dock = DockStyle.Fill;
            lblLoai.Location = new Point(10, 145);
            lblLoai.Name = "lblLoai";
            lblLoai.Size = new Size(129, 46);
            lblLoai.TabIndex = 6;
            lblLoai.Text = "Loại *";
            lblLoai.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblSL
            // 
            lblSL.Dock = DockStyle.Fill;
            lblSL.Location = new Point(10, 191);
            lblSL.Name = "lblSL";
            lblSL.Size = new Size(129, 46);
            lblSL.TabIndex = 8;
            lblSL.Text = "Số lượng *";
            lblSL.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNgay
            // 
            lblNgay.Dock = DockStyle.Fill;
            lblNgay.Location = new Point(10, 237);
            lblNgay.Name = "lblNgay";
            lblNgay.Size = new Size(129, 46);
            lblNgay.TabIndex = 10;
            lblNgay.Text = "Ngày mua";
            lblNgay.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGia
            // 
            lblGia.Dock = DockStyle.Fill;
            lblGia.Location = new Point(10, 283);
            lblGia.Name = "lblGia";
            lblGia.Size = new Size(129, 46);
            lblGia.TabIndex = 12;
            lblGia.Text = "Đơn giá mua (đ)";
            lblGia.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTT
            // 
            lblTT.Dock = DockStyle.Fill;
            lblTT.Location = new Point(10, 329);
            lblTT.Name = "lblTT";
            lblTT.Size = new Size(129, 46);
            lblTT.TabIndex = 14;
            lblTT.Text = "Tình trạng *";
            lblTT.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGC
            // 
            lblGC.Dock = DockStyle.Fill;
            lblGC.Location = new Point(10, 375);
            lblGC.Name = "lblGC";
            lblGC.Size = new Size(129, 97);
            lblGC.TabIndex = 16;
            lblGC.Text = "Ghi chú";
            lblGC.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGT
            // 
            lblGT.Dock = DockStyle.Fill;
            lblGT.Location = new Point(10, 472);
            lblGT.Name = "lblGT";
            lblGT.Size = new Size(129, 46);
            lblGT.TabIndex = 18;
            lblGT.Text = "Giá trị";
            lblGT.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowMa
            // 
            flowMa.Controls.Add(txtMaCode);
            flowMa.Controls.Add(btnGoiY);
            flowMa.Dock = DockStyle.Fill;
            flowMa.Location = new Point(145, 10);
            flowMa.Name = "flowMa";
            flowMa.Size = new Size(309, 40);
            flowMa.TabIndex = 19;
            flowMa.WrapContents = false;
            // 
            // txtMaCode
            // 
            txtMaCode.CharacterCasing = CharacterCasing.Upper;
            txtMaCode.Location = new Point(3, 7);
            txtMaCode.Margin = new Padding(3, 7, 7, 3);
            txtMaCode.MaxLength = 20;
            txtMaCode.Name = "txtMaCode";
            txtMaCode.Size = new Size(213, 30);
            txtMaCode.TabIndex = 0;
            // 
            // btnGoiY
            // 
            btnGoiY.Location = new Point(226, 3);
            btnGoiY.Name = "btnGoiY";
            btnGoiY.Size = new Size(81, 34);
            btnGoiY.TabIndex = 1;
            btnGoiY.Text = "Gợi ý";
            btnGoiY.UseVisualStyleBackColor = true;
            // 
            // cboChiNhanh
            // 
            cboChiNhanh.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboChiNhanh.FormattingEnabled = true;
            cboChiNhanh.Location = new Point(145, 62);
            cboChiNhanh.Name = "cboChiNhanh";
            cboChiNhanh.Size = new Size(309, 31);
            cboChiNhanh.TabIndex = 20;
            // 
            // txtTen
            // 
            txtTen.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTen.Location = new Point(145, 107);
            txtTen.MaxLength = 200;
            txtTen.Name = "txtTen";
            txtTen.Size = new Size(309, 30);
            txtTen.TabIndex = 21;
            // 
            // cboLoai
            // 
            cboLoai.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoai.FormattingEnabled = true;
            cboLoai.Location = new Point(145, 154);
            cboLoai.Name = "cboLoai";
            cboLoai.Size = new Size(309, 31);
            cboLoai.TabIndex = 22;
            // 
            // numSoLuong
            // 
            numSoLuong.Anchor = AnchorStyles.Left;
            numSoLuong.Font = new System.Drawing.Font("Segoe UI", 9F);
            numSoLuong.Location = new Point(145, 200);
            numSoLuong.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            numSoLuong.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSoLuong.Name = "numSoLuong";
            numSoLuong.Size = new Size(146, 27);
            numSoLuong.TabIndex = 23;
            numSoLuong.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // dtpNgayMua
            // 
            dtpNgayMua.Anchor = AnchorStyles.Left;
            dtpNgayMua.Format = DateTimePickerFormat.Short;
            dtpNgayMua.Location = new Point(145, 245);
            dtpNgayMua.Name = "dtpNgayMua";
            dtpNgayMua.ShowCheckBox = true;
            dtpNgayMua.Size = new Size(191, 30);
            dtpNgayMua.TabIndex = 24;
            // 
            // numDonGia
            // 
            numDonGia.Anchor = AnchorStyles.Left;
            numDonGia.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
            numDonGia.Location = new Point(145, 291);
            numDonGia.Maximum = new decimal(new int[] { -727379968, 232, 0, 0 });
            numDonGia.Name = "numDonGia";
            numDonGia.Size = new Size(214, 30);
            numDonGia.TabIndex = 25;
            numDonGia.ThousandsSeparator = true;
            // 
            // cboTinhTrang
            // 
            cboTinhTrang.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboTinhTrang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTinhTrang.FormattingEnabled = true;
            cboTinhTrang.Location = new Point(145, 338);
            cboTinhTrang.Name = "cboTinhTrang";
            cboTinhTrang.Size = new Size(309, 31);
            cboTinhTrang.TabIndex = 26;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Dock = DockStyle.Fill;
            txtGhiChu.Location = new Point(145, 378);
            txtGhiChu.MaxLength = 255;
            txtGhiChu.Multiline = true;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.ScrollBars = ScrollBars.Vertical;
            txtGhiChu.Size = new Size(309, 91);
            txtGhiChu.TabIndex = 27;
            // 
            // lblGiaTri
            // 
            lblGiaTri.Dock = DockStyle.Fill;
            lblGiaTri.Location = new Point(145, 472);
            lblGiaTri.Name = "lblGiaTri";
            lblGiaTri.Size = new Size(309, 46);
            lblGiaTri.TabIndex = 28;
            lblGiaTri.Text = "label1";
            lblGiaTri.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowNut
            // 
            tlpForm.SetColumnSpan(flowNut, 2);
            flowNut.Controls.Add(btnMoi);
            flowNut.Controls.Add(btnLuu);
            flowNut.Dock = DockStyle.Fill;
            flowNut.Location = new Point(10, 649);
            flowNut.Name = "flowNut";
            flowNut.Size = new Size(444, 58);
            flowNut.TabIndex = 29;
            // 
            // btnMoi
            // 
            btnMoi.Location = new Point(3, 3);
            btnMoi.Name = "btnMoi";
            btnMoi.Size = new Size(158, 41);
            btnMoi.TabIndex = 0;
            btnMoi.Text = "+ Thêm mới";
            btnMoi.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(167, 3);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(158, 41);
            btnLuu.TabIndex = 1;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // FrmThietBi
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1386, 774);
            Controls.Add(tlpRoot);
            Name = "FrmThietBi";
            Text = "Thiết bị";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)Err).EndInit();
            tlpRoot.ResumeLayout(false);
            flowLoc.ResumeLayout(false);
            flowLoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
            grpChiTiet.ResumeLayout(false);
            tlpForm.ResumeLayout(false);
            tlpForm.PerformLayout();
            flowMa.ResumeLayout(false);
            flowMa.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSoLuong).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDonGia).EndInit();
            flowNut.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRoot;
        private FlowLayoutPanel flowLoc;
        private TextBox txtTim;
        private DataGridView dgv;
        private Label lblTongKet;
        private GroupBox grpChiTiet;
        private ComboBox cboLocCN;
        private ComboBox cboLocLoai;
        private ComboBox cboLocTT;
        private TableLayoutPanel tlpForm;
        private Label lblMa;
        private Label lblCN;
        private Label lblTen;
        private Label lblLoai;
        private Label lblSL;
        private Label lblNgay;
        private Label lblGia;
        private Label lblTT;
        private Label lblGC;
        private Label lblGT;
        private FlowLayoutPanel flowMa;
        private TextBox txtMaCode;
        private Button btnGoiY;
        private ComboBox cboChiNhanh;
        private TextBox txtTen;
        private ComboBox cboLoai;
        private NumericUpDown numSoLuong;
        private DateTimePicker dtpNgayMua;
        private NumericUpDown numDonGia;
        private ComboBox cboTinhTrang;
        private TextBox txtGhiChu;
        private Label lblGiaTri;
        private FlowLayoutPanel flowNut;
        private Button btnMoi;
        private Button btnLuu;
    }
}