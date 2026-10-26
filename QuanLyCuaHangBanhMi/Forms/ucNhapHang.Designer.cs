namespace QuanLyCuaHangBanhMi.Forms
{
    partial class ucNhapHang
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
            tabNhap = new TabControl();
            tpLap = new TabPage();
            tpDS = new TabPage();
            tlpLap = new TableLayoutPanel();
            grpInfo = new GroupBox();
            tlpInfo = new TableLayoutPanel();
            lblSo = new Label();
            txtSoPhieu = new TextBox();
            lblNgay = new Label();
            dtpNgayDat = new DateTimePicker();
            lblNCC = new Label();
            cboNCC = new ComboBox();
            lblCN = new Label();
            txtChiNhanh = new TextBox();
            lblNguoi = new Label();
            txtNguoiLap = new TextBox();
            lblGC = new Label();
            txtGhiChu = new TextBox();
            flowDong = new FlowLayoutPanel();
            lblNLCap = new Label();
            cboNL = new ComboBox();
            lblTon = new Label();
            lblSLCap = new Label();
            numSL = new NumericUpDown();
            lblDV = new Label();
            lblGiaCap = new Label();
            numGia = new NumericUpDown();
            btnThemDong = new Button();
            dgvCT = new DataGridView();
            tlpDuoi = new TableLayoutPanel();
            btnXoaDong = new Button();
            chkNhanNgay = new CheckBox();
            lblTong = new Label();
            btnLamMoi = new Button();
            btnLuu = new Button();
            tlpDS = new TableLayoutPanel();
            flowLocDS = new FlowLayoutPanel();
            dgvPhieu = new DataGridView();
            tlpAct = new TableLayoutPanel();
            lblChiTiet = new Label();
            dgvCTPhieu = new DataGridView();
            txtTimPhieu = new TextBox();
            cboLocTT = new ComboBox();
            chkConNo = new CheckBox();
            btnLamMoiDS = new Button();
            lblTongPhieu = new Label();
            flowAct = new FlowLayoutPanel();
            btnNhanHang = new Button();
            btnThanhToan = new Button();
            btnHuyPhieu = new Button();
            tabNhap.SuspendLayout();
            tpLap.SuspendLayout();
            tpDS.SuspendLayout();
            tlpLap.SuspendLayout();
            grpInfo.SuspendLayout();
            tlpInfo.SuspendLayout();
            flowDong.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numSL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numGia).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCT).BeginInit();
            tlpDuoi.SuspendLayout();
            tlpDS.SuspendLayout();
            flowLocDS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieu).BeginInit();
            tlpAct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvCTPhieu).BeginInit();
            flowAct.SuspendLayout();
            SuspendLayout();
            // 
            // tabNhap
            // 
            tabNhap.Appearance = TabAppearance.Buttons;
            tabNhap.Controls.Add(tpLap);
            tabNhap.Controls.Add(tpDS);
            tabNhap.Dock = DockStyle.Fill;
            tabNhap.ItemSize = new Size(170, 32);
            tabNhap.Location = new Point(0, 0);
            tabNhap.Name = "tabNhap";
            tabNhap.SelectedIndex = 0;
            tabNhap.Size = new Size(1100, 600);
            tabNhap.SizeMode = TabSizeMode.Fixed;
            tabNhap.TabIndex = 0;
            // 
            // tpLap
            // 
            tpLap.Controls.Add(tlpLap);
            tpLap.Location = new Point(4, 36);
            tpLap.Name = "tpLap";
            tpLap.Padding = new Padding(3);
            tpLap.Size = new Size(1092, 560);
            tpLap.TabIndex = 0;
            tpLap.Text = "Lập phiếu mới";
            tpLap.UseVisualStyleBackColor = true;
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
            grpInfo.Text = "Thông tin phiếu nhập";
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
            tlpInfo.Controls.Add(dtpNgayDat, 3, 0);
            tlpInfo.Controls.Add(lblNCC, 0, 1);
            tlpInfo.Controls.Add(cboNCC, 1, 1);
            tlpInfo.Controls.Add(lblCN, 2, 1);
            tlpInfo.Controls.Add(txtChiNhanh, 3, 1);
            tlpInfo.Controls.Add(lblNguoi, 0, 2);
            tlpInfo.Controls.Add(txtNguoiLap, 1, 2);
            tlpInfo.Controls.Add(lblGC, 2, 2);
            tlpInfo.Controls.Add(txtGhiChu, 3, 2);
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
            txtSoPhieu.Text = "(Tự động)";
            // 
            // lblNgay
            // 
            lblNgay.Dock = DockStyle.Fill;
            lblNgay.Location = new Point(532, 6);
            lblNgay.Name = "lblNgay";
            lblNgay.Size = new Size(104, 36);
            lblNgay.TabIndex = 2;
            lblNgay.Text = "Ngày đặt";
            lblNgay.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // dtpNgayDat
            // 
            dtpNgayDat.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            dtpNgayDat.Format = DateTimePickerFormat.Short;
            dtpNgayDat.Location = new Point(642, 9);
            dtpNgayDat.Name = "dtpNgayDat";
            dtpNgayDat.Size = new Size(407, 27);
            dtpNgayDat.TabIndex = 1;
            // 
            // lblNCC
            // 
            lblNCC.Dock = DockStyle.Fill;
            lblNCC.Location = new Point(9, 42);
            lblNCC.Name = "lblNCC";
            lblNCC.Size = new Size(104, 36);
            lblNCC.TabIndex = 4;
            lblNCC.Text = "Nhà cung cấp *";
            lblNCC.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cboNCC
            // 
            cboNCC.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboNCC.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNCC.FormattingEnabled = true;
            cboNCC.Location = new Point(119, 45);
            cboNCC.Name = "cboNCC";
            cboNCC.Size = new Size(407, 28);
            cboNCC.TabIndex = 0;
            // 
            // lblCN
            // 
            lblCN.Dock = DockStyle.Fill;
            lblCN.Location = new Point(532, 42);
            lblCN.Name = "lblCN";
            lblCN.Size = new Size(104, 36);
            lblCN.TabIndex = 6;
            lblCN.Text = "Chi nhánh";
            lblCN.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtChiNhanh
            // 
            txtChiNhanh.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtChiNhanh.Location = new Point(642, 46);
            txtChiNhanh.Name = "txtChiNhanh";
            txtChiNhanh.ReadOnly = true;
            txtChiNhanh.Size = new Size(407, 27);
            txtChiNhanh.TabIndex = 7;
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
            // txtNguoiLap
            // 
            txtNguoiLap.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtNguoiLap.Location = new Point(119, 82);
            txtNguoiLap.Name = "txtNguoiLap";
            txtNguoiLap.ReadOnly = true;
            txtNguoiLap.Size = new Size(407, 27);
            txtNguoiLap.TabIndex = 3;
            // 
            // lblGC
            // 
            lblGC.Dock = DockStyle.Fill;
            lblGC.Location = new Point(532, 78);
            lblGC.Name = "lblGC";
            lblGC.Size = new Size(104, 36);
            lblGC.TabIndex = 10;
            lblGC.Text = "Ghi chú";
            lblGC.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtGhiChu.Location = new Point(642, 82);
            txtGhiChu.MaxLength = 255;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(407, 27);
            txtGhiChu.TabIndex = 2;
            // 
            // flowDong
            // 
            flowDong.Controls.Add(lblNLCap);
            flowDong.Controls.Add(cboNL);
            flowDong.Controls.Add(lblTon);
            flowDong.Controls.Add(lblSLCap);
            flowDong.Controls.Add(numSL);
            flowDong.Controls.Add(lblDV);
            flowDong.Controls.Add(lblGiaCap);
            flowDong.Controls.Add(numGia);
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
            cboNL.Size = new Size(280, 28);
            cboNL.TabIndex = 3;
            // 
            // lblTon
            // 
            lblTon.AutoSize = true;
            lblTon.Location = new Point(395, 9);
            lblTon.Margin = new Padding(3, 9, 18, 3);
            lblTon.Name = "lblTon";
            lblTon.Size = new Size(50, 20);
            lblTon.TabIndex = 2;
            lblTon.Text = "label1";
            // 
            // lblSLCap
            // 
            lblSLCap.AutoSize = true;
            lblSLCap.Location = new Point(466, 9);
            lblSLCap.Margin = new Padding(3, 9, 3, 3);
            lblSLCap.Name = "lblSLCap";
            lblSLCap.Size = new Size(72, 20);
            lblSLCap.TabIndex = 3;
            lblSLCap.Text = "Số lượng:";
            // 
            // numSL
            // 
            numSL.DecimalPlaces = 3;
            numSL.Location = new Point(544, 6);
            numSL.Margin = new Padding(3, 6, 3, 3);
            numSL.Maximum = new decimal(new int[] { 10000000, 0, 0, 0 });
            numSL.Name = "numSL";
            numSL.Size = new Size(120, 27);
            numSL.TabIndex = 4;
            numSL.ThousandsSeparator = true;
            // 
            // lblDV
            // 
            lblDV.AutoSize = true;
            lblDV.Location = new Point(670, 9);
            lblDV.Margin = new Padding(3, 9, 18, 3);
            lblDV.Name = "lblDV";
            lblDV.Size = new Size(15, 20);
            lblDV.TabIndex = 5;
            lblDV.Text = "-";
            // 
            // lblGiaCap
            // 
            lblGiaCap.AutoSize = true;
            lblGiaCap.Location = new Point(706, 9);
            lblGiaCap.Margin = new Padding(3, 9, 3, 3);
            lblGiaCap.Name = "lblGiaCap";
            lblGiaCap.Size = new Size(135, 20);
            lblGiaCap.TabIndex = 6;
            lblGiaCap.Text = "Đơn giá (đ/đơn vị):";
            // 
            // numGia
            // 
            numGia.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            numGia.Location = new Point(847, 6);
            numGia.Margin = new Padding(3, 6, 12, 3);
            numGia.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numGia.Name = "numGia";
            numGia.Size = new Size(130, 27);
            numGia.TabIndex = 5;
            numGia.ThousandsSeparator = true;
            // 
            // btnThemDong
            // 
            btnThemDong.Location = new Point(3, 40);
            btnThemDong.Name = "btnThemDong";
            btnThemDong.Size = new Size(130, 34);
            btnThemDong.TabIndex = 6;
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
            dgvCT.TabIndex = 7;
            // 
            // tlpDuoi
            // 
            tlpDuoi.ColumnCount = 5;
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 270F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160F));
            tlpDuoi.Controls.Add(btnXoaDong, 0, 0);
            tlpDuoi.Controls.Add(chkNhanNgay, 1, 0);
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
            // chkNhanNgay
            // 
            chkNhanNgay.Anchor = AnchorStyles.Left;
            chkNhanNgay.AutoSize = true;
            chkNhanNgay.Location = new Point(133, 15);
            chkNhanNgay.Name = "chkNhanNgay";
            chkNhanNgay.Size = new Size(247, 24);
            chkNhanNgay.TabIndex = 8;
            chkNhanNgay.Text = "Nhận hàng ngay (công kho luôn)";
            chkNhanNgay.UseVisualStyleBackColor = true;
            // 
            // lblTong
            // 
            lblTong.Dock = DockStyle.Fill;
            lblTong.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTong.Location = new Point(403, 0);
            lblTong.Name = "lblTong";
            lblTong.Size = new Size(388, 54);
            lblTong.TabIndex = 2;
            lblTong.Text = "label1";
            lblTong.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Dock = DockStyle.Fill;
            btnLamMoi.Location = new Point(797, 10);
            btnLamMoi.Margin = new Padding(3, 10, 3, 10);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(104, 34);
            btnLamMoi.TabIndex = 3;
            btnLamMoi.Text = "Làm mơi";
            btnLamMoi.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Dock = DockStyle.Fill;
            btnLuu.Location = new Point(907, 10);
            btnLuu.Margin = new Padding(3, 10, 3, 10);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(154, 34);
            btnLuu.TabIndex = 9;
            btnLuu.Text = "Lưu phiếu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // tlpDS
            // 
            tlpDS.ColumnCount = 1;
            tlpDS.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
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
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpDS.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tlpDS.Size = new Size(1086, 554);
            tlpDS.TabIndex = 0;
            // 
            // flowLocDS
            // 
            flowLocDS.Controls.Add(txtTimPhieu);
            flowLocDS.Controls.Add(cboLocTT);
            flowLocDS.Controls.Add(chkConNo);
            flowLocDS.Controls.Add(btnLamMoiDS);
            flowLocDS.Dock = DockStyle.Fill;
            flowLocDS.Location = new Point(11, 11);
            flowLocDS.Name = "flowLocDS";
            flowLocDS.Size = new Size(1064, 40);
            flowLocDS.TabIndex = 0;
            flowLocDS.WrapContents = false;
            // 
            // dgvPhieu
            // 
            dgvPhieu.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPhieu.Dock = DockStyle.Fill;
            dgvPhieu.Location = new Point(11, 57);
            dgvPhieu.Name = "dgvPhieu";
            dgvPhieu.RowHeadersWidth = 51;
            dgvPhieu.Size = new Size(1064, 221);
            dgvPhieu.TabIndex = 1;
            // 
            // tlpAct
            // 
            tlpAct.ColumnCount = 2;
            tlpAct.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpAct.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 480F));
            tlpAct.Controls.Add(lblTongPhieu, 0, 0);
            tlpAct.Controls.Add(flowAct, 1, 0);
            tlpAct.Dock = DockStyle.Fill;
            tlpAct.Location = new Point(11, 284);
            tlpAct.Name = "tlpAct";
            tlpAct.RowCount = 1;
            tlpAct.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpAct.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpAct.Size = new Size(1064, 44);
            tlpAct.TabIndex = 2;
            // 
            // lblChiTiet
            // 
            lblChiTiet.Dock = DockStyle.Fill;
            lblChiTiet.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblChiTiet.Location = new Point(11, 331);
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
            dgvCTPhieu.Location = new Point(11, 362);
            dgvCTPhieu.Name = "dgvCTPhieu";
            dgvCTPhieu.RowHeadersWidth = 51;
            dgvCTPhieu.Size = new Size(1064, 181);
            dgvCTPhieu.TabIndex = 4;
            // 
            // txtTimPhieu
            // 
            txtTimPhieu.Location = new Point(3, 8);
            txtTimPhieu.Margin = new Padding(3, 8, 12, 3);
            txtTimPhieu.Name = "txtTimPhieu";
            txtTimPhieu.PlaceholderText = "Tìm theo số phiếu hoặc nhà cung cấp...";
            txtTimPhieu.Size = new Size(270, 27);
            txtTimPhieu.TabIndex = 0;
            // 
            // cboLocTT
            // 
            cboLocTT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocTT.FormattingEnabled = true;
            cboLocTT.Location = new Point(288, 8);
            cboLocTT.Margin = new Padding(3, 8, 12, 3);
            cboLocTT.Name = "cboLocTT";
            cboLocTT.Size = new Size(150, 28);
            cboLocTT.TabIndex = 1;
            // 
            // chkConNo
            // 
            chkConNo.AutoSize = true;
            chkConNo.Location = new Point(453, 10);
            chkConNo.Margin = new Padding(3, 10, 12, 3);
            chkConNo.Name = "chkConNo";
            chkConNo.Size = new Size(142, 24);
            chkConNo.TabIndex = 2;
            chkConNo.Text = "Chỉ phiếu còn nợ";
            chkConNo.UseVisualStyleBackColor = true;
            // 
            // btnLamMoiDS
            // 
            btnLamMoiDS.Location = new Point(610, 5);
            btnLamMoiDS.Margin = new Padding(3, 5, 3, 3);
            btnLamMoiDS.Name = "btnLamMoiDS";
            btnLamMoiDS.Size = new Size(100, 32);
            btnLamMoiDS.TabIndex = 3;
            btnLamMoiDS.Text = "Làm mới";
            btnLamMoiDS.UseVisualStyleBackColor = true;
            // 
            // lblTongPhieu
            // 
            lblTongPhieu.Dock = DockStyle.Fill;
            lblTongPhieu.Location = new Point(3, 0);
            lblTongPhieu.Name = "lblTongPhieu";
            lblTongPhieu.Size = new Size(578, 44);
            lblTongPhieu.TabIndex = 0;
            lblTongPhieu.Text = "label1";
            lblTongPhieu.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowAct
            // 
            flowAct.Controls.Add(btnNhanHang);
            flowAct.Controls.Add(btnThanhToan);
            flowAct.Controls.Add(btnHuyPhieu);
            flowAct.Dock = DockStyle.Fill;
            flowAct.FlowDirection = FlowDirection.RightToLeft;
            flowAct.Location = new Point(587, 3);
            flowAct.Name = "flowAct";
            flowAct.Size = new Size(474, 38);
            flowAct.TabIndex = 1;
            flowAct.WrapContents = false;
            // 
            // btnNhanHang
            // 
            btnNhanHang.Location = new Point(281, 3);
            btnNhanHang.Name = "btnNhanHang";
            btnNhanHang.Size = new Size(190, 36);
            btnNhanHang.TabIndex = 0;
            btnNhanHang.Text = "Xác nhận nhận hàng";
            btnNhanHang.UseVisualStyleBackColor = true;
            // 
            // btnThanhToan
            // 
            btnThanhToan.Location = new Point(155, 3);
            btnThanhToan.Name = "btnThanhToan";
            btnThanhToan.Size = new Size(120, 36);
            btnThanhToan.TabIndex = 1;
            btnThanhToan.Text = "Thanh toán";
            btnThanhToan.UseVisualStyleBackColor = true;
            // 
            // btnHuyPhieu
            // 
            btnHuyPhieu.Location = new Point(39, 3);
            btnHuyPhieu.Name = "btnHuyPhieu";
            btnHuyPhieu.Size = new Size(110, 36);
            btnHuyPhieu.TabIndex = 2;
            btnHuyPhieu.Text = "Hủy phiếu";
            btnHuyPhieu.UseVisualStyleBackColor = true;
            // 
            // ucNhapHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tabNhap);
            Name = "ucNhapHang";
            Size = new Size(1100, 600);
            tabNhap.ResumeLayout(false);
            tpLap.ResumeLayout(false);
            tpDS.ResumeLayout(false);
            tlpLap.ResumeLayout(false);
            grpInfo.ResumeLayout(false);
            tlpInfo.ResumeLayout(false);
            tlpInfo.PerformLayout();
            flowDong.ResumeLayout(false);
            flowDong.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numSL).EndInit();
            ((System.ComponentModel.ISupportInitialize)numGia).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCT).EndInit();
            tlpDuoi.ResumeLayout(false);
            tlpDuoi.PerformLayout();
            tlpDS.ResumeLayout(false);
            flowLocDS.ResumeLayout(false);
            flowLocDS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPhieu).EndInit();
            tlpAct.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvCTPhieu).EndInit();
            flowAct.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabNhap;
        private TabPage tpLap;
        private TabPage tpDS;
        private TableLayoutPanel tlpLap;
        private GroupBox grpInfo;
        private TableLayoutPanel tlpInfo;
        private Label lblSo;
        private TextBox txtSoPhieu;
        private Label lblNgay;
        private DateTimePicker dtpNgayDat;
        private Label lblNCC;
        private ComboBox cboNCC;
        private Label lblCN;
        private TextBox txtChiNhanh;
        private Label lblNguoi;
        private TextBox txtNguoiLap;
        private Label lblGC;
        private TextBox txtGhiChu;
        private FlowLayoutPanel flowDong;
        private Label lblNLCap;
        private ComboBox cboNL;
        private Label lblTon;
        private Label lblSLCap;
        private NumericUpDown numSL;
        private Label lblDV;
        private Label lblGiaCap;
        private NumericUpDown numGia;
        private Button btnThemDong;
        private DataGridView dgvCT;
        private TableLayoutPanel tlpDuoi;
        private Button btnXoaDong;
        private CheckBox chkNhanNgay;
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
        private CheckBox chkConNo;
        private Button btnLamMoiDS;
        private Label lblTongPhieu;
        private FlowLayoutPanel flowAct;
        private Button btnNhanHang;
        private Button btnThanhToan;
        private Button btnHuyPhieu;
    }
}
