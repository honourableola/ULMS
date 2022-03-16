using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Context.EFConfigurations
{
    public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");
            builder.HasKey(o => o.Id);
            builder.HasIndex(o => new { o.UserId, o.RoleId })
                .IsUnique();
            builder.Property(c => c.RowVersion)
                .IsRowVersion();
               
        }

       
    }
}
