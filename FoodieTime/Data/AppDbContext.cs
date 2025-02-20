
using FoodieTime.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodieTime.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; }
       
    }
}
