using Discord;
using Noob.Discord.Sockets;
namespace Noob.Discord.Test.Stub;

public class SocketGuildUserStub : ISocketGuildUser
{
    public ulong RawValue { get; set; }
    public GuildPermissions GuildPermissions => new GuildPermissions(RawValue);
}
