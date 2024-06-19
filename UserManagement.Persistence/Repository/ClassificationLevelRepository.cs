using PhoenixFramework.EntityFramework;
using UserManagement.Domain.ClassificationLevelAgg;

namespace UserManagement.Persistence.Repository;

public class ClassificationLevelRepository : BaseRepository<long, ClassificationLevel>, IClassificationLevelRepository
{
    private readonly UserManagementCommandContext _context;

    public ClassificationLevelRepository(UserManagementCommandContext context) : base(context)
    {
        _context = context;
    }
}