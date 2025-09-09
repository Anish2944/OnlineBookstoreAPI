using System.ComponentModel.DataAnnotations;

namespace onlineBookstoreAPI.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Book> Books { get; set; } = null!;
    }
}
