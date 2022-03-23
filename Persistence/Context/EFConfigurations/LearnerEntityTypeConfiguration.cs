using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Context.EFConfigurations
{
    public class LearnerEntityTypeConfiguration : IEntityTypeConfiguration<Learner>
    {
        public void Configure(EntityTypeBuilder<Learner> builder)
        {
            builder.ToTable("Learners");
            builder.HasKey(s => s.Id);
            builder.Property(p => p.Email).IsRequired();
            builder.HasIndex(s => s.Email).IsUnique();
            builder.HasMany<LearnerCourse>(i => i.LearnerCourses)
                .WithOne(d => d.Learner)
                .HasForeignKey(c => c.LearnerId);
            builder.HasMany<LearnerAssignment>(i => i.LearnerAssignments)
                .WithOne(d => d.Learner)
                .HasForeignKey(c => c.LearnerId);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();
        }
    }
}
