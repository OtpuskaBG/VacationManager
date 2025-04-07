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

public class UserService(IIdentityRepository<ApplicationUser> userRepository, IAuthenticationContext authContext) : IUserService
{
    private readonly IIdentityRepository<ApplicationUser> _userRepository = userRepository;
    private readonly IAuthenticationContext _authContext = authContext;

    public async Task<ApplicationUser[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAsync(userId, cancellationToken);
    }

    public async Task ChangeRoleAsync(ApplicationUser user, Role newRole, CancellationToken cancellationToken = default)
    {
        var currentUser = _authContext.CurrentUser;

        if (currentUser is null || currentUser.Role != Role.CEO)
            throw new UnauthorizedAccessException("Only CEO can change roles.");

        var userId = await GetByIdAsync(user.Id, cancellationToken);
        if (userId is null)
            throw new Exception("User not found");

        userId.Role = newRole;
        await _userRepository.UpdateAsync(userId, cancellationToken);
    }
}