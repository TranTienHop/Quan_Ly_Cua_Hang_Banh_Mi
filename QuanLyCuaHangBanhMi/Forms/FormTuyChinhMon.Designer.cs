namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormTuyChinhMon
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnClose = new Button();
            lblTitle = new Label();
            pnlScroll = new Panel();
            flpSections = new FlowLayoutPanel();
            pnlFooter = new Panel();
            tlpFooter = new TableLayoutPanel();
            lblPromo = new Label();
            flpQty = new FlowLayoutPanel();
            lblQty = new Label();
            btnQtyMinus = new Button();
            lblQtyValue = new Label();
            btnQtyPlus = new Button();
            btnThem = new Button();
            pnlHeader.SuspendLayout();
            pnlScroll.SuspendLayout();
            pnlFooter.SuspendLayout();
            tlpFooter.SuspendLayout();
            flpQty.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(btnClose);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(16, 10, 8, 8);
            pnlHeader.Size = new Size(786, 56);
            pnlHeader.TabIndex = 0;
            // 
            // btnClose
            // 
            btnClose.Cursor = Cursors.Hand;
            btnClose.Dock = DockStyle.Right;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 240, 240);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btnClose.ForeColor = Color.FromArgb(108, 117, 125);
            btnClose.Location = new Point(742, 10);
            btnClose.Margin = new Padding(0);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(36, 38);
            btnClose.TabIndex = 1;
            btnClose.TabStop = false;
            btnClose.Text = "×";
            btnClose.UseVisualStyleBackColor = false;
            btnClose.Click += btnClose_Click;
            // 
            // lblTitle
            // 
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(33, 37, 41);
            lblTitle.Location = new Point(16, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(0, 0, 8, 0);
            lblTitle.Size = new Size(726, 38);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "BÁNH MÌ CẦY SIGNATURE  69.000đ";
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;
            lblTitle.UseMnemonic = false;
            // 
            // pnlScroll
            // 
            pnlScroll.AutoScroll = true;
            pnlScroll.BackColor = Color.White;
            pnlScroll.Controls.Add(flpSections);
            pnlScroll.Dock = DockStyle.Fill;
            pnlScroll.Location = new Point(0, 56);
            pnlScroll.Name = "pnlScroll";
            pnlScroll.Padding = new Padding(12, 4, 4, 4);
            pnlScroll.Size = new Size(786, 412);
            pnlScroll.TabIndex = 1;
            // 
            // flpSections
            // 
            flpSections.AutoSize = true;
            flpSections.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flpSections.Dock = DockStyle.Top;
            flpSections.FlowDirection = FlowDirection.TopDown;
            flpSections.Location = new Point(12, 4);
            flpSections.Name = "flpSections";
            flpSections.Padding = new Padding(4, 0, 12, 8);
            flpSections.Size = new Size(770, 8);
            flpSections.TabIndex = 0;
            flpSections.WrapContents = false;
            // 
            // pnlFooter
            // 
            pnlFooter.BackColor = Color.White;
            pnlFooter.Controls.Add(tlpFooter);
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Location = new Point(0, 468);
            pnlFooter.Name = "pnlFooter";
            pnlFooter.Padding = new Padding(16, 6, 16, 12);
            pnlFooter.Size = new Size(786, 154);
            pnlFooter.TabIndex = 2;
            // 
            // tlpFooter
            // 
            tlpFooter.ColumnCount = 1;
            tlpFooter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpFooter.Controls.Add(lblPromo, 0, 0);
            tlpFooter.Controls.Add(flpQty, 0, 1);
            tlpFooter.Controls.Add(btnThem, 0, 2);
            tlpFooter.Dock = DockStyle.Fill;
            tlpFooter.Location = new Point(16, 6);
            tlpFooter.Name = "tlpFooter";
            tlpFooter.RowCount = 3;
            tlpFooter.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tlpFooter.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tlpFooter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpFooter.Size = new Size(754, 136);
            tlpFooter.TabIndex = 0;
            // 
            // lblPromo
            // 
            lblPromo.Dock = DockStyle.Fill;
            lblPromo.Font = new Font("Segoe UI", 9F);
            lblPromo.ForeColor = Color.FromArgb(39, 174, 96);
            lblPromo.Location = new Point(0, 0);
            lblPromo.Margin = new Padding(0);
            lblPromo.Name = "lblPromo";
            lblPromo.Size = new Size(754, 28);
            lblPromo.TabIndex = 0;
            lblPromo.TextAlign = ContentAlignment.MiddleLeft;
            lblPromo.UseMnemonic = false;
            // 
            // flpQty
            // 
            flpQty.Controls.Add(lblQty);
            flpQty.Controls.Add(btnQtyMinus);
            flpQty.Controls.Add(lblQtyValue);
            flpQty.Controls.Add(btnQtyPlus);
            flpQty.Dock = DockStyle.Fill;
            flpQty.Location = new Point(0, 28);
            flpQty.Margin = new Padding(0);
            flpQty.Name = "flpQty";
            flpQty.Size = new Size(754, 44);
            flpQty.TabIndex = 1;
            flpQty.WrapContents = false;
            // 
            // lblQty
            // 
            lblQty.AutoSize = true;
            lblQty.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblQty.ForeColor = Color.FromArgb(44, 62, 80);
            lblQty.Location = new Point(3, 8);
            lblQty.Margin = new Padding(3, 8, 10, 0);
            lblQty.Name = "lblQty";
            lblQty.Size = new Size(79, 23);
            lblQty.TabIndex = 0;
            lblQty.Text = "Số lượng";
            // 
            // btnQtyMinus
            // 
            btnQtyMinus.Cursor = Cursors.Hand;
            btnQtyMinus.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnQtyMinus.FlatStyle = FlatStyle.Flat;
            btnQtyMinus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnQtyMinus.Location = new Point(95, 3);
            btnQtyMinus.Name = "btnQtyMinus";
            btnQtyMinus.Size = new Size(36, 32);
            btnQtyMinus.TabIndex = 1;
            btnQtyMinus.Text = "−";
            btnQtyMinus.UseVisualStyleBackColor = true;
            btnQtyMinus.Click += btnQtyMinus_Click;
            // 
            // lblQtyValue
            // 
            lblQtyValue.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblQtyValue.Location = new Point(137, 0);
            lblQtyValue.Name = "lblQtyValue";
            lblQtyValue.Size = new Size(40, 32);
            lblQtyValue.TabIndex = 2;
            lblQtyValue.Text = "1";
            lblQtyValue.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnQtyPlus
            // 
            btnQtyPlus.Cursor = Cursors.Hand;
            btnQtyPlus.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
            btnQtyPlus.FlatStyle = FlatStyle.Flat;
            btnQtyPlus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnQtyPlus.Location = new Point(183, 3);
            btnQtyPlus.Name = "btnQtyPlus";
            btnQtyPlus.Size = new Size(36, 32);
            btnQtyPlus.TabIndex = 3;
            btnQtyPlus.Text = "+";
            btnQtyPlus.UseVisualStyleBackColor = true;
            btnQtyPlus.Click += btnQtyPlus_Click;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(41, 128, 185);
            btnThem.Cursor = Cursors.Hand;
            btnThem.Dock = DockStyle.Fill;
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.FlatAppearance.MouseOverBackColor = Color.FromArgb(32, 104, 151);
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(0, 76);
            btnThem.Margin = new Padding(0, 4, 0, 0);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(754, 60);
            btnThem.TabIndex = 2;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = false;
            btnThem.Click += btnThem_Click;
            // 
            // FormTuyChinhMon
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            ClientSize = new Size(786, 622);
            Controls.Add(pnlScroll);
            Controls.Add(pnlFooter);
            Controls.Add(pnlHeader);
            Font = new Font("Segoe UI", 10F);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormTuyChinhMon";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Tùy chỉnh món";
            KeyPreview = true;
            pnlHeader.ResumeLayout(false);
            pnlScroll.ResumeLayout(false);
            pnlScroll.PerformLayout();
            pnlFooter.ResumeLayout(false);
            tlpFooter.ResumeLayout(false);
            flpQty.ResumeLayout(false);
            flpQty.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Button btnClose;
        private Label lblTitle;
        private Panel pnlScroll;
        private FlowLayoutPanel flpSections;
        private Panel pnlFooter;
        private TableLayoutPanel tlpFooter;
        private Label lblPromo;
        private FlowLayoutPanel flpQty;
        private Label lblQty;
        private Button btnQtyMinus;
        private Label lblQtyValue;
        private Button btnQtyPlus;
        private Button btnThem;
    }
}
