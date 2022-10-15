using Discord;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class HelpCommandTest
{
    [TestCase]
    public async Task DisplaysHelpMessage()
    {
        var interaction = new InteractionStub();
        await new HelpCommand().HandleAsync(interaction);
        var embed = interaction.RespondAsyncParams.Embed;
        Assert.AreEqual("Help!", embed.Title);
        Assert.AreEqual(Color.Green, embed.Color);

        var description =
            "/daily\n" +
                "Redeem your daily Niblets!\n" +
                "Every 24 hours, you can collect Niblets.\n" +
                "\n" +
                "/weekly\n" +
                "Redeem your weekly Niblets!\n" +
                "Every week, you can collect Niblets.\n" +
                "\n" +
                "/give\n" +
                "Give Niblets to another player, earning yourself Brownie Points!\n" +
                "Provide the user you want to give Niblets to and the amount you want to give. " +
                "Every 5 Niblets given will earn you 1 Brownie Point.\n" +
                "\n" +
                "/steal\n" +
                "Steal Niblets from another player!\n" +
                "Provide the user you want to steal from. " +
                "Your level and equipped items will help determine your success. " +
                "Successful thefts will grant you XP while failures will take await XP. " +
                "Successful thefts are kept secret, but theft failures will be announced.\n" +
                "\n" +
                "/attack\n" +
                "Attack another player!\n" +
                "Provide the user you want to attack. " +
                "Your level and equipped items will help determine your success. " +
                "Successful attacks will grant you XP while failures will take await XP. " +
                "Attacks on other players are always announced.\n" +
                "\n" +
                "/stats\n" +
                "View your player stats.\n" +
                "\n" +
                "/shop\n" +
                "Purchase items from the shopkeeper.\n" +
                "Use your Niblets to purchase tons of nooby items!\n" +
                "\n" +
                "/inventory\n" +
                "See your inventory!\n" +
                "View and equip the items you have collected.\n" +
                "\n" +
                "/help\n" +
                "How to noob. (Displays this message)";

        Assert.AreEqual(description, embed.Description);
    }
}

