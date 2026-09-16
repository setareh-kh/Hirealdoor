using System.ComponentModel.DataAnnotations;

namespace Hirealdoor.Models
{
    public class ServedArea
    {
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;
        public int ProvinceId { get; set; }
        public Province Province { get; set; } = null!;
    }
}