using System;
using Discord;
using Noob.API.Discord;

namespace Noob.API.Test.Discord
{
    [TestFixture]
    public class SlashCommandHandlerTest
    {
        private IEnumerable<SlashCommandProperties> SlashCommands;

        [SetUp]
        public void SetUp() =>
            SlashCommands = SlashCommandHandler.CreateSlashCommands();

        [Test]
        public void CreatesSlashCommandProperties()
        {
            Assert.IsNotNull(FindCommand("daily", "Redeem your daily Niblets!"));
            Assert.IsNotNull(FindCommand("weekly", "Redeem your weekly Niblets!"));
        }

        [Test]
        public void CreatesGiveCommand()
        {
            var command = FindCommand("give", "Give Niblets to another player!");

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

        private SlashCommandProperties FindCommand(string name, string description) =>
            SlashCommands.FirstOrDefault(command =>
                command?.Name.Value == name &&
                command?.Description.Value == description);
    }
}
