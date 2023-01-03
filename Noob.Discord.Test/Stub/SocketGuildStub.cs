using Noob.Discord.Sockets;
namespace Noob.Discord.Test.Stub;

public class SocketGuildStub : ISocketGuild
{
    public ISocketGuildUser CurrentUser { get; set; }
}
