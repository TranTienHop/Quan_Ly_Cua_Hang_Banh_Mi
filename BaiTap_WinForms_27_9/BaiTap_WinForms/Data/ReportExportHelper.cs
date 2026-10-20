using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BaiTap_WinForms.Data
{
    /// <summary>
    /// Helper xuất file báo cáo (Excel .csv UTF-8 BOM, Báo cáo in ấn HTML / PDF)
    /// </summary>
    public static class ReportExportHelper
    {
        /// <summary>
        /// Xuất DataTable ra file CSV tương thích 100% Microsoft Excel tiếng Việt (UTF-8 with BOM)
        /// </summary>
        public static bool ExportToCsv(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis,
            out string savedPath)
        {
            savedPath = string.Empty;

            using var sfd = new SaveFileDialog
            {
                Filter = "Tệp Excel CSV (*.csv)|*.csv|Tất cả tệp (*.*)|*.*",
                FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}.csv",
                Title = "Lưu Báo Cáo Ra File Excel CSV"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return false;

            savedPath = sfd.FileName;

            try
            {
                var sb = new StringBuilder();

                // 1. Tiêu đề và thông tin chung
                sb.AppendLine($"\"{title.ToUpper()}\"");
                sb.AppendLine($"\"Thời gian:\",\"{EscapeCsv(period)}\"");
                sb.AppendLine($"\"Chi nhánh:\",\"{EscapeCsv(branchName)}\"");
                sb.AppendLine($"\"Thời điểm xuất:\",\"{DateTime.Now:dd/MM/yyyy HH:mm:ss}\"");
                sb.AppendLine();

                // 2. Chỉ số KPI tóm tắt (nếu có)
                if (summaryKpis != null && summaryKpis.Count > 0)
                {
                    sb.AppendLine("\"--- TÓM TẮT CHỈ SỐ QUAN TRỌNG ---\"");
                    foreach (var kvp in summaryKpis)
                    {
                        sb.AppendLine($"\"{EscapeCsv(kvp.Key)}\",\"{EscapeCsv(kvp.Value)}\"");
                    }
                    sb.AppendLine();
                }

                // 3. Tiêu đề các cột
                var colHeaders = new List<string>();
                foreach (DataColumn col in dataTable.Columns)
                {
                    colHeaders.Add($"\"{EscapeCsv(col.ColumnName)}\"");
                }
                sb.AppendLine(string.Join(",", colHeaders));

                // 4. Các dòng dữ liệu
                foreach (DataRow row in dataTable.Rows)
                {
                    var cellValues = new List<string>();
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        object val = row[col];
                        string cellStr = val == DBNull.Value ? "" : val.ToString() ?? "";

                        // Format số nếu là tiền hoặc số
                        if (val is decimal dec)
                        {
                            cellStr = dec.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
                        }
                        else if (val is double dbl)
                        {
                            cellStr = dbl.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
                        }

                        cellValues.Add($"\"{EscapeCsv(cellStr)}\"");
                    }
                    sb.AppendLine(string.Join(",", cellValues));
                }

                // Ghi ra file với UTF-8 BOM để Excel hiển thị đúng dấu tiếng Việt
                File.WriteAllText(savedPath, sb.ToString(), new UTF8Encoding(true));

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file CSV:\n" + ex.Message, "Lỗi xuất file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Xuất báo cáo ra định dạng HTML chuyên nghiệp (có thể in ấn hoặc lưu sang PDF dễ dàng)
        /// </summary>
        public static bool ExportToHtml(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis,
            out string savedPath)
        {
            savedPath = string.Empty;

            using var sfd = new SaveFileDialog
            {
                Filter = "Tài liệu HTML in ấn / PDF (*.html)|*.html|Tất cả tệp (*.*)|*.*",
                FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}.html",
                Title = "Lưu Báo Cáo Dạng HTML In Ấn / PDF"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return false;

            savedPath = sfd.FileName;

            try
            {
                var sb = new StringBuilder();

                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html lang=\"vi\">");
                sb.AppendLine("<head>");
                sb.AppendLine("    <meta charset=\"UTF-8\">");
                sb.AppendLine($"    <title>{title}</title>");
                sb.AppendLine("    <style>");
                sb.AppendLine("        body { font-family: 'Segoe UI', Arial, sans-serif; margin: 30px; color: #2c3e50; background: #f8f9fa; }");
                sb.AppendLine("        .report-card { background: #fff; padding: 30px; border-radius: 8px; box-shadow: 0 4px 15px rgba(0,0,0,0.06); max-width: 1000px; margin: auto; }");
                sb.AppendLine("        .header { border-bottom: 2px solid #e67e22; padding-bottom: 15px; margin-bottom: 20px; display: flex; justify-content: space-between; align-items: center; }");
                sb.AppendLine("        .shop-name { font-size: 20px; font-weight: bold; color: #d35400; }");
                sb.AppendLine("        .report-title { font-size: 22px; font-weight: bold; color: #2c3e50; margin-top: 10px; }");
                sb.AppendLine("        .meta-info { font-size: 13px; color: #7f8c8d; margin-top: 5px; }");
                sb.AppendLine("        .kpi-container { display: flex; gap: 15px; margin-bottom: 25px; flex-wrap: wrap; }");
                sb.AppendLine("        .kpi-card { flex: 1; min-width: 180px; background: #fdfefe; border: 1px solid #e1e8ed; border-left: 4px solid #3498db; padding: 12px 15px; border-radius: 5px; }");
                sb.AppendLine("        .kpi-card:nth-child(1) { border-left-color: #27ae60; }");
                sb.AppendLine("        .kpi-card:nth-child(2) { border-left-color: #2980b9; }");
                sb.AppendLine("        .kpi-card:nth-child(3) { border-left-color: #e67e22; }");
                sb.AppendLine("        .kpi-card:nth-child(4) { border-left-color: #8e44ad; }");
                sb.AppendLine("        .kpi-title { font-size: 11px; font-weight: bold; text-transform: uppercase; color: #95a5a6; }");
                sb.AppendLine("        .kpi-val { font-size: 18px; font-weight: bold; color: #2c3e50; margin-top: 5px; }");
                sb.AppendLine("        table { width: 100%; border-collapse: collapse; margin-top: 10px; font-size: 13.5px; }");
                sb.AppendLine("        th { background: #34495e; color: #fff; padding: 10px 12px; text-align: left; font-weight: 600; }");
                sb.AppendLine("        td { padding: 9px 12px; border-bottom: 1px solid #ecf0f1; }");
                sb.AppendLine("        tr:nth-child(even) { background: #fcfcfc; }");
                sb.AppendLine("        tr:hover { background: #f1f2f6; }");
                sb.AppendLine("        .signatures { display: flex; justify-content: space-between; margin-top: 50px; text-align: center; }");
                sb.AppendLine("        .sig-box { width: 200px; font-size: 14px; }");
                sb.AppendLine("        .sig-title { font-weight: bold; margin-bottom: 60px; }");
                sb.AppendLine("        @media print { body { background: #fff; margin: 0; } .report-card { box-shadow: none; padding: 0; } .no-print { display: none; } }");
                sb.AppendLine("    </style>");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
                sb.AppendLine("    <div class=\"report-card\">");

                // Header
                sb.AppendLine("        <div class=\"header\">");
                sb.AppendLine("            <div>");
                sb.AppendLine("                <div class=\"shop-name\">🥪 TIỆM BÁNH MÌ NGON</div>");
                sb.AppendLine($"                <div class=\"report-title\">{title.ToUpper()}</div>");
                sb.AppendLine($"                <div class=\"meta-info\">Khoảng thời gian: <b>{period}</b> | Chi nhánh: <b>{branchName}</b></div>");
                sb.AppendLine("            </div>");
                sb.AppendLine("            <div style=\"text-align: right;\">");
                sb.AppendLine($"                <div class=\"meta-info\">Ngày lập: {DateTime.Now:dd/MM/yyyy HH:mm}</div>");
                sb.AppendLine("                <button class=\"no-print\" onclick=\"window.print()\" style=\"margin-top: 8px; padding: 6px 14px; background: #e67e22; color: white; border: none; border-radius: 4px; cursor: pointer; font-weight: bold;\">🖨️ In Báo Cáo / Lưu PDF</button>");
                sb.AppendLine("            </div>");
                sb.AppendLine("        </div>");

                // KPIs
                if (summaryKpis != null && summaryKpis.Count > 0)
                {
                    sb.AppendLine("        <div class=\"kpi-container\">");
                    foreach (var kpi in summaryKpis)
                    {
                        sb.AppendLine("            <div class=\"kpi-card\">");
                        sb.AppendLine($"                <div class=\"kpi-title\">{kpi.Key}</div>");
                        sb.AppendLine($"                <div class=\"kpi-val\">{kpi.Value}</div>");
                        sb.AppendLine("            </div>");
                    }
                    sb.AppendLine("        </div>");
                }

                // Table
                sb.AppendLine("        <table>");
                sb.AppendLine("            <thead>");
                sb.AppendLine("                <tr>");
                foreach (DataColumn col in dataTable.Columns)
                {
                    sb.AppendLine($"                    <th>{col.ColumnName}</th>");
                }
                sb.AppendLine("                </tr>");
                sb.AppendLine("            </thead>");
                sb.AppendLine("            <tbody>");

                foreach (DataRow row in dataTable.Rows)
                {
                    sb.AppendLine("                <tr>");
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        object val = row[col];
                        string str = val == DBNull.Value ? "" : val.ToString() ?? "";
                        if (val is decimal dec)
                        {
                            str = dec.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
                        }
                        sb.AppendLine($"                    <td>{str}</td>");
                    }
                    sb.AppendLine("                </tr>");
                }

                sb.AppendLine("            </tbody>");
                sb.AppendLine("        </table>");

                // Chữ ký
                sb.AppendLine("        <div class=\"signatures\">");
                sb.AppendLine("            <div class=\"sig-box\">");
                sb.AppendLine("                <div class=\"sig-title\">Người Lập Biểu</div>");
                sb.AppendLine("                <div>(Ký và ghi rõ họ tên)</div>");
                sb.AppendLine("            </div>");
                sb.AppendLine("            <div class=\"sig-box\">");
                sb.AppendLine("                <div class=\"sig-title\">Kế Toán Trưởng</div>");
                sb.AppendLine("                <div>(Ký và ghi rõ họ tên)</div>");
                sb.AppendLine("            </div>");
                sb.AppendLine("            <div class=\"sig-box\">");
                sb.AppendLine("                <div class=\"sig-title\">Giám Đốc / Chủ Quán</div>");
                sb.AppendLine("                <div>(Ký và đóng dấu)</div>");
                sb.AppendLine("            </div>");
                sb.AppendLine("        </div>");

                sb.AppendLine("    </div>");
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");

                File.WriteAllText(savedPath, sb.ToString(), Encoding.UTF8);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xuất file HTML:\n" + ex.Message, "Lỗi xuất file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void PromptOpenFile(string filePath)
        {
            var res = MessageBox.Show(
                $"Xuất file thành công!\nĐường dẫn:\n{filePath}\n\nBạn có muốn mở file này ngay bây giờ không?",
                "Xuất file thành công",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (res == DialogResult.Yes)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể tự động mở file: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private static string EscapeCsv(string text)
        {
            return text.Replace("\"", "\"\"");
        }
    }
}
