using System;
using System.Data;

namespace QuanLyCuaHangBanhMi.Domain
{
    /// <summary>Factory tạo đúng subclass theo LoaiSanPham từ menu POS (DataRow).</summary>
    public static class SanPhamBanHangFactory
    {
        public static SanPhamBanHang FromPosMenuRow(DataRow row)
        {
            ArgumentNullException.ThrowIfNull(row);

            int ma = row.Field<int>("MaSanPham");
            string ten = row.Field<string>("TenSanPham") ?? "";
            string loai = row.Field<string>("LoaiSanPham") ?? "ThanhPham";
            string danhMuc = row.Table.Columns.Contains("TenDanhMuc")
                ? row.Field<string>("TenDanhMuc") ?? ""
                : "";
            decimal gia = row.Field<decimal>("GiaBan");
            decimal? coTheBan = row.Table.Columns.Contains("CoTheBan") && !row.IsNull("CoTheBan")
                ? Convert.ToDecimal(row["CoTheBan"])
                : null;

            return Create(ma, ten, loai, danhMuc, gia, coTheBan);
        }

        public static SanPhamBanHang Create(
            int maSanPham,
            string tenSanPham,
            string loaiSanPham,
            string tenDanhMuc,
            decimal giaBan,
            decimal? coTheBan)
        {
            return loaiSanPham switch
            {
                "Combo" => new ComboItem(maSanPham, tenSanPham, tenDanhMuc, giaBan, coTheBan),
                "KhongTruKho" => new KhongTruKhoItem(maSanPham, tenSanPham, tenDanhMuc, giaBan, coTheBan),
                _ => new ThanhPhamItem(maSanPham, tenSanPham, tenDanhMuc, giaBan, coTheBan)
            };
        }
    }
}
