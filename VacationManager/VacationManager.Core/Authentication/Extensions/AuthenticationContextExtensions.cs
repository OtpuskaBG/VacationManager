using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Core.Authentication.Abstractions;

namespace VacationManager.Core.Authentication.Extensions;

public static class AuthenticationContextExtensions
{
    public static IdentityUser GetCurrentUserRequired(this IAuthenticationContext authContext)
    {
        if (!authContext.IsAuthenticated) throw new InvalidOperationException("This action requires an authenticated user.");

        return authContext.CurrentUser;
    }
}
