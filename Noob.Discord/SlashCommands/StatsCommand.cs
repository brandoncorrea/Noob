using Discord;
using Noob.Discord.Helpers;
using Noob.DL;
namespace Noob.Discord.SlashCommands;

public class StatsCommand : ISlashCommandHandler
{
    public string CommandName => "stats";
    private IUserRepository UserRepository;
    public StatsCommand(IUserRepository userRepository) =>
        UserRepository = userRepository;

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "View your player stats."
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        var user = UserRepository.FindOrCreate(command.User.Id);
        var embed = new EmbedBuilder()
            .WithSimpleAuthor(command.User)
            .WithTitle("Stats")
            .WithDescription(
                $"Niblets: {user.Niblets}",
                $"Brownie Points: {user.BrowniePoints}",
                $"Level: {user.Level}",
                $"Experience: {user.Experience}")
            .WithColor(Color.Green)
            .Build();
        await command.RespondAsync(embed: embed);
    }
}
