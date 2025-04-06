using Microsoft.EntityFrameworkCore;
using UserServiceMS.Models;

namespace UserServiceMS.ContextDB
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
