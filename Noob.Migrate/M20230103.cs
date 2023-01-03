using Microsoft.EntityFrameworkCore;
using Noob.DL;
namespace Noob.Migrate;

/// <summary>
/// Create new GuildCounts table
/// </summary>
public class M20230103 : IMigration
{
    public void Migrate(NoobDbContext db) =>
        db.Database.ExecuteSqlRaw(
            "CREATE TABLE \"GuildCounts\" (" +
            "    \"GuildId\" INTEGER NOT NULL CONSTRAINT \"PK_GuildCounts\" PRIMARY KEY AUTOINCREMENT," +
            "    \"ChannelId\" INTEGER NOT NULL," +
            "    \"LastUserId\" INTEGER NOT NULL," +
            "    \"Current\" INTEGER NOT NULL," +
            "    \"Record\" INTEGER NOT NULL);"
        );
}
