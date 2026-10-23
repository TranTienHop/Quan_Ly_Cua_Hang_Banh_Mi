namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormDonHomNay
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
            System.Windows.Forms.DataGridViewCellStyle cellStyleOrdersAlt = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleOrdersHeader = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleNgayTao = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleTienGiam = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStylePhiShip = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleTongThanhToan = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleOrdersDefault = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleDetailsAlt = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleDetailsHeader = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleSoLuong = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleDonGia = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleThanhTien = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle cellStyleDetailsDefault = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.splOrders = new System.Windows.Forms.SplitContainer();
            this.dgvOrders = new System.Windows.Forms.DataGridView();
            this.colNgayTao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaDonLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNguonDon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLoaiDon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTenKhach = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHinhThuc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTienGiam = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhiShip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTongThanhToan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThuNgan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTrangThaiDon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvDetails = new System.Windows.Forms.DataGridView();
            this.colTenSanPhamChup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSoLuong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonGia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThanhTien = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGhiChu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblDetail = new System.Windows.Forms.Label();
            this.flpFilter = new System.Windows.Forms.FlowLayoutPanel();
            this.lblDate = new System.Windows.Forms.Label();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.btnReload = new System.Windows.Forms.Button();
            this.lblSummary = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splOrders)).BeginInit();
            this.splOrders.Panel1.SuspendLayout();
            this.splOrders.Panel2.SuspendLayout();
            this.splOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).BeginInit();
            this.flpFilter.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlBody
            // 
            this.pnlBody.Controls.Add(this.splOrders);
            this.pnlBody.Controls.Add(this.flpFilter);
            this.pnlBody.Controls.Add(this.pnlButtons);
            this.pnlBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBody.Location = new System.Drawing.Point(0, 0);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Padding = new System.Windows.Forms.Padding(12, 6, 12, 8);
            this.pnlBody.Size = new System.Drawing.Size(984, 581);
            this.pnlBody.TabIndex = 0;
            // 
            // splOrders
            // 
            this.splOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splOrders.Location = new System.Drawing.Point(12, 50);
            this.splOrders.Name = "splOrders";
            this.splOrders.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splOrders.Panel1
            // 
            this.splOrders.Panel1.Controls.Add(this.dgvOrders);
            // 
            // splOrders.Panel2
            // 
            this.splOrders.Panel2.Controls.Add(this.dgvDetails);
            this.splOrders.Panel2.Controls.Add(this.lblDetail);
            this.splOrders.Size = new System.Drawing.Size(960, 473);
            this.splOrders.SplitterDistance = 290;
            this.splOrders.SplitterWidth = 6;
            this.splOrders.TabIndex = 1;
            // 
            // dgvOrders
            // 
            this.dgvOrders.AllowUserToAddRows = false;
            this.dgvOrders.AllowUserToDeleteRows = false;
            this.dgvOrders.AllowUserToResizeRows = false;
            cellStyleOrdersAlt.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.dgvOrders.AlternatingRowsDefaultCellStyle = cellStyleOrdersAlt;
            this.dgvOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrders.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrders.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            cellStyleOrdersHeader.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            cellStyleOrdersHeader.BackColor = System.Drawing.Color.FromArgb(255, 244, 222);
            cellStyleOrdersHeader.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            cellStyleOrdersHeader.ForeColor = System.Drawing.SystemColors.WindowText;
            cellStyleOrdersHeader.SelectionBackColor = System.Drawing.Color.FromArgb(255, 244, 222);
            cellStyleOrdersHeader.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            cellStyleOrdersHeader.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOrders.ColumnHeadersDefaultCellStyle = cellStyleOrdersHeader;
            this.dgvOrders.ColumnHeadersHeight = 32;
            this.dgvOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNgayTao,
            this.colMaDonLocal,
            this.colNguonDon,
            this.colLoaiDon,
            this.colTenKhach,
            this.colHinhThuc,
            this.colTienGiam,
            this.colPhiShip,
            this.colTongThanhToan,
            this.colThuNgan,
            this.colTrangThaiDon});
            cellStyleOrdersDefault.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            cellStyleOrdersDefault.BackColor = System.Drawing.SystemColors.Window;
            cellStyleOrdersDefault.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            cellStyleOrdersDefault.ForeColor = System.Drawing.SystemColors.ControlText;
            cellStyleOrdersDefault.SelectionBackColor = System.Drawing.Color.FromArgb(255, 226, 168);
            cellStyleOrdersDefault.SelectionForeColor = System.Drawing.Color.Black;
            cellStyleOrdersDefault.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOrders.DefaultCellStyle = cellStyleOrdersDefault;
            this.dgvOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrders.EnableHeadersVisualStyles = false;
            this.dgvOrders.Location = new System.Drawing.Point(0, 0);
            this.dgvOrders.MultiSelect = false;
            this.dgvOrders.Name = "dgvOrders";
            this.dgvOrders.ReadOnly = true;
            this.dgvOrders.RowHeadersVisible = false;
            this.dgvOrders.RowTemplate.Height = 28;
            this.dgvOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrders.Size = new System.Drawing.Size(960, 290);
            this.dgvOrders.TabIndex = 0;
            this.dgvOrders.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrders_CellDoubleClick);
            this.dgvOrders.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvOrders_CellFormatting);
            this.dgvOrders.SelectionChanged += new System.EventHandler(this.dgvOrders_SelectionChanged);
            // 
            // colNgayTao
            // 
            cellStyleNgayTao.Format = "HH:mm:ss";
            this.colNgayTao.DataPropertyName = "NgayTao";
            this.colNgayTao.DefaultCellStyle = cellStyleNgayTao;
            this.colNgayTao.FillWeight = 55F;
            this.colNgayTao.HeaderText = "Giờ";
            this.colNgayTao.Name = "colNgayTao";
            this.colNgayTao.ReadOnly = true;
            // 
            // colMaDonLocal
            // 
            this.colMaDonLocal.DataPropertyName = "MaDonLocal";
            this.colMaDonLocal.FillWeight = 110F;
            this.colMaDonLocal.HeaderText = "Mã đơn";
            this.colMaDonLocal.Name = "colMaDonLocal";
            this.colMaDonLocal.ReadOnly = true;
            // 
            // colNguonDon
            // 
            this.colNguonDon.DataPropertyName = "NguonDon";
            this.colNguonDon.FillWeight = 70F;
            this.colNguonDon.HeaderText = "Nguồn";
            this.colNguonDon.Name = "colNguonDon";
            this.colNguonDon.ReadOnly = true;
            // 
            // colLoaiDon
            // 
            this.colLoaiDon.DataPropertyName = "LoaiDon";
            this.colLoaiDon.FillWeight = 70F;
            this.colLoaiDon.HeaderText = "Loại đơn";
            this.colLoaiDon.Name = "colLoaiDon";
            this.colLoaiDon.ReadOnly = true;
            // 
            // colTenKhach
            // 
            this.colTenKhach.DataPropertyName = "TenKhach";
            this.colTenKhach.FillWeight = 110F;
            this.colTenKhach.HeaderText = "Khách hàng";
            this.colTenKhach.Name = "colTenKhach";
            this.colTenKhach.ReadOnly = true;
            // 
            // colHinhThuc
            // 
            this.colHinhThuc.DataPropertyName = "HinhThuc";
            this.colHinhThuc.FillWeight = 90F;
            this.colHinhThuc.HeaderText = "Thanh toán";
            this.colHinhThuc.Name = "colHinhThuc";
            this.colHinhThuc.ReadOnly = true;
            // 
            // colTienGiam
            // 
            cellStyleTienGiam.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            cellStyleTienGiam.Format = "N0";
            this.colTienGiam.DataPropertyName = "TienGiam";
            this.colTienGiam.DefaultCellStyle = cellStyleTienGiam;
            this.colTienGiam.FillWeight = 65F;
            this.colTienGiam.HeaderText = "Giảm";
            this.colTienGiam.Name = "colTienGiam";
            this.colTienGiam.ReadOnly = true;
            // 
            // colPhiShip
            // 
            cellStylePhiShip.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            cellStylePhiShip.Format = "N0";
            this.colPhiShip.DataPropertyName = "PhiShip";
            this.colPhiShip.DefaultCellStyle = cellStylePhiShip;
            this.colPhiShip.FillWeight = 60F;
            this.colPhiShip.HeaderText = "Ship";
            this.colPhiShip.Name = "colPhiShip";
            this.colPhiShip.ReadOnly = true;
            // 
            // colTongThanhToan
            // 
            cellStyleTongThanhToan.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            cellStyleTongThanhToan.Format = "N0";
            this.colTongThanhToan.DataPropertyName = "TongThanhToan";
            this.colTongThanhToan.DefaultCellStyle = cellStyleTongThanhToan;
            this.colTongThanhToan.FillWeight = 80F;
            this.colTongThanhToan.HeaderText = "Tổng tiền";
            this.colTongThanhToan.Name = "colTongThanhToan";
            this.colTongThanhToan.ReadOnly = true;
            // 
            // colThuNgan
            // 
            this.colThuNgan.DataPropertyName = "ThuNgan";
            this.colThuNgan.FillWeight = 100F;
            this.colThuNgan.HeaderText = "Thu ngân";
            this.colThuNgan.Name = "colThuNgan";
            this.colThuNgan.ReadOnly = true;
            // 
            // colTrangThaiDon
            // 
            this.colTrangThaiDon.DataPropertyName = "TrangThaiDon";
            this.colTrangThaiDon.FillWeight = 75F;
            this.colTrangThaiDon.HeaderText = "Trạng thái";
            this.colTrangThaiDon.Name = "colTrangThaiDon";
            this.colTrangThaiDon.ReadOnly = true;
            // 
            // dgvDetails
            // 
            this.dgvDetails.AllowUserToAddRows = false;
            this.dgvDetails.AllowUserToDeleteRows = false;
            this.dgvDetails.AllowUserToResizeRows = false;
            cellStyleDetailsAlt.BackColor = System.Drawing.Color.FromArgb(250, 250, 250);
            this.dgvDetails.AlternatingRowsDefaultCellStyle = cellStyleDetailsAlt;
            this.dgvDetails.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDetails.BackgroundColor = System.Drawing.Color.White;
            this.dgvDetails.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            cellStyleDetailsHeader.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            cellStyleDetailsHeader.BackColor = System.Drawing.Color.FromArgb(255, 244, 222);
            cellStyleDetailsHeader.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            cellStyleDetailsHeader.ForeColor = System.Drawing.SystemColors.WindowText;
            cellStyleDetailsHeader.SelectionBackColor = System.Drawing.Color.FromArgb(255, 244, 222);
            cellStyleDetailsHeader.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            cellStyleDetailsHeader.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDetails.ColumnHeadersDefaultCellStyle = cellStyleDetailsHeader;
            this.dgvDetails.ColumnHeadersHeight = 32;
            this.dgvDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTenSanPhamChup,
            this.colSoLuong,
            this.colDonGia,
            this.colThanhTien,
            this.colGhiChu});
            cellStyleDetailsDefault.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            cellStyleDetailsDefault.BackColor = System.Drawing.SystemColors.Window;
            cellStyleDetailsDefault.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            cellStyleDetailsDefault.ForeColor = System.Drawing.SystemColors.ControlText;
            cellStyleDetailsDefault.SelectionBackColor = System.Drawing.Color.FromArgb(255, 226, 168);
            cellStyleDetailsDefault.SelectionForeColor = System.Drawing.Color.Black;
            cellStyleDetailsDefault.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDetails.DefaultCellStyle = cellStyleDetailsDefault;
            this.dgvDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetails.EnableHeadersVisualStyles = false;
            this.dgvDetails.Location = new System.Drawing.Point(0, 24);
            this.dgvDetails.MultiSelect = false;
            this.dgvDetails.Name = "dgvDetails";
            this.dgvDetails.ReadOnly = true;
            this.dgvDetails.RowHeadersVisible = false;
            this.dgvDetails.RowTemplate.Height = 28;
            this.dgvDetails.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetails.Size = new System.Drawing.Size(960, 153);
            this.dgvDetails.TabIndex = 1;
            // 
            // colTenSanPhamChup
            // 
            this.colTenSanPhamChup.DataPropertyName = "TenSanPhamChup";
            this.colTenSanPhamChup.FillWeight = 220F;
            this.colTenSanPhamChup.HeaderText = "Tên món";
            this.colTenSanPhamChup.Name = "colTenSanPhamChup";
            this.colTenSanPhamChup.ReadOnly = true;
            // 
            // colSoLuong
            // 
            cellStyleSoLuong.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            cellStyleSoLuong.Format = "0.##";
            this.colSoLuong.DataPropertyName = "SoLuong";
            this.colSoLuong.DefaultCellStyle = cellStyleSoLuong;
            this.colSoLuong.FillWeight = 40F;
            this.colSoLuong.HeaderText = "SL";
            this.colSoLuong.Name = "colSoLuong";
            this.colSoLuong.ReadOnly = true;
            // 
            // colDonGia
            // 
            cellStyleDonGia.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            cellStyleDonGia.Format = "N0";
            this.colDonGia.DataPropertyName = "DonGia";
            this.colDonGia.DefaultCellStyle = cellStyleDonGia;
            this.colDonGia.FillWeight = 70F;
            this.colDonGia.HeaderText = "Đơn giá";
            this.colDonGia.Name = "colDonGia";
            this.colDonGia.ReadOnly = true;
            // 
            // colThanhTien
            // 
            cellStyleThanhTien.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            cellStyleThanhTien.Format = "N0";
            this.colThanhTien.DataPropertyName = "ThanhTien";
            this.colThanhTien.DefaultCellStyle = cellStyleThanhTien;
            this.colThanhTien.FillWeight = 80F;
            this.colThanhTien.HeaderText = "Thành tiền";
            this.colThanhTien.Name = "colThanhTien";
            this.colThanhTien.ReadOnly = true;
            // 
            // colGhiChu
            // 
            this.colGhiChu.DataPropertyName = "GhiChu";
            this.colGhiChu.FillWeight = 160F;
            this.colGhiChu.HeaderText = "Ghi chú";
            this.colGhiChu.Name = "colGhiChu";
            this.colGhiChu.ReadOnly = true;
            // 
            // lblDetail
            // 
            this.lblDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDetail.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblDetail.ForeColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.lblDetail.Location = new System.Drawing.Point(0, 0);
            this.lblDetail.Name = "lblDetail";
            this.lblDetail.Size = new System.Drawing.Size(960, 24);
            this.lblDetail.Text = "Chi tiết đơn đang chọn";
            // 
            // flpFilter
            // 
            this.flpFilter.Controls.Add(this.lblDate);
            this.flpFilter.Controls.Add(this.dtpDate);
            this.flpFilter.Controls.Add(this.btnReload);
            this.flpFilter.Controls.Add(this.lblSummary);
            this.flpFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.flpFilter.Location = new System.Drawing.Point(12, 6);
            this.flpFilter.Name = "flpFilter";
            this.flpFilter.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.flpFilter.Size = new System.Drawing.Size(960, 44);
            this.flpFilter.TabIndex = 0;
            this.flpFilter.WrapContents = false;
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Margin = new System.Windows.Forms.Padding(0, 7, 4, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Text = "📅 Ngày:";
            // 
            // dtpDate
            // 
            this.dtpDate.CustomFormat = "dd/MM/yyyy";
            this.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(120, 24);
            this.dtpDate.TabIndex = 0;
            this.dtpDate.ValueChanged += new System.EventHandler(this.dtpDate_ValueChanged);
            // 
            // btnReload
            // 
            this.btnReload.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            this.btnReload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReload.FlatAppearance.BorderSize = 0;
            this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReload.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnReload.ForeColor = System.Drawing.Color.White;
            this.btnReload.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(100, 34);
            this.btnReload.TabIndex = 1;
            this.btnReload.Text = "⟳ Tải lại";
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // lblSummary
            // 
            this.lblSummary.AutoSize = true;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSummary.ForeColor = System.Drawing.Color.FromArgb(204, 122, 0);
            this.lblSummary.Margin = new System.Windows.Forms.Padding(16, 7, 0, 0);
            this.lblSummary.Name = "lblSummary";
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnClose);
            this.pnlButtons.Controls.Add(this.btnPrint);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.pnlButtons.Location = new System.Drawing.Point(12, 523);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlButtons.Size = new System.Drawing.Size(960, 50);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(149, 165, 166);
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 34);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Đóng";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(242, 153, 0);
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(160, 34);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "🖨 In lại hóa đơn";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // FormDonHomNay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(984, 581);
            this.Controls.Add(this.pnlBody);
            this.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(760, 480);
            this.Name = "FormDonHomNay";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đơn hàng trong ngày";
            this.Load += new System.EventHandler(this.FormDonHomNay_Load);
            this.pnlBody.ResumeLayout(false);
            this.splOrders.Panel1.ResumeLayout(false);
            this.splOrders.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splOrders)).EndInit();
            this.splOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetails)).EndInit();
            this.flpFilter.ResumeLayout(false);
            this.flpFilter.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.SplitContainer splOrders;
        private System.Windows.Forms.DataGridView dgvOrders;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNgayTao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaDonLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNguonDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLoaiDon;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKhach;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHinhThuc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTienGiam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhiShip;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTongThanhToan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThuNgan;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTrangThaiDon;
        private System.Windows.Forms.DataGridView dgvDetails;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenSanPhamChup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSoLuong;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonGia;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThanhTien;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGhiChu;
        private System.Windows.Forms.Label lblDetail;
        private System.Windows.Forms.FlowLayoutPanel flpFilter;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Label lblSummary;
        private System.Windows.Forms.FlowLayoutPanel pnlButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrint;
    }
}
