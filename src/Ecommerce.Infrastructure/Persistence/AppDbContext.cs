using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Extensions;
using Ecommerce.SharedKernel.Common;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveColumnType("datetime2");

        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.ApplyGlobalQueryFilter<Entity>(x => !x.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> User => Set<User>();

    public DbSet<Credentials> Credentials => Set<Credentials>();

    public DbSet<RefreshToken> RefreshToken => Set<RefreshToken>();

    public DbSet<EmailCode> EmailCode => Set<EmailCode>();

    public DbSet<Template> Template => Set<Template>();

    public DbSet<TemplateCategory> TemplateCategory => Set<TemplateCategory>();

    public DbSet<UserTemplate> UserTemplate => Set<UserTemplate>();

    public DbSet<Variable> Variable => Set<Variable>();

    public DbSet<VariableType> VariableType => Set<VariableType>();

    public DbSet<TemplateVariable> TemplateVariable => Set<TemplateVariable>();

    public DbSet<Image> Image => Set<Image>();

    public DbSet<UserImage> UserImage => Set<UserImage>();
}