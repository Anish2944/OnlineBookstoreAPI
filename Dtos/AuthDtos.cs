namespace onlineBookstoreAPI.Dtos
{
    public class AuthDtos
    {
        public class UserRegisterDto
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public string Role { get; set; } = "Customer"; // Default role is "Customer"
        }
        public class UserLoginDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }
        public class AuthResponseDto
        {
            public string Token { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }
    }
}
