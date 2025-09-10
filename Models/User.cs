using System.ComponentModel.DataAnnotations;

namespace onlineBookstoreAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Default role is "User"
        public ICollection<Order> Orders { get; set; } = null!;
    }
}
