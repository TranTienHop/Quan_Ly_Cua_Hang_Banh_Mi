using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using System.Data;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormDieuChuyen : FrmBase
    {
        // ----- tab Lập phiếu -----
        private readonly DataTable _ct = new();                  // các dòng của phiếu đang lập
        private bool _dangNap;                                   // chặn sự kiện khi đang nạp dữ liệu
        private int _khoGuiCu;                                   // để hoàn lại khi người dùng từ chối đổi kho gửi

        // ----- tab Danh sách -----
        private int? _maPhieu;                                   // phiếu đang chọn
        private string _tt = "";
        private int _khoGui, _khoNhan;
        private DataTable? _ctPhieu;

        private int MaKhoGui => cboKhoGui.SelectedValue is int v ? v : Session.MaChiNhanh;

        /// <summary>null = ADMIN (không giới hạn); còn lại chỉ thao tác cho chi nhánh của mình.</summary>
        private static int? GioiHanCN => Session.LaAdmin ? null : Session.MaChiNhanh;

        public FormDieuChuyen()
        {
            InitializeComponent();

            // ----- giao diện -----
            foreach (var tp in new[] { tpLap, tpDS })
            {
                tp.UseVisualStyleBackColor = false;
                tp.BackColor = Theme.Background;
            }
            foreach (var g in new[] { dgvCT, dgvPhieu, dgvCTPhieu }) g.ApplyTheme();
            dgvCT.ReadOnly = false;                       // sửa số lượng gửi ngay trong lưới
            dgvCTPhieu.ReadOnly = false;                  // sửa số lượng thực nhận (chỉ khi đang chuyển)
            btnLuu.StylePrimary(); btnThemDong.StylePrimary(); btnNhanHang.StylePrimary(); btnGuiHang.StylePrimary();
            btnXoaDong.StyleSecondary(); btnLamMoi.StyleSecondary(); btnLamMoiDS.StyleSecondary();
            btnHuyPhieu.StyleSecondary();
            TrangThaiUi.Hook(dgvPhieu, "TrangThai");
            lblTon.ForeColor = Theme.Muted;
            lblDV.ForeColor = Theme.Muted;
            lblTong.ForeColor = Theme.Muted;
            lblTongPhieu.ForeColor = Theme.Muted;
            lblChiTiet.ForeColor = Theme.Text;
            KeyPreview = true;

            txtSoPhieu.ReadOnly = txtNgay.ReadOnly = txtNguoi.ReadOnly = true;
            txtSoPhieu.Text = "(Tự động)";
            txtNgay.Text = "(Tự động: lúc lưu)";
            cboKhoGui.DropDownStyle = ComboBoxStyle.DropDownList;
            cboKhoNhan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocTT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocTT.Items.AddRange(new object[] { "Tất cả", "Chờ gửi", "Đang chuyển", "Đã nhận", "Đã hủy" });
            cboLocTT.SelectedIndex = 0;

            XayBangDong();

            // ----- sự kiện tab Lập phiếu -----
            cboKhoGui.SelectedIndexChanged += (_, _) => KhoGuiThayDoi();
            cboKhoNhan.SelectedIndexChanged += (_, _) => Err.SetError(cboKhoNhan, "");
            cboNL.SelectedIndexChanged += (_, _) => NLThayDoi();
            btnThemDong.Click += (_, _) => ThemDong();
            btnXoaDong.Click += (_, _) => XoaDong();
            btnLamMoi.Click += (_, _) => LamMoi();
            btnLuu.Click += (_, _) => LuuPhieu();
            dgvCT.CellValidating += DgvCT_CellValidating;
            dgvCT.CellEndEdit += (_, e) => { dgvCT.Rows[e.RowIndex].ErrorText = ""; CapNhatTong(); };
            dgvCT.DataError += (_, e) => e.ThrowException = false;

            // ----- sự kiện tab Danh sách -----
            tabMain.SelectedIndexChanged += (_, _) =>
            {
                if (tabMain.SelectedTab == tpDS) NapDanhSach(_maPhieu);
                else NapNL();                              // tồn có thể đã đổi sau khi gửi hàng
            };
            txtTimPhieu.TextChanged += (_, _) => NapDanhSach(null);
            cboLocTT.SelectedIndexChanged += (_, _) => NapDanhSach(null);
            chkCanXuLy.CheckedChanged += (_, _) => NapDanhSach(null);
            btnLamMoiDS.Click += (_, _) => NapDanhSach(_maPhieu);
            dgvPhieu.SelectionChanged += (_, _) => ChonPhieu();
            dgvCTPhieu.CellValidating += DgvCTPhieu_CellValidating;
            dgvCTPhieu.CellEndEdit += (_, e) => dgvCTPhieu.Rows[e.RowIndex].ErrorText = "";
            dgvCTPhieu.CellFormatting += DgvCTPhieu_CellFormatting;
            dgvCTPhieu.DataError += (_, e) => e.ThrowException = false;
            btnGuiHang.Click += (_, _) => GuiHang();
            btnNhanHang.Click += (_, _) => NhanHang();
            btnHuyPhieu.Click += (_, _) => HuyPhieu();

            // ----- chung -----
            KeyDown += Form_KeyDown;
            FormClosing += (_, e) =>
            {
                if (_ct.Rows.Count > 0 && !Msg.Hoi("Phiếu đang lập dở chưa lưu.\nĐóng form và bỏ các dòng đã nhập?"))
                    e.Cancel = true;
            };
        }

        /// <summary>FrmBase tự gọi khi form mở (sự kiện Load).</summary>
        public override void RefreshData()
        {
            NapChiNhanh();
            txtNguoi.Text = Session.TenNhanVien;
            NapNL();
            NapDanhSach(_maPhieu);
        }

        private static string SoPhieu(int ma) => "DC" + ma.ToString("D5");

        // ==================================================================
        //  TAB 1: LẬP PHIẾU
        // ==================================================================

        private void NapChiNhanh()
        {
            int giu = cboKhoGui.SelectedValue is int g ? g : Session.MaChiNhanh;
            _dangNap = true;
            cboKhoGui.BindList(LookupService.ChiNhanh(), "TenChiNhanh", "MaChiNhanh");
            cboKhoGui.SelectedValue = Session.LaAdmin ? giu : Session.MaChiNhanh;
            cboKhoGui.Enabled = Session.LaAdmin;            // quản lý chỉ gửi từ kho của mình
            _khoGuiCu = MaKhoGui;
            NapKhoNhan();
            _dangNap = false;
        }

        /// <summary>Kho nhận phải khác kho gửi (DB có ràng buộc CK_DC_KhacKho).</summary>
        private void NapKhoNhan()
        {
            int? giu = cboKhoNhan.SelectedValue as int?;
            var dt = LookupService.ChiNhanh();
            dt.DefaultView.RowFilter = $"MaChiNhanh <> {MaKhoGui}";
            cboKhoNhan.BindList(dt.DefaultView.ToTable(), "TenChiNhanh", "MaChiNhanh");
            if (giu != null && giu != MaKhoGui) cboKhoNhan.SelectedValue = giu;
            else cboKhoNhan.SelectedIndex = -1;             // bắt buộc người dùng chọn
        }

        private void KhoGuiThayDoi()
        {
            if (_dangNap) return;

            if (_ct.Rows.Count > 0 &&
                !Msg.Hoi("Đổi kho gửi sẽ xóa các dòng đã nhập (tồn của mỗi kho khác nhau).\nTiếp tục?"))
            {
                _dangNap = true;
                cboKhoGui.SelectedValue = _khoGuiCu;
                _dangNap = false;
                return;
            }

            _ct.Clear();
            _khoGuiCu = MaKhoGui;
            _dangNap = true;
            NapKhoNhan();
            _dangNap = false;
            NapNL();
            CapNhatTong();
        }

        private void XayBangDong()
        {
            _ct.Columns.Add("MaNguyenLieu", typeof(int));
            _ct.Columns.Add("TenNguyenLieu", typeof(string));
            _ct.Columns.Add("TenDonVi", typeof(string));
            _ct.Columns.Add("SoLuongGui", typeof(decimal));
            _ct.Columns.Add("TonKhoGui", typeof(decimal));

            dgvCT.DataSource = _ct;
            dgvCT.HideCols("MaNguyenLieu");
            dgvCT.Heads(
                ("TenNguyenLieu", "Nguyên liệu"),
                ("TenDonVi", "ĐVT"),
                ("SoLuongGui", "Số lượng gửi"),
                ("TonKhoGui", "Tồn kho gửi"));
            dgvCT.Columns["TenNguyenLieu"]!.FillWeight = 240;
            dgvCT.Columns["TenDonVi"]!.FillWeight = 60;
            dgvCT.Columns["SoLuongGui"]!.FillWeight = 110;
            dgvCT.Columns["TonKhoGui"]!.FillWeight = 110;
            // Cột sửa được không dùng dấu phân cách hàng nghìn để ô sửa hiện đúng số (vd: 200 hoặc 2,5)
            dgvCT.Columns["SoLuongGui"]!.AsNumber("0.###");
            dgvCT.Columns["TonKhoGui"]!.AsNumber("#,##0.###");

            foreach (DataGridViewColumn col in dgvCT.Columns)
                col.ReadOnly = col.Name != "SoLuongGui";

            CapNhatTong();
        }

        private void NapNL()
        {
            int? giu = cboNL.SelectedValue as int?;
            _dangNap = true;
            cboNL.BindList(LookupService.NguyenLieuCoTon(MaKhoGui), "TenNguyenLieu", "MaNguyenLieu");
            if (giu != null) cboNL.SelectedValue = giu;
            else { cboNL.SelectedIndex = -1; cboNL.Text = ""; }
            _dangNap = false;
            NLThayDoi();
        }

        private void NLThayDoi()
        {
            if (_dangNap) return;
            if (cboNL.SelectedItem is not DataRowView r)
            {
                lblTon.Text = "";
                lblDV.Text = "—";
                return;
            }
            string dv = Convert.ToString(r["TenDonVi"]) ?? "";
            lblDV.Text = dv;
            lblTon.Text = $"Tồn kho gửi: {Convert.ToDecimal(r["SoLuongTon"]):#,##0.###} {dv}";
        }

        private void ThemDong()
        {
            Err.Clear();

            if (cboNL.SelectedItem is not DataRowView nl || cboNL.SelectedValue is not int maNL)
            {
                Err.SetError(cboNL, "Chọn nguyên liệu trong danh sách");
                cboNL.Focus();
                return;
            }
            decimal ton = Convert.ToDecimal(nl["SoLuongTon"]);
            string dv = Convert.ToString(nl["TenDonVi"]) ?? "";

            if (numSL.Value <= 0)
            {
                Err.SetError(numSL, "Số lượng gửi phải lớn hơn 0");
                numSL.Focus();
                return;
            }
            if (numSL.Value > ton)
            {
                Err.SetError(numSL, $"Vượt quá tồn kho gửi (chỉ còn {ton:#,##0.###} {dv})");
                numSL.Focus();
                return;
            }
            if (_ct.AsEnumerable().Any(r => Convert.ToInt32(r["MaNguyenLieu"]) == maNL))
            {
                Err.SetError(cboNL, "Nguyên liệu đã có trong phiếu, hãy sửa số lượng ở bảng bên dưới");
                return;
            }

            // Thứ tự khớp các cột: MaNguyenLieu, TenNguyenLieu, TenDonVi, SoLuongGui, TonKhoGui
            _ct.Rows.Add(maNL, nl["TenNguyenLieu"], nl["TenDonVi"], numSL.Value, ton);

            numSL.Value = 0;
            _dangNap = true;
            cboNL.SelectedIndex = -1;
            cboNL.Text = "";
            _dangNap = false;
            NLThayDoi();
            cboNL.Focus();
            CapNhatTong();
        }

        private void XoaDong()
        {
            if (dgvCT.CurrentRow?.DataBoundItem is not DataRowView v) return;
            _ct.Rows.Remove(v.Row);
            CapNhatTong();
        }

        private void CapNhatTong() => lblTong.Text = $"{_ct.Rows.Count} dòng";

        private void LamMoi()
        {
            Err.Clear();
            _ct.Clear();
            txtGhiChu.Clear();
            numSL.Value = 0;
            chkGuiNgay.Checked = false;
            _dangNap = true;
            cboNL.SelectedIndex = -1;
            cboNL.Text = "";
            cboKhoNhan.SelectedIndex = -1;
            _dangNap = false;
            NLThayDoi();
            CapNhatTong();
        }

        private void DgvCT_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvCT.Columns[e.ColumnIndex].Name != "SoLuongGui") return;

            // Chỉ nhận dấu thập phân theo máy (vi-VN là dấu phẩy) để "0.5" không bị hiểu thành 5
            bool ok = decimal.TryParse(Convert.ToString(e.FormattedValue), NumberStyles.AllowDecimalPoint,
                                       CultureInfo.CurrentCulture, out decimal v) && v > 0;
            if (!ok)
            {
                e.Cancel = true;
                dgvCT.Rows[e.RowIndex].ErrorText =
                    "Số lượng phải là số lớn hơn 0 (dùng dấu phẩy cho phần thập phân, ví dụ 2,5). Nhấn Esc để hoàn lại.";
                return;
            }

            if (dgvCT.Rows[e.RowIndex].DataBoundItem is DataRowView row && v > Convert.ToDecimal(row["TonKhoGui"]))
            {
                e.Cancel = true;
                dgvCT.Rows[e.RowIndex].ErrorText =
                    $"Vượt quá tồn kho gửi (chỉ còn {Convert.ToDecimal(row["TonKhoGui"]):#,##0.###}). Nhấn Esc để hoàn lại.";
            }
        }

        private void LuuPhieu()
        {
            Err.Clear();

            if (cboKhoNhan.SelectedValue is not int khoNhan)
            {
                Err.SetError(cboKhoNhan, "Chọn kho nhận");
                cboKhoNhan.Focus();
                return;
            }
            if (!dgvCT.EndEdit()) return;                    // ô đang sửa chưa hợp lệ thì dừng
            if (_ct.Rows.Count == 0)
            {
                Err.SetError(btnLuu, "Phiếu chưa có dòng nào");
                return;
            }

            int ma;
            try
            {
                ma = DieuChuyenService.TaoPhieu(MaKhoGui, khoNhan, Session.MaNhanVien, txtGhiChu.Text, _ct, GioiHanCN);
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                return;
            }

            string so = SoPhieu(ma);
            if (chkGuiNgay.Checked)
            {
                try
                {
                    DieuChuyenService.GuiHang(ma, Session.MaNhanVien, GioiHanCN);
                    Msg.Bao($"Đã lập phiếu {so} và gửi hàng. Kho gửi đã bị trừ.");
                }
                catch (Exception ex)
                {
                    // Phiếu đã lưu nhưng chưa trừ kho: nói rõ để người dùng không lập trùng
                    Msg.Loi(new InvalidOperationException(
                        $"Đã lưu phiếu {so} (Chờ gửi) nhưng chưa gửi được:\n{ex.Message}"));
                }
            }
            else
            {
                Msg.Bao($"Đã lưu phiếu {so} (chờ gửi).\nKho chưa bị trừ cho tới khi bấm 'Gửi hàng' trong tab Danh sách phiếu.");
            }

            LamMoi();
            NapNL();
            tabMain.SelectedTab = tpDS;                      // SelectedIndexChanged sẽ nạp danh sách
            NapDanhSach(ma);                                 // và chọn đúng phiếu vừa lập
        }

        // ==================================================================
        //  TAB 2: DANH SÁCH PHIẾU
        // ==================================================================

        private void NapDanhSach(int? chonMa)
        {
            string? tt = cboLocTT.SelectedIndex switch
            { 1 => "ChoGui", 2 => "DangChuyen", 3 => "DaNhan", 4 => "DaHuy", _ => null };

            try
            {
                _dangNap = true;
                var dt = DieuChuyenService.DanhSach(GioiHanCN, tt, chkCanXuLy.Checked, txtTimPhieu.Text.Trim());
                dgvPhieu.DataSource = dt;

                dgvPhieu.HideCols("MaPhieu", "MaKhoGui", "MaKhoNhan", "GhiChu");
                dgvPhieu.Heads(
                    ("SoPhieu", "Số phiếu"),
                    ("KhoGui", "Kho gửi"),
                    ("KhoNhan", "Kho nhận"),
                    ("NgayGui", "Ngày gửi"),
                    ("NgayNhan", "Ngày nhận"),
                    ("TrangThai", "Trạng thái"),
                    ("NguoiGui", "Người gửi"),
                    ("NguoiNhan", "Người nhận"));
                dgvPhieu.Columns["SoPhieu"]!.FillWeight = 75;
                dgvPhieu.Columns["KhoGui"]!.FillWeight = 170;
                dgvPhieu.Columns["KhoNhan"]!.FillWeight = 170;
                dgvPhieu.Columns["NgayGui"]!.FillWeight = 120;
                dgvPhieu.Columns["NgayNhan"]!.FillWeight = 120;
                dgvPhieu.Columns["TrangThai"]!.FillWeight = 100;
                dgvPhieu.Columns["NguoiGui"]!.FillWeight = 120;
                dgvPhieu.Columns["NguoiNhan"]!.FillWeight = 120;
                dgvPhieu.Columns["NgayGui"]!.AsDate("dd/MM/yyyy HH:mm");
                dgvPhieu.Columns["NgayNhan"]!.AsDate("dd/MM/yyyy HH:mm");

                int cho = dt.AsEnumerable().Count(r => Convert.ToString(r["TrangThai"]) is "ChoGui" or "DangChuyen");
                lblTongPhieu.ForeColor = cho > 0 ? Theme.Warning : Theme.Muted;
                lblTongPhieu.Text = $"{dt.Rows.Count} phiếu   •   {cho} chưa hoàn tất";

                if (chonMa != null)
                {
                    foreach (DataGridViewRow row in dgvPhieu.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["MaPhieu"].Value) == chonMa)
                        {
                            dgvPhieu.CurrentCell = row.Cells["SoPhieu"];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                return;
            }
            finally
            {
                _dangNap = false;
            }

            ChonPhieu();
        }

        private static string TienTrinh(string tt) => tt switch
        {
            "ChoGui" => "●  Chờ gửi     →     ○  Đang chuyển     →     ○  Đã nhận",
            "DangChuyen" => "✔  Chờ gửi     →     ●  Đang chuyển     →     ○  Đã nhận",
            "DaNhan" => "✔  Chờ gửi     →     ✔  Đang chuyển     →     ●  Đã nhận",
            "DaHuy" => "✖  Phiếu đã hủy",
            _ => ""
        };

        /// <summary>Có được nhập số thực nhận ở lưới chi tiết không (phiếu đang chuyển và mình là bên nhận).</summary>
        private bool DuocNhap => _tt == "DangChuyen" && (Session.LaAdmin || Session.MaChiNhanh == _khoNhan);

        private void ChonPhieu()
        {
            if (_dangNap) return;

            if (dgvPhieu.CurrentRow == null)
            {
                _maPhieu = null;
                _tt = "";
                _ctPhieu = null;
                dgvCTPhieu.DataSource = null;
                lblChiTiet.Text = "Chi tiết phiếu";
                CapNhatNut();
                return;
            }

            var c = dgvPhieu.CurrentRow.Cells;
            _maPhieu = Convert.ToInt32(c["MaPhieu"].Value);
            _tt = Convert.ToString(c["TrangThai"].Value) ?? "";
            _khoGui = Convert.ToInt32(c["MaKhoGui"].Value);
            _khoNhan = Convert.ToInt32(c["MaKhoNhan"].Value);

            _ctPhieu = DieuChuyenService.GetChiTiet(_maPhieu.Value);
            if (_tt == "DangChuyen")                         // mặc định "nhận đủ", người dùng chỉ sửa dòng lệch
                foreach (DataRow r in _ctPhieu.Rows)
                    if (r["SoLuongNhanThucTe"] == DBNull.Value) r["SoLuongNhanThucTe"] = r["SoLuongGui"];

            dgvCTPhieu.DataSource = _ctPhieu;
            dgvCTPhieu.HideCols("MaChiTiet");
            dgvCTPhieu.Heads(
                ("TenNguyenLieu", "Nguyên liệu"),
                ("TenDonVi", "ĐVT"),
                ("SoLuongGui", "SL gửi"),
                ("SoLuongNhanThucTe", "SL thực nhận"),
                ("HaoHut", "Hao hụt"),
                ("TonKhoGui", "Tồn kho gửi (hiện tại)"));
            dgvCTPhieu.Columns["SoLuongGui"]!.AsNumber("#,##0.###");
            // Cột sửa được không dùng dấu phân cách hàng nghìn để ô sửa hiện đúng số
            dgvCTPhieu.Columns["SoLuongNhanThucTe"]!.AsNumber("0.###");
            dgvCTPhieu.Columns["HaoHut"]!.AsNumber("#,##0.###");
            dgvCTPhieu.Columns["TonKhoGui"]!.AsNumber("#,##0.###");

            // Chỉ hiện cột có nghĩa với trạng thái hiện tại
            dgvCTPhieu.Columns["HaoHut"]!.Visible = _tt == "DaNhan";
            dgvCTPhieu.Columns["TonKhoGui"]!.Visible = _tt == "ChoGui";
            dgvCTPhieu.Columns["SoLuongNhanThucTe"]!.Visible = _tt is "DangChuyen" or "DaNhan";

            foreach (DataGridViewColumn col in dgvCTPhieu.Columns)   // chỉ ô "SL thực nhận" sửa được, và chỉ khi đang chuyển
                col.ReadOnly = !(DuocNhap && col.Name == "SoLuongNhanThucTe");

            string ghiChu = Convert.ToString(c["GhiChu"].Value) ?? "";
            lblChiTiet.Text = $"{Convert.ToString(c["SoPhieu"].Value)}:  {Convert.ToString(c["KhoGui"].Value)}  →  {Convert.ToString(c["KhoNhan"].Value)}" +
                              (ghiChu.Length > 0 ? $"   •   Ghi chú: {ghiChu}" : "") +
                              $"\n{TienTrinh(_tt)}";

            CapNhatNut();
        }

        private void CapNhatNut()
        {
            bool co = _maPhieu != null;
            bool laKhoGui = Session.LaAdmin || Session.MaChiNhanh == _khoGui;
            bool laKhoNhan = Session.LaAdmin || Session.MaChiNhanh == _khoNhan;

            btnGuiHang.Enabled = co && _tt == "ChoGui" && laKhoGui;
            btnHuyPhieu.Enabled = co && _tt == "ChoGui" && laKhoGui;
            btnNhanHang.Enabled = co && _tt == "DangChuyen" && laKhoNhan;
        }

        // ---------- Sửa số lượng thực nhận ngay trong lưới ----------
        private void DgvCTPhieu_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvCTPhieu.Columns[e.ColumnIndex].Name != "SoLuongNhanThucTe") return;

            bool ok = decimal.TryParse(Convert.ToString(e.FormattedValue), NumberStyles.AllowDecimalPoint,
                                       CultureInfo.CurrentCulture, out decimal v) && v >= 0;
            if (!ok)
            {
                e.Cancel = true;
                dgvCTPhieu.Rows[e.RowIndex].ErrorText =
                    "Số lượng thực nhận phải là số không âm (dùng dấu phẩy cho phần thập phân). Nhấn Esc để hoàn lại.";
                return;
            }

            if (dgvCTPhieu.Rows[e.RowIndex].DataBoundItem is DataRowView row && v > Convert.ToDecimal(row["SoLuongGui"]))
            {
                e.Cancel = true;
                dgvCTPhieu.Rows[e.RowIndex].ErrorText =
                    $"Không được nhận nhiều hơn số đã gửi ({Convert.ToDecimal(row["SoLuongGui"]):#,##0.###}). Nhấn Esc để hoàn lại.";
            }
        }

        /// <summary>Tô màu giúp nhìn nhanh: kho gửi thiếu tồn (chờ gửi) hoặc nhận ít hơn đã gửi (đang chuyển).</summary>
        private void DgvCTPhieu_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgvCTPhieu.Columns[e.ColumnIndex].Name;
            var cells = dgvCTPhieu.Rows[e.RowIndex].Cells;

            if (_tt == "ChoGui" && col == "TonKhoGui" &&
                Convert.ToDecimal(cells["SoLuongGui"].Value) > Convert.ToDecimal(cells["TonKhoGui"].Value))
            {
                e.CellStyle!.ForeColor = Theme.Danger;
                e.CellStyle.Font = Theme.Bold;
            }
            else if (_tt == "DangChuyen" && col == "SoLuongNhanThucTe" &&
                     cells["SoLuongNhanThucTe"].Value is decimal nhan &&
                     nhan < Convert.ToDecimal(cells["SoLuongGui"].Value))
            {
                e.CellStyle!.ForeColor = Theme.Warning;
                e.CellStyle.Font = Theme.Bold;
            }
            else if (_tt == "DaNhan" && col == "HaoHut" && e.Value is decimal h && h > 0)
            {
                e.CellStyle!.ForeColor = Theme.Danger;
                e.CellStyle.Font = Theme.Bold;
            }
        }

        // ---------- Hành động ----------
        private void GuiHang()
        {
            if (_maPhieu == null) return;
            int ma = _maPhieu.Value;
            if (!Msg.Hoi($"Gửi hàng phiếu {SoPhieu(ma)}?\n\nKho gửi sẽ bị trừ ngay và không thể hoàn tác.")) return;

            try
            {
                DieuChuyenService.GuiHang(ma, Session.MaNhanVien, GioiHanCN);
                NapDanhSach(ma);
                NapNL();                                     // tồn kho gửi ở tab Lập phiếu đã đổi
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);                                 // có thể là thiếu tồn (phiếu vẫn Chờ gửi)
                NapDanhSach(ma);
            }
        }

        private void NhanHang()
        {
            if (_maPhieu == null || _ctPhieu == null) return;
            if (!dgvCTPhieu.EndEdit()) return;               // ô đang sửa chưa hợp lệ thì dừng
            int ma = _maPhieu.Value;

            try
            {
                var thucNhan = _ctPhieu.AsEnumerable().ToDictionary(
                    r => Convert.ToInt32(r["MaChiTiet"]), r => Convert.ToDecimal(r["SoLuongNhanThucTe"]));
                bool hao = _ctPhieu.AsEnumerable().Any(r =>
                    Convert.ToDecimal(r["SoLuongNhanThucTe"]) < Convert.ToDecimal(r["SoLuongGui"]));

                string cauHoi = hao
                    ? $"Có dòng nhận ÍT HƠN số đã gửi (hao hụt vận chuyển).\nXác nhận nhận hàng phiếu {SoPhieu(ma)} theo số thực nhận?"
                    : $"Xác nhận đã nhận đủ hàng của phiếu {SoPhieu(ma)}?";
                if (!Msg.Hoi(cauHoi + "\n\nKho nhận sẽ được cộng ngay và không thể hoàn tác.")) return;

                DieuChuyenService.NhanHang(ma, Session.MaNhanVien, thucNhan, GioiHanCN);
                NapDanhSach(ma);
                NapNL();
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                NapDanhSach(ma);
            }
        }

        private void HuyPhieu()
        {
            if (_maPhieu == null) return;
            int ma = _maPhieu.Value;
            if (!Msg.Hoi($"Hủy phiếu {SoPhieu(ma)}?\n\nPhiếu chưa gửi nên kho không bị ảnh hưởng.")) return;

            try
            {
                DieuChuyenService.HuyPhieu(ma, GioiHanCN);
                NapDanhSach(ma);
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                NapDanhSach(ma);
            }
        }

        // ---------- Phím tắt ----------
        private void Form_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S && tabMain.SelectedTab == tpLap)
            {
                LuuPhieu();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                if (tabMain.SelectedTab == tpDS) NapDanhSach(_maPhieu);
                else NapNL();
            }
        }
    }
}