using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Services;

namespace QuanLyCuaHangBanhMi.Forms
{
    /// <summary>
    /// Danh sách đơn đã bán trong ngày của chi nhánh, xem chi tiết món và in lại hóa đơn.
    /// </summary>
    public sealed partial class FormDonHomNay : Form
    {
        private readonly CultureInfo _vnCulture = new CultureInfo("vi-VN");
        private readonly int _maChiNhanh;

        public FormDonHomNay(int maChiNhanh, string tenChiNhanh)
        {
            _maChiNhanh = maChiNhanh;
            InitializeComponent();
            Text = "Đơn hàng trong ngày — " + tenChiNhanh;

            // AutoScaleMode.Dpi không co giãn chiều cao tiêu đề / dòng của DataGridView và thanh chia
            foreach (var dgv in new[] { dgvOrders, dgvDetails })
            {
                dgv.AutoGenerateColumns = false;
                dgv.ColumnHeadersHeight = S(dgv.ColumnHeadersHeight);
                dgv.RowTemplate.Height = S(dgv.RowTemplate.Height);
                foreach (DataGridViewColumn col in dgv.Columns)
                    if (!string.IsNullOrEmpty(col.DefaultCellStyle.Format))
                        col.DefaultCellStyle.FormatProvider = _vnCulture;
            }
            splOrders.SplitterWidth = S(splOrders.SplitterWidth);
        }

        private int S(int value) => (int)Math.Round(value * DeviceDpi / 96f);

        private void FormDonHomNay_Load(object? sender, EventArgs e)
        {
            splOrders.SplitterDistance = (int)(splOrders.Height * 0.62);
            LoadOrders();
        }

        private void dtpDate_ValueChanged(object? sender, EventArgs e) => LoadOrders();

        private void btnReload_Click(object? sender, EventArgs e) => LoadOrders();

        private void dgvOrders_SelectionChanged(object? sender, EventArgs e) => LoadDetails();

        private void dgvOrders_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) PrintSelected();
        }

        private void btnPrint_Click(object? sender, EventArgs e) => PrintSelected();

        private void btnClose_Click(object? sender, EventArgs e) => Close();

        private void LoadOrders()
        {
            try
            {
                var dt = OrderService.GetOrdersByDate(_maChiNhanh, dtpDate.Value.Date);

                dgvOrders.DataSource = dt;

                var done = dt.AsEnumerable().Where(r => r.Field<string>("TrangThaiDon") == "HoanThanh").ToList();
                decimal doanhThu = done.Sum(r => r.Field<decimal>("TongThanhToan") - r.Field<decimal>("PhiShip"));
                lblSummary.Text = $"{done.Count:N0} đơn hoàn thành  •  Doanh thu {doanhThu.ToString("N0", _vnCulture)} đ (không tính ship)";
                LoadDetails();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int? SelectedOrderId()
        {
            var row = (dgvOrders.CurrentRow?.DataBoundItem as DataRowView)?.Row;
            return row?.Field<int>("MaHoaDon");
        }

        private void LoadDetails()
        {
            int? maHoaDon = SelectedOrderId();
            btnPrint.Enabled = maHoaDon.HasValue;
            if (!maHoaDon.HasValue)
            {
                dgvDetails.DataSource = null;
                return;
            }

            try
            {
                dgvDetails.DataSource = OrderService.GetOrderDetails(maHoaDon.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintSelected()
        {
            int? maHoaDon = SelectedOrderId();
            if (maHoaDon.HasValue) ReceiptPrinter.ShowPreview(this, maHoaDon.Value);
        }

        private void dgvOrders_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value is not string code || e.CellStyle == null) return;
            switch (dgvOrders.Columns[e.ColumnIndex].Name)
            {
                case "colNguonDon":
                    e.Value = FormOrder.SourceText(code);
                    e.FormattingApplied = true;
                    break;
                case "colLoaiDon":
                    e.Value = FormOrder.OrderTypeText(code);
                    e.FormattingApplied = true;
                    break;
                case "colHinhThuc":
                    e.Value = string.Join(", ", code.Split(',', StringSplitOptions.TrimEntries).Select(FormOrder.PaymentText));
                    e.FormattingApplied = true;
                    break;
                case "colTrangThaiDon":
                    bool done = code == "HoanThanh";
                    e.Value = done ? "Hoàn thành" : code == "DaHuy" ? "Đã hủy" : code;
                    e.CellStyle.ForeColor = done ? Color.FromArgb(39, 174, 96) : Color.FromArgb(192, 57, 43);
                    e.FormattingApplied = true;
                    break;
            }
        }
    }
}
