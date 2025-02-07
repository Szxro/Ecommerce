using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ecommerce.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.OwnsOne(x => x.Email, emailOptions =>
        {
            emailOptions.HasIndex(x => x.Value).IsUnique();

            emailOptions.Property(x => x.Value).HasColumnName("email");
        });

        builder.OwnsOne(x => x.Username, usernameOptions =>
        {
            usernameOptions.HasIndex(x => x.Value).IsUnique();

            usernameOptions.Property(x => x.Value).HasColumnName("username");
        });
    }
}