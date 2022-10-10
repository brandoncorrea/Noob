using Discord;

namespace Noob.Discord.SlashCommands
{
    public interface ISlashCommandHandler
    {
        string CommandName { get; }
        SlashCommandProperties GetSlashCommandProperties();
        Task HandleAsync(ISlashCommandInteraction command);
    }
}
