using System.ComponentModel.DataAnnotations;

namespace Hirealdoor.Dtos.Requests
{

    public class CreatePersonDto
    {
        [Required, MaxLength(250)]
        public required string FullName { get; set; }
        [MaxLength(1000)]
        public string? Bio { get; set; }
        public int UserId { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}