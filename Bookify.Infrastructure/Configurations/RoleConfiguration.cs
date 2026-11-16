using Bookify.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles");

        builder.HasKey(role => role.Id);

        builder.HasMany(role => role.Users)
            .WithMany(user => user.Roles);

        builder.HasMany(role => role.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>(
                right => right.HasOne<Permission>().WithMany().HasForeignKey(rp => rp.PermissionId),
                left => left.HasOne<Role>().WithMany().HasForeignKey(rp => rp.RoleId));

        builder.HasData(Role.Registered);
    }
}
