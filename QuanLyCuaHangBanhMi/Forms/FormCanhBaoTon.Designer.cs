using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormCanhBaoTon
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
            lblCN = new Label();
            cboChiNhanh = new ComboBox();
            txtTim = new TextBox();
            chkChiSapHet = new CheckBox();
            btnLamMoi = new Button();
            dgvTon = new DataGridView();
            lblTongKet = new Label();
            ((System.ComponentModel.ISupportInitialize)Err).BeginInit();
            tlpRoot.SuspendLayout();
            flowLoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTon).BeginInit();
            SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.ColumnCount = 1;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.Controls.Add(flowLoc, 0, 0);
            tlpRoot.Controls.Add(dgvTon, 0, 1);
            tlpRoot.Controls.Add(lblTongKet, 0, 2);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.Padding = new Padding(12);
            tlpRoot.RowCount = 3;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tlpRoot.Size = new Size(1217, 693);
            tlpRoot.TabIndex = 0;
            // 
            // flowLoc
            // 
            flowLoc.Controls.Add(lblCN);
            flowLoc.Controls.Add(cboChiNhanh);
            flowLoc.Controls.Add(txtTim);
            flowLoc.Controls.Add(chkChiSapHet);
            flowLoc.Controls.Add(btnLamMoi);
            flowLoc.Dock = DockStyle.Fill;
            flowLoc.Location = new Point(15, 15);
            flowLoc.Name = "flowLoc";
            flowLoc.Size = new Size(1187, 46);
            flowLoc.TabIndex = 0;
            // 
            // lblCN
            // 
            lblCN.AutoSize = true;
            lblCN.Location = new Point(3, 9);
            lblCN.Margin = new Padding(3, 9, 3, 3);
            lblCN.Name = "lblCN";
            lblCN.Size = new Size(98, 23);
            lblCN.TabIndex = 0;
            lblCN.Text = "Chi nhánh: ";
            // 
            // cboChiNhanh
            // 
            cboChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboChiNhanh.FormattingEnabled = true;
            cboChiNhanh.Location = new Point(107, 6);
            cboChiNhanh.Margin = new Padding(3, 6, 18, 3);
            cboChiNhanh.Name = "cboChiNhanh";
            cboChiNhanh.Size = new Size(240, 31);
            cboChiNhanh.TabIndex = 1;
            // 
            // txtTim
            // 
            txtTim.Location = new Point(368, 6);
            txtTim.Margin = new Padding(3, 6, 18, 3);
            txtTim.Name = "txtTim";
            txtTim.PlaceholderText = "Tìm nguyên liệu ...";
            txtTim.Size = new Size(240, 30);
            txtTim.TabIndex = 2;
            // 
            // chkChiSapHet
            // 
            chkChiSapHet.AutoSize = true;
            chkChiSapHet.Checked = true;
            chkChiSapHet.CheckState = CheckState.Checked;
            chkChiSapHet.Location = new Point(629, 9);
            chkChiSapHet.Margin = new Padding(3, 9, 18, 3);
            chkChiSapHet.Name = "chkChiSapHet";
            chkChiSapHet.Size = new Size(200, 27);
            chkChiSapHet.TabIndex = 3;
            chkChiSapHet.Text = "Chỉ hiện hàng sắp hết";
            chkChiSapHet.UseVisualStyleBackColor = true;
            // 
            // btnLamMoi
            // 
            btnLamMoi.Location = new Point(850, 3);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(110, 29);
            btnLamMoi.TabIndex = 4;
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;
            // 
            // dgvTon
            // 
            dgvTon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTon.Dock = DockStyle.Fill;
            dgvTon.Location = new Point(15, 67);
            dgvTon.Name = "dgvTon";
            dgvTon.RowHeadersWidth = 51;
            dgvTon.Size = new Size(1187, 579);
            dgvTon.TabIndex = 1;
            // 
            // lblTongKet
            // 
            lblTongKet.Dock = DockStyle.Fill;
            lblTongKet.Location = new Point(15, 649);
            lblTongKet.Name = "lblTongKet";
            lblTongKet.Size = new Size(1187, 32);
            lblTongKet.TabIndex = 2;
            lblTongKet.Text = "label1";
            lblTongKet.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // FrmCanhBaoTon
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1217, 693);
            Controls.Add(tlpRoot);
            MinimumSize = new Size(900, 500);
            Name = "FrmCanhBaoTon";
            Text = "Cảnh báo tồn";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)Err).EndInit();
            tlpRoot.ResumeLayout(false);
            flowLoc.ResumeLayout(false);
            flowLoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTon).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRoot;
        private FlowLayoutPanel flowLoc;
        private Label lblCN;
        private ComboBox cboChiNhanh;
        private DataGridView dgvTon;
        private Label lblTongKet;
        private TextBox txtTim;
        private CheckBox chkChiSapHet;
        private Button btnLamMoi;
    }
}