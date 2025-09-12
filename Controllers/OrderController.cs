using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using onlineBookstoreAPI.Models;
using onlineBookstoreAPI.Data;
using onlineBookstoreAPI.Dtos;
using onlineBookstoreAPI.Common;

namespace onlineBookstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .ToListAsync();
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
        {
            var userId = User.GetUserId(); // custom extension from auth part
            if (userId == null) return Unauthorized();

            return await _db.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var order = await _db.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Book)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId ==  userId);

            if(order == null) return NotFound();

            return order;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody] List<OrderItem> items)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            if (items.Count == 0) return BadRequest("No item in Order");

            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.UtcNow,
                OrderItems = items,
                TotalAmount = items.Sum(i => i.UnitPrice * i.Quantity)
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }
    }
}
