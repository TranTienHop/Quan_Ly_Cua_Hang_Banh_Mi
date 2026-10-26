using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using QuanLyCuaHangBanhMi.Models;
using System.Data;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormNguyenLieu : FrmBase
    {
        // ----- tab Danh mục -----
        private int? _ma;                          // null = đang thêm mới
        private bool _daLuuHoatDong = true;        // trạng thái đang lưu trong DB của dòng được chọn
        private bool _dangNap;                     // chặn sự kiện khi đang nạp dữ liệu (dùng chung 2 tab)

        // ----- tab Công thức -----
        private int? _maSP;                        // món đang mở công thức
        private string _tenMon = "";
        private DataTable _ct = new();
        private bool _daSua;                       // có thay đổi chưa lưu
        private string _timCu = "";                // nội dung ô tìm món trước đó (để hoàn lại khi người dùng từ chối)

        public FormNguyenLieu()
        {
            InitializeComponent();

            // ----- giao diện chung -----
            foreach (var g in new[] { dgvNL, dgvMon, dgvCT }) g.ApplyTheme();
            dgvCT.ReadOnly = false;                // cho sửa ô định lượng; các cột khác khóa riêng trong MoCongThuc
            foreach (var tp in new[] { tpDanhMuc, tpCongThuc })
            {
                tp.UseVisualStyleBackColor = false;
                tp.BackColor = Theme.Background;
            }
            btnLuu.StylePrimary();
            btnMoi.StyleSecondary();
            btnDungTrong.StyleSecondary();
            btnThemCT.StylePrimary();
            btnLuuCT.StylePrimary();
            btnHoanTac.StyleSecondary();
            btnXoaCT.StyleSecondary();
            TrangThaiUi.Hook(dgvNL, "TrangThai");          // chỉ nối một lần ở constructor
            lblTongKet.ForeColor = Theme.Muted;
            lblMonTong.ForeColor = Theme.Muted;
            lblThongKe.ForeColor = Theme.Muted;
            lblTenMon.ForeColor = Theme.Text;
            KeyPreview = true;

            // ----- tab Danh mục: cấu hình ô -----
            txtMa.ReadOnly = true;
            txtTen.MaxLength = 200;
            cboLoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoc.Items.AddRange(new object[] { "Đang dùng", "Ngừng dùng", "Tất cả" });
            cboLoc.SelectedIndex = 0;

            // ----- phân quyền (menu đã chặn STAFF, đây là lớp bảo vệ thứ hai) -----
            bool choSua = Session.LaQuanLy;
            grpNL.Enabled = choSua;
            flowThem.Enabled = choSua;
            btnXoaCT.Enabled = choSua;

            // ----- sự kiện tab Danh mục -----
            txtTim.TextChanged += (_, _) => NapDanhMuc(null);
            cboLoc.SelectedIndexChanged += (_, _) => NapDanhMuc(null);
            dgvNL.SelectionChanged += (_, _) => HienNL();
            btnMoi.Click += (_, _) => ThemMoiNL();
            btnLuu.Click += (_, _) => LuuNL();
            btnDungTrong.Click += (_, _) => XemMonDung();
            txtTen.TextChanged += (_, _) => Err.SetError(txtTen, "");
            cboDonVi.SelectedIndexChanged += (_, _) => Err.SetError(cboDonVi, "");

            // ----- sự kiện tab Công thức -----
            txtTimMon.TextChanged += (_, _) => TimMonThayDoi();
            dgvMon.SelectionChanged += DgvMon_SelectionChanged;
            cboNLCT.SelectedIndexChanged += (_, _) => HienDonViCT();
            btnThemCT.Click += (_, _) => ThemDongCT();
            btnXoaCT.Click += (_, _) => XoaDongCT();
            btnHoanTac.Click += (_, _) => HoanTacCT();
            btnLuuCT.Click += (_, _) => LuuCT();
            dgvCT.CellValidating += DgvCT_CellValidating;
            dgvCT.CellEndEdit += DgvCT_CellEndEdit;
            dgvCT.DataError += (_, e) => e.ThrowException = false;
            tabMain.SelectedIndexChanged += (_, _) =>
            {
                if (tabMain.SelectedTab == tpCongThuc) NapComboNL();   // nguyên liệu vừa thêm ở tab 1 phải xuất hiện ở đây
            };

            // ----- chung -----
            KeyDown += Form_KeyDown;
            FormClosing += (_, e) =>
            {
                if (_daSua && !Msg.Hoi("Công thức đang sửa chưa được lưu.\nĐóng form và bỏ các thay đổi?"))
                    e.Cancel = true;
            };
        }

        /// <summary>FrmBase tự gọi khi form mở (sự kiện Load).</summary>
        public override void RefreshData()
        {
            _dangNap = true;
            cboDonVi.BindList(NguyenLieuService.GetDonVi(), "TenDonVi", "MaDonVi");
            _dangNap = false;

            NapDanhMuc(_ma);
            NapComboNL();
            NapMon(_maSP);
        }

        // ==================================================================
        //  TAB 1: DANH MỤC NGUYÊN LIỆU
        // ==================================================================

        private void NapDanhMuc(int? chonMa)
        {
            bool? hd = cboLoc.SelectedIndex switch { 0 => true, 1 => false, _ => null };

            try
            {
                _dangNap = true;
                var dt = NguyenLieuService.GetAll(txtTim.Text.Trim(), hd, Session.MaChiNhanh);
                dgvNL.DataSource = dt;

                dgvNL.HideCols("MaNguyenLieu", "MaDonViGoc", "TongTon", "DangHoatDong");
                dgvNL.Heads(
                    ("MaHienThi", "Mã"),
                    ("TenNguyenLieu", "Nguyên liệu"),
                    ("TenDonVi", "Đơn vị"),
                    ("TonToiThieuCanhBao", "Tồn tối thiểu"),
                    ("TonTaiCN", "Tồn (CN của bạn)"),
                    ("SoMon", "Số món dùng"),
                    ("TrangThai", "Trạng thái"));
                dgvNL.Columns["MaHienThi"]!.FillWeight = 60;
                dgvNL.Columns["TenNguyenLieu"]!.FillWeight = 220;
                dgvNL.Columns["TenDonVi"]!.FillWeight = 70;
                dgvNL.Columns["TonToiThieuCanhBao"]!.FillWeight = 100;
                dgvNL.Columns["TonTaiCN"]!.FillWeight = 110;
                dgvNL.Columns["SoMon"]!.FillWeight = 80;
                dgvNL.Columns["TrangThai"]!.FillWeight = 110;
                dgvNL.Columns["TonToiThieuCanhBao"]!.AsNumber("#,##0.###");
                dgvNL.Columns["TonTaiCN"]!.AsNumber("#,##0.###");
                dgvNL.Columns["SoMon"]!.AsNumber("N0");

                lblTongKet.ForeColor = Theme.Muted;
                lblTongKet.Text = $"{dt.Rows.Count} nguyên liệu";
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

            if (chonMa != null)
            {
                foreach (DataGridViewRow r in dgvNL.Rows)
                {
                    if (Convert.ToInt32(r.Cells["MaNguyenLieu"].Value) == chonMa)
                    {
                        dgvNL.CurrentCell = r.Cells["TenNguyenLieu"];
                        break;
                    }
                }
            }
            HienNL();
        }

        private void HienNL()
        {
            if (_dangNap) return;
            if (dgvNL.CurrentRow == null) { XoaTrangNL(); return; }

            var c = dgvNL.CurrentRow.Cells;
            _ma = Convert.ToInt32(c["MaNguyenLieu"].Value);
            _daLuuHoatDong = Convert.ToBoolean(c["DangHoatDong"].Value);

            Err.Clear();
            txtMa.Text = Convert.ToString(c["MaHienThi"].Value);
            txtTen.Text = Convert.ToString(c["TenNguyenLieu"].Value) ?? "";
            cboDonVi.SelectedValue = Convert.ToInt32(c["MaDonViGoc"].Value);
            numTonMin.Value = Math.Min(numTonMin.Maximum, Convert.ToDecimal(c["TonToiThieuCanhBao"].Value));
            chkHoatDong.Checked = _daLuuHoatDong;

            string dv = Convert.ToString(c["TenDonVi"].Value) ?? "";
            decimal tonCN = Convert.ToDecimal(c["TonTaiCN"].Value);
            decimal tong = Convert.ToDecimal(c["TongTon"].Value);
            int soMon = Convert.ToInt32(c["SoMon"].Value);

            bool khoa = NguyenLieuService.CoPhatSinh(_ma.Value);
            cboDonVi.Enabled = !khoa;
            btnDungTrong.Enabled = soMon > 0;

            lblThongKe.Text =
                $"Tồn tại chi nhánh của bạn: {tonCN:#,##0.###} {dv}\n" +
                $"Tổng tồn mọi chi nhánh: {tong:#,##0.###} {dv}\n" +
                $"Dùng trong {soMon} món" +
                (khoa ? "\nĐơn vị gốc đã khóa (đã có dữ liệu phát sinh)." : "");
        }

        private void XoaTrangNL()
        {
            _ma = null;
            _daLuuHoatDong = true;
            Err.Clear();
            txtMa.Text = "(Tự động)";
            txtTen.Clear();
            cboDonVi.Enabled = true;
            cboDonVi.SelectedIndex = -1;              // bắt buộc người dùng chọn đơn vị
            numTonMin.Value = 0;
            chkHoatDong.Checked = true;
            lblThongKe.Text = "—";
            btnDungTrong.Enabled = false;
        }

        private void ThemMoiNL()
        {
            _dangNap = true;                          // bỏ chọn dòng mà không kích hoạt HienNL
            dgvNL.ClearSelection();
            dgvNL.CurrentCell = null;
            _dangNap = false;

            XoaTrangNL();
            txtTen.Focus();
        }

        private void LuuNL()
        {
            Err.Clear();

            string ten = txtTen.Text.Trim();
            if (ten.Length == 0)
            {
                Err.SetError(txtTen, "Nhập tên nguyên liệu");
                txtTen.Focus();
                return;
            }
            if (cboDonVi.SelectedValue is not int maDonVi)
            {
                Err.SetError(cboDonVi, "Chọn đơn vị gốc");
                cboDonVi.Focus();
                return;
            }

            if (_ma != null && _daLuuHoatDong && !chkHoatDong.Checked && !XacNhanNgung())
                return;

            try
            {
                int ma = NguyenLieuService.Save(_ma, ten, maDonVi, numTonMin.Value, chkHoatDong.Checked);
                NapDanhMuc(ma);
                lblTongKet.ForeColor = Theme.Success;
                lblTongKet.Text = $"Đã lưu \"{ten}\" lúc {DateTime.Now:HH:mm:ss}.";
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        private bool XacNhanNgung()
        {
            decimal ton = Convert.ToDecimal(dgvNL.CurrentRow!.Cells["TongTon"].Value);
            int soMon = NguyenLieuService.DemMonDangBan(_ma!.Value);

            var sb = new StringBuilder("Ngừng sử dụng nguyên liệu này?\n");
            if (ton > 0)
                sb.AppendLine($"\n• Đang còn tồn tổng {ton:#,##0.###} ở các chi nhánh (nên xử lý hết tồn trước).");
            if (soMon > 0)
                sb.AppendLine($"• Đang nằm trong công thức của {soMon} món đang bán (công thức giữ nguyên).");
            sb.AppendLine("\nNguyên liệu sẽ không còn xuất hiện khi lập phiếu mới hoặc thêm vào công thức.");
            return Msg.Hoi(sb.ToString());
        }

        private void XemMonDung()
        {
            if (_ma == null) return;
            try
            {
                var dt = NguyenLieuService.GetMonDung(_ma.Value);
                string dv = dgvNL.CurrentRow == null ? "" : Convert.ToString(dgvNL.CurrentRow.Cells["TenDonVi"].Value) ?? "";

                if (dt.Rows.Count == 0)
                {
                    Msg.Bao($"\"{txtTen.Text}\" chưa được dùng trong công thức món nào.");
                    return;
                }

                var dong = dt.AsEnumerable().Take(15).Select(r =>
                    $"• {r["MaSKU"]}  {r["TenSanPham"]}: {Convert.ToDecimal(r["SoLuongTrenMotPhan"]):0.######} {dv}");
                string them = dt.Rows.Count > 15 ? $"\n… và {dt.Rows.Count - 15} món khác" : "";
                Msg.Bao($"\"{txtTen.Text}\" được dùng trong {dt.Rows.Count} món:\n\n{string.Join("\n", dong)}{them}");
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        // ==================================================================
        //  TAB 2: CÔNG THỨC MÓN
        // ==================================================================

        private void NapComboNL()
        {
            cboNLCT.BindList(NguyenLieuService.GetCombo(), "TenNguyenLieu", "MaNguyenLieu");
            cboNLCT.SelectedIndex = -1;
            cboNLCT.Text = "";
            HienDonViCT();
        }

        private void HienDonViCT()
        {
            lblDV.Text = cboNLCT.SelectedItem is DataRowView r ? Convert.ToString(r["TenDonVi"]) : "—";
        }

        // ---------- Danh sách món bên trái ----------
        private void NapMon(int? chonMaSP)
        {
            try
            {
                _dangNap = true;
                var dt = NguyenLieuService.GetSanPham(txtTimMon.Text.Trim());
                dgvMon.DataSource = dt;

                dgvMon.HideCols("MaSanPham");
                dgvMon.Heads(
                    ("MaSKU", "Mã"),
                    ("TenSanPham", "Món"),
                    ("TenDanhMuc", "Danh mục"),
                    ("SoNguyenLieu", "Số NL"));
                dgvMon.Columns["MaSKU"]!.FillWeight = 60;
                dgvMon.Columns["TenSanPham"]!.FillWeight = 200;
                dgvMon.Columns["TenDanhMuc"]!.FillWeight = 110;
                dgvMon.Columns["SoNguyenLieu"]!.FillWeight = 55;
                dgvMon.Columns["SoNguyenLieu"]!.AsNumber("N0");

                lblMonTong.Text = $"{dt.Rows.Count} món";
                ChonDongMon(chonMaSP);
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

            MoDongHienTai();
        }

        private bool ChonDongMon(int? ma)
        {
            if (ma == null) return false;
            foreach (DataGridViewRow r in dgvMon.Rows)
            {
                if (Convert.ToInt32(r.Cells["MaSanPham"].Value) == ma)
                {
                    dgvMon.CurrentCell = r.Cells["TenSanPham"];
                    return true;
                }
            }
            return false;
        }

        private void MoDongHienTai()
        {
            if (dgvMon.CurrentRow == null) { DongCongThuc(); return; }
            var c = dgvMon.CurrentRow.Cells;
            MoCongThuc(Convert.ToInt32(c["MaSanPham"].Value), Convert.ToString(c["TenSanPham"].Value) ?? "");
        }

        /// <summary>Nếu đang có thay đổi chưa lưu thì hỏi; trả về true nếu được phép rời đi (và bỏ thay đổi).</summary>
        private bool ChoPhepBoThayDoi()
        {
            if (!_daSua) return true;
            if (Msg.Hoi($"Công thức món \"{_tenMon}\" đang sửa chưa lưu.\nBỏ các thay đổi này?"))
            {
                _daSua = false;
                return true;
            }
            return false;
        }

        private void TimMonThayDoi()
        {
            if (_dangNap) return;
            if (!ChoPhepBoThayDoi())
            {
                _dangNap = true;                      // hoàn lại nội dung ô tìm, không nạp lại
                txtTimMon.Text = _timCu;
                txtTimMon.SelectionStart = txtTimMon.Text.Length;
                _dangNap = false;
                return;
            }
            _timCu = txtTimMon.Text;
            NapMon(_maSP);
        }

        private void DgvMon_SelectionChanged(object? sender, EventArgs e)
        {
            if (_dangNap || dgvMon.CurrentRow == null) return;

            int maMoi = Convert.ToInt32(dgvMon.CurrentRow.Cells["MaSanPham"].Value);
            if (maMoi == _maSP) return;

            if (!ChoPhepBoThayDoi())
            {
                // Quay lại món cũ. Dùng BeginInvoke để tránh đổi ô hiện tại ngay trong sự kiện chọn dòng.
                BeginInvoke(() =>
                {
                    _dangNap = true;
                    ChonDongMon(_maSP);
                    _dangNap = false;
                });
                return;
            }
            MoDongHienTai();
        }

        // ---------- Công thức bên phải ----------
        private void MoCongThuc(int maSP, string tenMon)
        {
            try
            {
                _maSP = maSP;
                _tenMon = tenMon;
                _ct = NguyenLieuService.GetCongThuc(maSP);
                _daSua = false;

                dgvCT.DataSource = _ct;
                dgvCT.HideCols("MaNguyenLieu");
                dgvCT.Heads(
                    ("TenNguyenLieu", "Nguyên liệu"),
                    ("TenDonVi", "Đơn vị"),
                    ("SoLuongTrenMotPhan", "Định lượng / 1 phần"));
                dgvCT.Columns["TenNguyenLieu"]!.FillWeight = 220;
                dgvCT.Columns["TenDonVi"]!.FillWeight = 70;
                dgvCT.Columns["SoLuongTrenMotPhan"]!.FillWeight = 120;
                // Định dạng không có dấu phân cách hàng nghìn để ô sửa hiện đúng số (vd: 60 hoặc 0,5)
                dgvCT.Columns["SoLuongTrenMotPhan"]!.AsNumber("0.######");

                foreach (DataGridViewColumn col in dgvCT.Columns)   // chỉ ô Định lượng được sửa
                    col.ReadOnly = !(Session.LaQuanLy && col.Name == "SoLuongTrenMotPhan");

                lblTenMon.Text = $"Công thức: {tenMon}";
                CapNhatTrangThaiCT();
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        private void DongCongThuc()
        {
            _maSP = null;
            _tenMon = "";
            _ct = new DataTable();
            _daSua = false;
            dgvCT.DataSource = null;
            lblTenMon.Text = "Chưa chọn món";
            CapNhatTrangThaiCT();
        }

        private void DanhDauSua()
        {
            _daSua = true;
            CapNhatTrangThaiCT();
        }

        private void CapNhatTrangThaiCT()
        {
            if (_maSP == null)
            {
                lblCTTong.ForeColor = Theme.Muted;
                lblCTTong.Text = "Hãy chọn một món ở danh sách bên trái.";
                btnLuuCT.Enabled = btnHoanTac.Enabled = false;
                return;
            }

            lblCTTong.ForeColor = _daSua ? Theme.Warning : Theme.Muted;
            lblCTTong.Text = _daSua
                ? $"● Có thay đổi chưa lưu ({_ct.Rows.Count} nguyên liệu)"
                : $"{_ct.Rows.Count} nguyên liệu  •  nhấp đúp ô \"Định lượng\" để sửa";
            btnLuuCT.Enabled = btnHoanTac.Enabled = _daSua && Session.LaQuanLy;
        }

        private void ThemDongCT()
        {
            Err.Clear();
            if (_maSP == null)
            {
                Msg.Bao("Hãy chọn một món ở danh sách bên trái.");
                return;
            }
            if (cboNLCT.SelectedValue is not int maNL)
            {
                Err.SetError(cboNLCT, "Chọn nguyên liệu trong danh sách");
                cboNLCT.Focus();
                return;
            }
            if (numDL.Value <= 0)
            {
                Err.SetError(numDL, "Định lượng phải lớn hơn 0");
                numDL.Focus();
                return;
            }
            if (_ct.AsEnumerable().Any(r => Convert.ToInt32(r["MaNguyenLieu"]) == maNL))
            {
                Err.SetError(cboNLCT, "Nguyên liệu đã có trong công thức, hãy sửa định lượng ở bảng bên dưới");
                return;
            }

            var nl = (DataRowView)cboNLCT.SelectedItem!;
            // Thứ tự cột phải khớp NguyenLieuService.GetCongThuc: MaNguyenLieu, TenNguyenLieu, TenDonVi, SoLuongTrenMotPhan
            _ct.Rows.Add(maNL, nl["TenNguyenLieu"], nl["TenDonVi"], numDL.Value);

            numDL.Value = 0;
            cboNLCT.SelectedIndex = -1;
            cboNLCT.Text = "";
            HienDonViCT();
            cboNLCT.Focus();
            DanhDauSua();
        }

        private void XoaDongCT()
        {
            if (dgvCT.CurrentRow?.DataBoundItem is not DataRowView v) return;
            _ct.Rows.Remove(v.Row);
            DanhDauSua();
        }

        private void HoanTacCT()
        {
            if (_maSP == null) return;
            if (!Msg.Hoi("Bỏ mọi thay đổi chưa lưu và nạp lại công thức từ cơ sở dữ liệu?")) return;
            MoCongThuc(_maSP.Value, _tenMon);
        }

        private void LuuCT()
        {
            if (_maSP == null) return;
            if (!dgvCT.EndEdit()) return;             // ô đang sửa chưa hợp lệ thì dừng

            try
            {
                var dong = _ct.AsEnumerable()
                    .Select(r => (Convert.ToInt32(r["MaNguyenLieu"]), Convert.ToDecimal(r["SoLuongTrenMotPhan"])))
                    .ToList();

                NguyenLieuService.SaveCongThuc(_maSP.Value, dong);
                _daSua = false;

                NapMon(_maSP);                        // nạp lại cột "Số NL" bên trái và công thức bên phải
                NapDanhMuc(_ma);                      // cột "Số món dùng" ở tab 1 cũng đổi
                lblCTTong.ForeColor = Theme.Success;
                lblCTTong.Text = $"Đã lưu công thức \"{_tenMon}\" lúc {DateTime.Now:HH:mm:ss}.";
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        // ---------- Sửa định lượng ngay trong lưới ----------
        private void DgvCT_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvCT.Columns[e.ColumnIndex].Name != "SoLuongTrenMotPhan") return;

            // Chỉ nhận dấu thập phân theo máy (vi-VN là dấu phẩy); "0.5" bị từ chối để khỏi hiểu nhầm thành 5
            bool ok = decimal.TryParse(Convert.ToString(e.FormattedValue), NumberStyles.AllowDecimalPoint,
                                       CultureInfo.CurrentCulture, out decimal v) && v > 0;
            if (!ok)
            {
                e.Cancel = true;
                dgvCT.Rows[e.RowIndex].ErrorText = "Định lượng phải là số lớn hơn 0 (dùng dấu phẩy cho phần thập phân, ví dụ 0,5). Nhấn Esc để hoàn lại.";
            }
        }

        private void DgvCT_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            dgvCT.Rows[e.RowIndex].ErrorText = "";
            if (dgvCT.Columns[e.ColumnIndex].Name == "SoLuongTrenMotPhan") DanhDauSua();
        }

        // ==================================================================
        //  Phím tắt
        // ==================================================================
        private void Form_KeyDown(object? sender, KeyEventArgs e)
        {
            bool tab1 = tabMain.SelectedTab == tpDanhMuc;

            if (e.Control && e.KeyCode == Keys.S)
            {
                if (tab1) LuuNL(); else LuuCT();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F2 && tab1)
            {
                ThemMoiNL();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F5)
            {
                if (tab1) NapDanhMuc(_ma);
                else if (ChoPhepBoThayDoi()) NapMon(_maSP);
            }
        }
    }
}