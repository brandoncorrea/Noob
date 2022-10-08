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
        private RecurrentCommand RecurrentCommandHandler;
        private GiveCommand GiveCommandHandler;
        private StealCommand StealCommandHandler;

        public SlashCommandHandler(
            IUserRepository userRepository,
            IUserCommandRepository userCommandRepository)
        {
            RecurrentCommandHandler = new RecurrentCommand(userRepository, userCommandRepository);
            GiveCommandHandler = new GiveCommand(userRepository);
            StealCommandHandler = new StealCommand(userRepository);
            SlashCommands = CreateSlashCommands();
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
                    "Give Niblets to another player, earning yourself Brownie Points!",
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
                ),
                (
                    "steal",
                    "Steal Niblets from another player!",
                    new List<SlashCommandOptionBuilder>()
                    {
                        new SlashCommandOptionBuilder
                        {
                            Name = "victim",
                            Description = "The person you will be stealing from.",
                            Type = ApplicationCommandOptionType.User,
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
            switch (command.Data.Name)
            {
                case "daily":
                    await RecurrentCommandHandler.Daily(command);
                    break;
                case "weekly":
                    await RecurrentCommandHandler.Weekly(command);
                    break;
                case "give":
                    await GiveCommandHandler.Give(command);
                    break;
                case "steal":
                    await StealCommandHandler.Steal(command);
                    break;
            }
        }

        public async Task RegisterGuild(SocketGuild guild)
        {
            foreach (var command in SlashCommands)
                await guild.CreateApplicationCommandAsync(command);
        }
    }
}

