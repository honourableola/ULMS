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
    public class CourseRequestEntityTypeConfiguration : IEntityTypeConfiguration<CourseRequest>
    {
        public void Configure(EntityTypeBuilder<CourseRequest> builder)
        {
            builder.ToTable("CourseRequests");
            builder.HasKey(c => c.Id);
            builder.Property(o => o.CourseId).IsRequired();
            builder.Property(o => o.LearnerId).IsRequired();

        }
    }
}
