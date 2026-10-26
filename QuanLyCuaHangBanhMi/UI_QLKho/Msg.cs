namespace QuanLyCuaHangBanhMi.UI_QLKho
{
    public static class Msg
    {
        public static void Loi(Exception ex) =>
            MessageBox.Show(ex.Message, "Không thực hiện được", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        public static void Bao(string m) =>
            MessageBox.Show(m, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        public static bool Hoi(string m) =>
            MessageBox.Show(m, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    public static class Prompt
    {
        private static Form Khung(string title, string label, Control input, int h = 140)
        {
            var f = new Form
            {
                Text = title,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                ShowInTaskbar = false,
                ClientSize = new Size(360, h),
                Font = Theme.Base
            };
            var lbl = new Label { Text = label, Left = 16, Top = 14, Width = 330, Height = 40 };
            input.Left = 16; input.Top = 56; input.Width = 328;
            var ok = new Button { Text = "Đồng ý", Left = 150, Top = h - 44, Width = 94, DialogResult = DialogResult.OK };
            var no = new Button { Text = "Hủy", Left = 250, Top = h - 44, Width = 94, DialogResult = DialogResult.Cancel };
            f.Controls.AddRange(new Control[] { lbl, input, ok, no });
            f.AcceptButton = ok; f.CancelButton = no;
            return f;
        }

        public static decimal? Number(string title, string label, decimal max, decimal def)
        {
            var num = new NumericUpDown
            { Maximum = max, Minimum = 0, DecimalPlaces = 0, ThousandsSeparator = true, Value = Math.Min(def, max) };
            using var f = Khung(title, label, num);
            return f.ShowDialog() == DialogResult.OK ? num.Value : null;
        }

        public static string? Text(string title, string label)
        {
            var txt = new TextBox();
            using var f = Khung(title, label, txt);
            return f.ShowDialog() == DialogResult.OK ? txt.Text.Trim() : null;
        }
    }
}