using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context.EFConfigurations
{
    public class InstructorEntityTypeConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.ToTable("Instructors");
            builder.HasKey(s => s.Id);
            builder.Property(p => p.Email).IsRequired();
            builder.HasIndex(s => s.Email).IsUnique();
            builder.HasMany<InstructorCourse>(i => i.InstructorCourses)
                .WithOne(d => d.Instructor)
                .HasForeignKey(c => c.InstructorId);
        }
    }
}
