using Microsoft.EntityFrameworkCore;

namespace _20240723_SqlDb_Gai.Models
{
    public class CarContext : DbContext
    {
        public CarContext(DbContextOptions<CarContext> options) : base(options) { }
        public DbSet<Car> Cars { get; set; } = null!;
    }
}
