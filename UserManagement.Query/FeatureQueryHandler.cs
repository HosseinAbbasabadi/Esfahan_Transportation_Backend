using System.Linq;
using System.Collections.Generic;
using PhoenixFramework.Application.Query;
using PhoenixFramework.Dapper;
using UserManagement.Application.Contracts.ViewModels;
using UserManagement.Application.Contracts.SearchModels;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Persistence;

namespace UserManagement.Query;

public class FeatureQueryHandler :
    IQueryHandler<List<FeatureViewModel>, FeatureSearchModel>,
    IQueryHandlerAsync<FeatureClassificationLevelViewModel, FeatureClassificationLevelSearchModel>
{
    private readonly UserManagementQueryContext _context;
    private readonly BaseDapperRepository _repository;

    public FeatureQueryHandler(UserManagementQueryContext context, BaseDapperRepository repository)
    {
        _context = context;
        _repository = repository;
    }

    public List<FeatureViewModel> Handle(FeatureSearchModel searchModel)
    {
        // return _context.Features.Where(a => a.IsPage == true)
        //     .Select(a => new FeatureViewModel
        //     {
        //         Id = a.Id,
        //         Name = a.Name,
        //         Guid = a.Guid,
        //         ClassificationLevelId = a.ClassificationLevelId.Value
        //     }).ToList();

        return _repository.Select<FeatureViewModel>($@"SELECT 
				    F.Guid,
				    F.Name,
				    ClassificationLevelGuid = CL.Guid
			    FROM tbFeatures AS F 
				    JOIN tbClassificationLevel AS CL ON 
					    F.ClassificationLevelId = CL.ID
			    WHERE F.IsPage = 1");
    }

    public async Task<FeatureClassificationLevelViewModel> Handle(FeatureClassificationLevelSearchModel searchModel)
    {
        var userClassification =
            await _context.ClassificationLevel.FirstAsync(x => x.Guid == searchModel.UserClassificationLevelGuid);

        var featureClassification = await (from feature in _context.Features
                join cl in _context.ClassificationLevel on
                    feature.ClassificationLevelId equals cl.Id
                where feature.Title == searchModel.FeatureTitle
                select new { cl.Title, cl.Level })
            .FirstOrDefaultAsync();

        if (featureClassification is null) return null;

        if (userClassification.Level >= featureClassification.Level)
            return new FeatureClassificationLevelViewModel(featureClassification.Title);

        return null;
    }
}