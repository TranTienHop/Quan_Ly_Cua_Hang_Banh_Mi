using System.Collections.Generic;
using System.Data;

namespace QuanLyCuaHangBanhMi.Services.Abstractions
{
    /// <summary>Hợp đồng xuất báo cáo (CSV / HTML…).</summary>
    public interface IReportExporter
    {
        string FormatDisplayName { get; }
        string FileFilter { get; }
        string DefaultExtension { get; }

        string BuildFileContent(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis);
    }
}
