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

        public SlashCommandHandler(
            IUserRepository userRepository,
            IUserCommandRepository userCommandRepository)
        {
            RecurrentCommandHandler = new RecurrentCommand(userRepository, userCommandRepository);
            GiveCommandHandler = new GiveCommand(userRepository);
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
            switch (command.Data.Name)
            {
                case "daily":
                    await command.RespondAsync(RecurrentCommandHandler.Daily(command).Message);
                    break;
                case "weekly":
                    await command.RespondAsync(RecurrentCommandHandler.Weekly(command).Message);
                    break;
                case "give":
                    await GiveCommandHandler.Give(command);
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

