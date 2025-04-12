using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Core.Prototypes;
using VacationManager.Data.Enums;
using VacationManager.Data.Models;

namespace VacationManager.Core.Services.Abstractions;

public interface IUserService
{
    Task<ApplicationUser[]> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ApplicationUser?> GetByIdAsync(string userId, CancellationToken cancellationToken = default);
    Task ChangeRoleAsync(ApplicationUser user, Role newRole, CancellationToken cancellationToken = default);
}

