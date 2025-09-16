namespace onlineBookstoreAPI.Dtos
{
    public class OrderDtos
    {
        public class OrderItemCreateDto
        {
            public int BookId { get; set; }
            public int Quantity { get; set; }
        }

        public class OrderCreateDto
        {
            // You don’t accept TotalAmount from client, compute it inside controller
            public List<OrderItemCreateDto> Items { get; set; } = new List<OrderItemCreateDto>();
        }

        public class OrderReadDto
        {
            public int Id { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalAmount { get; set; }
            public int UserId { get; set; }
            public List<OrderItemReadDto> Items { get; set; } = new List<OrderItemReadDto>();
        }

        public class OrderItemReadDto
        {
            public int BookId { get; set; }
            public string BookTitle { get; set; } = string.Empty;
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
