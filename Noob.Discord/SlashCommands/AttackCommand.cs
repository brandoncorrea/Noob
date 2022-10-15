using Discord;
using Noob.Core.Helpers;
using Noob.Core.Models;
using Noob.DL;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Noob.Discord.SlashCommands;

public class AttackCommand : ISlashCommandHandler
{
    public string CommandName => "attack";
    private IUserRepository UserRepository;
    private IItemRepository ItemRepository;
    private IEquippedItemRepository EquippedItemRepository;

    public static string[] SuccessMessages = new[]
    {
        "{0} just beat the living daylights out of {1}!",
        "{0} shoved {1} off the edge of a cliff.",
        "{0} knocked {1} into the middle of next week!",
        "{0} smacked {1} upside the head!",
    };

    public static string[] FailureMessages = new[]
    {
        "{0} tried attacking {1} and got PWND!",
        "{0} went to do a roundhouse kick on {1}, but fell on their face instead.",
        "{0}'s plan to train an army of bees to sting {1} dreadfully backfired.",
        "{0} accidentally gave {1} the wrong cup of wine and wound up drinking the poison.",
    };

    public AttackCommand(
        IUserRepository userRepository,
        IItemRepository itemRepository,
        IEquippedItemRepository equippedItemRepository)
    {
        UserRepository = userRepository;
        ItemRepository = itemRepository;
        EquippedItemRepository = equippedItemRepository;
    }

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "Attack another player!",
            Options = new List<SlashCommandOptionBuilder>
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
        var user = UserRepository.FindOrCreate(command.User.Id);
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
        await RespondRandomMessage(FailureMessages, command, discordTarget);
    }

    private async Task AttackTarget(ISlashCommandInteraction command, IUser discordTarget, User user, User victim)
    {
        AddExperienceTo(user, CalculateExperience(user, victim));
        RemoveExperienceFrom(victim, CalculateExperience(victim, user));
        UserRepository.Save(user);
        UserRepository.Save(victim);
        await RespondRandomMessage(SuccessMessages, command, discordTarget);
    }

    private async Task RespondRandomMessage(string[] messages, ISlashCommandInteraction interaction, IUser target) =>
        await interaction.RespondAsync(string.Format(messages.RandomChoice(), interaction.User.Username, target.Username));

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
        var userRoll = random.Next(1, 20) + modifier + EquippedItemRepository.AttackBonus(user);
        var victimRoll = random.Next(1, 20) + EquippedItemRepository.DefenseBonus(victim);
        return userRoll > victimRoll;
    }
}
