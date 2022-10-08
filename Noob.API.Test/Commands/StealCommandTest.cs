using System;
using Discord;
using Microsoft.VisualBasic;
using Noob.API.Commands;
using Noob.API.Test.Stub;

namespace Noob.API.Test.Commands
{
    [TestFixture]
    public class StealCommandTest
    {
        [SetUp]
        public void SetUp() => Noobs.Initialize();


        [TestCase]
        public async Task NoBrowniePoints()
        {
            Noobs.Ted.Niblets = 50;
            var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
            Assert.AreEqual("You need Brownie Points to steal from other players.", interaction.RespondAsyncParams.Text);
            Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
            Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
        }

        [TestCase]
        public async Task OneBrowniePointAndTwentyLevelsBelow()
        {
            Noobs.Bill.BrowniePoints = 1;
            Noobs.Bill.Experience = 50;
            Noobs.Ted.Niblets = 50;
            Noobs.Ted.Experience = 2100;
            var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
            Assert.AreEqual("Bill was caught trying to steal from Ted. What a noob!", interaction.RespondAsyncParams.Text);
            Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
            Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
            Assert.Less(Noobs.Bill.Experience, 50);
            Assert.AreEqual(2100, Noobs.Ted.Experience);
        }

        [TestCase]
        public async Task TwentyLevelsBelowWithNoExperience()
        {
            Noobs.Bill.BrowniePoints = 1;
            Noobs.Ted.Niblets = 50;
            Noobs.Ted.Experience = 2000;
            var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
            Assert.AreEqual("Bill was caught trying to steal from Ted. What a noob!", interaction.RespondAsyncParams.Text);
            Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
            Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(0, Noobs.Bill.Experience);
            Assert.AreEqual(2000, Noobs.Ted.Experience);
        }

        [TestCase]
        public async Task OneBrowniePointAndTwentyLevelsAbove()
        {
            Noobs.Bill.BrowniePoints = 1;
            Noobs.Bill.Experience = 2100;
            Noobs.Ted.Niblets = 50;
            Noobs.Ted.Experience = 50;
            var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
            Assert.AreEqual($"You stole {Noobs.Bill.Niblets} Niblets from Ted >:)", interaction.RespondAsyncParams.Text);
            Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
            Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(2100, Noobs.Bill.Experience);
            Assert.Less(Noobs.Ted.Experience, 50);
        }

        [TestCase]
        public async Task TargetHasNoNiblets()
        {
            Noobs.Bill.BrowniePoints = 1;
            Noobs.Bill.Experience = 2100;
            Noobs.Ted.Experience = 50;
            var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
            Assert.AreEqual($"Ted doesn't have any Niblets to steal :(", interaction.RespondAsyncParams.Text);
            Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
            Assert.AreEqual(1, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(2100, Noobs.Bill.Experience);
            Assert.AreEqual(50, Noobs.Ted.Experience);
        }

        [TestCase]
        public async Task TargetDoesNotExist()
        {
            Noobs.Bill.BrowniePoints = 1;
            Noobs.Bill.Experience = 2100;
            Noobs.UserRepository.Delete(Noobs.Ted);
            var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
            Assert.AreEqual($"Ted doesn't have any Niblets to steal :(", interaction.RespondAsyncParams.Text);
            Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
            Assert.AreEqual(1, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(2100, Noobs.Bill.Experience);
            Assert.AreEqual(0, Noobs.Ted.Experience);
        }

        [TestCase]
        public async Task StealsOneNiblet()
        {
            Noobs.Bill.BrowniePoints = 2;
            Noobs.Bill.Experience = 2100;
            Noobs.Ted.Niblets = 1;
            Noobs.Ted.Experience = 50;
            var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
            Assert.AreEqual($"You stole 1 Niblet from Ted >:)", interaction.RespondAsyncParams.Text);
            Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
            Assert.AreEqual(2100, Noobs.Bill.Experience);
            Assert.Less(Noobs.Ted.Experience, 50);
            Assert.AreEqual(1, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(1, Noobs.Bill.Niblets);
            Assert.AreEqual(0, Noobs.Ted.Niblets);
        }

        private async Task<InteractionStub> Steal(IUser user, IUser victim)
        {
            var interaction = new InteractionStub(
                user,
                new List<(string, object)> { ("victim", victim) }
            );

            await new StealCommand(Noobs.UserRepository).Steal(interaction);
            return interaction;
        }
    }
}

