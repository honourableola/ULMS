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
    public class AssignmentEntityTypeConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("Assignments");
            builder.HasKey(a => a.Id);
            builder.HasIndex(a => a.Name).IsUnique();
            builder.Property(a => a.Name).IsRequired();
            builder.HasMany<LearnerAssignment>(i => i.LearnerAssignments)
                .WithOne(d => d.Assignment)
                .HasForeignKey(c => c.LearnerId);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();
        }
    }
}
