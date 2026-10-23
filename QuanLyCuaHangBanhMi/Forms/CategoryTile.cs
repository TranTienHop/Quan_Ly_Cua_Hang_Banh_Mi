using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QuanLyCuaHangBanhMi.Forms
{
    /// <summary>
    /// Thẻ danh mục ở thanh dọc bên trái màn order: chỉ hiện tên danh mục (không icon).
    /// </summary>
    public sealed class CategoryTile : Control
    {
        private static readonly Font NameFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        private static readonly Color Amber = Color.FromArgb(242, 153, 0);
        private static readonly Color AmberLight = Color.FromArgb(255, 244, 222);
        private static readonly Color BorderGray = Color.FromArgb(226, 222, 214);
        private static readonly Color TextDark = Color.FromArgb(44, 62, 80);

        private const TextFormatFlags NameFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
            | TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;
        private const int MaxNameLines = 4;

        private bool _isHover;
        private bool _isSelected;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CategoryId { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CategoryName { get; set; } = "";

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color AccentColor { get; set; } = Amber;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                Invalidate();
            }
        }

        public CategoryTile()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            Font = NameFont;
        }

        private int Px(float value) => (int)Math.Round(value * DeviceDpi / 96f);

        /// <summary>Chiều cao tối thiểu cho tên ở độ rộng cho trước.</summary>
        public int PreferredHeightFor(int width)
        {
            int textWidth = Math.Max(1, width - Px(10));
            int lineHeight = TextRenderer.MeasureText("Ag", NameFont, new Size(textWidth, int.MaxValue), NameFlags).Height;
            int textHeight = TextRenderer.MeasureText(CategoryName, NameFont, new Size(textWidth, int.MaxValue), NameFlags).Height;
            textHeight = Math.Min(textHeight, lineHeight * MaxNameLines);
            return Math.Max(Px(56), Px(12) + textHeight + Px(12));
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

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var card = new Rectangle(0, 0, Width - 1, Height - 1);
            Color fill = _isSelected ? Amber : _isHover ? AmberLight : Color.White;
            using (var path = RoundedRect(card, Px(10)))
            using (var brush = new SolidBrush(fill))
            {
                g.FillPath(brush, path);
                if (!_isSelected)
                {
                    using var pen = new Pen(_isHover ? Blend(Amber, Color.White, 0.45f) : BorderGray, 1f);
                    g.DrawPath(pen, path);
                }
            }

            var nameRect = new Rectangle(Px(5), Px(6), Width - Px(10), Math.Max(0, Height - Px(12)));
            TextRenderer.DrawText(g, CategoryName, NameFont, nameRect, _isSelected ? Color.White : TextDark, NameFlags);
        }

        private static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = Math.Max(1, radius * 2);
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private static Color Blend(Color a, Color b, float amountOfB)
        {
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * amountOfB),
                (int)(a.G + (b.G - a.G) * amountOfB),
                (int)(a.B + (b.B - a.B) * amountOfB));
        }
    }
}
