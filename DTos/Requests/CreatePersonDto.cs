using System.ComponentModel.DataAnnotations;

namespace Hirealdoor.Dtos.Requests
{
    public class CreatePersonDto
    {
        [Required, MaxLength(250)] public required string FullName { get; set; }
        [MaxLength(1000)] public string? Bio { get; set; }

        public string? LangIds { get; set; }
        public string? AreaIds { get; set; }
        public int UserId { get; set; }
        public int OfficeId { get; set; }
        
        public IFormFile? ProfilePicture { get; set; }
    }
}