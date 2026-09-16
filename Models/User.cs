using System.ComponentModel.DataAnnotations;

namespace Hirealdoor.Models
{
    public enum UserType
    {
        Realtor=1,
        PropertyManager=2,
        Office=3
    }
    public class User:ISqlEntity
    {
        public int Id { get; set; }
        [Required,MaxLength(250)]
        public string Email { get; set; } = null!;
        [MaxLength(250)]
        public string? Mobile { get; set; }
        public UserType? Type { get; set; } 
        [MaxLength(250)]
        public string VerifyCode { get; set; } = null!;
        public DateTime ExpiresAtCode { get; set; }
        public DateTime UpdatedAtCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEmailVerified { get; set; }
        //one- to-one :shared primery key
        public Person? Person { get; set; }
        public Office? Office { get; set; }
    }
}