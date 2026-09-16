using System.ComponentModel.DataAnnotations;

namespace Hirealdoor.Models
{
   public enum LocationType
    {
        Province = 1, City = 2, Area = 3
    }
    public class Province:ISqlEntity
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; } = null!;
        public LocationType Type { get; set; }
        public int? ParentId { get; set; }
        public Province? Parent { get; set; }
        public ICollection<Province> Children { get; set; }= new List<Province>();
    }
}