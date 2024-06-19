using System;

namespace UserManagement.Application.Contracts.SearchModels;

public class FeatureClassificationLevelSearchModel
{
    public string FeatureTitle { get; set; }
    public Guid UserClassificationLevelGuid { get; set; }
}