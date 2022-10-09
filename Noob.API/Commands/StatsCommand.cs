using System;
using Discord;
using Noob.API.Repositories;
namespace Noob.API.Commands;

public class StatsCommand
{
    private IUserRepository UserRepository;
    public StatsCommand(IUserRepository userRepository) =>
        UserRepository = userRepository;

    public async Task Stats(ISlashCommandInteraction command)
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
