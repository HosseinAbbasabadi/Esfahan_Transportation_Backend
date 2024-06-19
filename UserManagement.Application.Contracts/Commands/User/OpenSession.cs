using System;

namespace UserManagement.Application.Contracts.Commands.User;

public class OpenSession
{
    public Guid Guid { get; set; }
    public bool IsSuccessful { get; set; }
    public string? ClientIpAddress { get; set; }
}