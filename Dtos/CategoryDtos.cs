namespace onlineBookstoreAPI.Dtos
{
    public class CategoryDtos
    {
        public class CategoryCreateDto
        {
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
        }
        public class CategoryReadDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Description { get; set; }
        }
    }
}
