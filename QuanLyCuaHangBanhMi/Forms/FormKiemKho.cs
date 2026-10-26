using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using System.Data;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormKiemKho : FrmBase
    {
        private int? _maPhieu;                                   // phiếu đang mở
        private string _tt = "";
        private DataTable? _ct;
        private bool _dangNap;                                   // chặn sự kiện khi đang nạp dữ liệu

        private int MaCN => cboChiNhanh.SelectedValue is int v ? v : Session.MaChiNhanh;

        /// <summary>null = ADMIN (không giới hạn); còn lại chỉ thao tác cho chi nhánh của mình.</summary>
        private static int? GioiHanCN => Session.LaAdmin ? null : Session.MaChiNhanh;

        public FormKiemKho()
        {
            InitializeComponent();

            // ----- giao diện -----
            dgvPhieu.ApplyTheme();
            dgvCT.ApplyTheme();
            dgvCT.ReadOnly = false;                       // chỉ cột "SL thực tế" được mở khi phiếu đang kiểm
            btnBatDau.StylePrimary();
            btnHoanThanh.StylePrimary();
            btnXoaPhieu.StyleDanger();
            TrangThaiUi.Hook(dgvPhieu, "TrangThai");
            lblPhieuTong.ForeColor = Theme.Muted;
            lblTomTat.ForeColor = Theme.Muted;
            foreach (var t in new[] { txtSoPhieu, txtTrangThai, txtNgay, txtNguoi, txtGhiChu }) t.ReadOnly = true;
            cboChiNhanh.DropDownStyle = ComboBoxStyle.DropDownList;
            KeyPreview = true;

            // ----- sự kiện -----
            cboChiNhanh.SelectedIndexChanged += (_, _) => { if (!_dangNap) NapDanhSach(null); };
            dgvPhieu.SelectionChanged += (_, _) => MoPhieu();
            btnBatDau.Click += (_, _) => BatDau();
            btnHoanThanh.Click += (_, _) => HoanThanh();
            btnXoaPhieu.Click += (_, _) => XoaPhieu();
            chkChiLech.CheckedChanged += (_, _) => LocDong();
            txtTimNL.TextChanged += (_, _) => LocDong();
            dgvCT.CellValidating += DgvCT_CellValidating;
            dgvCT.CellEndEdit += DgvCT_CellEndEdit;
            dgvCT.CellFormatting += DgvCT_CellFormatting;
            dgvCT.DataError += (_, e) => e.ThrowException = false;
            KeyDown += (_, e) => { if (e.KeyCode == Keys.F5) NapDanhSach(_maPhieu); };
        }

        /// <summary>FrmBase tự gọi khi form mở (sự kiện Load).</summary>
        public override void RefreshData()
        {
            int giu = cboChiNhanh.SelectedValue is int g ? g : Session.MaChiNhanh;
            _dangNap = true;
            cboChiNhanh.BindList(LookupService.ChiNhanh(), "TenChiNhanh", "MaChiNhanh");
            cboChiNhanh.SelectedValue = Session.LaAdmin ? giu : Session.MaChiNhanh;
            cboChiNhanh.Enabled = Session.LaAdmin;          // chỉ ADMIN được đổi chi nhánh
            _dangNap = false;

            NapDanhSach(_maPhieu);
        }

        private static string SoPhieu(int ma) => "KK" + ma.ToString("D5");

        private static string TenTT(string tt) => tt switch
        { "DangKiem" => "Đang kiểm", "HoanThanh" => "Hoàn thành", _ => tt };

        // ==================================================================
        //  Danh sách phiếu (bên trái)
        // ==================================================================
        private void NapDanhSach(int? chonMa)
        {
            try
            {
                _dangNap = true;
                var dt = KiemKhoService.DanhSach(MaCN);
                dgvPhieu.DataSource = dt;

                dgvPhieu.HideCols("MaPhieu", "GhiChu");
                dgvPhieu.Heads(
                    ("SoPhieu", "Số phiếu"),
                    ("NgayKiem", "Ngày kiểm"),
                    ("NguoiKiem", "Người kiểm"),
                    ("TrangThai", "Trạng thái"),
                    ("SoDong", "Số dòng"),
                    ("SoLech", "Dòng lệch"));
                dgvPhieu.Columns["SoPhieu"]!.FillWeight = 75;
                dgvPhieu.Columns["NgayKiem"]!.FillWeight = 120;
                dgvPhieu.Columns["NguoiKiem"]!.FillWeight = 110;
                dgvPhieu.Columns["TrangThai"]!.FillWeight = 100;
                dgvPhieu.Columns["SoDong"]!.FillWeight = 55;
                dgvPhieu.Columns["SoLech"]!.FillWeight = 60;
                dgvPhieu.Columns["NgayKiem"]!.AsDate("dd/MM/yyyy HH:mm");
                dgvPhieu.Columns["SoDong"]!.AsNumber("N0");
                dgvPhieu.Columns["SoLech"]!.AsNumber("N0");

                int dang = dt.AsEnumerable().Count(r => Convert.ToString(r["TrangThai"]) == "DangKiem");
                lblPhieuTong.ForeColor = dang > 0 ? Theme.Warning : Theme.Muted;
                lblPhieuTong.Text = $"{dt.Rows.Count} phiếu   •   {dang} đang kiểm";

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

            MoPhieu();
        }

        // ==================================================================
        //  Chi tiết phiếu (bên phải)
        // ==================================================================
        private void MoPhieu()
        {
            if (_dangNap) return;

            if (dgvPhieu.CurrentRow == null)
            {
                _maPhieu = null;
                _tt = "";
                _ct = null;
                dgvCT.DataSource = null;
                txtSoPhieu.Clear(); txtTrangThai.Clear(); txtNgay.Clear(); txtNguoi.Clear(); txtGhiChu.Clear();
                lblTomTat.Text = "";
                CapNhatNut();
                return;
            }

            var c = dgvPhieu.CurrentRow.Cells;
            _maPhieu = Convert.ToInt32(c["MaPhieu"].Value);
            _tt = Convert.ToString(c["TrangThai"].Value) ?? "";
            txtSoPhieu.Text = Convert.ToString(c["SoPhieu"].Value);
            txtTrangThai.Text = TenTT(_tt);
            txtNgay.Text = Convert.ToDateTime(c["NgayKiem"].Value).ToString("dd/MM/yyyy HH:mm");
            txtNguoi.Text = Convert.ToString(c["NguoiKiem"].Value);
            txtGhiChu.Text = Convert.ToString(c["GhiChu"].Value);

            try
            {
                _ct = KiemKhoService.GetChiTiet(_maPhieu.Value);
                dgvCT.DataSource = _ct;
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                return;
            }

            bool sua = _tt == "DangKiem";
            dgvCT.HideCols("MaChiTiet", "MaNguyenLieu");
            dgvCT.Heads(
                ("TenNguyenLieu", "Nguyên liệu"),
                ("TenDonVi", "ĐVT"),
                ("SoLuongHeThong", "SL hệ thống"),
                ("SoLuongThucTe", "SL thực tế"),
                ("ChenhLech", "Chênh lệch"),
                ("TonHienTai", "Tồn hiện tại"));
            dgvCT.Columns["TenNguyenLieu"]!.FillWeight = 230;
            dgvCT.Columns["TenDonVi"]!.FillWeight = 60;
            dgvCT.Columns["SoLuongHeThong"]!.FillWeight = 105;
            dgvCT.Columns["SoLuongThucTe"]!.FillWeight = 105;
            dgvCT.Columns["ChenhLech"]!.FillWeight = 100;
            dgvCT.Columns["TonHienTai"]!.FillWeight = 105;
            dgvCT.Columns["SoLuongHeThong"]!.AsNumber("#,##0.###");
            // Cột sửa được không dùng dấu phân cách hàng nghìn để ô sửa hiện đúng số (vd: 3070 hoặc 2,5)
            dgvCT.Columns["SoLuongThucTe"]!.AsNumber("0.###");
            dgvCT.Columns["ChenhLech"]!.AsNumber("+#,##0.###;-#,##0.###;0");
            dgvCT.Columns["TonHienTai"]!.AsNumber("#,##0.###");

            // "Tồn hiện tại" chỉ có ý nghĩa khi phiếu còn đang kiểm
            dgvCT.Columns["TonHienTai"]!.Visible = sua;

            foreach (DataGridViewColumn col in dgvCT.Columns)       // chỉ ô "SL thực tế" sửa được, và chỉ khi đang kiểm
            {
                bool choSua = sua && col.Name == "SoLuongThucTe";
                col.ReadOnly = !choSua;
                if (choSua) col.DefaultCellStyle.BackColor = Color.FromArgb(255, 251, 235);   // nền vàng nhạt cho ô nhập
            }

            LocDong();
            CapNhatTomTat();
            CapNhatNut();
        }

        private void CapNhatNut()
        {
            bool dangKiem = _maPhieu != null && _tt == "DangKiem";
            btnHoanThanh.Enabled = dangKiem && Session.LaQuanLy;
            btnXoaPhieu.Enabled = dangKiem && Session.LaQuanLy;
        }

        private void CapNhatTomTat()
        {
            if (_ct == null) { lblTomTat.Text = ""; return; }

            int lech = _ct.AsEnumerable().Count(r => Convert.ToDecimal(r["ChenhLech"]) != 0);
            lblTomTat.ForeColor = lech > 0 && _tt == "DangKiem" ? Theme.Warning : Theme.Muted;
            lblTomTat.Text = $"{_ct.Rows.Count} nguyên liệu   •   {lech} dòng lệch" +
                             (_tt == "DangKiem" ? "   •   Nhấp đúp ô \"SL thực tế\" để nhập; số được lưu ngay" : "");
        }

        private void LocDong()
        {
            if (_ct == null) return;

            var dk = new List<string>();
            if (chkChiLech.Checked) dk.Add("ChenhLech <> 0");
            string k = txtTimNL.Text.Trim();
            if (k.Length > 0) dk.Add($"TenNguyenLieu LIKE '%{EscLike(k)}%'");
            _ct.DefaultView.RowFilter = string.Join(" AND ", dk);
        }

        /// <summary>Escape ký tự đặc biệt cho biểu thức LIKE của DataView.RowFilter.</summary>
        private static string EscLike(string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s)
                sb.Append(c switch
                {
                    '\'' => "''",
                    '[' => "[[]",
                    ']' => "[]]",
                    '*' => "[*]",
                    '%' => "[%]",
                    _ => c.ToString()
                });
            return sb.ToString();
        }

        // ==================================================================
        //  Nhập số thực tế (lưu ngay từng ô)
        // ==================================================================
        private void DgvCT_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvCT.Columns[e.ColumnIndex].Name != "SoLuongThucTe") return;

            // Chỉ nhận dấu thập phân theo máy (vi-VN là dấu phẩy) để "0.5" không bị hiểu thành 5
            bool ok = decimal.TryParse(Convert.ToString(e.FormattedValue), NumberStyles.AllowDecimalPoint,
                                       CultureInfo.CurrentCulture, out decimal v) && v >= 0;
            if (!ok)
            {
                e.Cancel = true;
                dgvCT.Rows[e.RowIndex].ErrorText =
                    "Số thực tế phải là số không âm (dùng dấu phẩy cho phần thập phân, ví dụ 2,5). Nhấn Esc để hoàn lại.";
            }
        }

        private void DgvCT_CellEndEdit(object? sender, DataGridViewCellEventArgs e)
        {
            dgvCT.Rows[e.RowIndex].ErrorText = "";
            if (dgvCT.Columns[e.ColumnIndex].Name != "SoLuongThucTe") return;
            if (dgvCT.Rows[e.RowIndex].DataBoundItem is not DataRowView drv) return;

            try
            {
                decimal thucTe = Convert.ToDecimal(drv["SoLuongThucTe"]);
                KiemKhoService.CapNhatThucTe(Convert.ToInt32(drv["MaChiTiet"]), thucTe);     // lưu ngay vào DB
                drv["ChenhLech"] = thucTe - Convert.ToDecimal(drv["SoLuongHeThong"]);         // cập nhật cột lệch trên màn hình
                CapNhatTomTat();
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                MoPhieu();                                    // nạp lại cho khớp với DB
            }
        }

        private void DgvCT_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgvCT.Columns[e.ColumnIndex].Name;

            if (col == "ChenhLech" && e.Value is decimal d)
            {
                var (fore, back) = d < 0 ? (Theme.Danger, Color.FromArgb(254, 226, 226))
                                 : d > 0 ? (Theme.Success, Color.FromArgb(220, 252, 231))
                                         : (Theme.Muted, Color.White);
                e.CellStyle!.ForeColor = fore;
                e.CellStyle.BackColor = back;
            }
            else if (col == "TonHienTai" && _tt == "DangKiem")
            {
                // Tồn đã đổi kể từ lúc bắt đầu kiểm (có nhập, bán, hủy...) thì tô cam để người kiểm biết
                var cells = dgvCT.Rows[e.RowIndex].Cells;
                if (Convert.ToDecimal(cells["TonHienTai"].Value) != Convert.ToDecimal(cells["SoLuongHeThong"].Value))
                {
                    e.CellStyle!.ForeColor = Theme.Warning;
                    e.CellStyle.Font = Theme.Bold;
                }
            }
        }

        // ==================================================================
        //  Hành động
        // ==================================================================
        private void BatDau()
        {
            try
            {
                var nl = KiemKhoService.NguyenLieuDeKiem(MaCN);
                using var dlg = new KiemKhoMoiDialog(nl, cboChiNhanh.Text);
                if (dlg.ShowDialog(this) != DialogResult.OK) return;

                int ma = KiemKhoService.TaoPhieu(MaCN, Session.MaNhanVien, dlg.GhiChu, dlg.DaChon, GioiHanCN);
                NapDanhSach(ma);
                Msg.Bao($"Đã tạo phiếu {SoPhieu(ma)} ({dlg.DaChon.Count} nguyên liệu).\n\n" +
                        "Hãy đếm hàng và nhập số thực tế vào cột \"SL thực tế\" (nhấp đúp ô). Số đã nhập được lưu ngay.");
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        private void HoanThanh()
        {
            if (_maPhieu == null || _ct == null) return;
            if (!dgvCT.EndEdit()) return;                    // ô đang sửa chưa hợp lệ thì dừng
            int ma = _maPhieu.Value;

            int lech = _ct.AsEnumerable().Count(r => Convert.ToDecimal(r["ChenhLech"]) != 0);
            int doiTon = _ct.AsEnumerable().Count(r =>
                Convert.ToDecimal(r["TonHienTai"]) != Convert.ToDecimal(r["SoLuongHeThong"]));

            var sb = new StringBuilder();
            sb.AppendLine(lech == 0
                ? "Không có dòng nào chênh lệch, tồn kho sẽ không thay đổi."
                : $"Có {lech} dòng chênh lệch. Tồn kho sẽ được điều chỉnh theo mức chênh lệch.");
            if (doiTon > 0)
                sb.AppendLine($"\nLưu ý: {doiTon} nguyên liệu đã có nhập, xuất hoặc hủy kể từ lúc bắt đầu kiểm " +
                              "(cột \"Tồn hiện tại\" màu cam). Điều chỉnh vẫn tính theo chênh lệch, không ghi đè bằng số thực tế.");
            sb.AppendLine($"\nHoàn thành phiếu {SoPhieu(ma)}? Việc này không thể hoàn tác và phiếu sẽ bị khóa.");

            if (!Msg.Hoi(sb.ToString())) return;

            try
            {
                int n = KiemKhoService.HoanThanh(ma, Session.MaNhanVien, GioiHanCN);
                NapDanhSach(ma);
                Msg.Bao(n == 0
                    ? $"Đã hoàn thành phiếu {SoPhieu(ma)}. Không có chênh lệch nên kho không đổi."
                    : $"Đã hoàn thành phiếu {SoPhieu(ma)}. Đã điều chỉnh tồn kho cho {n} nguyên liệu.");
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);                                  // có thể là mức giảm lớn hơn tồn hiện tại (phiếu vẫn Đang kiểm)
                NapDanhSach(ma);
            }
        }

        private void XoaPhieu()
        {
            if (_maPhieu == null) return;
            int ma = _maPhieu.Value;
            if (!Msg.Hoi($"Xóa phiếu kiểm kho {SoPhieu(ma)}?\n\nSố đã nhập sẽ mất. Kho không bị ảnh hưởng.")) return;

            try
            {
                KiemKhoService.XoaPhieu(ma, GioiHanCN);
                _maPhieu = null;
                NapDanhSach(null);
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
                NapDanhSach(ma);
            }
        }
    }
}