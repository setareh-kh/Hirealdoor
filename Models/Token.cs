using System.ComponentModel.DataAnnotations;

namespace Hirealdoor.Models;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class Token : ISqlEntity
{
    public int Id { get; set; }
    [Required] [MaxLength(500)] public string? Hash { get; set; }
    [Required] [MaxLength(500)] public string? Os { get; set; }
    [MaxLength(500)] public string? IpAddress { get; set; }
    [Required] [MaxLength(500)] public string? Browser { get; set; }
    [Required] public int UserId { get; set; }
    public User? User { get; set; }
    [Required] public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
}