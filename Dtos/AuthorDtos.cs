namespace onlineBookstoreAPI.Dtos
{
    public class AuthorDtos
    {
        public class AuthorCreateDto
        {
            public string Name { get; set; } = string.Empty;
            public string? Bio { get; set; }
        }
        public class AuthorReadDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string? Bio { get; set; }
        }
    }
}
