using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using System.Runtime.Intrinsics.Arm;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormNhapXuatKho : FrmBase
    {
        private readonly ucNhapHang _nhap = new() { Dock = DockStyle.Fill };
        private readonly ucHuyHaoHut _huy = new() { Dock = DockStyle.Fill };
        private readonly ucLichSuKho _lichSu = new() { Dock = DockStyle.Fill };
        private bool _dangNap;                     // chặn sự kiện khi đang gán danh sách chi nhánh

        private int MaCN => cboChiNhanh.SelectedValue is int v ? v : Session.MaChiNhanh;

        public FormNhapXuatKho()
        {
            InitializeComponent();

            foreach (var tp in new[] { tpNhap, tpHuy, tpLichSu })
            {
                tp.UseVisualStyleBackColor = false;
                tp.BackColor = Theme.Background;
            }
            tpNhap.Controls.Add(_nhap);
            tpHuy.Controls.Add(_huy);
            tpLichSu.Controls.Add(_lichSu);

            cboChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            KeyPreview = true;

            cboChiNhanh.SelectedIndexChanged += (_, _) => NapTabHienTai();
            tabMain.SelectedIndexChanged += (_, _) => NapTabHienTai();
            KeyDown += (_, e) => { if (e.KeyCode == Keys.F5) NapTabHienTai(); };
        }

        /// <summary>FrmBase tự gọi khi form mở (sự kiện Load).</summary>
        public override void RefreshData()
        {
            _dangNap = true;
            cboChiNhanh.BindList(LookupService.ChiNhanh(), "TenChiNhanh", "MaChiNhanh");
            cboChiNhanh.SelectedValue = Session.MaChiNhanh;
            cboChiNhanh.Enabled = Session.LaAdmin;      // chỉ ADMIN được đổi chi nhánh
            _dangNap = false;

            NapTabHienTai();
        }

        private void NapTabHienTai()
        {
            if (_dangNap) return;

            switch (tabMain.SelectedIndex)
            {
                case 0:
                    _nhap.SetChiNhanh(MaCN, cboChiNhanh.Text);
                    _nhap.RefreshData();
                    break;
                case 1:
                    _huy.SetChiNhanh(MaCN, cboChiNhanh.Text);
                    _huy.RefreshData();
                    break;
                case 2:
                    _lichSu.SetChiNhanh(MaCN, cboChiNhanh.Text);
                    _lichSu.RefreshData();
                    break;
            }
        }
    }
}