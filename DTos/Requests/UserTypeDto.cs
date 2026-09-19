using Hirealdoor.Models;

namespace Hirealdoor.Dtos.Requests
{

    public class UserTypeDto
    {
        public int UserId { get; set; }
        public required UserType Type { get; set; }
    }
}