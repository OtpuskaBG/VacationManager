using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Models.Abstractions;

namespace VacationManager.Core.Services.Abstractions
{
    public interface IService<TEntity, TPrototype> : ICreateService<TEntity, TPrototype>, IReadService<TEntity>, IUpdateService<TEntity, TPrototype>, IDeleteService
    where TEntity : class, IEntity
    where TPrototype : class
    {
    }
}
