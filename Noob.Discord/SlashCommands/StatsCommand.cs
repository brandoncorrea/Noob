using Discord;
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
            .WithAuthor(command.User.Username, command.User.GetAvatarUrl() ?? command.User.GetDefaultAvatarUrl())
            .WithTitle("Stats")
            .WithDescription(
                $"Niblets: {user.Niblets}\n" +
                $"Brownie Points: {user.BrowniePoints}\n" +
                $"Level: {user.Level}\n" +
                $"Experience: {user.Experience}")
            .WithColor(Color.Green)
            .Build();
        await command.RespondAsync(embed: embed);
    }
}
