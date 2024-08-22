using _20240723_SqlDb_Gai.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _20240723_SqlDb_Gai.Database
{
    public class ColorConfuguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable("colors");
        }
    }
}
