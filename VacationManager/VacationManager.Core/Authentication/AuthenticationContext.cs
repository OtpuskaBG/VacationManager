using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Core.Authentication.Abstractions;

namespace VacationManager.Core.Authentication;

public class AuthenticationContext : IAuthenticationContext
{
    public bool IsAuthenticated => this.CurrentUser != null;
    public IdentityUser? CurrentUser { get; private set; }

    public void Authenticate(IdentityUser user)
    {
        ArgumentNullException.ThrowIfNull(user);
        this.CurrentUser = user;
    }
}
