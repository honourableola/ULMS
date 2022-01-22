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
    class ModuleEntityTypeConfiguration : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.ToTable("Modules");
            builder.HasKey(d => d.Id);
            builder.Property(c => c.Name).IsRequired();
            builder.HasMany<Topic>(s => s.Topics)
                .WithOne(d => d.Module)
                .HasForeignKey(a => a.ModuleId);
        }
    }
}
