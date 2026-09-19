using System.ComponentModel.DataAnnotations;


namespace Hirealdoor.Dtos.Requests
{

    public class VerifyEmailDto
    {
        public int UserId { get; set; }
        public required string VerifyCode { get; set; }
    }
}