using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Services.Abstractions;
using QuanLyCuaHangBanhMi.Services.Reporting;

namespace QuanLyCuaHangBanhMi.Data
{
    /// <summary>
    /// UI helper xuất báo cáo — ủy thác nội dung file cho IReportExporter (CSV / HTML).
    /// </summary>
    public static class ReportExportHelper
    {
        public static bool ExportToCsv(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis,
            out string savedPath)
            => ExportWith(new CsvReportExporter(), dataTable, title, period, branchName, summaryKpis, utf8Bom: true, out savedPath);

        public static bool ExportToHtml(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis,
            out string savedPath)
            => ExportWith(new HtmlReportExporter(), dataTable, title, period, branchName, summaryKpis, utf8Bom: false, out savedPath);

        /// <summary>Đa hình: nhận bất kỳ IReportExporter (CSV, HTML…).</summary>
        public static bool ExportWith(
            IReportExporter exporter,
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis,
            bool utf8Bom,
            out string savedPath)
        {
            savedPath = string.Empty;
            ArgumentNullException.ThrowIfNull(exporter);

            using var sfd = new SaveFileDialog
            {
                Filter = exporter.FileFilter,
                FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}{exporter.DefaultExtension}",
                Title = $"Lưu báo cáo ({exporter.FormatDisplayName})"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return false;
            savedPath = sfd.FileName;

            try
            {
                string content = exporter.BuildFileContent(dataTable, title, period, branchName, summaryKpis);
                var encoding = utf8Bom ? new UTF8Encoding(true) : Encoding.UTF8;
                File.WriteAllText(savedPath, content, encoding);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi xuất file {exporter.FormatDisplayName}:\n{ex.Message}",
                    "Lỗi xuất file",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

            if (res != DialogResult.Yes) return;

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
}
