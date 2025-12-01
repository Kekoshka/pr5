using Microsoft.EntityFrameworkCore;
using pr5.Models;

namespace pr5.Context
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options) 
        {
            Database.EnsureCreatedAsync();
        }
        public DbSet<User> Users { get; set; }
    }
}
