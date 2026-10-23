using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuanLyCuaHangBanhMi.Services.Reporting
{
    public sealed class HtmlReportExporter : ReportExporterBase
    {
        public override string FormatDisplayName => "HTML / PDF";
        public override string FileFilter => "Tài liệu HTML in ấn / PDF (*.html)|*.html|Tất cả tệp (*.*)|*.*";
        public override string DefaultExtension => ".html";

        public override string BuildFileContent(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis)
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
            sb.AppendLine("        <div class=\"header\">");
            sb.AppendLine("            <div>");
            sb.AppendLine("                <div class=\"shop-name\">🥪 TIỆM BÁNH MÌ NGON</div>");
            sb.AppendLine($"                <div class=\"report-title\">{title.ToUpperInvariant()}</div>");
            sb.AppendLine($"                <div class=\"meta-info\">Khoảng thời gian: <b>{period}</b> | Chi nhánh: <b>{branchName}</b></div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("            <div style=\"text-align: right;\">");
            sb.AppendLine($"                <div class=\"meta-info\">Ngày lập: {DateTime.Now:dd/MM/yyyy HH:mm}</div>");
            sb.AppendLine("                <button class=\"no-print\" onclick=\"window.print()\" style=\"margin-top: 8px; padding: 6px 14px; background: #e67e22; color: white; border: none; border-radius: 4px; cursor: pointer; font-weight: bold;\">🖨️ In Báo Cáo / Lưu PDF</button>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </div>");

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

            sb.AppendLine("        <table>");
            sb.AppendLine("            <thead><tr>");
            foreach (DataColumn col in dataTable.Columns)
                sb.AppendLine($"                <th>{col.ColumnName}</th>");
            sb.AppendLine("            </tr></thead><tbody>");

            foreach (DataRow row in dataTable.Rows)
            {
                sb.AppendLine("                <tr>");
                foreach (DataColumn col in dataTable.Columns)
                    sb.AppendLine($"                    <td>{FormatCell(row[col])}</td>");
                sb.AppendLine("                </tr>");
            }

            sb.AppendLine("            </tbody></table>");
            sb.AppendLine("        <div class=\"signatures\">");
            sb.AppendLine("            <div class=\"sig-box\"><div class=\"sig-title\">Người Lập Biểu</div><div>(Ký và ghi rõ họ tên)</div></div>");
            sb.AppendLine("            <div class=\"sig-box\"><div class=\"sig-title\">Kế Toán Trưởng</div><div>(Ký và ghi rõ họ tên)</div></div>");
            sb.AppendLine("            <div class=\"sig-box\"><div class=\"sig-title\">Giám Đốc / Chủ Quán</div><div>(Ký và đóng dấu)</div></div>");
            sb.AppendLine("        </div>");
            sb.AppendLine("    </div></body></html>");
            return sb.ToString();
        }
    }
}
