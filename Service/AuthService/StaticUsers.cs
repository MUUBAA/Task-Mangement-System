namespace TaskManagement.Service.AuthService
{
    public static class StaticUsers
    {
        public static readonly List<AppUser> Users =
       [
           new AppUser
            {
                UserId = "admin-1",
                UserName = "admin",
                Password = "Admin@123",
                Role = "Admin"
            },
            new AppUser
            {
                UserId = "user-1",
                UserName = "user",
                Password = "User@123",
                Role = "User"
            }
       ];

        public static AppUser? Validate(string username, string password) =>
            Users.FirstOrDefault(u =>
                u.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)
                && u.Password == password);
    }

    public class AppUser
    {
        public string UserId { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}

