using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services.Reporting
{
    /// <summary>
    /// Helper hỗ trợ Print Preview (Xem trước bản in) và In thực tế qua Windows Print System (PrintDocument)
    /// </summary>
    public static class ReportPrintHelper
    {
        public static void ShowPrintPreview(
            IWin32Window owner,
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis)
        {
            var printDoc = CreatePrintDocument(dataTable, title, period, branchName, summaryKpis);

            using var previewDlg = new PrintPreviewDialog
            {
                Document = printDoc,
                Width = 980,
                Height = 720,
                StartPosition = FormStartPosition.CenterParent,
                Text = $"Xem Trước Bản In — {title}"
            };

            // Tối ưu phóng to 100% khi mở
            if (previewDlg.Controls.Count > 0 && previewDlg.Controls[0] is PrintPreviewControl ppc)
            {
                ppc.Zoom = 1.0;
            }

            previewDlg.ShowDialog(owner);
        }

        public static void ShowPrintDialog(
            IWin32Window owner,
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis)
        {
            var printDoc = CreatePrintDocument(dataTable, title, period, branchName, summaryKpis);

            using var printDlg = new PrintDialog
            {
                Document = printDoc,
                UseEXDialog = true,
                AllowSomePages = true
            };

            if (printDlg.ShowDialog(owner) == DialogResult.OK)
            {
                try
                {
                    printDoc.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi gửi lệnh in: " + ex.Message, "Lỗi in ấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private static PrintDocument CreatePrintDocument(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis)
        {
            var doc = new PrintDocument();
            doc.DocumentName = $"{title}_{DateTime.Now:yyyyMMdd_HHmm}";

            bool isLandscape = dataTable.Columns.Count > 5;
            doc.DefaultPageSettings.Landscape = isLandscape;
            doc.DefaultPageSettings.Margins = new Margins(35, 35, 35, 40);

            int currentRowIndex = 0;
            int pageNumber = 0;

            doc.BeginPrint += (s, e) =>
            {
                currentRowIndex = 0;
                pageNumber = 0;
            };

            doc.PrintPage += (s, e) =>
            {
                pageNumber++;
                Graphics g = e.Graphics!;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                Rectangle bounds = e.MarginBounds;
                float currentY = bounds.Top;

                using var fontTitle = new Font("Segoe UI", 14F, FontStyle.Bold);
                using var fontSub = new Font("Segoe UI", 9F, FontStyle.Regular);
                using var fontHeader = new Font("Segoe UI", 9F, FontStyle.Bold);
                using var fontCell = new Font("Segoe UI", 8.5F, FontStyle.Regular);
                using var fontCellBold = new Font("Segoe UI", 8.5F, FontStyle.Bold);
                using var fontFooter = new Font("Segoe UI", 8F, FontStyle.Italic);

                using var brushTitle = new SolidBrush(Color.FromArgb(44, 62, 80));
                using var brushText = new SolidBrush(Color.FromArgb(40, 40, 40));
                using var brushHeaderBg = new SolidBrush(Color.FromArgb(44, 62, 80));
                using var brushHeaderWhite = new SolidBrush(Color.White);
                using var brushAltRow = new SolidBrush(Color.FromArgb(248, 249, 250));
                using var brushKpiBg = new SolidBrush(Color.FromArgb(240, 244, 248));
                using var penGrid = new Pen(Color.FromArgb(215, 220, 225), 1);
                using var penBorder = new Pen(Color.FromArgb(44, 62, 80), 1.2F);

                int colCount = dataTable.Columns.Count;
                float[] colWidths = new float[colCount];

                // Ước lượng tỷ lệ các cột
                float totalWeight = 0;
                float[] weights = new float[colCount];
                for (int c = 0; c < colCount; c++)
                {
                    string colName = dataTable.Columns[c].ColumnName;
                    float w = Math.Max(colName.Length, 8);
                    if (colName.Contains("Tên") || colName.Contains("Địa chỉ") || colName.Contains("Ghi chú"))
                        w = Math.Max(w, 24);
                    else if (colName.Contains("Mã") || colName.Contains("ĐVT") || colName.Contains("Hạng"))
                        w = Math.Min(w, 10);
                    weights[c] = w;
                    totalWeight += w;
                }

                for (int c = 0; c < colCount; c++)
                {
                    colWidths[c] = (weights[c] / totalWeight) * bounds.Width;
                }

                // 1. Header báo cáo
                if (pageNumber == 1)
                {
                    using (var brandBrush = new SolidBrush(Color.FromArgb(230, 126, 34)))
                    {
                        g.FillRectangle(brandBrush, bounds.Left, currentY, bounds.Width, 4);
                    }
                    currentY += 10;

                    g.DrawString(title.ToUpperInvariant(), fontTitle, brushTitle, bounds.Left, currentY);
                    currentY += 26;

                    string info1 = $"Thời gian: {period}   |   Chi nhánh: {branchName}";
                    string info2 = $"Ngày in: {DateTime.Now:dd/MM/yyyy HH:mm:ss}   |   Người lập: {(string.IsNullOrEmpty(UserSession.FullName) ? "Quản trị viên" : UserSession.FullName)}";
                    g.DrawString(info1, fontSub, brushText, bounds.Left, currentY);
                    currentY += 15;
                    g.DrawString(info2, fontSub, brushText, bounds.Left, currentY);
                    currentY += 18;

                    // Khối KPI
                    if (summaryKpis != null && summaryKpis.Count > 0)
                    {
                        float kpiBoxY = currentY;
                        float kpiBoxHeight = 16 + ((float)Math.Ceiling(summaryKpis.Count / 2.0) * 16);
                        g.FillRectangle(brushKpiBg, bounds.Left, kpiBoxY, bounds.Width, kpiBoxHeight);
                        g.DrawRectangle(penGrid, bounds.Left, kpiBoxY, bounds.Width, kpiBoxHeight);

                        g.DrawString("CHỈ SỐ BÁO CÁO:", fontHeader, brushTitle, bounds.Left + 10, kpiBoxY + 3);

                        int kIdx = 0;
                        float kpiColW = bounds.Width / 2.0F;
                        foreach (var kvp in summaryKpis)
                        {
                            int row = kIdx / 2;
                            int col = kIdx % 2;
                            float kX = bounds.Left + 10 + (col * kpiColW);
                            float kY = kpiBoxY + 20 + (row * 15);

                            g.DrawString($"• {kvp.Key}: {kvp.Value}", fontCellBold, brushText, kX, kY);
                            kIdx++;
                        }

                        currentY += kpiBoxHeight + 14;
                    }
                }
                else
                {
                    g.DrawString($"{title} ({period})", fontSub, brushTitle, bounds.Left, currentY);
                    currentY += 16;
                    g.DrawLine(penGrid, bounds.Left, currentY, bounds.Right, currentY);
                    currentY += 10;
                }

                // 2. Header bảng
                float headerH = 24;
                g.FillRectangle(brushHeaderBg, bounds.Left, currentY, bounds.Width, headerH);
                g.DrawRectangle(penBorder, bounds.Left, currentY, bounds.Width, headerH);

                float curX = bounds.Left;
                using var centerFormat = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                for (int c = 0; c < colCount; c++)
                {
                    var rect = new RectangleF(curX, currentY, colWidths[c], headerH);
                    g.DrawString(dataTable.Columns[c].ColumnName, fontHeader, brushHeaderWhite, rect, centerFormat);
                    curX += colWidths[c];
                    if (c < colCount - 1)
                        g.DrawLine(penGrid, curX, currentY, curX, currentY + headerH);
                }
                currentY += headerH;

                // 3. Dòng dữ liệu
                float rowH = 20;
                using var leftFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
                using var rightFormat = new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Center };

                while (currentRowIndex < dataTable.Rows.Count)
                {
                    if (currentY + rowH > bounds.Bottom - 30)
                    {
                        // Còn dòng -> chuyển trang tiếp theo
                        e.HasMorePages = true;
                        DrawPageFooter(g, bounds, fontFooter, penGrid, pageNumber);
                        return;
                    }

                    DataRow row = dataTable.Rows[currentRowIndex];
                    bool isEven = (currentRowIndex % 2 == 0);

                    if (!isEven)
                    {
                        g.FillRectangle(brushAltRow, bounds.Left, currentY, bounds.Width, rowH);
                    }
                    g.DrawRectangle(penGrid, bounds.Left, currentY, bounds.Width, rowH);

                    curX = bounds.Left;
                    for (int c = 0; c < colCount; c++)
                    {
                        object val = row[c];
                        string text = "";
                        StringFormat fmt = leftFormat;

                        if (val != null && val != DBNull.Value)
                        {
                            if (val is decimal dec)
                            {
                                text = dec.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
                                fmt = rightFormat;
                            }
                            else if (val is double dbl)
                            {
                                text = dbl.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
                                fmt = rightFormat;
                            }
                            else if (val is int || val is long)
                            {
                                text = string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0}", val);
                                fmt = rightFormat;
                            }
                            else if (val is DateTime dt)
                            {
                                text = dt.ToString("dd/MM/yyyy");
                                fmt = centerFormat;
                            }
                            else
                            {
                                text = val.ToString() ?? "";
                                string colName = dataTable.Columns[c].ColumnName.ToLowerInvariant();
                                if (colName.Contains("tiền") || colName.Contains("thu") || colName.Contains("giá") || colName.Contains("giảm"))
                                    fmt = rightFormat;
                                else if (colName.Contains("mã") || colName.Contains("đvt") || colName.Contains("hạng"))
                                    fmt = centerFormat;
                            }
                        }

                        var cellRect = new RectangleF(curX + 4, currentY, colWidths[c] - 8, rowH);
                        g.DrawString(text, fontCell, brushText, cellRect, fmt);

                        curX += colWidths[c];
                        if (c < colCount - 1)
                            g.DrawLine(penGrid, curX, currentY, curX, currentY + rowH);
                    }

                    currentY += rowH;
                    currentRowIndex++;
                }

                // Vẽ trang cuối cùng
                e.HasMorePages = false;
                DrawPageFooter(g, bounds, fontFooter, penGrid, pageNumber);
            };

            return doc;
        }

        private static void DrawPageFooter(Graphics g, Rectangle bounds, Font fontFooter, Pen penGrid, int pageNumber)
        {
            using var brushFooter = new SolidBrush(Color.FromArgb(127, 140, 141));
            g.DrawLine(penGrid, bounds.Left, bounds.Bottom - 18, bounds.Right, bounds.Bottom - 18);
            g.DrawString("Tiệm Bánh Mì — Hệ Thống Quản Lý Bán Hàng", fontFooter, brushFooter, bounds.Left, bounds.Bottom - 14);

            string pageText = $"Trang {pageNumber}";
            using var rightFmt = new StringFormat { Alignment = StringAlignment.Far };
            g.DrawString(pageText, fontFooter, brushFooter, bounds.Right, bounds.Bottom - 14, rightFmt);
        }
    }
}
