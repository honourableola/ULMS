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
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(o => o.Id);
            builder.HasMany(o => o.UserRoles)
                .WithOne(a => a.Role)
                .HasForeignKey(o => o.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();
        }
    }
}
