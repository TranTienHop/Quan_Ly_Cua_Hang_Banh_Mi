namespace QuanLyCuaHangBanhMi.Models
{
    /// <summary>Dữ liệu cần thiết để cấp tài khoản cho một nhân viên hiện có.</summary>
    public sealed class AccountCreateModel
    {
        public int EmployeeId { get; init; }
        public int RoleId { get; init; }
        public int? BranchId { get; init; }
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public bool IsActive { get; init; } = true;
    }
}
