using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms.Design;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class ucHuyHaoHut : UserControl, IRefreshable
    {
        private readonly DataTable _ct = new();                  // các dòng của phiếu đang lập
        private readonly ErrorProvider _err = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
        private int _maCN;
        private string _tenCN = "";
        private bool _dangNap;

        // phiếu đang chọn ở tab Danh sách
        private int? _maPhieu;
        private string _ttPhieu = "";

        public ucHuyHaoHut()
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
            dgvCT.ReadOnly = false;                       // sửa số lượng và ghi chú ngay trong lưới
            btnLuu.StylePrimary(); btnThemDong.StylePrimary(); btnDuyet.StylePrimary();
            btnXoaDong.StyleSecondary(); btnLamMoi.StyleSecondary(); btnLamMoiDS.StyleSecondary();
            btnTuChoi.StyleDanger();
            TrangThaiUi.Hook(dgvPhieu, "TrangThai");
            lblTon.ForeColor = Theme.Muted;
            lblDV.ForeColor = Theme.Muted;
            lblTong.ForeColor = Theme.Muted;
            lblTongPhieu.ForeColor = Theme.Muted;
            lblChiTiet.ForeColor = Theme.Text;

            txtSoPhieu.ReadOnly = txtNgay.ReadOnly = txtChiNhanh.ReadOnly = txtNguoiLap.ReadOnly = true;
            txtSoPhieu.Text = "(Tự động)";
            txtNgay.Text = "(Tự động: lúc lưu)";
            cboLyDo.DropDownStyle = ComboBoxStyle.DropDown;             // gợi ý nhưng vẫn gõ tự do được
            cboLyDo.Items.AddRange(new object[]
            {
                "Hết hạn sử dụng", "Hư hỏng, héo, dập", "Rơi vãi, đổ vỡ",
                "Mất lạnh, đổi màu", "Hao hụt khi chế biến", "Tồn cuối ngày, không dùng được nữa"
            });
            cboLocTT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocTT.Items.AddRange(new object[] { "Tất cả", "Chờ duyệt", "Đã duyệt", "Từ chối" });
            cboLocTT.SelectedIndex = 0;

            chkDuyetNgay.Visible = Session.LaQuanLy;      // chỉ quản lý mới có quyền duyệt ngay

            XayBangDong();

            // ----- sự kiện tab Lập phiếu -----
            cboLyDo.TextChanged += (_, _) => _err.SetError(cboLyDo, "");
            cboNL.SelectedIndexChanged += (_, _) => NLThayDoi();
            btnThemDong.Click += (_, _) => ThemDong();
            btnXoaDong.Click += (_, _) => XoaDong();
            btnLamMoi.Click += (_, _) => LamMoi();
            btnLuu.Click += (_, _) => LuuPhieu();
            dgvCT.CellValidating += DgvCT_CellValidating;
            dgvCT.CellEndEdit += DgvCT_CellEndEdit;
            dgvCT.DataError += (_, e) => e.ThrowException = false;

            // ----- sự kiện tab Danh sách -----
            tabHuy.SelectedIndexChanged += (_, _) =>
            {
                if (tabHuy.SelectedTab == tpDS) NapDanhSach(_maPhieu);
                else NapNL();                              // tồn có thể đã đổi sau khi duyệt
            };
            txtTimPhieu.TextChanged += (_, _) => NapDanhSach(null);
            cboLocTT.SelectedIndexChanged += (_, _) => NapDanhSach(null);
            btnLamMoiDS.Click += (_, _) => NapDanhSach(_maPhieu);
            dgvPhieu.SelectionChanged += (_, _) => ChonPhieu();
            dgvCTPhieu.CellFormatting += DgvCTPhieu_CellFormatting;
            btnDuyet.Click += (_, _) => Duyet();
            btnTuChoi.Click += (_, _) => TuChoi();
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
            txtNguoiLap.Text = Session.TenNhanVien;
            NapNL();
            NapDanhSach(_maPhieu);
        }

        private static string SoPhieu(int ma) => "HH" + ma.ToString("D5");

        // ==================================================================
        //  TAB 1: LẬP PHIẾU HỦY
        // ==================================================================

        private void XayBangDong()
        {
            _ct.Columns.Add("MaNguyenLieu", typeof(int));
            _ct.Columns.Add("TenNguyenLieu", typeof(string));
            _ct.Columns.Add("TenDonVi", typeof(string));
            _ct.Columns.Add("SoLuong", typeof(decimal));
            _ct.Columns.Add("TonHienTai", typeof(decimal));
            _ct.Columns.Add("GhiChu", typeof(string));

            dgvCT.DataSource = _ct;
            dgvCT.HideCols("MaNguyenLieu");
            dgvCT.Heads(
                ("TenNguyenLieu", "Nguyên liệu"),
                ("TenDonVi", "ĐVT"),
                ("SoLuong", "Số lượng hủy"),
                ("TonHienTai", "Tồn hiện tại"),
                ("GhiChu", "Ghi chú"));
            dgvCT.Columns["TenNguyenLieu"]!.FillWeight = 220;
            dgvCT.Columns["TenDonVi"]!.FillWeight = 60;
            dgvCT.Columns["SoLuong"]!.FillWeight = 100;
            dgvCT.Columns["TonHienTai"]!.FillWeight = 100;
            dgvCT.Columns["GhiChu"]!.FillWeight = 200;
            // Không dùng dấu phân cách hàng nghìn ở cột sửa được để ô sửa hiện đúng số (vd: 300 hoặc 2,5)
            dgvCT.Columns["SoLuong"]!.AsNumber("0.###");
            dgvCT.Columns["TonHienTai"]!.AsNumber("#,##0.###");

            foreach (DataGridViewColumn col in dgvCT.Columns)      // chỉ Số lượng và Ghi chú được sửa trực tiếp
                col.ReadOnly = !(col.Name is "SoLuong" or "GhiChu");

            CapNhatTong();
        }

        private void NapNL()
        {
            if (_maCN == 0) return;
            int? giu = cboNL.SelectedValue as int?;
            _dangNap = true;
            cboNL.BindList(LookupService.NguyenLieuCoTon(_maCN), "TenNguyenLieu", "MaNguyenLieu");
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
            lblTon.Text = $"Tồn: {Convert.ToDecimal(r["SoLuongTon"]):#,##0.###} {dv}";
        }

        private void ThemDong()
        {
            _err.Clear();

            if (cboNL.SelectedItem is not DataRowView nl || cboNL.SelectedValue is not int maNL)
            {
                _err.SetError(cboNL, "Chọn nguyên liệu trong danh sách");
                cboNL.Focus();
                return;
            }
            decimal ton = Convert.ToDecimal(nl["SoLuongTon"]);
            string dv = Convert.ToString(nl["TenDonVi"]) ?? "";

            if (numSL.Value <= 0)
            {
                _err.SetError(numSL, "Số lượng hủy phải lớn hơn 0");
                numSL.Focus();
                return;
            }
            if (numSL.Value > ton)
            {
                _err.SetError(numSL, $"Vượt quá tồn kho (chỉ còn {ton:#,##0.###} {dv})");
                numSL.Focus();
                return;
            }
            if (_ct.AsEnumerable().Any(r => Convert.ToInt32(r["MaNguyenLieu"]) == maNL))
            {
                _err.SetError(cboNL, "Nguyên liệu đã có trong phiếu, hãy sửa số lượng ở bảng bên dưới");
                return;
            }

            // Thứ tự khớp các cột: MaNguyenLieu, TenNguyenLieu, TenDonVi, SoLuong, TonHienTai, GhiChu
            _ct.Rows.Add(maNL, nl["TenNguyenLieu"], nl["TenDonVi"], numSL.Value, ton, txtGhiChuDong.Text.Trim());

            numSL.Value = 0;
            txtGhiChuDong.Clear();
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
            lblTong.Text = $"{_ct.Rows.Count} dòng";
        }

        private void LamMoi()
        {
            _err.Clear();
            _ct.Clear();
            cboLyDo.Text = "";
            txtGhiChuDong.Clear();
            numSL.Value = 0;
            chkDuyetNgay.Checked = false;
            _dangNap = true;
            cboNL.SelectedIndex = -1;
            cboNL.Text = "";
            _dangNap = false;
            NLThayDoi();
            CapNhatTong();
        }

        // ---------- Sửa số lượng ngay trong lưới ----------
        private void DgvCT_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvCT.Columns[e.ColumnIndex].Name != "SoLuong") return;

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

            if (dgvCT.Rows[e.RowIndex].DataBoundItem is DataRowView row)
            {
                decimal ton = Convert.ToDecimal(row["TonHienTai"]);
                if (v > ton)
                {
                    e.Cancel = true;
                    dgvCT.Rows[e.RowIndex].ErrorText =
                        $"Vượt quá tồn kho (chỉ còn {ton:#,##0.###}). Nhấn Esc để hoàn lại.";
                }
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

            string lyDo = cboLyDo.Text.Trim();
            if (lyDo.Length == 0)
            {
                _err.SetError(cboLyDo, "Nhập hoặc chọn lý do hủy");
                cboLyDo.Focus();
                return;
            }
            if (!dgvCT.EndEdit()) return;                  // ô đang sửa chưa hợp lệ thì dừng
            if (_ct.Rows.Count == 0)
            {
                _err.SetError(btnLuu, "Phiếu chưa có dòng nào");
                return;
            }

            int ma;
            try
            {
                ma = HuyService.TaoPhieu(_maCN, Session.MaNhanVien, lyDo, _ct);
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                return;
            }

            string so = SoPhieu(ma);
            if (chkDuyetNgay.Checked && Session.LaQuanLy)
            {
                try
                {
                    HuyService.Duyet(ma, Session.MaNhanVien);
                    Msg.Bao($"Đã lập và duyệt phiếu {so}. Kho đã bị trừ.");
                }
                catch (Exception ex)
                {
                    // Phiếu đã lưu nhưng chưa trừ kho: nói rõ để người dùng không lập trùng
                    Msg.Loi(new InvalidOperationException(
                        $"Đã lưu phiếu {so} (Nháp) nhưng chưa duyệt được:\n{ex.Message}"));
                }
            }
            else
            {
                Msg.Bao($"Đã lưu phiếu {so} (Nháp).\nKho chỉ bị trừ khi quản lý duyệt phiếu trong tab 'Danh sách phiếu'.");
            }

            LamMoi();
            NapNL();
            tabHuy.SelectedTab = tpDS;                     // SelectedIndexChanged sẽ nạp danh sách
            NapDanhSach(ma);                               // và chọn đúng phiếu vừa lập
        }

        // ==================================================================
        //  TAB 2: DANH SÁCH PHIẾU
        // ==================================================================

        private void NapDanhSach(int? chonMa)
        {
            if (_maCN == 0) return;
            string? tt = cboLocTT.SelectedIndex switch { 1 => "Draft", 2 => "DaDuyet", 3 => "TuChoi", _ => null };

            try
            {
                _dangNap = true;
                var dt = NapDanhSachDuLieu(tt);
                dgvPhieu.DataSource = dt;

                dgvPhieu.HideCols("MaPhieu");
                dgvPhieu.Heads(
                    ("SoPhieu", "Số phiếu"),
                    ("NgayLap", "Ngày lập"),
                    ("NguoiLap", "Người lập"),
                    ("LyDo", "Lý do"),
                    ("SoDong", "Số dòng"),
                    ("TrangThai", "Trạng thái"));
                dgvPhieu.Columns["SoPhieu"]!.FillWeight = 75;
                dgvPhieu.Columns["NgayLap"]!.FillWeight = 120;
                dgvPhieu.Columns["NguoiLap"]!.FillWeight = 130;
                dgvPhieu.Columns["LyDo"]!.FillWeight = 330;
                dgvPhieu.Columns["SoDong"]!.FillWeight = 65;
                dgvPhieu.Columns["TrangThai"]!.FillWeight = 95;
                dgvPhieu.Columns["NgayLap"]!.AsDate("dd/MM/yyyy HH:mm");
                dgvPhieu.Columns["SoDong"]!.AsNumber("N0");

                int cho = dt.AsEnumerable().Count(r => Convert.ToString(r["TrangThai"]) == "Draft");
                lblTongPhieu.ForeColor = cho > 0 ? Theme.Warning : Theme.Muted;
                lblTongPhieu.Text = $"{dt.Rows.Count} phiếu   •   {cho} chờ duyệt";

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

        private DataTable NapDanhSachDuLieu(string? tt) => HuyService.DanhSach(_maCN, tt, txtTimPhieu.Text.Trim());

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
            _maPhieu = Convert.ToInt32(c["MaPhieu"].Value);
            _ttPhieu = Convert.ToString(c["TrangThai"].Value) ?? "";

            dgvCTPhieu.DataSource = HuyService.GetChiTiet(_maPhieu.Value);
            dgvCTPhieu.Heads(
                ("TenNguyenLieu", "Nguyên liệu"),
                ("TenDonVi", "ĐVT"),
                ("SoLuong", "Số lượng hủy"),
                ("TonHienTai", "Tồn hiện tại"),
                ("GhiChu", "Ghi chú"));
            dgvCTPhieu.Columns["SoLuong"]!.AsNumber("#,##0.###");
            dgvCTPhieu.Columns["TonHienTai"]!.AsNumber("#,##0.###");

            lblChiTiet.Text = $"Chi tiết phiếu {Convert.ToString(c["SoPhieu"].Value)}  •  Người lập: {Convert.ToString(c["NguoiLap"].Value)}";
            CapNhatNut();
        }

        /// <summary>Phiếu Nháp: đánh dấu đỏ dòng có số hủy lớn hơn tồn hiện tại (duyệt sẽ bị từ chối).</summary>
        private void DgvCTPhieu_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || _ttPhieu != "Draft") return;
            if (dgvCTPhieu.Columns[e.ColumnIndex].Name != "TonHienTai") return;

            var cells = dgvCTPhieu.Rows[e.RowIndex].Cells;
            if (Convert.ToDecimal(cells["SoLuong"].Value) > Convert.ToDecimal(cells["TonHienTai"].Value))
            {
                e.CellStyle!.ForeColor = Theme.Danger;
                e.CellStyle.Font = Theme.Bold;
            }
        }

        private void CapNhatNut()
        {
            bool choDuyet = _maPhieu != null && _ttPhieu == "Draft" && Session.LaQuanLy;
            btnDuyet.Enabled = choDuyet;
            btnTuChoi.Enabled = choDuyet;
        }

        private void Duyet()
        {
            if (_maPhieu == null) return;
            int ma = _maPhieu.Value;
            if (!Msg.Hoi($"Duyệt phiếu {SoPhieu(ma)}?\n\nKho sẽ bị trừ ngay và không thể hoàn tác.")) return;

            try
            {
                HuyService.Duyet(ma, Session.MaNhanVien);
                NapDanhSach(ma);
                NapNL();                                   // tồn ở tab Lập phiếu đã đổi
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);                               // có thể là thiếu tồn (phiếu vẫn là Nháp)
                NapDanhSach(ma);
            }
        }

        private void TuChoi()
        {
            if (_maPhieu == null) return;
            int ma = _maPhieu.Value;
            if (!Msg.Hoi($"Từ chối phiếu {SoPhieu(ma)}?\n\nKho không bị ảnh hưởng và phiếu không thể duyệt lại.")) return;

            try
            {
                HuyService.TuChoi(ma);
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