using System.Linq.Expressions;
using System.Reflection;

namespace Kanbardoo.Domain.Filters;

public class OrderByClause<T>
{
    public string ColumnName { get; set; } = string.Empty;
    public OrderByOrder Order { get; set; } = OrderByOrder.Asc;
    public Expression<Func<T, object>> Expression(T value)
    {
        return T => typeof(T).GetProperty(ColumnName)!.GetValue(value, null)!;
    }
}
