using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using onlineBookstoreAPI.Data;
using onlineBookstoreAPI.Models;
using onlineBookstoreAPI.Dtos;

namespace onlineBookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Category
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDtos.CategoryReadDto>>> GetCategories()
        {
            var categories = await _db.Categories
                .AsNoTracking()
                .ToListAsync();

            var categoryDtos = categories.Select(c => new CategoryDtos.CategoryReadDto
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

            return Ok(categoryDtos);
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDtos.CategoryReadDto>> GetCategory(int id)
        {
            var category = await _db.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            var dto = new CategoryDtos.CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
            };

            return Ok(dto);
        }

        // POST: api/Category
        [HttpPost]
        public async Task<ActionResult<CategoryDtos.CategoryReadDto>> PostCategory(CategoryDtos.CategoryCreateDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            var readDto = new CategoryDtos.CategoryReadDto
            {
                Id = category.Id,
                Name = category.Name,
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, readDto);
        }
    }
}
