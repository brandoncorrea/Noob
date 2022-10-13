using Microsoft.EntityFrameworkCore.Migrations;
using Noob.DL;
namespace Noob.Migrate;

public class Migration
{
    const string MigrationHistoryPath = "./migrations.txt";
    const string MigrationLogPath = "./migrations.log";

    public static void Migrate(NoobDbContext db)
    {
        Log("Getting Applied Migrations...");
        var applied = GetAppliedMigrations();
        Log("Looking for Migrations...");
        foreach (var migration in GetMigrations())
            MigrateIfNotApplied(applied, migration, db);
        Log("Migrate Complete.");
    }

    public static void MigrateIfNotApplied(string[] applied, Type migration, NoobDbContext db)
    {
        if (applied.Any(name => name == migration.FullName))
            Log($"Skipping {migration.FullName}");
        else
            ExecuteMigration(migration, db);
    }

    public static void ExecuteMigration(Type migration, NoobDbContext db)
    {
        try
        {
            Log($"Migrating {migration.FullName}");
            ((IMigration)Activator.CreateInstance(migration)).Migrate(db);
            Log($"Succeeded {migration.FullName}");
            File.AppendAllText(MigrationHistoryPath, $"{migration.FullName}\n");
        }
        catch(Exception ex)
        {
            Log($"Migration Failed! {migration.FullName}");
            Log($"Message: {ex.Message}");
            Log($"Stack Trace: {ex.StackTrace}");
            throw;
        }
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

    private static void Log(string message) =>
        File.AppendAllText(MigrationLogPath, $"{DateTime.Now} | {message}\n");
}
