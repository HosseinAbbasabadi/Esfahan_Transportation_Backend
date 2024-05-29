using PhoenixFramework.Application.Command;

namespace Transportation.Application.VehicleType;

public class CreateVehicleType : ICommand
{
    public string Title { get; set; }
    public string? Description { get; set; }
}