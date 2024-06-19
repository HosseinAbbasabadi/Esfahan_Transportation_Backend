using System;

namespace UserManagement.Application.Contracts.Commands;

public class UpdateClassificationLevel
{
    public Guid Guid { get; set; }
    public Guid ClassificationLevelGuid { get; set; }
}