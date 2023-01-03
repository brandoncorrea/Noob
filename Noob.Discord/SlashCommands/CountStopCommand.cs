using Discord;
using Discord.WebSocket;
using Noob.Core.Models;
using Noob.Discord.Sockets;
using Noob.DL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Noob.Discord.SlashCommands;

public class CountStopCommand : ISlashCommandHandler
{
    private ISocketClient Client;
    private IGuildCountRepository GuildCountRepository;

    public CountStopCommand(ISocketClient client, IGuildCountRepository guildCountRepository)
    {
        Client = client;
        GuildCountRepository = guildCountRepository;
    }

    public string CommandName => "count-stop";

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "Stop counting on this server.",
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        if (Client.CanManageChannels(command))
        {
            StopCountingGuild(command.GuildId.Value);
            await command.RespondAsync("You are no longer counting.");
        }
        else
            await command.RespondAsync("You do not have permission to do this.", ephemeral: true);
    }

    private void StopCountingGuild(ulong guildId)
    {
        var guildCount = GuildCountRepository.Find(guildId);
        if (guildCount != null)
        {
            guildCount.ChannelId = 0;
            GuildCountRepository.Save(guildCount);
        }
    }
}
