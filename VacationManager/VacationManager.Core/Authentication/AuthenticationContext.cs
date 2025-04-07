using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Core.Authentication.Abstractions;
using VacationManager.Data.Models;

namespace VacationManager.Core.Authentication;

public class AuthenticationContext : IAuthenticationContext
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationContext(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public bool IsAuthenticated => this.CurrentUser != null;
    public ApplicationUser? CurrentUser { get; private set; }

    public void Authenticate(ApplicationUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        this.CurrentUser = user;
    }
    public async Task<ApplicationUser?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString(), cancellationToken);
    }


}
