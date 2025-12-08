using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace server.Classes
{
    public class context : DbContext
    {
        public context()
        {
            Database.EnsureCreated();
        }
        public DbSet<Client> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=PR5;User Id=DESKTOP-9EF7JJH;Password=;Trusted_Connection=true;Encrypt=false;");
        }
    }
}
//"server=localhost;user=root;password=strong_password_123;database=pr5;"
