using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using onlineBookstoreAPI.Data;
using onlineBookstoreAPI.Models;
using onlineBookstoreAPI.Dtos;


namespace onlineBookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _db.Categories.Include(c => c.Books).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _db.Categories.Include(c => c.Books).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();

            return category;
        }
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
    }
}
