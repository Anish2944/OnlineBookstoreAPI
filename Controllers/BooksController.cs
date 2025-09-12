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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _db.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);

            if(book == null) return NotFound();

            return book;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Book>> CreateBook([FromForm] BookDtos.BookCreateDto book)
        {
            string? imageUrl = null;   

            if(book.Image != null && book.Image.Length > 0)
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

            return CreatedAtAction(nameof(GetBook), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] BookDtos.BookCreateDto book)
        {
            var existingBook = await _db.Books.FindAsync(id);
            if (existingBook == null) return NotFound();

            if(book.Image != null && book.Image.Length > 0)
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

                if (existingBook.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingBook.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                existingBook.ImageUrl = $"/images/{fileName}";
            }

            existingBook.Title = book.Title;
            existingBook.Description = book.Description;
            existingBook.Price = book.Price;
            existingBook.Stock = book.Stock;
            existingBook.Created = DateTime.UtcNow;
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
