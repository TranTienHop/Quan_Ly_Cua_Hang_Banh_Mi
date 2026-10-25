# Bản đồ kiến trúc hệ thống

Cập nhật sau đợt liên kết chức năng, cleanup và tích hợp chọn lọc Login ngày 23/10/2026.

## Phạm vi solution

- Ứng dụng chính: `QuanLyCuaHangBanhMi.slnx`
- Project WinForms: `QuanLyCuaHangBanhMi/QuanLyCuaHangBanhMi.csproj`
- Target framework: `net10.0-windows`
- CSDL chính: SQL Server LocalDB, database `QuanLyBanhMi`
- Data access runtime: `Microsoft.Data.SqlClient` và SQL parameterized
- Solution `../Login/Login.slnx` là project tham khảo độc lập, không phải dependency của ứng dụng chính.
- Custom controls và assets dùng bởi `FormLogin` nằm cục bộ trong project chính (`Controls/`, `Assets/`); không còn linked compile/content đến thư mục Login.

## Luồng phụ thuộc

```text
Program
  -> Forms / UserControls
      -> Services
          -> Repositories
              -> DatabaseHelper
                  -> SQL Server: QuanLyBanhMi

AuthServiceImpl
  -> AuthRepository
  -> UserSession (thông tin người dùng + permission codes)
```

Đây là hướng phụ thuộc mục tiêu. Hai ngoại lệ legacy đang được theo dõi:

- `MenuManagementControl` còn gọi `DatabaseHelper` trực tiếp.
- `Data/ReportService.cs` và `Data/ReportExportHelper.cs` đang trộn trách nhiệm business/UI trong tầng Data.

## Các module đang hoạt động

| Nhóm | Presentation | Service | Repository / Data |
|---|---|---|---|
| Đăng nhập, phân quyền | `FormLogin`, `FormMain`, `FormOrder` | `AuthServiceImpl` | `AuthRepository` |
| Tài khoản | `AccountManagementControl` | `AccountService` | `AccountRepository` |
| Vai trò | `RoleManagementControl` | `RoleService` | `RoleRepository` |
| Chi nhánh | `BranchManagementControl` | `BranchService` | `BranchRepository` |
| Ca làm | `ShiftManagementControl` | `ShiftService` | `ShiftRepository` |
| Bán hàng | `FormOrder`, `FormDonHomNay` | `OrderServiceImpl`, facade `OrderService` | `OrderRepository`, `OrderHistoryRepository` |
| Khách hàng | tab trong `FormMain` | `CustomerService` | `CustomerRepository` |
| Khuyến mãi | `FormChonKhuyenMai`, tab trong `FormMain` | `PromotionService` | `PromotionRepository` |
| Kho nguyên liệu | tab trong `FormMain` | `IngredientService` | `IngredientRepository` |
| Hóa đơn | `ReceiptPrinter` | `ReceiptService` | `ReceiptRepository` |
| Báo cáo | `ReportChartView`, tab trong `FormMain` | `Data/ReportService.cs` | SQL hiện nằm trong `ReportService` |
| Thực đơn | `MenuManagementControl` | Chưa tách | SQL hiện nằm trong control |

`AdministrationControl` là màn hình quản trị tổng hợp, chứa bốn tab Tài khoản, Vai trò, Chi nhánh và Ca làm.

## Đăng nhập và phân quyền

```text
Program
  -> FormLogin
      -> IAuthService / AuthServiceImpl.TryLogin
          -> AuthRepository.FindByCredentials
          -> AuthRepository.GetPermissionCodes
          -> UserSession
  -> FormMain
      -> bật/tắt chức năng theo UserSession.HasPermission(...)
```

`UserSession` lưu mã tài khoản, nhân viên, chi nhánh, vai trò, thông tin hiển thị và tập permission codes. Vai trò `ADMIN` được phép toàn bộ; các vai trò khác được kiểm tra bằng `HasPermission(code)`.

## Models

Thư mục `Models/` chỉ chứa DTO/session được ứng dụng sử dụng:

- `AccountCreateModel.cs`
- `BranchInfo.cs`
- `BranchSaveModels.cs`
- `CheckoutRequest.cs`
- `LoginResult.cs`
- `ManagementSaveModels.cs`
- `RoleSaveModels.cs`
- `ShiftModels.cs`
- `UserSession.cs`

EF Core scaffold cũ đã được loại bỏ vì không có consumer runtime. Project không còn package Entity Framework.

## Luồng ca làm

```text
Mở ca
  -> ShiftService.OpenShift
      -> ShiftRepository kiểm tra nhân viên/chi nhánh
      -> tạo NhatKyCaLam

Đóng ca
  -> ShiftService.CloseShift
      -> cập nhật tiền cuối ca và trạng thái

Duyệt ca
  -> ShiftService.ApproveShift
      -> yêu cầu quyền quản lý phù hợp
      -> cập nhật người duyệt và trạng thái
```

Một chi nhánh không thể bị vô hiệu hóa khi còn ca đang mở.

## Luồng thanh toán

```text
FormOrder.Checkout
  -> IOrderService / OrderServiceImpl
      -> OrderRepository.SaveCheckout
          -> transaction:
             HoaDon
             -> ChiTiet_HoaDon
             -> cập nhật tồn kho
             -> ThanhToan_HoaDon
             -> giao hàng / điểm thành viên khi có
```

`FormDonHomNay` vẫn dùng facade tĩnh `OrderService`; chỉ hợp nhất facade sau khi chuyển consumer này sang `IOrderService`.

## Cleanup đã thực hiện

- Loại bỏ EF Core scaffold và các package EF không được sử dụng.
- Loại bỏ cụm legacy không có consumer: Product, Employee, ReportRepository và static `AuthService`.
- Hợp nhất ba DTO quản lý nhỏ vào `ManagementSaveModels.cs`.
- Giữ nguyên các cặp WinForms code-behind/Designer và các `.resx` mặc định; đây không phải file trùng lặp.
- Không xóa toàn bộ `.vs`, `bin`, `obj` hoặc `*.user` vì chúng là artifact/cấu hình máy và không ảnh hưởng mã nguồn; chỉ loại bỏ hai manifest build cũ còn ghi dependency EF đã bỏ.

## Nợ kỹ thuật đã xác định

Ưu tiên tiếp theo, không thuộc cleanup an toàn của bốn module:

1. Sửa `IngredientRepository.Insert/Update`: phần đọc dùng schema tiếng Việt nhưng phần ghi còn tham chiếu bảng legacy `Ingredients`.
2. Tách SQL khỏi `MenuManagementControl` thành `MenuRepository` và `MenuService`.
3. Tách `Data/ReportService.cs` đúng tầng và bỏ fallback schema `BanhMiShopDB` sau khi kiểm thử báo cáo.
4. Tách parameter tiền và số lượng trong `DatabaseHelper` để tránh dùng chung decimal scale.
5. Xử lý các nullable warning còn lại trong `FormMain.cs`.

## Quy tắc khi mở rộng

- Form/UserControl gọi service, không thêm SQL mới vào UI.
- Service chịu trách nhiệm validate, authorization và orchestration.
- Repository chứa SQL parameterized và transaction.
- Thay đổi schema phải kiểm thử với database `QuanLyBanhMi`.
- Sau mỗi nhóm chức năng: rebuild solution, chạy smoke test CRUD và dọn dữ liệu test.
