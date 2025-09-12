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
    public class AuthorController : Controller
    {
        private readonly AppDbContext _db;

        public AuthorController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return await _db.Authors.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await _db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
            if (author == null) return NotFound();
            return author;
        }
        [HttpPost]
        public async Task<ActionResult<Author>> CreateAuthor(AuthorDtos.AuthorCreateDto author)
        {
            var newAuthor = new Author
            {
                Name = author.Name,
                Bio = author.Bio ?? string.Empty
            };
            _db.Authors.Add(newAuthor);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAuthor), new { id = newAuthor.Id }, newAuthor);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, AuthorDtos.AuthorCreateDto author)
        {
            var existingAuthor = await _db.Authors.FindAsync(id);
            if (existingAuthor == null) return NotFound();
            existingAuthor.Name = author.Name;
            existingAuthor.Bio = author.Bio ?? string.Empty;
            await _db.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _db.Authors.FindAsync(id);
            if (author == null) return NotFound();
            _db.Authors.Remove(author);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
