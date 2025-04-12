using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Models;

namespace VacationManager.Core.Authentication.Abstractions;

public interface IAuthenticationContext
{
    [MemberNotNullWhen(true, nameof(CurrentUser))]
    bool IsAuthenticated { get; }

    ApplicationUser? CurrentUser { get; }

    void Authenticate(ApplicationUser user);
    Task<ApplicationUser?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken);

}