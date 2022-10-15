using Discord;
using Noob.Discord.Helpers;

namespace Noob.Discord.SlashCommands;

public class HelpCommand : ISlashCommandHandler
{
    public string CommandName => "help";

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "How to noob."
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command) =>
        await command.RespondAsync(embed: new EmbedBuilder()
            .WithTitle("Help!")
            .WithColor(Color.Green)
            .WithDescription(
                "/daily",
                "Redeem your daily Niblets!",
                "Every 24 hours, you can collect Niblets.",
                "",
                "/weekly",
                "Redeem your weekly Niblets!",
                "Every week, you can collect Niblets.",
                "",
                "/give",
                "Give Niblets to another player, earning yourself Brownie Points!",
                "Provide the user you want to give Niblets to and the amount you want to give. " +
                "Every 5 Niblets given will earn you 1 Brownie Point.",
                "",
                "/steal",
                "Steal Niblets from another player!",
                "Provide the user you want to steal from. " +
                "Your level and equipped items will help determine your success. " +
                "Successful thefts will grant you XP while failures will take await XP. " +
                "Successful thefts are kept secret, but theft failures will be announced.",
                "",
                "/attack",
                "Attack another player!",
                "Provide the user you want to attack. " +
                "Your level and equipped items will help determine your success. " +
                "Successful attacks will grant you XP while failures will take await XP. " +
                "Attacks on other players are always announced.",
                "",
                "/stats",
                "View your player stats.",
                "",
                "/shop",
                "Purchase items from the shopkeeper.",
                "Use your Niblets to purchase tons of nooby items!",
                "",
                "/inventory",
                "See your inventory!",
                "View and equip the items you have collected.",
                "",
                "/help",
                "How to noob. (Displays this message)"
            )
            .Build());
}
