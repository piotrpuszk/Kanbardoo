using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Kanbardoo.Domain.Entities;
public class Entity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "serial")]
    public int ID { get; set; }

    public bool Exists()
    {
        return ID != default;
    }

    public static bool ColumnExists<T>(string columnName) where T : Entity
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(e => e.Name).Contains(columnName);
    }
}
