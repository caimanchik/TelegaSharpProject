using Microsoft.EntityFrameworkCore;
using TelegaSharpProject.Infrastructure.Data.Interfaces;
using TelegaSharpProject.Infrastructure.Models;

namespace TelegaSharpProject.Infrastructure.Data;

public class TelegaSharpProjectContext : DbContext
{
    private readonly IConnectionStringProvider _connectionStringProvider;
    
    public TelegaSharpProjectContext(IConnectionStringProvider connectionStringProvider)
    {
        _connectionStringProvider = connectionStringProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        optionsBuilder.UseNpgsql(
            connectionString: _connectionStringProvider.ConnectionString
        );
            base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<User> Users => Set<User>();
    //public DbSet<Discipline> Disciplines => Set<Discipline>();
    //public DbSet<Rating> Ratings => Set<Rating>();
    public DbSet<Work> Works => Set<Work>();
    public DbSet<Comment> Comments => Set<Comment>();
    //public DbSet<Dialog> Dialogs => Set<Dialog>();
}