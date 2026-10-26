using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class ucNhapHang : UserControl, IRefreshable
    {
        private readonly DataTable _ct = new();                  // các dòng của phiếu đang lập
        private readonly ErrorProvider _err = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
        private int _maCN;
        private string _tenCN = "";
        private bool _dangNap;

        // phiếu đang chọn ở tab Danh sách
        private int? _maPhieu;
        private string _ttPhieu = "";
        private decimal _daTTPhieu;
        private decimal _conNoPhieu;

        public ucNhapHang()
        {
            InitializeComponent();
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            BackColor = Theme.Background;
            Font = Theme.Base;

            // ----- giao diện -----
            foreach (var tp in new[] { tpLap, tpDS })
            {
                tp.UseVisualStyleBackColor = false;
                tp.BackColor = Theme.Background;
            }
            foreach (var g in new[] { dgvCT, dgvPhieu, dgvCTPhieu }) g.ApplyTheme();
            dgvCT.ReadOnly = false;                       // sửa số lượng, đơn giá ngay trong lưới
            btnLuu.StylePrimary(); btnThemDong.StylePrimary(); btnNhanHang.StylePrimary(); btnThanhToan.StylePrimary();
            btnXoaDong.StyleSecondary(); btnLamMoi.StyleSecondary(); btnLamMoiDS.StyleSecondary(); btnHuyPhieu.StyleSecondary();
            TrangThaiUi.Hook(dgvPhieu, "TrangThai");
            TrangThaiUi.Hook(dgvPhieu, "TrangThaiCongNo");
            lblTon.ForeColor = Theme.Muted;
            lblDV.ForeColor = Theme.Muted;
            lblTongPhieu.ForeColor = Theme.Muted;
            lblChiTiet.ForeColor = Theme.Text;

            txtSoPhieu.ReadOnly = txtNguoiLap.ReadOnly = txtChiNhanh.ReadOnly = true;
            txtSoPhieu.Text = "(Tự động)";
            dtpNgayDat.Value = DateTime.Today;
            cboNCC.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocTT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocTT.Items.AddRange(new object[] { "Tất cả", "Chờ nhập", "Đã nhập", "Đã hủy" });
            cboLocTT.SelectedIndex = 0;

            XayBangDong();

            // ----- sự kiện tab Lập phiếu -----
            cboNCC.SelectedIndexChanged += (_, _) => { _err.SetError(cboNCC, ""); NLThayDoi(); };
            cboNL.SelectedIndexChanged += (_, _) => NLThayDoi();
            btnThemDong.Click += (_, _) => ThemDong();
            btnXoaDong.Click += (_, _) => XoaDong();
            btnLamMoi.Click += (_, _) => LamMoi();
            btnLuu.Click += (_, _) => LuuPhieu();
            dgvCT.CellValidating += DgvCT_CellValidating;
            dgvCT.CellEndEdit += DgvCT_CellEndEdit;
            dgvCT.DataError += (_, e) => e.ThrowException = false;

            // ----- sự kiện tab Danh sách -----
            tabNhap.SelectedIndexChanged += (_, _) =>
            {
                if (tabNhap.SelectedTab == tpDS) NapDanhSach(_maPhieu);
                else NapNL();                              // tồn có thể đã đổi sau khi nhận hàng
            };
            txtTimPhieu.TextChanged += (_, _) => NapDanhSach(null);
            cboLocTT.SelectedIndexChanged += (_, _) => NapDanhSach(null);
            chkConNo.CheckedChanged += (_, _) => NapDanhSach(null);
            btnLamMoiDS.Click += (_, _) => NapDanhSach(_maPhieu);
            dgvPhieu.SelectionChanged += (_, _) => ChonPhieu();
            btnNhanHang.Click += (_, _) => NhanHang();
            btnThanhToan.Click += (_, _) => ThanhToan();
            btnHuyPhieu.Click += (_, _) => HuyPhieu();
        }

        /// <summary>Form chủ gọi trước RefreshData để cho biết đang làm việc với chi nhánh nào.</summary>
        public void SetChiNhanh(int maChiNhanh, string tenChiNhanh)
        {
            _maCN = maChiNhanh;
            _tenCN = tenChiNhanh;
            txtChiNhanh.Text = tenChiNhanh;
        }

        public void RefreshData()
        {
            if (_maCN == 0) return;                        // form chủ chưa gán chi nhánh

            int? giuNCC = cboNCC.SelectedValue as int?;    // giữ lựa chọn đang có khi nạp lại
            _dangNap = true;
            cboNCC.BindList(NccService.GetCombo(), "TenNCC", "MaNCC");
            if (giuNCC != null) cboNCC.SelectedValue = giuNCC; else cboNCC.SelectedIndex = -1;
            _dangNap = false;

            txtNguoiLap.Text = Session.TenNhanVien;
            NapNL();
            NapDanhSach(_maPhieu);
        }

        private static string SoPhieu(int ma) => "PN" + ma.ToString("D5");

        // ==================================================================
        //  TAB 1: LẬP PHIẾU
        // ==================================================================

        private void XayBangDong()
        {
            _ct.Columns.Add("MaNguyenLieu", typeof(int));
            _ct.Columns.Add("TenNguyenLieu", typeof(string));
            _ct.Columns.Add("TenDonVi", typeof(string));
            _ct.Columns.Add("SoLuong", typeof(decimal));
            _ct.Columns.Add("DonGiaNhap", typeof(decimal));
            _ct.Columns.Add("ThanhTien", typeof(decimal), "SoLuong * DonGiaNhap");

            dgvCT.DataSource = _ct;
            dgvCT.HideCols("MaNguyenLieu");
            dgvCT.Heads(
                ("TenNguyenLieu", "Nguyên liệu"),
                ("TenDonVi", "ĐVT"),
                ("SoLuong", "Số lượng"),
                ("DonGiaNhap", "Đơn giá (đ)"),
                ("ThanhTien", "Thành tiền (đ)"));
            dgvCT.Columns["TenNguyenLieu"]!.FillWeight = 230;
            dgvCT.Columns["TenDonVi"]!.FillWeight = 60;
            dgvCT.Columns["SoLuong"]!.FillWeight = 100;
            dgvCT.Columns["DonGiaNhap"]!.FillWeight = 100;
            dgvCT.Columns["ThanhTien"]!.FillWeight = 120;
            // Số lượng không dùng dấu phân cách hàng nghìn để ô sửa hiện đúng số (vd: 3900 hoặc 2,5)
            dgvCT.Columns["SoLuong"]!.AsNumber("0.###");
            dgvCT.Columns["DonGiaNhap"]!.AsNumber("N0");
            dgvCT.Columns["ThanhTien"]!.AsNumber("N0");

            foreach (DataGridViewColumn col in dgvCT.Columns)      // chỉ hai cột này được sửa trực tiếp
                col.ReadOnly = !(col.Name is "SoLuong" or "DonGiaNhap");

            CapNhatTong();
        }

        private void NapNL()
        {
            int? giu = cboNL.SelectedValue as int?;
            _dangNap = true;
            cboNL.BindList(LookupService.NguyenLieu(_maCN == 0 ? null : _maCN), "TenNguyenLieu", "MaNguyenLieu");
            if (giu != null) cboNL.SelectedValue = giu;
            else { cboNL.SelectedIndex = -1; cboNL.Text = ""; }
            _dangNap = false;
            NLThayDoi();
        }

        /// <summary>Hiện tồn, đơn vị và gợi ý giá nhập gần nhất khi chọn nguyên liệu.</summary>
        private void NLThayDoi()
        {
            if (_dangNap) return;
            if (cboNL.SelectedItem is not DataRowView r)
            {
                lblTon.Text = "";
                lblDV.Text = "—";
                return;
            }

            int maNL = Convert.ToInt32(r["MaNguyenLieu"]);
            string dv = Convert.ToString(r["TenDonVi"]) ?? "";
            lblDV.Text = dv;
            string ton = $"Tồn: {Convert.ToDecimal(r["SoLuongTon"]):#,##0.###} {dv}";

            try
            {
                decimal? gia = NhapKhoService.GoiYGia(maNL, cboNCC.SelectedValue as int?);
                if (gia != null)
                {
                    numGia.Value = Math.Min(numGia.Maximum, gia.Value);
                    ton += $"  •  Giá nhập gần nhất: {gia.Value:N0} đ";
                }
            }
            catch
            {
                // Gợi ý giá chỉ để tiện nhập, lỗi ở đây không được cản việc lập phiếu
            }
            lblTon.Text = ton;
        }

        private void ThemDong()
        {
            _err.Clear();

            if (cboNL.SelectedValue is not int maNL)
            {
                _err.SetError(cboNL, "Chọn nguyên liệu trong danh sách");
                cboNL.Focus();
                return;
            }
            if (numSL.Value <= 0)
            {
                _err.SetError(numSL, "Số lượng phải lớn hơn 0");
                numSL.Focus();
                return;
            }
            if (_ct.AsEnumerable().Any(r => Convert.ToInt32(r["MaNguyenLieu"]) == maNL))
            {
                _err.SetError(cboNL, "Nguyên liệu đã có trong phiếu, hãy sửa số lượng ở bảng bên dưới");
                return;
            }

            var nl = (DataRowView)cboNL.SelectedItem!;
            // Thứ tự khớp các cột: MaNguyenLieu, TenNguyenLieu, TenDonVi, SoLuong, DonGiaNhap (ThanhTien tự tính)
            _ct.Rows.Add(maNL, nl["TenNguyenLieu"], nl["TenDonVi"], numSL.Value, numGia.Value);

            numSL.Value = 0;
            numGia.Value = 0;
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

        private void CapNhatTong()
        {
            decimal tong = _ct.AsEnumerable().Sum(r =>
                Convert.ToDecimal(r["SoLuong"]) * Convert.ToDecimal(r["DonGiaNhap"]));
            lblTong.Text = $"Tổng tiền: {tong:N0} đ";
        }

        private void LamMoi()
        {
            _err.Clear();
            _ct.Clear();
            txtGhiChu.Clear();
            numSL.Value = 0;
            numGia.Value = 0;
            chkNhanNgay.Checked = false;
            _dangNap = true;
            cboNL.SelectedIndex = -1;
            cboNL.Text = "";
            _dangNap = false;
            NLThayDoi();
            CapNhatTong();
        }

        // ---------- Sửa số lượng / đơn giá ngay trong lưới ----------
        private void DgvCT_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            string col = dgvCT.Columns[e.ColumnIndex].Name;
            if (col != "SoLuong" && col != "DonGiaNhap") return;

            // Số lượng: chỉ chấp nhận dấu thập phân theo máy (vi-VN là dấu phẩy) để "0.5" không bị hiểu thành 5.
            // Đơn giá: cho phép dấu chấm phân cách hàng nghìn vì tiền Việt viết như 3.500.
            var style = col == "SoLuong" ? NumberStyles.AllowDecimalPoint : NumberStyles.Number;
            bool ok = decimal.TryParse(Convert.ToString(e.FormattedValue), style, CultureInfo.CurrentCulture, out decimal v)
                      && (col == "SoLuong" ? v > 0 : v >= 0);

            if (!ok)
            {
                e.Cancel = true;
                dgvCT.Rows[e.RowIndex].ErrorText = col == "SoLuong"
                    ? "Số lượng phải là số lớn hơn 0 (dùng dấu phẩy cho phần thập phân, ví dụ 2,5). Nhấn Esc để hoàn lại."
                    : "Đơn giá phải là số không âm. Nhấn Esc để hoàn lại.";
            }
        }

        private void DgvCT_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            dgvCT.Rows[e.RowIndex].ErrorText = "";
            CapNhatTong();
        }

        // ---------- Lưu phiếu ----------
        private void LuuPhieu()
        {
            _err.Clear();

            if (_maCN == 0) return;
            if (cboNCC.SelectedValue is not int maNCC)
            {
                _err.SetError(cboNCC, "Chọn nhà cung cấp");
                cboNCC.Focus();
                return;
            }
            if (dtpNgayDat.Value.Date > DateTime.Today)
            {
                _err.SetError(dtpNgayDat, "Ngày đặt không được ở tương lai");
                return;
            }
            if (!dgvCT.EndEdit()) return;                  // ô đang sửa chưa hợp lệ thì dừng
            if (_ct.Rows.Count == 0)
            {
                _err.SetError(btnLuu, "Phiếu chưa có dòng nào");
                return;
            }
            if (_ct.AsEnumerable().Any(r => Convert.ToDecimal(r["DonGiaNhap"]) == 0) &&
                !Msg.Hoi("Có dòng đơn giá bằng 0. Vẫn lưu phiếu?"))
                return;

            int ma;
            try
            {
                ma = NhapKhoService.TaoPhieu(_maCN, maNCC, dtpNgayDat.Value, Session.MaNhanVien, txtGhiChu.Text, _ct);
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                return;
            }

            string so = SoPhieu(ma);
            if (chkNhanNgay.Checked)
            {
                try
                {
                    NhapKhoService.NhanHang(ma, Session.MaNhanVien);
                    Msg.Bao($"Đã lập phiếu {so} và nhận hàng. Kho đã được cộng.");
                }
                catch (Exception ex)
                {
                    // Phiếu đã lưu nhưng chưa cộng kho: nói rõ để người dùng không lập trùng
                    Msg.Loi(new InvalidOperationException(
                        $"Đã lưu phiếu {so} nhưng chưa nhận hàng được:\n{ex.Message}\n\n" +
                        "Bạn có thể nhận hàng lại trong tab Danh sách phiếu."));
                }
            }
            else
            {
                Msg.Bao($"Đã lưu phiếu {so} (chờ nhập).\nKhi hàng về tới kho, vào 'Danh sách phiếu' để xác nhận nhận hàng.");
            }

            LamMoi();
            NapNL();
            tabNhap.SelectedTab = tpDS;                    // SelectedIndexChanged sẽ nạp danh sách
            NapDanhSach(ma);                               // và chọn đúng phiếu vừa lập
        }

        // ==================================================================
        //  TAB 2: DANH SÁCH PHIẾU
        // ==================================================================

        private void NapDanhSach(int? chonMa)
        {
            if (_maCN == 0) return;
            string? tt = cboLocTT.SelectedIndex switch { 1 => "ChoNhap", 2 => "DaNhap", 3 => "DaHuy", _ => null };

            try
            {
                _dangNap = true;
                var dt = NhapKhoService.DanhSach(_maCN, tt, chkConNo.Checked, txtTimPhieu.Text.Trim());
                dgvPhieu.DataSource = dt;

                dgvPhieu.HideCols("MaPhieuNhap", "GhiChu");
                dgvPhieu.Heads(
                    ("SoPhieu", "Số phiếu"),
                    ("TenNCC", "Nhà cung cấp"),
                    ("NgayDat", "Ngày đặt"),
                    ("NgayNhan", "Ngày nhận"),
                    ("TongTien", "Tổng tiền"),
                    ("DaThanhToan", "Đã thanh toán"),
                    ("ConNo", "Còn nợ"),
                    ("TrangThai", "Trạng thái"),
                    ("TrangThaiCongNo", "Công nợ"),
                    ("NguoiLap", "Người lập"));
                dgvPhieu.Columns["SoPhieu"]!.FillWeight = 75;
                dgvPhieu.Columns["TenNCC"]!.FillWeight = 190;
                dgvPhieu.Columns["NgayDat"]!.FillWeight = 85;
                dgvPhieu.Columns["NgayNhan"]!.FillWeight = 85;
                dgvPhieu.Columns["TongTien"]!.FillWeight = 100;
                dgvPhieu.Columns["DaThanhToan"]!.FillWeight = 100;
                dgvPhieu.Columns["ConNo"]!.FillWeight = 90;
                dgvPhieu.Columns["TrangThai"]!.FillWeight = 90;
                dgvPhieu.Columns["TrangThaiCongNo"]!.FillWeight = 115;
                dgvPhieu.Columns["NguoiLap"]!.FillWeight = 120;
                dgvPhieu.Columns["NgayDat"]!.AsDate();
                dgvPhieu.Columns["NgayNhan"]!.AsDate();
                foreach (var c in new[] { "TongTien", "DaThanhToan", "ConNo" }) dgvPhieu.Columns[c]!.AsNumber("N0");

                decimal tong = dt.AsEnumerable().Where(r => Convert.ToString(r["TrangThai"]) != "DaHuy")
                                 .Sum(r => Convert.ToDecimal(r["TongTien"]));
                decimal no = dt.AsEnumerable().Sum(r => Convert.ToDecimal(r["ConNo"]));
                lblTongPhieu.Text = $"{dt.Rows.Count} phiếu   •   Tổng tiền {tong:N0} đ   •   Còn nợ {no:N0} đ";

                if (chonMa != null)
                {
                    foreach (DataGridViewRow row in dgvPhieu.Rows)
                    {
                        if (Convert.ToInt32(row.Cells["MaPhieuNhap"].Value) == chonMa)
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

        private void ChonPhieu()
        {
            if (_dangNap) return;

            if (dgvPhieu.CurrentRow == null)
            {
                _maPhieu = null;
                _ttPhieu = "";
                dgvCTPhieu.DataSource = null;
                lblChiTiet.Text = "Chi tiết phiếu";
                CapNhatNut();
                return;
            }

            var c = dgvPhieu.CurrentRow.Cells;
            _maPhieu = Convert.ToInt32(c["MaPhieuNhap"].Value);
            _ttPhieu = Convert.ToString(c["TrangThai"].Value) ?? "";
            _daTTPhieu = Convert.ToDecimal(c["DaThanhToan"].Value);
            _conNoPhieu = Convert.ToDecimal(c["ConNo"].Value);

            dgvCTPhieu.DataSource = NhapKhoService.GetChiTiet(_maPhieu.Value);
            dgvCTPhieu.Heads(
                ("TenNguyenLieu", "Nguyên liệu"),
                ("TenDonVi", "ĐVT"),
                ("SoLuong", "Số lượng"),
                ("DonGiaNhap", "Đơn giá (đ)"),
                ("ThanhTien", "Thành tiền (đ)"));
            dgvCTPhieu.Columns["SoLuong"]!.AsNumber("#,##0.###");
            dgvCTPhieu.Columns["DonGiaNhap"]!.AsNumber("N0");
            dgvCTPhieu.Columns["ThanhTien"]!.AsNumber("N0");

            string nguoi = Convert.ToString(c["NguoiLap"].Value);
            string gc = Convert.ToString(c["GhiChu"].Value) ?? "";
            lblChiTiet.Text = $"Chi tiết phiếu {Convert.ToString(c["SoPhieu"].Value)}  •  Người lập: " +
                              $"{(string.IsNullOrWhiteSpace(nguoi) ? "—" : nguoi)}" +
                              (gc.Length > 0 ? $"  •  Ghi chú: {gc}" : "");

            CapNhatNut();
        }

        private void CapNhatNut()
        {
            bool coPhieu = _maPhieu != null;
            bool ql = Session.LaQuanLy;

            btnNhanHang.Enabled = coPhieu && _ttPhieu == "ChoNhap";
            btnHuyPhieu.Enabled = ql && coPhieu && _ttPhieu == "ChoNhap" && _daTTPhieu == 0;
            btnThanhToan.Enabled = ql && coPhieu && _ttPhieu == "DaNhap" && _conNoPhieu > 0;
        }

        private void NhanHang()
        {
            if (_maPhieu == null) return;
            int ma = _maPhieu.Value;
            if (!Msg.Hoi($"Xác nhận đã nhận đủ hàng của phiếu {SoPhieu(ma)}?\n\nKho sẽ được cộng ngay và không thể hoàn tác.")) return;

            try
            {
                NhapKhoService.NhanHang(ma, Session.MaNhanVien);
                NapDanhSach(ma);
                NapNL();                                   // tồn ở tab Lập phiếu đã đổi
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                NapDanhSach(ma);                           // có thể phiếu đã được người khác xử lý
            }
        }

        private void ThanhToan()
        {
            if (_maPhieu == null) return;
            int ma = _maPhieu.Value;

            decimal? so = Prompt.Number("Thanh toán công nợ",
                $"Số tiền thanh toán cho phiếu {SoPhieu(ma)} (còn nợ {_conNoPhieu:N0} đ):",
                _conNoPhieu, _conNoPhieu);
            if (so is not > 0m) return;

            try
            {
                NhapKhoService.ThanhToan(ma, so.Value);
                NapDanhSach(ma);
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
            if (!Msg.Hoi($"Hủy phiếu {SoPhieu(ma)}?\n\nPhiếu chưa nhập hàng nên kho không bị ảnh hưởng.")) return;

            try
            {
                NhapKhoService.HuyPhieu(ma);
                NapDanhSach(ma);
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                NapDanhSach(ma);
            }
        }
    }
}