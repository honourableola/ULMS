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
    public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.ToTable("Courses");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Name).IsRequired();
            builder.HasMany<Module>(h => h.Modules)
                .WithOne(m => m.Course)
                .HasForeignKey(n => n.CourseId);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();
        }
    }
}
