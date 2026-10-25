# Báo cáo tích hợp Login vào ứng dụng chính

Ngày thực hiện: 23/10/2026  
Nguồn: `D:\BTL_LTTQ\project\Login`  
Đích: `D:\BTL_LTTQ\project\Quan_Ly_Cua_Hang_Banh_Mi-main`

## 1. Kết luận

Đã tích hợp controls/assets, luồng xác thực hiện tại, giao diện danh sách tài khoản nguồn, shell quản trị phù hợp với UI nguồn và module danh mục/lịch phân ca chạy trên ADO.NET của project chính. Migration được người dùng duyệt và áp dụng ngày 23/10/2026 lên `(localdb)\MSSQLLocalDB` / `QuanLyBanhMi`; đã xác minh các cột, bảng, khóa và ràng buộc trên SQL Server. Lỗi hộp thoại yêu cầu chọn vai trò xuất hiện ngay sau đăng nhập đã được sửa bằng cách không tải dữ liệu các trang quản trị nền lúc khởi động và chỉ hiện nhắc chọn vai trò khi người dùng chủ động thao tác Sửa/Lưu. Chưa kiểm thử toàn bộ luồng login/lockout và CRUD lịch bằng dữ liệu thực, hoặc mở Visual Studio Designer. Không tạo ứng dụng thứ ba và không xóa file.

So sánh `Login/Forms/Authentication/Form1` với `Forms/FormLogin` của đích: hai form đã có cùng bố cục chính (poster bên trái, vùng đăng nhập bên phải, hai cột 43/57), các nhóm nhập liệu và chọn vai trò tương ứng. Vì vậy giữ nguyên `FormLogin` cùng Designer/resx đích, chỉ chuyển control/assets cần thiết và duy trì dịch vụ xác thực riêng của project chính.

## 2. File đã chuyển từ Login

Đã sao chép 9 file custom control vào `QuanLyCuaHangBanhMi/Controls/` và đổi namespace `Login.Controls` thành `QuanLyCuaHangBanhMi.Controls`:

- `CustomInputBox.cs`
- `DesignTimeHelper.cs`
- `DividerLabel.cs`
- `ModernButton.cs`
- `ModernCheckBox.cs`
- `ModernDataGrid.cs`
- `RoleCardButton.cs`
- `RoundedPanel.cs`
- `SidebarNavButton.cs`

Đã sao chép nguyên trạng 29 assets vào `QuanLyCuaHangBanhMi/Assets/`:

- `Assets/Icons/`: 25 file icon, bao gồm `app.ico`.
- `Assets/Images/`: `bread_icon.png`, `bread_icon_trans.png`, `poster.png`, `poster_rounded.png`.

Đợt tích hợp tiếp theo đã chuyển giao diện `UserManagementView.cs` và `UserManagementView.Designer.cs` vào `QuanLyCuaHangBanhMi/Forms/Administration/`, đổi namespace sang project chính. Không chuyển `AdminDashboardForm` nguyên khối vì code-behind phụ thuộc EF Core và database nguồn; thay vào đó tạo shell quản trị trong project chính, giữ các repository/service ADO.NET hiện có.

Đã đối chiếu hash; các asset cục bộ giống bản nguồn. Thư mục nguồn Login không bị thay đổi.

## 3. File đích đã chỉnh sửa

- `QuanLyCuaHangBanhMi/QuanLyCuaHangBanhMi.csproj`
  - Chuyển ApplicationIcon và asset glob sang đường dẫn nội bộ project.
  - Bỏ linked compile/content tới `..\..\Login\Login`.
  - Khai báo SubType, DependentUpon và resource metadata cho FormLogin để giữ nhóm Designer.
- `QuanLyCuaHangBanhMi/Forms/FormLogin.cs`
  - Dùng namespace custom control thuộc project chính.
- `QuanLyCuaHangBanhMi/Forms/FormLogin.Designer.cs`
  - Giữ nguyên partial class/InitializeComponent; cập nhật namespace các kiểu control.
- `QuanLyCuaHangBanhMi/Forms/FormMain.cs`
  - Khởi tạo UI các tab động trước khi áp quyền/tải dữ liệu. Các vùng Dashboard, thực đơn, khách hàng, khuyến mãi, kho, báo cáo và quản trị được khởi tạo khi Form load.
- `QuanLyCuaHangBanhMi/Forms/AccountManagementControl.cs`
  - Dùng `UserManagementView` nguồn làm giao diện; ánh xạ dữ liệu `TaiKhoan` sang các cột của view. Tạo/reset mật khẩu/khóa tài khoản vẫn gọi `AccountService`.
- `QuanLyCuaHangBanhMi/Forms/AdministrationDashboardControl.cs` và `.Designer.cs`
  - Thêm shell điều hướng quản trị theo giao diện Login, nhúng các màn quản trị của project chính và kiểm tra quyền trước điều hướng.
- `QuanLyCuaHangBanhMi/Forms/EmployeeShiftScheduleControl.cs` và `.Designer.cs`
  - Thêm giao diện quản lý danh mục ca và xếp/hủy lịch cho nhân viên; không thay thế `NhatKy_CaLam`.
- `QuanLyCuaHangBanhMi/Data/Repositories/ShiftScheduleRepository.cs`, `Services/ShiftScheduleService.cs`
  - Thêm truy cập ADO.NET, kiểm tra quyền/chi nhánh, kiểm tra nhân viên và ca hoạt động, ngăn phân công trùng.
- `QuanLyCuaHangBanhMi/Data/Repositories/AuthRepository.cs`, `Services/AuthServiceImpl.cs`, `Services/Abstractions/IAuthService.cs`, `Forms/FormLogin.cs`
  - Bổ sung lockout 5 lần sai/15 phút; schema lockout đã được áp dụng trên LocalDB.
- `QuanLyCuaHangBanhMi/Data/Repositories/AccountRepository.cs`
  - Đặt lại mật khẩu admin đồng thời xóa trạng thái lockout nếu các cột đó đã được thêm.
- `Database/03_Migration_TichHop_Login.sql`
  - Migration cộng thêm các cột lockout và hai bảng lịch phân ca; đã áp dụng thủ công trên LocalDB sau khi người dùng xác nhận đã sao lưu, không chạy tự động.
- `Quan_Ly_Cua_Hang_Banh_Mi-main/ARCHITECTURE.md`
  - Ghi nhận vị trí controls/assets và việc bỏ đường dẫn phụ thuộc Login.
- `SYNC_PLAN.md`
  - Ghi nhận trạng thái sau triển khai.

Không sửa `Program.cs`: project đích đã có một entry point, một dịch vụ đăng nhập và vòng đời LoginForm → MainForm → logout → LoginForm.

## 4. Cấu trúc liên quan sau tích hợp

```text
Quan_Ly_Cua_Hang_Banh_Mi-main/
├─ QuanLyCuaHangBanhMi.slnx
├─ ARCHITECTURE.md
├─ INTEGRATION_RESULT.md
├─ Database/
└─ QuanLyCuaHangBanhMi/
   ├─ Program.cs
   ├─ Forms/
   │  ├─ FormLogin.cs / .Designer.cs / .resx
   │  ├─ FormMain.cs / .Designer.cs
   │  ├─ AdministrationControl.cs / .Designer.cs
   │  ├─ AccountManagementControl.cs / .Designer.cs
   │  ├─ RoleManagementControl.cs / .Designer.cs
   │  ├─ BranchManagementControl.cs / .Designer.cs
   │  └─ ShiftManagementControl.cs / .Designer.cs
   ├─ Controls/            # custom controls cục bộ, namespace project chính
   ├─ Assets/              # assets cục bộ
   ├─ Services/
   ├─ Data/Repositories/
   ├─ Models/
   └─ Security/
```

Solution chính tiếp tục chứa duy nhất project `QuanLyCuaHangBanhMi`; `Login.slnx` không được thêm làm project reference.

## 5. Trạng thái chức năng nguồn và đích

| Chức năng | Trạng thái sau tích hợp |
|---|---|
| Đăng nhập/đăng xuất | Dùng FormLogin, AuthServiceImpl, UserSession và luồng đăng xuất hiện có của project đích |
| Mật khẩu | Project đích đã dùng BCrypt cho mật khẩu mới, xác minh SHA-256 legacy và nâng hash cũ lên BCrypt khi đăng nhập thành công |
| Đăng ký/cấp tài khoản | Project đích có luồng quản trị cấp tài khoản cho nhân viên; chưa có tự đăng ký công khai |
| Quản lý tài khoản | Dùng `UserManagementView` nguồn; thao tác tạo, reset mật khẩu, bật/tắt vẫn qua service/repository đích |
| Vai trò/phân quyền | Giữ RoleManagementControl và mô hình VaiTro/MenuChucNang của đích |
| Chi nhánh | Giữ CRUD và quy tắc nghiệp vụ trong BranchManagementControl/BranchService của đích |
| Ca làm | Giữ nhật ký mở/đóng/duyệt trong NhatKy_CaLam; thêm CRUD danh mục ca và lịch phân ca riêng, schema đã áp dụng |
| Điều hướng | FormMain nhúng dashboard quản trị; ẩn/kiểm tra điều hướng theo quyền SYS; các CRUD dùng services đích |

## 6. Phần chưa chuyển và lý do

- `Login/BLL/Services/AuthService.cs` và các entity/DbContext không được đưa vào runtime project đích: chúng phụ thuộc EF Core và database `BanhMiShopDB`; project chính dùng ADO.NET và `QuanLyBanhMi`.
- `AdminDashboardForm` code-behind nguồn chưa được chuyển nguyên khối: nó gọi trực tiếp EF và thao tác `BakeryDbContext`. Shell quản trị thay thế đã được viết trên controls chuẩn, nhúng các module/CRUD ADO.NET sẵn có; vì vậy giao diện không phải bản sao pixel-perfect của form nguồn.
- Role/branch screens vẫn là các control đích để giữ nguyên CRUD đã hoạt động; chưa port từng bố cục trang nguồn.
- Lockout code đã được bổ sung và schema đã có trên LocalDB. Cần kiểm thử ngưỡng 5 lần/15 phút bằng tài khoản thử nghiệm; không thử với tài khoản thật để tránh khóa ngoài ý muốn.
- `ForgotPasswordForm` nguồn không gửi email, xác minh danh tính, tạo token hoặc reset mật khẩu; nó chỉ kiểm tra chuỗi email rồi hiển thị thông báo. Không chuyển một UI có thể gây hiểu nhầm là khôi phục thật. Link ở FormLogin đích tiếp tục giải thích quy trình liên hệ quản trị viên.
- Lịch phân ca có repository/service/UI, CRUD danh mục ca và phân/hủy lịch; schema `DanhMuc_CaLam` và `LichPhanCa` đã áp dụng trên LocalDB. Migration không seed danh mục ca; cần tạo ca trong giao diện trước khi xếp lịch.
- Không có màn hình đăng ký tự phục vụ trong Login; nghiệp vụ cấp tài khoản cho nhân viên là do admin thực hiện.

## 7. Xung đột đã xử lý

- Namespace `Login.Controls` đổi sang `QuanLyCuaHangBanhMi.Controls`.
- Project file không còn đường dẫn tài nguyên/mã nguồn ngoài cây project.
- Giữ một ứng dụng, một Program, một luồng xác thực và một database chuẩn `QuanLyBanhMi`.
- Không nhập chéo bảng/identity giữa hai database; migration tích hợp chỉ được chạy trên LocalDB sau khi người dùng xác nhận backup, không chạy seed.
- Giữ nguyên Login gốc và các file mã nguồn/Designer/resource khác; không có file nào bị xóa.

## 8. Build và kiểm thử

### Sửa lỗi hộp thoại sau đăng nhập

Ảnh ngày 23/10/2026 cho thấy MainForm đã mở nhưng đồng thời hiện “Vui lòng chọn một vai trò.” Nguyên nhân là `FormMain_Load` gọi `ApplyPermissionsAndLoad()` trong khi các trang quản trị còn ẩn; `RoleManagementControl.LoadData()` khi chưa có vai trò được chọn đã gọi hàm lấy selection có side effect hiển thị hộp thoại. Đã sửa hàm đọc selection thành im lặng khi chưa có dòng chọn, chỉ nhắc ở thao tác Sửa/Lưu có chủ ý; việc tải các trang admin được trì hoãn tới khi mở đúng mục. Build sau sửa thành công.

Build trực tiếp vào `bin` bị chặn vì ứng dụng đang chạy và khóa file EXE. Để không dừng hoặc can thiệp tiến trình người dùng, đã rebuild vào thư mục output tạm:

```powershell
dotnet build "D:\BTL_LTTQ\project\Quan_Ly_Cua_Hang_Banh_Mi-main\QuanLyCuaHangBanhMi.slnx" --no-restore -t:Rebuild -p:OutDir="C:\Users\hoang\AppData\Local\Temp\BanhMiIntegrationBuildFinal\"
```

Kết quả build cuối: **Build succeeded, 0 errors, 20 warnings**. Cả 20 cảnh báo nullable đều nằm trong `Forms/FormMain.cs`; không có cảnh báo mới trong phần vừa tích hợp.

Đã xác minh:

- Tìm kiếm trong project đích không còn `Login.Controls`, `Login\Login`, `ProjectReference` hoặc `BakeryDbContext`.
- Assembly, `app.ico`, poster được tạo trong output riêng.
- 29 assets đích khớp hash với nguồn.
- `Login` source folder vẫn tồn tại.
- Kết nối LocalDB và truy vấn xác nhận các cột `SoLanDangNhapThatBai`, `KhoaDen`, bảng `DanhMuc_CaLam`, `LichPhanCa`, PK/FK/UNIQUE thành công; `DBCC CHECKCONSTRAINTS` hoàn tất không báo vi phạm.

Không có test project tự động cho project chính. Migration đã chạy sau khi người dùng xác nhận backup; không chạy seed/CRUD và không thử đăng nhập sai lặp lại trên tài khoản thật. Chưa thể xác minh mở View Designer trong Visual Studio tại môi trường này; metadata/cặp resource đã khai báo và build thành công nhưng cần kiểm tra thủ công.

## 9. File đề xuất xóa

Không đề xuất xóa file nào. Các bản Login gốc được giữ; không xóa Form/Designer/resx/service chỉ vì trùng tên.

## 10. Việc còn cần phê duyệt/hoàn thiện

1. Migration đã được áp dụng trên LocalDB `QuanLyBanhMi`; sao lưu và rà soát trước khi áp dụng lên môi trường khác.
2. Duyệt quy trình quên mật khẩu admin-assisted đã chọn; phần Login nguồn không có email/token hoặc reset thật.
3. Tạo danh mục ca trong giao diện trước khi xếp lịch; migration không seed dữ liệu.
4. Mở `FormLogin.cs`, `AdministrationControl.cs` và các UserControl bằng Visual Studio View Designer.
5. Nghiệm thu login, phân quyền, CRUD tài khoản/vai trò/chi nhánh/ca trên database thử nghiệm có backup.
