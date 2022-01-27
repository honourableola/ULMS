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
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(f => f.Id);
            builder.Property(o => o.Name).IsRequired();
            builder.HasMany<Course>(d => d.Courses)
                .WithOne(a => a.Category)
                .HasForeignKey(s => s.CategoryId);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();

        }
    }
}
