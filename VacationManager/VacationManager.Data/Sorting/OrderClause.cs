using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Sorting.Abstractions;

namespace VacationManager.Data.Sorting;

public class OrderClause<TEntity> : IOrderClause<TEntity>
{
    public required Expression<Func<TEntity, object>> Expression { get; init; }
    public bool IsAscending { get; init; } = true;
}
