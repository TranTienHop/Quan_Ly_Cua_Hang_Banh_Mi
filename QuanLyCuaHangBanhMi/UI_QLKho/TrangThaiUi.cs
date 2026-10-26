namespace QuanLyCuaHangBanhMi.UI_QLKho
{
    public static class TrangThaiUi
    {
        private static readonly Dictionary<string, (string Text, Color Fore, Color Back)> Map = new()
        {
            ["ChoNhap"] = ("Chờ nhập", Theme.Warning, Color.FromArgb(254, 243, 199)),
            ["DaNhap"] = ("Đã nhập", Theme.Success, Color.FromArgb(220, 252, 231)),
            ["ChoGui"] = ("Chờ gửi", Theme.Warning, Color.FromArgb(254, 243, 199)),
            ["DangChuyen"] = ("Đang chuyển", Theme.Info, Color.FromArgb(219, 234, 254)),
            ["HoanThanh"] = ("Hoàn thành", Theme.Success, Color.FromArgb(220, 252, 231)),
            ["DaHuy"] = ("Đã hủy", Theme.Danger, Color.FromArgb(254, 226, 226)),
            ["TuChoi"] = ("Từ chối", Theme.Danger, Color.FromArgb(254, 226, 226)),
            ["ChuaTT"] = ("Chưa thanh toán", Theme.Danger, Color.FromArgb(254, 226, 226)),
            ["TraMotPhan"] = ("Trả một phần", Theme.Warning, Color.FromArgb(254, 243, 199)),
            ["DaTT"] = ("Đã thanh toán", Theme.Success, Color.FromArgb(220, 252, 231)),
            ["DaNhan"] = ("Đã nhận", Theme.Success, Color.FromArgb(220, 252, 231)),
            ["Draft"] = ("Nháp", Theme.Muted, Color.FromArgb(241, 245, 249)),
            ["DaDuyet"] = ("Đã duyệt", Theme.Success, Color.FromArgb(220, 252, 231)),
            ["DangKiem"] = ("Đang kiểm", Theme.Warning, Color.FromArgb(254, 243, 199)),
            ["Het"] = ("● Hết hàng", Theme.Danger, Color.FromArgb(254, 226, 226)),
            ["SapHet"] = ("▲ Sắp hết", Theme.Warning, Color.FromArgb(254, 243, 199)),
            ["Con"] = ("✔ Còn hàng", Theme.Success, Color.FromArgb(220, 252, 231)),
            ["Đang dùng"] = ("Đang dùng", Theme.Success, Color.FromArgb(220, 252, 231)),
            ["Ngừng dùng"] = ("Ngừng dùng", Theme.Muted, Color.FromArgb(241, 245, 249)),
            ["Đang hợp tác"] = ("Đang hợp tác", Theme.Success, Color.FromArgb(220, 252, 231)),
            ["Ngừng hợp tác"] = ("Ngừng hợp tác", Theme.Muted, Color.FromArgb(241, 245, 249)),
            ["DangDung"] = ("Đang dùng", Theme.Success, Color.FromArgb(220, 252, 231)),
            ["BaoTri"] = ("Bảo trì", Theme.Warning, Color.FromArgb(254, 243, 199)),
            ["HuHong"] = ("Hư hỏng", Theme.Danger, Color.FromArgb(254, 226, 226)),
            ["ThanhLy"] = ("Đã thanh lý", Theme.Muted, Color.FromArgb(241, 245, 249)),
        };

        /// <summary>Giá trị gốc trong ô vẫn là mã (vd "ChoNhap"); chỉ phần hiển thị đổi.</summary>
        public static void Hook(DataGridView g, string colName) => g.CellFormatting += (_, e) =>
        {
            if (e.RowIndex < 0 || e.Value is not string s || g.Columns[e.ColumnIndex].Name != colName) return;
            if (!Map.TryGetValue(s, out var m)) return;
            e.Value = m.Text;
            e.CellStyle!.ForeColor = m.Fore; e.CellStyle.BackColor = m.Back;
            e.CellStyle.SelectionForeColor = m.Fore; e.CellStyle.SelectionBackColor = m.Back;
            e.FormattingApplied = true;
        };
    }
}