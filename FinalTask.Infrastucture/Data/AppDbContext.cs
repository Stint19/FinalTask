using FinalTask.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalTask.Infrastucture.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }
    }
}
