using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace QuanLyCuaHangBanhMi.UI_QLKho
{
    public static class UiExtensions
    {
        // ---------- Nút ----------
        public static void StylePrimary(this Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = Theme.Primary; b.ForeColor = Color.White;
            b.Font = Theme.Bold; b.Height = 36; b.Cursor = Cursors.Hand;
        }

        public static void StyleSecondary(this Button b)    // nút viền: Làm mới, Đóng...
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = Theme.Border;
            b.BackColor = Color.White; b.ForeColor = Theme.Text;
            b.Font = Theme.Base; b.Height = 36; b.Cursor = Cursors.Hand;
        }

        public static void StyleDanger(this Button b)
        {
            b.StylePrimary();
            b.BackColor = Theme.Danger;
        }

        // ---------- DataGridView ----------
        public static void ApplyTheme(this DataGridView g)
        {
            g.EnableHeadersVisualStyles = false;
            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Theme.Surface;
            g.GridColor = Theme.Border;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToResizeRows = false;
            g.ReadOnly = true;
            g.MultiSelect = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowTemplate.Height = 34;

            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 38;
            var h = g.ColumnHeadersDefaultCellStyle;
            h.BackColor = Color.FromArgb(241, 245, 249);
            h.ForeColor = Theme.Text;
            h.Font = Theme.Bold;
            h.SelectionBackColor = h.BackColor;
            h.SelectionForeColor = h.ForeColor;

            g.DefaultCellStyle.Font = Theme.Base;
            g.DefaultCellStyle.ForeColor = Theme.Text;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 237, 213);
            g.DefaultCellStyle.SelectionForeColor = Theme.Text;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 251);

            // Chống nháy khi cuộn (DoubleBuffered là thuộc tính protected)
            typeof(DataGridView).GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(g, true);
        }

        public static void Heads(this DataGridView g, params (string Col, string Text)[] hs)
        {
            foreach (var (c, t) in hs)
                if (g.Columns.Contains(c)) g.Columns[c]!.HeaderText = t;
        }

        public static void HideCols(this DataGridView g, params string[] cols)
        {
            foreach (var c in cols)
                if (g.Columns.Contains(c)) g.Columns[c]!.Visible = false;
        }

        /// <summary>Cột số: canh phải + định dạng. Gọi SAU khi gán DataSource.</summary>
        public static void AsNumber(this DataGridViewColumn c, string format = "N0")
        {
            c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            c.DefaultCellStyle.Format = format;
            c.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        public static void AsDate(this DataGridViewColumn c, string fmt = "dd/MM/yyyy")
            => c.DefaultCellStyle.Format = fmt;

        /// <summary>Các cột lưu giờ UTC (SYSUTCDATETIME) → hiển thị giờ máy.</summary>
        public static void UtcColumns(this DataGridView g, params string[] cols)
        {
            if (cols.Length == 0) return;
            g.CellFormatting += (_, e) =>
            {
                if (e.RowIndex < 0 || e.Value is not DateTime d) return;
                if (!cols.Contains(g.Columns[e.ColumnIndex].Name)) return;
                e.Value = d.ToLocalTime().ToString("dd/MM/yyyy HH:mm");
                e.FormattingApplied = true;
            };
        }

        public static void ExportCsv(this DataGridView g, string path)
        {
            string sep = CultureInfo.CurrentCulture.TextInfo.ListSeparator;   // vi-VN dùng ';'
            static string Esc(string? s) => "\"" + (s ?? "").Replace("\"", "\"\"") + "\"";
            var cols = g.Columns.Cast<DataGridViewColumn>().Where(c => c.Visible).ToList();
            var sb = new StringBuilder();
            sb.AppendLine(string.Join(sep, cols.Select(c => Esc(c.HeaderText))));
            foreach (DataGridViewRow r in g.Rows)
                sb.AppendLine(string.Join(sep, cols.Select(c => Esc(r.Cells[c.Index].FormattedValue?.ToString()))));
            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));  // BOM để Excel đọc đúng tiếng Việt
        }

        // ---------- ComboBox / DataTable ----------
        public static void BindList(this ComboBox c, DataTable dt, string display, string value)
        {
            c.DisplayMember = display; c.ValueMember = value; c.DataSource = dt;   // đặt Member trước DataSource
        }

        public static DataTable ThemTatCa(this DataTable dt, string textCol, string valueCol,
                                          object allValue, string text = "(Tất cả)")
        {
            var r = dt.NewRow(); r[valueCol] = allValue; r[textCol] = text;
            dt.Rows.InsertAt(r, 0);
            return dt;
        }
    }
}