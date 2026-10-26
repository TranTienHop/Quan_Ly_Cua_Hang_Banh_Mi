using System.Data;
using QuanLyCuaHangBanhMi.Data;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class ChungTuService
    {
        private static string TT(string? s) => s switch
        {
            "ChoNhap" => "Chờ nhập",
            "DaNhap" => "Đã nhập",
            "Draft" => "Nháp",
            "DaDuyet" => "Đã duyệt",
            "TuChoi" => "Từ chối",
            "ChoGui" => "Chờ gửi",
            "DangChuyen" => "Đang chuyển",
            "DaNhan" => "Đã nhận",
            "DangKiem" => "Đang kiểm",
            "HoanThanh" => "Hoàn thành",
            "DaHuy" => "Đã hủy",
            _ => s ?? ""
        };

        private static string Ngay(object v, string fmt = "dd/MM/yyyy") =>
            v == DBNull.Value ? "—" : Convert.ToDateTime(v).ToString(fmt);

        /// <summary>
        /// Nội dung chứng từ gắn với một dòng nhật ký kho.
        /// Trả về null nếu loại chứng từ lạ hoặc chứng từ không còn tồn tại.
        /// </summary>
        public static (string TieuDe, DataTable Dong)? Xem(string loaiThamChieu, int ma)
        {
            switch (loaiThamChieu)
            {
                case "PHIEU_NHAP":
                    {
                        var h = Db_QLKho.Query(@"
                        SELECT ncc.TenNCC, p.NgayDat, p.NgayNhan, p.TrangThai, p.TongTien
                        FROM dbo.Phieu_NhapHang p JOIN dbo.NhaCungCap ncc ON ncc.MaNCC = p.MaNCC
                        WHERE p.MaPhieuNhap = @p", Db_QLKho.P("@p", ma));
                        if (h.Rows.Count == 0) return null;
                        var r = h.Rows[0];
                        string tieuDe = $"Phiếu nhập PN{ma:D5}  •  {TT(Convert.ToString(r["TrangThai"]))}\n" +
                                        $"{r["TenNCC"]}  •  đặt {Ngay(r["NgayDat"])}  •  nhận {Ngay(r["NgayNhan"])}\n" +
                                        $"Tổng tiền: {Convert.ToDecimal(r["TongTien"]):N0} đ";
                        return (tieuDe, NhapKhoService.GetChiTiet(ma).DoiTenCot(
                            ("TenNguyenLieu", "Nguyên liệu"), ("TenDonVi", "ĐVT"), ("SoLuong", "Số lượng"),
                            ("DonGiaNhap", "Đơn giá (đ)"), ("ThanhTien", "Thành tiền (đ)")));
                    }

                case "HUY":
                    {
                        var h = Db_QLKho.Query(@"
                        SELECT p.NgayLap, p.LyDo, p.TrangThai, nv.HoTen
                        FROM dbo.Phieu_HuyHaoHut p JOIN dbo.NhanVien nv ON nv.MaNhanVien = p.NguoiLap
                        WHERE p.MaPhieu = @p", Db_QLKho.P("@p", ma));
                        if (h.Rows.Count == 0) return null;
                        var r = h.Rows[0];
                        string tieuDe = $"Phiếu hủy hao hụt HH{ma:D5}  •  {TT(Convert.ToString(r["TrangThai"]))}\n" +
                                        $"{Ngay(r["NgayLap"], "dd/MM/yyyy HH:mm")}  •  người lập: {r["HoTen"]}\n" +
                                        $"Lý do: {r["LyDo"]}";
                        var dong = Db_QLKho.Query(@"
                        SELECT nl.TenNguyenLieu AS [Nguyên liệu], d.TenDonVi AS [ĐVT],
                               ct.SoLuong AS [Số lượng hủy], ct.GhiChu AS [Ghi chú]
                        FROM dbo.ChiTiet_HuyHaoHut ct
                        JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
                        JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
                        WHERE ct.MaPhieu = @p ORDER BY ct.MaChiTiet", Db_QLKho.P("@p", ma));
                        return (tieuDe, dong);
                    }

                case "DIEU_CHUYEN":
                    {
                        var h = Db_QLKho.Query(@"
                        SELECT g.TenChiNhanh AS KhoGui, n.TenChiNhanh AS KhoNhan,
                               p.NgayGui, p.NgayNhan, p.TrangThai
                        FROM dbo.Phieu_DieuChuyen p
                        JOIN dbo.ChiNhanh g ON g.MaChiNhanh = p.MaKhoGui
                        JOIN dbo.ChiNhanh n ON n.MaChiNhanh = p.MaKhoNhan
                        WHERE p.MaPhieu = @p", Db_QLKho.P("@p", ma));
                        if (h.Rows.Count == 0) return null;
                        var r = h.Rows[0];
                        string tieuDe = $"Phiếu điều chuyển DC{ma:D5}  •  {TT(Convert.ToString(r["TrangThai"]))}\n" +
                                        $"{r["KhoGui"]}  →  {r["KhoNhan"]}\n" +
                                        $"Gửi {Ngay(r["NgayGui"], "dd/MM/yyyy HH:mm")}  •  nhận {Ngay(r["NgayNhan"], "dd/MM/yyyy HH:mm")}";
                        var dong = Db_QLKho.Query(@"
                        SELECT nl.TenNguyenLieu AS [Nguyên liệu], d.TenDonVi AS [ĐVT],
                               ct.SoLuongGui AS [Số lượng gửi], ct.SoLuongNhanThucTe AS [Số lượng nhận thực tế]
                        FROM dbo.ChiTiet_DieuChuyen ct
                        JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
                        JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
                        WHERE ct.MaPhieu = @p ORDER BY ct.MaChiTiet", Db_QLKho.P("@p", ma));
                        return (tieuDe, dong);
                    }

                case "KIEM_KHO":
                    {
                        var h = Db_QLKho.Query(@"
                        SELECT p.NgayKiem, p.TrangThai, p.GhiChu, nv.HoTen
                        FROM dbo.Phieu_KiemKho p JOIN dbo.NhanVien nv ON nv.MaNhanVien = p.NguoiKiem
                        WHERE p.MaPhieu = @p", Db_QLKho.P("@p", ma));
                        if (h.Rows.Count == 0) return null;
                        var r = h.Rows[0];
                        string gc = Convert.ToString(r["GhiChu"]) ?? "";
                        string tieuDe = $"Phiếu kiểm kho KK{ma:D5}  •  {TT(Convert.ToString(r["TrangThai"]))}\n" +
                                        $"{Ngay(r["NgayKiem"], "dd/MM/yyyy HH:mm")}  •  người kiểm: {r["HoTen"]}" +
                                        (gc.Length > 0 ? $"\nGhi chú: {gc}" : "");
                        var dong = Db_QLKho.Query(@"
                        SELECT nl.TenNguyenLieu AS [Nguyên liệu], d.TenDonVi AS [ĐVT],
                               ct.SoLuongHeThong AS [SL hệ thống], ct.SoLuongThucTe AS [SL thực tế],
                               ct.ChenhLech AS [Chênh lệch]
                        FROM dbo.ChiTiet_KiemKho ct
                        JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = ct.MaNguyenLieu
                        JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
                        WHERE ct.MaPhieu = @p
                        ORDER BY CASE WHEN ct.ChenhLech = 0 THEN 1 ELSE 0 END, nl.TenNguyenLieu",
                            Db_QLKho.P("@p", ma));
                        return (tieuDe, dong);
                    }

                case "HOADON":
                    {
                        var h = Db_QLKho.Query(@"
                        SELECT MaDonLocal, NgayTao, TongThanhToan, TrangThaiDon
                        FROM dbo.HoaDon WHERE MaHoaDon = @p", Db_QLKho.P("@p", ma));
                        if (h.Rows.Count == 0) return null;
                        var r = h.Rows[0];
                        string ten = Convert.ToString(r["MaDonLocal"]) is { Length: > 0 } m ? m : $"#{ma}";
                        string tieuDe = $"Hóa đơn {ten}  •  {r["TrangThaiDon"]}\n" +
                                        $"{Ngay(r["NgayTao"], "dd/MM/yyyy HH:mm")}\n" +
                                        $"Tổng thanh toán: {Convert.ToDecimal(r["TongThanhToan"]):N0} đ";
                        var dong = Db_QLKho.Query(@"
                        SELECT TenSanPhamChup AS [Món], SoLuong AS [Số lượng],
                               DonGia AS [Đơn giá (đ)], ThanhTien AS [Thành tiền (đ)]
                        FROM dbo.ChiTiet_HoaDon WHERE MaHoaDon = @p ORDER BY MaChiTiet", Db_QLKho.P("@p", ma));
                        return (tieuDe, dong);
                    }

                default:
                    return null;
            }
        }

        /// <summary>Đổi tên cột của DataTable để hiển thị tiếng Việt.</summary>
        private static DataTable DoiTenCot(this DataTable dt, params (string Cu, string Moi)[] ds)
        {
            foreach (var (cu, moi) in ds)
                if (dt.Columns.Contains(cu)) dt.Columns[cu]!.ColumnName = moi;
            return dt;
        }
    }
}