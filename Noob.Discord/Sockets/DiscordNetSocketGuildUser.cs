using Discord;
using Discord.WebSocket;
namespace Noob.Discord.Sockets;

public class DiscordNetSocketGuildUser : ISocketGuildUser
{
    private readonly SocketGuildUser User;
    public DiscordNetSocketGuildUser(SocketGuildUser user) => User = user;
    public GuildPermissions GuildPermissions => User.GuildPermissions;
}
