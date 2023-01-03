using Discord.WebSocket;
namespace Noob.Discord.Sockets;

public interface ISocketGuild
{
    ISocketGuildUser CurrentUser { get; }
}
