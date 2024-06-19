using System;
using System.Collections.Generic;
using UserManagement.Application.Contracts.Commands;
using UserManagement.Application.Contracts.ViewModels;

namespace UserManagement.Application.Contracts.Contracts;

public interface IFeatureApplication
{
    List<FeatureViewModel> GetAll(Guid? roleGuid);
    void UpdateClassificationLevel(UpdateClassificationLevel command);
}