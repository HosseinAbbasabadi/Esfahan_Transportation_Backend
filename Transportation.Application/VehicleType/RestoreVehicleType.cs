using PhoenixFramework.Application.Command;

namespace Transportation.Application.VehicleType;

public class RestoreVehicleType : ICommand
{
    public Guid Guid { get; set; }
}