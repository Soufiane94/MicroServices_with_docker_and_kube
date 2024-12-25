using Microsoft.EntityFrameworkCore;
using PlatFormService.Models;

namespace PlatFormService.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> opt) : DbContext(opt)
    {
        public DbSet<Platform> Platforms { get; set; }
    }
}