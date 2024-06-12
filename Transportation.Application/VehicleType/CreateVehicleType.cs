using System.ComponentModel.DataAnnotations;
using PhoenixFramework.Application.Command;

namespace Transportation.Application.VehicleType;

public class CreateVehicleType : ICommand
{
    [Required]
    [MaxLength]
    [MinLength(5)]
    public string Title { get; set; }

    public string? Description { get; set; }
}