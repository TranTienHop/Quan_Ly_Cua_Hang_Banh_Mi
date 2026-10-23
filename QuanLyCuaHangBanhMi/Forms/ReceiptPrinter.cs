using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Services;

namespace QuanLyCuaHangBanhMi.Forms
{
    /// <summary>
    /// In hóa đơn khổ 80mm (xem trước rồi mới in). Dữ liệu đọc lại từ DB theo MaHoaDon.
    /// </summary>
    public static class ReceiptPrinter
    {
        private const int PaperWidth = 315;   // 80mm, đơn vị 1/100 inch
        private const int Margin = 12;

        private static readonly CultureInfo VnCulture = new CultureInfo("vi-VN");

        public static void ShowPreview(IWin32Window owner, int maHoaDon, decimal? tienKhachDua = null, decimal? tienThoi = null)
        {
            DataTable header, lines, payments;
            try
            {
                (header, lines, payments) = ReceiptService.LoadForPrint(maHoaDon);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu hóa đơn để in: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (header.Rows.Count == 0)
            {
                MessageBox.Show("Không tìm thấy hóa đơn cần in.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataRow h = header.Rows[0];
            int noteCount = 0;
            foreach (DataRow r in lines.Rows)
                if (!r.IsNull("GhiChu")) noteCount++;
            int paperHeight = 520 + lines.Rows.Count * 34 + noteCount * 14 + payments.Rows.Count * 15;

            try
            {
                using var doc = new PrintDocument { DocumentName = "HoaDon_" + h["MaDonLocal"] };
                doc.DefaultPageSettings.PaperSize = new PaperSize("Bill 80mm", PaperWidth, paperHeight);
                doc.DefaultPageSettings.Margins = new Margins(Margin, Margin, Margin, Margin);
                doc.PrintPage += (s, e) =>
                {
                    if (e.Graphics != null) DrawReceipt(e.Graphics, h, lines, payments, tienKhachDua, tienThoi);
                };

                using var preview = new PrintPreviewDialog
                {
                    Document = doc,
                    Text = $"Xem trước hóa đơn {h["MaDonLocal"]}",
                    ShowIcon = false,
                    StartPosition = FormStartPosition.CenterParent,
                    Size = new Size(560, 780)
                };
                preview.PrintPreviewControl.Zoom = 1.0;
                preview.ShowDialog(owner);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không mở được bản in (máy chưa cài máy in?): " + ex.Message, "Lỗi in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void DrawReceipt(Graphics g, DataRow h, DataTable lines, DataTable payments, decimal? tienKhachDua, decimal? tienThoi)
        {
            using var fTitle = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            using var fBold = new Font("Segoe UI", 8F, FontStyle.Bold);
            using var fNormal = new Font("Segoe UI", 8F);
            using var fSmall = new Font("Segoe UI", 7F);
            using var fItalic = new Font("Segoe UI", 7.5F, FontStyle.Italic);
            using var fTotal = new Font("Segoe UI", 10F, FontStyle.Bold);
            using var pen = new Pen(Color.Black, 0.6f) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dash };

            float left = Margin, right = PaperWidth - Margin, width = right - left;
            float y = Margin;
            var center = new StringFormat { Alignment = StringAlignment.Center };
            var alignRight = new StringFormat { Alignment = StringAlignment.Far };

            void Center(string text, Font font)
            {
                var size = g.MeasureString(text, font, (int)width, center);
                g.DrawString(text, font, Brushes.Black, new RectangleF(left, y, width, size.Height), center);
                y += size.Height;
            }

            void Row(string label, string value, Font font)
            {
                var size = g.MeasureString(label, font, (int)(width * 0.62f));
                g.DrawString(label, font, Brushes.Black, new RectangleF(left, y, width * 0.62f, size.Height));
                g.DrawString(value, font, Brushes.Black, new RectangleF(left, y, width, size.Height), alignRight);
                y += Math.Max(size.Height, font.GetHeight(g));
            }

            void Line()
            {
                y += 3;
                g.DrawLine(pen, left, y, right, y);
                y += 4;
            }

            string Money(decimal v) => v.ToString("N0", VnCulture);

            Center(h["TenChiNhanh"].ToString()!.ToUpper(), fTitle);
            if (!h.IsNull("DiaChi")) Center(h["DiaChi"].ToString()!, fSmall);
            if (!h.IsNull("SdtChiNhanh")) Center("ĐT: " + h["SdtChiNhanh"], fSmall);
            y += 4;
            Center("HÓA ĐƠN BÁN HÀNG", fTotal);
            if (h.Field<string>("TrangThaiDon") == "DaHuy") Center("(ĐÃ HỦY)", fBold);
            y += 2;
            Row("Số: " + h["MaDonLocal"], Convert.ToDateTime(h["NgayTao"]).ToString("dd/MM/yyyy HH:mm"), fNormal);
            Row("Thu ngân: " + h["ThuNgan"], FormOrder.OrderTypeText(h.Field<string>("LoaiDon") ?? ""), fNormal);
            if (h.Field<string>("NguonDon") != "POS")
                Row("Nguồn đơn:", FormOrder.SourceText(h.Field<string>("NguonDon") ?? ""), fNormal);
            if (!h.IsNull("TenKhach"))
                Row("Khách: " + h["TenKhach"], h["SdtKhach"].ToString() ?? "", fNormal);
            if (!h.IsNull("DiaChiGiao"))
            {
                var size = g.MeasureString("Giao đến: " + h["DiaChiGiao"], fNormal, (int)width);
                g.DrawString("Giao đến: " + h["DiaChiGiao"], fNormal, Brushes.Black, new RectangleF(left, y, width, size.Height));
                y += size.Height;
            }
            Line();

            Row("Món", "Thành tiền", fBold);
            Line();
            foreach (DataRow r in lines.Rows)
            {
                string name = r["TenSanPhamChup"].ToString()!;
                var size = g.MeasureString(name, fBold, (int)width);
                g.DrawString(name, fBold, Brushes.Black, new RectangleF(left, y, width, size.Height));
                y += size.Height;
                Row($"   {Convert.ToDecimal(r["SoLuong"]):0.##} x {Money(Convert.ToDecimal(r["DonGia"]))}", Money(Convert.ToDecimal(r["ThanhTien"])), fNormal);
                if (!r.IsNull("GhiChu"))
                {
                    var noteSize = g.MeasureString("   * " + r["GhiChu"], fItalic, (int)width);
                    g.DrawString("   * " + r["GhiChu"], fItalic, Brushes.Black, new RectangleF(left, y, width, noteSize.Height));
                    y += noteSize.Height;
                }
                y += 2;
            }
            Line();

            Row("Tổng tiền hàng", Money(Convert.ToDecimal(h["TongTienHang"])), fNormal);
            decimal tienGiam = Convert.ToDecimal(h["TienGiam"]);
            if (tienGiam > 0)
            {
                Row("Giảm giá", "-" + Money(tienGiam), fNormal);
                if (!h.IsNull("TenKhuyenMai")) Row("   KM: " + h["TenKhuyenMai"], "", fItalic);
                if (Convert.ToInt32(h["DiemTru"]) > 0) Row($"   Đổi {Convert.ToInt32(h["DiemTru"]):N0} điểm", "", fItalic);
            }
            decimal phiShip = Convert.ToDecimal(h["PhiShip"]);
            if (phiShip > 0) Row("Phí ship", Money(phiShip), fNormal);
            y += 2;
            Row("TỔNG THANH TOÁN", Money(Convert.ToDecimal(h["TongThanhToan"])) + " đ", fTotal);
            y += 2;

            foreach (DataRow p in payments.Rows)
                Row(FormOrder.PaymentText(p.Field<string>("HinhThuc") ?? ""), Money(Convert.ToDecimal(p["SoTien"])), fNormal);
            if (tienKhachDua.HasValue && tienKhachDua.Value > 0)
            {
                Row("Khách đưa", Money(tienKhachDua.Value), fNormal);
                Row("Tiền thừa", Money(tienThoi ?? 0), fNormal);
            }
            if (!h.IsNull("TenKhach"))
                Row("Điểm tích lũy", $"+{Convert.ToInt32(h["DiemCong"]):N0} / -{Convert.ToInt32(h["DiemTru"]):N0}", fNormal);

            Line();
            Center("Cảm ơn quý khách! Hẹn gặp lại ♥", fItalic);
        }
    }
}
