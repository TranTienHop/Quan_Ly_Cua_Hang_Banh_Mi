using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormThietBi : FrmBase
    {
        private int? _ma;                                   // null = đang thêm mới
        private string _ttCu = "DangDung";                  // tình trạng đang lưu trong DB của dòng đang chọn
        private bool _dangNap;                              // chặn sự kiện khi đang nạp dữ liệu / bỏ chọn dòng
        private string _maGoiYCu = "";                      // mã gợi ý gần nhất (để biết có được tự đổi theo chi nhánh không)

        /// <summary>null = ADMIN (không giới hạn); còn lại chỉ thao tác cho chi nhánh của mình.</summary>
        private static int? GioiHanCN => Session.LaAdmin ? null : Session.MaChiNhanh;

        public FormThietBi()
        {
            InitializeComponent();

            // ----- giao diện -----
            dgv.ApplyTheme();
            btnLuu.StylePrimary();
            btnMoi.StyleSecondary();
            btnGoiY.StyleSecondary();
            TrangThaiUi.Hook(dgv, "TinhTrang");
            lblTongKet.ForeColor = Theme.Muted;
            lblGiaTri.ForeColor = Theme.Muted;
            KeyPreview = true;
            grpChiTiet.Enabled = Session.LaQuanLy;           // menu đã chặn STAFF, đây là lớp bảo vệ thứ hai

            foreach (var c in new[] { cboLocCN, cboLocLoai, cboLocTT, cboChiNhanh, cboLoai, cboTinhTrang })
                c.DropDownStyle = ComboBoxStyle.DropDownList;

            // Danh sách cố định (không cần truy vấn DB)
            cboLocLoai.BindList(ThietBiService.Pairs(ThietBiService.Loai, "(Tất cả loại)"), "Text", "Code");
            cboLocTT.BindList(ThietBiService.Pairs(ThietBiService.TinhTrang, "(Tất cả tình trạng)"), "Text", "Code");
            cboLoai.BindList(ThietBiService.Pairs(ThietBiService.Loai), "Text", "Code");
            cboTinhTrang.BindList(ThietBiService.Pairs(ThietBiService.TinhTrang), "Text", "Code");

            // ----- sự kiện -----
            txtTim.TextChanged += (_, _) => NapDanhSach(null);
            cboLocCN.SelectedIndexChanged += (_, _) => NapDanhSach(null);
            cboLocLoai.SelectedIndexChanged += (_, _) => NapDanhSach(null);
            cboLocTT.SelectedIndexChanged += (_, _) => NapDanhSach(null);
            dgv.SelectionChanged += (_, _) => HienChiTiet();
            dgv.CellFormatting += Dgv_CellFormatting;
            btnMoi.Click += (_, _) => ThemMoi();
            btnLuu.Click += (_, _) => Luu();
            btnGoiY.Click += (_, _) => GoiYMa();
            cboChiNhanh.SelectedIndexChanged += (_, _) => ChiNhanhThayDoi();
            numSoLuong.ValueChanged += (_, _) => CapNhatGiaTri();
            numDonGia.ValueChanged += (_, _) => CapNhatGiaTri();
            txtMaCode.TextChanged += (_, _) => Err.SetError(txtMaCode, "");
            txtTen.TextChanged += (_, _) => Err.SetError(txtTen, "");
            cboLoai.SelectedIndexChanged += (_, _) => Err.SetError(cboLoai, "");
            dtpNgayMua.ValueChanged += (_, _) => Err.SetError(dtpNgayMua, "");
            KeyDown += Form_KeyDown;
        }

        /// <summary>FrmBase tự gọi khi form mở (sự kiện Load).</summary>
        public override void RefreshData()
        {
            int giuLoc = cboLocCN.SelectedValue is int g ? g : -1;
            int giuForm = cboChiNhanh.SelectedValue is int f ? f : Session.MaChiNhanh;

            _dangNap = true;
            var dtLoc = LookupService.ChiNhanh();
            if (Session.LaAdmin) dtLoc.ThemTatCa("TenChiNhanh", "MaChiNhanh", -1, "(Tất cả chi nhánh)");
            cboLocCN.BindList(dtLoc, "TenChiNhanh", "MaChiNhanh");
            cboLocCN.SelectedValue = Session.LaAdmin ? giuLoc : Session.MaChiNhanh;
            cboLocCN.Enabled = Session.LaAdmin;               // quản lý chỉ xem chi nhánh của mình

            cboChiNhanh.BindList(LookupService.ChiNhanh(), "TenChiNhanh", "MaChiNhanh");
            cboChiNhanh.SelectedValue = giuForm;
            _dangNap = false;

            NapDanhSach(_ma);
        }

        // ==================================================================
        //  Danh sách (bên trái)
        // ==================================================================
        private void NapDanhSach(int? chonMa)
        {
            if (_dangNap) return;

            int? cn = GioiHanCN ?? (cboLocCN.SelectedValue is int v && v > 0 ? v : null);
            string loai = cboLocLoai.SelectedValue as string ?? "";
            string tt = cboLocTT.SelectedValue as string ?? "";

            try
            {
                _dangNap = true;
                var dt = ThietBiService.GetAll(cn, loai, tt, txtTim.Text.Trim());
                dgv.DataSource = dt;

                dgv.HideCols("MaThietBi", "MaChiNhanh", "GhiChu");
                dgv.Heads(
                    ("MaCode", "Mã"),
                    ("TenChiNhanh", "Chi nhánh"),
                    ("TenThietBi", "Tên thiết bị"),
                    ("LoaiThietBi", "Loại"),
                    ("SoLuong", "SL"),
                    ("NgayMua", "Ngày mua"),
                    ("DonGiaMua", "Đơn giá mua"),
                    ("GiaTri", "Giá trị"),
                    ("TinhTrang", "Tình trạng"));
                dgv.Columns["MaCode"]!.FillWeight = 90;
                dgv.Columns["TenChiNhanh"]!.FillWeight = 120;
                dgv.Columns["TenThietBi"]!.FillWeight = 220;
                dgv.Columns["LoaiThietBi"]!.FillWeight = 105;
                dgv.Columns["SoLuong"]!.FillWeight = 45;
                dgv.Columns["NgayMua"]!.FillWeight = 85;
                dgv.Columns["DonGiaMua"]!.FillWeight = 95;
                dgv.Columns["GiaTri"]!.FillWeight = 100;
                dgv.Columns["TinhTrang"]!.FillWeight = 100;
                dgv.Columns["NgayMua"]!.AsDate();
                dgv.Columns["SoLuong"]!.AsNumber("N0");
                dgv.Columns["DonGiaMua"]!.AsNumber("N0");
                dgv.Columns["GiaTri"]!.AsNumber("N0");

                CapNhatTongKet(dt);

                if (chonMa != null)
                {
                    foreach (DataGridViewRow r in dgv.Rows)
                    {
                        if (Convert.ToInt32(r.Cells["MaThietBi"].Value) == chonMa)
                        {
                            dgv.CurrentCell = r.Cells["TenThietBi"];
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

            HienChiTiet();
        }

        private void CapNhatTongKet(DataTable dt)
        {
            // Thiết bị đã thanh lý không còn là tài sản nên không tính vào số lượng và giá trị
            var conQuanLy = dt.AsEnumerable().Where(r => Convert.ToString(r["TinhTrang"]) != "ThanhLy").ToList();
            int tongSL = conQuanLy.Sum(r => Convert.ToInt32(r["SoLuong"]));
            decimal giaTri = conQuanLy.Where(r => r["GiaTri"] != DBNull.Value).Sum(r => Convert.ToDecimal(r["GiaTri"]));
            int chuY = conQuanLy.Count(r => Convert.ToString(r["TinhTrang"]) is "BaoTri" or "HuHong");

            lblTongKet.ForeColor = chuY > 0 ? Theme.Warning : Theme.Muted;
            lblTongKet.Text = $"{dt.Rows.Count} dòng   •   {tongSL:N0} thiết bị đang quản lý   •   Giá trị mua {giaTri:N0} đ" +
                              (chuY > 0 ? $"   •   {chuY} cần chú ý (bảo trì / hư hỏng)" : "");
        }

        private void Dgv_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value is not string code) return;
            if (dgv.Columns[e.ColumnIndex].Name == "LoaiThietBi")
            {
                e.Value = ThietBiService.NhanLoai(code);
                e.FormattingApplied = true;
            }
        }

        // ==================================================================
        //  Panel nhập liệu (bên phải)
        // ==================================================================
        private void HienChiTiet()
        {
            if (_dangNap) return;
            if (dgv.CurrentRow == null) { XoaTrang(); return; }

            var c = dgv.CurrentRow.Cells;
            _ma = Convert.ToInt32(c["MaThietBi"].Value);
            _ttCu = Convert.ToString(c["TinhTrang"].Value) ?? "DangDung";

            Err.Clear();
            _dangNap = true;
            txtMaCode.Text = Convert.ToString(c["MaCode"].Value);
            cboChiNhanh.SelectedValue = Convert.ToInt32(c["MaChiNhanh"].Value);
            txtTen.Text = Convert.ToString(c["TenThietBi"].Value);
            cboLoai.SelectedValue = Convert.ToString(c["LoaiThietBi"].Value) ?? "";
            numSoLuong.Value = Math.Max(numSoLuong.Minimum, Math.Min(numSoLuong.Maximum, Convert.ToDecimal(c["SoLuong"].Value)));
            if (c["NgayMua"].Value is DateTime d) { dtpNgayMua.Value = d; dtpNgayMua.Checked = true; }
            else dtpNgayMua.Checked = false;
            numDonGia.Value = c["DonGiaMua"].Value is decimal g ? Math.Min(numDonGia.Maximum, g) : 0;
            cboTinhTrang.SelectedValue = _ttCu;
            txtGhiChu.Text = Convert.ToString(c["GhiChu"].Value) ?? "";
            _dangNap = false;

            // Mã và chi nhánh chỉ đặt một lần khi tạo
            txtMaCode.ReadOnly = true;
            btnGoiY.Enabled = false;
            cboChiNhanh.Enabled = false;
            CapNhatGiaTri();
        }

        private void XoaTrang()
        {
            _ma = null;
            _ttCu = "DangDung";
            Err.Clear();

            _dangNap = true;
            txtTen.Clear();
            txtGhiChu.Clear();
            cboLoai.SelectedIndex = -1;                      // bắt buộc người dùng chọn
            cboTinhTrang.SelectedValue = "DangDung";
            numSoLuong.Value = 1;
            numDonGia.Value = 0;
            dtpNgayMua.Value = DateTime.Today;
            dtpNgayMua.Checked = false;
            // Chi nhánh mặc định: chi nhánh đang lọc (ADMIN) hoặc chi nhánh của mình
            int cn = GioiHanCN ?? (cboLocCN.SelectedValue is int v && v > 0 ? v : Session.MaChiNhanh);
            cboChiNhanh.SelectedValue = cn;
            _dangNap = false;

            txtMaCode.ReadOnly = false;
            btnGoiY.Enabled = true;
            cboChiNhanh.Enabled = Session.LaAdmin;           // quản lý chỉ tạo cho chi nhánh của mình
            GoiYMa();
            CapNhatGiaTri();
        }

        private void ThemMoi()
        {
            _dangNap = true;                                 // bỏ chọn dòng mà không kích hoạt HienChiTiet
            dgv.ClearSelection();
            dgv.CurrentCell = null;
            _dangNap = false;

            XoaTrang();
            txtTen.Focus();
        }

        private void GoiYMa()
        {
            if (_ma != null || cboChiNhanh.SelectedValue is not int cn) return;
            try
            {
                _maGoiYCu = ThietBiService.GoiYMa(cn);
                txtMaCode.Text = _maGoiYCu;
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        /// <summary>ADMIN đổi chi nhánh khi đang thêm mới: mã gợi ý đi theo, trừ khi người dùng đã tự sửa mã.</summary>
        private void ChiNhanhThayDoi()
        {
            if (_dangNap || _ma != null) return;
            if (txtMaCode.Text.Trim().Length == 0 || txtMaCode.Text.Trim() == _maGoiYCu) GoiYMa();
        }

        private void CapNhatGiaTri()
        {
            lblGiaTri.Text = numDonGia.Value > 0
                ? $"{numSoLuong.Value * numDonGia.Value:N0} đ  ({numSoLuong.Value:N0} × {numDonGia.Value:N0})"
                : "— (chưa nhập đơn giá)";
        }

        // ==================================================================
        //  Lưu
        // ==================================================================
        private void Luu()
        {
            Err.Clear();

            string ma = txtMaCode.Text.Trim();
            if (_ma == null && ma.Length == 0)
            {
                Err.SetError(txtMaCode, "Nhập mã hoặc bấm 'Gợi ý'");
                txtMaCode.Focus();
                return;
            }
            if (cboChiNhanh.SelectedValue is not int cn)
            {
                Err.SetError(cboChiNhanh, "Chọn chi nhánh");
                return;
            }
            string ten = txtTen.Text.Trim();
            if (ten.Length == 0)
            {
                Err.SetError(txtTen, "Nhập tên thiết bị");
                txtTen.Focus();
                return;
            }
            if (cboLoai.SelectedValue is not string loai || loai.Length == 0)
            {
                Err.SetError(cboLoai, "Chọn loại thiết bị");
                cboLoai.Focus();
                return;
            }
            if (cboTinhTrang.SelectedValue is not string tt)
            {
                Err.SetError(cboTinhTrang, "Chọn tình trạng");
                return;
            }
            if (dtpNgayMua.Checked && dtpNgayMua.Value.Date > DateTime.Today)
            {
                Err.SetError(dtpNgayMua, "Ngày mua không được ở tương lai");
                return;
            }

            // Thanh lý là thao tác "xóa mềm" nên hỏi lại trước khi đổi
            if (_ma != null && _ttCu != "ThanhLy" && tt == "ThanhLy" &&
                !Msg.Hoi("Chuyển thiết bị này sang \"Đã thanh lý\"?\n\n" +
                         "Thiết bị sẽ không còn tính vào số lượng và giá trị tài sản. " +
                         "Dòng vẫn được giữ lại để tra cứu."))
                return;

            try
            {
                var dto = new ThietBiDto(
                    _ma, ma, cn, ten, loai, (int)numSoLuong.Value,
                    dtpNgayMua.Checked ? dtpNgayMua.Value.Date : null,
                    numDonGia.Value > 0 ? numDonGia.Value : null,
                    tt, txtGhiChu.Text);

                int id = ThietBiService.Save(dto, GioiHanCN);
                NapDanhSach(id);
                lblTongKet.ForeColor = Theme.Success;
                lblTongKet.Text = $"Đã lưu \"{ten}\" lúc {DateTime.Now:HH:mm:ss}.";
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        // ==================================================================
        //  Phím tắt
        // ==================================================================
        private void Form_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) { Luu(); e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.F2) { ThemMoi(); e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.F5) { NapDanhSach(_ma); }
        }
    }
}