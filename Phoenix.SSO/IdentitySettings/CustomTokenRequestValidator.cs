using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Validation;
using Microsoft.EntityFrameworkCore;
using UserManagement.Persistence;

namespace Phoenix.SSO.IdentitySettings;

public class CustomTokenRequestValidator : ICustomTokenRequestValidator
{
    private readonly UserManagementCommandContext _context;

    public CustomTokenRequestValidator(UserManagementCommandContext context)
    {
        _context = context;
    }

    public async Task ValidateAsync(CustomTokenRequestValidationContext context)
    {
        var client = context.Result.ValidatedRequest.Client;
        var userGuid = context.Result
            .ValidatedRequest
            .Subject
            .Claims
            .First(x => x.Type == "sub")
            .Value;

        var user = await _context.Users
            .Include(user => user.Roles)
            .Include(user => user.OrganiztionChart)
            .FirstAsync(x => x.Guid == Guid.Parse(userGuid));

        var claims = new List<Claim>
        {
            new("id", user.Guid.ToString()),
            new("role", string.Join(",", user.Roles.Select(x => x.RoleId))),
            new("dbName", "dbName"),
            new("organizationChartGuid", user.OrganiztionChart.Guid.ToString())
        };

        claims.ToList().ForEach(claim => context.Result.ValidatedRequest.ClientClaims.Add(claim));
    }
}