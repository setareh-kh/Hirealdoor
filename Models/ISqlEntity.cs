using System.ComponentModel.DataAnnotations;

namespace Hirealdoor.Models
{
    public interface ISqlEntity
    {
        [Key]  public int Id  { get; set; }
    }
}