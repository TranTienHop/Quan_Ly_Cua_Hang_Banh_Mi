using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyCuaHangBanhMi.Forms
{
    /// <summary>
    /// Hộp thoại nhập ghi chú cho 1 món trong đơn (ít cay, không hành...), có nút ghi chú nhanh.
    /// </summary>
    public sealed partial class FormGhiChuMon : Form
    {
        private static readonly string[] QuickNotes =
        {
            "Không cay", "Ít cay", "Nhiều cay", "Không hành", "Không rau", "Thêm pate",
            "Nướng giòn", "Ít đá", "Không đá", "Ít ngọt", "Đá riêng"
        };

        public string NoteText => txtNote.Text.Trim();

        public FormGhiChuMon(string tenMon, string? currentNote)
        {
            InitializeComponent();
            lblTitle.Text = "📝 " + tenMon;
            txtNote.Text = currentNote ?? "";

            foreach (string note in QuickNotes)
            {
                var btn = new Button
                {
                    Text = note,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(255, 244, 222),
                    ForeColor = Color.FromArgb(160, 95, 0),
                    Font = new Font("Segoe UI", 9F),
                    Margin = new Padding(S(3)),
                    Padding = new Padding(S(4), S(2), S(4), S(2)),
                    Cursor = Cursors.Hand
                };
                btn.FlatAppearance.BorderColor = Color.FromArgb(242, 153, 0);
                btn.Click += (s, e) => ToggleQuickNote(note);
                flpQuick.Controls.Add(btn);
            }
        }

        private int S(int value) => (int)Math.Round(value * DeviceDpi / 96f);

        private void FormGhiChuMon_Shown(object? sender, EventArgs e)
        {
            txtNote.Focus();
            txtNote.SelectionStart = txtNote.TextLength;
        }

        private void ToggleQuickNote(string note)
        {
            var parts = txtNote.Text.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
            if (parts.Contains(note, StringComparer.OrdinalIgnoreCase))
                parts.RemoveAll(p => p.Equals(note, StringComparison.OrdinalIgnoreCase));
            else
                parts.Add(note);

            string text = string.Join(", ", parts);
            txtNote.Text = text.Length <= txtNote.MaxLength ? text : text[..txtNote.MaxLength];
            txtNote.SelectionStart = txtNote.TextLength;
            txtNote.Focus();
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
