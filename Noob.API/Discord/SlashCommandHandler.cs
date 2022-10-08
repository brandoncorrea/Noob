using System;
using Discord;
using Discord.WebSocket;
using Noob.API.Commands;
using Noob.API.Repositories;

namespace Noob.API.Discord
{
    public class SlashCommandHandler
    {
        private IEnumerable<SlashCommandProperties> SlashCommands;
        private Dictionary<string, Func<ISlashCommandInteraction, CommandResponse>> Handlers;

        public SlashCommandHandler(
            IUserRepository userRepository,
            IUserCommandRepository userCommandRepository)
        {
            var recurrentCommandHandler = new RecurrentCommand(userRepository, userCommandRepository);
            var giveCommandHandler = new GiveCommand(userRepository);
            SlashCommands = CreateSlashCommands();
            Handlers = new Dictionary<string, Func<ISlashCommandInteraction, CommandResponse>>
            {
                { "daily", recurrentCommandHandler.Daily },
                { "weekly", recurrentCommandHandler.Weekly },
                { "give", giveCommandHandler.Give },
            };
        }

        public static IEnumerable<SlashCommandProperties> CreateSlashCommands() =>
            new List<(String, String, List<SlashCommandOptionBuilder>)>
            {
                (
                    "daily",
                    "Redeem your daily Niblets!",
                    new List<SlashCommandOptionBuilder>()
                ),
                (
                    "weekly",
                    "Redeem your weekly Niblets!",
                    new List<SlashCommandOptionBuilder>()
                ),
                (
                    "give",
                    "Give Niblets to another player!",
                    new List<SlashCommandOptionBuilder>()
                    {
                        new SlashCommandOptionBuilder
                        {
                            Name = "recipient",
                            Description = "The person who will receive the Niblets.",
                            Type = ApplicationCommandOptionType.User,
                            IsRequired = true,
                        },
                        new SlashCommandOptionBuilder
                        {
                            Name = "amount",
                            Description = "The number of Niblets to give.",
                            Type = ApplicationCommandOptionType.Integer,
                            IsRequired = true,
                        }
                    }
                )
            }
            .Select(c => new SlashCommandBuilder
            {
                Name = c.Item1,
                Description = c.Item2,
                Options = c.Item3
            }.Build());

        public async Task Handle(ISlashCommandInteraction command)
        {
            var handler = Handlers[command.Data.Name];
            if (handler == null) return;
            await command.RespondAsync(handler.Invoke(command).Message);
        }

        public async Task RegisterGuild(SocketGuild guild)
        {
            foreach (var command in SlashCommands)
                await guild.CreateApplicationCommandAsync(command);
        }
    }
}

