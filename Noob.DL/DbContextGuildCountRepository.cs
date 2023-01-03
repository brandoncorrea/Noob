using Noob.Core.Models;

namespace Noob.DL;

public class DbContextGuildCountRepository : IGuildCountRepository
{
    private NoobDbContext Db;
    public DbContextGuildCountRepository(NoobDbContext db) => Db = db;

    public GuildCount Find(ulong guildId) =>
        Db.GuildCounts.FirstOrDefault(i => i.GuildId == guildId);

    public void Save(GuildCount command)
    {
        if (Find(command.GuildId) == null)
            Db.GuildCounts.Add(command);
        else
            Db.GuildCounts.Update(command);
        Db.SaveChanges();
    }
}
