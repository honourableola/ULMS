using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Context.EFConfigurations
{
    public class AssessmentEntityTypeConfiguration : IEntityTypeConfiguration<Assessment>
    {
        public void Configure(EntityTypeBuilder<Assessment> builder)
        {
            builder.ToTable("Assessments");
            builder.HasIndex(a => a.Title).IsUnique();
            builder.Property(a => a.Title).IsRequired();
            builder.HasOne(m => m.Result)
                .WithOne(s => s.Assessment)
                .HasForeignKey<Result>(m => m.AssessmentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
