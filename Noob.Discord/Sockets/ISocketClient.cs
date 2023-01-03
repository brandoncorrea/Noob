using Discord;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
namespace Noob.Discord.Sockets;

public interface ISocketClient
{
    ISocketGuild GetGuild(ulong id);
    event Func<LogMessage, Task> Log;
    event Func<SocketSlashCommand, Task> SlashCommandExecuted;
    event Func<SocketMessageComponent, Task> SelectMenuExecuted;
    event Func<SocketMessageComponent, Task> ButtonExecuted;
    event Func<SocketGuild, Task> JoinedGuild;
    event Func<Task> Ready;
    Task LoginAsync(TokenType tokenType, string token, bool validateToken = true);
    Task StartAsync();
    IReadOnlyCollection<SocketGuild> Guilds { get; }

    bool CanManageChannels(ISlashCommandInteraction command) =>
        command.GuildId != null &&
        GetGuild((ulong)command.GuildId)
        .CurrentUser
        .GuildPermissions
        .ManageChannels;
}
