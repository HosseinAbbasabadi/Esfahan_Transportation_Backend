using PhoenixFramework.Application.Query;
using System.Collections.Generic;
using System.Linq;
using UserManagement.Persistence;
using UserManagement.Query.Contracts.ClassificationLevel;

namespace UserManagement.Query
{
    public class ClassificationLevelQueryHandler : IQueryHandler<List<ClassificationLevelViewModel>>
    {
        private readonly UserManagementQueryContext _context;

        public ClassificationLevelQueryHandler(UserManagementQueryContext context)
        {
            _context = context;
        }

        public List<ClassificationLevelViewModel> Handle()
        {
            return _context.ClassificationLevel
                .Select(x => new ClassificationLevelViewModel { Guid = x.Guid, Title = x.Title })
                .ToList();
        }
    }
}