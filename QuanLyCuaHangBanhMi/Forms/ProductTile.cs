using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyCuaHangBanhMi.Forms
{
    /// <summary>
    /// Thẻ món trên màn hình order: giá + nhãn tồn kho ở trên, khung ảnh (biểu tượng) ở giữa, tên món ở dưới.
    /// Món hết nguyên liệu được tô mờ và không bấm được.
    /// </summary>
    public sealed class ProductTile : Control
    {
        // Dùng chung cho mọi thẻ: thẻ được tạo lại mỗi lần lọc món, tránh rò rỉ handle GDI
        private static readonly Font NameFont = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        private static readonly Font PriceFont = new Font("Segoe UI", 11F, FontStyle.Bold);
        private static readonly Font BadgeFont = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        private static readonly Font IconFont = new Font("Segoe UI Emoji", 28F);

        private static readonly Color Amber = Color.FromArgb(242, 153, 0);
        private static readonly Color TextDark = Color.FromArgb(44, 62, 80);
        private static readonly Color Muted = Color.FromArgb(160, 155, 145);
        private static readonly Color CardBorder = Color.FromArgb(230, 224, 214);
        private static readonly Color HoverFill = Color.FromArgb(255, 249, 238);
        private static readonly Color OutOfStockFill = Color.FromArgb(240, 236, 228);
        private static readonly Color ImageFill = Color.FromArgb(250, 235, 205);
        private static readonly Color OutOfStockImageFill = Color.FromArgb(225, 220, 210);
        private static readonly Color IconColor = Color.FromArgb(196, 120, 40);
        private static readonly Color InStockBadge = Color.FromArgb(39, 174, 96);
        private static readonly Color OutOfStockBadge = Color.FromArgb(192, 57, 43);

        private const TextFormatFlags NameFlags = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak
            | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;

        private bool _isHover;
        private bool _isOutOfStock;
        private bool _isFlashing;
        private System.Windows.Forms.Timer? _flashTimer;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ItemName { get; set; } = "";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string PriceText { get; set; } = "";

        /// <summary>"Còn N" / "Hết hàng"; để trống với món không trừ kho (không hiện nhãn).</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string StockText { get; set; } = "";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string IconText { get; set; } = "🥖";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsOutOfStock
        {
            get => _isOutOfStock;
            set
            {
                _isOutOfStock = value;
                Cursor = value ? Cursors.Default : Cursors.Hand;
                Invalidate();
            }
        }

        public ProductTile()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            Font = NameFont;
        }

        private int Px(float value) => (int)Math.Round(value * DeviceDpi / 96f);

        /// <summary>Viền cam ngắn (~300ms) để báo món vừa được thêm vào giỏ.</summary>
        public void Flash()
        {
            if (IsDisposed) return;
            _isFlashing = true;
            Invalidate();
            if (_flashTimer == null)
            {
                _flashTimer = new System.Windows.Forms.Timer { Interval = 300 };
                _flashTimer.Tick += (s, e) =>
                {
                    _flashTimer!.Stop();
                    _isFlashing = false;
                    if (!IsDisposed) Invalidate();
                };
            }
            _flashTimer.Stop();
            _flashTimer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _flashTimer?.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHover = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHover = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnClick(EventArgs e)
        {
            if (!_isOutOfStock) base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            bool highlight = !_isOutOfStock && (_isHover || _isFlashing);
            float borderWidth = highlight ? Px(2) : 1f;
            float inset = borderWidth / 2f;
            var card = new RectangleF(inset, inset, Width - 1 - borderWidth, Height - 1 - borderWidth);
            Color fill = _isOutOfStock ? OutOfStockFill : highlight ? HoverFill : Color.White;
            using (var path = RoundedRect(card, Px(10)))
            using (var brush = new SolidBrush(fill))
            using (var pen = new Pen(highlight ? Amber : CardBorder, borderWidth))
            {
                g.FillPath(brush, path);
                g.DrawPath(pen, path);
            }

            int pad = Px(8);
            int contentWidth = Width - pad * 2;

            // Hàng trên: giá bên trái (ưu tiên hiện đủ), nhãn tồn kho bên phải
            Size priceSize = TextRenderer.MeasureText(PriceText, PriceFont, Size.Empty, TextFormatFlags.NoPadding);
            var topRow = new Rectangle(pad, Px(8), contentWidth, priceSize.Height);
            int badgeWidth = 0;
            if (StockText.Length > 0)
            {
                Size textSize = TextRenderer.MeasureText(StockText, BadgeFont, Size.Empty, TextFormatFlags.NoPadding);
                badgeWidth = Math.Max(Px(20), Math.Min(textSize.Width + Px(8), contentWidth - priceSize.Width - Px(3)));
                int badgeHeight = textSize.Height + Px(3);
                var badge = new Rectangle(topRow.Right - badgeWidth, topRow.Y + (topRow.Height - badgeHeight) / 2, badgeWidth, badgeHeight);
                using (var path = RoundedRect(badge, badgeHeight / 2f))
                using (var brush = new SolidBrush(_isOutOfStock ? OutOfStockBadge : InStockBadge))
                {
                    g.FillPath(brush, path);
                }
                TextRenderer.DrawText(g, StockText, BadgeFont, badge, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            }
            var priceRect = new Rectangle(topRow.X, topRow.Y, Math.Max(0, topRow.Width - badgeWidth - Px(3)), topRow.Height);
            TextRenderer.DrawText(g, PriceText, PriceFont, priceRect, _isOutOfStock ? Muted : TextDark,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

            // Tên món: tối đa 2 dòng ở đáy thẻ
            int lineHeight = TextRenderer.MeasureText("Ag", NameFont, Size.Empty, TextFormatFlags.NoPadding).Height;
            int nameHeight = lineHeight * 2;
            var nameRect = new Rectangle(pad, Height - Px(8) - nameHeight, contentWidth, nameHeight);
            TextRenderer.DrawText(g, ItemName, NameFont, nameRect, _isOutOfStock ? Muted : TextDark, NameFlags);

            // Khung "ảnh" ở giữa (DB chưa có ảnh món → dùng biểu tượng)
            int imageTop = topRow.Bottom + Px(6);
            var imageRect = new Rectangle(pad, imageTop, contentWidth, Math.Max(0, nameRect.Top - Px(6) - imageTop));
            using (var path = RoundedRect(imageRect, Px(8)))
            using (var brush = new SolidBrush(_isOutOfStock ? OutOfStockImageFill : ImageFill))
            {
                g.FillPath(brush, path);
            }
            TextRenderer.DrawText(g, IconText, IconFont, imageRect, _isOutOfStock ? Muted : IconColor,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        private static GraphicsPath RoundedRect(RectangleF r, float radius)
        {
            float d = Math.Max(1f, Math.Min(radius * 2, Math.Min(r.Width, r.Height)));
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
