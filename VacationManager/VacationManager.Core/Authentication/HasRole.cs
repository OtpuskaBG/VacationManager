using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using VacationManager.Data.Enums;
using VacationManager.Data.Models;
using Microsoft.AspNetCore.Authorization;


namespace VacationManager.Core.Authentication;

public class HasRoleHandler : AuthorizationHandler<HasRoleRequirement>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public HasRoleHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasRoleRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return;

        if (user.Role == requirement.RequiredRole)
        {
            context.Succeed(requirement);
        }
    }
}

public class HasRoleRequirement : IAuthorizationRequirement
{
    public Role RequiredRole { get; }

    public HasRoleRequirement(Role requiredRole)
    {
        RequiredRole = requiredRole;
    }
}

public class HasAnyRoleHandler : AuthorizationHandler<HasAnyRoleRequirement>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public HasAnyRoleHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        HasAnyRoleRequirement requirement)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return;

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return;

        if (requirement.AllowedRoles.Contains(user.Role))
        {
            context.Succeed(requirement);
        }
    }
}

public class HasAnyRoleRequirement : IAuthorizationRequirement
{
    public Role[] AllowedRoles { get; }

    public HasAnyRoleRequirement(params Role[] allowedRoles)
    {
        AllowedRoles = allowedRoles;
    }
}
