using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryData.Models;

public abstract class Entity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; init; } = null;


    public Entity(int? id)
    {
        Id = id;
    }

    public Entity()
    { }
}