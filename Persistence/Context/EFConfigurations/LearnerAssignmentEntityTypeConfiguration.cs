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
    public class LearnerAssignmentEntityTypeConfiguration : IEntityTypeConfiguration<LearnerAssignment>
    {
        public void Configure(EntityTypeBuilder<LearnerAssignment> builder)
        {
            builder.ToTable("LearnerAssignments");

            builder.HasKey(u => u.Id);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();
        }
    }
}
