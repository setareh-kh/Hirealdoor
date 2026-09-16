using System.ComponentModel.DataAnnotations;
using Org.BouncyCastle.Asn1.X509.SigI;

namespace Hirealdoor.Models
{
    public class Person : ISqlEntity
    {
        public int Id { get; set; }  // PK + FK → User.Id
        public User User { get; set; } = null!;
        [Required, MaxLength(250)]
        public required string FullName { get; set; }
        [MaxLength(1000)]
        public string? Bio { get; set; }
        [Required]
        public required DateOnly BirthDay { get; set; }= default;
        [Required, MaxLength(250)]
        public required string LicenseNumber { get; set; }=string.Empty;
        [Required]
        public DateTime? CreatedAt { get; set; }
        [Required]
        public DateTime? UpdatedAt { get; set; }
        //
        public int? OfficeId { get; set; }
        public Office? Office { get; set; }
        //
        public ICollection<Language> SpeaksLanguages { get; set; } = new List<Language>();
        public ICollection <ServedArea> ServedAreas { get; set; } = new List<ServedArea>();

    }
}