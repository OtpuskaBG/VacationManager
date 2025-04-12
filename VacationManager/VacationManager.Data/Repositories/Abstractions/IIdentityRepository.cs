using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Models;
using VacationManager.Data.Models.Abstractions;

namespace VacationManager.Data.Repositories.Abstractions
{
    public interface IIdentityRepository<TEntity>
        where TEntity : class, IIdentityEntity
    {
        Task<TEntity?> GetAsync(string userId, CancellationToken cancellationToken);
        Task<TEntity[]> GetAllAsync(CancellationToken cancellationToken);
        Task UpdateAsync(TEntity entity, CancellationToken cancellationToken);
    }
}
