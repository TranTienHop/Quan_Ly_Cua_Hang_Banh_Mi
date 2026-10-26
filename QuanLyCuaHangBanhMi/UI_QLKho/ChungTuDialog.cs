using System.Data;

namespace QuanLyCuaHangBanhMi.UI_QLKho
{
    /// <summary>Hộp thoại chỉ đọc hiển thị chứng từ gốc của một dòng nhật ký kho.</summary>
    public sealed class ChungTuDialog : Form
    {
        public ChungTuDialog(string tieuDe, DataTable dong)
        {
            Text = "Chứng từ gốc";
            Size = new Size(820, 520);
            MinimumSize = new Size(600, 360);
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            MinimizeBox = false;
            Font = Theme.Base;
            BackColor = Theme.Background;

            var tlp = new TableLayoutPanel
            { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Padding = new Padding(12) };
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 84));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));

            var lbl = new Label
            {
                Text = tieuDe,
                Dock = DockStyle.Fill,
                Font = Theme.Bold,
                ForeColor = Theme.Text,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var dgv = new DataGridView { Dock = DockStyle.Fill };
            dgv.ApplyTheme();
            dgv.DataSource = dong;
            foreach (DataGridViewColumn c in dgv.Columns)
                if (c.ValueType == typeof(decimal) || c.ValueType == typeof(int))
                    c.AsNumber("#,##0.###");

            var btn = new Button { Text = "Đóng", Width = 110, DialogResult = DialogResult.Cancel };
            btn.StyleSecondary();
            var flow = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            flow.Controls.Add(btn);
            CancelButton = btn;                         // Esc để đóng

            tlp.Controls.Add(lbl, 0, 0);
            tlp.Controls.Add(dgv, 0, 1);
            tlp.Controls.Add(flow, 0, 2);
            Controls.Add(tlp);
        }
    }
}