using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Services.Abstractions;
using QuanLyCuaHangBanhMi.Services.Reporting;

namespace QuanLyCuaHangBanhMi.Data
{
    /// <summary>
    /// UI helper xuất báo cáo — hỗ trợ Excel (.xlsx), PDF (.pdf), Print Preview, CSV và HTML.
    /// Có chạy nền bất đồng bộ và báo cáo tiến trình (ProgressBar).
    /// </summary>
    public static class ReportExportHelper
    {
        public static async Task<bool> ExportToExcelAsync(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis,
            IProgress<int>? progress,
            IProgress<string>? statusProgress,
            Action<string> onCompleted)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "Tệp Microsoft Excel (*.xlsx)|*.xlsx",
                FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                Title = "Lưu Báo Cáo Excel (.xlsx)"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return false;
            string savedPath = sfd.FileName;

            try
            {
                await ExcelReportExporter.ExportAsync(dataTable, title, period, branchName, summaryKpis, savedPath, progress, statusProgress);
                onCompleted(savedPath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file Excel (.xlsx):\n{ex.Message}", "Lỗi xuất file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static async Task<bool> ExportToPdfAsync(
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis,
            IProgress<int>? progress,
            IProgress<string>? statusProgress,
            Action<string> onCompleted)
        {
            using var sfd = new SaveFileDialog
            {
                Filter = "Tệp Adobe PDF (*.pdf)|*.pdf",
                FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                Title = "Lưu Báo Cáo PDF (.pdf)"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return false;
            string savedPath = sfd.FileName;

            try
            {
                await PdfReportExporter.ExportAsync(dataTable, title, period, branchName, summaryKpis, savedPath, progress, statusProgress);
                onCompleted(savedPath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file PDF (.pdf):\n{ex.Message}", "Lỗi xuất file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static void ShowPrintPreview(
            IWin32Window owner,
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis)
        {
            try
            {
                ReportPrintHelper.ShowPrintPreview(owner, dataTable, title, period, branchName, summaryKpis);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở Xem trước bản in: {ex.Message}", "Lỗi In Ấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ShowPrintDialog(
            IWin32Window owner,
            DataTable dataTable,
            string title,
            string period,
            string branchName,
            Dictionary<string, string>? summaryKpis)
        {
            try
            {
                ReportPrintHelper.ShowPrintDialog(owner, dataTable, title, period, branchName, summaryKpis);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi lệnh in: {ex.Message}", "Lỗi In Ấn", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
