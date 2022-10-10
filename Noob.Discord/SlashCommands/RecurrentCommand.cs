using Discord;
using Noob.Core.Helpers;
using Noob.Core.Models;
using Noob.DL;
namespace Noob.Discord.SlashCommands;

public abstract class RecurrentCommand : ISlashCommandHandler
{
    public abstract string CommandName { get; }
    internal int IntervalDays;
    internal int CommandId;
    internal abstract int GetNiblets();

    IUserRepository UserRepository;
    IUserCommandRepository UserCommandRepository;

    public RecurrentCommand(
        IUserRepository userRepository,
        IUserCommandRepository userCommandRepository)
    {
        UserRepository = userRepository;
        UserCommandRepository = userCommandRepository;
    }

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = $"Redeem your {CommandName} Niblets!"
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        User user = UserRepository.FindOrCreate(command.User.Id);

        UserCommand userCommand = UserCommandRepository.Find(user.Id, CommandId);
        if (userCommand == null)
            CreateNewUserCommand(user, CommandId);
        else if (userCommand.ExecutedAt.LessThanDaysAgo(IntervalDays))
        {
            await command.RespondAsync($"Your {CommandName} reward will be ready in {Formatting.TimeFromNow(userCommand.ExecutedAt.AddDays(IntervalDays))}!", ephemeral: true);
            return;
        }
        else
            ResetCommandTimestamp(userCommand);

        int newNiblets = GetNiblets();
        user.Niblets += newNiblets;
        UserRepository.Save(user);
        await command.RespondAsync($"{command.User.Username} received {Formatting.NibletTerm(newNiblets)}!");
    }

    private void ResetCommandTimestamp(UserCommand userCommand)
    {
        userCommand.ExecutedAt = DateTime.Now;
        UserCommandRepository.Save(userCommand);
    }

    private void CreateNewUserCommand(User user, int commandId) =>
        UserCommandRepository.Save(new UserCommand
        {
            UserId = user.Id,
            CommandId = commandId,
            ExecutedAt = DateTime.Now
        });
}
