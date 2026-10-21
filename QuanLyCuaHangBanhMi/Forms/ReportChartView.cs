using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Forms
{
    public enum ReportChartType
    {
        Bar,
        Line,
        HorizontalBar,
        Pie
    }

    /// <summary>
    /// Custom UserControl vẽ biểu đồ chuyên nghiệp bằng GDI+ (Anti-alias, High DPI, No dependencies)
    /// Hỗ trợ xuất ảnh PNG/JPEG chất lượng cao
    /// </summary>
    public partial class ReportChartView : UserControl
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Title { get; set; } = "Biểu Đồ Thống Kê";

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Subtitle { get; set; } = "";

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ReportChartType ChartType { get; set; } = ReportChartType.Bar;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ReportService.ChartPoint> DataPoints { get; set; } = new List<ReportService.ChartPoint>();

        public ReportChartView()
        {
            InitializeComponent();
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            DrawChart(g, this.ClientRectangle);
        }

        public void DrawChart(Graphics g, Rectangle bounds)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // Nền trắng mềm mại
            using var bgBrush = new SolidBrush(this.BackColor);
            g.FillRectangle(bgBrush, bounds);

            // Vẽ tiêu đề
            int topOffset = 20;
            using (var titleFont = new Font("Segoe UI", 13.5F, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(Color.FromArgb(44, 62, 80)))
            {
                var titleSize = g.MeasureString(Title, titleFont);
                g.DrawString(Title, titleFont, titleBrush, bounds.X + 25, topOffset);
                topOffset += (int)titleSize.Height + 5;
            }

            if (!string.IsNullOrEmpty(Subtitle))
            {
                using (var subFont = new Font("Segoe UI", 9.5F, FontStyle.Regular))
                using (var subBrush = new SolidBrush(Color.FromArgb(127, 140, 141)))
                {
                    g.DrawString(Subtitle, subFont, subBrush, bounds.X + 25, topOffset);
                    topOffset += 22;
                }
            }
            else
            {
                topOffset += 10;
            }

            // Nếu không có dữ liệu
            if (DataPoints == null || DataPoints.Count == 0)
            {
                using var noDataFont = new Font("Segoe UI", 12F, FontStyle.Italic);
                using var noDataBrush = new SolidBrush(Color.Gray);
                string noDataText = "Không có dữ liệu trong khoảng thời gian đã chọn";
                var sz = g.MeasureString(noDataText, noDataFont);
                g.DrawString(noDataText, noDataFont, noDataBrush,
                    bounds.X + (bounds.Width - sz.Width) / 2,
                    bounds.Y + (bounds.Height - sz.Height) / 2);
                return;
            }

            Rectangle plotArea = new Rectangle(
                bounds.X + 60,
                topOffset,
                bounds.Width - 120,
                bounds.Height - topOffset - 60
            );

            if (plotArea.Width <= 50 || plotArea.Height <= 50) return;

            switch (ChartType)
            {
                case ReportChartType.Bar:
                    DrawBarChart(g, plotArea);
                    break;
                case ReportChartType.Line:
                    DrawLineChart(g, plotArea);
                    break;
                case ReportChartType.HorizontalBar:
                    DrawHorizontalBarChart(g, plotArea);
                    break;
                case ReportChartType.Pie:
                    DrawPieChart(g, plotArea);
                    break;
            }
        }

        #region ====================== 1. BIỂU ĐỒ CỘT (BAR CHART) ======================

        private void DrawBarChart(Graphics g, Rectangle plotArea)
        {
            decimal maxValue = 0;
            foreach (var pt in DataPoints)
            {
                if (pt.Value > maxValue) maxValue = pt.Value;
            }
            if (maxValue <= 0) maxValue = 1;

            // Làm tròn cận trên trục Y
            decimal yMax = GetNiceMax(maxValue);

            // Vẽ lưới Y ngang
            int gridSteps = 5;
            using var gridPen = new Pen(Color.FromArgb(235, 237, 239), 1f) { DashStyle = DashStyle.Dash };
            using var axisPen = new Pen(Color.FromArgb(189, 195, 199), 1.5f);
            using var labelFont = new Font("Segoe UI", 8.5F);
            using var labelBrush = new SolidBrush(Color.FromArgb(127, 140, 141));
            using var valFont = new Font("Segoe UI", 8F, FontStyle.Bold);

            for (int i = 0; i <= gridSteps; i++)
            {
                decimal v = (yMax / gridSteps) * i;
                float y = plotArea.Bottom - (float)((double)v / (double)yMax * plotArea.Height);
                g.DrawLine(gridPen, plotArea.Left, y, plotArea.Right, y);

                string yText = FormatCompact(v);
                var sz = g.MeasureString(yText, labelFont);
                g.DrawString(yText, labelFont, labelBrush, plotArea.Left - sz.Width - 8, y - sz.Height / 2);
            }

            // Trục tọa độ X và Y
            g.DrawLine(axisPen, plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);
            g.DrawLine(axisPen, plotArea.Left, plotArea.Top, plotArea.Left, plotArea.Bottom);

            // Vẽ từng cột
            int count = DataPoints.Count;
            float totalGap = plotArea.Width / (float)count;
            float barWidth = Math.Max(12f, Math.Min(totalGap * 0.65f, 45f));

            for (int i = 0; i < count; i++)
            {
                var pt = DataPoints[i];
                float barH = (float)((double)pt.Value / (double)yMax * plotArea.Height);
                float cx = plotArea.Left + (i + 0.5f) * totalGap;
                float x = cx - barWidth / 2;
                float y = plotArea.Bottom - barH;

                RectangleF barRect = new RectangleF(x, y, barWidth, barH);

                // Màu gradient cho cột
                Color barColor = pt.Color.IsEmpty ? Color.FromArgb(41, 128, 185) : pt.Color;
                Color barColorLight = ControlPaint.Light(barColor, 0.3f);

                if (barH > 2)
                {
                    using (var barBrush = new LinearGradientBrush(new PointF(x, y), new PointF(x + barWidth, y + barH), barColor, barColorLight))
                    {
                        g.FillRectangle(barBrush, barRect);
                    }
                    using (var borderPen = new Pen(ControlPaint.Dark(barColor, 0.1f), 1f))
                    {
                        g.DrawRectangle(borderPen, x, y, barWidth, barH);
                    }
                }

                // Nhãn giá trị phía trên cột (nếu vừa đủ chỗ)
                if (count <= 20)
                {
                    string vText = FormatCompact(pt.Value);
                    var vSz = g.MeasureString(vText, valFont);
                    using var valBrush = new SolidBrush(Color.FromArgb(44, 62, 80));
                    g.DrawString(vText, valFont, valBrush, cx - vSz.Width / 2, y - vSz.Height - 3);
                }

                // Nhãn X dưới đáy
                var xSz = g.MeasureString(pt.Label, labelFont);
                float labelY = plotArea.Bottom + 6;

                // Nếu nhãn quá dài hoặc nhiều ngày, vẽ nghiêng hoặc cách quãng
                if (count > 15 && i % 2 != 0) continue; // Bỏ qua bớt nhãn nếu quá dày

                g.DrawString(pt.Label, labelFont, labelBrush, cx - xSz.Width / 2, labelY);
            }
        }

        #endregion

        #region ====================== 2. BIỂU ĐỒ ĐƯỜNG (LINE CHART) ======================

        private void DrawLineChart(Graphics g, Rectangle plotArea)
        {
            decimal maxValue = 0;
            foreach (var pt in DataPoints)
            {
                if (pt.Value > maxValue) maxValue = pt.Value;
            }
            if (maxValue <= 0) maxValue = 1;

            decimal yMax = GetNiceMax(maxValue);
            int gridSteps = 5;
            using var gridPen = new Pen(Color.FromArgb(235, 237, 239), 1f) { DashStyle = DashStyle.Dash };
            using var axisPen = new Pen(Color.FromArgb(189, 195, 199), 1.5f);
            using var labelFont = new Font("Segoe UI", 8.5F);
            using var labelBrush = new SolidBrush(Color.FromArgb(127, 140, 141));
            using var valFont = new Font("Segoe UI", 8F, FontStyle.Bold);

            for (int i = 0; i <= gridSteps; i++)
            {
                decimal v = (yMax / gridSteps) * i;
                float y = plotArea.Bottom - (float)((double)v / (double)yMax * plotArea.Height);
                g.DrawLine(gridPen, plotArea.Left, y, plotArea.Right, y);

                string yText = FormatCompact(v);
                var sz = g.MeasureString(yText, labelFont);
                g.DrawString(yText, labelFont, labelBrush, plotArea.Left - sz.Width - 8, y - sz.Height / 2);
            }

            g.DrawLine(axisPen, plotArea.Left, plotArea.Bottom, plotArea.Right, plotArea.Bottom);
            g.DrawLine(axisPen, plotArea.Left, plotArea.Top, plotArea.Left, plotArea.Bottom);

            int count = DataPoints.Count;
            if (count < 2) return;

            PointF[] points = new PointF[count];
            float totalGap = plotArea.Width / (float)(count - 1);

            for (int i = 0; i < count; i++)
            {
                float x = plotArea.Left + i * totalGap;
                float y = plotArea.Bottom - (float)((double)DataPoints[i].Value / (double)yMax * plotArea.Height);
                points[i] = new PointF(x, y);

                // Nhãn X
                if (count <= 15 || i % 2 == 0)
                {
                    var xSz = g.MeasureString(DataPoints[i].Label, labelFont);
                    g.DrawString(DataPoints[i].Label, labelFont, labelBrush, x - xSz.Width / 2, plotArea.Bottom + 6);
                }
            }

            // Vẽ vùng gradient mờ phía dưới đường biểu đồ
            using (var path = new GraphicsPath())
            {
                path.AddLines(points);
                path.AddLine(points[^1].X, points[^1].Y, points[^1].X, plotArea.Bottom);
                path.AddLine(points[^1].X, plotArea.Bottom, points[0].X, plotArea.Bottom);
                path.CloseFigure();

                using var areaBrush = new LinearGradientBrush(
                    new PointF(0, plotArea.Top),
                    new PointF(0, plotArea.Bottom),
                    Color.FromArgb(80, 52, 152, 219),
                    Color.FromArgb(5, 52, 152, 219));
                g.FillPath(areaBrush, path);
            }

            // Vẽ đường nối nét mượt
            using (var linePen = new Pen(Color.FromArgb(41, 128, 185), 2.5f) { LineJoin = LineJoin.Round })
            {
                g.DrawLines(linePen, points);
            }

            // Vẽ điểm mốc (Markers)
            using var pointBrush = new SolidBrush(Color.White);
            using var pointPen = new Pen(Color.FromArgb(41, 128, 185), 2.5f);
            for (int i = 0; i < count; i++)
            {
                g.FillEllipse(pointBrush, points[i].X - 4f, points[i].Y - 4f, 8f, 8f);
                g.DrawEllipse(pointPen, points[i].X - 4f, points[i].Y - 4f, 8f, 8f);

                if (count <= 15)
                {
                    string vText = FormatCompact(DataPoints[i].Value);
                    var vSz = g.MeasureString(vText, valFont);
                    using var valBrush = new SolidBrush(Color.FromArgb(44, 62, 80));
                    g.DrawString(vText, valFont, valBrush, points[i].X - vSz.Width / 2, points[i].Y - vSz.Height - 5);
                }
            }
        }

        #endregion

        #region ====================== 3. BIỂU ĐỒ CỘT NGANG (HORIZONTAL BAR) ======================

        private void DrawHorizontalBarChart(Graphics g, Rectangle plotArea)
        {
            decimal maxValue = 0;
            foreach (var pt in DataPoints)
            {
                if (pt.Value > maxValue) maxValue = pt.Value;
            }
            if (maxValue <= 0) maxValue = 1;

            int count = Math.Min(DataPoints.Count, 12); // Tối đa 12 món trên biểu đồ
            float rowHeight = plotArea.Height / (float)count;
            float barHeight = Math.Max(14f, Math.Min(rowHeight * 0.65f, 32f));

            float nameWidth = Math.Min(plotArea.Width * 0.35f, 220f);
            float barAreaX = plotArea.Left + nameWidth + 10;
            float barAreaWidth = plotArea.Right - barAreaX - 90;

            using var nameFont = new Font("Segoe UI", 9F, FontStyle.Regular);
            using var nameBrush = new SolidBrush(Color.FromArgb(44, 62, 80));
            using var valFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            using var valBrush = new SolidBrush(Color.FromArgb(52, 73, 94));
            using var rankFont = new Font("Segoe UI", 8F, FontStyle.Bold);

            for (int i = 0; i < count; i++)
            {
                var pt = DataPoints[i];
                float yCenter = plotArea.Top + (i + 0.5f) * rowHeight;
                float yBar = yCenter - barHeight / 2;

                // Huy hiệu thứ hạng (#1, #2, #3...)
                Color badgeColor = i == 0 ? Color.FromArgb(241, 196, 15) :
                                   i == 1 ? Color.FromArgb(189, 195, 199) :
                                   i == 2 ? Color.FromArgb(211, 84, 0) : Color.FromArgb(236, 240, 241);
                Color badgeTextColor = i < 3 ? Color.White : Color.FromArgb(127, 140, 141);

                using (var badgeBrush = new SolidBrush(badgeColor))
                {
                    g.FillRectangle(badgeBrush, plotArea.Left, yCenter - 10, 24, 20);
                }
                using (var badgeTextBrush = new SolidBrush(badgeTextColor))
                {
                    string rankStr = $"#{i + 1}";
                    var rSz = g.MeasureString(rankStr, rankFont);
                    g.DrawString(rankStr, rankFont, badgeTextBrush, plotArea.Left + (24 - rSz.Width) / 2, yCenter - rSz.Height / 2);
                }

                // Tên món (cắt ngắn nếu dài)
                string displayName = pt.Label;
                var nSz = g.MeasureString(displayName, nameFont);
                while (nSz.Width > nameWidth - 35 && displayName.Length > 3)
                {
                    displayName = displayName.Substring(0, displayName.Length - 4) + "...";
                    nSz = g.MeasureString(displayName, nameFont);
                }
                g.DrawString(displayName, nameFont, nameBrush, plotArea.Left + 28, yCenter - nSz.Height / 2);

                // Thanh Bar
                float barW = (float)((double)pt.Value / (double)maxValue * barAreaWidth);
                barW = Math.Max(barW, 6f);

                Color barCol = pt.Color.IsEmpty ? Color.FromArgb(230, 126, 34) : pt.Color;
                using (var barBrush = new SolidBrush(barCol))
                {
                    g.FillRectangle(barBrush, barAreaX, yBar, barW, barHeight);
                }

                // Nhãn giá trị bên phải thanh bar
                string valStr = pt.FormattedValue;
                var vSz = g.MeasureString(valStr, valFont);
                g.DrawString(valStr, valFont, valBrush, barAreaX + barW + 8, yCenter - vSz.Height / 2);
            }
        }

        #endregion

        #region ====================== 4. BIỂU ĐỒ TRÒN / DONUT (PIE CHART) ======================

        private void DrawPieChart(Graphics g, Rectangle plotArea)
        {
            decimal total = 0;
            foreach (var pt in DataPoints) total += pt.Value;
            if (total <= 0) return;

            // Chia thành 2 cột: Trái vẽ hình tròn, Phải vẽ chú thích (Legend)
            int legendWidth = Math.Min(260, plotArea.Width / 2);
            int pieDiameter = Math.Min(plotArea.Height - 20, plotArea.Width - legendWidth - 40);
            if (pieDiameter < 80) pieDiameter = 80;

            Rectangle pieRect = new Rectangle(
                plotArea.Left + 20,
                plotArea.Top + (plotArea.Height - pieDiameter) / 2,
                pieDiameter,
                pieDiameter
            );

            Rectangle legendRect = new Rectangle(
                pieRect.Right + 30,
                plotArea.Top + 10,
                plotArea.Right - pieRect.Right - 35,
                plotArea.Height - 20
            );

            float startAngle = -90f;
            using var borderPen = new Pen(Color.White, 2f);

            foreach (var pt in DataPoints)
            {
                float sweepAngle = (float)((double)pt.Value / (double)total * 360.0);
                if (sweepAngle <= 0) continue;

                using (var brush = new SolidBrush(pt.Color))
                {
                    g.FillPie(brush, pieRect, startAngle, sweepAngle);
                    g.DrawPie(borderPen, pieRect, startAngle, sweepAngle);
                }

                startAngle += sweepAngle;
            }

            // Khoét lỗ ở giữa tạo thành Donut Chart sang trọng
            int holeDiameter = (int)(pieDiameter * 0.52f);
            Rectangle holeRect = new Rectangle(
                pieRect.X + (pieDiameter - holeDiameter) / 2,
                pieRect.Y + (pieDiameter - holeDiameter) / 2,
                holeDiameter,
                holeDiameter
            );
            using (var holeBrush = new SolidBrush(this.BackColor))
            {
                g.FillEllipse(holeBrush, holeRect);
            }

            // Chữ tổng cộng ở giữa Donut
            using (var midFont = new Font("Segoe UI", 9F, FontStyle.Bold))
            using (var midValFont = new Font("Segoe UI", 11F, FontStyle.Bold))
            using (var midBrush = new SolidBrush(Color.FromArgb(44, 62, 80)))
            {
                string topL = "TỔNG CỘNG";
                var tSz = g.MeasureString(topL, midFont);
                g.DrawString(topL, midFont, midBrush, holeRect.X + (holeDiameter - tSz.Width) / 2, holeRect.Y + holeDiameter / 2 - 18);

                string vL = FormatCompact(total) + " đ";
                var vSz = g.MeasureString(vL, midValFont);
                g.DrawString(vL, midValFont, midBrush, holeRect.X + (holeDiameter - vSz.Width) / 2, holeRect.Y + holeDiameter / 2 + 2);
            }

            // Vẽ Chú thích (Legend)
            using var legFont = new Font("Segoe UI", 9F, FontStyle.Regular);
            using var legPctFont = new Font("Segoe UI", 9F, FontStyle.Bold);
            using var legBrush = new SolidBrush(Color.FromArgb(44, 62, 80));

            int maxItems = Math.Min(DataPoints.Count, 9);
            float legItemH = Math.Min(32f, (legendRect.Height - 10) / (float)maxItems);

            for (int i = 0; i < maxItems; i++)
            {
                var pt = DataPoints[i];
                float y = legendRect.Top + i * legItemH;
                float pct = (float)((double)pt.Value / (double)total * 100.0);

                // Hộp màu
                using (var b = new SolidBrush(pt.Color))
                {
                    g.FillRectangle(b, legendRect.Left, y + 4, 14, 14);
                }

                // Tên danh mục + %
                string itemText = pt.Label;
                if (itemText.Length > 18) itemText = itemText.Substring(0, 16) + "...";
                g.DrawString(itemText, legFont, legBrush, legendRect.Left + 22, y + 2);

                string pctText = $"{pct:0.0}% ({FormatCompact(pt.Value)})";
                g.DrawString(pctText, legPctFont, legBrush, legendRect.Left + 155, y + 2);
            }
        }

        #endregion

        #region ====================== 5. TIỆN ÍCH XUẤT ẢNH & ĐỊNH DẠNG ======================

        public void SaveAsImage(string filePath)
        {
            int w = Math.Max(this.Width, 900);
            int h = Math.Max(this.Height, 600);

            using var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                DrawChart(g, new Rectangle(0, 0, w, h));
            }
            bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        }

        public void CopyToClipboard()
        {
            int w = Math.Max(this.Width, 900);
            int h = Math.Max(this.Height, 600);

            using var bmp = new Bitmap(w, h);
            using (var g = Graphics.FromImage(bmp))
            {
                DrawChart(g, new Rectangle(0, 0, w, h));
            }
            Clipboard.SetImage(bmp);
        }

        private static string FormatCompact(decimal val)
        {
            if (val >= 1_000_000_000)
                return (val / 1_000_000_000m).ToString("0.#") + " tỷ";
            if (val >= 1_000_000)
                return (val / 1_000_000m).ToString("0.#") + " tr";
            if (val >= 1_000)
                return (val / 1_000m).ToString("0.#") + "k";
            return val.ToString("N0");
        }

        private static decimal GetNiceMax(decimal val)
        {
            if (val <= 0) return 10;
            double v = (double)val;
            double exponent = Math.Floor(Math.Log10(v));
            double fraction = v / Math.Pow(10, exponent);
            double niceFraction;

            if (fraction <= 1) niceFraction = 1;
            else if (fraction <= 2) niceFraction = 2;
            else if (fraction <= 5) niceFraction = 5;
            else niceFraction = 10;

            return (decimal)(niceFraction * Math.Pow(10, exponent));
        }

        #endregion
    }
}
