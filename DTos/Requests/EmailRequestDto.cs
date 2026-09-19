using System.ComponentModel.DataAnnotations;


namespace Hirealdoor.Dtos.Requests
{

    public class EmailRequestDto
    {
        [Required]
        public required string Email { get; set; }
    }
}