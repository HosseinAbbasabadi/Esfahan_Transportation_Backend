using System;
using System.Collections.Generic;
using UserManagement.Domain.Feature;
using UserManagement.Domain.ClassificationLevelAgg;
using UserManagement.Application.Contracts.Commands;
using UserManagement.Application.Contracts.Contracts;
using UserManagement.Application.Contracts.ViewModels;

namespace UserManagement.Application;

public class FeatureApplication : IFeatureApplication
{
    private readonly IFeatureRepository _featureRepository;
    private readonly IClassificationLevelRepository _classificationLevelRepository;

    public FeatureApplication(IFeatureRepository featureRepository,
        IClassificationLevelRepository classificationLevelRepository)
    {
        _featureRepository = featureRepository;
        _classificationLevelRepository = classificationLevelRepository;
    }

    public List<FeatureViewModel> GetAll(Guid? roleGuid)
    {
        return _featureRepository.GetList(roleGuid);
    }

    public void UpdateClassificationLevel(UpdateClassificationLevel command)
    {
        var feature = _featureRepository.Load(command.Guid);
        var classificationLevelId = _classificationLevelRepository.GetIdBy(command.ClassificationLevelGuid);
            
        feature.SetClassificationLevel(classificationLevelId);
            
        _featureRepository.SaveChanges();
    }
}