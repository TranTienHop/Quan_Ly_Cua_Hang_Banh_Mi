using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyCuaHangBanhMi.Forms
{
    /// <summary>
    /// Chọn chương trình khuyến mãi cho đơn đang order. Hiện số tiền giảm dự kiến và điều kiện của từng chương trình.
    /// </summary>
    public sealed partial class FormChonKhuyenMai : Form
    {
        private readonly CultureInfo _vnCulture = new CultureInfo("vi-VN");
        private readonly DataTable _promoTable;
        private readonly decimal _baseAmount;
        private readonly bool _isAllowed;
        private readonly DataTable _viewTable = new DataTable();

        /// <summary>Mã khuyến mãi được chọn; null = bỏ áp dụng.</summary>
        public int? SelectedPromoId { get; private set; }

        public FormChonKhuyenMai(DataTable promoTable, decimal baseAmount, int? currentPromoId, bool isAllowed)
        {
            _promoTable = promoTable;
            _baseAmount = baseAmount;
            _isAllowed = isAllowed;
            SelectedPromoId = currentPromoId;

            InitializeComponent();

            // AutoScaleMode.Dpi không co giãn chiều cao tiêu đề / dòng của DataGridView
            dgvPromos.ColumnHeadersHeight = S(dgvPromos.ColumnHeadersHeight);
            dgvPromos.RowTemplate.Height = S(dgvPromos.RowTemplate.Height);
            colDonToiThieu.DefaultCellStyle.FormatProvider = _vnCulture;
            colHanDung.DefaultCellStyle.FormatProvider = _vnCulture;
            colTienGiam.DefaultCellStyle.FormatProvider = _vnCulture;

            lblStatus.Text = _isAllowed
                ? $"Tiền hàng tính khuyến mãi (không tính combo): {_baseAmount.ToString("N0", _vnCulture)} đ"
                : "⚠ Khuyến mãi chỉ áp dụng cho đơn bán tại quầy (không áp cho GrabFood / ShopeeFood).";
            if (!_isAllowed) lblStatus.ForeColor = Color.FromArgb(211, 84, 0);

            _viewTable.Columns.Add("MaKhuyenMai", typeof(int));
            _viewTable.Columns.Add("MaCode", typeof(string));
            _viewTable.Columns.Add("TenKhuyenMai", typeof(string));
            _viewTable.Columns.Add("MucGiam", typeof(string));
            _viewTable.Columns.Add("DonToiThieu", typeof(decimal));
            _viewTable.Columns.Add("HanDung", typeof(DateTime));
            _viewTable.Columns.Add("TienGiam", typeof(decimal));
            _viewTable.Columns.Add("DieuKien", typeof(string));

            // Cột đã định nghĩa trong Designer, gán DataSource sau cùng
            dgvPromos.AutoGenerateColumns = false;
            dgvPromos.DataSource = _viewTable;
        }

        private int S(int value) => (int)Math.Round(value * DeviceDpi / 96f);

        private void FormChonKhuyenMai_Shown(object? sender, EventArgs e)
        {
            FillView();
            txtSearch.Focus();
        }

        private void txtSearch_TextChanged(object? sender, EventArgs e) => FillView();

        private void dgvPromos_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) ApplySelected();
        }

        private void dgvPromos_SelectionChanged(object? sender, EventArgs e) => UpdateApplyButton();

        private void btnApply_Click(object? sender, EventArgs e) => ApplySelected();

        private void btnRemove_Click(object? sender, EventArgs e)
        {
            SelectedPromoId = null;
            DialogResult = DialogResult.OK;
        }

        private void FillView()
        {
            string keyword = FormOrder.RemoveDiacritics(txtSearch.Text.Trim());
            _viewTable.Rows.Clear();
            foreach (DataRow p in _promoTable.Rows)
            {
                string ten = p.Field<string>("TenKhuyenMai") ?? "";
                string ma = p.Field<string>("MaCode") ?? "";
                if (keyword.Length > 0
                    && !FormOrder.RemoveDiacritics(ten).Contains(keyword)
                    && !FormOrder.RemoveDiacritics(ma).Contains(keyword)) continue;

                decimal giaTri = p.Field<decimal>("GiaTriGiam");
                decimal donToiThieu = p.Field<decimal>("DonToiThieu");
                string mucGiam = p.Field<string>("LoaiGiam") == "PhanTram"
                    ? $"{giaTri:0.##}%" + (p.IsNull("GiamToiDa") ? "" : $" (tối đa {p.Field<decimal>("GiamToiDa").ToString("N0", _vnCulture)})")
                    : giaTri.ToString("N0", _vnCulture) + " đ";

                decimal tienGiam = _isAllowed ? FormOrder.CalculatePromoDiscount(p, _baseAmount) : 0;
                string dieuKien = !_isAllowed ? "Không áp dụng"
                    : _baseAmount >= donToiThieu ? "✔ Đủ điều kiện"
                    : $"Thiếu {(donToiThieu - _baseAmount).ToString("N0", _vnCulture)} đ";

                _viewTable.Rows.Add(p["MaKhuyenMai"], ma, ten, mucGiam, donToiThieu, p["NgayKetThuc"], tienGiam, dieuKien);
            }

            if (_promoTable.Rows.Count == 0)
                lblStatus.Text = "Hiện chi nhánh không có chương trình khuyến mãi nào đang chạy.";

            // Giữ dòng đang áp dụng được chọn sẵn
            dgvPromos.ClearSelection();
            foreach (DataGridViewRow row in dgvPromos.Rows)
            {
                if (row.DataBoundItem is DataRowView v && SelectedPromoId.HasValue && v.Row.Field<int>("MaKhuyenMai") == SelectedPromoId.Value)
                {
                    row.Selected = true;
                    dgvPromos.CurrentCell = row.Cells[0];
                    break;
                }
            }
            UpdateApplyButton();
        }

        private DataRow? CurrentPromo() => (dgvPromos.CurrentRow?.DataBoundItem as DataRowView)?.Row;

        private void UpdateApplyButton()
        {
            var row = CurrentPromo();
            btnApply.Enabled = _isAllowed && row != null && row.Field<decimal>("TienGiam") > 0;
        }

        private void ApplySelected()
        {
            var row = CurrentPromo();
            if (row == null) return;
            if (!_isAllowed || row.Field<decimal>("TienGiam") <= 0)
            {
                MessageBox.Show("Đơn hàng chưa đủ điều kiện áp dụng chương trình này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SelectedPromoId = row.Field<int>("MaKhuyenMai");
            DialogResult = DialogResult.OK;
        }

        private void dgvPromos_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || dgvPromos.Columns[e.ColumnIndex].Name != "colDieuKien" || e.CellStyle == null) return;
            string text = e.Value?.ToString() ?? "";
            e.CellStyle.ForeColor = text.StartsWith("✔") ? Color.FromArgb(39, 174, 96) : Color.FromArgb(192, 57, 43);
        }
    }
}
