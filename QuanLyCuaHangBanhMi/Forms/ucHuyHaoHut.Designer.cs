namespace QuanLyCuaHangBanhMi.Forms
{
    partial class ucHuyHaoHut
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
            tabHuy = new TabControl();
            tpLap = new TabPage();
            tlpLap = new TableLayoutPanel();
            grpInfo = new GroupBox();
            tlpInfo = new TableLayoutPanel();
            lblSo = new Label();
            txtSoPhieu = new TextBox();
            lblNgay = new Label();
            txtNgay = new TextBox();
            lblCN = new Label();
            txtChiNhanh = new TextBox();
            lblNguoi = new Label();
            txtNguoiLap = new TextBox();
            lblLyDo = new Label();
            cboLyDo = new ComboBox();
            flowDong = new FlowLayoutPanel();
            lblNLCap = new Label();
            cboNL = new ComboBox();
            lblTon = new Label();
            lblSLCap = new Label();
            numSL = new NumericUpDown();
            lblDV = new Label();
            lblGCCap = new Label();
            txtGhiChuDong = new TextBox();
            btnThemDong = new Button();
            dgvCT = new DataGridView();
            tlpDuoi = new TableLayoutPanel();
            btnXoaDong = new Button();
            chkDuyetNgay = new CheckBox();
            lblTong = new Label();
            btnLamMoi = new Button();
            btnLuu = new Button();
            tpDS = new TabPage();
            tlpDS = new TableLayoutPanel();
            flowLocDS = new FlowLayoutPanel();
            txtTimPhieu = new TextBox();
            cboLocTT = new ComboBox();
            btnLamMoiDS = new Button();
            dgvPhieu = new DataGridView();
            tlpAct = new TableLayoutPanel();
            lblTongPhieu = new Label();
            flowAct = new FlowLayoutPanel();
            btnDuyet = new Button();
            btnTuChoi = new Button();
            lblChiTiet = new Label();
            dgvCTPhieu = new DataGridView();
            tabHuy.SuspendLayout();
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
            // tabHuy
            // 
            tabHuy.Appearance = TabAppearance.Buttons;
            tabHuy.Controls.Add(tpLap);
            tabHuy.Controls.Add(tpDS);
            tabHuy.Dock = DockStyle.Fill;
            tabHuy.ItemSize = new Size(170, 32);
            tabHuy.Location = new Point(0, 0);
            tabHuy.Name = "tabHuy";
            tabHuy.SelectedIndex = 0;
            tabHuy.Size = new Size(1100, 600);
            tabHuy.SizeMode = TabSizeMode.Fixed;
            tabHuy.TabIndex = 0;
            // 
            // tpLap
            // 
            tpLap.Controls.Add(tlpLap);
            tpLap.Location = new Point(4, 36);
            tpLap.Name = "tpLap";
            tpLap.Padding = new Padding(3);
            tpLap.Size = new Size(1092, 560);
            tpLap.TabIndex = 0;
            tpLap.Text = "Lập phiếu hủy";
            tpLap.UseVisualStyleBackColor = true;
            // 
            // tlpLap
            // 
            tlpLap.ColumnCount = 1;
            tlpLap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
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
            tlpLap.Size = new Size(1086, 554);
            tlpLap.TabIndex = 0;
            // 
            // grpInfo
            // 
            grpInfo.Controls.Add(tlpInfo);
            grpInfo.Dock = DockStyle.Fill;
            grpInfo.Location = new Point(11, 11);
            grpInfo.Name = "grpInfo";
            grpInfo.Size = new Size(1064, 144);
            grpInfo.TabIndex = 0;
            grpInfo.TabStop = false;
            grpInfo.Text = "Thông tin phiếu hủy";
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
            tlpInfo.Controls.Add(lblCN, 0, 1);
            tlpInfo.Controls.Add(txtChiNhanh, 1, 1);
            tlpInfo.Controls.Add(lblNguoi, 2, 1);
            tlpInfo.Controls.Add(txtNguoiLap, 3, 1);
            tlpInfo.Controls.Add(lblLyDo, 0, 2);
            tlpInfo.Controls.Add(cboLyDo, 1, 2);
            tlpInfo.Dock = DockStyle.Fill;
            tlpInfo.Location = new Point(3, 23);
            tlpInfo.Name = "tlpInfo";
            tlpInfo.Padding = new Padding(6);
            tlpInfo.RowCount = 3;
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tlpInfo.Size = new Size(1058, 118);
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
            txtSoPhieu.Location = new Point(119, 10);
            txtSoPhieu.Name = "txtSoPhieu";
            txtSoPhieu.ReadOnly = true;
            txtSoPhieu.Size = new Size(407, 27);
            txtSoPhieu.TabIndex = 1;
            // 
            // lblNgay
            // 
            lblNgay.Dock = DockStyle.Fill;
            lblNgay.Location = new Point(532, 6);
            lblNgay.Name = "lblNgay";
            lblNgay.Size = new Size(104, 36);
            lblNgay.TabIndex = 2;
            lblNgay.Text = "Ngày lập";
            lblNgay.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNgay
            // 
            txtNgay.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNgay.Location = new Point(642, 10);
            txtNgay.Name = "txtNgay";
            txtNgay.ReadOnly = true;
            txtNgay.Size = new Size(407, 27);
            txtNgay.TabIndex = 3;
            // 
            // lblCN
            // 
            lblCN.Dock = DockStyle.Fill;
            lblCN.Location = new Point(9, 42);
            lblCN.Name = "lblCN";
            lblCN.Size = new Size(104, 36);
            lblCN.TabIndex = 4;
            lblCN.Text = "Chi nhánh";
            lblCN.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtChiNhanh
            // 
            txtChiNhanh.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtChiNhanh.Location = new Point(119, 46);
            txtChiNhanh.Name = "txtChiNhanh";
            txtChiNhanh.ReadOnly = true;
            txtChiNhanh.Size = new Size(407, 27);
            txtChiNhanh.TabIndex = 5;
            // 
            // lblNguoi
            // 
            lblNguoi.Dock = DockStyle.Fill;
            lblNguoi.Location = new Point(532, 42);
            lblNguoi.Name = "lblNguoi";
            lblNguoi.Size = new Size(104, 36);
            lblNguoi.TabIndex = 6;
            lblNguoi.Text = "Người lập";
            lblNguoi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtNguoiLap
            // 
            txtNguoiLap.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNguoiLap.Location = new Point(642, 46);
            txtNguoiLap.Name = "txtNguoiLap";
            txtNguoiLap.ReadOnly = true;
            txtNguoiLap.Size = new Size(407, 27);
            txtNguoiLap.TabIndex = 7;
            // 
            // lblLyDo
            // 
            lblLyDo.Dock = DockStyle.Fill;
            lblLyDo.Location = new Point(9, 78);
            lblLyDo.Name = "lblLyDo";
            lblLyDo.Size = new Size(104, 36);
            lblLyDo.TabIndex = 8;
            lblLyDo.Text = "Lý do *";
            lblLyDo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboLyDo
            // 
            cboLyDo.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tlpInfo.SetColumnSpan(cboLyDo, 3);
            cboLyDo.FormattingEnabled = true;
            cboLyDo.Location = new Point(119, 82);
            cboLyDo.MaxLength = 255;
            cboLyDo.Name = "cboLyDo";
            cboLyDo.Size = new Size(930, 28);
            cboLyDo.TabIndex = 9;
            // 
            // flowDong
            // 
            flowDong.Controls.Add(lblNLCap);
            flowDong.Controls.Add(cboNL);
            flowDong.Controls.Add(lblTon);
            flowDong.Controls.Add(lblSLCap);
            flowDong.Controls.Add(numSL);
            flowDong.Controls.Add(lblDV);
            flowDong.Controls.Add(lblGCCap);
            flowDong.Controls.Add(txtGhiChuDong);
            flowDong.Controls.Add(btnThemDong);
            flowDong.Dock = DockStyle.Fill;
            flowDong.Location = new Point(11, 161);
            flowDong.Name = "flowDong";
            flowDong.Size = new Size(1064, 86);
            flowDong.TabIndex = 1;
            // 
            // lblNLCap
            // 
            lblNLCap.AutoSize = true;
            lblNLCap.Location = new Point(3, 9);
            lblNLCap.Margin = new Padding(3, 9, 3, 3);
            lblNLCap.Name = "lblNLCap";
            lblNLCap.Size = new Size(91, 20);
            lblNLCap.TabIndex = 0;
            lblNLCap.Text = "Nguyên liệu:";
            // 
            // cboNL
            // 
            cboNL.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboNL.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboNL.FormattingEnabled = true;
            cboNL.Location = new Point(100, 6);
            cboNL.Margin = new Padding(3, 6, 12, 3);
            cboNL.Name = "cboNL";
            cboNL.Size = new Size(244, 28);
            cboNL.TabIndex = 1;
            // 
            // lblTon
            // 
            lblTon.AutoSize = true;
            lblTon.Location = new Point(359, 9);
            lblTon.Margin = new Padding(3, 9, 18, 3);
            lblTon.Name = "lblTon";
            lblTon.Size = new Size(50, 20);
            lblTon.TabIndex = 2;
            lblTon.Text = "label1";
            // 
            // lblSLCap
            // 
            lblSLCap.AutoSize = true;
            lblSLCap.Location = new Point(430, 9);
            lblSLCap.Margin = new Padding(3, 9, 3, 3);
            lblSLCap.Name = "lblSLCap";
            lblSLCap.Size = new Size(99, 20);
            lblSLCap.TabIndex = 3;
            lblSLCap.Text = "Số lượng hủy:";
            // 
            // numSL
            // 
            numSL.DecimalPlaces = 3;
            numSL.Location = new Point(535, 6);
            numSL.Margin = new Padding(3, 6, 3, 3);
            numSL.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numSL.Name = "numSL";
            numSL.Size = new Size(120, 27);
            numSL.TabIndex = 4;
            numSL.ThousandsSeparator = true;
            // 
            // lblDV
            // 
            lblDV.AutoSize = true;
            lblDV.Location = new Point(661, 9);
            lblDV.Margin = new Padding(3, 9, 18, 3);
            lblDV.Name = "lblDV";
            lblDV.Size = new Size(15, 20);
            lblDV.TabIndex = 5;
            lblDV.Text = "-";
            // 
            // lblGCCap
            // 
            lblGCCap.AutoSize = true;
            lblGCCap.Location = new Point(697, 9);
            lblGCCap.Margin = new Padding(3, 9, 3, 3);
            lblGCCap.Name = "lblGCCap";
            lblGCCap.Size = new Size(100, 20);
            lblGCCap.TabIndex = 6;
            lblGCCap.Text = "Ghi chú dòng:";
            // 
            // txtGhiChuDong
            // 
            txtGhiChuDong.Location = new Point(803, 6);
            txtGhiChuDong.Margin = new Padding(3, 6, 12, 3);
            txtGhiChuDong.MaxLength = 255;
            txtGhiChuDong.Name = "txtGhiChuDong";
            txtGhiChuDong.Size = new Size(132, 27);
            txtGhiChuDong.TabIndex = 7;
            // 
            // btnThemDong
            // 
            btnThemDong.Location = new Point(3, 40);
            btnThemDong.Name = "btnThemDong";
            btnThemDong.Size = new Size(130, 34);
            btnThemDong.TabIndex = 8;
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
            dgvCT.Size = new Size(1064, 230);
            dgvCT.TabIndex = 2;
            // 
            // tlpDuoi
            // 
            tlpDuoi.ColumnCount = 5;
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tlpDuoi.Controls.Add(btnXoaDong, 0, 0);
            tlpDuoi.Controls.Add(chkDuyetNgay, 1, 0);
            tlpDuoi.Controls.Add(lblTong, 2, 0);
            tlpDuoi.Controls.Add(btnLamMoi, 3, 0);
            tlpDuoi.Controls.Add(btnLuu, 4, 0);
            tlpDuoi.Dock = DockStyle.Fill;
            tlpDuoi.Location = new Point(11, 489);
            tlpDuoi.Name = "tlpDuoi";
            tlpDuoi.RowCount = 1;
            tlpDuoi.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDuoi.Size = new Size(1064, 54);
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
            // chkDuyetNgay
            // 
            chkDuyetNgay.Anchor = AnchorStyles.Left;
            chkDuyetNgay.AutoSize = true;
            chkDuyetNgay.Location = new Point(133, 15);
            chkDuyetNgay.Name = "chkDuyetNgay";
            chkDuyetNgay.Size = new Size(200, 24);
            chkDuyetNgay.TabIndex = 1;
            chkDuyetNgay.Text = "Duyệt ngay (trừ kho luôn)";
            chkDuyetNgay.UseVisualStyleBackColor = true;
            // 
            // lblTong
            // 
            lblTong.Dock = DockStyle.Fill;
            lblTong.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTong.Location = new Point(383, 0);
            lblTong.Name = "lblTong";
            lblTong.Size = new Size(368, 54);
            lblTong.TabIndex = 2;
            lblTong.Text = "label1";
            lblTong.TextAlign = ContentAlignment.MiddleRight;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Dock = DockStyle.Fill;
            btnLamMoi.Location = new Point(757, 10);
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
            btnLuu.Location = new Point(867, 10);
            btnLuu.Margin = new Padding(3, 10, 3, 10);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(194, 34);
            btnLuu.TabIndex = 4;
            btnLuu.Text = "Lưu phiếu (chờ duyệt)";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // tpDS
            // 
            tpDS.Controls.Add(tlpDS);
            tpDS.Location = new Point(4, 36);
            tpDS.Name = "tpDS";
            tpDS.Padding = new Padding(3);
            tpDS.Size = new Size(1092, 560);
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
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpDS.Size = new Size(1086, 554);
            tlpDS.TabIndex = 0;
            // 
            // flowLocDS
            // 
            flowLocDS.Controls.Add(txtTimPhieu);
            flowLocDS.Controls.Add(cboLocTT);
            flowLocDS.Controls.Add(btnLamMoiDS);
            flowLocDS.Dock = DockStyle.Fill;
            flowLocDS.Location = new Point(11, 11);
            flowLocDS.Name = "flowLocDS";
            flowLocDS.Size = new Size(1064, 40);
            flowLocDS.TabIndex = 0;
            flowLocDS.WrapContents = false;
            // 
            // txtTimPhieu
            // 
            txtTimPhieu.Location = new Point(3, 8);
            txtTimPhieu.Margin = new Padding(3, 8, 12, 3);
            txtTimPhieu.Name = "txtTimPhieu";
            txtTimPhieu.PlaceholderText = "Tìm theo số phiếu, người lập hoặc lý do...";
            txtTimPhieu.Size = new Size(300, 27);
            txtTimPhieu.TabIndex = 0;
            // 
            // cboLocTT
            // 
            cboLocTT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocTT.FormattingEnabled = true;
            cboLocTT.Location = new Point(318, 8);
            cboLocTT.Margin = new Padding(3, 8, 12, 3);
            cboLocTT.Name = "cboLocTT";
            cboLocTT.Size = new Size(150, 28);
            cboLocTT.TabIndex = 1;
            // 
            // btnLamMoiDS
            // 
            btnLamMoiDS.Location = new Point(483, 5);
            btnLamMoiDS.Margin = new Padding(3, 5, 3, 3);
            btnLamMoiDS.Name = "btnLamMoiDS";
            btnLamMoiDS.Size = new Size(100, 32);
            btnLamMoiDS.TabIndex = 2;
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
            dgvPhieu.Size = new Size(1064, 201);
            dgvPhieu.TabIndex = 1;
            // 
            // tlpAct
            // 
            tlpAct.ColumnCount = 2;
            tlpAct.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpAct.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 340F));
            tlpAct.Controls.Add(lblTongPhieu, 0, 0);
            tlpAct.Controls.Add(flowAct, 1, 0);
            tlpAct.Dock = DockStyle.Fill;
            tlpAct.Location = new Point(11, 264);
            tlpAct.Name = "tlpAct";
            tlpAct.RowCount = 1;
            tlpAct.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAct.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpAct.Size = new Size(1064, 44);
            tlpAct.TabIndex = 2;
            // 
            // lblTongPhieu
            // 
            lblTongPhieu.Dock = DockStyle.Fill;
            lblTongPhieu.Location = new Point(3, 0);
            lblTongPhieu.Name = "lblTongPhieu";
            lblTongPhieu.Size = new Size(718, 44);
            lblTongPhieu.TabIndex = 0;
            lblTongPhieu.Text = "label1";
            lblTongPhieu.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowAct
            // 
            flowAct.Controls.Add(btnDuyet);
            flowAct.Controls.Add(btnTuChoi);
            flowAct.Dock = DockStyle.Fill;
            flowAct.FlowDirection = FlowDirection.RightToLeft;
            flowAct.Location = new Point(727, 3);
            flowAct.Name = "flowAct";
            flowAct.Size = new Size(334, 38);
            flowAct.TabIndex = 1;
            flowAct.WrapContents = false;
            // 
            // btnDuyet
            // 
            btnDuyet.Location = new Point(171, 3);
            btnDuyet.Name = "btnDuyet";
            btnDuyet.Size = new Size(160, 36);
            btnDuyet.TabIndex = 0;
            btnDuyet.Text = "Duyệt phiếu";
            btnDuyet.UseVisualStyleBackColor = true;
            // 
            // btnTuChoi
            // 
            btnTuChoi.Location = new Point(55, 3);
            btnTuChoi.Name = "btnTuChoi";
            btnTuChoi.Size = new Size(110, 36);
            btnTuChoi.TabIndex = 1;
            btnTuChoi.Text = "Từ chối";
            btnTuChoi.UseVisualStyleBackColor = true;
            // 
            // lblChiTiet
            // 
            lblChiTiet.Dock = DockStyle.Fill;
            lblChiTiet.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblChiTiet.Location = new Point(11, 311);
            lblChiTiet.Name = "lblChiTiet";
            lblChiTiet.Size = new Size(1064, 28);
            lblChiTiet.TabIndex = 3;
            lblChiTiet.Text = "Chi tiết phiếu";
            lblChiTiet.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dgvCTPhieu
            // 
            dgvCTPhieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCTPhieu.Dock = DockStyle.Fill;
            dgvCTPhieu.Location = new Point(11, 342);
            dgvCTPhieu.Name = "dgvCTPhieu";
            dgvCTPhieu.RowHeadersWidth = 51;
            dgvCTPhieu.Size = new Size(1064, 201);
            dgvCTPhieu.TabIndex = 4;
            // 
            // ucHuyHaoHut
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabHuy);
            Name = "ucHuyHaoHut";
            Size = new Size(1100, 600);
            tabHuy.ResumeLayout(false);
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

        private TabControl tabHuy;
        private TabPage tpLap;
        private TabPage tpDS;
        private TableLayoutPanel tlpLap;
        private GroupBox grpInfo;
        private TableLayoutPanel tlpInfo;
        private Label lblSo;
        private TextBox txtSoPhieu;
        private Label lblNgay;
        private TextBox txtNgay;
        private Label lblCN;
        private TextBox txtChiNhanh;
        private Label lblNguoi;
        private TextBox txtNguoiLap;
        private Label lblLyDo;
        private ComboBox cboLyDo;
        private FlowLayoutPanel flowDong;
        private Label lblNLCap;
        private ComboBox cboNL;
        private Label lblTon;
        private Label lblSLCap;
        private NumericUpDown numSL;
        private Label lblDV;
        private Label lblGCCap;
        private TextBox txtGhiChuDong;
        private Button btnThemDong;
        private DataGridView dgvCT;
        private TableLayoutPanel tlpDuoi;
        private Button btnXoaDong;
        private CheckBox chkDuyetNgay;
        private Label lblTong;
        private Button btnLamMoi;
        private Button btnLuu;
        private TableLayoutPanel tlpDS;
        private FlowLayoutPanel flowLocDS;
        private TextBox txtTimPhieu;
        private DataGridView dgvPhieu;
        private TableLayoutPanel tlpAct;
        private Label lblChiTiet;
        private DataGridView dgvCTPhieu;
        private ComboBox cboLocTT;
        private Button btnLamMoiDS;
        private Label lblTongPhieu;
        private FlowLayoutPanel flowAct;
        private Button btnDuyet;
        private Button btnTuChoi;
    }
}
