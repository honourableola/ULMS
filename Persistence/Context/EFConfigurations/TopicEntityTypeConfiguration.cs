using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Context.EFConfigurations
{
    public class TopicEntityTypeConfiguration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            builder.ToTable("Topics");
            builder.HasKey(u => u.Id);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();
            //builder.Property(u => u.ModuleId).HasColumnType("Guid").IsRequired();

        }
    }
}
