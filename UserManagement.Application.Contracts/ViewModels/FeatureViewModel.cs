using System;

namespace UserManagement.Application.Contracts.ViewModels;

public class FeatureViewModel
{
    public int Id { get; set; }
    public string Parent { get; set; }
    public string Text { get; set; }
    public bool Selected { get; set; }
    public string Name { get; set; }
    public Guid Guid { get; set; }
    public int SystemId { get; set; }
    public Guid ClassificationLevelGuid { get; set; }
}