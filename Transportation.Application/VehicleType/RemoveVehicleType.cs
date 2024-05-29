using PhoenixFramework.Application.Command;

namespace Transportation.Application.VehicleType;

public class RemoveVehicleType : ICommand
{
    public Guid Guid { get; set; }
}