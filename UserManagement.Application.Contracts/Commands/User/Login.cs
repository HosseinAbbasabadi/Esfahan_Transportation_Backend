using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Contracts.Commands.User;

public class Login
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string DbName { get; set; }
}