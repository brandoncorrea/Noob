using Microsoft.EntityFrameworkCore;
using Noob.DL;
namespace Noob.Migrate;

/// <summary>
/// Create Items and UserItems tables
/// </summary>
public class M20221010 : IMigration
{
    const string CreateQuery =
        "CREATE TABLE \"Items\" (" +
        "    \"Id\" INTEGER NOT NULL CONSTRAINT \"PK_Items\" PRIMARY KEY AUTOINCREMENT," +
        "    \"Name\" TEXT NOT NULL," +
        "    \"Description\" TEXT NOT NULL," +
        "    \"Price\" INTEGER NOT NULL," +
        "    \"Level\" INTEGER NOT NULL" +
        ");" +
        "CREATE TABLE \"UserItems\" (" +
        "    \"UserId\" INTEGER NOT NULL," +
        "    \"ItemId\" INTEGER NOT NULL," +
        "    CONSTRAINT \"PK_UserItems\" PRIMARY KEY (\"UserId\", \"ItemId\")" +
        ");";

    public void Migrate(NoobDbContext db) =>
        db.Database.ExecuteSqlRaw(CreateQuery);
}
