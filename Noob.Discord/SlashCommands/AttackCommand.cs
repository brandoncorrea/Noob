using Discord;
using Noob.Core.Helpers;
using Noob.Core.Models;
using Noob.DL;
namespace Noob.Discord.SlashCommands;

public class AttackCommand : ISlashCommandHandler
{
    public string CommandName => "attack";
    private IUserRepository UserRepository;

    public AttackCommand(IUserRepository userRepository) =>
        UserRepository = userRepository;

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "Attack another player!",
            Options = new List<SlashCommandOptionBuilder>()
            {
                new SlashCommandOptionBuilder
                {
                    Name = "target",
                    Description = "The person you want to attack.",
                    Type = ApplicationCommandOptionType.User,
                    IsRequired = true,
                }
            }
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        IUser discordTarget = (IUser)command.Data.Options.First().Value;
        if (discordTarget.Id == command.User.Id)
            await command.RespondAsync("Self harm is NOT okay 😭❤️", ephemeral: true);
        else
            await AttemptAttack(command, discordTarget);
    }

    private async Task AttemptAttack(ISlashCommandInteraction command, IUser discordTarget)
    {
        var user = UserRepository.Find(command.User.Id);
        var victim = UserRepository.FindOrCreate(discordTarget.Id);
        if (AttacksSuccessfully(user, victim))
            await AttackTarget(command, discordTarget, user, victim);
        else
            await GetPwnd(command, discordTarget, user, victim);
    }

    private async Task GetPwnd(ISlashCommandInteraction command, IUser discordTarget, User user, User victim)
    {
        RemoveExperienceFrom(user, CalculateExperience(user, victim));
        AddExperienceTo(victim, CalculateExperience(victim, user));
        UserRepository.Save(user);
        UserRepository.Save(victim);
        await command.RespondAsync($"{command.User.Username} tried attacking {discordTarget.Username} and got PWND!");
    }

    private async Task AttackTarget(ISlashCommandInteraction command, IUser discordTarget, User user, User victim)
    {
        AddExperienceTo(user, CalculateExperience(user, victim));
        RemoveExperienceFrom(victim, CalculateExperience(victim, user));
        UserRepository.Save(user);
        UserRepository.Save(victim);
        await command.RespondAsync($"{command.User.Username} just beat the living daylights out of {discordTarget.Username}!");
    }

    private long CalculateExperience(User user, User opponent) =>
        Math.Abs(opponent.Level - user.Level) >= 5
            ? 0 : opponent.Level - user.Level + 5;

    private void AddExperienceTo(User user, long xp)
    {
        if (xp <= 0) return;
        user.Experience += xp;
    }

    private void RemoveExperienceFrom(User user, long xp)
    {
        if (xp <= 0) return;
        user.Experience -= xp;
        if (user.Experience < 0) user.Experience = 0;
    }

    private bool AttacksSuccessfully(User user, User victim)
    {
        var random = new Random();
        var modifier = user.Level - victim.Level;
        var userRoll = random.Next(1, 20) + modifier;
        var victimRoll = random.Next(1, 20);
        return userRoll > victimRoll;
    }
}

