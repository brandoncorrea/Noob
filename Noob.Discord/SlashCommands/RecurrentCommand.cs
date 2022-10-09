using Discord;
using Noob.Core.Helpers;
using Noob.Core.Models;
using Noob.DL;
namespace Noob.Discord.SlashCommands;

public class RecurrentCommand
{
    private IUserRepository UserRepository { get; set; }
    private IUserCommandRepository UserCommandRepository { get; set; }

    public RecurrentCommand(
        IUserRepository userRepository,
        IUserCommandRepository userCommandRepository)
    {
        UserRepository = userRepository;
        UserCommandRepository = userCommandRepository;
    }

    public async Task Daily(ISlashCommandInteraction command) =>
        await Recurrent("daily", 1, 1, RandomNibletsDaily, command);
    
    public async Task Weekly(ISlashCommandInteraction command) =>
        await Recurrent("weekly", 7, 2, RandomNibletsWeekly, command);

    private async Task Recurrent(string kind, int interval, int commandId, Func<int> getNiblets, ISlashCommandInteraction command)
    {
        User user = UserRepository.FindOrCreate(command.User.Id);

        UserCommand userCommand = UserCommandRepository.Find(user.Id, commandId);
        if (userCommand == null)
            CreateNewUserCommand(user, commandId);
        else if (userCommand.ExecutedAt.LessThanDaysAgo(interval))
        {
            await command.RespondAsync($"Your {kind} reward will be ready in {Formatting.TimeFromNow(userCommand.ExecutedAt.AddDays(interval))}!", ephemeral: true);
            return;
        }
        else 
            ResetCommandTimestamp(userCommand);

        int newNiblets = getNiblets.Invoke();
        user.Niblets += newNiblets;
        UserRepository.Save(user);
        await command.RespondAsync($"{command.User.Username} received {newNiblets} Niblets!");
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

    private static int RandomNibletsDaily() => new Random().Next(1, 50);
    private static int RandomNibletsWeekly() => new Random().Next(50, 250);
}
