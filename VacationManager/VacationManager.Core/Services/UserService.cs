using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Core.Authentication.Abstractions;
using VacationManager.Core.Services.Abstractions;
using VacationManager.Data.Enums;
using VacationManager.Data.Models;
using VacationManager.Data.Repositories;
using VacationManager.Data.Repositories.Abstractions;


namespace VacationManager.Core.Services;

public class UserService(IIdentityRepository<ApplicationUser> userRepository, IAuthenticationContext authContext, UserManager<ApplicationUser> userManager) : IUserService
{
    private readonly IIdentityRepository<ApplicationUser> _userRepository = userRepository;
    private readonly IAuthenticationContext _authContext = authContext;
    private readonly UserManager<ApplicationUser> userManager = userManager;

    public async Task<ApplicationUser[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAsync(userId, cancellationToken);
    }

    public async Task ChangeRoleAsync(ApplicationUser user, Role newRole, CancellationToken cancellationToken)
    {
        var existingUser = await userManager.FindByIdAsync(user.Id.ToString());

        if (existingUser == null) return;

        existingUser.Role = newRole;
        await userManager.UpdateAsync(existingUser);
    }
}