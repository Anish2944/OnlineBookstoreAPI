using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using onlineBookstoreAPI.Models;
using onlineBookstoreAPI.Data;
using onlineBookstoreAPI.Dtos;

namespace onlineBookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _db;

        public BooksController(AppDbContext db)
        {
            _db = db;
        }
        //GET api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDtos.BookReadDto>>> GetBooks()
        {
            var books = await _db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();

            var bookDtos = books.Select(b => new BookDtos.BookReadDto
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                Price = b.Price,
                Stock = b.Stock,
                ImageUrl = b.ImageUrl,
                Created = b.Created,
                Author = new AuthorDtos.AuthorReadDto
                {
                    Id = b.Author.Id,
                    Name = b.Author.Name
                },
                Category = new CategoryDtos.CategoryReadDto
                {
                    Id = b.Category.Id,
                    Name = b.Category.Name
                }
            }).ToList();

            return Ok(bookDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDtos.BookReadDto>> GetBook(int id)
        {
            var book = await _db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
                return NotFound();

            var dto = new BookDtos.BookReadDto
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                Stock = book.Stock,
                ImageUrl = book.ImageUrl,
                Created = book.Created,
                Author = new AuthorDtos.AuthorReadDto
                {
                    Id = book.Author.Id,
                    Name = book.Author.Name
                },
                Category = new CategoryDtos.CategoryReadDto
                {
                    Id = book.Category.Id,
                    Name = book.Category.Name
                }
            };

            return Ok(dto);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<BookDtos.BookReadDto>> CreateBook([FromForm] BookDtos.BookCreateDto book)
        {
            string? imageUrl = null;

            // Case 1: File upload
            if (book.Image != null && book.Image.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(book.Image.FileName)}";
                var filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await book.Image.CopyToAsync(stream);
                }

                imageUrl = $"/images/{fileName}";
            }
            // Case 2: Fallback to URL from form
            else if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                imageUrl = book.ImageUrl;
            }

            var newBook = new Book
            {
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                Stock = book.Stock,
                ImageUrl = imageUrl,
                Created = DateTime.UtcNow,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId
            };

            _db.Books.Add(newBook);
            await _db.SaveChangesAsync();

            // Return DTO instead of entity (to avoid cycles)
            var readDto = new BookDtos.BookReadDto
            {
                Id = newBook.Id,
                Title = newBook.Title,
                Description = newBook.Description,
                Price = newBook.Price,
                Stock = newBook.Stock,
                ImageUrl = newBook.ImageUrl,
                Created = newBook.Created,
                Author = null!,    // you can load Author if you want
                Category = null!   // same for Category
            };

            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, readDto);
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] BookDtos.BookCreateDto book)
        {
            var existingBook = await _db.Books.FindAsync(id);
            if (existingBook == null) return NotFound();

            // Case 1: File upload
            if (book.Image != null && book.Image.Length > 0)
            {
                var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(book.Image.FileName)}";
                var filePath = Path.Combine(uploadsDir, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await book.Image.CopyToAsync(stream);
                }

                // Delete old image if exists and was local
                if (!string.IsNullOrEmpty(existingBook.ImageUrl) && existingBook.ImageUrl.StartsWith("/images/"))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingBook.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                existingBook.ImageUrl = $"/images/{fileName}";
            }
            // Case 2: Fallback to ImageUrl string
            else if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                existingBook.ImageUrl = book.ImageUrl;
            }
            // Case 3: Neither provided → keep old ImageUrl (do nothing)

            // Update other fields
            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.Price = book.Price;
            existingBook.Stock = book.Stock;
            existingBook.Created = DateTime.UtcNow; // <-- consider using Updated field instead
            existingBook.AuthorId = book.AuthorId;
            existingBook.CategoryId = book.CategoryId;

            await _db.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _db.Books.FindAsync(id);
            if (book == null) return NotFound();
            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return NoContent();
        }

    }
}
