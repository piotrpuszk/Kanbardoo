using Kanbardoo.Domain.Entities;
using Kanbardoo.Domain.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanbardoo.Infrastructure.Extensions;
internal static class QueryableExtensions
{
    internal static IQueryable<T> OrderByClauses<T>(this IQueryable<T> query, IEnumerable<OrderByClause<T>> orderByClauses)
    {
        if (orderByClauses is null || orderByClauses.Count() == 0)
        {
            return query;
        }

        var first = true;
        IOrderedQueryable<T>? orderedQuery = null;
        foreach (var orderByClause in orderByClauses)
        {
            switch (orderByClause.Order)
            {
                case OrderByOrder.Desc:
                    if (first) orderedQuery = query.OrderByDescending(e => EF.Property<T>(e!, orderByClause.ColumnName));
                    else orderedQuery = orderedQuery!.ThenByDescending(e => EF.Property<T>(e!, orderByClause.ColumnName));
                    break;
                default:
                    if (first) orderedQuery = query.OrderBy(e => EF.Property<T>(e!, orderByClause.ColumnName));
                    else orderedQuery = orderedQuery!.ThenBy(e => EF.Property<T>(e!, orderByClause.ColumnName));
                    break;
            }

            first = false;
        }

        return orderedQuery!.AsQueryable();
    }
}
