using _20240723_SqlDb_Gai.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _20240723_SqlDb_Gai.Database
{
    public class MarkConfuguration : IEntityTypeConfiguration<Mark>
    {
        public void Configure(EntityTypeBuilder<Mark> builder)
        {
            builder.ToTable("marks");
        }
    }
}
