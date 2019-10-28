using Microsoft.EntityFrameworkCore;

namespace Location.Models
{
    public class LocationDbContext : DbContext
    {
        public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options) { }

        public DbSet<Locations> Locations { get; set; }
    }
}