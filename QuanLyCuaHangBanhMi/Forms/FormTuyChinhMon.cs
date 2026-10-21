using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyCuaHangBanhMi.Forms
{
    /// <summary>
    /// Form tùy chỉnh món kiểu POS: nhóm tùy chọn dạng ô lớn (mức cay/đá, topping…), nút Thêm.
    /// </summary>
    public sealed partial class FormTuyChinhMon : Form
    {
        private static readonly Color SelectFill = Color.FromArgb(210, 232, 246);
        private static readonly Color TileBorder = Color.FromArgb(210, 214, 220);
        private static readonly Color TextDark = Color.FromArgb(33, 37, 41);
        private static readonly Color TextMuted = Color.FromArgb(120, 120, 120);
        private static readonly Color Success = Color.FromArgb(39, 174, 96);

        private static readonly HashSet<string> SpicyIngredients = new(StringComparer.OrdinalIgnoreCase)
        {
            "Ớt tươi", "Tương ớt", "Wasabi"
        };

        private static readonly Dictionary<string, string> RemovableLabels = new(StringComparer.OrdinalIgnoreCase)
        {
            ["Pate gan"] = "Không pate",
            ["Ngò rí"] = "Không ngò",
            ["Lá mơ"] = "Không lá mơ",
            ["Dưa leo"] = "Không dưa leo",
            ["Đồ chua (cà rốt, củ cải)"] = "Không đồ chua",
            ["Xà lách"] = "Không xà lách",
            ["Hành tây"] = "Không hành",
            ["Sốt mayonnaise"] = "Không mayo",
            ["Chà bông"] = "Không chà bông",
            ["Tương cà"] = "Không tương cà",
            ["Mỡ hành"] = "Không mỡ hành",
            ["Phô mai Mozzarella"] = "Không phô mai",
            ["Bơ sáp (quả bơ)"] = "Không bơ"
        };

        private readonly CultureInfo _vn;
        private readonly List<OptionSection> _sections = new();
        private decimal _giaBan;
        private DataTable _promoTable = new();
        private decimal _cartNenBefore;
        private bool _isPos = true;
        private bool _isCombo;
        private int _quantity = 1;
        private int _maxQty = 99;

        public string NoteText => BuildNote();
        public int Quantity => _quantity;
        public decimal ExtraPrice => SelectedExtrasTotal();
        public decimal UnitPrice => _giaBan + ExtraPrice;
        public int? SuggestedPromoId { get; private set; }

        public FormTuyChinhMon()
        {
            _vn = new CultureInfo("vi-VN");
            InitializeComponent();
            ApplyBorderlessChrome();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                SeedDesignerPreview();
        }

        public FormTuyChinhMon(
            string tenMon,
            decimal giaBan,
            decimal? coTheBan,
            string loaiSanPham,
            string tenDanhMuc,
            IReadOnlyList<(string TenNguyenLieu, decimal SoLuong)> congThuc,
            decimal cartNenBefore,
            DataTable promoTable,
            bool isPosSource,
            CultureInfo? vnCulture = null)
        {
            _vn = vnCulture ?? new CultureInfo("vi-VN");
            InitializeComponent();
            ApplyBorderlessChrome();

            _giaBan = giaBan;
            _promoTable = promoTable;
            _cartNenBefore = cartNenBefore;
            _isPos = isPosSource;
            _isCombo = string.Equals(loaiSanPham, "Combo", StringComparison.OrdinalIgnoreCase);
            _maxQty = coTheBan.HasValue ? Math.Max(1, (int)Math.Floor(coTheBan.Value)) : 99;
            _quantity = 1;

            lblTitle.Text = $"{tenMon.ToUpperInvariant()}  {giaBan.ToString("N0", _vn)}đ";
            BuildSections(congThuc, tenDanhMuc, loaiSanPham);
            RenderSections();
            UpdateQtyUi();
            RefreshPromoHint();
        }

        /// <summary>Bỏ title bar Windows. Viền vẽ tay; nút × / Esc để hủy.</summary>
        private void ApplyBorderlessChrome()
        {
            FormBorderStyle = FormBorderStyle.None;
            ControlBox = false;
            Padding = new Padding(1);
            Paint += (_, e) =>
            {
                using var pen = new Pen(Color.FromArgb(180, 180, 180), 1f);
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            };
        }

        private int S(int value) => (int)Math.Round(value * DeviceDpi / 96f);

        private void btnThem_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void btnQtyMinus_Click(object? sender, EventArgs e)
        {
            if (_quantity <= 1) return;
            _quantity--;
            UpdateQtyUi();
            RefreshPromoHint();
        }

        private void btnQtyPlus_Click(object? sender, EventArgs e)
        {
            if (_quantity >= _maxQty) return;
            _quantity++;
            UpdateQtyUi();
            RefreshPromoHint();
        }

        private void UpdateQtyUi()
        {
            lblQtyValue.Text = _quantity.ToString(_vn);
            btnQtyMinus.Enabled = _quantity > 1;
            btnQtyPlus.Enabled = _quantity < _maxQty;
            decimal total = UnitPrice * _quantity;
            btnThem.Text = $"Thêm  ·  {total.ToString("N0", _vn)}đ";
        }

        private void SeedDesignerPreview()
        {
            _sections.Clear();
            _sections.Add(new OptionSection("Mức đá", exclusive: true, new[]
            {
                Opt("Nhiều đá", 0), Opt("Không đá", 0), Opt("Đá riêng", 0)
            }));
            _sections.Add(new OptionSection("Mức đường", exclusive: true, new[]
            {
                Opt("Ít đường", 0), Opt("Nhiều đường", 0), Opt("Không đường", 0)
            }));
            RenderSections();
        }

        private static OptionItem Opt(string label, decimal price, bool selected = false) =>
            new() { Label = label, ExtraPrice = price, Selected = selected };

        private void AddDrinkIceSugarSections(string iceTitle = "Mức đá", string sugarTitle = "Mức đường")
        {
            _sections.Add(new OptionSection(iceTitle, exclusive: true, new[]
            {
                Opt("Nhiều đá", 0), Opt("Không đá", 0), Opt("Đá riêng", 0)
            }));
            _sections.Add(new OptionSection(sugarTitle, exclusive: true, new[]
            {
                Opt("Ít đường", 0), Opt("Nhiều đường", 0), Opt("Không đường", 0)
            }));
        }

        private void AddFoodSections(IReadOnlyList<(string TenNguyenLieu, decimal SoLuong)> congThuc)
        {
            bool hasSpicy = congThuc.Any(x => SpicyIngredients.Contains(x.TenNguyenLieu));

            if (hasSpicy)
            {
                _sections.Add(new OptionSection("Độ cay", exclusive: true, new[]
                {
                    Opt("Cay vừa", 0, true), Opt("Không cay", 0), Opt("Ít cay", 0), Opt("Nhiều cay", 0)
                }));
            }

            var removable = congThuc
                .Select(x => x.TenNguyenLieu)
                .Where(n => RemovableLabels.ContainsKey(n))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .Select(n => Opt(RemovableLabels[n], 0))
                .ToList();
            if (removable.Count > 0)
                _sections.Add(new OptionSection("Bỏ thành phần", exclusive: false, removable));

            var extras = new List<OptionItem>();
            if (congThuc.Any(x => x.TenNguyenLieu.Equals("Pate gan", StringComparison.OrdinalIgnoreCase)))
                extras.Add(Opt("Thêm pate", 5000));
            extras.Add(Opt("Nướng giòn", 0));
            extras.Add(Opt("Thêm ớt", 0));

            if (!hasSpicy && removable.Count == 0)
            {
                _sections.Add(new OptionSection("Độ cay", exclusive: true, new[]
                {
                    Opt("Cay vừa", 0, true), Opt("Không cay", 0), Opt("Ít cay", 0), Opt("Nhiều cay", 0)
                }));
                _sections.Add(new OptionSection("Yêu cầu", exclusive: false, new[]
                {
                    Opt("Không rau", 0), Opt("Không hành", 0), Opt("Nướng giòn", 0)
                }));
            }
            else if (extras.Count > 0)
            {
                _sections.Add(new OptionSection("Thêm / khác", exclusive: false, extras));
            }
        }

        private void BuildSections(
            IReadOnlyList<(string TenNguyenLieu, decimal SoLuong)> congThuc,
            string tenDanhMuc,
            string loaiSanPham)
        {
            _sections.Clear();
            string dm = FormOrder.RemoveDiacritics(tenDanhMuc ?? "");
            // Bánh mì ngọt: đóng gói sẵn — không tùy chỉnh
            if (dm.Contains("ngot")) return;

            bool isDrink = dm.Contains("nuoc") || dm.Contains("pha che") || dm.Contains("dong chai");
            bool isCombo = string.Equals(loaiSanPham, "Combo", StringComparison.OrdinalIgnoreCase);

            // Món nước: chỉ mức đá + mức đường
            if (isDrink)
            {
                AddDrinkIceSugarSections();
                return;
            }

            // Cách A — Combo: tùy chỉnh bánh mì + mức đá/đường cho nước trong combo
            if (isCombo)
            {
                AddFoodSections(congThuc);
                if (_sections.Count == 0)
                {
                    _sections.Add(new OptionSection("Yêu cầu", exclusive: false, new[]
                    {
                        Opt("Ít cay", 0), Opt("Không rau", 0), Opt("Không hành", 0)
                    }));
                }
                AddDrinkIceSugarSections("Mức đá (nước)", "Mức đường (nước)");
                return;
            }

            AddFoodSections(congThuc);
        }

        private void RenderSections()
        {
            flpSections.SuspendLayout();
            flpSections.Controls.Clear();
            int contentW = Math.Max(S(280), pnlScroll.ClientSize.Width - flpSections.Padding.Horizontal - S(8));

            foreach (var section in _sections)
            {
                flpSections.Controls.Add(new Label
                {
                    AutoSize = true,
                    Text = section.Title,
                    Font = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold),
                    ForeColor = TextDark,
                    Margin = new Padding(0, S(10), 0, S(6))
                });

                int gap = S(8);
                int tileH = S(72);

                if (section.Exclusive)
                {
                    // 1 hàng cố định: TableLayoutPanel — không bị FlowLayout xuống dòng
                    int cols = Math.Clamp(section.Items.Count, 1, 4);
                    var table = new TableLayoutPanel
                    {
                        ColumnCount = cols,
                        RowCount = 1,
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        Margin = new Padding(0, 0, 0, S(4)),
                        Width = contentW,
                        Padding = Padding.Empty
                    };
                    for (int c = 0; c < cols; c++)
                        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / cols));
                    table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                    for (int i = 0; i < section.Items.Count && i < cols; i++)
                    {
                        var tile = CreateTile(section, section.Items[i], 0, tileH);
                        tile.Dock = DockStyle.Fill;
                        tile.Margin = new Padding(0, 0, i < cols - 1 ? gap : 0, gap);
                        table.Controls.Add(tile, i, 0);
                    }
                    flpSections.Controls.Add(table);
                }
                else
                {
                    int cols = 4;
                    int tileW = Math.Max(S(100), (contentW - gap * cols) / cols);
                    var grid = new FlowLayoutPanel
                    {
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        WrapContents = true,
                        Margin = new Padding(0, 0, 0, S(4)),
                        MaximumSize = new Size(contentW, 0),
                        MinimumSize = new Size(contentW, 0),
                        Width = contentW
                    };
                    foreach (var item in section.Items)
                    {
                        var tile = CreateTile(section, item, tileW, tileH);
                        grid.Controls.Add(tile);
                    }
                    flpSections.Controls.Add(grid);
                }
            }

            if (_sections.Count == 0)
            {
                flpSections.Controls.Add(new Label
                {
                    AutoSize = true,
                    Text = "Không có tùy chọn — bấm Thêm để đưa món vào đơn.",
                    ForeColor = TextMuted,
                    Margin = new Padding(0, S(12), 0, 0)
                });
            }

            flpSections.ResumeLayout();
        }

        private Control CreateTile(OptionSection section, OptionItem item, int w, int h)
        {
            var tile = new Panel
            {
                Size = w > 0 ? new Size(w, h) : new Size(S(120), h),
                MinimumSize = new Size(0, h),
                Margin = new Padding(0, 0, S(8), S(8)),
                Cursor = Cursors.Hand,
                Tag = (section, item),
                BackColor = item.Selected ? SelectFill : Color.White
            };
            tile.Paint += (s, e) =>
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(0, 0, tile.Width - 1, tile.Height - 1);
                using var path = Rounded(rect, S(8));
                using var brush = new SolidBrush(item.Selected ? SelectFill : Color.White);
                using var pen = new Pen(item.Selected ? Color.FromArgb(41, 128, 185) : TileBorder, item.Selected ? 2f : 1f);
                g.FillPath(brush, path);
                g.DrawPath(pen, path);

                var nameRect = new Rectangle(S(8), S(8), tile.Width - S(16), S(34));
                TextRenderer.DrawText(g, item.Label, new Font("Segoe UI", 9F), nameRect, TextDark,
                    TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis | TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPrefix);

                string price = item.ExtraPrice <= 0 ? "0đ" : item.ExtraPrice.ToString("N0", _vn) + "đ";
                var priceRect = new Rectangle(S(8), tile.Height - S(28), tile.Width - S(16), S(22));
                TextRenderer.DrawText(g, price, new Font("Segoe UI Semibold", 10F, FontStyle.Bold), priceRect, TextDark,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            };
            tile.Click += (_, _) => ToggleItem(section, item);
            return tile;
        }

        private void ToggleItem(OptionSection section, OptionItem item)
        {
            if (section.Exclusive)
            {
                // Bấm lại ô đang chọn → bỏ chọn (về mặc định ẩn)
                bool turnOff = item.Selected;
                foreach (var o in section.Items) o.Selected = !turnOff && o == item;
            }
            else
            {
                item.Selected = !item.Selected;
            }
            RenderSections();
            UpdateQtyUi();
            RefreshPromoHint();
        }

        private static GraphicsPath Rounded(Rectangle r, int radius)
        {
            float d = Math.Max(1, radius * 2);
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private decimal SelectedExtrasTotal() =>
            _sections.SelectMany(s => s.Items).Where(i => i.Selected).Sum(i => i.ExtraPrice);

        private string BuildNote()
        {
            var lines = new List<string>();
            foreach (var section in _sections)
            {
                foreach (var item in section.Items.Where(i => i.Selected))
                {
                    // Exclusive mặc định (option đầu / Cay vừa / Mặc định) không ghi vào đơn
                    if (section.Exclusive && IsDefaultSelection(section, item)) continue;
                    string price = item.ExtraPrice <= 0 ? "0đ" : item.ExtraPrice.ToString("N0", _vn) + "đ";
                    lines.Add($"+ {item.Label} {price}");
                }
            }
            string note = string.Join("\n", lines);
            return note.Length <= 255 ? note : note[..255];
        }

        private static bool IsDefaultSelection(OptionSection section, OptionItem item)
        {
            string l = item.Label;
            return l.Equals("Mặc định", StringComparison.OrdinalIgnoreCase)
                || l.Equals("Cay vừa", StringComparison.OrdinalIgnoreCase)
                || (section.Items.Count > 0 && section.Items[0] == item && item.ExtraPrice == 0
                    && (l.Contains("vừa", StringComparison.OrdinalIgnoreCase) || l.Equals("Mặc định", StringComparison.OrdinalIgnoreCase)));
        }

        private void RefreshPromoHint()
        {
            SuggestedPromoId = null;
            if (!_isPos)
            {
                lblPromo.Visible = false;
                return;
            }

            decimal addNen = _isCombo ? 0 : UnitPrice * _quantity;
            decimal nen = _cartNenBefore + addNen;
            int? bestId = null;
            decimal best = 0;
            string? bestName = null;
            foreach (DataRow p in _promoTable.Rows)
            {
                decimal d = FormOrder.CalculatePromoDiscount(p, nen);
                if (d > best)
                {
                    best = d;
                    bestId = p.Field<int>("MaKhuyenMai");
                    bestName = p.Field<string>("TenKhuyenMai");
                }
            }

            if (bestId.HasValue && best > 0)
            {
                SuggestedPromoId = bestId;
                lblPromo.Visible = true;
                lblPromo.ForeColor = Success;
                lblPromo.Text = $"🎁 {bestName} (−{best.ToString("N0", _vn)} đ)";
            }
            else
            {
                // Không hiện chữ cảnh báo — chỉ hiện khi thực sự có KM áp dụng
                lblPromo.Visible = false;
                lblPromo.Text = "";
            }
        }

        private sealed class OptionSection
        {
            public string Title { get; }
            public bool Exclusive { get; }
            public List<OptionItem> Items { get; }

            public OptionSection(string title, bool exclusive, IEnumerable<OptionItem> items)
            {
                Title = title;
                Exclusive = exclusive;
                Items = items.ToList();
            }
        }

        private sealed class OptionItem
        {
            public string Label { get; init; } = "";
            public decimal ExtraPrice { get; init; }
            public bool Selected { get; set; }
        }
    }
}
