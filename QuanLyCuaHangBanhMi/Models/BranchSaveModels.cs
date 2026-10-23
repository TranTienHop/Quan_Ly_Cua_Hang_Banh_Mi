namespace QuanLyCuaHangBanhMi.Models
{
    public sealed class BranchCreateModel
    {
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public byte Type { get; init; }
        public string? Address { get; init; }
        public string? Phone { get; init; }
        public bool IsActive { get; init; } = true;
    }

    public sealed class BranchUpdateModel
    {
        public int BranchId { get; init; }
        public string Name { get; init; } = string.Empty;
        public byte Type { get; init; }
        public string? Address { get; init; }
        public string? Phone { get; init; }
    }
}
