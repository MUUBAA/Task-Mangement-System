namespace TaskManagement.Data.Dto
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
