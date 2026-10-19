# Database SQL Server — Quản lý bán bánh mì

## File

| File | Nội dung |
|------|----------|
| `01_TaoBang_Khoa.sql` | Tạo DB `QuanLyBanhMi`, **35 bảng**, PK / FK / UNIQUE / CHECK |
| `02_DuLieuMau.sql` | Toàn bộ dữ liệu mẫu tháng 9/2026, chạy trong **1 transaction** (lỗi ở đâu cũng rollback hết) |
| `gen_banhang.py` | Công cụ (không phải file chạy DB): sinh lại mục 26–30 trong `02_DuLieuMau.sql` |

Thứ tự chạy: `01_TaoBang_Khoa.sql` → `02_DuLieuMau.sql`.

Nội dung `02_DuLieuMau.sql`:

| Nhóm | Mục | Nội dung |
|------|-----|----------|
| 1 | 1–8 | Vai trò, menu, phân quyền, chi nhánh, nhân viên, tài khoản, ca làm, thiết bị |
| 2 | 9–16 | Nhà cung cấp, đơn vị, danh mục, sản phẩm, giá, nguyên liệu, công thức, tồn kho 30/09 |
| 3 | 17–20 | Combo, khuyến mãi, hạng thành viên, khách hàng |
| 4 | 21–22 | Phiếu nhập và chi tiết nhập tháng 9 |
| 5 | 23–25 | Điều chuyển, hủy hao hụt, kiểm kho |
| 6 | 26–30 | Hóa đơn, chi tiết, thanh toán, giao hàng, tích điểm (do `gen_banhang.py` sinh giữa 2 dòng `-- <<< BẮT ĐẦU NHÓM 6` / `-- <<< KẾT THÚC NHÓM 6 >>>`, không sửa tay) |
| 7 | 31 | Nhật ký kho: dẫn xuất từ nhóm 4, 5, 6 bằng `INSERT ... SELECT`, kèm `TonSau` |

Chỉ chạy lại `gen_banhang.py` khi đổi giá, công thức, kế hoạch bán hay tồn kho ở mục 1–25. Script ghi đè mục 26–30, đồng thời cập nhật tiền ca (mục 7) và số lượng từng đợt nhập (mục 22). Số "hệ thống" của kiểm kho giữa tháng (mục 25) tính theo dữ liệu bán hiện tại; nếu lệch, guard ở mục 31 sẽ báo lỗi.

Khóa ngoại trong file dữ liệu mẫu được tra theo cột `MaCode` (`CN01`, `NV001`, `ADMIN`, `POS_ORDER`), không chèn tay khóa chính IDENTITY.

## Chạy script

**SSMS / Azure Data Studio:** mở file → Execute (F5).

**sqlcmd:**

```bat
sqlcmd -S "(localdb)\MSSQLLocalDB" -f 65001 -i "01_TaoBang_Khoa.sql"
sqlcmd -S "(localdb)\MSSQLLocalDB" -f 65001 -i "02_DuLieuMau.sql"
```

Sinh lại phần bán hàng (cần Python 3): `python gen_banhang.py`

Hoặc đổi `-S` thành tên instance Express của bạn.

## Số bảng

- Phân quyền RBAC0: 3  
- Chính + chi tiết nghiệp vụ: 29  
- Thiết bị chi nhánh: 1 (`ThietBi`)  
- Combo + khuyến mãi: 2 (`Combo_ChiTiet`, `KhuyenMai`)  
- **Tổng: 35**

## Lưu ý thiết kế trong script

- `HoaDon.PhiShip` — nguồn chính phí ship (không có trên `NhatKy_GiaoHang`)  
- `NhatKy_Kho.LoaiThamChieu` + `MaThamChieu` — tham chiếu mềm  
- `DiemHienTai`, `TongTienHang`, `TongThanhToan` — dẫn xuất có chủ đích  
- `Phieu_DieuChuyen`: `MaKhoGui <> MaKhoNhan` (CHECK)
- `ThietBi` — tài sản dùng lâu dài (lò, tủ, máy POS, bàn ghế), tách khỏi `NguyenLieu` vì không trừ kho khi bán. Quan hệ: ChiNhanh (1,1) — trang bị — (0,N) ThietBi
- Combo là một `SanPham` có `LoaiSanPham = 'Combo'`, `BatBuocCongThuc = 0`, giá nằm trong `SanPham_ChiNhanh`. `Combo_ChiTiet` là bảng trung gian của quan hệ nhiều-nhiều SanPham (combo) (1,N) — gồm — (0,N) SanPham (thành phần). Bán combo thì trừ kho theo `CongThuc` của từng món thành phần × `SoLuong`
- `KhuyenMai` — chương trình giảm giá theo % (có trần `GiamToiDa`) hoặc số tiền, điều kiện `DonToiThieu`, thời gian, chi nhánh (`NULL` = mọi chi nhánh). `HoaDon.MaKhuyenMai` (NULL được) cho biết hóa đơn dùng chương trình nào, số tiền giảm ghi ở `HoaDon.TienGiam`. Quan hệ: KhuyenMai (0,1) — áp dụng — (0,N) HoaDon; ChiNhanh (0,1) — có — (0,N) KhuyenMai
- `DiaChi` (ChiNhanh, NhaCungCap, NhatKy_GiaoHang) chỉ ghi cấp Quận/Huyện, Tỉnh/Thành phố
