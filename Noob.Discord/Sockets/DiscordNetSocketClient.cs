using Discord.WebSocket;
namespace Noob.Discord.Sockets;

public class DiscordNetSocketClient : DiscordSocketClient, ISocketClient
{
    ISocketGuild ISocketClient.GetGuild(ulong id) =>
        new DiscordNetSocketGuild(this.GetGuild(id));
}
