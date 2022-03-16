using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Context.EFConfigurations
{
    public class LearnerCourseEntityTypeConfiguration : IEntityTypeConfiguration<LearnerCourse>
    {
        public void Configure(EntityTypeBuilder<LearnerCourse> builder)
        {
            builder.ToTable("LearnerCourses");

            builder.HasKey(u => u.Id);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();

            /*builder.Property(u => u.CourseId)
                .HasColumnType("Guid")
                .IsRequired();

            builder.Property(u => u.LearnerId)
               .HasColumnType("Guid")
               .IsRequired();*/
        }
    }
}
