using Microsoft.EntityFrameworkCore;
using VakantieProject.Models;

namespace VakantieProject.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
    }
}
