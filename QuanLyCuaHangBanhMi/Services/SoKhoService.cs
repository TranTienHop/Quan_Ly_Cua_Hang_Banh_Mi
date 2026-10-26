using System.Data;
using QuanLyCuaHangBanhMi.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyCuaHangBanhMi.Services
{
    public static class SoKhoService
    {
        /// <summary>
        /// Ghi biến động kho. delta > 0 là nhập thêm, delta &lt; 0 là xuất bớt.
        /// PHẢI gọi trong transaction của nghiệp vụ gọi nó.
        /// </summary>
        public static void BienDong(SqlConnection c, SqlTransaction t,
            int maChiNhanh, int maNguyenLieu, decimal delta, string loaiGiaoDich,
            string? loaiThamChieu, int? maThamChieu, int? maNhanVien, string? ghiChu = null)
        {
            // 1) Khóa dòng tồn để hai người thao tác cùng lúc không bị lệch số
            var cur = Db_QLKho.Scalar(c, t,
                @"SELECT SoLuongTon FROM dbo.TonKho_ChiNhanh WITH (UPDLOCK, HOLDLOCK)
                  WHERE MaChiNhanh = @cn AND MaNguyenLieu = @nl",
                Db_QLKho.P("@cn", maChiNhanh), Db_QLKho.P("@nl", maNguyenLieu));

            decimal tonSau;
            if (cur == null)
            {
                if (delta < 0)
                    throw new InvalidOperationException("Chi nhánh chưa có tồn của nguyên liệu này.");
                tonSau = delta;
                Db_QLKho.Exec(c, t,
                    @"INSERT dbo.TonKho_ChiNhanh (MaChiNhanh, MaNguyenLieu, SoLuongTon, NgayCapNhat)
                      VALUES (@cn, @nl, @ton, SYSUTCDATETIME())",
                    Db_QLKho.P("@cn", maChiNhanh), Db_QLKho.P("@nl", maNguyenLieu), Db_QLKho.P("@ton", tonSau));
            }
            else
            {
                tonSau = Convert.ToDecimal(cur) + delta;
                if (tonSau < 0)
                    throw new InvalidOperationException(
                        $"Không đủ tồn kho (hiện còn {Convert.ToDecimal(cur):#,##0.###}, cần xuất {-delta:#,##0.###}).");
                Db_QLKho.Exec(c, t,
                    @"UPDATE dbo.TonKho_ChiNhanh
                      SET SoLuongTon = @ton, NgayCapNhat = SYSDATETIME()
                      WHERE MaChiNhanh = @cn AND MaNguyenLieu = @nl",
                    Db_QLKho.P("@ton", tonSau), Db_QLKho.P("@cn", maChiNhanh), Db_QLKho.P("@nl", maNguyenLieu));
            }

            // 2) Ghi sổ kho
            Db_QLKho.Exec(c, t,
                @"INSERT dbo.NhatKy_Kho
                    (MaChiNhanh, MaNguyenLieu, LoaiGiaoDich, SoLuongThayDoi, TonSau,
                     MaNhanVien, NgayTao, LoaiThamChieu, MaThamChieu, GhiChu)
                  VALUES (@cn, @nl, @loai, @delta, @tonSau, @nv, SYSDATETIME(), @ltc, @mtc, @gc)",
                Db_QLKho.P("@cn", maChiNhanh), Db_QLKho.P("@nl", maNguyenLieu), Db_QLKho.P("@loai", loaiGiaoDich),
                Db_QLKho.P("@delta", delta), Db_QLKho.P("@tonSau", tonSau), Db_QLKho.P("@nv", maNhanVien),
                Db_QLKho.P("@ltc", loaiThamChieu), Db_QLKho.P("@mtc", maThamChieu), Db_QLKho.P("@gc", ghiChu));
        }

        /// <summary>
        /// Sổ kho của một chi nhánh theo khoảng ngày, trả tối đa toiDa dòng MỚI NHẤT.
        /// Giờ trong NgayTao là giờ địa phương, không đổi múi giờ.
        /// </summary>
        public static DataTable LichSu(int maChiNhanh, int? maNguyenLieu, string? loaiGiaoDich,
                                       DateTime tuNgay, DateTime denNgay, int toiDa) => Db_QLKho.Query(@"
    SELECT TOP (@max)
           nk.NgayTao,
           nk.LoaiGiaoDich,
           nl.TenNguyenLieu,
           d.TenDonVi,
           nk.SoLuongThayDoi,
           nk.TonSau,
           nv.HoTen AS NguoiThucHien,
           ISNULL(CASE nk.LoaiThamChieu
                    WHEN N'PHIEU_NHAP'  THEN N'PN' + RIGHT('00000' + CAST(nk.MaThamChieu AS VARCHAR(10)), 5)
                    WHEN N'DIEU_CHUYEN' THEN N'DC' + RIGHT('00000' + CAST(nk.MaThamChieu AS VARCHAR(10)), 5)
                    WHEN N'HUY'         THEN N'HH' + RIGHT('00000' + CAST(nk.MaThamChieu AS VARCHAR(10)), 5)
                    WHEN N'KIEM_KHO'    THEN N'KK' + RIGHT('00000' + CAST(nk.MaThamChieu AS VARCHAR(10)), 5)
                    WHEN N'HOADON'      THEN N'HD' + CAST(nk.MaThamChieu AS NVARCHAR(10))
                  END, N'') AS ChungTu,
           nk.GhiChu,
           nk.LoaiThamChieu,
           nk.MaThamChieu
    FROM dbo.NhatKy_Kho nk
    JOIN dbo.NguyenLieu nl ON nl.MaNguyenLieu = nk.MaNguyenLieu
    JOIN dbo.DonVi d ON d.MaDonVi = nl.MaDonViGoc
    LEFT JOIN dbo.NhanVien nv ON nv.MaNhanVien = nk.MaNhanVien
    WHERE nk.MaChiNhanh = @cn
      AND (@nl IS NULL OR nk.MaNguyenLieu = @nl)
      AND (@loai IS NULL OR nk.LoaiGiaoDich = @loai)
      AND nk.NgayTao >= @tu AND nk.NgayTao < @den
    ORDER BY nk.NgayTao DESC, nk.MaNhatKy DESC",
            Db_QLKho.P("@max", toiDa), Db_QLKho.P("@cn", maChiNhanh), Db_QLKho.P("@nl", maNguyenLieu),
            Db_QLKho.P("@loai", loaiGiaoDich),
            Db_QLKho.P("@tu", tuNgay.Date), Db_QLKho.P("@den", denNgay.Date.AddDays(1)));
    }
}