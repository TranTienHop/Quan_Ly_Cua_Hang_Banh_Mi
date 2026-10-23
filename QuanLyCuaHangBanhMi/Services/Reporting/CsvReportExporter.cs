using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace QuanLyCuaHangBanhMi.Services.Reporting
{
    public sealed class CsvReportExporter : ReportExporterBase
    {
        public override string FormatDisplayName => "Excel CSV";
        public override string FileFilter => "Tệp Excel CSV (*.csv)|*.csv|Tất cả tệp (*.*)|*.*";
        public override string DefaultExtension => ".csv";

        public override string BuildFileContent(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"\"{title.ToUpperInvariant()}\"");
            sb.AppendLine($"\"Thời gian:\",\"{EscapeCsv(period)}\"");
            sb.AppendLine($"\"Chi nhánh:\",\"{EscapeCsv(branchName)}\"");
            sb.AppendLine($"\"Thời điểm xuất:\",\"{DateTime.Now:dd/MM/yyyy HH:mm:ss}\"");
            sb.AppendLine();

            AppendKpiCsv(sb, summaryKpis);

            var colHeaders = new List<string>();
            foreach (DataColumn col in dataTable.Columns)
                colHeaders.Add($"\"{EscapeCsv(col.ColumnName)}\"");
            sb.AppendLine(string.Join(",", colHeaders));

            foreach (DataRow row in dataTable.Rows)
            {
                var cellValues = new List<string>();
                foreach (DataColumn col in dataTable.Columns)
                    cellValues.Add($"\"{EscapeCsv(FormatCell(row[col]))}\"");
                sb.AppendLine(string.Join(",", cellValues));
            }

            return sb.ToString();
        }
    }
}
