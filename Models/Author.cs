using System.ComponentModel.DataAnnotations;

namespace onlineBookstoreAPI.Models
{
    public class Author
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Bio { get; set; } = "";
        public ICollection<Book> Books { get; set; } = null!;

    }
}
