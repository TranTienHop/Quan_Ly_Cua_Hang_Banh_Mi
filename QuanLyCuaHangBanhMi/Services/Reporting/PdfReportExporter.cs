using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services.Reporting
{
    /// <summary>
    /// Xuất báo cáo ra định dạng PDF (.pdf) thực tế chuẩn Vector bằng PDFsharp.
    /// Tự động phân trang (multi-page), vẽ header/footer, bảng kẻ ô, định dạng số và tiếng Việt Unicode.
    /// </summary>
    public static class PdfReportExporter
    {
        public static async Task ExportAsync(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis,
            string filePath,
            IProgress<int>? progress = null,
            IProgress<string>? statusProgress = null)
        {
            await Task.Run(() =>
            {
                statusProgress?.Report("Khởi tạo cấu trúc tài liệu PDF...");
                progress?.Report(15);

                WindowsFontResolver.EnsureRegistered();

                using var document = new PdfDocument();
                document.Info.Title = title;
                document.Info.Author = string.IsNullOrEmpty(UserSession.FullName) ? "Hệ Thống Quản Lý Bánh Mì" : UserSession.FullName;
                document.Info.Subject = $"Báo cáo {title} - {period}";

                // Chọn hướng trang: Nếu nhiều hơn 5 cột thì Landscape, ngược lại Portrait
                bool isLandscape = dataTable.Columns.Count > 5;
                PageOrientation orientation = isLandscape ? PageOrientation.Landscape : PageOrientation.Portrait;

                var fontTitle = new XFont("Arial", 15, XFontStyleEx.Bold);
                var fontSub = new XFont("Arial", 9, XFontStyleEx.Regular);
                var fontHeader = new XFont("Arial", 9, XFontStyleEx.Bold);
                var fontCell = new XFont("Arial", 8.5, XFontStyleEx.Regular);
                var fontCellBold = new XFont("Arial", 8.5, XFontStyleEx.Bold);
                var fontFooter = new XFont("Arial", 8, XFontStyleEx.Italic);

                var brushTitle = new XSolidBrush(XColor.FromArgb(44, 62, 80));
                var brushText = new XSolidBrush(XColor.FromArgb(50, 50, 50));
                var brushHeaderBg = new XSolidBrush(XColor.FromArgb(44, 62, 80));
                var brushHeaderWhite = new XSolidBrush(XColor.FromArgb(255, 255, 255));
                var brushAltRow = new XSolidBrush(XColor.FromArgb(248, 249, 250));
                var brushKpiBg = new XSolidBrush(XColor.FromArgb(240, 244, 248));
                var penGrid = new XPen(XColor.FromArgb(215, 220, 225), 0.7);
                var penBorder = new XPen(XColor.FromArgb(44, 62, 80), 1.2);

                double marginLeft = 35;
                double marginRight = 35;
                double marginTop = 35;
                double marginBottom = 40;

                // Chuẩn bị tính toán độ rộng các cột
                int colCount = dataTable.Columns.Count;
                double[] colWidths = new double[colCount];

                // Ước lượng tỷ trọng độ rộng theo tên cột
                double totalWeight = 0;
                double[] weights = new double[colCount];
                for (int c = 0; c < colCount; c++)
                {
                    string colName = dataTable.Columns[c].ColumnName;
                    double w = Math.Max(colName.Length, 8);
                    if (colName.Contains("Tên") || colName.Contains("Địa chỉ") || colName.Contains("Ghi chú"))
                        w = Math.Max(w, 24);
                    else if (colName.Contains("Mã") || colName.Contains("ĐVT") || colName.Contains("Hạng"))
                        w = Math.Min(w, 10);
                    weights[c] = w;
                    totalWeight += w;
                }

                // Danh sách các trang đã tạo để vẽ footer số trang sau cùng
                var pages = new List<PdfPage>();

                PdfPage currentPage = document.AddPage();
                currentPage.Size = PageSize.A4;
                currentPage.Orientation = orientation;
                pages.Add(currentPage);

                XGraphics gfx = XGraphics.FromPdfPage(currentPage);

                double pageWidth = currentPage.Width.Point;
                double pageHeight = currentPage.Height.Point;
                double printableWidth = pageWidth - marginLeft - marginRight;

                for (int c = 0; c < colCount; c++)
                {
                    colWidths[c] = Math.Round((weights[c] / totalWeight) * printableWidth, 1);
                }

                double currentY = marginTop;

                // Hàm vẽ Header báo cáo (chỉ trên trang đầu hoặc trang tiếp theo)
                void DrawReportHeader(bool isFirstPage)
                {
                    if (isFirstPage)
                    {
                        // Thanh thương hiệu
                        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(230, 126, 34)), marginLeft, currentY, printableWidth, 4);
                        currentY += 12;

                        // Tiêu đề
                        gfx.DrawString(title.ToUpperInvariant(), fontTitle, brushTitle, new XPoint(marginLeft, currentY + 12));
                        currentY += 24;

                        // Thông tin phụ
                        string info1 = $"Thời gian: {period}   |   Chi nhánh: {branchName}";
                        string info2 = $"Ngày xuất: {DateTime.Now:dd/MM/yyyy HH:mm:ss}   |   Người lập: {(string.IsNullOrEmpty(UserSession.FullName) ? "Quản trị viên" : UserSession.FullName)}";
                        gfx.DrawString(info1, fontSub, brushText, new XPoint(marginLeft, currentY));
                        currentY += 14;
                        gfx.DrawString(info2, fontSub, brushText, new XPoint(marginLeft, currentY));
                        currentY += 18;

                        // Khối KPI nếu có
                        if (summaryKpis != null && summaryKpis.Count > 0)
                        {
                            statusProgress?.Report("Đang vẽ bảng chỉ số tổng quan...");
                            progress?.Report(30);

                            double kpiBoxY = currentY;
                            double kpiBoxHeight = 16 + (Math.Ceiling(summaryKpis.Count / 2.0) * 16);
                            gfx.DrawRectangle(penGrid, brushKpiBg, marginLeft, kpiBoxY, printableWidth, kpiBoxHeight);

                            gfx.DrawString("CHỈ SỐ BÁO CÁO:", fontHeader, brushTitle, new XPoint(marginLeft + 10, kpiBoxY + 12));

                            int kIdx = 0;
                            double kpiColW = printableWidth / 2.0;
                            foreach (var kvp in summaryKpis)
                            {
                                int row = kIdx / 2;
                                int col = kIdx % 2;
                                double kX = marginLeft + 10 + (col * kpiColW);
                                double kY = kpiBoxY + 26 + (row * 15);

                                string kpiText = $"• {kvp.Key}: {kvp.Value}";
                                gfx.DrawString(kpiText, fontCellBold, brushText, new XPoint(kX, kY));
                                kIdx++;
                            }

                            currentY += kpiBoxHeight + 14;
                        }
                    }
                    else
                    {
                        // Header thu gọn cho các trang 2, 3...
                        gfx.DrawString($"{title} ({period})", fontSub, brushTitle, new XPoint(marginLeft, currentY));
                        currentY += 14;
                        gfx.DrawLine(penGrid, marginLeft, currentY, marginLeft + printableWidth, currentY);
                        currentY += 10;
                    }
                }

                // Hàm vẽ Header của bảng
                void DrawTableHeader()
                {
                    double rowH = 22;
                    gfx.DrawRectangle(penBorder, brushHeaderBg, marginLeft, currentY, printableWidth, rowH);

                    double curX = marginLeft;
                    for (int c = 0; c < colCount; c++)
                    {
                        var rect = new XRect(curX + 3, currentY + 4, colWidths[c] - 6, rowH - 8);
                        var format = new XStringFormat
                        {
                            Alignment = XStringAlignment.Center,
                            LineAlignment = XLineAlignment.Center
                        };
                        gfx.DrawString(dataTable.Columns[c].ColumnName, fontHeader, brushHeaderWhite, rect, format);
                        curX += colWidths[c];
                        if (c < colCount - 1)
                        {
                            gfx.DrawLine(penGrid, curX, currentY, curX, currentY + rowH);
                        }
                    }
                    currentY += rowH;
                }

                // Bắt đầu vẽ trang 1
                DrawReportHeader(true);
                DrawTableHeader();

                statusProgress?.Report("Đang vẽ dữ liệu các dòng...");
                progress?.Report(40);

                // Vẽ các dòng dữ liệu
                double defaultRowH = 19;
                int totalRows = dataTable.Rows.Count;

                for (int r = 0; r < totalRows; r++)
                {
                    // Kiểm tra trang đầy
                    if (currentY + defaultRowH > pageHeight - marginBottom)
                    {
                        gfx.Dispose();

                        currentPage = document.AddPage();
                        currentPage.Size = PageSize.A4;
                        currentPage.Orientation = orientation;
                        pages.Add(currentPage);

                        gfx = XGraphics.FromPdfPage(currentPage);
                        currentY = marginTop;

                        DrawReportHeader(false);
                        DrawTableHeader();
                    }

                    DataRow row = dataTable.Rows[r];
                    bool isEven = (r % 2 == 0);

                    if (!isEven)
                    {
                        gfx.DrawRectangle(brushAltRow, marginLeft, currentY, printableWidth, defaultRowH);
                    }

                    gfx.DrawRectangle(penGrid, marginLeft, currentY, printableWidth, defaultRowH);

                    double curX = marginLeft;
                    for (int c = 0; c < colCount; c++)
                    {
                        object val = row[c];
                        string text = "";
                        XStringAlignment align = XStringAlignment.Near;

                        if (val != null && val != DBNull.Value)
                        {
                            if (val is decimal dec)
                            {
                                text = dec.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
                                align = XStringAlignment.Far;
                            }
                            else if (val is double dbl)
                            {
                                text = dbl.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
                                align = XStringAlignment.Far;
                            }
                            else if (val is int || val is long)
                            {
                                text = string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:N0}", val);
                                align = XStringAlignment.Far;
                            }
                            else if (val is DateTime dt)
                            {
                                text = dt.ToString("dd/MM/yyyy");
                                align = XStringAlignment.Center;
                            }
                            else
                            {
                                text = val.ToString() ?? "";
                                string colName = dataTable.Columns[c].ColumnName.ToLowerInvariant();
                                if (colName.Contains("tiền") || colName.Contains("thu") || colName.Contains("giá") || colName.Contains("giảm"))
                                    align = XStringAlignment.Far;
                                else if (colName.Contains("mã") || colName.Contains("đvt") || colName.Contains("hạng"))
                                    align = XStringAlignment.Center;
                            }
                        }

                        var cellRect = new XRect(curX + 4, currentY + 3, colWidths[c] - 8, defaultRowH - 6);
                        var format = new XStringFormat
                        {
                            Alignment = align,
                            LineAlignment = XLineAlignment.Center
                        };

                        // Cắt ngắn nếu tràn ô
                        gfx.DrawString(text, fontCell, brushText, cellRect, format);

                        curX += colWidths[c];
                        if (c < colCount - 1)
                        {
                            gfx.DrawLine(penGrid, curX, currentY, curX, currentY + defaultRowH);
                        }
                    }

                    currentY += defaultRowH;

                    if (r % 15 == 0 || r == totalRows - 1)
                    {
                        int currentPct = 40 + (int)((r + 1.0) / Math.Max(totalRows, 1) * 45);
                        progress?.Report(currentPct);
                    }
                }

                gfx.Dispose();

                // Đánh số trang footer cho toàn bộ tài liệu
                statusProgress?.Report("Đang hoàn thiện đánh số trang...");
                progress?.Report(90);

                int totalPages = document.Pages.Count;
                for (int i = 0; i < totalPages; i++)
                {
                    using var fGfx = XGraphics.FromPdfPage(document.Pages[i]);
                    double pW = document.Pages[i].Width.Point;
                    double pH = document.Pages[i].Height.Point;

                    fGfx.DrawLine(penGrid, marginLeft, pH - 28, pW - marginRight, pH - 28);
                    fGfx.DrawString("Tiệm Bánh Mì — Hệ Thống Quản Lý Bán Hàng", fontFooter, new XSolidBrush(XColor.FromArgb(127, 140, 141)), new XPoint(marginLeft, pH - 15));

                    string pageStr = $"Trang {i + 1} / {totalPages}";
                    var pRect = new XRect(pW - marginRight - 120, pH - 22, 120, 15);
                    var pFormat = new XStringFormat { Alignment = XStringAlignment.Far, LineAlignment = XLineAlignment.Center };
                    fGfx.DrawString(pageStr, fontFooter, new XSolidBrush(XColor.FromArgb(127, 140, 141)), pRect, pFormat);
                }

                // Lưu tài liệu
                statusProgress?.Report("Đang lưu tệp PDF vào đĩa...");
                progress?.Report(95);

                document.Save(filePath);

                statusProgress?.Report("Xuất file PDF thành công!");
                progress?.Report(100);
            });
        }
    }
}
