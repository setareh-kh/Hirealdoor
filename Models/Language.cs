using System.ComponentModel.DataAnnotations;

namespace Hirealdoor.Models;

public class Language:ISqlEntity
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public required string Name { get; set; }

    public ICollection<Person> Persons { get; set; } = new List<Person>();
}