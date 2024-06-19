using System;
using PhoenixFramework.Domain;

namespace UserManagement.Domain.ClassificationLevelAgg;

public class ClassificationLevel : AggregateRootBase<long>
{
    public int Level { get; set; }
    public string Title { get; private set; }
    public string Code { get; private set; }
    public string Color { get; private set; }

    protected ClassificationLevel()
    {
    }
}