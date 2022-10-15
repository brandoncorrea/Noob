using System.Runtime.Intrinsics.X86;
using Discord;
using Noob.Discord.Test.Stub;

namespace Noob.Discord.Test;

[TestFixture]
public class SlashCommandHandlerTest
{
    private IEnumerable<SlashCommandProperties> SlashCommands;

    [SetUp]
    public void SetUp() =>
        SlashCommands = new SlashCommandHandler(
            Noobs.UserRepository,
            Noobs.UserCommandRepository,
            Noobs.ItemRepository,
            Noobs.UserItemRepository,
            Noobs.EquippedItemRepository)
        .CreateSlashCommands();

    [Test]
    public void CreatesSlashCommandProperties()
    {
        Assert.IsNotNull(FindCommand("daily", "Redeem your daily Niblets!"));
        Assert.IsNotNull(FindCommand("weekly", "Redeem your weekly Niblets!"));
        Assert.IsNotNull(FindCommand("stats", "View your player stats."));
        Assert.IsNotNull(FindCommand("shop", "Purchase items from the shopkeeper."));
        Assert.IsNotNull(FindCommand("inventory", "See your inventory!"));
        Assert.IsNotNull(FindCommand("help", "How to noob."));
    }

    [Test]
    public void CreatesGiveCommand()
    {
        var command = FindCommand("give", "Give Niblets to another player, earning yourself Brownie Points!");

        var recipientOption = command.Options.Value.First();
        Assert.AreEqual("recipient", recipientOption.Name);
        Assert.AreEqual("The person who will receive the Niblets.", recipientOption.Description);
        Assert.AreEqual(ApplicationCommandOptionType.User, recipientOption.Type);
        Assert.IsTrue(recipientOption.IsRequired);

        var amountOption = command.Options.Value.Last();
        Assert.AreEqual("amount", amountOption.Name);
        Assert.AreEqual("The number of Niblets to give.", amountOption.Description);
        Assert.AreEqual(ApplicationCommandOptionType.Integer, amountOption.Type);
        Assert.IsTrue(amountOption.IsRequired);
    }

    [Test]
    public void CreatesStealCommand()
    {
        var command = FindCommand("steal", "Steal Niblets from another player!");
        var victimOption = command.Options.Value.First();

        Assert.AreEqual("victim", victimOption.Name);
        Assert.AreEqual("The person you will be stealing from.", victimOption.Description);
        Assert.AreEqual(ApplicationCommandOptionType.User, victimOption.Type);
        Assert.IsTrue(victimOption.IsRequired);
    }

    [Test]
    public void CreatesAttackCommand()
    {
        var command = FindCommand("attack", "Attack another player!");
        var targetOption = command.Options.Value.First();

        Assert.AreEqual("target", targetOption.Name);
        Assert.AreEqual("The person you want to attack.", targetOption.Description);
        Assert.AreEqual(ApplicationCommandOptionType.User, targetOption.Type);
        Assert.IsTrue(targetOption.IsRequired);
    }

    private SlashCommandProperties FindCommand(string name, string description) =>
        SlashCommands.FirstOrDefault(command =>
            command?.Name.Value == name &&
            command?.Description.Value == description);
}
