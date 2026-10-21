namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormChonKhuyenMai
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            pnlBody = new Panel();
            dgvPromos = new DataGridView();
            colMaCode = new DataGridViewTextBoxColumn();
            colTenKhuyenMai = new DataGridViewTextBoxColumn();
            colMucGiam = new DataGridViewTextBoxColumn();
            colDonToiThieu = new DataGridViewTextBoxColumn();
            colHanDung = new DataGridViewTextBoxColumn();
            colTienGiam = new DataGridViewTextBoxColumn();
            colDieuKien = new DataGridViewTextBoxColumn();
            lblStatus = new Label();
            tlpSearch = new TableLayoutPanel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            pnlButtons = new FlowLayoutPanel();
            btnClose = new Button();
            btnRemove = new Button();
            btnApply = new Button();
            pnlBody.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPromos).BeginInit();
            tlpSearch.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBody
            // 
            pnlBody.Controls.Add(dgvPromos);
            pnlBody.Controls.Add(lblStatus);
            pnlBody.Controls.Add(tlpSearch);
            pnlBody.Controls.Add(pnlButtons);
            pnlBody.Dock = DockStyle.Fill;
            pnlBody.Location = new Point(0, 0);
            pnlBody.Name = "pnlBody";
            pnlBody.Padding = new Padding(14, 10, 14, 10);
            pnlBody.Size = new Size(760, 440);
            pnlBody.TabIndex = 0;
            // 
            // dgvPromos
            // 
            dgvPromos.AllowUserToAddRows = false;
            dgvPromos.AllowUserToDeleteRows = false;
            dgvPromos.AllowUserToResizeRows = false;
            dgvPromos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPromos.BackgroundColor = Color.White;
            dgvPromos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(255, 244, 222);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvPromos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvPromos.ColumnHeadersHeight = 34;
            dgvPromos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvPromos.Columns.AddRange(new DataGridViewColumn[] { colMaCode, colTenKhuyenMai, colMucGiam, colDonToiThieu, colHanDung, colTienGiam, colDieuKien });
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(255, 226, 168);
            dataGridViewCellStyle5.SelectionForeColor = Color.Black;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvPromos.DefaultCellStyle = dataGridViewCellStyle5;
            dgvPromos.Dock = DockStyle.Fill;
            dgvPromos.EnableHeadersVisualStyles = false;
            dgvPromos.Location = new Point(14, 80);
            dgvPromos.MultiSelect = false;
            dgvPromos.Name = "dgvPromos";
            dgvPromos.ReadOnly = true;
            dgvPromos.RowHeadersVisible = false;
            dgvPromos.RowHeadersWidth = 51;
            dgvPromos.RowTemplate.Height = 32;
            dgvPromos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPromos.Size = new Size(732, 300);
            dgvPromos.TabIndex = 1;
            dgvPromos.CellDoubleClick += dgvPromos_CellDoubleClick;
            dgvPromos.CellFormatting += dgvPromos_CellFormatting;
            dgvPromos.SelectionChanged += dgvPromos_SelectionChanged;
            // 
            // colMaCode
            // 
            colMaCode.DataPropertyName = "MaCode";
            colMaCode.FillWeight = 80F;
            colMaCode.HeaderText = "Mã";
            colMaCode.MinimumWidth = 6;
            colMaCode.Name = "colMaCode";
            colMaCode.ReadOnly = true;
            // 
            // colTenKhuyenMai
            // 
            colTenKhuyenMai.DataPropertyName = "TenKhuyenMai";
            colTenKhuyenMai.FillWeight = 190F;
            colTenKhuyenMai.HeaderText = "Tên chương trình";
            colTenKhuyenMai.MinimumWidth = 6;
            colTenKhuyenMai.Name = "colTenKhuyenMai";
            colTenKhuyenMai.ReadOnly = true;
            // 
            // colMucGiam
            // 
            colMucGiam.DataPropertyName = "MucGiam";
            colMucGiam.FillWeight = 90F;
            colMucGiam.HeaderText = "Mức giảm";
            colMucGiam.MinimumWidth = 6;
            colMucGiam.Name = "colMucGiam";
            colMucGiam.ReadOnly = true;
            // 
            // colDonToiThieu
            // 
            colDonToiThieu.DataPropertyName = "DonToiThieu";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N0";
            colDonToiThieu.DefaultCellStyle = dataGridViewCellStyle2;
            colDonToiThieu.FillWeight = 80F;
            colDonToiThieu.HeaderText = "Đơn từ";
            colDonToiThieu.MinimumWidth = 6;
            colDonToiThieu.Name = "colDonToiThieu";
            colDonToiThieu.ReadOnly = true;
            // 
            // colHanDung
            // 
            colHanDung.DataPropertyName = "HanDung";
            dataGridViewCellStyle3.Format = "dd/MM/yyyy";
            colHanDung.DefaultCellStyle = dataGridViewCellStyle3;
            colHanDung.FillWeight = 90F;
            colHanDung.HeaderText = "Hạn dùng";
            colHanDung.MinimumWidth = 6;
            colHanDung.Name = "colHanDung";
            colHanDung.ReadOnly = true;
            // 
            // colTienGiam
            // 
            colTienGiam.DataPropertyName = "TienGiam";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            colTienGiam.DefaultCellStyle = dataGridViewCellStyle4;
            colTienGiam.HeaderText = "Được giảm";
            colTienGiam.MinimumWidth = 6;
            colTienGiam.Name = "colTienGiam";
            colTienGiam.ReadOnly = true;
            // 
            // colDieuKien
            // 
            colDieuKien.DataPropertyName = "DieuKien";
            colDieuKien.FillWeight = 110F;
            colDieuKien.HeaderText = "Điều kiện";
            colDieuKien.MinimumWidth = 6;
            colDieuKien.Name = "colDieuKien";
            colDieuKien.ReadOnly = true;
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Top;
            lblStatus.Font = new Font("Segoe UI", 9.5F, FontStyle.Italic);
            lblStatus.ForeColor = Color.FromArgb(127, 140, 141);
            lblStatus.Location = new Point(14, 50);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(732, 30);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Tiền hàng tính khuyến mãi";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblStatus.UseMnemonic = false;
            // 
            // tlpSearch
            // 
            tlpSearch.ColumnCount = 2;
            tlpSearch.ColumnStyles.Add(new ColumnStyle());
            tlpSearch.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpSearch.Controls.Add(lblSearch, 0, 0);
            tlpSearch.Controls.Add(txtSearch, 1, 0);
            tlpSearch.Dock = DockStyle.Top;
            tlpSearch.Location = new Point(14, 10);
            tlpSearch.Name = "tlpSearch";
            tlpSearch.RowCount = 1;
            tlpSearch.RowStyles.Add(new RowStyle());
            tlpSearch.Size = new Size(732, 40);
            tlpSearch.TabIndex = 0;
            // 
            // lblSearch
            // 
            lblSearch.Anchor = AnchorStyles.Left;
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(3, 8);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(107, 23);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "🔍 Tìm kiếm";
            // 
            // txtSearch
            // 
            txtSearch.Dock = DockStyle.Fill;
            txtSearch.Location = new Point(119, 6);
            txtSearch.Margin = new Padding(6, 6, 0, 0);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tên hoặc mã chương trình";
            txtSearch.Size = new Size(613, 30);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // pnlButtons
            // 
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Controls.Add(btnRemove);
            pnlButtons.Controls.Add(btnApply);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.FlowDirection = FlowDirection.RightToLeft;
            pnlButtons.Location = new Point(14, 380);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(0, 8, 0, 0);
            pnlButtons.Size = new Size(732, 50);
            pnlButtons.TabIndex = 2;
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.FromArgb(236, 240, 241);
            btnClose.Cursor = Cursors.Hand;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnClose.ForeColor = Color.Black;
            btnClose.Location = new Point(642, 8);
            btnClose.Margin = new Padding(6, 0, 0, 0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 36);
            btnClose.TabIndex = 2;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = false;
            // 
            // btnRemove
            // 
            btnRemove.BackColor = Color.FromArgb(231, 76, 60);
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnRemove.ForeColor = Color.White;
            btnRemove.Location = new Point(486, 8);
            btnRemove.Margin = new Padding(6, 0, 0, 0);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(150, 36);
            btnRemove.TabIndex = 1;
            btnRemove.Text = "✖ Bỏ khuyến mãi";
            btnRemove.UseVisualStyleBackColor = false;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnApply
            // 
            btnApply.BackColor = Color.FromArgb(242, 153, 0);
            btnApply.Cursor = Cursors.Hand;
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.FlatStyle = FlatStyle.Flat;
            btnApply.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnApply.ForeColor = Color.White;
            btnApply.Location = new Point(350, 8);
            btnApply.Margin = new Padding(6, 0, 0, 0);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(130, 36);
            btnApply.TabIndex = 0;
            btnApply.Text = "✔ Áp dụng";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // FormChonKhuyenMai
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = btnClose;
            ClientSize = new Size(760, 440);
            Controls.Add(pnlBody);
            Font = new Font("Segoe UI", 10F);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormChonKhuyenMai";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chọn chương trình khuyến mãi";
            Shown += FormChonKhuyenMai_Shown;
            pnlBody.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvPromos).EndInit();
            tlpSearch.ResumeLayout(false);
            tlpSearch.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.DataGridView dgvPromos;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTenKhuyenMai;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMucGiam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonToiThieu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHanDung;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTienGiam;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDieuKien;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.TableLayoutPanel tlpSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.FlowLayoutPanel pnlButtons;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnApply;
    }
}
