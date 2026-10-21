# Phần Mềm Quản Lý & Bán Hàng Quán Bánh Mì

Ứng dụng Windows Desktop (WinForms C# / .NET 10): POS bán hàng, thực đơn, khách hàng, khuyến mãi, kho, báo cáo, nhân viên.

---

## Thông tin dự án

| Mục | Chi tiết |
|-----|----------|
| **Solution** | `QuanLyCuaHangBanhMi.slnx` |
| **Database** | `QuanLyBanhMi` (SQL Server) |
| **NuGet** | Microsoft.Data.SqlClient |
| **Kiến trúc** | 3 lớp — xem `ARCHITECTURE.md` |

Connection string nằm trong `QuanLyCuaHangBanhMi/Data/DatabaseHelper.cs` (đổi instance nếu máy bạn khác).

---

## Cách chạy

1. Tạo DB theo hướng dẫn `database/README.md` (`01_TaoBang_Khoa.sql` → `02_DuLieuMau.sql`).
2. Mở `QuanLyCuaHangBanhMi.slnx` trong Visual Studio → **F5**.

Tài khoản mẫu thường có trong script dữ liệu (ví dụ `admin` / `123456`) — mật khẩu SHA-256.

---

## Tài liệu Markdown còn giữ

| File | Mục đích |
|------|----------|
| `README.md` | Tổng quan & chạy dự án |
| `ARCHITECTURE.md` | Kiến trúc 3 lớp |
| `CODING_RULES.md` | Quy tắc đặt tên / UI / SQL |
| `database/README.md` | Script SQL & thứ tự chạy |

---

## Module chính

| Module | Mô tả |
|--------|--------|
| **Tổng quan** | Dashboard sau đăng nhập |
| **Bán hàng (POS)** | Order, khuyến mãi, thanh toán, trừ kho |
| **Thực đơn** | Loại món, sản phẩm, ĐVT, giá bán |
| **Khách hàng / KM** | Thành viên, chương trình khuyến mãi |
| **Kho / Báo cáo / NV** | Tồn NL, thống kê, danh sách nhân viên |
