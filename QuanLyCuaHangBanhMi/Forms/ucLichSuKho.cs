using QuanLyCuaHangBanhMi.Services;
using QuanLyCuaHangBanhMi.UI_QLKho;
using System.ComponentModel;
using System.Data;

namespace QuanLyCuaHangBanhMi.Forms
{
    public partial class ucLichSuKho : UserControl, IRefreshable
    {
        private const int GioiHan = 2000;                        // số dòng tối đa hiển thị mỗi lần

        private static readonly (string Code, string Text)[] Loai =
        {
            ("NHAP", "Nhập hàng"), ("XUAT_BAN", "Bán hàng"), ("CHUYEN_DI", "Chuyển đi"),
            ("CHUYEN_DEN", "Chuyển đến"), ("HUY", "Hủy hao hụt"), ("KIEM_KE", "Kiểm kê")
        };

        private readonly ErrorProvider _err = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
        private int _maCN;
        private bool _dangNap;

        public ucLichSuKho()
        {
            InitializeComponent();
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;

            BackColor = Theme.Background;
            Font = Theme.Base;

            // ----- giao diện -----
            dgv.ApplyTheme();
            btnXem.StylePrimary();
            btnXuat.StyleSecondary();
            lblTongKet.ForeColor = Theme.Muted;

            dtpTu.Value = DateTime.Today.AddDays(-30);          // mặc định 30 ngày gần nhất
            dtpDen.Value = DateTime.Today;
            cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboNL.DropDownStyle = ComboBoxStyle.DropDownList;

            var loai = new DataTable();
            loai.Columns.Add("Code", typeof(string));
            loai.Columns.Add("Text", typeof(string));
            loai.Rows.Add("", "(Tất cả loại giao dịch)");
            foreach (var (code, text) in Loai) loai.Rows.Add(code, text);
            cboLoai.BindList(loai, "Text", "Code");

            // ----- sự kiện -----
            dgv.CellFormatting += Dgv_CellFormatting;
            dgv.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) MoChungTu(e.RowIndex); };
            dtpTu.ValueChanged += (_, _) => _err.SetError(dtpTu, "");
            btnXem.Click += (_, _) => Xem();
            btnXuat.Click += (_, _) => XuatCsv();
        }

        /// <summary>Form chủ gọi trước RefreshData để cho biết đang làm việc với chi nhánh nào.</summary>
        public void SetChiNhanh(int maChiNhanh, string tenChiNhanh) => _maCN = maChiNhanh;

        public void RefreshData()
        {
            if (_maCN == 0) return;                              // form chủ chưa gán chi nhánh

            int? giu = cboNL.SelectedValue as int?;              // giữ lựa chọn nguyên liệu đang có
            _dangNap = true;
            cboNL.BindList(
                LookupService.NguyenLieuTatCa().ThemTatCa("TenNguyenLieu", "MaNguyenLieu", -1, "(Tất cả nguyên liệu)"),
                "TenNguyenLieu", "MaNguyenLieu");
            cboNL.SelectedValue = giu ?? -1;
            _dangNap = false;

            Xem();
        }

        // ==================================================================
        //  Tra cứu
        // ==================================================================
        private void Xem()
        {
            if (_maCN == 0 || _dangNap) return;

            _err.Clear();
            if (dtpTu.Value.Date > dtpDen.Value.Date)
            {
                _err.SetError(dtpTu, "Ngày bắt đầu phải trước hoặc bằng ngày kết thúc");
                return;
            }

            int? nl = cboNL.SelectedValue is int v && v > 0 ? v : null;
            string? loai = cboLoai.SelectedValue is string s && s.Length > 0 ? s : null;

            try
            {
                // Lấy GioiHan + 1 dòng: nếu thừa một dòng nghĩa là còn dữ liệu chưa hiển thị
                DataTable dt = SoKhoService.LichSu(_maCN, nl, loai, dtpTu.Value, dtpDen.Value, GioiHan + 1);
                bool catBot = dt.Rows.Count > GioiHan;
                if (catBot) dt.Rows.RemoveAt(GioiHan);

                dgv.DataSource = dt;
                dgv.HideCols("LoaiThamChieu", "MaThamChieu");
                dgv.Heads(
                    ("NgayTao", "Thời gian"),
                    ("LoaiGiaoDich", "Loại giao dịch"),
                    ("TenNguyenLieu", "Nguyên liệu"),
                    ("TenDonVi", "ĐVT"),
                    ("SoLuongThayDoi", "SL thay đổi"),
                    ("TonSau", "Tồn sau"),
                    ("NguoiThucHien", "Người thực hiện"),
                    ("ChungTu", "Chứng từ"),
                    ("GhiChu", "Ghi chú"));
                dgv.Columns["NgayTao"]!.FillWeight = 110;
                dgv.Columns["LoaiGiaoDich"]!.FillWeight = 95;
                dgv.Columns["TenNguyenLieu"]!.FillWeight = 190;
                dgv.Columns["TenDonVi"]!.FillWeight = 55;
                dgv.Columns["SoLuongThayDoi"]!.FillWeight = 95;
                dgv.Columns["TonSau"]!.FillWeight = 90;
                dgv.Columns["NguoiThucHien"]!.FillWeight = 130;
                dgv.Columns["ChungTu"]!.FillWeight = 80;
                dgv.Columns["GhiChu"]!.FillWeight = 190;
                dgv.Columns["NgayTao"]!.AsDate("dd/MM/yyyy HH:mm");
                dgv.Columns["SoLuongThayDoi"]!.AsNumber("+#,##0.###;-#,##0.###;0");
                dgv.Columns["TonSau"]!.AsNumber("#,##0.###");

                // Cột chứng từ trông như liên kết (nhấp đúp để mở)
                var c = dgv.Columns["ChungTu"]!.DefaultCellStyle;
                c.ForeColor = Theme.Info;
                c.Font = new Font(Theme.Base, FontStyle.Underline);

                CapNhatTongKet(dt, catBot, nl);
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        private void CapNhatTongKet(DataTable dt, bool catBot, int? nl)
        {
            if (catBot)
            {
                lblTongKet.ForeColor = Theme.Warning;
                lblTongKet.Text = $"Hiển thị {GioiHan:N0} dòng mới nhất, còn nhiều dòng nữa. " +
                                  "Hãy thu hẹp khoảng ngày hoặc lọc theo loại / nguyên liệu.";
                return;
            }

            lblTongKet.ForeColor = Theme.Muted;
            string tk = $"{dt.Rows.Count:N0} dòng";

            // Chỉ cộng dồn khi lọc đúng một nguyên liệu (cộng khác đơn vị không có nghĩa)
            if (nl != null && dt.Rows.Count > 0)
            {
                decimal tang = dt.AsEnumerable().Select(r => Convert.ToDecimal(r["SoLuongThayDoi"])).Where(x => x > 0).Sum();
                decimal giam = dt.AsEnumerable().Select(r => Convert.ToDecimal(r["SoLuongThayDoi"])).Where(x => x < 0).Sum();
                string dv = Convert.ToString(dt.Rows[0]["TenDonVi"]) ?? "";
                tk += $"   •   Tăng +{tang:#,##0.###}   •   Giảm {giam:#,##0.###} {dv}";
            }
            lblTongKet.Text = tk + "   •   Nhấp đúp một dòng để xem chứng từ";
        }

        private void Dgv_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string col = dgv.Columns[e.ColumnIndex].Name;

            if (col == "LoaiGiaoDich" && e.Value is string code)
            {
                e.Value = Loai.FirstOrDefault(x => x.Code == code).Text ?? code;
                e.FormattingApplied = true;
            }
            else if (col == "SoLuongThayDoi" && e.Value is decimal d)
            {
                e.Value = d.ToString("+#,##0.###;-#,##0.###;0");
                e.CellStyle!.ForeColor = d > 0 ? Theme.Success : d < 0 ? Theme.Danger : Theme.Muted;
                e.FormattingApplied = true;
            }
        }

        // ==================================================================
        //  Xem chứng từ gốc / xuất CSV
        // ==================================================================
        private void MoChungTu(int rowIndex)
        {
            var c = dgv.Rows[rowIndex].Cells;
            string? loai = Convert.ToString(c["LoaiThamChieu"].Value);
            if (string.IsNullOrEmpty(loai) || c["MaThamChieu"].Value == DBNull.Value)
            {
                Msg.Bao("Dòng này không gắn với chứng từ nào.");
                return;
            }

            try
            {
                var ct = ChungTuService.Xem(loai, Convert.ToInt32(c["MaThamChieu"].Value));
                if (ct == null)
                {
                    Msg.Bao("Không tìm thấy chứng từ (có thể đã bị xóa).");
                    return;
                }
                using var dlg = new ChungTuDialog(ct.Value.TieuDe, ct.Value.Dong);
                dlg.ShowDialog(FindForm());
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);
            }
        }

        private void XuatCsv()
        {
            if (dgv.Rows.Count == 0)
            {
                Msg.Bao("Không có dữ liệu để xuất.");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Filter = "CSV (*.csv)|*.csv",
                FileName = $"LichSuKho_{DateTime.Now:yyyyMMdd_HHmm}.csv"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                dgv.ExportCsv(sfd.FileName);
                Msg.Bao($"Đã xuất {dgv.Rows.Count:N0} dòng.");
            }
            catch (Exception ex)
            {
                Msg.Loi(ex);                                      // thường do file đang mở trong Excel
            }
        }
    }
}