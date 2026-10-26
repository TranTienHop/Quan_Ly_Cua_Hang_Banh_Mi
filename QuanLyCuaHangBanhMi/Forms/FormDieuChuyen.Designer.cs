using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormDieuChuyen
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
            tabMain = new TabControl();
            tpLap = new TabPage();
            tlpLap = new TableLayoutPanel();
            grpInfo = new GroupBox();
            tlpInfo = new TableLayoutPanel();
            lblSo = new Label();
            txtSoPhieu = new TextBox();
            lblNgay = new Label();
            txtNgay = new TextBox();
            lblGui = new Label();
            cboKhoGui = new ComboBox();
            lblNhan = new Label();
            cboKhoNhan = new ComboBox();
            lblNguoi = new Label();
            txtNguoi = new TextBox();
            lblGC = new Label();
            txtGhiChu = new TextBox();
            flowDong = new FlowLayoutPanel();
            lblNLCap = new Label();
            cboNL = new ComboBox();
            lblTon = new Label();
            lblSLCap = new Label();
            numSL = new NumericUpDown();
            lblDV = new Label();
            btnThemDong = new Button();
            dgvCT = new DataGridView();
            tlpDuoi = new TableLayoutPanel();
            btnXoaDong = new Button();
            chkGuiNgay = new CheckBox();
            lblTong = new Label();
            btnLamMoi = new Button();
            btnLuu = new Button();
            tpDS = new TabPage();
            tlpDS = new TableLayoutPanel();
            flowLocDS = new FlowLayoutPanel();
            txtTimPhieu = new TextBox();
            cboLocTT = new ComboBox();
            chkCanXuLy = new CheckBox();
            btnLamMoiDS = new Button();
            dgvPhieu = new DataGridView();
            tlpAct = new TableLayoutPanel();
            lblTongPhieu = new Label();
            flowAct = new FlowLayoutPanel();
            btnNhanHang = new Button();
            btnGuiHang = new Button();
            btnHuyPhieu = new Button();
            lblChiTiet = new Label();
            dgvCTPhieu = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)Err).BeginInit();
            tabMain.SuspendLayout();
            tpLap.SuspendLayout();
            tlpLap.SuspendLayout();
            grpInfo.SuspendLayout();
            tlpInfo.SuspendLayout();
            flowDong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCT).BeginInit();
            tlpDuoi.SuspendLayout();
            tpDS.SuspendLayout();
            tlpDS.SuspendLayout();
            flowLocDS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieu).BeginInit();
            tlpAct.SuspendLayout();
            flowAct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCTPhieu).BeginInit();
            SuspendLayout();
            // 
            // tabMain
            // 
            tabMain.Appearance = TabAppearance.Buttons;
            tabMain.Controls.Add(tpLap);
            tabMain.Controls.Add(tpDS);
            tabMain.Dock = DockStyle.Fill;
            tabMain.ItemSize = new Size(190, 32);
            tabMain.Location = new Point(0, 0);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1182, 673);
            tabMain.SizeMode = TabSizeMode.Fixed;
            tabMain.TabIndex = 0;
            // 
            // tpLap
            // 
            tpLap.Controls.Add(tlpLap);
            tpLap.Location = new Point(4, 36);
            tpLap.Name = "tpLap";
            tpLap.Padding = new Padding(3);
            tpLap.Size = new Size(1174, 633);
            tpLap.TabIndex = 0;
            tpLap.Text = "Lập phiếu điều chuyển";
            tpLap.UseVisualStyleBackColor = true;
            // 
            // tlpLap
            // 
            tlpLap.ColumnCount = 1;
            tlpLap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLap.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpLap.Controls.Add(grpInfo, 0, 0);
            tlpLap.Controls.Add(flowDong, 0, 1);
            tlpLap.Controls.Add(dgvCT, 0, 2);
            tlpLap.Controls.Add(tlpDuoi, 0, 3);
            tlpLap.Dock = DockStyle.Fill;
            tlpLap.Location = new Point(3, 3);
            tlpLap.Name = "tlpLap";
            tlpLap.Padding = new Padding(8);
            tlpLap.RowCount = 4;
            tlpLap.RowStyles.Add(new RowStyle(SizeType.Absolute, 150F));
            tlpLap.RowStyles.Add(new RowStyle(SizeType.Absolute, 92F));
            tlpLap.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpLap.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tlpLap.Size = new Size(1168, 627);
            tlpLap.TabIndex = 0;
            // 
            // grpInfo
            // 
            grpInfo.Controls.Add(tlpInfo);
            grpInfo.Dock = DockStyle.Fill;
            grpInfo.Location = new Point(11, 11);
            grpInfo.Name = "grpInfo";
            grpInfo.Size = new Size(1146, 144);
            grpInfo.TabIndex = 0;
            grpInfo.TabStop = false;
            grpInfo.Text = "Thông tin phiếu điều chuyển";
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
            tlpInfo.Controls.Add(lblNgay, 2, 0);
            tlpInfo.Controls.Add(txtNgay, 3, 0);
            tlpInfo.Controls.Add(lblGui, 0, 1);
            tlpInfo.Controls.Add(cboKhoGui, 1, 1);
            tlpInfo.Controls.Add(lblNhan, 2, 1);
            tlpInfo.Controls.Add(cboKhoNhan, 3, 1);
            tlpInfo.Controls.Add(lblNguoi, 0, 2);
            tlpInfo.Controls.Add(txtNguoi, 1, 2);
            tlpInfo.Controls.Add(lblGC, 2, 2);
            tlpInfo.Controls.Add(txtGhiChu, 3, 2);
            tlpInfo.Dock = DockStyle.Fill;
            tlpInfo.Location = new Point(3, 26);
            tlpInfo.Name = "tlpInfo";
            tlpInfo.Padding = new Padding(6);
            tlpInfo.RowCount = 3;
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpInfo.Size = new Size(1140, 115);
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
            txtSoPhieu.Size = new Size(448, 30);
            txtSoPhieu.TabIndex = 1;
            // 
            // lblNgay
            // 
            lblNgay.Dock = DockStyle.Fill;
            lblNgay.Location = new Point(573, 6);
            lblNgay.Name = "lblNgay";
            lblNgay.Size = new Size(104, 36);
            lblNgay.TabIndex = 2;
            lblNgay.Text = "Ngày lập";
            lblNgay.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNgay
            // 
            txtNgay.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNgay.Location = new Point(683, 9);
            txtNgay.Name = "txtNgay";
            txtNgay.ReadOnly = true;
            txtNgay.Size = new Size(448, 30);
            txtNgay.TabIndex = 3;
            // 
            // lblGui
            // 
            lblGui.Dock = DockStyle.Fill;
            lblGui.Location = new Point(9, 42);
            lblGui.Name = "lblGui";
            lblGui.Size = new Size(104, 36);
            lblGui.TabIndex = 4;
            lblGui.Text = "Kho gửi *";
            lblGui.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboKhoGui
            // 
            cboKhoGui.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboKhoGui.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhoGui.FormattingEnabled = true;
            cboKhoGui.Location = new Point(119, 45);
            cboKhoGui.Name = "cboKhoGui";
            cboKhoGui.Size = new Size(448, 31);
            cboKhoGui.TabIndex = 5;
            // 
            // lblNhan
            // 
            lblNhan.Dock = DockStyle.Fill;
            lblNhan.Location = new Point(573, 42);
            lblNhan.Name = "lblNhan";
            lblNhan.Size = new Size(104, 36);
            lblNhan.TabIndex = 6;
            lblNhan.Text = "Kho nhận *";
            lblNhan.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboKhoNhan
            // 
            cboKhoNhan.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboKhoNhan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhoNhan.FormattingEnabled = true;
            cboKhoNhan.Location = new Point(683, 45);
            cboKhoNhan.Name = "cboKhoNhan";
            cboKhoNhan.Size = new Size(448, 31);
            cboKhoNhan.TabIndex = 7;
            // 
            // lblNguoi
            // 
            lblNguoi.Dock = DockStyle.Fill;
            lblNguoi.Location = new Point(9, 78);
            lblNguoi.Name = "lblNguoi";
            lblNguoi.Size = new Size(104, 36);
            lblNguoi.TabIndex = 8;
            lblNguoi.Text = "Người lập";
            lblNguoi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNguoi
            // 
            txtNguoi.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNguoi.Location = new Point(119, 81);
            txtNguoi.Name = "txtNguoi";
            txtNguoi.ReadOnly = true;
            txtNguoi.Size = new Size(448, 30);
            txtNguoi.TabIndex = 9;
            // 
            // lblGC
            // 
            lblGC.Dock = DockStyle.Fill;
            lblGC.Location = new Point(573, 78);
            lblGC.Name = "lblGC";
            lblGC.Size = new Size(104, 36);
            lblGC.TabIndex = 10;
            lblGC.Text = "Ghi chú";
            lblGC.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtGhiChu.Location = new Point(683, 81);
            txtGhiChu.MaxLength = 255;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(448, 30);
            txtGhiChu.TabIndex = 11;
            // 
            // flowDong
            // 
            flowDong.Controls.Add(lblNLCap);
            flowDong.Controls.Add(cboNL);
            flowDong.Controls.Add(lblTon);
            flowDong.Controls.Add(lblSLCap);
            flowDong.Controls.Add(numSL);
            flowDong.Controls.Add(lblDV);
            flowDong.Controls.Add(btnThemDong);
            flowDong.Dock = DockStyle.Fill;
            flowDong.Location = new Point(11, 161);
            flowDong.Name = "flowDong";
            flowDong.Size = new Size(1146, 86);
            flowDong.TabIndex = 1;
            // 
            // lblNLCap
            // 
            lblNLCap.AutoSize = true;
            lblNLCap.Location = new Point(3, 9);
            lblNLCap.Margin = new Padding(3, 9, 3, 3);
            lblNLCap.Name = "lblNLCap";
            lblNLCap.Size = new Size(106, 23);
            lblNLCap.TabIndex = 0;
            lblNLCap.Text = "Nguyên liệu:";
            // 
            // cboNL
            // 
            cboNL.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboNL.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboNL.FormattingEnabled = true;
            cboNL.Location = new Point(115, 6);
            cboNL.Margin = new Padding(3, 6, 12, 3);
            cboNL.Name = "cboNL";
            cboNL.Size = new Size(280, 31);
            cboNL.TabIndex = 1;
            // 
            // lblTon
            // 
            lblTon.AutoSize = true;
            lblTon.Location = new Point(410, 9);
            lblTon.Margin = new Padding(3, 9, 18, 3);
            lblTon.Name = "lblTon";
            lblTon.Size = new Size(55, 23);
            lblTon.TabIndex = 1;
            lblTon.Text = "label1";
            // 
            // lblSLCap
            // 
            lblSLCap.AutoSize = true;
            lblSLCap.Location = new Point(486, 9);
            lblSLCap.Margin = new Padding(3, 9, 3, 3);
            lblSLCap.Name = "lblSLCap";
            lblSLCap.Size = new Size(111, 23);
            lblSLCap.TabIndex = 2;
            lblSLCap.Text = "Số lượng gửi:";
            // 
            // numSL
            // 
            numSL.DecimalPlaces = 3;
            numSL.Location = new Point(603, 6);
            numSL.Margin = new Padding(3, 6, 3, 3);
            numSL.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numSL.Name = "numSL";
            numSL.Size = new Size(120, 30);
            numSL.TabIndex = 3;
            numSL.ThousandsSeparator = true;
            // 
            // lblDV
            // 
            lblDV.AutoSize = true;
            lblDV.Location = new Point(729, 9);
            lblDV.Margin = new Padding(3, 9, 18, 3);
            lblDV.Name = "lblDV";
            lblDV.Size = new Size(17, 23);
            lblDV.TabIndex = 4;
            lblDV.Text = "-";
            // 
            // btnThemDong
            // 
            btnThemDong.Location = new Point(767, 3);
            btnThemDong.Name = "btnThemDong";
            btnThemDong.Size = new Size(130, 34);
            btnThemDong.TabIndex = 5;
            btnThemDong.Text = "+ Thêm dòng";
            btnThemDong.UseVisualStyleBackColor = true;
            // 
            // dgvCT
            // 
            dgvCT.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCT.Dock = DockStyle.Fill;
            dgvCT.Location = new Point(11, 253);
            dgvCT.Name = "dgvCT";
            dgvCT.RowHeadersWidth = 51;
            dgvCT.Size = new Size(1146, 303);
            dgvCT.TabIndex = 2;
            // 
            // tlpDuoi
            // 
            tlpDuoi.ColumnCount = 5;
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 290F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            tlpDuoi.Controls.Add(btnXoaDong, 0, 0);
            tlpDuoi.Controls.Add(chkGuiNgay, 1, 0);
            tlpDuoi.Controls.Add(lblTong, 2, 0);
            tlpDuoi.Controls.Add(btnLamMoi, 3, 0);
            tlpDuoi.Controls.Add(btnLuu, 4, 0);
            tlpDuoi.Dock = DockStyle.Fill;
            tlpDuoi.Location = new Point(11, 562);
            tlpDuoi.Name = "tlpDuoi";
            tlpDuoi.RowCount = 1;
            tlpDuoi.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDuoi.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpDuoi.Size = new Size(1146, 54);
            tlpDuoi.TabIndex = 3;
            // 
            // btnXoaDong
            // 
            btnXoaDong.Dock = DockStyle.Fill;
            btnXoaDong.Location = new Point(3, 10);
            btnXoaDong.Margin = new Padding(3, 10, 3, 10);
            btnXoaDong.Name = "btnXoaDong";
            btnXoaDong.Size = new Size(124, 34);
            btnXoaDong.TabIndex = 0;
            btnXoaDong.Text = "Xóa dòng";
            btnXoaDong.UseVisualStyleBackColor = true;
            // 
            // chkGuiNgay
            // 
            chkGuiNgay.Anchor = AnchorStyles.Left;
            chkGuiNgay.AutoSize = true;
            chkGuiNgay.Location = new Point(133, 13);
            chkGuiNgay.Name = "chkGuiNgay";
            chkGuiNgay.Size = new Size(144, 27);
            chkGuiNgay.TabIndex = 1;
            chkGuiNgay.Text = "Gửi hàng ngay";
            chkGuiNgay.UseVisualStyleBackColor = true;
            // 
            // lblTong
            // 
            lblTong.Dock = DockStyle.Fill;
            lblTong.Font = new System.Drawing.Font("Segoe UI", 11F, FontStyle.Bold);
            lblTong.Location = new Point(423, 0);
            lblTong.Name = "lblTong";
            lblTong.Size = new Size(440, 54);
            lblTong.TabIndex = 2;
            lblTong.Text = "label1";
            lblTong.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Dock = DockStyle.Fill;
            btnLamMoi.Location = new Point(869, 10);
            btnLamMoi.Margin = new Padding(3, 10, 3, 10);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(104, 34);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Dock = DockStyle.Fill;
            btnLuu.Location = new Point(979, 10);
            btnLuu.Margin = new Padding(3, 10, 3, 10);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(164, 34);
            btnLuu.TabIndex = 4;
            btnLuu.Text = "Lưu phiếu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // tpDS
            // 
            tpDS.Controls.Add(tlpDS);
            tpDS.Location = new Point(4, 36);
            tpDS.Name = "tpDS";
            tpDS.Padding = new Padding(3);
            tpDS.Size = new Size(1174, 633);
            tpDS.TabIndex = 1;
            tpDS.Text = "Danh sách phiếu";
            tpDS.UseVisualStyleBackColor = true;
            // 
            // tlpDS
            // 
            tlpDS.ColumnCount = 1;
            tlpDS.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDS.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tlpDS.Controls.Add(flowLocDS, 0, 0);
            tlpDS.Controls.Add(dgvPhieu, 0, 1);
            tlpDS.Controls.Add(tlpAct, 0, 2);
            tlpDS.Controls.Add(lblChiTiet, 0, 3);
            tlpDS.Controls.Add(dgvCTPhieu, 0, 4);
            tlpDS.Dock = DockStyle.Fill;
            tlpDS.Location = new Point(3, 3);
            tlpDS.Name = "tlpDS";
            tlpDS.Padding = new Padding(8);
            tlpDS.RowCount = 5;
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Absolute, 46F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tlpDS.Size = new Size(1168, 627);
            tlpDS.TabIndex = 0;
            // 
            // flowLocDS
            // 
            flowLocDS.Controls.Add(txtTimPhieu);
            flowLocDS.Controls.Add(cboLocTT);
            flowLocDS.Controls.Add(chkCanXuLy);
            flowLocDS.Controls.Add(btnLamMoiDS);
            flowLocDS.Dock = DockStyle.Fill;
            flowLocDS.Location = new Point(11, 11);
            flowLocDS.Name = "flowLocDS";
            flowLocDS.Size = new Size(1146, 40);
            flowLocDS.TabIndex = 0;
            flowLocDS.WrapContents = false;
            // 
            // txtTimPhieu
            // 
            txtTimPhieu.Location = new Point(3, 8);
            txtTimPhieu.Margin = new Padding(3, 8, 12, 3);
            txtTimPhieu.Name = "txtTimPhieu";
            txtTimPhieu.PlaceholderText = "Tìm theo số phiếu, chi nhánh hoặc người gửi/nhận...";
            txtTimPhieu.Size = new Size(290, 30);
            txtTimPhieu.TabIndex = 0;
            // 
            // cboLocTT
            // 
            cboLocTT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocTT.FormattingEnabled = true;
            cboLocTT.Location = new Point(308, 8);
            cboLocTT.Margin = new Padding(3, 8, 12, 3);
            cboLocTT.Name = "cboLocTT";
            cboLocTT.Size = new Size(150, 31);
            cboLocTT.TabIndex = 1;
            // 
            // chkCanXuLy
            // 
            chkCanXuLy.AutoSize = true;
            chkCanXuLy.Location = new Point(473, 10);
            chkCanXuLy.Margin = new Padding(3, 10, 12, 3);
            chkCanXuLy.Name = "chkCanXuLy";
            chkCanXuLy.Size = new Size(202, 27);
            chkCanXuLy.TabIndex = 2;
            chkCanXuLy.Text = "Chỉ phiếu cần tôi xử lý";
            chkCanXuLy.UseVisualStyleBackColor = true;
            // 
            // btnLamMoiDS
            // 
            btnLamMoiDS.Location = new Point(690, 5);
            btnLamMoiDS.Margin = new Padding(3, 5, 3, 3);
            btnLamMoiDS.Name = "btnLamMoiDS";
            btnLamMoiDS.Size = new Size(100, 32);
            btnLamMoiDS.TabIndex = 3;
            btnLamMoiDS.Text = "Làm mới";
            btnLamMoiDS.UseVisualStyleBackColor = true;
            // 
            // dgvPhieu
            // 
            dgvPhieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPhieu.Dock = DockStyle.Fill;
            dgvPhieu.Location = new Point(11, 57);
            dgvPhieu.Name = "dgvPhieu";
            dgvPhieu.RowHeadersWidth = 51;
            dgvPhieu.Size = new Size(1146, 202);
            dgvPhieu.TabIndex = 1;
            // 
            // tlpAct
            // 
            tlpAct.ColumnCount = 2;
            tlpAct.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpAct.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 470F));
            tlpAct.Controls.Add(lblTongPhieu, 0, 0);
            tlpAct.Controls.Add(flowAct, 1, 0);
            tlpAct.Dock = DockStyle.Fill;
            tlpAct.Location = new Point(11, 265);
            tlpAct.Name = "tlpAct";
            tlpAct.RowCount = 1;
            tlpAct.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAct.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpAct.Size = new Size(1146, 44);
            tlpAct.TabIndex = 2;
            // 
            // lblTongPhieu
            // 
            lblTongPhieu.Dock = DockStyle.Fill;
            lblTongPhieu.Location = new Point(3, 0);
            lblTongPhieu.Name = "lblTongPhieu";
            lblTongPhieu.Size = new Size(670, 44);
            lblTongPhieu.TabIndex = 0;
            lblTongPhieu.Text = "label1";
            lblTongPhieu.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowAct
            // 
            flowAct.Controls.Add(btnNhanHang);
            flowAct.Controls.Add(btnGuiHang);
            flowAct.Controls.Add(btnHuyPhieu);
            flowAct.Dock = DockStyle.Fill;
            flowAct.FlowDirection = FlowDirection.RightToLeft;
            flowAct.Location = new Point(679, 3);
            flowAct.Name = "flowAct";
            flowAct.Size = new Size(464, 38);
            flowAct.TabIndex = 1;
            flowAct.WrapContents = false;
            // 
            // btnNhanHang
            // 
            btnNhanHang.Location = new Point(271, 3);
            btnNhanHang.Name = "btnNhanHang";
            btnNhanHang.Size = new Size(190, 36);
            btnNhanHang.TabIndex = 0;
            btnNhanHang.Text = "Xác nhận nhận hàng";
            btnNhanHang.UseVisualStyleBackColor = true;
            // 
            // btnGuiHang
            // 
            btnGuiHang.Location = new Point(135, 3);
            btnGuiHang.Name = "btnGuiHang";
            btnGuiHang.Size = new Size(130, 36);
            btnGuiHang.TabIndex = 1;
            btnGuiHang.Text = "Gửi hàng";
            btnGuiHang.UseVisualStyleBackColor = true;
            // 
            // btnHuyPhieu
            // 
            btnHuyPhieu.Location = new Point(19, 3);
            btnHuyPhieu.Name = "btnHuyPhieu";
            btnHuyPhieu.Size = new Size(110, 36);
            btnHuyPhieu.TabIndex = 2;
            btnHuyPhieu.Text = "Hủy phiếu";
            btnHuyPhieu.UseVisualStyleBackColor = true;
            // 
            // lblChiTiet
            // 
            lblChiTiet.Dock = DockStyle.Fill;
            lblChiTiet.Font = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Bold);
            lblChiTiet.Location = new Point(11, 312);
            lblChiTiet.Name = "lblChiTiet";
            lblChiTiet.Size = new Size(1146, 52);
            lblChiTiet.TabIndex = 3;
            lblChiTiet.Text = "Chi tiết phiếu";
            lblChiTiet.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvCTPhieu
            // 
            dgvCTPhieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCTPhieu.Dock = DockStyle.Fill;
            dgvCTPhieu.Location = new Point(11, 367);
            dgvCTPhieu.Name = "dgvCTPhieu";
            dgvCTPhieu.RowHeadersWidth = 51;
            dgvCTPhieu.Size = new Size(1146, 249);
            dgvCTPhieu.TabIndex = 4;
            // 
            // FormDieuChuyen
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1182, 673);
            Controls.Add(tabMain);
            MinimumSize = new Size(1000, 600);
            Name = "FormDieuChuyen";
            Text = "Điều chuyển kho";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)Err).EndInit();
            tabMain.ResumeLayout(false);
            tpLap.ResumeLayout(false);
            tlpLap.ResumeLayout(false);
            grpInfo.ResumeLayout(false);
            tlpInfo.ResumeLayout(false);
            tlpInfo.PerformLayout();
            flowDong.ResumeLayout(false);
            flowDong.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSL).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCT).EndInit();
            tlpDuoi.ResumeLayout(false);
            tlpDuoi.PerformLayout();
            tpDS.ResumeLayout(false);
            tlpDS.ResumeLayout(false);
            flowLocDS.ResumeLayout(false);
            flowLocDS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieu).EndInit();
            tlpAct.ResumeLayout(false);
            flowAct.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCTPhieu).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabMain;
        private TabPage tpLap;
        private TabPage tpDS;
        private TableLayoutPanel tlpLap;
        private GroupBox grpInfo;
        private TableLayoutPanel tlpInfo;
        private Label lblSo;
        private TextBox txtSoPhieu;
        private Label lblNgay;
        private TextBox txtNgay;
        private Label lblGui;
        private ComboBox cboKhoGui;
        private Label lblNhan;
        private ComboBox cboKhoNhan;
        private Label lblNguoi;
        private TextBox txtNguoi;
        private Label lblGC;
        private TextBox txtGhiChu;
        private FlowLayoutPanel flowDong;
        private Label lblNLCap;
        private ComboBox cboNL;
        private Label lblTon;
        private Label lblSLCap;
        private NumericUpDown numSL;
        private Label lblDV;
        private Button btnThemDong;
        private DataGridView dgvCT;
        private TableLayoutPanel tlpDuoi;
        private Button btnXoaDong;
        private CheckBox chkGuiNgay;
        private Label lblTong;
        private Button btnLamMoi;
        private Button btnLuu;
        private TableLayoutPanel tlpDS;
        private FlowLayoutPanel flowLocDS;
        private DataGridView dgvPhieu;
        private TableLayoutPanel tlpAct;
        private Label lblChiTiet;
        private DataGridView dgvCTPhieu;
        private TextBox txtTimPhieu;
        private ComboBox cboLocTT;
        private CheckBox chkCanXuLy;
        private Button btnLamMoiDS;
        private Label lblTongPhieu;
        private FlowLayoutPanel flowAct;
        private Button btnNhanHang;
        private Button btnGuiHang;
        private Button btnHuyPhieu;
    }
}