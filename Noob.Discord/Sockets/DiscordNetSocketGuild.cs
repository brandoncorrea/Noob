using Discord.WebSocket;
namespace Noob.Discord.Sockets;

public class DiscordNetSocketGuild : ISocketGuild
{
    private readonly SocketGuild Guild;
    public DiscordNetSocketGuild(SocketGuild guild) => Guild = guild;
    public ISocketGuildUser CurrentUser =>
        new DiscordNetSocketGuildUser(Guild.CurrentUser);
}
