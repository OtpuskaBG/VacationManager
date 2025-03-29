using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Models.Abstractions;

namespace VacationManager.Core.Services.Abstractions
{
    public interface IUpdateService<TEntity, TPrototype>
    where TEntity : class, IEntity
    where TPrototype : class
    {
        Task<TEntity> UpdateAsync(Guid entityId, TPrototype prototype, CancellationToken cancellationToken);
    }
}
