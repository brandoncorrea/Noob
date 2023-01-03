using Noob.Core.Models;
namespace Noob.DL;

public interface IGuildCountRepository
{
    GuildCount Find(ulong guildId);
    void Save(GuildCount command);
}
