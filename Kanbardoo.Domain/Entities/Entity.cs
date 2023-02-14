using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Kanbardoo.Domain.Entities;
public class Entity
{
    [Key]
    public int ID { get; set; }

    public static bool ColumnExists<T>(string columnName) where T : Entity
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(e => e.Name).Contains(columnName);
    }
}
