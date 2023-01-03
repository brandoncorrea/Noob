using Discord;
using Discord.WebSocket;
using Noob.Core.Models;
using Noob.Discord.Sockets;
using Noob.DL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Noob.Discord.SlashCommands;

public class CountStartCommand : ISlashCommandHandler
{
    private ISocketClient Client;
    private IGuildCountRepository GuildCountRepository;

    public CountStartCommand(ISocketClient client, IGuildCountRepository guildCountRepository)
    {
        Client = client;
        GuildCountRepository = guildCountRepository;
    }

    public string CommandName => "count-start";

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "Start counting on this channel.",
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        if (command.ChannelId == null)
            await command.RespondAsync("This is not a channel.", ephemeral: true);
        else if (Client.CanManageChannels(command))
        {
            SetChannelForGuild(command.GuildId.Value, command.ChannelId.Value);
            await command.RespondAsync("You are now counting on this channel.");
        }
        else
            await command.RespondAsync("You do not have permission to do this.", ephemeral: true);
    }

    private void SetChannelForGuild(ulong guildId, ulong channelId)
    {
        var guildCount = GuildCountRepository.Find(guildId);
        if (guildCount == null)
            guildCount = new GuildCount
            {
                GuildId = guildId
            };

        guildCount.ChannelId = channelId;
        GuildCountRepository.Save(guildCount);
    }
}
