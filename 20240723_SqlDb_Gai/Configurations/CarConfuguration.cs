using _20240723_SqlDb_Gai.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _20240723_SqlDb_Gai.Configurations
{
    public class CarConfuguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.ToTable("cars").HasKey(c => c.Id);
            builder.Property(c => c.Number).IsRequired().HasDefaultValue(null).HasMaxLength(8);
            builder.HasIndex(c => c.Number).IsUnique().HasDatabaseName("IndexNumber");

            builder.Property(c => c.VinCode).HasDefaultValue("none").HasMaxLength(17); ;
            builder.Property(c => c.Model).IsRequired().HasDefaultValue(null).HasMaxLength(8);
            builder.Property(c => c.Volume).IsRequired().HasDefaultValue(0);

            builder.Property(c => c.MarkId).HasColumnName("FK_MarkId");
            builder.HasOne(c => c._Mark).WithMany(m => m.Cars).HasForeignKey(c => c.MarkId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.ColorId).HasColumnName("FK_ColorId");
            builder.HasOne(c => c._Color).WithMany(m => m.Cars).HasForeignKey(c => c.ColorId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(c => c.HasCheckConstraint("ValidVolume", "Volume > 0 AND Volume <= 6"));
        }
    }
}
