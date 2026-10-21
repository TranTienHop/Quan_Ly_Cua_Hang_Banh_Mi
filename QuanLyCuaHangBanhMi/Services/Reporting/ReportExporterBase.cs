using System.Collections.Generic;
using System.Data;
using System.Text;
using QuanLyCuaHangBanhMi.Services.Abstractions;

namespace QuanLyCuaHangBanhMi.Services.Reporting
{
    /// <summary>Lớp trừu tượng xuất báo cáo — CSV / HTML kế thừa và override BuildFileContent.</summary>
    public abstract class ReportExporterBase : IReportExporter
    {
        public abstract string FormatDisplayName { get; }
        public abstract string FileFilter { get; }
        public abstract string DefaultExtension { get; }

        public abstract string BuildFileContent(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis);

        protected static string EscapeCsv(string text) => text.Replace("\"", "\"\"");

        protected static string FormatCell(object? val)
        {
            if (val == null || val == DBNull.Value) return "";
            if (val is decimal dec)
                return dec.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN"));
            return val.ToString() ?? "";
        }

        protected static void AppendKpiCsv(StringBuilder sb, Dictionary<string, string>? summaryKpis)
        {
            if (summaryKpis == null || summaryKpis.Count == 0) return;
            sb.AppendLine("\"--- TÓM TẮT CHỈ SỐ QUAN TRỌNG ---\"");
            foreach (var kvp in summaryKpis)
                sb.AppendLine($"\"{EscapeCsv(kvp.Key)}\",\"{EscapeCsv(kvp.Value)}\"");
            sb.AppendLine();
        }
    }
}
