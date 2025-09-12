using System;
using System.Collections.Generic;
namespace onlineBookstoreAPI.Dtos
{
    public class BookDtos
    {
        public class BookCreateDto
        {
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public string? ImageUrl { get; set; }
            public int AuthorId { get; set; }
            public int CategoryId { get; set; }
            public IFormFile? Image { get; set; }
        }
        public class BookReadDto
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public string? ImageUrl { get; set; }
            public DateTime Created { get; set; }
            public IFormFile? Image { get; set; }
            public AuthorDtos.AuthorReadDto Author { get; set; } = null!;
            public CategoryDtos.CategoryReadDto Category { get; set; } = null!;
        }
    }
}
