using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormNguyenLieu
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
            tpDanhMuc = new TabPage();
            tlpDM = new TableLayoutPanel();
            flowLoc = new FlowLayoutPanel();
            txtTim = new TextBox();
            cboLoc = new ComboBox();
            dgvNL = new DataGridView();
            lblTongKet = new Label();
            grpNL = new GroupBox();
            tlpForm = new TableLayoutPanel();
            lblTK = new Label();
            lblTT = new Label();
            lblTonMin = new Label();
            lblDonVi = new Label();
            lblTen = new Label();
            lblMa = new Label();
            txtMa = new TextBox();
            txtTen = new TextBox();
            cboDonVi = new ComboBox();
            numTonMin = new NumericUpDown();
            chkHoatDong = new CheckBox();
            lblThongKe = new Label();
            btnDungTrong = new Button();
            flowNut = new FlowLayoutPanel();
            btnMoi = new Button();
            btnLuu = new Button();
            tpCongThuc = new TabPage();
            tlpCT = new TableLayoutPanel();
            tlpTrai = new TableLayoutPanel();
            txtTimMon = new TextBox();
            dgvMon = new DataGridView();
            lblMonTong = new Label();
            tlpPhai = new TableLayoutPanel();
            lblTenMon = new Label();
            flowThem = new FlowLayoutPanel();
            cboNLCT = new ComboBox();
            numDL = new NumericUpDown();
            lblDV = new Label();
            btnThemCT = new Button();
            dgvCT = new DataGridView();
            lblCTTong = new Label();
            flowNutCT = new FlowLayoutPanel();
            btnLuuCT = new Button();
            btnHoanTac = new Button();
            btnXoaCT = new Button();
            ((System.ComponentModel.ISupportInitialize)Err).BeginInit();
            tabMain.SuspendLayout();
            tpDanhMuc.SuspendLayout();
            tlpDM.SuspendLayout();
            flowLoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNL).BeginInit();
            grpNL.SuspendLayout();
            tlpForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numTonMin).BeginInit();
            flowNut.SuspendLayout();
            tpCongThuc.SuspendLayout();
            tlpCT.SuspendLayout();
            tlpTrai.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMon).BeginInit();
            tlpPhai.SuspendLayout();
            flowThem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDL).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvCT).BeginInit();
            flowNutCT.SuspendLayout();
            SuspendLayout();
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tpDanhMuc);
            tabMain.Controls.Add(tpCongThuc);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Location = new Point(0, 0);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1132, 653);
            tabMain.TabIndex = 0;
            // 
            // tpDanhMuc
            // 
            tpDanhMuc.Controls.Add(tlpDM);
            tpDanhMuc.Location = new Point(4, 32);
            tpDanhMuc.Name = "tpDanhMuc";
            tpDanhMuc.Padding = new Padding(3);
            tpDanhMuc.Size = new Size(1124, 617);
            tpDanhMuc.TabIndex = 0;
            tpDanhMuc.Text = "Danh mục nguyên liệu";
            tpDanhMuc.UseVisualStyleBackColor = true;
            // 
            // tlpDM
            // 
            tlpDM.ColumnCount = 2;
            tlpDM.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDM.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            tlpDM.Controls.Add(flowLoc, 0, 0);
            tlpDM.Controls.Add(dgvNL, 0, 1);
            tlpDM.Controls.Add(lblTongKet, 0, 2);
            tlpDM.Controls.Add(grpNL, 1, 0);
            tlpDM.Dock = DockStyle.Fill;
            tlpDM.Location = new Point(3, 3);
            tlpDM.Name = "tlpDM";
            tlpDM.Padding = new Padding(12);
            tlpDM.RowCount = 3;
            tlpDM.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tlpDM.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpDM.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tlpDM.Size = new Size(1118, 611);
            tlpDM.TabIndex = 0;
            // 
            // flowLoc
            // 
            flowLoc.Controls.Add(txtTim);
            flowLoc.Controls.Add(cboLoc);
            flowLoc.Dock = DockStyle.Fill;
            flowLoc.Location = new Point(15, 15);
            flowLoc.Name = "flowLoc";
            flowLoc.Size = new Size(688, 46);
            flowLoc.TabIndex = 0;
            flowLoc.WrapContents = false;
            // 
            // txtTim
            // 
            txtTim.Location = new Point(3, 6);
            txtTim.Margin = new Padding(3, 6, 18, 3);
            txtTim.Name = "txtTim";
            txtTim.PlaceholderText = "Tìm theo nguyên liệu ...";
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
            cboLoc.Size = new Size(160, 31);
            cboLoc.TabIndex = 1;
            // 
            // dgvNL
            // 
            dgvNL.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvNL.Dock = DockStyle.Fill;
            dgvNL.Location = new Point(15, 67);
            dgvNL.Name = "dgvNL";
            dgvNL.RowHeadersWidth = 51;
            dgvNL.Size = new Size(688, 497);
            dgvNL.TabIndex = 2;
            // 
            // lblTongKet
            // 
            lblTongKet.Dock = DockStyle.Fill;
            lblTongKet.Location = new Point(15, 567);
            lblTongKet.Name = "lblTongKet";
            lblTongKet.Size = new Size(688, 32);
            lblTongKet.TabIndex = 2;
            lblTongKet.Text = "label1";
            lblTongKet.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpNL
            // 
            grpNL.Controls.Add(tlpForm);
            grpNL.Dock = DockStyle.Fill;
            grpNL.Location = new Point(718, 12);
            grpNL.Margin = new Padding(12, 0, 0, 0);
            grpNL.Name = "grpNL";
            tlpDM.SetRowSpan(grpNL, 3);
            grpNL.Size = new Size(388, 587);
            grpNL.TabIndex = 3;
            grpNL.TabStop = false;
            grpNL.Text = "Thông tin nguyên liệu";
            // 
            // tlpForm
            // 
            tlpForm.ColumnCount = 2;
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
            tlpForm.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpForm.Controls.Add(lblTK, 0, 5);
            tlpForm.Controls.Add(lblTT, 0, 4);
            tlpForm.Controls.Add(lblTonMin, 0, 3);
            tlpForm.Controls.Add(lblDonVi, 0, 2);
            tlpForm.Controls.Add(lblTen, 0, 1);
            tlpForm.Controls.Add(lblMa, 0, 0);
            tlpForm.Controls.Add(txtMa, 1, 0);
            tlpForm.Controls.Add(txtTen, 1, 1);
            tlpForm.Controls.Add(cboDonVi, 1, 2);
            tlpForm.Controls.Add(numTonMin, 1, 3);
            tlpForm.Controls.Add(chkHoatDong, 1, 4);
            tlpForm.Controls.Add(lblThongKe, 1, 5);
            tlpForm.Controls.Add(btnDungTrong, 1, 6);
            tlpForm.Controls.Add(flowNut, 0, 8);
            tlpForm.Dock = DockStyle.Fill;
            tlpForm.Location = new Point(3, 26);
            tlpForm.Name = "tlpForm";
            tlpForm.Padding = new Padding(6);
            tlpForm.RowCount = 9;
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 84F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpForm.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpForm.Size = new Size(382, 558);
            tlpForm.TabIndex = 0;
            // 
            // lblTK
            // 
            lblTK.Dock = DockStyle.Fill;
            lblTK.Location = new Point(9, 206);
            lblTK.Name = "lblTK";
            lblTK.Size = new Size(104, 84);
            lblTK.TabIndex = 10;
            lblTK.Text = "Thống kê";
            lblTK.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTT
            // 
            lblTT.Dock = DockStyle.Fill;
            lblTT.Location = new Point(9, 166);
            lblTT.Name = "lblTT";
            lblTT.Size = new Size(104, 40);
            lblTT.TabIndex = 8;
            lblTT.Text = "Trạng thái";
            lblTT.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTonMin
            // 
            lblTonMin.Dock = DockStyle.Fill;
            lblTonMin.Location = new Point(9, 126);
            lblTonMin.Name = "lblTonMin";
            lblTonMin.Size = new Size(104, 40);
            lblTonMin.TabIndex = 6;
            lblTonMin.Text = "Tồn tối thiểu";
            lblTonMin.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDonVi
            // 
            lblDonVi.Dock = DockStyle.Fill;
            lblDonVi.Location = new Point(9, 86);
            lblDonVi.Name = "lblDonVi";
            lblDonVi.Size = new Size(104, 40);
            lblDonVi.TabIndex = 4;
            lblDonVi.Text = "Đơn vị gốc *";
            lblDonVi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTen
            // 
            lblTen.Dock = DockStyle.Fill;
            lblTen.Location = new Point(9, 46);
            lblTen.Name = "lblTen";
            lblTen.Size = new Size(104, 40);
            lblTen.TabIndex = 2;
            lblTen.Text = "Tên *";
            lblTen.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMa
            // 
            lblMa.Dock = DockStyle.Fill;
            lblMa.Location = new Point(9, 6);
            lblMa.Name = "lblMa";
            lblMa.Size = new Size(104, 40);
            lblMa.TabIndex = 0;
            lblMa.Text = "Mã";
            lblMa.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMa
            // 
            txtMa.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtMa.Location = new Point(119, 11);
            txtMa.Name = "txtMa";
            txtMa.ReadOnly = true;
            txtMa.Size = new Size(254, 30);
            txtMa.TabIndex = 11;
            // 
            // txtTen
            // 
            txtTen.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTen.Location = new Point(119, 51);
            txtTen.MaxLength = 200;
            txtTen.Name = "txtTen";
            txtTen.Size = new Size(254, 30);
            txtTen.TabIndex = 3;
            // 
            // cboDonVi
            // 
            cboDonVi.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cboDonVi.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDonVi.FormattingEnabled = true;
            cboDonVi.Location = new Point(119, 92);
            cboDonVi.Name = "cboDonVi";
            cboDonVi.Size = new Size(254, 31);
            cboDonVi.TabIndex = 4;
            // 
            // numTonMin
            // 
            numTonMin.Anchor = AnchorStyles.Left;
            numTonMin.DecimalPlaces = 3;
            numTonMin.Location = new Point(119, 131);
            numTonMin.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numTonMin.Name = "numTonMin";
            numTonMin.Size = new Size(160, 30);
            numTonMin.TabIndex = 5;
            numTonMin.ThousandsSeparator = true;
            // 
            // chkHoatDong
            // 
            chkHoatDong.Anchor = AnchorStyles.Left;
            chkHoatDong.AutoSize = true;
            chkHoatDong.Checked = true;
            chkHoatDong.CheckState = CheckState.Checked;
            chkHoatDong.Location = new Point(119, 172);
            chkHoatDong.Name = "chkHoatDong";
            chkHoatDong.Size = new Size(140, 27);
            chkHoatDong.TabIndex = 6;
            chkHoatDong.Text = "Đang sử dụng";
            chkHoatDong.UseVisualStyleBackColor = true;
            // 
            // lblThongKe
            // 
            lblThongKe.Dock = DockStyle.Fill;
            lblThongKe.Location = new Point(119, 206);
            lblThongKe.Name = "lblThongKe";
            lblThongKe.Size = new Size(254, 84);
            lblThongKe.TabIndex = 16;
            lblThongKe.Text = "label1";
            lblThongKe.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnDungTrong
            // 
            btnDungTrong.Anchor = AnchorStyles.Left;
            btnDungTrong.Location = new Point(119, 295);
            btnDungTrong.Name = "btnDungTrong";
            btnDungTrong.Size = new Size(200, 34);
            btnDungTrong.TabIndex = 7;
            btnDungTrong.Text = "Xem món đang dùng";
            btnDungTrong.UseVisualStyleBackColor = true;
            // 
            // flowNut
            // 
            tlpForm.SetColumnSpan(flowNut, 2);
            flowNut.Controls.Add(btnMoi);
            flowNut.Controls.Add(btnLuu);
            flowNut.Dock = DockStyle.Fill;
            flowNut.Location = new Point(9, 499);
            flowNut.Name = "flowNut";
            flowNut.Size = new Size(364, 50);
            flowNut.TabIndex = 18;
            // 
            // btnMoi
            // 
            btnMoi.Location = new Point(3, 3);
            btnMoi.Name = "btnMoi";
            btnMoi.Size = new Size(130, 36);
            btnMoi.TabIndex = 8;
            btnMoi.Text = "+ Thêm mới";
            btnMoi.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Location = new Point(139, 3);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(130, 36);
            btnLuu.TabIndex = 9;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // tpCongThuc
            // 
            tpCongThuc.Controls.Add(tlpCT);
            tpCongThuc.Location = new Point(4, 32);
            tpCongThuc.Name = "tpCongThuc";
            tpCongThuc.Padding = new Padding(3);
            tpCongThuc.Size = new Size(1124, 617);
            tpCongThuc.TabIndex = 1;
            tpCongThuc.Text = "Công thức món";
            tpCongThuc.UseVisualStyleBackColor = true;
            // 
            // tlpCT
            // 
            tlpCT.ColumnCount = 2;
            tlpCT.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 380F));
            tlpCT.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCT.Controls.Add(tlpTrai, 0, 0);
            tlpCT.Controls.Add(tlpPhai, 1, 0);
            tlpCT.Dock = DockStyle.Fill;
            tlpCT.Location = new Point(3, 3);
            tlpCT.Name = "tlpCT";
            tlpCT.Padding = new Padding(12);
            tlpCT.RowCount = 1;
            tlpCT.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpCT.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tlpCT.Size = new Size(1118, 611);
            tlpCT.TabIndex = 0;
            // 
            // tlpTrai
            // 
            tlpTrai.ColumnCount = 1;
            tlpTrai.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpTrai.Controls.Add(txtTimMon, 0, 0);
            tlpTrai.Controls.Add(dgvMon, 0, 1);
            tlpTrai.Controls.Add(lblMonTong, 0, 2);
            tlpTrai.Dock = DockStyle.Fill;
            tlpTrai.Location = new Point(12, 12);
            tlpTrai.Margin = new Padding(0, 0, 12, 0);
            tlpTrai.Name = "tlpTrai";
            tlpTrai.RowCount = 3;
            tlpTrai.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpTrai.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpTrai.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpTrai.Size = new Size(368, 587);
            tlpTrai.TabIndex = 0;
            // 
            // txtTimMon
            // 
            txtTimMon.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtTimMon.Location = new Point(3, 9);
            txtTimMon.Margin = new Padding(3, 8, 3, 3);
            txtTimMon.Name = "txtTimMon";
            txtTimMon.PlaceholderText = "Tìm món theo tên hoặc mã ...";
            txtTimMon.Size = new Size(362, 30);
            txtTimMon.TabIndex = 0;
            // 
            // dgvMon
            // 
            dgvMon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvMon.Dock = DockStyle.Fill;
            dgvMon.Location = new Point(3, 47);
            dgvMon.Name = "dgvMon";
            dgvMon.RowHeadersWidth = 51;
            dgvMon.Size = new Size(362, 507);
            dgvMon.TabIndex = 1;
            // 
            // lblMonTong
            // 
            lblMonTong.Dock = DockStyle.Fill;
            lblMonTong.Location = new Point(3, 557);
            lblMonTong.Name = "lblMonTong";
            lblMonTong.Size = new Size(362, 30);
            lblMonTong.TabIndex = 2;
            lblMonTong.Text = "label1";
            lblMonTong.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tlpPhai
            // 
            tlpPhai.ColumnCount = 1;
            tlpPhai.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpPhai.Controls.Add(lblTenMon, 0, 0);
            tlpPhai.Controls.Add(flowThem, 0, 1);
            tlpPhai.Controls.Add(dgvCT, 0, 2);
            tlpPhai.Controls.Add(lblCTTong, 0, 3);
            tlpPhai.Controls.Add(flowNutCT, 0, 4);
            tlpPhai.Dock = DockStyle.Fill;
            tlpPhai.Location = new Point(395, 15);
            tlpPhai.Name = "tlpPhai";
            tlpPhai.RowCount = 5;
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tlpPhai.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            tlpPhai.Size = new Size(708, 581);
            tlpPhai.TabIndex = 1;
            // 
            // lblTenMon
            // 
            lblTenMon.Dock = DockStyle.Fill;
            lblTenMon.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold);
            lblTenMon.Location = new Point(3, 0);
            lblTenMon.Name = "lblTenMon";
            lblTenMon.Size = new Size(702, 40);
            lblTenMon.TabIndex = 0;
            lblTenMon.Text = "Chưa chọn món";
            lblTenMon.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowThem
            // 
            flowThem.Controls.Add(cboNLCT);
            flowThem.Controls.Add(numDL);
            flowThem.Controls.Add(lblDV);
            flowThem.Controls.Add(btnThemCT);
            flowThem.Dock = DockStyle.Fill;
            flowThem.Location = new Point(3, 43);
            flowThem.Name = "flowThem";
            flowThem.Size = new Size(702, 50);
            flowThem.TabIndex = 1;
            flowThem.WrapContents = false;
            // 
            // cboNLCT
            // 
            cboNLCT.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboNLCT.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboNLCT.FormattingEnabled = true;
            cboNLCT.Location = new Point(3, 6);
            cboNLCT.Margin = new Padding(3, 6, 12, 3);
            cboNLCT.Name = "cboNLCT";
            cboNLCT.Size = new Size(300, 31);
            cboNLCT.TabIndex = 2;
            // 
            // numDL
            // 
            numDL.DecimalPlaces = 3;
            numDL.Location = new Point(318, 6);
            numDL.Margin = new Padding(3, 6, 3, 3);
            numDL.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numDL.Name = "numDL";
            numDL.Size = new Size(110, 30);
            numDL.TabIndex = 3;
            // 
            // lblDV
            // 
            lblDV.AutoSize = true;
            lblDV.Location = new Point(434, 9);
            lblDV.Margin = new Padding(3, 9, 18, 3);
            lblDV.Name = "lblDV";
            lblDV.Size = new Size(17, 23);
            lblDV.TabIndex = 2;
            lblDV.Text = "-";
            // 
            // btnThemCT
            // 
            btnThemCT.Location = new Point(472, 3);
            btnThemCT.Name = "btnThemCT";
            btnThemCT.Size = new Size(130, 34);
            btnThemCT.TabIndex = 4;
            btnThemCT.Text = "+ Thêm dòng";
            btnThemCT.UseVisualStyleBackColor = true;
            // 
            // dgvCT
            // 
            dgvCT.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCT.Dock = DockStyle.Fill;
            dgvCT.Location = new Point(3, 99);
            dgvCT.Name = "dgvCT";
            dgvCT.RowHeadersWidth = 51;
            dgvCT.Size = new Size(702, 393);
            dgvCT.TabIndex = 5;
            // 
            // lblCTTong
            // 
            lblCTTong.Dock = DockStyle.Fill;
            lblCTTong.Location = new Point(3, 495);
            lblCTTong.Name = "lblCTTong";
            lblCTTong.Size = new Size(702, 30);
            lblCTTong.TabIndex = 3;
            lblCTTong.Text = "label1";
            lblCTTong.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowNutCT
            // 
            flowNutCT.Controls.Add(btnLuuCT);
            flowNutCT.Controls.Add(btnHoanTac);
            flowNutCT.Controls.Add(btnXoaCT);
            flowNutCT.Dock = DockStyle.Fill;
            flowNutCT.FlowDirection = FlowDirection.RightToLeft;
            flowNutCT.Location = new Point(3, 528);
            flowNutCT.Name = "flowNutCT";
            flowNutCT.Size = new Size(702, 50);
            flowNutCT.TabIndex = 4;
            // 
            // btnLuuCT
            // 
            btnLuuCT.Location = new Point(549, 3);
            btnLuuCT.Name = "btnLuuCT";
            btnLuuCT.Size = new Size(150, 36);
            btnLuuCT.TabIndex = 8;
            btnLuuCT.Text = "Lưu công thức";
            btnLuuCT.UseVisualStyleBackColor = true;
            // 
            // btnHoanTac
            // 
            btnHoanTac.Location = new Point(413, 3);
            btnHoanTac.Name = "btnHoanTac";
            btnHoanTac.Size = new Size(130, 36);
            btnHoanTac.TabIndex = 7;
            btnHoanTac.Text = "Hủy thay đổi";
            btnHoanTac.UseVisualStyleBackColor = true;
            // 
            // btnXoaCT
            // 
            btnXoaCT.Location = new Point(297, 3);
            btnXoaCT.Name = "btnXoaCT";
            btnXoaCT.Size = new Size(110, 36);
            btnXoaCT.TabIndex = 6;
            btnXoaCT.Text = "Xóa dòng";
            btnXoaCT.UseVisualStyleBackColor = true;
            // 
            // FrmNguyenLieu
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1132, 653);
            Controls.Add(tabMain);
            MinimumSize = new Size(1000, 580);
            Name = "FrmNguyenLieu";
            Text = " Nguyên liệu - công thức";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)Err).EndInit();
            tabMain.ResumeLayout(false);
            tpDanhMuc.ResumeLayout(false);
            tlpDM.ResumeLayout(false);
            flowLoc.ResumeLayout(false);
            flowLoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNL).EndInit();
            grpNL.ResumeLayout(false);
            tlpForm.ResumeLayout(false);
            tlpForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numTonMin).EndInit();
            flowNut.ResumeLayout(false);
            tpCongThuc.ResumeLayout(false);
            tlpCT.ResumeLayout(false);
            tlpTrai.ResumeLayout(false);
            tlpTrai.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvMon).EndInit();
            tlpPhai.ResumeLayout(false);
            flowThem.ResumeLayout(false);
            flowThem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDL).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvCT).EndInit();
            flowNutCT.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabMain;
        private TabPage tpDanhMuc;
        private TabPage tpCongThuc;
        private TabPage tabPage1;
        private TableLayoutPanel tlpDM;
        private FlowLayoutPanel flowLoc;
        private TextBox txtTim;
        private ComboBox cboLoc;
        private DataGridView dgvNL;
        private Label lblTongKet;
        private GroupBox grpNL;
        private TableLayoutPanel tlpForm;
        private Label lblTK;
        private Label lblTT;
        private Label lblTonMin;
        private Label lblDonVi;
        private Label lblTen;
        private Label lblMa;
        private TextBox txtMa;
        private TextBox txtTen;
        private ComboBox cboDonVi;
        private NumericUpDown numTonMin;
        private CheckBox chkHoatDong;
        private Label lblThongKe;
        private Button btnDungTrong;
        private FlowLayoutPanel flowNut;
        private Button btnMoi;
        private Button btnLuu;
        private TableLayoutPanel tlpCT;
        private TableLayoutPanel tlpTrai;
        private TextBox txtTimMon;
        private DataGridView dgvMon;
        private Label lblMonTong;
        private TableLayoutPanel tlpPhai;
        private Label lblTenMon;
        private FlowLayoutPanel flowThem;
        private ComboBox cboNLCT;
        private NumericUpDown numDL;
        private DataGridView dgvCT;
        private Label lblCTTong;
        private FlowLayoutPanel flowNutCT;
        private Label lblDV;
        private Button btnThemCT;
        private Button btnLuuCT;
        private Button btnHoanTac;
        private Button btnXoaCT;
    }
}