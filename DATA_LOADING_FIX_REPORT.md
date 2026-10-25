# Báo cáo kiểm tra và sửa lỗi tải dữ liệu

Ngày kiểm tra: 25/10/2026  
Solution: `QuanLyCuaHangBanhMi.slnx`  
Công nghệ dữ liệu thực tế: ADO.NET với `Microsoft.Data.SqlClient` (không có Entity Framework Core/DbContext).

## 1. Kết luận chính

- Ứng dụng đang kết nối tới SQL Server LocalDB instance `(localdb)\MSSQLLocalDB`, database `QuanLyBanhMi`.
- Kết nối thật đã thành công; database tồn tại và ở trạng thái `ONLINE`.
- Schema chính khớp với phần đọc dữ liệu của các module đang hoạt động. Các bảng nghiệp vụ đều có dữ liệu, ngoại trừ `DanhMuc_CaLam` và `LichPhanCa` đang thực sự rỗng.
- Nguyên nhân quan trọng khiến báo cáo có thể mở nhưng trống:
  1. `ReportService` nuốt exception và trả `DataTable`/danh sách rỗng hoặc KPI bằng 0, khiến lỗi truy vấn bị hiểu nhầm là “không có dữ liệu”.
  2. Tất cả bộ lọc báo cáo bị hard-code từ `01/09/2026` đến `30/09/2026`, trong khi dữ liệu hiện có tới `23/10/2026`.
  3. Danh sách chi nhánh báo cáo trước đây cho tài khoản không phải ADMIN chọn “Tất cả chi nhánh”, có nguy cơ xem ngoài phạm vi chi nhánh.
  4. `OrderServiceImpl.GetRecipe()` nuốt lỗi truy vấn và trả danh sách nguyên liệu rỗng, làm lỗi DB/schema trông giống sản phẩm không có công thức.
- Không tạo dữ liệu giả, không chạy seed, không chạy migration, không thay đổi schema và không thực hiện lệnh ghi/xóa dữ liệu trong quá trình kiểm tra.

## 2. Cấu hình kết nối và kiến trúc dữ liệu

Thứ tự chọn connection string:

1. Biến môi trường `BANHMI_DB_CONNECTION` nếu có.
2. `appsettings.json` trong thư mục chạy ứng dụng.
3. `appsettings.json` trong current working directory.
4. Chuỗi mặc định trong `DatabaseHelper` nếu không có tệp cấu hình.

Connection string hiện tại:

```text
Server=(localdb)\MSSQLLocalDB;Database=QuanLyBanhMi;Trusted_Connection=True;TrustServerCertificate=True;
```

Kết quả xác minh trực tiếp:

| Thuộc tính | Giá trị |
|---|---|
| SQL Server thực tế | `LAPTOP-TA8PSPTP\LOCALDB#653D9094` |
| Database thực tế | `QuanLyBanhMi` |
| Trạng thái database | `ONLINE` |
| Cơ chế truy cập | Static repositories/services + `DatabaseHelper` |
| EF Core / DbContext | Không sử dụng |
| DI container | Không sử dụng; `ApplicationServices` chỉ là composition root tối thiểu cho `IAuthService` |
| Lifetime kết nối | Mỗi lệnh mở/đóng `SqlConnection`; transaction dùng chung connection trong phạm vi nghiệp vụ |
| Cấu hình Debug/Release | Cùng `appsettings.json`, được copy bằng `PreserveNewest`; không có connection string riêng theo configuration |

Tệp cấu hình sai JSON hoặc không đọc được nay không còn bị bỏ qua để âm thầm kết nối sang database mặc định. Ứng dụng ghi log và dừng với thông báo cấu hình rõ ràng.

## 3. Danh mục màn hình và luồng tải dữ liệu

| Tên màn hình | Control hiển thị | Phương thức tải | Service | Entity/Bảng SQL | Trạng thái | Nguyên nhân nghi ngờ/kết luận |
|---|---|---|---|---|---|---|
| FormLogin | TextBox/role cards | `btnLogin_Click` | `IAuthService`, `AuthServiceImpl` | `TaiKhoan`, `NhanVien`, `VaiTro`, `VaiTro_Menu`, `MenuChucNang` | Đã kiểm tra | Query và session mapping đúng; role cards là lựa chọn UI cố định, không phải dữ liệu giả nghiệp vụ. |
| FormMain - Dashboard | KPI labels, `dgvDashTopProducts` | `FormMain_Load`, `LoadDashboardTab` | `ReportService` | `HoaDon`, `ChiTiet_HoaDon`, `SanPham`, `DanhMuc`, `TonKho_ChiNhanh`, `NhatKy_Kho` | Đã sửa | Exception báo cáo từng bị nuốt; ngày hiện tại không có đơn nên KPI hôm nay bằng 0 là hợp lệ. |
| Quản lý thực đơn - Danh mục | `dgvMenuCategories` | `LoadMenuData`, `LoadMenuCategoryPage` | SQL qua `DatabaseHelper` | `DanhMuc` | Có dữ liệu thật | 9 bản ghi; cột khai báo bằng code và `DataPropertyName` khớp SELECT. |
| Quản lý thực đơn - Sản phẩm | `dgvMenuProducts`, ComboBox danh mục/đơn vị/loại | `LoadMenuProductLookups`, `LoadMenuProductPage` | SQL qua `DatabaseHelper` | `SanPham`, `DanhMuc`, `DonVi`, `SanPham_ChiNhanh` | Có dữ liệu thật | 46 sản phẩm tại chi nhánh 1; filter chi nhánh cho tài khoản không phải ADMIN được giữ nguyên. |
| Quản lý thực đơn - Đơn vị | `dgvMenuUnits` | `LoadMenuUnitPage` | SQL qua `DatabaseHelper` | `DonVi`, `SanPham`, `NguyenLieu` | Có dữ liệu thật | 10 đơn vị; binding động khớp alias query. |
| Quản lý thực đơn - Bảng giá | `dgvMenuPrices`, ComboBox chi nhánh/sản phẩm | `LoadMenuBranchOptions`, `LoadMenuPriceProductLookup`, `LoadMenuPricePage` | SQL qua `DatabaseHelper` | `ChiNhanh`, `SanPham`, `SanPham_ChiNhanh`, `DonVi` | Có dữ liệu thật | 46 dòng/chi nhánh; ADMIN xem tất cả, tài khoản khác bị khóa đúng chi nhánh. |
| Khách hàng | `dgvCustomers`, `cboCustTier` | `LoadCustomersTab` | `CustomerService` | `KhachHang`, `CauHinh_HangThanhVien` | Có dữ liệu thật | 39 khách hàng; tìm kiếm rỗng trả toàn bộ. |
| Khuyến mãi quản trị | `dgvPromos`, `cboPromoBranch` | `LoadPromosTab` | `PromotionService` | `KhuyenMai`, `ChiNhanh` | Có dữ liệu thật | 5 khuyến mãi; không có filter mặc định làm mất dữ liệu. |
| Kho nguyên liệu | `dgvIngredients` | `LoadIngredientsTab` | `IngredientService` | `NguyenLieu`, `DonVi`, `TonKho_ChiNhanh`, `ChiTiet_NhapHang`, `Phieu_NhapHang` | Phần đọc hoạt động | 60 nguyên liệu tại chi nhánh 1. Editor ghi đang bị khóa vì repository ghi còn trỏ schema legacy `dbo.Ingredients`; không bật lại để tránh ghi sai. |
| Báo cáo doanh thu | `dgvRevenue`, KPI labels, ComboBox chi nhánh | `LoadReportsTab`, `LoadRevenueReport` | `ReportService` | `HoaDon`, `ChiNhanh`, `NhanVien`, `KhachHang`, `ThanhToan_HoaDon` | Đã sửa | Bỏ ngày tháng 09 hard-code; mặc định 30 ngày gần nhất; lỗi query được ném lên UI và ghi log. |
| Báo cáo bán chạy | `dgvBestSeller`, KPI labels | `LoadBestSellerReport` | `ReportService` | `ChiTiet_HoaDon`, `HoaDon`, `SanPham`, `DanhMuc`, `DonVi` | Đã sửa | Mặc định 30 ngày; query thật trả 45 sản phẩm cho chi nhánh 1. |
| Báo cáo tồn kho/tiêu hao | `dgvInventory`, KPI labels | `LoadInventoryReport` | `ReportService` | `NguyenLieu`, `DonVi`, `TonKho_ChiNhanh`, `NhatKy_Kho` | Đã sửa | Mặc định 30 ngày; query thật trả 60 nguyên liệu cho chi nhánh 1. |
| Biểu đồ báo cáo | `ReportChartView` | `LoadChartReport` | `ReportService` | Như các báo cáo doanh thu/bán chạy | Đã sửa | Các hàm biểu đồ không còn nuốt exception; control đã có thông báo “Không có dữ liệu…” khi danh sách rỗng. |
| Xem trước/xuất báo cáo | `dgvExpPreview`, `lblExpStatus` | `LoadExportPreview` | `ReportService`, `ReportExportHelper` | Theo loại báo cáo | Đã sửa | Mặc định 30 ngày; label phân biệt số dòng 0 với lỗi tải. Không tạo file trong lần kiểm tra này. |
| POS/FormOrder | Category/Product tiles, `dgvCart`, bảng khuyến mãi, khách hàng | `FormOrder_Load`, `LoadBranchInfo`, `LoadShiftInfo`, `LoadPromotions`, `LoadProducts` | `IOrderService`, `OrderServiceImpl` | `NhanVien`, `ChiNhanh`, `NhatKy_CaLam`, `KhuyenMai`, `SanPham`, `SanPham_ChiNhanh`, `DanhMuc`, `CongThuc`, `TonKho_ChiNhanh` | Có dữ liệu thật; đã sửa xử lý lỗi công thức | 46 món tại chi nhánh 1. Khi menu thật sự rỗng, UI đã hiển thị trạng thái “chi nhánh chưa có món…”. |
| Chọn khuyến mãi | `dgvPromos` fixed columns | `Shown` → `FillView` | Dữ liệu truyền từ POS | `KhuyenMai` | Binding đúng | `AutoGenerateColumns=false`; mọi `DataPropertyName` khớp `_viewTable`. |
| Đơn trong ngày | `dgvOrders`, `dgvDetails`, summary label | `FormDonHomNay_Load`, `LoadOrders`, `LoadDetails` | `OrderService` | `HoaDon`, `NhanVien`, `KhachHang`, `ThanhToan_HoaDon`, `ChiTiet_HoaDon` | Hoạt động; hôm nay rỗng hợp lệ | Có 0 đơn ngày 25/10/2026; summary hiển thị 0 đơn. Dữ liệu DB gần nhất là 23/10/2026. Fixed-column binding khớp query. |
| Tùy chỉnh món | Dynamic option controls | Constructor từ POS | `OrderServiceImpl.GetRecipe` trước khi mở form | `CongThuc`, `NguyenLieu`, dữ liệu khuyến mãi đã tải | Đã sửa xử lý lỗi | Lỗi query công thức không còn bị đổi thành danh sách rỗng giả. Form designer preview chỉ chạy ở design-time. |
| In hóa đơn | PrintPreviewDialog | `ReceiptPrinter.ShowPreview` | `ReceiptService` | `HoaDon`, `ChiTiet_HoaDon`, `ThanhToan_HoaDon`, `ChiNhanh`, `NhanVien`, `KhachHang` | Luồng đọc đúng | Báo rõ khi không tìm thấy hóa đơn; chưa kích hoạt in/preview trong kiểm thử để tránh thao tác ngoài phạm vi. |
| Quản lý tài khoản | `UserManagementView.UsersGrid`, status/search ComboBox | `AccountManagementControl.LoadData` | `AccountService` | `TaiKhoan`, `NhanVien`, `VaiTro`, `ChiNhanh` | Có dữ liệu thật | 25 tài khoản; fixed columns khớp chính xác bảng display trung gian. Chỉ quyền `SYS_ACCOUNT`. |
| Quản lý vai trò | `dgvRoles`, `CheckedListBox` | `RoleManagementControl.LoadData`, `LoadMenus`, `LoadRoles`, `LoadSelectedPermissions` | `RoleService` | `VaiTro`, `MenuChucNang`, `VaiTro_Menu`, `TaiKhoan` | Có dữ liệu thật | 3 vai trò, 84 liên kết quyền; sự kiện selection gọi tải quyền. |
| Quản lý chi nhánh | `dgvBranches`, ComboBox filter | `BranchManagementControl.LoadData` | `BranchService` | `ChiNhanh`, `NhanVien`, `NhatKy_CaLam` | Có dữ liệu thật | 3 chi nhánh; count label phân biệt kết quả filter rỗng. |
| Quản lý nhật ký ca | `dgvShifts`, ComboBox chi nhánh/trạng thái | `ShiftManagementControl.LoadData` | `ShiftService` | `NhatKy_CaLam`, `ChiNhanh`, `NhanVien`, `ThanhToan_HoaDon` | Có dữ liệu thật | 45 ca trong 30 ngày gần nhất; service cưỡng chế phạm vi chi nhánh cho non-ADMIN. |
| Lịch phân ca | `dgvSchedule`, ComboBox chi nhánh/nhân viên/ca | `EmployeeShiftScheduleControl.LoadData`, `LoadShiftTypes`, `LoadEmployees`, `LoadSchedule` | `ShiftScheduleService` | `DanhMuc_CaLam`, `LichPhanCa`, `ChiNhanh`, `NhanVien` | Bảng thật rỗng; đã sửa empty state | Cả hai bảng phân ca có 0 dòng. Không còn báo đây là lỗi tải; UI hướng dẫn tạo danh mục ca và vô hiệu thao tác phụ thuộc. |
| AdministrationDashboard/AdministrationControl | TabControl + các UserControl trên | `ApplyPermissionsAndLoad`, `NavigateToSection`, `LoadSelectedTab` | Các service quản trị | Các bảng quản trị tương ứng | Sự kiện tải đúng | UserControl nhúng chỉ gọi tải khi mở section; không thấy trường hợp quên gọi load. |
| `UserManagementView`, `ReportChartView`, `CategoryTile`, `ProductTile`, `FormGhiChuMon` | Control trình bày | Dữ liệu được truyền/bind từ control cha | Không truy vấn trực tiếp | Không áp dụng | Không phải màn hình tải SQL độc lập | Không tạo dữ liệu nghiệp vụ hard-code. |

## 4. Số lượng bản ghi thật đã xác minh

| Bảng | Số dòng |
|---|---:|
| `ChiNhanh` | 3 |
| `NhanVien` | 25 |
| `TaiKhoan` | 25 |
| `VaiTro` | 3 |
| `VaiTro_Menu` | 84 |
| `DanhMuc` | 9 |
| `SanPham` | 46 |
| `SanPham_ChiNhanh` | 138 |
| `NguyenLieu` | 60 |
| `TonKho_ChiNhanh` | 180 |
| `KhachHang` | 39 |
| `KhuyenMai` | 5 |
| `HoaDon` | 3.787 |
| `ChiTiet_HoaDon` | 7.262 |
| `NhatKy_Kho` | 48.381 |
| `NhatKy_CaLam` | 270 |
| `DanhMuc_CaLam` | 0 |
| `LichPhanCa` | 0 |

`HoaDon` có dữ liệu từ `01/09/2026 06:05:37` đến `23/10/2026 14:37:25`; ngày kiểm tra 25/10/2026 có 0 đơn. Trong 30 ngày gần nhất có 657 hóa đơn trên toàn hệ thống. Vì vậy Dashboard “hôm nay” và Đơn trong ngày có thể rỗng hợp lệ, trong khi báo cáo 30 ngày vẫn có dữ liệu.

## 5. Sửa đổi đã thực hiện

1. Đổi tất cả khoảng ngày mặc định của báo cáo từ tháng 09/2026 hard-code sang 30 ngày gần nhất.
2. Đổi nút nhanh “Cả Tháng 9” thành “30 Ngày”.
3. Bỏ hành vi nuốt exception trong `ReportService`; lỗi tải nay truyền tới lớp UI thay vì trả dữ liệu rỗng giả.
4. Bỏ hành vi nuốt exception trong `OrderServiceImpl.GetRecipe()`.
5. Cưỡng chế scope báo cáo theo `UserSession.MaChiNhanh` cho mọi tài khoản không phải ADMIN; danh sách chi nhánh của họ không còn mục “Tất cả chi nhánh”.
6. Cưỡng chế scope cho biểu đồ so sánh chi nhánh: non-ADMIN chỉ nhận dữ liệu chi nhánh hiện tại.
7. Bổ sung logging tập trung cho kết nối, query, scalar, non-query và lệnh trong transaction. Log không chứa SQL text/connection string và che các cặp `password`, `pwd`, `user id`, `uid` nếu xuất hiện trong message/stack trace.
8. Không còn bỏ qua `appsettings.json` hỏng để âm thầm dùng database mặc định; ứng dụng báo lỗi cấu hình rõ ràng.
9. Màn hình lịch phân ca xử lý bảng rỗng như empty state, không như lỗi truy vấn.

Vị trí log runtime:

```text
%LOCALAPPDATA%\QuanLyCuaHangBanhMi\Logs\application-yyyyMMdd.log
```

## 6. File đã chỉnh sửa

- `QuanLyCuaHangBanhMi/Diagnostics/ApplicationLogger.cs` (mới)
- `QuanLyCuaHangBanhMi/Data/DatabaseConfiguration.cs`
- `QuanLyCuaHangBanhMi/Data/DatabaseHelper.cs`
- `QuanLyCuaHangBanhMi/Data/ReportService.cs`
- `QuanLyCuaHangBanhMi/Services/OrderServiceImpl.cs`
- `QuanLyCuaHangBanhMi/Forms/FormMain.cs`
- `QuanLyCuaHangBanhMi/Forms/EmployeeShiftScheduleControl.cs`
- `QuanLyCuaHangBanhMi/Program.cs`
- `DATA_LOADING_FIX_REPORT.md`

Workspace đã có nhiều thay đổi chưa commit trước lần kiểm tra. Các thay đổi đó được giữ nguyên; danh sách trên chỉ nêu file có phần chỉnh sửa liên quan đến đợt sửa này.

## 7. Data binding WinForms

- `FormDonHomNay`, `FormChonKhuyenMai`, giỏ hàng POS và `UserManagementView` dùng cột Designer với `AutoGenerateColumns=false`; `DataPropertyName` đã được đối chiếu và khớp tên cột nguồn.
- `MenuManagementControl` dùng `AutoGenerateColumns=false`, nhưng tạo cột bằng code qua `AddMenuTextColumn`/`AddMenuCheckColumn`; tên property khớp alias SELECT.
- Các grid tạo động trong `FormMain` dùng auto-generated columns và bind trực tiếp `DataTable`.
- Không xóa cột Designer, không đổi tên control, không bật `AutoGenerateColumns` tùy tiện và không làm mất event handler.
- Không có luồng background cập nhật control. Mọi query hiện chạy đồng bộ trên UI thread; không có lỗi cross-thread, nhưng kết nối chậm vẫn có thể làm UI chờ. Chưa chuyển toàn bộ sang async vì đó là thay đổi kiến trúc lớn ngoài nguyên nhân lỗi đã xác minh.

## 8. Kết quả build và kiểm thử

### Build

```text
dotnet build .\QuanLyCuaHangBanhMi.slnx --no-restore -v:minimal
Build succeeded.
0 Error(s)
20 Warning(s)
```

20 cảnh báo là nullable warnings đã có trong `FormMain.cs`; không có lỗi compile mới.

### Kiểm thử SQL Server chỉ đọc

- Kết nối LocalDB: thành công.
- Database/schema/bảng: tồn tại.
- Đối chiếu toàn bộ cột của 35 bảng: thành công.
- Truy vấn đại diện với khoảng 30 ngày, chi nhánh 1:
  - Doanh thu theo ngày: 6 dòng nhóm ngày.
  - Best seller: 45 sản phẩm.
  - Tồn kho: 60 nguyên liệu.
  - Cơ cấu doanh thu theo danh mục: 9 danh mục.
- Không có lệnh INSERT/UPDATE/DELETE/TRUNCATE/DROP hoặc migration nào được chạy.

### Kiểm thử GUI

Đã yêu cầu mở ứng dụng WinForms để kiểm tra trực tiếp nhưng quyền Computer Use hết thời gian chờ, nên chưa xác minh tương tác GUI/login/navigation trong phiên này. Các phần đã xác minh chắc chắn là build, kết nối thật, schema, dữ liệu, query chỉ đọc, event wiring và binding bằng phân tích mã nguồn.

## 9. Điểm chưa sửa hoặc cần xác minh thêm

1. `IngredientRepository.Insert/Update` còn tham chiếu schema legacy `dbo.Ingredients`, không tồn tại trong `QuanLyBanhMi`. UI hiện cố ý khóa toàn bộ editor/nhập thêm nên lỗi này không ảnh hưởng tải danh sách và không thể phát sinh thao tác ghi từ màn hình. Không tự ý ánh xạ sang `NguyenLieu`/`TonKho_ChiNhanh` vì nghiệp vụ nhập kho bắt buộc phải cập nhật ledger `NhatKy_Kho`; cần thiết kế nghiệp vụ riêng trước khi mở khóa.
2. `DanhMuc_CaLam` và `LichPhanCa` rỗng thật. Không thêm dữ liệu mẫu. ADMIN có thể tạo danh mục ca từ UI để bắt đầu sử dụng.
3. `MenuManagementControl` còn truy vấn ADO.NET trực tiếp trong code-behind thay vì service/repository. Đây là nợ kiến trúc có sẵn, nhưng query/binding hiện đúng và không nằm trong Designer. Không refactor lớn trong đợt sửa lỗi tải dữ liệu.
4. Chưa chạy CRUD thật để tránh thay đổi dữ liệu. Cần test bằng transaction/database test riêng nếu muốn xác minh toàn bộ thao tác ghi.

## 10. Câu lệnh SELECT kiểm tra thủ công trong SSMS

```sql
-- Database/instance thực tế
SELECT DB_NAME() AS ActualDatabase, @@SERVERNAME AS ActualServer;

-- Số dòng theo bảng (chỉ đọc)
SELECT s.name AS SchemaName, t.name AS TableName, SUM(p.rows) AS RowCount
FROM sys.tables t
JOIN sys.schemas s ON s.schema_id = t.schema_id
LEFT JOIN sys.partitions p ON p.object_id = t.object_id AND p.index_id IN (0, 1)
GROUP BY s.name, t.name
ORDER BY t.name;

-- Phạm vi ngày hóa đơn và lý do màn hình “hôm nay” có thể rỗng
SELECT MIN(NgayTao) AS MinOrderDate,
       MAX(NgayTao) AS MaxOrderDate,
       COUNT(*) AS TotalOrders,
       SUM(CASE WHEN CAST(NgayTao AS date) = CAST(SYSDATETIME() AS date) THEN 1 ELSE 0 END) AS OrdersToday
FROM dbo.HoaDon;

-- Dữ liệu POS theo chi nhánh
DECLARE @MaChiNhanh int = 1;
SELECT sp.MaSanPham, sp.MaSKU, sp.TenSanPham, dm.TenDanhMuc, spcn.GiaBan
FROM dbo.SanPham sp
JOIN dbo.SanPham_ChiNhanh spcn
  ON spcn.MaSanPham = sp.MaSanPham
 AND spcn.MaChiNhanh = @MaChiNhanh
 AND spcn.ConBan = 1
JOIN dbo.DanhMuc dm
  ON dm.MaDanhMuc = sp.MaDanhMuc
 AND dm.DangHoatDong = 1
WHERE sp.DangHoatDong = 1
ORDER BY dm.TenDanhMuc, sp.TenSanPham;

-- Xác minh hai bảng phân ca đang rỗng thật
SELECT COUNT(*) AS ShiftTypeCount FROM dbo.DanhMuc_CaLam;
SELECT COUNT(*) AS AssignmentCount FROM dbo.LichPhanCa;

-- Phạm vi quyền theo vai trò
SELECT vt.MaCode AS RoleCode, m.MaCode AS PermissionCode
FROM dbo.VaiTro vt
LEFT JOIN dbo.VaiTro_Menu vm ON vm.MaVaiTro = vt.MaVaiTro
LEFT JOIN dbo.MenuChucNang m ON m.MaMenu = vm.MaMenu
ORDER BY vt.MaCode, m.MaCode;
```

## 11. Checklist kiểm tra thủ công trong Visual Studio

1. Chạy ứng dụng bằng Debug, đăng nhập lần lượt ADMIN, MANAGER và STAFF.
2. Với ADMIN, xác nhận các ComboBox báo cáo có “Tất cả chi nhánh”.
3. Với MANAGER/STAFF có quyền báo cáo, xác nhận chỉ thấy chi nhánh của phiên đăng nhập và không thể lấy dữ liệu chi nhánh khác.
4. Mở Doanh thu, Best seller, Tồn kho, Biểu đồ và Xuất báo cáo; mặc định phải là 30 ngày gần nhất.
5. Chọn khoảng ngày có dữ liệu (01/09/2026–23/10/2026) và xác nhận grid/KPI/biểu đồ có bản ghi.
6. Chọn ngày 25/10/2026 ở Đơn trong ngày; xác nhận summary 0 đơn, không báo lỗi.
7. Mở Lịch phân ca; xác nhận thông báo chưa có danh mục ca, nút tạo ca vẫn bật và các nút phụ thuộc dữ liệu bị khóa.
8. Tạm cấu hình sai tên database trong bản sao cấu hình test; xác nhận UI báo lỗi và file log được tạo, sau đó phục hồi cấu hình. Không làm bước này trên cấu hình dùng chung.
9. Mở từng Form/UserControl bằng WinForms Designer để xác nhận Designer load bình thường; đợt sửa không thay đổi file `.Designer.cs`.

## 12. Bổ sung sau phản hồi runtime: Khách hàng, Nguyên liệu và Khuyến mãi

Phản hồi ngày 25/10/2026 cho biết ba màn hình có thể mở nhưng không nhìn thấy dữ liệu. Kiểm tra lại bằng truy vấn chỉ đọc xác nhận database đang kết nối vẫn có:

| Bảng | Số bản ghi |
|---|---:|
| `KhachHang` | 39 |
| `NguyenLieu` | 60 |
| `KhuyenMai` | 5 |

Nguyên nhân ở tầng UI: các phương thức `LoadCustomersTab()`, `LoadPromosTab()` và `LoadIngredientsTab()` trước đây chỉ được gọi trong lần tải ban đầu của `FormMain`. `NavigateToTab()` chỉ đổi tab hiển thị, không tải lại dữ liệu của chức năng vừa mở. Vì vậy một lần tải sớm bị gián đoạn/thất bại có thể để màn hình tiếp tục trống trong suốt phiên chạy.

Thay đổi bổ sung trong `Forms/FormMain.cs`:

1. Thêm `LoadTabData(int tabIndex)` và gọi ngay sau `SwitchTab()`; mỗi chức năng được tải/làm mới đúng lúc người dùng mở.
2. Bỏ tải đồng loạt mọi module lúc `FormMain_Load` và sau khi đóng POS, tránh truy vấn trùng lặp; `NavigateToTab()` nay là một điểm tải thống nhất.
3. Thêm trạng thái số bản ghi cho ba màn hình, phân biệt rõ dữ liệu đã tải, kết quả rỗng và tải thất bại.
4. Khi tải thất bại, grid được xóa nguồn cũ, lỗi được ghi bằng `ApplicationLogger` và người dùng vẫn nhận thông báo dễ hiểu.
5. Không sửa file Designer, connection string, schema hoặc dữ liệu SQL.

Kết quả build sau thay đổi:

```text
Build succeeded.
0 Error(s)
20 Warning(s)
```

Các warning vẫn là cảnh báo nullable đã có trong `FormMain.cs`, không phải lỗi biên dịch hoặc lỗi truy vấn mới.

### 12.1. Kết quả kiểm tra khi người dùng vẫn thấy màn hình trống

Lần build/chạy gần nhất lúc 23:18:08 không xuất phát từ workspace đã sửa. Máy đang có ít nhất ba bản sao của dự án. DLL được tạo trong thư mục `D:\BTL_LTTQ\ĐỒNG BỘ DỰ ÁN\Quan_Ly_Cua_Hang_Banh_Mi-main` khác với DLL của workspace hiện tại:

| Bản chạy | Thời điểm build | Kích thước DLL | SHA-256 |
|---|---|---:|---|
| Workspace đã sửa (`D:\BTL_LTTQ\project\...`) | 23:15:36 | 592384 byte | `8BE55ACA0F0E2B2B739C0E06BCC5399989FF503F37D6207F857A8472AAC12794` |
| Bản `ĐỒNG BỘ DỰ ÁN` được chạy sau đó | 23:18:08 | 541184 byte | `9834FE3CBCB385FBD215496BD64C92C606AAE875149B6D90F314D7181D6782B0` |

Vì vậy phản hồi “vẫn không hiện” là kết quả của việc chạy một bản source/binary khác, chưa chứa thay đổi tại `FormMain.cs`. File cần mở là:

```text
D:\BTL_LTTQ\project\Quan_Ly_Cua_Hang_Banh_Mi-main\QuanLyCuaHangBanhMi.slnx
```

Hoặc chạy trực tiếp bản Debug đã build:

```text
D:\BTL_LTTQ\project\Quan_Ly_Cua_Hang_Banh_Mi-main\QuanLyCuaHangBanhMi\bin\Debug\net10.0-windows\QuanLyCuaHangBanhMi.exe
```

### 12.2. Nguyên nhân UI cuối cùng và kết quả xác minh

`InitializeComponent()` trong `FormMain.Designer.cs` đã gọi các hàm dựng nội dung tab. Tuy nhiên `FormMain_Load` trước đây gọi lại toàn bộ các hàm này lần thứ hai. Với các tab Khách hàng, Khuyến mãi và Nguyên liệu, lần gọi thứ hai tạo một `SplitContainer` và `DataGridView` khác nằm phía sau control đang hiển thị. Dữ liệu được nạp đúng vào grid mới nhưng người dùng nhìn thấy grid cũ rỗng.

Đã sửa như sau:

1. Bỏ lần khởi tạo tab trùng trong `FormMain_Load`; mỗi tab chỉ còn một cây control.
2. Giữ `LoadTabData()` tại thời điểm điều hướng để dữ liệu luôn được làm mới khi mở tab.
3. Bỏ kiểm tra `navButtons[tab].Visible` trong `NavigateToTab()`. Thuộc tính này trả về `false` khi sidebar cha đang đóng dù bản thân chức năng vẫn được cấp quyền, làm điều hướng bằng mã quay nhầm về Dashboard.
4. Chuẩn hóa thứ tự docking của thanh tìm kiếm/bộ lọc và grid để các control không che nhau.
5. Tắt dòng nhập mới mặc định của grid Nguyên liệu (`AllowUserToAddRows = false`) và chuẩn hóa chế độ chọn dòng.

Kiểm thử giao diện chỉ đọc được chạy trên đúng project hiện tại, với form thực sự được hiển thị và điều hướng lần lượt tới từng tab:

```text
dgvCustomers:   data=39, grid=39, visibleGrid=True, size=1522x896
dgvPromos:       data=5,  grid=5,  visibleGrid=True, size=1522x896
dgvIngredients: data=60, grid=60, visibleGrid=True, size=1522x896
```

Build cuối: thành công, 0 lỗi; 20 cảnh báo nullable có sẵn.
