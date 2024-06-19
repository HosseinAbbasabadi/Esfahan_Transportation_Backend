using PhoenixFramework.Domain;

namespace UserManagement.Domain.SystemAgg;

public class System : AggregateRootBase<int>
{
    public string Title { get; private set; }
    public string? Image { get; private set; }
    public string Url { get; private set; }
    public string? Description { get; private set; }
}