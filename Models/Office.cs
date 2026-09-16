using System.ComponentModel.DataAnnotations;

namespace Hirealdoor.Models
{
    public class Office : ISqlEntity
    {
        public int Id { get; set; } // PK + FK → User.Id
        public User User { get; set; } = null!;

        [Required, MaxLength(250)]
        public required string OfficeName { get; set; }

        [Required, MaxLength(4)]
        public int YearFounded { get; set; }

        [Required, MaxLength(500)]
        public required string Description { get; set; }
        //
        public int CityId { get; set; }
        public Province City { get; set; } = null!;
        //
        [Required, MaxLength(250)]
        public required string StreetAddress { get; set; }

        [Required, MaxLength(250)]
        public required string PostalCode { get; set; }
        public bool Headquarters { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime UpdatedAt { get; set; }
        //
        public ICollection<Person>? Employees { get; set; }



    }
}