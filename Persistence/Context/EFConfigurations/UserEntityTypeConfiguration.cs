using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Context.EFConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(o => o.Id);
            builder.HasOne(o => o.Learner)
                .WithOne(a => a.User)
                .HasForeignKey<Learner>(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Admin)
                .WithOne(a => a.User)
                .HasForeignKey<Admin>(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Instructor)
                .WithOne(a => a.User)
                .HasForeignKey<Instructor>(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(o => o.UserRoles)
                .WithOne(a => a.User)
                .HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(c => c.RowVersion)
               .IsRowVersion();
        }
    }
}
