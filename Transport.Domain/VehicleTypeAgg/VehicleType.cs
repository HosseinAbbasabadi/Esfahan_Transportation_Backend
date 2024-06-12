using PhoenixFramework.Core.Exceptions;
using PhoenixFramework.Domain;

namespace Transport.Domain.VehicleTypeAgg;

public class VehicleType : AuditableAggregateRootBase<long>
{
    public string Title { get; private set; }
    public string? Description { get; private set; }

    public VehicleType(Guid creator, string title, string? description) : base(creator)
    {
        Title = title;
        Description = description;
    }
    
    public void Edit(Guid editor, string title, string? description)
    {
        Title = title;
        Description = description;

        if (Title == title)
            throw new BusinessException("01", "title is duplicated");
        
        Modified(editor);
        
        // Remove(editor);
        // Restore(editor);
    }
}