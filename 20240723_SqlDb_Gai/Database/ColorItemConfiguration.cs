using _20240723_SqlDb_Gai.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _20240723_SqlDb_Gai.Database
{
    public class ColorItemConfiguration : IEntityTypeConfiguration<ColorItem>
    {
        public void Configure(EntityTypeBuilder<ColorItem> builder)
        {
            builder.ToTable("coloritems").HasKey(c => c.name);
            builder.HasIndex(c => c.name).IsUnique();
        }
    }
}
