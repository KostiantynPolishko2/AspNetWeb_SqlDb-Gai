using Microsoft.EntityFrameworkCore;
using _20240723_SqlDb_Gai.Models;

namespace _20240723_SqlDb_Gai.Database
{
    public class CarContext : DbContext
    {
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Mark> Marks { get; set; } = null!;
        public DbSet<Color> Colors { get; set; } = null!;
        public CarContext(DbContextOptions<CarContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarConfuguration());
            modelBuilder.ApplyConfiguration(new MarkConfuguration());
            modelBuilder.ApplyConfiguration(new ColorConfuguration());

            modelBuilder.Ignore<CarMarkPaint>();
        }
    }
}
