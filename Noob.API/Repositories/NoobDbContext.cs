using System;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Noob.API.Models;
namespace Noob.API.Repositories;

public abstract class NoobDbContext : DbContext
{
    public NoobDbContext() => Database.EnsureCreated();

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
