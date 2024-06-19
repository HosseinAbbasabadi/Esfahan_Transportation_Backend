using System;
using System.Collections.Generic;
using System.Linq;
using PhoenixFramework.Dapper;
using PhoenixFramework.EntityFramework;
using UserManagement.Application.Contracts.ViewModels;
using UserManagement.Domain.Feature;

namespace UserManagement.Persistence.Repository
{
    public class FeatureRepository : BaseRepository<int, Feature>, IFeatureRepository
    {
        private readonly UserManagementCommandContext _context;
        private readonly BaseDapperRepository _repository;

        public FeatureRepository(BaseDapperRepository repository, UserManagementCommandContext commandContext) : base(commandContext)
        {
            _repository = repository;
            _context = commandContext;
        }

        public List<FeatureViewModel> GetList(Guid? roleGuid)
        {
            roleGuid ??= Guid.Empty;

            return _repository.Select<FeatureViewModel>($@"
                 Select
                     Id = Cast(Id As Nvarchar(10)),
                     Parent = IsNull(Cast(ParentId As Nvarchar(10)), '#'),
                     [Text] = Name,
                     Selected = Case When P.FeatureId Is not Null Then 1 Else 0 End
                 From tbFeatures A
	                Left Join 
	                (Select FeatureId From tbRolePermissions AS UGP
		                JOIN tbRoles AS UG ON UGP.RoleId = UG.Id
	                Where UG.Guid = '{roleGuid}') P On P.FeatureId = A.Id");
        }

        public int GetIdBy(string Title)
        {
            return _context.Features
                .Where(x => x.Title == Title)
                .Select(x => x.Id)
                .FirstOrDefault();
        }
    }
}