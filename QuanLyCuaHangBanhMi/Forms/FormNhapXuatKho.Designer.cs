using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace QuanLyCuaHangBanhMi.Forms
{
    partial class FormNhapXuatKho
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
            flowTop = new FlowLayoutPanel();
            lblCN = new Label();
            cboChiNhanh = new ComboBox();
            tabMain = new TabControl();
            tpNhap = new TabPage();
            tpHuy = new TabPage();
            tpLichSu = new TabPage();
            ((System.ComponentModel.ISupportInitialize)Err).BeginInit();
            tlpRoot.SuspendLayout();
            flowTop.SuspendLayout();
            tabMain.SuspendLayout();
            SuspendLayout();
            // 
            // tlpRoot
            // 
            tlpRoot.ColumnCount = 1;
            tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpRoot.Controls.Add(flowTop, 0, 0);
            tlpRoot.Controls.Add(tabMain, 0, 1);
            tlpRoot.Dock = DockStyle.Fill;
            tlpRoot.Location = new Point(0, 0);
            tlpRoot.Name = "tlpRoot";
            tlpRoot.Padding = new Padding(12);
            tlpRoot.RowCount = 2;
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.Size = new Size(1182, 673);
            tlpRoot.TabIndex = 0;
            // 
            // flowTop
            // 
            flowTop.Controls.Add(lblCN);
            flowTop.Controls.Add(cboChiNhanh);
            flowTop.Dock = DockStyle.Fill;
            flowTop.Location = new Point(15, 15);
            flowTop.Name = "flowTop";
            flowTop.Size = new Size(1152, 42);
            flowTop.TabIndex = 0;
            flowTop.WrapContents = false;
            // 
            // lblCN
            // 
            lblCN.AutoSize = true;
            lblCN.Location = new Point(3, 9);
            lblCN.Margin = new Padding(3, 9, 3, 3);
            lblCN.Name = "lblCN";
            lblCN.Size = new Size(93, 23);
            lblCN.TabIndex = 0;
            lblCN.Text = "Chi nhánh:";
            // 
            // cboChiNhanh
            // 
            cboChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboChiNhanh.FormattingEnabled = true;
            cboChiNhanh.Location = new Point(102, 6);
            cboChiNhanh.Margin = new Padding(3, 6, 3, 3);
            cboChiNhanh.Name = "cboChiNhanh";
            cboChiNhanh.Size = new Size(300, 31);
            cboChiNhanh.TabIndex = 1;
            // 
            // tabMain
            // 
            tabMain.Controls.Add(tpNhap);
            tabMain.Controls.Add(tpHuy);
            tabMain.Controls.Add(tpLichSu);
            tabMain.Dock = DockStyle.Fill;
            tabMain.Location = new Point(15, 63);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(1152, 595);
            tabMain.TabIndex = 1;
            // 
            // tpNhap
            // 
            tpNhap.Location = new Point(4, 32);
            tpNhap.Name = "tpNhap";
            tpNhap.Padding = new Padding(3);
            tpNhap.Size = new Size(1144, 559);
            tpNhap.TabIndex = 0;
            tpNhap.Text = "Nhập hàng";
            tpNhap.UseVisualStyleBackColor = true;
            // 
            // tpHuy
            // 
            tpHuy.Location = new Point(4, 32);
            tpHuy.Name = "tpHuy";
            tpHuy.Padding = new Padding(3);
            tpHuy.Size = new Size(1144, 559);
            tpHuy.TabIndex = 1;
            tpHuy.Text = "Hủy hao hụt";
            tpHuy.UseVisualStyleBackColor = true;
            // 
            // tpLichSu
            // 
            tpLichSu.Location = new Point(4, 32);
            tpLichSu.Name = "tpLichSu";
            tpLichSu.Size = new Size(1144, 559);
            tpLichSu.TabIndex = 2;
            tpLichSu.Text = "Lịch sử kho";
            tpLichSu.UseVisualStyleBackColor = true;
            // 
            // FrmNhapXuatKho
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1182, 673);
            Controls.Add(tlpRoot);
            MinimumSize = new Size(1000, 600);
            Name = "FrmNhapXuatKho";
            Text = "Nhập xuất kho";
            WindowState = FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)Err).EndInit();
            tlpRoot.ResumeLayout(false);
            flowTop.ResumeLayout(false);
            flowTop.PerformLayout();
            tabMain.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tlpRoot;
        private FlowLayoutPanel flowTop;
        private Label lblCN;
        private ComboBox cboChiNhanh;
        private TabControl tabMain;
        private TabPage tpNhap;
        private TabPage tpHuy;
        private TabPage tpLichSu;
    }
}