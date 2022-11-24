using Discord;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using Noob.Core.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Noob.Discord.SlashCommands;

public class LoveCommand : ISlashCommandHandler
{
    public string CommandName => "love";

    public static string[] LoveSelfMessages = new[]
    {
        "{0} stares deeply into their eyes in the mirror, then intensely gives the mirror a big smooch. 😘",
        "{0} treats themselves.",
        "{0}'s multiple personalities all come out at once to express extreme gratitude toward each other.",
        "{0} throws themself a birthday party, inviting all their stuffed animals and imaginary friends."
    };

    public static string[] LoveOthersMessages = new[]
    {
        "{0} whispers carelessly into {1}'s ear.",
        "{0} hugs {1} so tightly that they suffocate and die.",
        "{0} stands outside {1}'s window holding a boombox in the air playing In Your Eyes.",
        "{0} serenades {1} with sweet sweet love songs by Barry White.",
        "{0} walks a thousand miles to fall down at {1}'s door.",
        "{0} loves {1} (like a friend 😔)",
        "{0} writes a letter to {1} expressing their true love."
    };

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "Love another player ❤️",
            Options = new List<SlashCommandOptionBuilder>
            {
                new SlashCommandOptionBuilder
                {
                    Name = "user",
                    Description = "The user you love.",
                    Type = ApplicationCommandOptionType.User,
                    IsRequired = true,
                }
            }
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        await command.RespondAsync(
            LoveMessage(
                command.User,
                (IUser)command.Data.Options.First().Value));
    }

    private string LoveMessage(IUser initiator, IUser user) =>
        initiator.Id == user.Id
            ? string.Format(LoveSelfMessages.RandomChoice(), initiator.Username)
            : string.Format(LoveOthersMessages.RandomChoice(), initiator.Username, user.Username);
}

