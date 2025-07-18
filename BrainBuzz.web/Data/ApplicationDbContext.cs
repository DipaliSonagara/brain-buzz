using BrainBuzz.web.Models.DbTable;
using Microsoft.EntityFrameworkCore;

namespace BrainBuzz.web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){ }

        public DbSet<Users> Users { get; set; }
    }
}
