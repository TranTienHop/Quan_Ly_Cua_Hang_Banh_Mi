using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuanLyCuaHangBanhMi.Domain;
using QuanLyCuaHangBanhMi.Models;
using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.Services.Abstractions;

namespace QuanLyCuaHangBanhMi.Forms
{
    /// <summary>
    /// Màn hình nhân viên order món cho khách: chọn món → giỏ hàng → khuyến mãi / điểm → thanh toán.
    /// Lưu HoaDon + ChiTiet_HoaDon + ThanhToan_HoaDon và trừ kho theo công thức trong 1 transaction.
    /// </summary>
    public partial class FormOrder : Form
    {
        // Quy tắc nghiệp vụ lấy theo dữ liệu mẫu (database\gen_banhang.py)
        private const decimal PhiShipNoiBoMacDinh = 15000m;
        private const decimal TienMotDiemDoi = 1000m;      // 1 điểm đổi = 1.000đ
        private const decimal TienTichMotDiem = 10000m;    // 10.000đ (không tính ship) = 1 điểm

        private static readonly Color Amber = Color.FromArgb(242, 153, 0);
        private static readonly Color AmberDark = Color.FromArgb(204, 122, 0);
        private static readonly Color AmberLight = Color.FromArgb(255, 244, 222);
        private static readonly Color AmberSelect = Color.FromArgb(255, 226, 168);
        private static readonly Color CheckoutYellow = Color.FromArgb(255, 196, 0);
        private static readonly Color PageBack = Color.FromArgb(244, 245, 247);
        private static readonly Color MenuBack = Color.FromArgb(248, 246, 242);
        private static readonly Color TextDark = Color.FromArgb(44, 62, 80);
        private static readonly Color TextMuted = Color.FromArgb(127, 140, 141);
        private static readonly Color Danger = Color.FromArgb(231, 76, 60);
        private static readonly Color Success = Color.FromArgb(39, 174, 96);
        private static readonly Color BorderGray = Color.FromArgb(214, 219, 223);
        private static readonly Color NavSidebar = Color.FromArgb(44, 62, 80);
        private static readonly Color NavActive = Color.FromArgb(230, 126, 34);

        /// <summary>OOP: phụ thuộc interface nghiệp vụ (POS), không gọi thẳng Repository.</summary>
        private readonly IOrderService _orderService = new OrderServiceImpl();

        /// <summary>Sidebar hamburger — Dock Left, đẩy UI bán hàng sang phải.</summary>
        private Panel? _navFlyout;
        private static readonly Color[] CategoryPalette =
        {
            Color.FromArgb(230, 126, 34), Color.FromArgb(192, 57, 43), Color.FromArgb(39, 174, 96),
            Color.FromArgb(41, 128, 185), Color.FromArgb(142, 68, 173), Color.FromArgb(22, 160, 133),
            Color.FromArgb(211, 84, 0), Color.FromArgb(52, 73, 94), Color.FromArgb(243, 156, 18)
        };

        private static readonly (string Code, string Text)[] OrderSources =
        {
            ("POS", "Tại quầy"), ("Grab", "GrabFood"), ("ShopeeFood", "ShopeeFood")
        };

        private readonly CultureInfo _vnCulture = new CultureInfo("vi-VN");
        private readonly DataTable _cartTable = new DataTable();
        private DataTable _productTable = new DataTable();
        private DataTable _promoTable = new DataTable();
        private readonly Dictionary<int, Color> _categoryColors = new();
        private readonly List<ParkedOrder> _parkedOrders = new();
        private readonly Font _cartNameFont = new Font("Segoe UI Semibold", 10F);
        private readonly Font _cartNoteFont = new Font("Segoe UI", 8.5F, FontStyle.Italic);
        private readonly Font _emptyCartFont = new Font("Segoe UI", 10.5F, FontStyle.Italic);

        // Chi nhánh của nhân viên đăng nhập
        private int _maChiNhanh;
        private string _maCodeChiNhanh = "";
        private string _tenChiNhanh = "";
        private string _shiftText = "";

        // Trạng thái đơn đang order
        private int _selectedCategoryId;
        private string _orderType = "MangDi";
        private int? _tableNo; // 1–4 bàn vỉa hè khi Tại bàn
        private string _paymentMethod = "TienMat";
        private FlowLayoutPanel flpTables = null!;
        private readonly List<Button> _tableButtons = new();
        private int? _maKhuyenMai;
        private int? _maKhachHang;
        private string _tenKhachHang = "";
        private int _diemKhachHang;
        private decimal _phanTramGiamHang;
        private bool _isRecalculating;
        private bool _skipCloseConfirm;
        private Size _lastTileSize;
        private decimal _lastQuickCashTotal = -1;

        // Kết quả tính tiền gần nhất
        private decimal _tienHang, _tienGiamKhuyenMai, _tienGiamHang, _tienDoiDiem, _phiShip, _tongThanhToan;
        private int _diemTru, _diemCong;

        private bool _isFittingCategories;
        private string _customerInfoText = "";

        // Khu thanh toán kiểu POS (build động trong RebuildCheckoutPanel)
        private TextBox txtVoucherCode = null!;
        private TextBox txtBillNote = null!;
        private CheckBox chkEInvoice = null!;
        private TextBox txtSubtotalBox = null!;
        private TextBox txtDiscountBox = null!;
        private TextBox txtPayableBox = null!;
        private TextBox txtChangeBox = null!;
        private FlowLayoutPanel flpPayRadios = null!;
        private readonly List<RadioButton> _payRadios = new();

        /// <summary>true khi người dùng bấm Đăng xuất (FormMain / Program quay lại màn hình đăng nhập).</summary>
        public bool IsLoggedOut { get; private set; }

        /// <summary>Tab FormMain cần mở sau khi đóng Order (0 Tổng quan, 1 Thực đơn … 6 Nhân viên).</summary>
        public int? RequestedAdminTab { get; private set; }

        private sealed class ParkedOrder
        {
            public DateTime Time { get; init; }
            public DataTable Items { get; init; } = null!;
            public string CustomerPhone { get; init; } = "";
            public string CustomerName { get; init; } = "";
            public string Source { get; init; } = "POS";
            public string OrderType { get; init; } = "MangDi";
            public int? TableNo { get; init; }
            public string Address { get; init; } = "";
            public decimal ShipFee { get; init; }
            public int? PromoId { get; init; }
            public decimal Total { get; init; }
        }

        public FormOrder()
        {
            InitializeComponent();
            SetupOrderScreen();
            SetupTakeawayHeader();
            RebuildCheckoutPanel();
        }

        #region ======================== TIỆN ÍCH CHUNG ========================

        private int S(int value) => (int)Math.Round(value * DeviceDpi / 96f);

        private string Money(decimal value) => value.ToString("N0", _vnCulture) + " đ";

        private static decimal RoundDownThousand(decimal value) => Math.Floor(value / 1000m) * 1000m;

        internal static string PaymentText(string code) => code switch
        {
            "TienMat" => "Tiền mặt",
            "ChuyenKhoan" => "Chuyển khoản",
            "The" => "Thẻ",
            "Vi" => "Ví điện tử",
            "ZaloPay" => "ZaloPay",
            "ShopeePay" => "ShopeePay",
            "VNPay" => "VNPay",
            "Momo" => "Momo",
            "MomoQR" => "Momo-QR",
            "NowFoody" => "Now/Foody",
            "Grab" => "Grab",
            "beFood" => "beFood",
            "GoFood" => "GoFood",
            "TikTokShop" => "TikTokSho",
            _ => code
        };

        /// <summary>Mã lưu DB: ví/app → Vi, còn lại giữ nguyên.</summary>
        private static string PaymentDbCode(string uiCode) => uiCode switch
        {
            "TienMat" => "TienMat",
            "The" => "The",
            "ChuyenKhoan" => "ChuyenKhoan",
            _ => "Vi"
        };

        internal static string SourceText(string code) => code switch
        {
            "POS" => "Tại quầy",
            "Grab" => "GrabFood",
            "ShopeeFood" => "ShopeeFood",
            _ => code
        };

        internal static string OrderTypeText(string code) => code switch
        {
            "TaiQuay" => "Tại bàn",
            "MangDi" => "Mang về",
            "GiaoHang" => "Giao hàng",
            _ => code
        };

        internal static string RemoveDiacritics(string text)
        {
            string normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(normalized.Length);
            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC).Replace('đ', 'd').Replace('Đ', 'D').ToLowerInvariant();
        }

        /// <summary>
        /// Tiền giảm của 1 chương trình khuyến mãi trên phần tiền hàng không phải combo.
        /// PhanTram: làm tròn xuống nghìn, không vượt GiamToiDa. Chưa đủ DonToiThieu → 0.
        /// </summary>
        internal static decimal CalculatePromoDiscount(DataRow promo, decimal nen)
        {
            if (nen <= 0 || nen < promo.Field<decimal>("DonToiThieu")) return 0;

            decimal giaTri = promo.Field<decimal>("GiaTriGiam");
            decimal tien = giaTri;
            if (promo.Field<string>("LoaiGiam") == "PhanTram")
            {
                tien = RoundDownThousand(nen * giaTri / 100m);
                if (!promo.IsNull("GiamToiDa")) tien = Math.Min(tien, promo.Field<decimal>("GiamToiDa"));
            }
            return Math.Min(tien, nen);
        }

        private static Color Blend(Color a, Color b, float amountOfB)
        {
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * amountOfB),
                (int)(a.G + (b.G - a.G) * amountOfB),
                (int)(a.B + (b.B - a.B) * amountOfB));
        }

        private static void SetToggleState(Button btn, bool active)
        {
            btn.BackColor = active ? Amber : Color.White;
            btn.ForeColor = active ? Color.White : TextDark;
            btn.FlatAppearance.BorderColor = active ? Amber : BorderGray;
            btn.FlatAppearance.MouseOverBackColor = active ? AmberDark : AmberLight;
        }

        #endregion

        #region ======================== DỰNG GIAO DIỆN ========================

        // Khung giao diện tĩnh nằm trong FormOrder.Designer.cs (mở bằng Designer để kéo thả).
        // Ở đây chỉ còn phần phụ thuộc dữ liệu: bảng giỏ hàng, định dạng vi-VN, danh sách nguồn đơn.
        private void SetupOrderScreen()
        {
            _cartTable.Columns.Add("MaSanPham", typeof(int));
            _cartTable.Columns.Add("TenSanPham", typeof(string));
            _cartTable.Columns.Add("LoaiSanPham", typeof(string));
            _cartTable.Columns.Add("GiaGoc", typeof(decimal));
            _cartTable.Columns.Add("DonGia", typeof(decimal));
            _cartTable.Columns.Add("SoLuong", typeof(int));
            _cartTable.Columns.Add("GiamDong", typeof(decimal));
            _cartTable.Columns.Add("ThanhTien", typeof(decimal));
            _cartTable.Columns.Add("GhiChu", typeof(string));
            _cartTable.Columns.Add("CoTheBan", typeof(decimal));   // số phần tối đa còn làm được (NULL = không giới hạn)

            // AutoScaleMode.Dpi không co giãn độ rộng cột / chiều cao tiêu đề / padding của DataGridView
            foreach (DataGridViewColumn col in dgvCart.Columns)
            {
                if (col.AutoSizeMode != DataGridViewAutoSizeColumnMode.Fill) col.Width = S(col.Width);
                var pad = col.DefaultCellStyle.Padding;
                col.DefaultCellStyle.Padding = new Padding(S(pad.Left), S(pad.Top), S(pad.Right), S(pad.Bottom));
            }
            dgvCart.ColumnHeadersHeight = S(dgvCart.ColumnHeadersHeight);
            dgvCart.RowTemplate.Height = S(dgvCart.RowTemplate.Height);

            foreach (DataGridViewColumn col in new DataGridViewColumn[] { colPrice, colQty, colDiscount, colTotal })
                col.HeaderCell.Style.Alignment = col.DefaultCellStyle.Alignment;
            colPrice.DefaultCellStyle.FormatProvider = _vnCulture;
            colDiscount.DefaultCellStyle.FormatProvider = _vnCulture;
            colTotal.DefaultCellStyle.FormatProvider = _vnCulture;

            // Cột: Tên món | Đơn giá | − SL + | Giảm giá | Thành tiền | ⋯
            dgvCart.AutoGenerateColumns = false;
            dgvCart.DataSource = _cartTable;

            cboSource.Items.AddRange(OrderSources.Select(src => (object)src.Text).ToArray());
        }

        /// <summary>Header takeaway-first: Mang về mặc định, Tại bàn + chọn bàn 1–4, Giao ẩn chi tiết tới khi cần.</summary>
        private void SetupTakeawayHeader()
        {
            flpTables = new FlowLayoutPanel
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                WrapContents = false,
                Margin = new Padding(S(6), 2, S(4), 2),
                Visible = false
            };
            _tableButtons.Clear();
            for (int i = 1; i <= 4; i++)
            {
                int no = i;
                var btn = new Button
                {
                    Text = $"Bàn {no}",
                    Tag = no,
                    Size = new Size(S(56), S(30)),
                    Margin = new Padding(1, 1, 1, 1),
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    BackColor = Color.White,
                    ForeColor = TextDark,
                    TabStop = false
                };
                btn.FlatAppearance.BorderColor = BorderGray;
                btn.Click += (_, _) => SetTableNo(no);
                _tableButtons.Add(btn);
                flpTables.Controls.Add(btn);
            }

            // Chèn chip bàn ngay sau nút Tại bàn
            int idxTai = flpOrderType.Controls.GetChildIndex(btnTypeTaiQuay);
            flpOrderType.Controls.Add(flpTables);
            flpOrderType.Controls.SetChildIndex(flpTables, idxTai + 1);

            SetOrderType("MangDi");
        }

        private void SetTableNo(int? no)
        {
            _tableNo = no;
            for (int i = 0; i < _tableButtons.Count; i++)
                SetToggleState(_tableButtons[i], _tableNo == i + 1);
        }

        /// <summary>
        /// Khu vực thanh toán/hóa đơn (~40%): hộp vàng (voucher + ghi chú | tổng tiền) + radio TT + nút hành động.
        /// </summary>
        private void RebuildCheckoutPanel()
        {
            Color yellowBorder = Color.FromArgb(255, 196, 0);
            Color yellowBtn = Color.FromArgb(255, 196, 0);
            Color yellowBtnHover = Color.FromArgb(234, 180, 0);
            Color panelBg = Color.FromArgb(245, 245, 245);

            // Ẩn layout cũ (giữ control để logic cũ vẫn tham chiếu được)
            tlpPaymentOptions.Visible = false;
            tlpTotals.Visible = false;
            btnPayTienMat.Visible = btnPayChuyenKhoan.Visible = btnPayThe.Visible = btnPayVi.Visible = false;
            flpQuickCash.Visible = false;
            lblPoints.Visible = numRedeemPoints.Visible = lblRedeemHint.Visible = false;

            tlpCheckout.SuspendLayout();
            tlpCheckout.Controls.Clear();
            tlpCheckout.ColumnCount = 1;
            tlpCheckout.ColumnStyles.Clear();
            tlpCheckout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpCheckout.RowCount = 3;
            tlpCheckout.RowStyles.Clear();
            tlpCheckout.RowStyles.Add(new RowStyle(SizeType.Percent, 58F)); // hộp hóa đơn
            tlpCheckout.RowStyles.Add(new RowStyle(SizeType.Percent, 27F)); // hình thức TT
            tlpCheckout.RowStyles.Add(new RowStyle(SizeType.Percent, 15F)); // Xóa / Cất / Thanh toán

            // ----- Hộp vàng: trái voucher/ghi chú, phải tổng tiền -----
            var pnlBill = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(S(6)),
                Margin = new Padding(0, 0, 0, S(4))
            };
            pnlBill.Paint += (_, e) =>
            {
                using var pen = new Pen(yellowBorder, 2f);
                e.Graphics.DrawRectangle(pen, 1, 1, pnlBill.Width - 3, pnlBill.Height - 3);
            };

            var tlpBill = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.White,
                Padding = new Padding(S(4))
            };
            tlpBill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tlpBill.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));

            // Left: Voucher + Ghi chú + checkbox
            var tlpLeft = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
                Margin = new Padding(0, 0, S(6), 0)
            };
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, S(78)));
            tlpLeft.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Absolute, S(28)));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpLeft.RowStyles.Add(new RowStyle(SizeType.Absolute, S(26)));

            tlpLeft.Controls.Add(MakeCaption("Voucher KM"), 0, 0);
            txtVoucherCode = new TextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9F),
                Margin = new Padding(0, 1, 0, 1)
            };
            tlpLeft.Controls.Add(txtVoucherCode, 1, 0);

            tlpLeft.Controls.Add(MakeCaption("Ghi chú"), 0, 1);
            txtBillNote = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 9F),
                Margin = new Padding(0, 2, 0, 2)
            };
            tlpLeft.Controls.Add(txtBillNote, 1, 1);

            chkEInvoice = new CheckBox
            {
                Text = "Xuất Hóa đơn điện tử",
                AutoSize = true,
                Dock = DockStyle.Left,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextDark,
                Margin = new Padding(0)
            };
            tlpLeft.SetColumnSpan(chkEInvoice, 2);
            tlpLeft.Controls.Add(chkEInvoice, 0, 2);

            // Right: totals
            var tlpRight = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5,
                Margin = new Padding(S(4), 0, 0, 0)
            };
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tlpRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            for (int i = 0; i < 5; i++)
                tlpRight.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

            txtSubtotalBox = MakeMoneyBox(readOnly: true);
            txtDiscountBox = MakeMoneyBox(readOnly: true);
            txtPayableBox = MakeMoneyBox(readOnly: true);
            txtCustomerMoney.Parent?.Controls.Remove(txtCustomerMoney);
            txtCustomerMoney.Dock = DockStyle.Fill;
            txtCustomerMoney.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            txtCustomerMoney.TextAlign = HorizontalAlignment.Right;
            txtCustomerMoney.PlaceholderText = "0";
            txtCustomerMoney.Margin = new Padding(0, 1, 0, 1);
            txtChangeBox = MakeMoneyBox(readOnly: true);

            AddBillRow(tlpRight, 0, "Thành tiền", txtSubtotalBox);
            AddBillRow(tlpRight, 1, "Giảm giá / KM", txtDiscountBox);
            AddBillRow(tlpRight, 2, "Phải trả", txtPayableBox);
            AddBillRow(tlpRight, 3, "Tiền khách đưa", txtCustomerMoney);
            AddBillRow(tlpRight, 4, "Tiền thừa", txtChangeBox);

            // Overlay mã GD khi không phải tiền mặt
            txtPaymentRef.Parent?.Controls.Remove(txtPaymentRef);
            txtPaymentRef.Dock = DockStyle.Fill;
            txtPaymentRef.Visible = false;
            txtPaymentRef.Margin = new Padding(0, 1, 0, 1);
            // đặt cùng ô với tiền khách đưa — toggle visibility
            var pnlCashOrRef = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0, 1, 0, 1) };
            txtCustomerMoney.Dock = DockStyle.Fill;
            txtPaymentRef.Dock = DockStyle.Fill;
            pnlCashOrRef.Controls.Add(txtPaymentRef);
            pnlCashOrRef.Controls.Add(txtCustomerMoney);
            tlpRight.Controls.Remove(txtCustomerMoney);
            tlpRight.Controls.Add(pnlCashOrRef, 1, 3);

            tlpBill.Controls.Add(tlpLeft, 0, 0);
            tlpBill.Controls.Add(tlpRight, 1, 0);
            pnlBill.Controls.Add(tlpBill);
            tlpCheckout.Controls.Add(pnlBill, 0, 0);

            // ----- Radio hình thức thanh toán -----
            var pnlPayHost = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = panelBg,
                Padding = new Padding(S(4), S(2), S(4), S(2)),
                Margin = new Padding(0, 0, 0, S(2))
            };
            flpPayRadios = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = panelBg,
                WrapContents = true,
                AutoScroll = true
            };
            _payRadios.Clear();
            string[][] payGrid =
            {
                new[] { "TienMat", "Tiền mặt" },
                new[] { "ShopeePay", "ShopeePay" },
                new[] { "NowFoody", "Now/Foody" },
                new[] { "beFood", "beFood" },
                new[] { "The", "Thẻ" },
                new[] { "VNPay", "VNPay" },
                new[] { "Grab", "Grab" },
                new[] { "GoFood", "GoFood" },
                new[] { "ZaloPay", "ZaloPay" },
                new[] { "Momo", "Momo" },
            };
            foreach (var p in payGrid)
            {
                var rb = new RadioButton
                {
                    Text = p[1],
                    Tag = p[0],
                    AutoSize = true,
                    Font = new Font("Segoe UI", 8.5F),
                    ForeColor = TextDark,
                    Margin = new Padding(S(2), S(1), S(10), S(1)),
                    Checked = p[0] == "TienMat"
                };
                rb.CheckedChanged += (_, _) =>
                {
                    if (!rb.Checked) return;
                    SetPaymentMethod((string)rb.Tag!);
                };
                _payRadios.Add(rb);
                flpPayRadios.Controls.Add(rb);
            }
            pnlPayHost.Controls.Add(flpPayRadios);
            tlpCheckout.Controls.Add(pnlPayHost, 0, 1);

            // ----- Khuyến mại: giữa vùng thông tin (60%) và thanh toán (40%) -----
            if (tlpOrder.RowStyles.Count > 4)
                tlpOrder.RowStyles[4] = new RowStyle(SizeType.Absolute, S(44));
            btnPromo.Parent?.Controls.Remove(btnPromo);
            StyleYellowAction(btnPromo, yellowBtn, yellowBtnHover);
            btnPromo.Text = "🎁  Khuyến mại";
            btnPromo.ForeColor = Color.White;
            btnPromo.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            btnPromo.Dock = DockStyle.Fill;
            btnPromo.Margin = new Padding(0, S(4), 0, S(4));
            tlpOrder.Controls.Add(btnPromo, 0, 4);

            // ----- Nút dưới: Xóa 12% + Cất 12% sát trái | Thanh toán 35% sát phải -----
            tlpActions.Parent?.Controls.Remove(tlpActions);
            tlpActions.Dock = DockStyle.Fill;
            tlpActions.Margin = new Padding(0);
            tlpActions.ColumnCount = 4;
            tlpActions.ColumnStyles.Clear();
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F)); // Xóa ĐH
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F)); // Cất
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 41F)); // khoảng trống giữa
            tlpActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F)); // Thanh toán
            tlpActions.Controls.Clear();

            StyleYellowAction(btnClearOrder, yellowBtn, yellowBtnHover);
            btnClearOrder.Text = "Xóa ĐH";
            btnClearOrder.ForeColor = Color.White;
            btnClearOrder.Margin = new Padding(0, 2, 2, 2);

            StyleYellowAction(btnParkOrder, yellowBtn, yellowBtnHover);
            btnParkOrder.Text = "Cất";
            btnParkOrder.ForeColor = Color.White;
            btnParkOrder.Margin = new Padding(0, 2, 2, 2);

            StyleYellowAction(btnCheckout, yellowBtn, yellowBtnHover);
            btnCheckout.Text = "💵  Thanh toán";
            btnCheckout.ForeColor = Color.White;
            btnCheckout.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            btnCheckout.Margin = new Padding(2, 2, 0, 2);

            // Giữ chkPrintReceipt ẩn nhưng vẫn dùng khi thanh toán
            chkPrintReceipt.Visible = false;
            chkPrintReceipt.Checked = true;

            tlpActions.Controls.Add(btnClearOrder, 0, 0);
            tlpActions.Controls.Add(btnParkOrder, 1, 0);
            tlpActions.Controls.Add(new Panel { Dock = DockStyle.Fill, Margin = Padding.Empty }, 2, 0);
            tlpActions.Controls.Add(btnCheckout, 3, 0);
            tlpCheckout.Controls.Add(tlpActions, 0, 2);

            tlpCheckout.ResumeLayout();

            SyncBillBoxesFromLabels();
            SetPaymentMethod(_paymentMethod);
        }

        private static Label MakeCaption(string text) => new()
        {
            Text = text,
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Font = new Font("Segoe UI", 8.5F),
            ForeColor = TextDark,
            Margin = new Padding(0, 4, 4, 0)
        };

        private static TextBox MakeMoneyBox(bool readOnly) => new()
        {
            Dock = DockStyle.Fill,
            ReadOnly = readOnly,
            TextAlign = HorizontalAlignment.Right,
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            Text = "0",
            Margin = new Padding(0, 1, 0, 1),
            BorderStyle = BorderStyle.FixedSingle
        };

        private static void AddBillRow(TableLayoutPanel tlp, int row, string caption, Control value)
        {
            tlp.Controls.Add(new Label
            {
                Text = caption,
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = TextDark,
                Margin = new Padding(0, 4, 4, 0)
            }, 0, row);
            tlp.Controls.Add(value, 1, row);
        }

        private static void StyleYellowAction(Button btn, Color bg, Color hover)
        {
            btn.BackColor = bg;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = hover;
            btn.Dock = DockStyle.Fill;
            btn.Cursor = Cursors.Hand;
            btn.Margin = new Padding(2);
            btn.UseVisualStyleBackColor = false;
        }

        private void SyncBillBoxesFromLabels()
        {
            if (txtSubtotalBox == null) return;
            txtSubtotalBox.Text = StripMoneySuffix(lblSubtotal.Text);
            txtDiscountBox.Text = StripMoneySuffix(lblDiscount.Text).TrimStart('-').Trim();
            if (txtDiscountBox.Text.Length == 0) txtDiscountBox.Text = "0";
            txtPayableBox.Text = StripMoneySuffix(lblFinal.Text);
        }

        private static string StripMoneySuffix(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "0";
            return text.Replace(" đ", "", StringComparison.OrdinalIgnoreCase).Trim();
        }

        #endregion

        #region ======================== SỰ KIỆN GIAO DIỆN ========================

        private void btnMenu_Click(object? sender, EventArgs e) => ShowMainMenu();

        private void btnOrders_Click(object? sender, EventArgs e) => ShowOrdersMenu();

        private void btnSettings_Click(object? sender, EventArgs e) => ShowSettingsMenu();

        private void tmrClock_Tick(object? sender, EventArgs e) => UpdateClock();

        // Thanh cuộn đẩy ra ngoài khung; vẫn cuộn bằng con lăn chuột
        private void pnlCategoryHost_Resize(object? sender, EventArgs e)
        {
            int padL = pnlCategoryHost.Padding.Left;
            int padT = pnlCategoryHost.Padding.Top;
            int innerW = Math.Max(1, pnlCategoryHost.ClientSize.Width - padL - pnlCategoryHost.Padding.Right);
            int innerH = Math.Max(1, pnlCategoryHost.ClientSize.Height - padT - pnlCategoryHost.Padding.Bottom);
            flpCategories.SetBounds(padL, padT,
                innerW + SystemInformation.GetVerticalScrollBarWidthForDpi(DeviceDpi), innerH);
            FitCategoryTiles();
        }

        private void flpProducts_Resize(object? sender, EventArgs e) => ResizeProductTiles();

        private void txtCustomerPhone_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            FindCustomer(false);
        }

        private void btnFindCustomer_Click(object? sender, EventArgs e) => FindCustomer(false);

        private void btnClearCustomer_Click(object? sender, EventArgs e)
        {
            txtCustomerPhone.Clear();
            SetCustomer(null);
        }

        // Tag của nút = mã HinhThucNhan (TaiQuay / MangDi / GiaoHang)
        private void btnOrderType_Click(object? sender, EventArgs e)
        {
            if (sender is Button { Tag: string code }) SetOrderType(code);
        }

        private void cboSource_SelectedIndexChanged(object? sender, EventArgs e) => OnSourceChanged();

        private void numShipFee_ValueChanged(object? sender, EventArgs e) => Recalculate();

        private void dgvCart_DataError(object? sender, DataGridViewDataErrorEventArgs e) => e.ThrowException = false;

        private void btnPromo_Click(object? sender, EventArgs e) => OpenPromoDialog();

        private void numRedeemPoints_ValueChanged(object? sender, EventArgs e) => Recalculate();

        // Tag của nút = HinhThuc trong ThanhToan_HoaDon (TienMat / ChuyenKhoan / The / Vi)
        private void btnPayment_Click(object? sender, EventArgs e)
        {
            if (!DemandPermission(PermissionCodes.PosPayment)) return;
            if (sender is Button { Tag: string code }) SetPaymentMethod(code);
        }

        private void txtCustomerMoney_TextChanged(object? sender, EventArgs e) => CalculateChange();

        private void txtCustomerMoney_Leave(object? sender, EventArgs e)
        {
            if (TryParseMoney(txtCustomerMoney.Text, out decimal value))
                txtCustomerMoney.Text = value.ToString("N0", _vnCulture);
        }

        private void txtCustomerMoney_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;
            Checkout();
        }

        private void btnClearOrder_Click(object? sender, EventArgs e) => ClearOrderWithConfirm();

        private void btnParkOrder_Click(object? sender, EventArgs e) => ParkCurrentOrder(true);

        private void btnCheckout_Click(object? sender, EventArgs e) => Checkout();

        #endregion

        #region ======================== TẢI DỮ LIỆU ========================

        private void FormOrder_Load(object? sender, EventArgs e)
        {
            if (!DemandPermission(PermissionCodes.PosOrder))
            {
                _skipCloseConfirm = true;
                Close();
                return;
            }

            ApplyOrderPermissions();
            if (!LoadBranchInfo())
            {
                IsLoggedOut = true;
                _skipCloseConfirm = true;
                Close();
                return;
            }

            LoadShiftInfo();
            LoadPromotions();
            LoadProducts();
            ResetOrder();
            UpdateParkedButton();
            UpdateClock();
            tmrClock.Start();
            ActiveControl = flpProducts;
        }

        private void ApplyOrderPermissions()
        {
            bool canUseCustomer = UserSession.HasPermission(PermissionCodes.PosCustomer);
            txtCustomerPhone.Enabled = canUseCustomer;
            btnFindCustomer.Enabled = canUseCustomer;
            btnClearCustomer.Enabled = canUseCustomer;
            numRedeemPoints.Enabled = canUseCustomer;

            bool canPay = UserSession.HasPermission(PermissionCodes.PosPayment);
            foreach (Control control in flpPayRadios.Controls)
                control.Enabled = canPay;
            btnPayTienMat.Enabled = canPay;
            btnPayChuyenKhoan.Enabled = canPay;
            btnPayThe.Enabled = canPay;
            btnPayVi.Enabled = canPay;
            btnCheckout.Enabled = canPay && _cartTable.Rows.Count > 0;

            chkPrintReceipt.Checked = UserSession.HasPermission(PermissionCodes.PosPrint);
        }

        private static bool DemandPermission(string permission)
        {
            if (UserSession.HasPermission(permission)) return true;
            MessageBox.Show("Tài khoản không có quyền thực hiện thao tác này.", "Không đủ quyền",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private bool LoadBranchInfo()
        {
            try
            {
                var branch = _orderService.GetBranchByEmployee(UserSession.EmployeeID);
                if (branch == null)
                {
                    MessageBox.Show("Không xác định được chi nhánh của nhân viên đang đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                _maChiNhanh = branch.MaChiNhanh;
                _maCodeChiNhanh = branch.MaCode;
                _tenChiNhanh = branch.TenChiNhanh;
                Text = $"Bánh Mì Ngon — Order bán hàng — {_tenChiNhanh}";
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thông tin chi nhánh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void LoadShiftInfo()
        {
            try
            {
                _shiftText = _orderService.GetOpenShiftText(_maChiNhanh);
            }
            catch (Exception ex)
            {
                _shiftText = "Không đọc được ca làm: " + ex.Message;
            }
        }

        private void LoadPromotions()
        {
            try
            {
                _promoTable = _orderService.GetActivePromotions(_maChiNhanh);
            }
            catch (Exception ex)
            {
                _promoTable = new DataTable();
                MessageBox.Show("Lỗi tải chương trình khuyến mãi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts()
        {
            try
            {
                _productTable = _orderService.GetPosMenu(_maChiNhanh);

                _categoryColors.Clear();
                var categories = _productTable.AsEnumerable()
                    .Select(r => (Id: r.Field<int>("MaDanhMuc"), Name: r.Field<string>("TenDanhMuc") ?? ""))
                    .Distinct()
                    .OrderBy(c => CategorySortOrder(c.Name))
                    .ThenBy(c => c.Name)
                    .ToList();
                for (int i = 0; i < categories.Count; i++)
                    _categoryColors[categories[i].Id] = CategoryPalette[i % CategoryPalette.Length];

                if (_selectedCategoryId != 0 && !categories.Any(c => c.Id == _selectedCategoryId))
                    _selectedCategoryId = 0;

                RenderCategoryButtons(categories);
                RenderProductTiles();
                RefreshCartStockLimits();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thực đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateClock()
        {
            lblClock.Text = DateTime.Now.ToString("HH:mm  dd/MM/yyyy");
        }

        #endregion

        #region ======================== THỰC ĐƠN ========================

        /// <summary>
        /// Signature → Combo → truyền thống → cá → bò & xúc xích → ngọt → nước → (Tất cả cuối).
        /// </summary>
        private static int CategorySortOrder(string tenDanhMuc)
        {
            string ten = RemoveDiacritics(tenDanhMuc);
            if (ten == "signature") return 0;
            if (ten == "combo") return 1;
            if (ten.Contains("truyen thong")) return 2;
            if (ten.Contains("banh mi ca")) return 3;
            if (ten.Contains("xuc xich") || ten.Contains("banh mi bo")) return 4;
            if (ten.Contains("ngot")) return 5;
            if (ten == "nuoc" || ten.Contains("pha che") || ten.Contains("dong chai")) return 6;
            return 90;
        }

        private void RenderCategoryButtons(List<(int Id, string Name)> categories)
        {
            flpCategories.SuspendLayout();
            while (flpCategories.Controls.Count > 0)
                flpCategories.Controls[0].Dispose();

            void AddTile(int id, string name, Color color)
            {
                var tile = new CategoryTile
                {
                    CategoryId = id,
                    CategoryName = name,
                    AccentColor = color,
                    IsSelected = id == _selectedCategoryId,
                    Margin = new Padding(S(2), S(3), S(2), S(3))
                };
                tile.Click += (s, e) => SelectCategory(id);
                flpCategories.Controls.Add(tile);
            }

            foreach (var c in categories)
                AddTile(c.Id, c.Name, _categoryColors[c.Id]);
            AddTile(0, "Tất cả", Amber);

            flpCategories.ResumeLayout();
            FitCategoryTiles();
        }

        private void SelectCategory(int id)
        {
            if (id == _selectedCategoryId) return;
            _selectedCategoryId = id;
            foreach (var tile in flpCategories.Controls.OfType<CategoryTile>())
                tile.IsSelected = tile.CategoryId == id;
            RenderProductTiles();
        }

        private void FitCategoryTiles()
        {
            if (_isFittingCategories) return;
            var tiles = flpCategories.Controls.OfType<CategoryTile>().ToList();
            if (tiles.Count == 0) return;

            int sidePad = S(2);
            int width = pnlCategoryHost.ClientSize.Width - pnlCategoryHost.Padding.Horizontal - sidePad * 2;
            if (width <= 0) return;

            int availH = Math.Max(1, pnlCategoryHost.ClientSize.Height - pnlCategoryHost.Padding.Vertical);
            int marginV = tiles.Sum(t => t.Margin.Vertical);
            int preferredSum = tiles.Sum(t => t.PreferredHeightFor(width));
            int extra = Math.Max(0, availH - preferredSum - marginV);
            int extraEach = extra / tiles.Count;
            int rem = extra % tiles.Count;

            _isFittingCategories = true;
            try
            {
                flpCategories.SuspendLayout();
                foreach (var tile in tiles)
                {
                    int h = tile.PreferredHeightFor(width) + extraEach + (rem > 0 ? 1 : 0);
                    if (rem > 0) rem--;
                    tile.Margin = new Padding(sidePad, tile.Margin.Top, sidePad, tile.Margin.Bottom);
                    tile.Size = new Size(width, h);
                }
                flpCategories.ResumeLayout();
            }
            finally
            {
                _isFittingCategories = false;
            }
        }

        private IEnumerable<DataRow> VisibleProducts()
        {
            foreach (DataRow r in _productTable.Rows)
            {
                if (_selectedCategoryId != 0 && r.Field<int>("MaDanhMuc") != _selectedCategoryId) continue;
                yield return r;
            }
        }

        private void RenderProductTiles()
        {
            flpProducts.SuspendLayout();
            while (flpProducts.Controls.Count > 0)
                flpProducts.Controls[0].Dispose();

            Size tileSize = TileSize();
            _lastTileSize = tileSize;
            int count = 0;
            foreach (DataRow r in VisibleProducts())
            {
                flpProducts.Controls.Add(CreateProductTile(r, tileSize));
                count++;
            }

            if (count == 0)
            {
                flpProducts.Controls.Add(new Label
                {
                    Text = _productTable.Rows.Count == 0 ? "Chi nhánh chưa có món nào đang bán." : "Danh mục này chưa có món.",
                    AutoSize = true,
                    Margin = new Padding(S(10)),
                    Font = new Font("Segoe UI", 10F, FontStyle.Italic),
                    ForeColor = TextMuted
                });
            }
            flpProducts.ResumeLayout();
        }

        private Size TileSize()
        {
            // Luôn chừa chỗ thanh cuộn dọc để số cột không nhảy và không bao giờ hiện cuộn ngang
            int available = flpProducts.Width - flpProducts.Padding.Horizontal
                            - SystemInformation.GetVerticalScrollBarWidthForDpi(DeviceDpi) - 1;
            int gap = S(10);
            // Ưu tiên thẻ to hơn: ~2–3 cột trên màn rộng
            int columns = Math.Max(2, available / (S(160) + gap));
            int width = Math.Max(S(140), available / columns - gap);
            return new Size(width, S(175));
        }

        private void ResizeProductTiles()
        {
            Size size = TileSize();
            if (size == _lastTileSize) return;
            _lastTileSize = size;

            flpProducts.SuspendLayout();
            foreach (Control c in flpProducts.Controls)
                if (c is ProductTile tile) tile.Size = size;
            flpProducts.ResumeLayout();
        }

        private ProductTile CreateProductTile(DataRow r, Size size)
        {
            // OOP: factory → abstract SanPhamBanHang (ThanhPham / Combo / KhongTruKho)
            ISanPhamBanHang mon = SanPhamBanHangFactory.FromPosMenuRow(r);

            var tile = new ProductTile
            {
                ItemName = mon.TenSanPham,
                PriceText = mon.GiaBan.ToString("N0", _vnCulture),
                StockText = mon.FormatTonKho(_vnCulture),
                IconText = mon.IconEmoji,
                IsOutOfStock = mon.HetHang,
                Size = size,
                Margin = new Padding(S(5)),
                Tag = r
            };
            tile.Click += (s, e) =>
            {
                int before = CartQuantityOf(r.Field<int>("MaSanPham"));
                if (TryAddProductWithOptions(r) && CartQuantityOf(r.Field<int>("MaSanPham")) > before)
                    tile.Flash();
            };
            return tile;
        }

        #endregion

        #region ======================== GIỎ HÀNG ========================

        private DataRow? CartRowAt(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgvCart.Rows.Count) return null;
            return (dgvCart.Rows[rowIndex].DataBoundItem as DataRowView)?.Row;
        }

        private int CartQuantityOf(int maSanPham)
        {
            return _cartTable.AsEnumerable().Where(r => r.Field<int>("MaSanPham") == maSanPham).Sum(r => r.Field<int>("SoLuong"));
        }

        private void CommitCartEdit()
        {
            dgvCart.EndEdit();
            if (BindingContext?[_cartTable] is CurrencyManager cm) cm.EndCurrentEdit();
        }

        /// <summary>Mở form tùy chỉnh theo công thức → thêm giỏ + tự áp KM tốt nhất.</summary>
        private bool TryAddProductWithOptions(DataRow product)
        {
            CommitCartEdit();
            int maSanPham = product.Field<int>("MaSanPham");
            string tenSanPham = product.Field<string>("TenSanPham") ?? "";
            decimal? canMake = product.IsNull("CoTheBan") ? null : Convert.ToDecimal(product["CoTheBan"]);

            if (canMake.HasValue && canMake.Value <= 0)
            {
                MessageBox.Show($"\"{tenSanPham}\" đã hết nguyên liệu tại chi nhánh.",
                    "Không đủ nguyên liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            int remain = canMake.HasValue
                ? Math.Max(0, (int)Math.Floor(canMake.Value) - CartQuantityOf(maSanPham))
                : 99;
            if (remain <= 0)
            {
                MessageBox.Show($"Chi nhánh chỉ còn đủ nguyên liệu cho {canMake:N0} phần \"{tenSanPham}\".",
                    "Không đủ nguyên liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Bánh mì ngọt = hàng đóng gói sẵn → thêm thẳng vào giỏ, không mở form tùy chỉnh
            string tenDanhMuc = product.Field<string>("TenDanhMuc") ?? "";
            if (RemoveDiacritics(tenDanhMuc).Contains("ngot"))
            {
                if (IsPosSource)
                    _maKhuyenMai = BestPromoId(CartNenForPromo() + product.Field<decimal>("GiaBan"));
                AddProductToCart(product, 1);
                return true;
            }

            var congThuc = LoadCongThuc(maSanPham);
            decimal nenBefore = CartNenForPromo();
            using var dlg = new FormTuyChinhMon(
                tenSanPham,
                product.Field<decimal>("GiaBan"),
                canMake.HasValue ? remain : null,
                product.Field<string>("LoaiSanPham") ?? "",
                tenDanhMuc,
                congThuc,
                nenBefore,
                _promoTable,
                IsPosSource,
                _vnCulture);

            if (dlg.ShowDialog(this) != DialogResult.OK) return false;

            int qty = Math.Min(dlg.Quantity, remain);
            if (qty <= 0) return false;

            _maKhuyenMai = dlg.SuggestedPromoId;
            AddProductToCart(product, qty, dlg.NoteText, dlg.UnitPrice, product.Field<decimal>("GiaBan") + dlg.ExtraPrice);
            return true;
        }

        private int? BestPromoId(decimal nen)
        {
            int? bestId = null;
            decimal best = 0;
            foreach (DataRow p in _promoTable.Rows)
            {
                decimal d = CalculatePromoDiscount(p, nen);
                if (d > best)
                {
                    best = d;
                    bestId = p.Field<int>("MaKhuyenMai");
                }
            }
            return bestId;
        }

        private List<(string TenNguyenLieu, decimal SoLuong)> LoadCongThuc(int maSanPham)
            => _orderService.GetRecipe(maSanPham);

        private decimal CartNenForPromo()
        {
            decimal nen = 0;
            foreach (DataRow row in _cartTable.Rows)
            {
                if (row.Field<string>("LoaiSanPham") == "Combo") continue;
                nen += row.Field<decimal>("DonGia") * row.Field<int>("SoLuong");
            }
            return nen;
        }

        private void AddProductToCart(DataRow product, int quantity = 1, string? note = null, decimal? unitPrice = null, decimal? giaGoc = null)
        {
            CommitCartEdit();
            int maSanPham = product.Field<int>("MaSanPham");
            string tenSanPham = product.Field<string>("TenSanPham") ?? "";
            decimal? canMake = product.IsNull("CoTheBan") ? null : Convert.ToDecimal(product["CoTheBan"]);
            string ghiChu = (note ?? "").Trim();
            decimal donGia = unitPrice ?? product.Field<decimal>("GiaBan");
            decimal goc = giaGoc ?? donGia;

            if (canMake.HasValue && CartQuantityOf(maSanPham) + quantity > canMake.Value)
            {
                string msg = canMake.Value <= 0
                    ? $"\"{tenSanPham}\" đã hết nguyên liệu tại chi nhánh."
                    : $"Chi nhánh chỉ còn đủ nguyên liệu cho {canMake.Value:N0} phần \"{tenSanPham}\".";
                MessageBox.Show(msg, "Không đủ nguyên liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Gộp vào dòng cùng món + cùng ghi chú + cùng đơn giá
            DataRow? row = _cartTable.AsEnumerable().FirstOrDefault(r =>
                r.Field<int>("MaSanPham") == maSanPham
                && r.Field<decimal>("DonGia") == donGia
                && string.Equals(r.Field<string>("GhiChu") ?? "", ghiChu, StringComparison.Ordinal));
            if (row != null)
            {
                row["SoLuong"] = row.Field<int>("SoLuong") + quantity;
            }
            else
            {
                row = _cartTable.NewRow();
                row["MaSanPham"] = maSanPham;
                row["TenSanPham"] = tenSanPham;
                row["LoaiSanPham"] = product["LoaiSanPham"];
                row["GiaGoc"] = goc;
                row["DonGia"] = donGia;
                row["SoLuong"] = quantity;
                row["GiamDong"] = 0m;
                row["ThanhTien"] = donGia * quantity;
                row["CoTheBan"] = canMake.HasValue ? canMake.Value : DBNull.Value;
                row["GhiChu"] = ghiChu.Length == 0 ? DBNull.Value : ghiChu;
                _cartTable.Rows.Add(row);
            }

            Recalculate();
            SelectCartRow(row);
        }

        private void SelectCartRow(DataRow row)
        {
            foreach (DataGridViewRow gridRow in dgvCart.Rows)
            {
                if ((gridRow.DataBoundItem as DataRowView)?.Row == row)
                {
                    dgvCart.CurrentCell = gridRow.Cells["colName"];
                    break;
                }
            }
        }

        private void ChangeQuantity(DataRow row, int delta)
        {
            CommitCartEdit();
            int maSanPham = row.Field<int>("MaSanPham");
            int qty = row.Field<int>("SoLuong") + delta;

            if (qty <= 0)
            {
                _cartTable.Rows.Remove(row);
                Recalculate();
                return;
            }
            if (qty > 999) return;
            if (delta > 0 && !row.IsNull("CoTheBan") && CartQuantityOf(maSanPham) + delta > row.Field<decimal>("CoTheBan"))
            {
                MessageBox.Show($"Chi nhánh chỉ còn đủ nguyên liệu cho {row.Field<decimal>("CoTheBan"):N0} phần \"{row["TenSanPham"]}\".",
                    "Không đủ nguyên liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            row["SoLuong"] = qty;
            Recalculate();
            SelectCartRow(row);
        }

        private void RemoveCartRow(DataRow row)
        {
            CommitCartEdit();
            _cartTable.Rows.Remove(row);
            Recalculate();
        }

        private void EditNote(DataRow row)
        {
            CommitCartEdit();
            using var dlg = new FormGhiChuMon(row.Field<string>("TenSanPham") ?? "", row.Field<string>("GhiChu"));
            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            row["GhiChu"] = dlg.NoteText.Length == 0 ? DBNull.Value : dlg.NoteText;
            Recalculate();
            SelectCartRow(row);
        }

        /// <summary>Cập nhật lại "còn làm được" của các món trong giỏ sau khi tải lại thực đơn.</summary>
        private void RefreshCartStockLimits()
        {
            foreach (DataRow row in _cartTable.Rows)
            {
                DataRow? product = _productTable.AsEnumerable().FirstOrDefault(p => p.Field<int>("MaSanPham") == row.Field<int>("MaSanPham"));
                row["CoTheBan"] = product == null || product.IsNull("CoTheBan") ? DBNull.Value : product["CoTheBan"];
            }
        }

        private void ShowCartRowMenu(DataRow row, int rowIndex)
        {
            mnuCartRow.Items.Clear();
            mnuCartRow.Items.Add("📝  Ghi chú món...", null, (s, e) => EditNote(row));
            mnuCartRow.Items.Add("➕  Thêm 1 phần", null, (s, e) => ChangeQuantity(row, 1));
            mnuCartRow.Items.Add("➖  Bớt 1 phần", null, (s, e) => ChangeQuantity(row, -1));
            mnuCartRow.Items.Add(new ToolStripSeparator());
            var removeItem = new ToolStripMenuItem("🗑  Xóa món khỏi đơn", null, (s, e) => RemoveCartRow(row)) { ForeColor = Danger };
            mnuCartRow.Items.Add(removeItem);

            Rectangle rect = dgvCart.GetCellDisplayRectangle(dgvCart.Columns["colMore"]!.Index, rowIndex, false);
            mnuCartRow.Show(dgvCart, new Point(rect.Right, rect.Bottom), ToolStripDropDownDirection.BelowLeft);
        }

        private void dgvCart_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            DataRow? row = CartRowAt(e.RowIndex);
            if (row == null || e.ColumnIndex < 0) return;

            switch (dgvCart.Columns[e.ColumnIndex].Name)
            {
                case "colMinus":
                    ChangeQuantity(row, -1);
                    break;
                case "colPlus":
                    ChangeQuantity(row, 1);
                    break;
                case "colMore":
                    ShowCartRowMenu(row, e.RowIndex);
                    break;
                case "colQty":
                    dgvCart.BeginEdit(true);
                    break;
            }
        }

        private void dgvCart_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.Graphics == null || dgvCart.Columns[e.ColumnIndex].Name != "colName") return;
            DataRow? row = CartRowAt(e.RowIndex);
            if (row == null) return;

            e.PaintBackground(e.CellBounds, true);
            Rectangle bounds = e.CellBounds;
            bounds.Inflate(-S(6), -S(4));
            string name = row.Field<string>("TenSanPham") ?? "";
            string note = row.Field<string>("GhiChu") ?? "";

            int y = bounds.Y;
            const TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine;
            int nameH = TextRenderer.MeasureText("Ag", _cartNameFont).Height;
            TextRenderer.DrawText(e.Graphics, name, _cartNameFont, new Rectangle(bounds.X, y, bounds.Width, nameH), TextDark, flags);
            y += nameH + S(1);

            if (note.Length > 0)
            {
                foreach (string line in note.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    string text = line.StartsWith('+') ? line : "+ " + line;
                    int h = TextRenderer.MeasureText(text, _cartNoteFont).Height;
                    TextRenderer.DrawText(e.Graphics, text, _cartNoteFont, new Rectangle(bounds.X, y, bounds.Width, h), TextMuted, flags);
                    y += h;
                }
            }
            e.Handled = true;
        }

        private void dgvCart_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvCart.Columns[e.ColumnIndex].Name != "colQty" || !dgvCart.IsCurrentCellInEditMode) return;
            if (!int.TryParse(e.FormattedValue?.ToString(), out int qty) || qty < 1 || qty > 999)
            {
                MessageBox.Show("Số lượng phải là số nguyên từ 1 đến 999.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dgvCart.CancelEdit();
            }
        }

        private void dgvCart_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            DataRow? row = CartRowAt(e.RowIndex);
            if (row == null) return;

            if (!row.IsNull("CoTheBan"))
            {
                decimal max = row.Field<decimal>("CoTheBan");
                int total = CartQuantityOf(row.Field<int>("MaSanPham"));
                if (total > max)
                {
                    int allowed = (int)Math.Max(1, row.Field<int>("SoLuong") - (total - max));
                    row["SoLuong"] = allowed;
                    MessageBox.Show($"Chi nhánh chỉ còn đủ nguyên liệu cho {max:N0} phần \"{row["TenSanPham"]}\". Đã chỉnh số lượng về {allowed}.",
                        "Không đủ nguyên liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            BeginInvoke(new Action(Recalculate));
        }

        private void dgvCart_KeyDown(object? sender, KeyEventArgs e)
        {
            DataRow? row = CartRowAt(dgvCart.CurrentCell?.RowIndex ?? -1);
            if (row == null) return;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    RemoveCartRow(row);
                    e.Handled = true;
                    break;
                case Keys.Add:
                case Keys.Oemplus:
                    ChangeQuantity(row, 1);
                    e.Handled = true;
                    break;
                case Keys.Subtract:
                case Keys.OemMinus:
                    ChangeQuantity(row, -1);
                    e.Handled = true;
                    break;
            }
        }

        private void dgvCart_Paint(object? sender, PaintEventArgs e)
        {
            if (_cartTable.Rows.Count > 0) return;
            Rectangle rect = dgvCart.ClientRectangle;
            rect.Y += dgvCart.ColumnHeadersHeight;
            rect.Height -= dgvCart.ColumnHeadersHeight;
            TextRenderer.DrawText(e.Graphics, "🛒  Chưa có món nào trong đơn\nBấm vào món ở thực đơn bên trái để thêm",
                _emptyCartFont, rect, TextMuted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
        }

        private void UpdateCartRowHeights()
        {
            int nameH = TextRenderer.MeasureText("Ag", _cartNameFont).Height;
            int lineH = TextRenderer.MeasureText("Ag", _cartNoteFont).Height;
            foreach (DataGridViewRow gridRow in dgvCart.Rows)
            {
                if (gridRow.DataBoundItem is not DataRowView drv) continue;
                string note = drv.Row.Field<string>("GhiChu") ?? "";
                int optionLines = note.Length == 0
                    ? 0
                    : note.Split(new[] { '\n', ';' }, StringSplitOptions.RemoveEmptyEntries).Length;
                int height = S(8) + nameH + S(2) + optionLines * lineH + S(6);
                height = Math.Max(S(40), height);
                if (gridRow.Height != height) gridRow.Height = height;
            }
        }

        #endregion

        #region ======================== KHÁCH HÀNG / LOẠI ĐƠN / THANH TOÁN ========================

        private string SelectedSourceCode => cboSource.SelectedIndex >= 0 ? OrderSources[cboSource.SelectedIndex].Code : "POS";

        private bool IsPosSource => SelectedSourceCode == "POS";

        private void FindCustomer(bool silent)
        {
            if (!UserSession.HasPermission(PermissionCodes.PosCustomer))
            {
                if (!silent) DemandPermission(PermissionCodes.PosCustomer);
                return;
            }
            string phone = new string(txtCustomerPhone.Text.Where(char.IsDigit).ToArray());
            if (phone.Length == 0)
            {
                if (!silent)
                    MessageBox.Show("Vui lòng nhập số điện thoại khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCustomerPhone.Focus();
                return;
            }

            try
            {
                var dt = _orderService.FindCustomerByPhone(phone);

                if (dt.Rows.Count == 0)
                {
                    SetCustomer(null);
                    if (!silent)
                        MessageBox.Show($"Không tìm thấy khách hàng thân thiết có SĐT {phone}.\nĐơn sẽ được tính là khách lẻ.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!dt.Rows[0].Field<bool>("DangHoatDong"))
                {
                    SetCustomer(null);
                    MessageBox.Show("Thẻ khách hàng này đã ngưng sử dụng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                SetCustomer(dt.Rows[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tra cứu khách hàng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetCustomer(DataRow? customer)
        {
            if (customer == null)
            {
                _maKhachHang = null;
                _tenKhachHang = "";
                _diemKhachHang = 0;
                _phanTramGiamHang = 0;
                _customerInfoText = "Khách lẻ — nhập SĐT để tích điểm";
                lblCustomerInfo.ForeColor = TextMuted;
            }
            else
            {
                _maKhachHang = customer.Field<int>("MaKhachHang");
                _tenKhachHang = customer.Field<string>("HoTen") ?? "";
                _diemKhachHang = customer.Field<int>("DiemHienTai");
                _phanTramGiamHang = customer.Field<decimal>("PhanTramGiam");
                txtCustomerPhone.Text = customer.Field<string>("SoDienThoai");
                string giam = _phanTramGiamHang > 0 ? $" (giảm {_phanTramGiamHang:0.##}%)" : "";
                _customerInfoText = $"⭐ {_tenKhachHang}  •  Hạng {customer["TenHang"]}{giam}  •  {_diemKhachHang:N0} điểm";
                lblCustomerInfo.ForeColor = AmberDark;
            }

            _isRecalculating = true;
            numRedeemPoints.Value = 0;
            numRedeemPoints.Enabled = _maKhachHang.HasValue;
            _isRecalculating = false;
            Recalculate();
        }

        private void SetOrderType(string orderType)
        {
            bool wasDelivery = _orderType == "GiaoHang";
            _orderType = orderType;
            bool delivery = orderType == "GiaoHang";
            bool atTable = orderType == "TaiQuay";

            SetToggleState(btnTypeTaiQuay, atTable);
            SetToggleState(btnTypeMangDi, orderType == "MangDi");
            SetToggleState(btnTypeGiaoHang, delivery);

            if (flpTables != null)
            {
                flpTables.Visible = atTable && IsPosSource;
                if (!atTable) SetTableNo(null);
                else if (!_tableNo.HasValue) SetTableNo(1); // mặc định Bàn 1
            }

            if (delivery && !wasDelivery && IsPosSource)
                numShipFee.Value = PhiShipNoiBoMacDinh;
            tlpDelivery.Visible = delivery;
            numShipFee.Enabled = delivery && IsPosSource;
            Recalculate();
        }

        private void OnSourceChanged()
        {
            bool isApp = !IsPosSource;
            foreach (var btn in new[] { btnTypeTaiQuay, btnTypeMangDi, btnTypeGiaoHang, btnPayTienMat, btnPayChuyenKhoan, btnPayThe, btnPayVi })
                btn.Enabled = !isApp;
            foreach (var rb in _payRadios)
                rb.Enabled = !isApp;
            if (flpTables != null)
                foreach (Control c in flpTables.Controls) c.Enabled = !isApp;

            if (isApp)
            {
                // Đơn app: luôn giao hàng, app thu tiền khách (ví), phí ship khách trả cho app
                numShipFee.Value = 0;
                SetOrderType("GiaoHang");
                string appPay = SelectedSourceCode switch
                {
                    "Grab" => "Grab",
                    "ShopeeFood" => "ShopeePay",
                    _ => "Momo"
                };
                SetPaymentMethod(appPay);
                txtPaymentRef.PlaceholderText = "Mã đơn trên " + SourceText(SelectedSourceCode);
            }
            else
            {
                if (_orderType == "GiaoHang") numShipFee.Value = PhiShipNoiBoMacDinh;
                // Quầy POS: mặc định mang về nếu đang bị khóa ở Giao từ app
                if (_orderType == "GiaoHang") SetOrderType("MangDi");
                else SetOrderType(_orderType);
                SetPaymentMethod("TienMat");
                txtPaymentRef.PlaceholderText = "Mã giao dịch / 4 số cuối thẻ (nếu có)";
            }
        }

        private void SetPaymentMethod(string code)
        {
            _paymentMethod = code;
            SetToggleState(btnPayTienMat, code == "TienMat");
            SetToggleState(btnPayChuyenKhoan, code == "ChuyenKhoan");
            SetToggleState(btnPayThe, code == "The");
            SetToggleState(btnPayVi, PaymentDbCode(code) == "Vi");

            foreach (var rb in _payRadios)
            {
                bool on = string.Equals((string?)rb.Tag, code, StringComparison.Ordinal);
                if (rb.Checked != on) rb.Checked = on;
            }

            bool cash = code == "TienMat";
            lblMoneyCaption.Text = cash ? "Khách đưa" : "Mã GD";
            txtCustomerMoney.Visible = cash;
            txtPaymentRef.Visible = !cash;
            lblChange.Visible = cash;
            if (txtChangeBox != null) txtChangeBox.Enabled = cash;
            flpQuickCash.Enabled = cash;
            if (!cash) txtCustomerMoney.Clear();
            if (cash) txtPaymentRef.Clear();
            CalculateChange();
        }

        private void OpenPromoDialog()
        {
            CommitCartEdit();
            Recalculate();
            decimal nen = _cartTable.AsEnumerable()
                .Where(r => r.Field<string>("LoaiSanPham") != "Combo")
                .Sum(r => r.Field<decimal>("DonGia") * r.Field<int>("SoLuong"));

            using var dlg = new FormChonKhuyenMai(_promoTable, nen, _maKhuyenMai, IsPosSource);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _maKhuyenMai = dlg.SelectedPromoId;
                Recalculate();
            }
        }

        #endregion

        #region ======================== TÍNH TIỀN ========================

        /// <summary>
        /// Phân bổ tổng giảm (KM + hạng + điểm) theo tỷ lệ tiền dòng → cột Giảm giá / Thành tiền.
        /// </summary>
        private void AllocateLineDiscounts(decimal tongGiam)
        {
            if (_cartTable.Rows.Count == 0) return;

            decimal tienHang = 0;
            foreach (DataRow row in _cartTable.Rows)
                tienHang += row.Field<decimal>("DonGia") * row.Field<int>("SoLuong");

            if (tongGiam <= 0 || tienHang <= 0)
            {
                foreach (DataRow row in _cartTable.Rows)
                {
                    decimal line = row.Field<decimal>("DonGia") * row.Field<int>("SoLuong");
                    row["GiamDong"] = 0m;
                    row["ThanhTien"] = line;
                }
                return;
            }

            decimal allocated = 0;
            for (int i = 0; i < _cartTable.Rows.Count; i++)
            {
                DataRow row = _cartTable.Rows[i];
                decimal line = row.Field<decimal>("DonGia") * row.Field<int>("SoLuong");
                decimal giam = i == _cartTable.Rows.Count - 1
                    ? Math.Max(0, tongGiam - allocated)   // dòng cuối nhận phần dư
                    : RoundDownThousand(tongGiam * line / tienHang);
                if (giam > line) giam = line;
                allocated += giam;
                row["GiamDong"] = giam;
                row["ThanhTien"] = line - giam;
            }
        }

        private void Recalculate()
        {
            if (_isRecalculating) return;
            _isRecalculating = true;
            try
            {
                _tienHang = 0;
                decimal nen = 0;   // tiền hàng không tính combo — nền tính khuyến mãi và giảm hạng
                int soMon = 0;
                foreach (DataRow row in _cartTable.Rows)
                {
                    decimal line = row.Field<decimal>("DonGia") * row.Field<int>("SoLuong");
                    _tienHang += line;
                    soMon += row.Field<int>("SoLuong");
                    // OOP: hành vi nền KM theo subclass (ComboItem.TinhVaoNenKhuyenMai = false)
                    var monKm = SanPhamBanHangFactory.Create(
                        row.Field<int>("MaSanPham"),
                        row.Field<string>("TenSanPham") ?? "",
                        row.Field<string>("LoaiSanPham") ?? "",
                        "",
                        row.Field<decimal>("DonGia"),
                        null);
                    if (monKm.TinhVaoNenKhuyenMai) nen += line;
                }

                // 1. Khuyến mãi: chỉ đơn tại quầy, tối đa 1 chương trình
                _tienGiamKhuyenMai = 0;
                string promoNote = "";
                DataRow? promo = _maKhuyenMai.HasValue
                    ? _promoTable.AsEnumerable().FirstOrDefault(p => p.Field<int>("MaKhuyenMai") == _maKhuyenMai.Value)
                    : null;
                if (_maKhuyenMai.HasValue && promo == null) _maKhuyenMai = null;
                if (promo != null)
                {
                    if (!IsPosSource) promoNote = "chỉ áp dụng đơn tại quầy";
                    else if (nen < promo.Field<decimal>("DonToiThieu")) promoNote = $"cần đơn từ {Money(promo.Field<decimal>("DonToiThieu"))}";
                    else _tienGiamKhuyenMai = CalculatePromoDiscount(promo, nen);
                }

                // 2. Giảm theo hạng thành viên, 3. Đổi điểm
                _tienGiamHang = _maKhachHang.HasValue ? RoundDownThousand(Math.Max(0, nen - _tienGiamKhuyenMai) * _phanTramGiamHang / 100m) : 0;
                decimal truocDiem = _tienHang - _tienGiamKhuyenMai - _tienGiamHang;
                int maxPoints = _maKhachHang.HasValue
                    ? (int)Math.Max(0, Math.Min(_diemKhachHang, Math.Floor(truocDiem / TienMotDiemDoi)))
                    : 0;
                numRedeemPoints.Maximum = maxPoints;
                _diemTru = (int)numRedeemPoints.Value;
                _tienDoiDiem = _diemTru * TienMotDiemDoi;

                // Phân bổ giảm giá xuống từng dòng (hiển thị cột Giảm giá / Thành tiền)
                AllocateLineDiscounts(_tienGiamKhuyenMai + _tienGiamHang + _tienDoiDiem);

                _phiShip = _orderType == "GiaoHang" && IsPosSource ? numShipFee.Value : 0;
                _tongThanhToan = Math.Max(0, _tienHang - _tienGiamKhuyenMai - _tienGiamHang - _tienDoiDiem + _phiShip);
                _diemCong = _maKhachHang.HasValue ? (int)Math.Floor((_tongThanhToan - _phiShip) / TienTichMotDiem) : 0;

                // ----- Hiển thị -----
                decimal tongGiam = _tienGiamKhuyenMai + _tienGiamHang + _tienDoiDiem;
                lblSubtotal.Text = Money(_tienHang);
                lblDiscount.Text = tongGiam > 0 ? "-" + Money(tongGiam) : "0 đ";
                var kinds = new List<string>();
                var details = new List<string>();
                if (_tienGiamKhuyenMai > 0) { kinds.Add("KM"); details.Add("Khuyến mãi: -" + Money(_tienGiamKhuyenMai)); }
                if (_tienGiamHang > 0) { kinds.Add("hạng"); details.Add($"Hạng thành viên {_phanTramGiamHang:0.##}%: -" + Money(_tienGiamHang)); }
                if (_tienDoiDiem > 0) { kinds.Add("điểm"); details.Add($"Đổi {_diemTru:N0} điểm: -" + Money(_tienDoiDiem)); }
                lblDiscountCaption.Text = kinds.Count > 0 ? $"Giảm ({string.Join(", ", kinds)})" : "Giảm giá";
                string discountTip = details.Count > 0 ? string.Join(Environment.NewLine, details) : "Chưa có giảm giá";
                ttpOrder.SetToolTip(lblDiscountCaption, discountTip);
                ttpOrder.SetToolTip(lblDiscount, discountTip);
                lblShip.Text = Money(_phiShip);
                lblShipCaption.Visible = lblShip.Visible = _phiShip > 0;
                lblFinal.Text = Money(_tongThanhToan);
                SyncBillBoxesFromLabels();

                if (promo == null)
                {
                    btnPromo.Text = _promoTable.Rows.Count == 0 ? "🎁  Không có KM" : "🎁  Khuyến mại";
                    btnPromo.ForeColor = Color.White;
                }
                else if (_tienGiamKhuyenMai > 0)
                {
                    btnPromo.Text = $"🎁  {promo["TenKhuyenMai"]}";
                    btnPromo.ForeColor = Color.White;
                }
                else
                {
                    btnPromo.Text = $"🎁  ⚠ {promo["TenKhuyenMai"]}";
                    btnPromo.ForeColor = Color.White;
                }
                ttpOrder.SetToolTip(btnPromo, (promo == null ? "Chọn khuyến mãi" : promo["TenKhuyenMai"]?.ToString()) + "  —  F4");

                lblRedeemHint.Text = _maKhachHang.HasValue ? $"/{maxPoints:N0}" : "";
                ttpOrder.SetToolTip(numRedeemPoints, _maKhachHang.HasValue
                    ? $"Đổi tối đa {maxPoints:N0} điểm cho đơn này (1 điểm = 1.000 đ)"
                    : "Cần khách thành viên để đổi điểm");
                lblCustomerInfo.Text = _maKhachHang.HasValue && _diemCong > 0
                    ? $"{_customerInfoText}  •  đơn này +{_diemCong:N0}"
                    : _customerInfoText;
                ttpOrder.SetToolTip(lblCustomerInfo, lblCustomerInfo.Text);

                dgvCart.Columns["colName"]!.HeaderText = soMon > 0 ? $"Tên món  ({soMon} món)" : "Tên món";
                bool hasItems = _cartTable.Rows.Count > 0;
                btnCheckout.Enabled = hasItems && UserSession.HasPermission(PermissionCodes.PosPayment);
                btnClearOrder.Enabled = hasItems;
                btnParkOrder.Enabled = hasItems;

                UpdateCartRowHeights();
                UpdateQuickCash();
                CalculateChange();
                dgvCart.Invalidate();
            }
            finally
            {
                _isRecalculating = false;
            }
        }

        private void UpdateQuickCash()
        {
            if (_tongThanhToan == _lastQuickCashTotal) return;
            _lastQuickCashTotal = _tongThanhToan;

            flpQuickCash.SuspendLayout();
            while (flpQuickCash.Controls.Count > 0)
                flpQuickCash.Controls[0].Dispose();

            if (_tongThanhToan > 0)
            {
                var amounts = new SortedSet<decimal> { _tongThanhToan };
                foreach (decimal step in new[] { 10000m, 50000m, 100000m })
                    amounts.Add(Math.Ceiling(_tongThanhToan / step) * step);
                foreach (decimal note in new[] { 200000m, 500000m })
                    if (note > _tongThanhToan) amounts.Add(note);

                foreach (decimal amount in amounts.Take(5))
                {
                    var btn = new Button
                    {
                        Text = amount == _tongThanhToan ? "Đủ tiền" : (amount / 1000m).ToString("N0", _vnCulture) + "k",
                        AutoSize = true,
                        AutoSizeMode = AutoSizeMode.GrowAndShrink,
                        FlatStyle = FlatStyle.Flat,
                        BackColor = Color.White,
                        ForeColor = AmberDark,
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                        Margin = new Padding(S(2), 0, 0, 0),
                        Padding = new Padding(S(2), 0, S(2), 0),
                        Cursor = Cursors.Hand,
                        TabStop = false
                    };
                    btn.FlatAppearance.BorderColor = Amber;
                    btn.Click += (s, e) => txtCustomerMoney.Text = amount.ToString("N0", _vnCulture);
                    flpQuickCash.Controls.Add(btn);
                }
            }
            flpQuickCash.ResumeLayout();
        }

        private static bool TryParseMoney(string text, out decimal value)
        {
            string digits = new string(text.Where(char.IsDigit).ToArray());
            return decimal.TryParse(digits, NumberStyles.None, CultureInfo.InvariantCulture, out value);
        }

        private void CalculateChange()
        {
            if (_paymentMethod != "TienMat")
            {
                lblChange.Text = "";
                if (txtChangeBox != null) txtChangeBox.Text = "0";
                return;
            }
            if (string.IsNullOrWhiteSpace(txtCustomerMoney.Text))
            {
                lblChange.Text = "Thừa 0 đ";
                lblChange.ForeColor = TextMuted;
                if (txtChangeBox != null) { txtChangeBox.Text = "0"; txtChangeBox.ForeColor = TextMuted; }
                return;
            }
            if (!TryParseMoney(txtCustomerMoney.Text, out decimal given))
            {
                lblChange.Text = "Không hợp lệ";
                lblChange.ForeColor = Danger;
                if (txtChangeBox != null) { txtChangeBox.Text = "?"; txtChangeBox.ForeColor = Danger; }
                return;
            }

            decimal change = given - _tongThanhToan;
            lblChange.Text = change >= 0 ? "Thừa " + Money(change) : "Thiếu " + Money(-change);
            lblChange.ForeColor = change >= 0 ? Success : Danger;
            if (txtChangeBox != null)
            {
                txtChangeBox.Text = Math.Abs(change).ToString("N0", _vnCulture);
                txtChangeBox.ForeColor = change >= 0 ? Success : Danger;
            }
        }

        #endregion

        #region ======================== ĐƠN CHỜ / XÓA ĐƠN ========================

        private void ResetOrder()
        {
            _cartTable.Rows.Clear();
            _maKhuyenMai = null;
            txtCustomerPhone.Clear();
            txtAddress.Clear();
            txtPaymentRef.Clear();
            txtCustomerMoney.Clear();
            txtVoucherCode?.Clear();
            txtBillNote?.Clear();
            if (chkEInvoice != null) chkEInvoice.Checked = false;

            if (cboSource.SelectedIndex != 0)
                cboSource.SelectedIndex = 0;   // gọi OnSourceChanged → bật lại các nút
            SetTableNo(null);
            SetOrderType("MangDi");
            SetPaymentMethod("TienMat");
            SetCustomer(null);
        }

        private void ClearOrderWithConfirm()
        {
            if (_cartTable.Rows.Count == 0) return;
            if (MessageBox.Show("Xóa toàn bộ đơn đang order?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                ResetOrder();
        }

        private void ParkCurrentOrder(bool showMessage)
        {
            CommitCartEdit();
            if (_cartTable.Rows.Count == 0)
            {
                if (showMessage)
                    MessageBox.Show("Đơn đang trống, không có gì để cất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _parkedOrders.Add(new ParkedOrder
            {
                Time = DateTime.Now,
                Items = _cartTable.Copy(),
                CustomerPhone = _maKhachHang.HasValue ? txtCustomerPhone.Text.Trim() : "",
                CustomerName = _tenKhachHang,
                Source = SelectedSourceCode,
                OrderType = _orderType,
                TableNo = _orderType == "TaiQuay" ? _tableNo : null,
                Address = txtAddress.Text,
                ShipFee = numShipFee.Value,
                PromoId = _maKhuyenMai,
                Total = _tongThanhToan
            });
            ResetOrder();
            UpdateParkedButton();
        }

        private void UpdateParkedButton()
        {
            btnOrders.Text = _parkedOrders.Count > 0 ? $"Đơn hàng ({_parkedOrders.Count})" : "Đơn hàng";
            btnOrders.BackColor = _parkedOrders.Count > 0 ? Color.FromArgb(52, 152, 219) : AmberDark;
            btnOrders.FlatAppearance.MouseOverBackColor = Blend(btnOrders.BackColor, Color.Black, 0.08f);
        }

        private void ShowMainMenu()
        {
            EnsureNavFlyout();
            if (_navFlyout!.Visible)
                HideNavFlyout();
            else
                OpenNavFlyout();
        }

        private void OpenNavFlyout()
        {
            EnsureNavFlyout();
            _navFlyout!.Visible = true;
            _navFlyout.Width = S(220);
            // Dock Left + tlpBody Dock Fill → UI bán hàng tự thu sang phải
            PerformLayout();
        }

        private void HideNavFlyout()
        {
            if (_navFlyout == null) return;
            _navFlyout.Visible = false;
            PerformLayout();
        }

        private void EnsureNavFlyout()
        {
            if (_navFlyout != null) return;

            _navFlyout = new Panel
            {
                Name = "pnlNavSidebar",
                Dock = DockStyle.Left,
                Width = S(220),
                BackColor = NavSidebar,
                Visible = false
            };

            // Thứ tự giống sidebar FormMain; null = đang ở Order (highlight cam)
            var items = new (string Text, int? Tab, bool Visible)[]
            {
                ("🛒  BÁN HÀNG", null, true),
                ("🏠  Tổng Quan", 0, true),
                ("🥪  Thực Đơn (Món)", 1, HasAny(PermissionCodes.MenuCategory, PermissionCodes.MenuProduct, PermissionCodes.MenuUnit, PermissionCodes.MenuPrice)),
                ("👤  Khách Hàng", 2, UserSession.HasPermission(PermissionCodes.PosCustomer)),
                ("🎁  Chương Trình KM", 3, UserSession.HasPermission(PermissionCodes.MenuPromotion)),
                ("🥬  Kho Nguyên Liệu", 4, HasAny(PermissionCodes.InventoryMaterial, PermissionCodes.InventoryWarning)),
                ("📊  Báo Cáo Doanh Thu", 5, HasAny(PermissionCodes.ReportRevenue, PermissionCodes.ReportTopProduct,
                    PermissionCodes.ReportStock, PermissionCodes.ReportChart, PermissionCodes.ReportExport)),
                ("⚙  Quản Trị", 6, HasAny("SYS_ACCOUNT", "SYS_ROLE", "SYS_BRANCH", "SYS_SHIFT"))
            };

            static bool HasAny(params string[] codes) => codes.Any(UserSession.HasPermission);

            // Dock.Top: thêm sau = nằm trên → thêm ngược
            for (int i = items.Length - 1; i >= 0; i--)
            {
                var item = items[i];
                if (!item.Visible) continue;

                bool isPos = item.Tab == null;
                var btn = new Button
                {
                    Dock = DockStyle.Top,
                    Height = S(55),
                    Text = item.Text,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(S(15), 0, 0, 0),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Font = new Font("Segoe UI", 11F, isPos ? FontStyle.Bold : FontStyle.Regular),
                    ForeColor = Color.White,
                    BackColor = isPos ? NavActive : NavSidebar,
                    Tag = item.Tab
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = isPos
                    ? Blend(NavActive, Color.Black, 0.08f)
                    : Color.FromArgb(52, 73, 94);

                int? tab = item.Tab;
                btn.Click += (s, e) =>
                {
                    if (tab is int t)
                        OpenAdminModule(t);
                    else
                        HideNavFlyout(); // BÁN HÀNG = đóng sidebar, ở lại Order
                };

                _navFlyout.Controls.Add(btn);
            }

            // Thêm sau tlpBody: Dock Left chiếm trái, tlpBody (Fill) thu lại
            Controls.Add(_navFlyout);
            _navFlyout.BringToFront();
        }

        private void OpenAdminModule(int tabIndex)
        {
            if (!ConfirmDiscardPendingOrders("Chuyển sang quản lý")) return;
            RequestedAdminTab = tabIndex;
            _skipCloseConfirm = true;
            Close();
        }

        private void ShowOrdersMenu()
        {
            mnuOrders.Items.Clear();

            var parked = new ToolStripMenuItem($"Đơn chờ ({_parkedOrders.Count})");
            if (_parkedOrders.Count == 0)
            {
                parked.DropDownItems.Add(new ToolStripMenuItem("Chưa có đơn nào đang chờ") { Enabled = false });
            }
            else
            {
                for (int i = 0; i < _parkedOrders.Count; i++)
                {
                    ParkedOrder p = _parkedOrders[i];
                    int qty = p.Items.AsEnumerable().Sum(r => r.Field<int>("SoLuong"));
                    string text = $"#{i + 1}   {p.Time:HH:mm}   •   {qty} món   •   {Money(p.Total)}";
                    if (p.CustomerName.Length > 0) text += "   •   " + p.CustomerName;
                    if (p.OrderType == "GiaoHang") text += "   •   Giao · " + SourceText(p.Source);
                    else if (p.OrderType == "TaiQuay" && p.TableNo.HasValue) text += $"   •   Bàn {p.TableNo}";
                    else if (p.OrderType == "MangDi") text += "   •   Mang về";
                    parked.DropDownItems.Add(text, null, (s, e) => RestoreParkedOrder(p));
                }
                parked.DropDownItems.Add(new ToolStripSeparator());
                var clearAll = new ToolStripMenuItem("Xóa tất cả đơn chờ") { ForeColor = Danger };
                clearAll.Click += (s, e) =>
                {
                    if (MessageBox.Show($"Xóa {_parkedOrders.Count} đơn đang chờ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                    _parkedOrders.Clear();
                    UpdateParkedButton();
                };
                parked.DropDownItems.Add(clearAll);
            }
            mnuOrders.Items.Add(parked);
            if (UserSession.HasPermission(PermissionCodes.PosSearch))
                mnuOrders.Items.Add("Đơn trong ngày", null, (s, e) => OpenTodayOrders());
            mnuOrders.Show(btnOrders, new Point(0, btnOrders.Height));
        }

        private void ShowSettingsMenu()
        {
            mnuSettings.Items.Clear();
            mnuSettings.Items.Add(new ToolStripMenuItem(UserSession.FullName) { Enabled = false });
            if (_tenChiNhanh.Length > 0)
                mnuSettings.Items.Add(new ToolStripMenuItem(_tenChiNhanh) { Enabled = false });
            if (_shiftText.Length > 0)
                mnuSettings.Items.Add(new ToolStripMenuItem(_shiftText) { Enabled = false });
            mnuSettings.Items.Add(new ToolStripSeparator());
            var logout = new ToolStripMenuItem("Đăng xuất") { ForeColor = Danger };
            logout.Click += (s, e) => Logout();
            mnuSettings.Items.Add(logout);
            mnuSettings.Show(btnSettings, new Point(0, btnSettings.Height));
        }

        private void ShowParkedMenu() => ShowOrdersMenu();

        private void RestoreParkedOrder(ParkedOrder parked)
        {
            if (_cartTable.Rows.Count > 0)
            {
                if (MessageBox.Show("Đơn đang order sẽ được cất vào danh sách chờ để mở đơn đã chọn. Tiếp tục?",
                        "Mở đơn chờ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                ParkCurrentOrder(false);
            }

            _parkedOrders.Remove(parked);
            ResetOrder();

            foreach (DataRow r in parked.Items.Rows)
                _cartTable.ImportRow(r);
            RefreshCartStockLimits();

            int sourceIndex = Array.FindIndex(OrderSources, s => s.Code == parked.Source);
            cboSource.SelectedIndex = Math.Max(0, sourceIndex);
            SetOrderType(parked.OrderType);
            if (parked.OrderType == "TaiQuay") SetTableNo(parked.TableNo ?? 1);
            txtAddress.Text = parked.Address;
            numShipFee.Value = Math.Min(numShipFee.Maximum, parked.ShipFee);
            _maKhuyenMai = parked.PromoId;
            if (parked.CustomerPhone.Length > 0)
            {
                txtCustomerPhone.Text = parked.CustomerPhone;
                FindCustomer(true);
            }

            UpdateParkedButton();
            Recalculate();
        }

        #endregion

        #region ======================== THANH TOÁN & LƯU HÓA ĐƠN ========================

        private void Checkout()
        {
            if (!DemandPermission(PermissionCodes.PosPayment)) return;
            CommitCartEdit();
            Recalculate();

            if (_cartTable.Rows.Count == 0)
            {
                MessageBox.Show("Đơn hàng đang trống! Vui lòng chọn món.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string loaiDon = _orderType;
            string diaChi = txtAddress.Text.Trim();
            if (loaiDon == "TaiQuay")
            {
                if (!_tableNo.HasValue)
                {
                    MessageBox.Show("Chọn bàn (1–4) cho đơn tại bàn vỉa hè.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                diaChi = $"Bàn {_tableNo}";
            }
            if (loaiDon == "GiaoHang" && diaChi.Length == 0)
            {
                MessageBox.Show("Đơn giao hàng cần nhập địa chỉ giao.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return;
            }

            decimal tienKhachDua = _tongThanhToan;
            if (_paymentMethod == "TienMat" && !string.IsNullOrWhiteSpace(txtCustomerMoney.Text))
            {
                if (!TryParseMoney(txtCustomerMoney.Text, out tienKhachDua))
                {
                    MessageBox.Show("Số tiền khách đưa không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerMoney.Focus();
                    return;
                }
                if (tienKhachDua < _tongThanhToan)
                {
                    MessageBox.Show($"Khách đưa chưa đủ tiền! Còn thiếu {Money(_tongThanhToan - tienKhachDua)}.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtCustomerMoney.Focus();
                    return;
                }
            }

            int soMon = _cartTable.AsEnumerable().Sum(r => r.Field<int>("SoLuong"));
            string nguonDon = SelectedSourceCode;
            var confirm = new StringBuilder();
            string typeLine = OrderTypeText(loaiDon);
            if (loaiDon == "TaiQuay" && _tableNo.HasValue) typeLine += $" {_tableNo}";
            confirm.AppendLine($"{soMon} món  •  {typeLine}  •  {SourceText(nguonDon)}");
            if (_maKhachHang.HasValue) confirm.AppendLine($"Khách: {_tenKhachHang}");
            confirm.AppendLine();
            confirm.AppendLine($"KHÁCH PHẢI TRẢ:  {Money(_tongThanhToan)}");
            confirm.AppendLine($"Hình thức:  {PaymentText(_paymentMethod)}");
            if (_paymentMethod == "TienMat" && tienKhachDua > _tongThanhToan)
                confirm.AppendLine($"Khách đưa {Money(tienKhachDua)}  →  Thối lại {Money(tienKhachDua - _tongThanhToan)}");

            if (MessageBox.Show(confirm.ToString(), "Xác nhận thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            int? maKhuyenMai = _tienGiamKhuyenMai > 0 ? _maKhuyenMai : null;
            int maHoaDon;
            string maDon;
            try
            {
                string payRef = txtPaymentRef.Text.Trim();
                if (PaymentDbCode(_paymentMethod) == "Vi" && payRef.Length == 0)
                    payRef = PaymentText(_paymentMethod);
                else if (PaymentDbCode(_paymentMethod) == "Vi" && !payRef.Contains(PaymentText(_paymentMethod), StringComparison.OrdinalIgnoreCase))
                    payRef = PaymentText(_paymentMethod) + " · " + payRef;
                var checkout = _orderService.Checkout(new CheckoutRequest
                {
                    MaChiNhanh = _maChiNhanh,
                    MaCodeChiNhanh = _maCodeChiNhanh,
                    MaThuNgan = UserSession.EmployeeID,
                    MaKhachHang = _maKhachHang,
                    MaKhuyenMai = maKhuyenMai,
                    NguonDon = nguonDon,
                    LoaiDon = loaiDon,
                    DiaChiGiao = diaChi,
                    HinhThucThanhToan = PaymentDbCode(_paymentMethod),
                    MaThamChieu = payRef,
                    TienHang = _tienHang,
                    TienGiam = (maKhuyenMai.HasValue ? _tienGiamKhuyenMai : 0) + _tienGiamHang + _tienDoiDiem,
                    PhiShip = _phiShip,
                    TongThanhToan = _tongThanhToan,
                    DiemCong = _diemCong,
                    DiemTru = _diemTru,
                    CartTable = _cartTable
                }, _vnCulture);
                maHoaDon = checkout.MaHoaDon;
                maDon = checkout.MaDon;
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Không thể thanh toán", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LoadProducts();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            decimal tienThoi = tienKhachDua - _tongThanhToan;
            var done = new StringBuilder();
            done.AppendLine($"Đã thanh toán đơn {maDon}");
            done.AppendLine();
            done.AppendLine($"Tổng tiền:  {Money(_tongThanhToan)}");
            if (_paymentMethod == "TienMat")
                done.AppendLine($"Tiền thối lại:  {Money(tienThoi)}");
            if (_maKhachHang.HasValue)
                done.AppendLine($"Điểm tích lũy:  +{_diemCong:N0}" + (_diemTru > 0 ? $"  /  -{_diemTru:N0}" : ""));
            MessageBox.Show(done.ToString(), "✅ Thanh toán thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (chkPrintReceipt.Checked && UserSession.HasPermission(PermissionCodes.PosPrint))
                ReceiptPrinter.ShowPreview(this, maHoaDon, _paymentMethod == "TienMat" ? tienKhachDua : null, _paymentMethod == "TienMat" ? tienThoi : null);

            ResetOrder();
            LoadProducts();
            LoadShiftInfo();
        }

        #endregion

        #region ======================== HEADER: LỊCH SỬ / ĐĂNG XUẤT / PHÍM TẮT ========================

        private void OpenTodayOrders()
        {
            if (!DemandPermission(PermissionCodes.PosSearch)) return;
            using var dlg = new FormDonHomNay(_maChiNhanh, _tenChiNhanh);
            dlg.ShowDialog(this);
        }

        private bool ConfirmDiscardPendingOrders(string action)
        {
            CommitCartEdit();
            if (_cartTable.Rows.Count == 0 && _parkedOrders.Count == 0) return true;

            var parts = new List<string>();
            if (_cartTable.Rows.Count > 0) parts.Add("đơn đang order");
            if (_parkedOrders.Count > 0) parts.Add($"{_parkedOrders.Count} đơn chờ");
            return MessageBox.Show($"Còn {string.Join(" và ", parts)} chưa thanh toán sẽ bị mất. {action}?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes;
        }

        private void Logout()
        {
            if (!ConfirmDiscardPendingOrders("Đăng xuất")) return;
            IsLoggedOut = true;
            _skipCloseConfirm = true;
            Close();
        }

        private void FormOrder_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_skipCloseConfirm || e.CloseReason != CloseReason.UserClosing) return;
            if (!ConfirmDiscardPendingOrders("Thoát chương trình"))
                e.Cancel = true;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape && _navFlyout != null && _navFlyout.Visible)
            {
                HideNavFlyout();
                return true;
            }

            switch (keyData)
            {
                case Keys.F3:
                    if (!DemandPermission(PermissionCodes.PosCustomer)) return true;
                    txtCustomerPhone.Focus();
                    txtCustomerPhone.SelectAll();
                    return true;
                case Keys.F4:
                    OpenPromoDialog();
                    return true;
                case Keys.F8:
                    ParkCurrentOrder(true);
                    return true;
                case Keys.F9:
                    Checkout();
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}
