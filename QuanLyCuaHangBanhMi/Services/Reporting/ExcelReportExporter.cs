using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using ClosedXML.Excel;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services.Reporting
{
    /// <summary>
    /// Xuất báo cáo ra định dạng Excel (.xlsx) thực tế bằng thư viện ClosedXML
    /// Hỗ trợ chạy bất đồng bộ nền và cập nhật tiến trình qua IProgress.
    /// </summary>
    public static class ExcelReportExporter
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
                statusProgress?.Report("Đang chuẩn bị dữ liệu xuất Excel...");
                progress?.Report(15);

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Báo Cáo");

                worksheet.ShowGridLines = true;

                int colCount = Math.Max(dataTable.Columns.Count, 4);

                // 1. Tiêu đề chính
                var titleRange = worksheet.Range(2, 2, 2, colCount + 1);
                titleRange.Merge();
                titleRange.Value = title.ToUpperInvariant();
                titleRange.Style.Font.Bold = true;
                titleRange.Style.Font.FontSize = 16;
                titleRange.Style.Font.FontColor = XLColor.White;
                titleRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#2C3E50");
                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Row(2).Height = 36;

                // 2. Thông tin phạm vi
                int currentRow = 4;
                worksheet.Cell(currentRow, 2).Value = "Khoảng thời gian:";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Value = period;

                currentRow++;
                worksheet.Cell(currentRow, 2).Value = "Chi nhánh:";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Value = branchName;

                currentRow++;
                worksheet.Cell(currentRow, 2).Value = "Ngày xuất:";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Value = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

                currentRow++;
                worksheet.Cell(currentRow, 2).Value = "Người lập báo cáo:";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 3).Value = string.IsNullOrEmpty(UserSession.FullName) ? "Quản trị viên" : $"{UserSession.FullName} ({UserSession.RoleName})";

                currentRow += 2;

                // 3. Khối KPI tóm tắt
                if (summaryKpis != null && summaryKpis.Count > 0)
                {
                    statusProgress?.Report("Đang định dạng các chỉ số KPI...");
                    progress?.Report(30);

                    var kpiTitle = worksheet.Range(currentRow, 2, currentRow, colCount + 1);
                    kpiTitle.Merge();
                    kpiTitle.Value = "TỔNG HỢP CHỈ SỐ QUAN TRỌNG";
                    kpiTitle.Style.Font.Bold = true;
                    kpiTitle.Style.Font.FontSize = 11;
                    kpiTitle.Style.Fill.BackgroundColor = XLColor.FromHtml("#EAECEE");
                    kpiTitle.Style.Font.FontColor = XLColor.FromHtml("#2C3E50");
                    worksheet.Row(currentRow).Height = 24;
                    currentRow++;

                    foreach (var kvp in summaryKpis)
                    {
                        worksheet.Cell(currentRow, 2).Value = kvp.Key;
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#F8F9FA");

                        worksheet.Cell(currentRow, 3).Value = kvp.Value;
                        worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Style.Font.FontColor = XLColor.FromHtml("#27AE60");

                        var kpiRange = worksheet.Range(currentRow, 2, currentRow, 3);
                        kpiRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        kpiRange.Style.Border.OutsideBorderColor = XLColor.FromHtml("#BDC3C7");
                        currentRow++;
                    }
                    currentRow++;
                }

                // 4. Bảng dữ liệu chính
                statusProgress?.Report("Đang ghi dữ liệu báo cáo vào Excel...");
                progress?.Report(45);

                int tableStartRow = currentRow;

                // Header cột
                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    var cell = worksheet.Cell(tableStartRow, c + 2);
                    cell.Value = dataTable.Columns[c].ColumnName;
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#34495E");
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    cell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#2C3E50");
                }
                worksheet.Row(tableStartRow).Height = 28;
                currentRow++;

                // Dòng dữ liệu
                int totalRows = dataTable.Rows.Count;
                for (int r = 0; r < totalRows; r++)
                {
                    DataRow row = dataTable.Rows[r];
                    bool isEven = (r % 2 == 0);

                    for (int c = 0; c < dataTable.Columns.Count; c++)
                    {
                        var cell = worksheet.Cell(currentRow, c + 2);
                        object val = row[c];

                        if (val == DBNull.Value || val == null)
                        {
                            cell.Value = "";
                        }
                        else if (val is decimal decVal)
                        {
                            cell.Value = (double)decVal;
                            cell.Style.NumberFormat.Format = "#,##0";
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        }
                        else if (val is double dblVal)
                        {
                            cell.Value = dblVal;
                            cell.Style.NumberFormat.Format = "#,##0";
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        }
                        else if (val is int intVal)
                        {
                            cell.Value = intVal;
                            cell.Style.NumberFormat.Format = "#,##0";
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        }
                        else if (val is long longVal)
                        {
                            cell.Value = longVal;
                            cell.Style.NumberFormat.Format = "#,##0";
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                        }
                        else if (val is DateTime dtVal)
                        {
                            cell.Value = dtVal;
                            cell.Style.DateFormat.Format = "dd/MM/yyyy";
                            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        }
                        else
                        {
                            string str = val.ToString() ?? "";
                            // Thử parse số nếu cột có tên tiền hoặc số lượng
                            string colName = dataTable.Columns[c].ColumnName.ToLowerInvariant();
                            if ((colName.Contains("tiền") || colName.Contains("thu") || colName.Contains("giảm") || colName.Contains("giá") || colName.Contains("số lượng"))
                                && decimal.TryParse(str.Replace(",", "").Replace(".", "").Replace("đ", "").Trim(), out decimal parsedNum))
                            {
                                cell.Value = (double)parsedNum;
                                cell.Style.NumberFormat.Format = "#,##0";
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            }
                            else
                            {
                                cell.Value = str;
                                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            }
                        }

                        if (!isEven)
                        {
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#F8F9FA");
                        }

                        cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        cell.Style.Border.OutsideBorderColor = XLColor.FromHtml("#E0E0E0");
                    }

                    currentRow++;

                    if (r % 10 == 0 || r == totalRows - 1)
                    {
                        int currentPct = 45 + (int)((r + 1.0) / Math.Max(totalRows, 1) * 35);
                        progress?.Report(currentPct);
                    }
                }

                // 5. Tự động chỉnh độ rộng cột
                statusProgress?.Report("Đang tự động căn chỉnh kích thước cột...");
                progress?.Report(85);

                for (int c = 0; c < dataTable.Columns.Count; c++)
                {
                    worksheet.Column(c + 2).AdjustToContents(tableStartRow, currentRow, 10, 45);
                }
                worksheet.Column(1).Width = 3;

                // 6. Lưu file
                statusProgress?.Report("Đang lưu tệp Excel vào đĩa...");
                progress?.Report(95);

                workbook.SaveAs(filePath);

                statusProgress?.Report("Xuất file Excel thành công!");
                progress?.Report(100);
            });
        }
    }
}
