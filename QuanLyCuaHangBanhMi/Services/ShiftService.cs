using System;
using System.Data;
using QuanLyCuaHangBanhMi.Data.Repositories;
using QuanLyCuaHangBanhMi.Models;

namespace QuanLyCuaHangBanhMi.Services
{
    /// <summary>Nghiệp vụ quản lý nhật ký ca làm.</summary>
    public static class ShiftService
    {
        private const decimal MaxMoney = 9999999999999999.99m;

        public static DataTable GetAll(DateTime from, DateTime to, int? branchId, string? status)
        {
            EnsureCanManageShifts();
            DateTime fromDate = from.Date;
            DateTime toDate = to.Date;
            if (toDate < fromDate)
                throw new ArgumentException("Ngày kết thúc không được trước ngày bắt đầu.", nameof(to));
            if ((toDate - fromDate).TotalDays + 1 > 366)
                throw new ArgumentException("Khoảng ngày tra cứu không được vượt quá 366 ngày.", nameof(to));
            if (toDate == DateTime.MaxValue.Date)
                throw new ArgumentException("Ngày kết thúc không hợp lệ.", nameof(to));

            int? effectiveBranchId = ResolveAccessibleBranch(branchId);
            string? normalizedStatus = NormalizeStatus(status);
            return ShiftRepository.GetAll(
                fromDate, toDate.AddDays(1), effectiveBranchId, normalizedStatus);
        }

        public static DataTable GetBranches()
        {
            EnsureCanManageShifts();
            int? restrictedBranchId = UserSession.IsAdmin ? null : RequireCurrentBranch();
            return ShiftRepository.GetBranches(restrictedBranchId);
        }

        public static DataTable GetEmployees(int branchId)
        {
            EnsureCanManageShifts();
            ValidateBranchId(branchId);
            EnsureBranchAccess(branchId);
            return ShiftRepository.GetEmployees(branchId);
        }

        public static int Open(ShiftOpenModel model)
        {
            EnsureCanManageShifts();
            ArgumentNullException.ThrowIfNull(model);
            ValidateBranchId(model.BranchId);
            if (model.EmployeeId <= 0)
                throw new ArgumentException("Nhân viên không hợp lệ.", nameof(model.EmployeeId));
            ValidateMoney(model.OpeningCash, nameof(model.OpeningCash));
            string? note = NormalizeNote(model.Note);
            EnsureBranchAccess(model.BranchId);

            return ShiftRepository.Open(
                model.BranchId, model.EmployeeId, model.OpeningCash, note);
        }

        public static void Close(ShiftCloseModel model)
        {
            EnsureCanManageShifts();
            ArgumentNullException.ThrowIfNull(model);
            ValidateShiftId(model.ShiftId);
            ValidateMoney(model.CountedCash, nameof(model.CountedCash));
            string? note = NormalizeNote(model.Note);

            ShiftRepository.Close(
                model.ShiftId, model.CountedCash, note, GetRestrictedBranchId());
        }

        public static void Approve(int shiftId)
        {
            EnsureCanManageShifts();
            ValidateShiftId(shiftId);
            ShiftRepository.Approve(shiftId, GetRestrictedBranchId());
        }

        private static void EnsureCanManageShifts()
        {
            if (!UserSession.HasPermission("SYS_SHIFT"))
                throw new UnauthorizedAccessException("Bạn không có quyền quản lý ca làm.");
        }

        private static int? ResolveAccessibleBranch(int? requestedBranchId)
        {
            if (requestedBranchId.HasValue)
                ValidateBranchId(requestedBranchId.Value);
            if (UserSession.IsAdmin)
                return requestedBranchId;

            int currentBranchId = RequireCurrentBranch();
            if (requestedBranchId.HasValue && requestedBranchId.Value != currentBranchId)
                throw new UnauthorizedAccessException("Bạn chỉ được xem ca tại chi nhánh hiện tại.");
            return currentBranchId;
        }

        private static int? GetRestrictedBranchId() =>
            UserSession.IsAdmin ? null : RequireCurrentBranch();

        private static void EnsureBranchAccess(int branchId)
        {
            if (!UserSession.IsAdmin && branchId != RequireCurrentBranch())
                throw new UnauthorizedAccessException("Bạn chỉ được quản lý ca tại chi nhánh hiện tại.");
        }

        private static int RequireCurrentBranch()
        {
            if (UserSession.MaChiNhanh <= 0)
                throw new UnauthorizedAccessException("Phiên đăng nhập chưa có chi nhánh hợp lệ.");
            return UserSession.MaChiNhanh;
        }

        private static void ValidateBranchId(int branchId)
        {
            if (branchId <= 0)
                throw new ArgumentException("Chi nhánh không hợp lệ.", nameof(branchId));
        }

        private static void ValidateShiftId(int shiftId)
        {
            if (shiftId <= 0)
                throw new ArgumentException("Ca làm không hợp lệ.", nameof(shiftId));
        }

        private static void ValidateMoney(decimal value, string parameterName)
        {
            if (value < 0 || value > MaxMoney)
                throw new ArgumentOutOfRangeException(
                    parameterName, "Số tiền phải từ 0 đến 9.999.999.999.999.999,99.");
        }

        private static string? NormalizeNote(string? note)
        {
            string? normalized = note?.Trim();
            if (string.IsNullOrEmpty(normalized))
                return null;
            if (normalized.Length > 255)
                throw new ArgumentException("Ghi chú không được vượt quá 255 ký tự.", nameof(note));
            return normalized;
        }

        private static string? NormalizeStatus(string? status)
        {
            string? normalized = status?.Trim();
            if (string.IsNullOrEmpty(normalized))
                return null;

            if (normalized.Equals("DangMo", StringComparison.OrdinalIgnoreCase))
                return "DangMo";
            if (normalized.Equals("DaDong", StringComparison.OrdinalIgnoreCase))
                return "DaDong";
            if (normalized.Equals("DaDuyet", StringComparison.OrdinalIgnoreCase))
                return "DaDuyet";

            throw new ArgumentException("Trạng thái ca làm không hợp lệ.", nameof(status));
        }
    }
}
