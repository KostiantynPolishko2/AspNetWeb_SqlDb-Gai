using Microsoft.EntityFrameworkCore;

namespace _20240723_SqlDb_Gai.Models
{
    public class CarContext : DbContext
    {
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Mark> Marks { get; set; } = null!;
        public CarContext(DbContextOptions<CarContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().ToTable("cars");
            modelBuilder.Entity<Mark>().ToTable("marks");
        }
    }
}
