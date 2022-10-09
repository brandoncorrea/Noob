using Microsoft.EntityFrameworkCore;
namespace Noob.DL;

public class SqlLiteDbContext : NoobDbContext
{
    public SqlLiteDbContext(DbContextOptions options) : base(options) { }
    public static SqlLiteDbContext Create(string connectionString) =>
        new SqlLiteDbContext(
            new DbContextOptionsBuilder<NoobDbContext>()
                .UseSqlite(connectionString)
                .Options);
}
