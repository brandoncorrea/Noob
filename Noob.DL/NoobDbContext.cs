using Microsoft.EntityFrameworkCore;
using Noob.Core.Models;
namespace Noob.DL;

public abstract class NoobDbContext : DbContext
{
    public NoobDbContext(DbContextOptions options) : base(options) =>
        Database.EnsureCreated();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCommand>()
            .HasKey(command => new { command.CommandId, command.UserId });
        modelBuilder.Entity<User>()
            .HasKey(user => new { user.Id });
        modelBuilder.Entity<User>()
            .HasMany<UserCommand>()
            .WithOne()
            .HasForeignKey(command => command.UserId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserCommand> UserCommands { get; set; }
}
