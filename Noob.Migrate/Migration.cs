using Microsoft.EntityFrameworkCore.Migrations;
using Noob.DL;
namespace Noob.Migrate;

public class Migration
{
    const string MigrationHistoryPath = "./migrations.txt";

    public static void Migrate(NoobDbContext db)
    {
        Console.WriteLine("Getting Applied Migrations...");
        var applied = GetAppliedMigrations();
        Console.WriteLine("Looking for Migrations...");
        foreach (var migration in GetMigrations())
            MigrateIfNotApplied(applied, migration, db);
        Console.WriteLine("Migrate Complete.");
    }

    public static void MigrateIfNotApplied(string[] applied, Type migration, NoobDbContext db)
    {
        if (applied.Any(name => name == migration.FullName))
            Console.WriteLine($"Skipping {migration.FullName}");
        else
            ExecuteMigration(migration, db);
    }

    public static void ExecuteMigration(Type migration, NoobDbContext db)
    {
        Console.WriteLine($"Migrating {migration.FullName}");
        ((IMigration)Activator.CreateInstance(migration)).Migrate(db);
        Console.WriteLine($"Succeeded {migration.FullName}");
        File.AppendAllText(MigrationHistoryPath, $"{migration.FullName}\n");
    }

    public static string[] GetAppliedMigrations() =>
        File.ReadAllLines(MigrationHistoryPath);

    public static IEnumerable<Type> GetMigrations()
    {
        var type = typeof(IMigration);
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && !p.IsInterface);
    }
}
