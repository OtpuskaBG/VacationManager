﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Data.Sorting.Abstractions;

namespace VacationManager.Data.Repositories.Extentions
{
    public static class QueryExtensions
    {
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query, IEnumerable<Expression<Func<TEntity, bool>>> filters)
        {
            foreach (Expression<Func<TEntity, bool>> clause in filters)
                query = query.Where(clause);

            return query;
        }

        public static IQueryable<TEntity> Include<TEntity>(this IQueryable<TEntity> query, IEnumerable<string> navigations)
            where TEntity : class
        {
            foreach (string navigation in navigations)
                query = query.Include(navigation);

            return query;
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, IEnumerable<IOrderClause<TEntity>> orderClauses)
        {
            IOrderedQueryable<TEntity>? orderedQuery = null;
            foreach (var clause in orderClauses)
            {
                if (clause.IsAscending)
                {
                    if (orderedQuery is null)
                        orderedQuery = query.OrderBy(clause.Expression);
                    else
                        orderedQuery = orderedQuery.ThenBy(clause.Expression);
                }
                else
                {
                    if (orderedQuery is null)
                        orderedQuery = query.OrderByDescending(clause.Expression);
                    else
                        orderedQuery = orderedQuery.ThenByDescending(clause.Expression);
                }
            }

            return orderedQuery ?? query;
        }
    }
}
