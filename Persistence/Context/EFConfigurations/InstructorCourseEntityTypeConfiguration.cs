using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Context.EFConfigurations
{
    public class InstructorCourseEntityTypeConfiguration : IEntityTypeConfiguration<InstructorCourse>
    {
        public void Configure(EntityTypeBuilder<InstructorCourse> builder)
        {
            builder.ToTable("InstructorCourses");

            builder.HasKey(u => u.Id);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();

            /* builder.Property(u => u.CourseId)
                 .HasColumnType("Guid")
                 .IsRequired();*/

            /*builder.Property(u => u.InstructorId)
               .HasColumnType("Guid")
               .IsRequired();*/
        }
    }
}
