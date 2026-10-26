using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormCanhBaoTon : FrmBase
    {
        private bool _dangNap;   // chặn sự kiện chạy lúc đang gán dữ liệu cho ComboBox

        public FormCanhBaoTon()
        {
            InitializeComponent();

            // ----- giao diện -----
            dgvTon.ApplyTheme();
            btnLamMoi.StyleSecondary();
            TrangThaiUi.Hook(dgvTon, "TrangThai");      // chỉ nối một lần ở đây
            lblTongKet.ForeColor = Theme.Muted;
            KeyPreview = true;                          // để bắt phím F5

            // ----- danh sách chi nhánh -----
            _dangNap = true;
            cboChiNhanh.BindList(LookupService.ChiNhanh(), "TenChiNhanh", "MaChiNhanh");
            cboChiNhanh.SelectedValue = Session.MaChiNhanh;
            cboChiNhanh.Enabled = Session.LaAdmin;      // chỉ ADMIN được xem chi nhánh khác
            _dangNap = false;

            // ----- sự kiện (nối bằng code, không double-click trong Designer) -----
            cboChiNhanh.SelectedIndexChanged += (_, _) => NapTon();
            txtTim.TextChanged += (_, _) => NapTon();
            chkChiSapHet.CheckedChanged += (_, _) => NapTon();
            btnLamMoi.Click += (_, _) => NapTon();
            KeyDown += (_, e) => { if (e.KeyCode == Keys.F5) NapTon(); };
        }

        /// <summary>FrmBase tự gọi hàm này khi form mở (sự kiện Load).</summary>
        public override void RefreshData() => NapTon();

        private void NapTon()
        {
            if (_dangNap) return;
            if (cboChiNhanh.SelectedValue is not int maCN) return;

            try
            {
                DataTable dt = TonKhoService.GetTon(maCN, txtTim.Text.Trim(), chkChiSapHet.Checked);
                dgvTon.DataSource = dt;

                dgvTon.HideCols("MaNguyenLieu");
                dgvTon.Heads(
                    ("TenNguyenLieu", "Nguyên liệu"),
                    ("TenDonVi", "Đơn vị"),
                    ("SoLuongTon", "Tồn kho"),
                    ("TonToiThieu", "Tồn tối thiểu"),
                    ("CanBoSung", "Cần bổ sung"),
                    ("NgayCapNhat", "Cập nhật lúc"),
                    ("TrangThai", "Trạng thái"));

                dgvTon.Columns["SoLuongTon"]!.AsNumber("#,##0.###");
                dgvTon.Columns["TonToiThieu"]!.AsNumber("#,##0.###");
                dgvTon.Columns["CanBoSung"]!.AsNumber("#,##0.###;-#,##0.###;");   // số 0 thì để trống
                dgvTon.Columns["NgayCapNhat"]!.AsDate("dd/MM/yyyy HH:mm");

                int het = dt.AsEnumerable().Count(r => (string)r["TrangThai"] == "Het");
                int sap = dt.AsEnumerable().Count(r => (string)r["TrangThai"] == "SapHet");

                lblTongKet.Text = dt.Rows.Count == 0 && chkChiSapHet.Checked
                    ? "Tất cả nguyên liệu đều trên mức tồn tối thiểu."
                    : $"{dt.Rows.Count} nguyên liệu   •   {het} hết hàng   •   {sap} sắp hết";
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }
    }
}