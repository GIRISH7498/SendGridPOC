using Microsoft.EntityFrameworkCore;
using SendGridPOC.Models;

namespace SendGridPOC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
