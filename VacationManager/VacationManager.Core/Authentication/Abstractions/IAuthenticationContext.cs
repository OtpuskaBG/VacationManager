using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManager.Core.Authentication.Abstractions;

public interface IAuthenticationContext
{
    [MemberNotNullWhen(true, nameof(CurrentUser))]
    bool IsAuthenticated { get; }

    IdentityUser? CurrentUser { get; }

    void Authenticate(IdentityUser user);
}