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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db)
        {
            _db = db;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDtos.OrderReadDto>>> GetOrders()
        {
            var orders = await _db.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Book)
                .AsNoTracking()
                .ToListAsync();

            var orderDtos = orders.Select(o => new OrderDtos.OrderReadDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                UserId = o.UserId,
                Items = o.OrderItems.Select(oi => new OrderDtos.OrderItemReadDto
                {
                    BookId = oi.BookId,
                    BookTitle = oi.Book.Title,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToList();

            return Ok(orderDtos);
        }

        // GET: api/Order/my
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<OrderDtos.OrderReadDto>>> GetMyOrders()
        {
            var userId = User.GetUserId(); // custom extension
            if (userId == null) return Unauthorized();

            var orders = await _db.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Book)
                .Where(o => o.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            var orderDtos = orders.Select(o => new OrderDtos.OrderReadDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                UserId = o.UserId,
                Items = o.OrderItems.Select(oi => new OrderDtos.OrderItemReadDto
                {
                    BookId = oi.BookId,
                    BookTitle = oi.Book.Title,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToList();

            return Ok(orderDtos);
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDtos.OrderReadDto>> GetOrder(int id)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            var order = await _db.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.Book)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null) return NotFound();

            var dto = new OrderDtos.OrderReadDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                UserId = order.UserId,
                Items = order.OrderItems.Select(oi => new OrderDtos.OrderItemReadDto
                {
                    BookId = oi.BookId,
                    BookTitle = oi.Book.Title,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            return Ok(dto);
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<OrderDtos.OrderReadDto>> CreateOrder([FromBody] OrderDtos.OrderCreateDto orderDto)
        {
            var userId = User.GetUserId();
            if (userId == null) return Unauthorized();

            if (orderDto.Items == null || !orderDto.Items.Any())
                return BadRequest("No items in order");

            // Fetch books and calculate prices
            var bookIds = orderDto.Items.Select(i => i.BookId).ToList();
            var books = await _db.Books.Where(b => bookIds.Contains(b.Id)).ToListAsync();

            if (books.Count != orderDto.Items.Count)
                return BadRequest("One or more books not found");

            var orderItems = orderDto.Items.Select(i =>
            {
                var book = books.First(b => b.Id == i.BookId);
                return new OrderItem
                {
                    BookId = book.Id,
                    Quantity = i.Quantity,
                    UnitPrice = book.Price
                };
            }).ToList();

            var order = new Order
            {
                UserId = userId.Value,
                OrderDate = DateTime.UtcNow,
                OrderItems = orderItems,
                TotalAmount = orderItems.Sum(i => i.UnitPrice * i.Quantity)
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            var readDto = new OrderDtos.OrderReadDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                UserId = order.UserId,
                Items = order.OrderItems.Select(oi => new OrderDtos.OrderItemReadDto
                {
                    BookId = oi.BookId,
                    BookTitle = books.First(b => b.Id == oi.BookId).Title,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, readDto);
        }
    }
}
