# ARCHITECTURE.md — Kiến Trúc Kỹ Thuật

## Tổng Quan

Dự án dùng kiến trúc **3 lớp** (phù hợp WinForms đồ án):

```
┌──────────────────────────────────────────────────────────┐
│                 PRESENTATION LAYER                       │
│   Forms/  — FormLogin, FormMain, FormOrder, ...          │
│   Chỉ xử lý UI, sự kiện; gọi Services (không viết SQL)   │
└─────────────────────────┬────────────────────────────────┘
                          │  gọi
┌─────────────────────────▼────────────────────────────────┐
│                  BUSINESS LAYER                          │
│   Services/ — AuthService, OrderService, ProductService… │
│   Nghiệp vụ: hash MK, session, checkout, validate…       │
└─────────────────────────┬────────────────────────────────┘
                          │  gọi
┌─────────────────────────▼────────────────────────────────┐
│                   DATA ACCESS LAYER                      │
│   Data/DatabaseHelper.cs     — connection, Execute*      │
│   Data/Repositories/         — SQL theo module           │
│   Models/                    — DTO / UserSession         │
└─────────────────────────┬────────────────────────────────┘
                          │  SQL qua SqlConnection
┌─────────────────────────▼────────────────────────────────┐
│              SQL SERVER DATABASE                         │
│   QuanLyBanhMi / BanhMiShopDB                            │
└──────────────────────────────────────────────────────────┘
```

### Cấu trúc thư mục

```
QuanLyCuaHangBanhMi/
├── Forms/                      ← Presentation
│   ├── FormLogin.cs
│   ├── FormMain.cs
│   ├── FormOrder.cs
│   ├── FormDonHomNay.cs
│   ├── ReceiptPrinter.cs
│   └── ...
├── Services/                   ← Business
│   ├── AuthService.cs
│   ├── OrderService.cs
│   ├── ProductService.cs
│   ├── CustomerService.cs
│   ├── PromotionService.cs
│   ├── IngredientService.cs
│   ├── ReportService.cs
│   ├── EmployeeService.cs
│   └── ReceiptService.cs
├── Data/                       ← Data Access
│   ├── DatabaseHelper.cs
│   └── Repositories/
│       ├── AuthRepository.cs
│       ├── OrderRepository.cs
│       ├── ProductRepository.cs
│       ├── CustomerRepository.cs
│       ├── PromotionRepository.cs
│       ├── IngredientRepository.cs
│       ├── ReportRepository.cs
│       ├── EmployeeRepository.cs
│       ├── OrderHistoryRepository.cs
│       └── ReceiptRepository.cs
├── Models/                     ← DTO / session
│   ├── UserSession.cs
│   ├── LoginResult.cs
│   ├── BranchInfo.cs
│   ├── CheckoutRequest.cs
│   └── ProductSaveModel.cs
└── Program.cs
```

---

## Quy tắc phụ thuộc

| Lớp | Được gọi từ | Được phép gọi |
|-----|-------------|----------------|
| **Forms** | Program / Form khác | Services, Models |
| **Services** | Forms | Repositories, DatabaseHelper (hash), Models |
| **Repositories** | Services | DatabaseHelper, Models |
| **DatabaseHelper** | Repositories / AuthService (hash) | SQL Server |

Forms **không** `using QuanLyCuaHangBanhMi.Data` (trừ khi cần — hiện tại không).

---

## Luồng Đăng Nhập

```
Program.cs
  └─> FormLogin
         └─> AuthService.TryLogin(user, pass)
                ├─ DatabaseHelper.HashPasswordSHA256
                ├─ AuthRepository.FindByCredentials
                ├─ AuthService.ApplySession → UserSession
                └─ AuthRepository.UpdateLastLogin
         └─ DialogResult.OK → FormMain / FormOrder
```

---

## Luồng Thanh Toán POS

```
FormOrder.Checkout()
  └─> OrderService.Checkout(CheckoutRequest)
         └─> OrderRepository.SaveCheckout(...)
                transaction:
                  HoaDon → ChiTiet_HoaDon → trừ kho
                  → ThanhToan_HoaDon → GiaoHang → điểm TV
```

---

## FormMain — Pattern Sidebar + Hidden TabControl

FormMain dùng pattern: **TabControl với tab headers ẩn**, kết hợp Button sidebar.

Mỗi tab gọi Service tương ứng:

| Tab | Nguồn / lớp |
|-----|-------------|
| Thực đơn | `MenuManagementControl` (từ QuanLiThucDon) — loại món, SP, ĐVT, giá bán |
| Khách hàng | `CustomerService` |
| Khuyến mãi | `PromotionService` |
| Kho NL | `IngredientService` |
| Báo cáo | `Data/ReportService` + `ReportChartView` + `ReportExportHelper` (từ QuanLyCuaHangBanhMi_27_9) |
| Nhân viên | `EmployeeService` |

---

## DatabaseHelper — API thấp (Data)

```csharp
public static class DatabaseHelper
{
    public static string ConnectionString { get; set; }
    public static SqlConnection GetConnection()
    public static DataTable ExecuteQuery(...)
    public static int ExecuteNonQuery(...)
    public static object? ExecuteScalar(...)
    // Overload có SqlConnection + SqlTransaction
    public static SqlParameter Param(...)
    public static string HashPasswordSHA256(string password)
}
```

SQL nghiệp vụ nằm trong **Repositories**, không nằm trong Forms.

---

## UserSession — Global State (Models)

```csharp
public static class UserSession
{
    public static int UserID { get; set; }
    public static int EmployeeID { get; set; }
    public static string FullName { get; set; }
    public static string RoleName { get; set; }  // ADMIN | STAFF | ...
    public static bool IsAdmin => ...
}
```

Gán qua `AuthService.ApplySession` sau khi đăng nhập thành công.

---

## DPI & Layout Rules

| Rule | Lý do |
|------|-------|
| `AutoScaleMode.None` trên FormLogin | Tránh UI scale sai @ 125% DPI |
| `Dock` cho controls trong FormMain | Tự co giãn khi maximize |
| FormMain / FormOrder Maximized | Tận dụng màn hình |
