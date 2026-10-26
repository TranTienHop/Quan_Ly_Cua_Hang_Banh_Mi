using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormKiemKho
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
            spl = new SplitContainer();
            tlpTrai = new TableLayoutPanel();
            cboChiNhanh = new ComboBox();
            btnBatDau = new Button();
            dgvPhieu = new DataGridView();
            lblPhieuTong = new Label();
            tlpPhai = new TableLayoutPanel();
            grpInfo = new GroupBox();
            tlpInfo = new TableLayoutPanel();
            lblSo = new Label();
            txtSoPhieu = new TextBox();
            lblTT = new Label();
            txtTrangThai = new TextBox();
            lblNgay = new Label();
            txtNgay = new TextBox();
            lblNguoi = new Label();
            txtNguoi = new TextBox();
            lblGC = new Label();
            txtGhiChu = new TextBox();
            flowLoc = new FlowLayoutPanel();
            chkChiLech = new CheckBox();
            txtTimNL = new TextBox();
            dgvCT = new DataGridView();
            lblTomTat = new Label();
            flowNut = new FlowLayoutPanel();
            btnHoanThanh = new Button();
            btnXoaPhieu = new Button();
            ((System.ComponentModel.ISupportInitialize)Err).BeginInit();
            ((System.ComponentModel.ISupportInitialize)spl).BeginInit();
            spl.Panel1.SuspendLayout();
            spl.Panel2.SuspendLayout();
            spl.SuspendLayout();
            tlpTrai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieu).BeginInit();
            tlpPhai.SuspendLayout();
            grpInfo.SuspendLayout();
            tlpInfo.SuspendLayout();
            flowLoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCT).BeginInit();
            flowNut.SuspendLayout();
            SuspendLayout();
            // 
            // spl
            // 
            spl.Dock = DockStyle.Fill;
            spl.FixedPanel = FixedPanel.Panel1;
            spl.Location = new Point(0, 0);
            spl.Name = "spl";
            // 
            // spl.Panel1
            // 
            spl.Panel1.Controls.Add(tlpTrai);
            spl.Panel1.Padding = new Padding(12, 12, 6, 12);
            spl.Panel1MinSize = 330;
            // 
            // spl.Panel2
            // 
            spl.Panel2.Controls.Add(tlpPhai);
            spl.Panel2.Padding = new Padding(6, 12, 12, 12);
            spl.Size = new Size(1232, 673);
            spl.SplitterDistance = 400;
            spl.TabIndex = 0;
            // 
            // tlpTrai
            // 
            tlpTrai.ColumnCount = 1;
            tlpTrai.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTrai.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpTrai.Controls.Add(cboChiNhanh, 0, 0);
            tlpTrai.Controls.Add(btnBatDau, 0, 1);
            tlpTrai.Controls.Add(dgvPhieu, 0, 2);
            tlpTrai.Controls.Add(lblPhieuTong, 0, 3);
            tlpTrai.Dock = DockStyle.Fill;
            tlpTrai.Location = new Point(12, 12);
            tlpTrai.Name = "tlpTrai";
            tlpTrai.RowCount = 4;
            tlpTrai.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpTrai.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpTrai.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTrai.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpTrai.Size = new Size(382, 649);
            tlpTrai.TabIndex = 0;
            // 
            // cboChiNhanh
            // 
            cboChiNhanh.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboChiNhanh.FormattingEnabled = true;
            cboChiNhanh.Location = new Point(3, 8);
            cboChiNhanh.Margin = new Padding(3, 8, 3, 3);
            cboChiNhanh.Name = "cboChiNhanh";
            cboChiNhanh.Size = new Size(376, 31);
            cboChiNhanh.TabIndex = 0;
            // 
            // btnBatDau
            // 
            btnBatDau.Dock = DockStyle.Fill;
            btnBatDau.Location = new Point(3, 43);
            btnBatDau.Name = "btnBatDau";
            btnBatDau.Size = new Size(376, 40);
            btnBatDau.TabIndex = 1;
            btnBatDau.Text = "+ Bắt đầu kiểm kho";
            btnBatDau.UseVisualStyleBackColor = true;
            // 
            // dgvPhieu
            // 
            dgvPhieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPhieu.Dock = DockStyle.Fill;
            dgvPhieu.Location = new Point(3, 89);
            dgvPhieu.Name = "dgvPhieu";
            dgvPhieu.RowHeadersWidth = 51;
            dgvPhieu.Size = new Size(376, 527);
            dgvPhieu.TabIndex = 2;
            // 
            // lblPhieuTong
            // 
            lblPhieuTong.Dock = DockStyle.Fill;
            lblPhieuTong.Location = new Point(3, 619);
            lblPhieuTong.Name = "lblPhieuTong";
            lblPhieuTong.Size = new Size(376, 30);
            lblPhieuTong.TabIndex = 3;
            lblPhieuTong.Text = "label1";
            lblPhieuTong.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tlpPhai
            // 
            tlpPhai.ColumnCount = 1;
            tlpPhai.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPhai.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpPhai.Controls.Add(grpInfo, 0, 0);
            tlpPhai.Controls.Add(flowLoc, 0, 1);
            tlpPhai.Controls.Add(dgvCT, 0, 2);
            tlpPhai.Controls.Add(lblTomTat, 0, 3);
            tlpPhai.Controls.Add(flowNut, 0, 4);
            tlpPhai.Dock = DockStyle.Fill;
            tlpPhai.Location = new Point(6, 12);
            tlpPhai.Name = "tlpPhai";
            tlpPhai.RowCount = 5;
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpPhai.Size = new Size(810, 649);
            tlpPhai.TabIndex = 0;
            // 
            // grpInfo
            // 
            grpInfo.Controls.Add(tlpInfo);
            grpInfo.Dock = DockStyle.Fill;
            grpInfo.Location = new Point(3, 3);
            grpInfo.Name = "grpInfo";
            grpInfo.Size = new Size(804, 144);
            grpInfo.TabIndex = 0;
            grpInfo.TabStop = false;
            grpInfo.Text = "Thông tin phiếu kiểm kho";
            // 
            // tlpInfo
            // 
            tlpInfo.ColumnCount = 4;
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpInfo.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpInfo.Controls.Add(lblSo, 0, 0);
            tlpInfo.Controls.Add(txtSoPhieu, 1, 0);
            tlpInfo.Controls.Add(lblTT, 2, 0);
            tlpInfo.Controls.Add(txtTrangThai, 3, 0);
            tlpInfo.Controls.Add(lblNgay, 0, 1);
            tlpInfo.Controls.Add(txtNgay, 1, 1);
            tlpInfo.Controls.Add(lblNguoi, 2, 1);
            tlpInfo.Controls.Add(txtNguoi, 3, 1);
            tlpInfo.Controls.Add(lblGC, 0, 2);
            tlpInfo.Controls.Add(txtGhiChu, 1, 2);
            tlpInfo.Dock = DockStyle.Fill;
            tlpInfo.Location = new Point(3, 26);
            tlpInfo.Name = "tlpInfo";
            tlpInfo.Padding = new Padding(6);
            tlpInfo.RowCount = 3;
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpInfo.Size = new Size(798, 115);
            tlpInfo.TabIndex = 0;
            // 
            // lblSo
            // 
            lblSo.Dock = DockStyle.Fill;
            lblSo.Location = new Point(9, 6);
            lblSo.Name = "lblSo";
            lblSo.Size = new Size(104, 36);
            lblSo.TabIndex = 0;
            lblSo.Text = "Số phiếu";
            lblSo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtSoPhieu
            // 
            txtSoPhieu.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtSoPhieu.Location = new Point(119, 9);
            txtSoPhieu.Name = "txtSoPhieu";
            txtSoPhieu.ReadOnly = true;
            txtSoPhieu.Size = new Size(277, 30);
            txtSoPhieu.TabIndex = 1;
            // 
            // lblTT
            // 
            lblTT.Dock = DockStyle.Fill;
            lblTT.Location = new Point(402, 6);
            lblTT.Name = "lblTT";
            lblTT.Size = new Size(104, 36);
            lblTT.TabIndex = 2;
            lblTT.Text = "Trạng thái";
            lblTT.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTrangThai
            // 
            txtTrangThai.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTrangThai.Location = new Point(512, 9);
            txtTrangThai.Name = "txtTrangThai";
            txtTrangThai.ReadOnly = true;
            txtTrangThai.Size = new Size(277, 30);
            txtTrangThai.TabIndex = 3;
            // 
            // lblNgay
            // 
            lblNgay.Dock = DockStyle.Fill;
            lblNgay.Location = new Point(9, 42);
            lblNgay.Name = "lblNgay";
            lblNgay.Size = new Size(104, 36);
            lblNgay.TabIndex = 4;
            lblNgay.Text = "Bắt đầu lúc";
            lblNgay.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNgay
            // 
            txtNgay.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNgay.Location = new Point(119, 45);
            txtNgay.Name = "txtNgay";
            txtNgay.ReadOnly = true;
            txtNgay.Size = new Size(277, 30);
            txtNgay.TabIndex = 5;
            // 
            // lblNguoi
            // 
            lblNguoi.Dock = DockStyle.Fill;
            lblNguoi.Location = new Point(402, 42);
            lblNguoi.Name = "lblNguoi";
            lblNguoi.Size = new Size(104, 36);
            lblNguoi.TabIndex = 6;
            lblNguoi.Text = "Người kiểm";
            lblNguoi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNguoi
            // 
            txtNguoi.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNguoi.Location = new Point(512, 45);
            txtNguoi.Name = "txtNguoi";
            txtNguoi.ReadOnly = true;
            txtNguoi.Size = new Size(277, 30);
            txtNguoi.TabIndex = 7;
            // 
            // lblGC
            // 
            lblGC.Dock = DockStyle.Fill;
            lblGC.Location = new Point(9, 78);
            lblGC.Name = "lblGC";
            lblGC.Size = new Size(104, 36);
            lblGC.TabIndex = 8;
            lblGC.Text = "Ghi chú";
            lblGC.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpInfo.SetColumnSpan(txtGhiChu, 3);
            txtGhiChu.Location = new Point(119, 81);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.ReadOnly = true;
            txtGhiChu.Size = new Size(670, 30);
            txtGhiChu.TabIndex = 9;
            // 
            // flowLoc
            // 
            flowLoc.Controls.Add(chkChiLech);
            flowLoc.Controls.Add(txtTimNL);
            flowLoc.Dock = DockStyle.Fill;
            flowLoc.Location = new Point(3, 153);
            flowLoc.Name = "flowLoc";
            flowLoc.Size = new Size(804, 40);
            flowLoc.TabIndex = 1;
            flowLoc.WrapContents = false;
            // 
            // chkChiLech
            // 
            chkChiLech.AutoSize = true;
            chkChiLech.Location = new Point(3, 10);
            chkChiLech.Margin = new Padding(3, 10, 18, 3);
            chkChiLech.Name = "chkChiLech";
            chkChiLech.Size = new Size(176, 27);
            chkChiLech.TabIndex = 0;
            chkChiLech.Text = "Chỉ hiện dòng lệch";
            chkChiLech.UseVisualStyleBackColor = true;
            // 
            // txtTimNL
            // 
            txtTimNL.Location = new Point(200, 8);
            txtTimNL.Margin = new Padding(3, 8, 3, 3);
            txtTimNL.Name = "txtTimNL";
            txtTimNL.PlaceholderText = "Tìm nguyên liệu...";
            txtTimNL.Size = new Size(260, 30);
            txtTimNL.TabIndex = 1;
            // 
            // dgvCT
            // 
            dgvCT.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCT.Dock = DockStyle.Fill;
            dgvCT.Location = new Point(3, 199);
            dgvCT.Name = "dgvCT";
            dgvCT.RowHeadersWidth = 51;
            dgvCT.Size = new Size(804, 361);
            dgvCT.TabIndex = 2;
            // 
            // lblTomTat
            // 
            lblTomTat.Dock = DockStyle.Fill;
            lblTomTat.Location = new Point(3, 563);
            lblTomTat.Name = "lblTomTat";
            lblTomTat.Size = new Size(804, 30);
            lblTomTat.TabIndex = 3;
            lblTomTat.Text = "label1";
            lblTomTat.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowNut
            // 
            flowNut.Controls.Add(btnHoanThanh);
            flowNut.Controls.Add(btnXoaPhieu);
            flowNut.Dock = DockStyle.Fill;
            flowNut.FlowDirection = FlowDirection.RightToLeft;
            flowNut.Location = new Point(3, 596);
            flowNut.Name = "flowNut";
            flowNut.Size = new Size(804, 50);
            flowNut.TabIndex = 4;
            flowNut.WrapContents = false;
            // 
            // btnHoanThanh
            // 
            btnHoanThanh.Location = new Point(551, 3);
            btnHoanThanh.Name = "btnHoanThanh";
            btnHoanThanh.Size = new Size(250, 36);
            btnHoanThanh.TabIndex = 0;
            btnHoanThanh.Text = "Hoàn thành - điều chỉnh kho";
            btnHoanThanh.UseVisualStyleBackColor = true;
            // 
            // btnXoaPhieu
            // 
            btnXoaPhieu.Location = new Point(425, 3);
            btnXoaPhieu.Name = "btnXoaPhieu";
            btnXoaPhieu.Size = new Size(120, 36);
            btnXoaPhieu.TabIndex = 1;
            btnXoaPhieu.Text = "Xóa phiếu";
            btnXoaPhieu.UseVisualStyleBackColor = true;
            // 
            // FormKiemKho
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1232, 673);
            Controls.Add(spl);
            MinimumSize = new Size(1050, 600);
            Name = "FormKiemKho";
            Text = "Kiểm kho";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)Err).EndInit();
            spl.Panel1.ResumeLayout(false);
            spl.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)spl).EndInit();
            spl.ResumeLayout(false);
            tlpTrai.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPhieu).EndInit();
            tlpPhai.ResumeLayout(false);
            grpInfo.ResumeLayout(false);
            tlpInfo.ResumeLayout(false);
            tlpInfo.PerformLayout();
            flowLoc.ResumeLayout(false);
            flowLoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCT).EndInit();
            flowNut.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer spl;
        private TableLayoutPanel tlpTrai;
        private ComboBox cboChiNhanh;
        private Button btnBatDau;
        private DataGridView dgvPhieu;
        private Label lblPhieuTong;
        private TableLayoutPanel tlpPhai;
        private GroupBox grpInfo;
        private TableLayoutPanel tlpInfo;
        private FlowLayoutPanel flowLoc;
        private DataGridView dgvCT;
        private Label lblTomTat;
        private FlowLayoutPanel flowNut;
        private Label lblSo;
        private TextBox txtSoPhieu;
        private Label lblTT;
        private TextBox txtTrangThai;
        private Label lblNgay;
        private TextBox txtNgay;
        private Label lblNguoi;
        private TextBox txtNguoi;
        private Label lblGC;
        private TextBox txtGhiChu;
        private CheckBox chkChiLech;
        private TextBox txtTimNL;
        private Button btnHoanThanh;
        private Button btnXoaPhieu;
    }
}