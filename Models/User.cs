using System.ComponentModel.DataAnnotations;

namespace onlineBookstoreAPI.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User"; // Default role is "User"
        public ICollection<Order> Orders { get; set; } = null!;
    }
}
