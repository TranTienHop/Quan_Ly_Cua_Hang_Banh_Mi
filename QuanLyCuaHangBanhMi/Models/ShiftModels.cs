namespace QuanLyCuaHangBanhMi.Models
{
    public sealed class ShiftOpenModel
    {
        public int BranchId { get; init; }
        public int EmployeeId { get; init; }
        public decimal OpeningCash { get; init; }
        public string? Note { get; init; }
    }

    public sealed class ShiftCloseModel
    {
        public int ShiftId { get; init; }
        public decimal CountedCash { get; init; }
        public string? Note { get; init; }
    }
}
