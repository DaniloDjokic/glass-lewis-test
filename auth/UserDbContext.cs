using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthServer;

public class UserDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }
}

public class User : IdentityUser<int>
{
    public string Username { get; set; } = string.Empty;
}
