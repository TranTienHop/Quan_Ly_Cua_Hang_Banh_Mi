using System;
using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>Nghiệp vụ lịch phân công nhân viên vào danh mục ca làm.</summary>
    public static class ShiftScheduleService
    {
        public static DataTable GetBranches()
        {
            EnsureCanManage();
            return ShiftScheduleRepository.GetBranches(UserSession.IsAdmin ? null : RequireCurrentBranch());
        }

        public static DataTable GetEmployees(int branchId)
        {
            EnsureCanManage();
            ValidateBranchAccess(branchId);
            return ShiftScheduleRepository.GetEmployees(branchId);
        }

        public static DataTable GetShiftTypes()
        {
            EnsureCanManage();
            return ShiftScheduleRepository.GetShiftTypes();
        }

        public static int CreateShiftType(string code, string name, TimeSpan start, TimeSpan end)
        {
            EnsureCanManage();
            string normalizedCode = (code ?? string.Empty).Trim().ToUpperInvariant();
            string normalizedName = (name ?? string.Empty).Trim();
            if (normalizedCode.Length is < 1 or > 30)
                throw new ArgumentException("Mã ca phải có từ 1 đến 30 ký tự.", nameof(code));
            if (normalizedName.Length is < 1 or > 100)
                throw new ArgumentException("Tên ca phải có từ 1 đến 100 ký tự.", nameof(name));
            if (start == end)
                throw new ArgumentException("Giờ bắt đầu và kết thúc không được trùng nhau.");
            return ShiftScheduleRepository.CreateShiftType(normalizedCode, normalizedName, start, end);
        }

        public static void UpdateShiftType(int shiftId, string code, string name, TimeSpan start, TimeSpan end)
        {
            EnsureCanManage();
            if (shiftId <= 0)
                throw new ArgumentException("Ca làm không hợp lệ.", nameof(shiftId));
            string normalizedCode = (code ?? string.Empty).Trim().ToUpperInvariant();
            string normalizedName = (name ?? string.Empty).Trim();
            if (normalizedCode.Length is < 1 or > 30)
                throw new ArgumentException("Mã ca phải có từ 1 đến 30 ký tự.", nameof(code));
            if (normalizedName.Length is < 1 or > 100)
                throw new ArgumentException("Tên ca phải có từ 1 đến 100 ký tự.", nameof(name));
            if (start == end)
                throw new ArgumentException("Giờ bắt đầu và kết thúc không được trùng nhau.");
            ShiftScheduleRepository.UpdateShiftType(shiftId, normalizedCode, normalizedName, start, end);
        }

        public static bool ToggleShiftType(int shiftId)
        {
            EnsureCanManage();
            if (shiftId <= 0)
                throw new ArgumentException("Ca làm không hợp lệ.", nameof(shiftId));
            return ShiftScheduleRepository.ToggleShiftType(shiftId);
        }

        public static DataTable GetSchedule(DateTime fromDate, DateTime toDate, int? branchId)
        {
            EnsureCanManage();
            if (fromDate.Date > toDate.Date)
                throw new ArgumentException("Ngày bắt đầu không được sau ngày kết thúc.");
            if ((toDate.Date - fromDate.Date).TotalDays > 30)
                throw new ArgumentException("Khoảng tra cứu lịch phân ca không được vượt quá 31 ngày.");

            int? effectiveBranch = branchId;
            if (!UserSession.IsAdmin)
            {
                int currentBranch = RequireCurrentBranch();
                if (branchId.HasValue && branchId.Value != currentBranch)
                    throw new UnauthorizedAccessException("Bạn chỉ được xem lịch tại chi nhánh hiện tại.");
                effectiveBranch = currentBranch;
            }

            return ShiftScheduleRepository.GetSchedule(fromDate, toDate, effectiveBranch);
        }

        public static int Assign(int employeeId, int shiftId, int branchId, DateTime workDate, string? note)
        {
            EnsureCanManage();
            ValidateBranchAccess(branchId);
            if (employeeId <= 0 || shiftId <= 0)
                throw new ArgumentException("Nhân viên hoặc ca làm không hợp lệ.");
            string? normalizedNote = string.IsNullOrWhiteSpace(note) ? null : note.Trim();
            if (normalizedNote?.Length > 255)
                throw new ArgumentException("Ghi chú không được vượt quá 255 ký tự.", nameof(note));

            return ShiftScheduleRepository.Assign(employeeId, shiftId, branchId, workDate, normalizedNote);
        }

        public static void Cancel(int assignmentId)
        {
            EnsureCanManage();
            if (assignmentId <= 0)
                throw new ArgumentException("Lịch phân ca không hợp lệ.", nameof(assignmentId));
            ShiftScheduleRepository.Cancel(assignmentId,
                UserSession.IsAdmin ? null : RequireCurrentBranch());
        }

        private static void EnsureCanManage()
        {
            if (!UserSession.HasPermission("SYS_SHIFT"))
                throw new UnauthorizedAccessException("Bạn không có quyền quản lý lịch phân ca.");
        }

        private static void ValidateBranchAccess(int branchId)
        {
            if (branchId <= 0)
                throw new ArgumentException("Chi nhánh không hợp lệ.", nameof(branchId));
            if (!UserSession.IsAdmin && branchId != RequireCurrentBranch())
                throw new UnauthorizedAccessException("Bạn chỉ được quản lý lịch tại chi nhánh hiện tại.");
        }

        private static int RequireCurrentBranch()
        {
            if (UserSession.MaChiNhanh <= 0)
                throw new UnauthorizedAccessException("Phiên đăng nhập chưa có chi nhánh hợp lệ.");
            return UserSession.MaChiNhanh;
        }
    }
}
