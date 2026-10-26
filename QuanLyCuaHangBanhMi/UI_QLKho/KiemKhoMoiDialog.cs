using System.Data;

namespace QuanLyCuaHangBanhMi.UI_QLKho
{
    /// <summary>Chọn các nguyên liệu cần kiểm và ghi chú cho một đợt kiểm kho mới.</summary>
    public sealed class KiemKhoMoiDialog : Form
    {
        private sealed class Muc
        {
            public int Ma;
            public string Text = "";
            public override string ToString() => Text;
        }

        private readonly CheckedListBox _lst = new()
        { Dock = DockStyle.Fill, CheckOnClick = true, IntegralHeight = false };
        private readonly TextBox _txtGhiChu = new()
        { Dock = DockStyle.Fill, MaxLength = 255, PlaceholderText = "Ghi chú đợt kiểm (không bắt buộc)" };

        public string? GhiChu => string.IsNullOrWhiteSpace(_txtGhiChu.Text) ? null : _txtGhiChu.Text.Trim();
        public List<int> DaChon { get; } = new();

        public KiemKhoMoiDialog(DataTable nguyenLieu, string tenChiNhanh)
        {
            Text = "Bắt đầu kiểm kho";
            Size = new Size(580, 660);
            MinimumSize = new Size(480, 480);
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MinimizeBox = false;
            MaximizeBox = false;
            Font = Theme.Base;
            BackColor = Theme.Background;

            foreach (DataRow r in nguyenLieu.Rows)
                _lst.Items.Add(new Muc
                {
                    Ma = Convert.ToInt32(r["MaNguyenLieu"]),
                    Text = $"{r["TenNguyenLieu"]} ({r["TenDonVi"]})  —  tồn {Convert.ToDecimal(r["SoLuongTon"]):#,##0.###}"
                }, true);                                           // mặc định chọn tất cả

            var lbl = new Label
            {
                Text = $"Chọn các nguyên liệu cần kiểm tại {tenChiNhanh}.\n" +
                       "Mặc định kiểm tất cả; bỏ chọn những thứ không kiểm lần này (ví dụ: chỉ kiểm hàng giá trị cao).",
                Dock = DockStyle.Fill,
                ForeColor = Theme.Text,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var btnTatCa = new Button { Text = "Chọn tất cả", Width = 120 };
            var btnBo = new Button { Text = "Bỏ chọn tất cả", Width = 140 };
            var btnOk = new Button { Text = "Bắt đầu kiểm", Width = 140 };
            var btnHuy = new Button { Text = "Hủy", Width = 90, DialogResult = DialogResult.Cancel };
            btnTatCa.StyleSecondary(); btnBo.StyleSecondary(); btnHuy.StyleSecondary(); btnOk.StylePrimary();

            btnTatCa.Click += (_, _) => DatTatCa(true);
            btnBo.Click += (_, _) => DatTatCa(false);
            btnOk.Click += (_, _) =>
            {
                DaChon.Clear();
                foreach (Muc m in _lst.CheckedItems) DaChon.Add(m.Ma);
                if (DaChon.Count == 0)
                {
                    Msg.Bao("Hãy chọn ít nhất một nguyên liệu để kiểm.");
                    return;                                         // không đóng hộp thoại
                }
                DialogResult = DialogResult.OK;
            };

            var flowTrai = new FlowLayoutPanel { Dock = DockStyle.Fill };
            flowTrai.Controls.Add(btnTatCa);
            flowTrai.Controls.Add(btnBo);
            var flowPhai = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            flowPhai.Controls.Add(btnHuy);                          // RightToLeft: nút thêm trước nằm ngoài cùng bên phải
            flowPhai.Controls.Add(btnOk);

            var nut = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
            nut.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            nut.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250));
            nut.Controls.Add(flowTrai, 0, 0);
            nut.Controls.Add(flowPhai, 1, 0);

            var tlp = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(12) };
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
            tlp.Controls.Add(lbl, 0, 0);
            tlp.Controls.Add(_txtGhiChu, 0, 1);
            tlp.Controls.Add(_lst, 0, 2);
            tlp.Controls.Add(nut, 0, 3);
            Controls.Add(tlp);

            AcceptButton = btnOk;
            CancelButton = btnHuy;
        }

        private void DatTatCa(bool chon)
        {
            for (int i = 0; i < _lst.Items.Count; i++) _lst.SetItemChecked(i, chon);
        }
    }
}