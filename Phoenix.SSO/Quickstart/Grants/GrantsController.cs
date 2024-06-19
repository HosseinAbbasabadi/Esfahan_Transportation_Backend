using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoenixFramework.Identity;
using UserManagement.Persistence;

namespace Phoenix.SSO.Quickstart.Grants;

[SecurityHeaders]
[Authorize]
public class GrantsController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IClientStore _clients;
    private readonly IResourceStore _resources;
    private readonly IEventService _events;
    private readonly UserManagementCommandContext _context;

    public GrantsController(IIdentityServerInteractionService interaction,
        IClientStore clients,
        IResourceStore resources,
        IEventService events, UserManagementCommandContext context)
    {
        _interaction = interaction;
        _clients = clients;
        _resources = resources;
        _events = events;
        _context = context;
    }

    /// <summary>
    /// Show list of grants
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View("Index", await BuildViewModelAsync());
    }

    /// <summary>
    /// Handle postback to revoke a client
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Revoke(string clientId)
    {
        await _interaction.RevokeUserConsentAsync(clientId);
        await _events.RaiseAsync(new GrantsRevokedEvent(User.GetSubjectId(), clientId));

        return RedirectToAction("Index");
    }

    private async Task<GrantsViewModel> BuildViewModelAsync()
    {
        var userGuid = Guid.Parse(HttpContext.User.Claims.First(x => x.Type == "sub").Value);
        var systems = await _context.Users
            .Where(x => x.Guid == userGuid)
            .Include(x => x.Systems)
            .ThenInclude(x => x.System)
            .SelectMany(x => x.Systems)
            .Select(x => x.System)
            .Where(x => x.IsActive == 1)
            .ToListAsync();

        // var grants = await _interaction.GetAllUserGrantsAsync();

        var list = systems.Select(system => new GrantViewModel
            {
                ClientId = system.Guid.ToString(),
                ClientName = system.Title,
                ClientLogoUrl = system.Image,
                ClientUrl = system.Url,
                Description = system.Description,
            })
            .ToList();

        return new GrantsViewModel
        {
            Grants = list
        };
    }
}