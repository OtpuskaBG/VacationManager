using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationManager.Core.Services.Abstractions
{
    public interface IDeleteService
    {
        Task SoftDeleteAsync(Guid entityId, CancellationToken cancellationToken);
        Task HardDeleteAsync(Guid entityId, CancellationToken cancellationToken);
    }
}
