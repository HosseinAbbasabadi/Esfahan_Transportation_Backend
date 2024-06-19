using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using PhoenixFramework.Application.Command;

namespace UserManagement.Application.Contracts.Company;

public class CreateCompany : ICommand
{
    [Required] [MaxLength(500)] 
    public string Title { get; set; }
    public Guid? ParentGuid { get; set; }
    public string? Address { get; set; }
    public IFormFile? Logo { get; set; }
    public string? Description { get; set; }
}