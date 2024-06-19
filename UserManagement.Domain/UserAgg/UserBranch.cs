using PhoenixFramework.Domain;

namespace UserManagement.Domain.UserAgg
{
    public record UserBranch : ValueObjectBase
    {
        public int BranchId { get; set; }
    }
}