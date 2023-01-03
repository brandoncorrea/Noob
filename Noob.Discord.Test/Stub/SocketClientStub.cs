using Discord;
using Discord.WebSocket;
using Noob.Discord.Sockets;
using Noob.DL;

namespace Noob.Discord.Test.Stub;

public class SocketClientStub : ISocketClient
{
    private readonly Dictionary<ulong, ulong> Permissions;
    public ulong CurrentUserId { get; set; }
    public IReadOnlyCollection<SocketGuild> Guilds => throw new NotImplementedException();

    public event Func<LogMessage, Task> Log;
    public event Func<SocketSlashCommand, Task> SlashCommandExecuted;
    public event Func<SocketMessageComponent, Task> SelectMenuExecuted;
    public event Func<SocketMessageComponent, Task> ButtonExecuted;
    public event Func<SocketGuild, Task> JoinedGuild;
    public event Func<Task> Ready;

    public SocketClientStub(Dictionary<ulong, ulong> permissions) =>
        Permissions = permissions;

    public ISocketGuild GetGuild(ulong id) => new SocketGuildStub
    {
        CurrentUser = new SocketGuildUserStub
        {
            RawValue = Permissions[CurrentUserId]
        }
    };

    public Task LoginAsync(TokenType tokenType, string token, bool validateToken = true)
    {
        throw new NotImplementedException();
    }

    public Task StartAsync()
    {
        throw new NotImplementedException();
    }
}
