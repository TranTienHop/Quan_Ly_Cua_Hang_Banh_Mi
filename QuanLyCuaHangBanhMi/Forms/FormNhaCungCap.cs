using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using System.Runtime.Intrinsics.Arm;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class FormNhaCungCap : FrmBase
    {
        private int? _ma;                        // null = đang thêm mới
        private bool _daLuuHoatDong = true;      // trạng thái hợp tác đang lưu trong DB của dòng đang chọn
        private bool _dangNap;                   // chặn SelectionChanged chạy khi đang nạp lưới / xóa chọn

        public FormNhaCungCap()
        {
            InitializeComponent();

            // ----- giao diện -----
            dgv.ApplyTheme();
            btnLuu.StylePrimary();
            btnMoi.StyleSecondary();
            TrangThaiUi.Hook(dgv, "TrangThai");      // nối một lần ở constructor
            lblTongKet.ForeColor = Theme.Muted;
            KeyPreview = true;

            txtMa.ReadOnly = true;
            txtTen.MaxLength = 200;
            txtSDT.MaxLength = 20;
            txtDiaChi.MaxLength = 255;

            cboLoc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoc.Items.AddRange(new object[] { "Đang hợp tác", "Ngừng hợp tác", "Tất cả" });
            cboLoc.SelectedIndex = 0;

            grpChiTiet.Enabled = Session.LaQuanLy;   // chỉ quản lý được sửa (menu đã chặn STAFF)

            // ----- sự kiện -----
            txtTim.TextChanged += (_, _) => NapDanhSach(null);
            cboLoc.SelectedIndexChanged += (_, _) => NapDanhSach(null);
            dgv.SelectionChanged += (_, _) => HienChiTiet();
            btnMoi.Click += (_, _) => ThemMoi();
            btnLuu.Click += (_, _) => Luu();
            txtTen.TextChanged += (_, _) => Err.SetError(txtTen, "");
            txtSDT.TextChanged += (_, _) => Err.SetError(txtSDT, "");
            KeyDown += Form_KeyDown;
        }

        public override void RefreshData() => NapDanhSach(null);

        // ================= Danh sách =================
        private void NapDanhSach(int? chonMa)
        {
            bool? hd = cboLoc.SelectedIndex switch { 0 => true, 1 => false, _ => null };

            try
            {
                _dangNap = true;
                var dt = NccService.GetAll(txtTim.Text.Trim(), hd);
                dgv.DataSource = dt;

                dgv.HideCols("MaNCC", "DangHoatDong");
                dgv.Heads(
                    ("TenNCC", "Nhà cung cấp"),
                    ("SoDienThoai", "Điện thoại"),
                    ("DiaChi", "Địa chỉ"),
                    ("SoPhieu", "Số phiếu nhập"),
                    ("ConNo", "Còn nợ (đ)"),
                    ("TrangThai", "Trạng thái"));
                dgv.Columns["TenNCC"]!.FillWeight = 200;
                dgv.Columns["SoDienThoai"]!.FillWeight = 100;
                dgv.Columns["DiaChi"]!.FillWeight = 170;
                dgv.Columns["SoPhieu"]!.FillWeight = 70;
                dgv.Columns["ConNo"]!.FillWeight = 90;
                dgv.Columns["TrangThai"]!.FillWeight = 110;
                dgv.Columns["SoPhieu"]!.AsNumber("N0");
                dgv.Columns["ConNo"]!.AsNumber("N0");

                lblTongKet.ForeColor = Theme.Muted;
                lblTongKet.Text = $"{dt.Rows.Count} nhà cung cấp";
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

            if (chonMa != null)                       // chọn lại dòng vừa lưu
            {
                foreach (DataGridViewRow r in dgv.Rows)
                {
                    if (Convert.ToInt32(r.Cells["MaNCC"].Value) == chonMa)
                    {
                        dgv.CurrentCell = r.Cells["TenNCC"];
                        break;
                    }
                }
            }
            HienChiTiet();
        }

        // ================= Panel bên phải =================
        private void HienChiTiet()
        {
            if (_dangNap) return;
            if (dgv.CurrentRow == null) { XoaTrang(); return; }

            var c = dgv.CurrentRow.Cells;
            _ma = Convert.ToInt32(c["MaNCC"].Value);
            _daLuuHoatDong = Convert.ToBoolean(c["DangHoatDong"].Value);

            Err.Clear();
            txtMa.Text = "NCC" + _ma.Value.ToString("D3");        // mã hiển thị, DB không có cột mã riêng
            txtTen.Text = c["TenNCC"].Value?.ToString() ?? "";
            txtSDT.Text = c["SoDienThoai"].Value?.ToString() ?? "";
            txtDiaChi.Text = c["DiaChi"].Value?.ToString() ?? "";
            chkHoatDong.Checked = _daLuuHoatDong;
            lblThongKe.Text =
                $"{Convert.ToInt32(c["SoPhieu"].Value):N0} phiếu nhập\nCòn nợ: {Convert.ToDecimal(c["ConNo"].Value):N0} đ";
        }

        private void XoaTrang()
        {
            _ma = null;
            _daLuuHoatDong = true;
            Err.Clear();
            txtMa.Text = "(Tự động)";
            txtTen.Clear();
            txtSDT.Clear();
            txtDiaChi.Clear();
            chkHoatDong.Checked = true;
            lblThongKe.Text = "—";
        }

        private void ThemMoi()
        {
            _dangNap = true;                 // bỏ chọn dòng mà không kích hoạt HienChiTiet
            dgv.ClearSelection();
            dgv.CurrentCell = null;
            _dangNap = false;

            XoaTrang();
            txtTen.Focus();
        }

        // ================= Lưu =================
        private void Luu()
        {
            Err.Clear();

            string ten = txtTen.Text.Trim();
            if (ten.Length == 0)
            {
                Err.SetError(txtTen, "Nhập tên nhà cung cấp");
                txtTen.Focus();
                return;
            }

            NccService.ChuanHoaSdt(txtSDT.Text, out string? loiSdt);
            if (loiSdt != null)
            {
                Err.SetError(txtSDT, loiSdt);
                txtSDT.Focus();
                return;
            }

            // Đang chuyển từ "hợp tác" sang "ngừng": hỏi lại vì đây là thao tác ảnh hưởng đến việc lập phiếu
            if (_ma != null && _daLuuHoatDong && !chkHoatDong.Checked &&
                !Msg.Hoi("Ngừng hợp tác với nhà cung cấp này?\n\n" +
                         "Các phiếu nhập cũ vẫn được giữ nguyên, nhưng nhà cung cấp sẽ không còn " +
                         "xuất hiện khi lập phiếu nhập mới."))
                return;

            try
            {
                int ma = NccService.Save(_ma, ten, txtSDT.Text, txtDiaChi.Text, chkHoatDong.Checked);
                NapDanhSach(ma);
                lblTongKet.ForeColor = Theme.Success;
                lblTongKet.Text = $"Đã lưu \"{ten}\" lúc {DateTime.Now:HH:mm:ss}.";
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        // ================= Phím tắt =================
        private void Form_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) { Luu(); e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.F2) { ThemMoi(); e.SuppressKeyPress = true; }
            else if (e.KeyCode == Keys.F5) { NapDanhSach(_ma); }
        }
    }
}