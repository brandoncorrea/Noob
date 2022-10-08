using System;
using Discord;
using Noob.API.Commands;
using Noob.API.Models;
using Noob.API.Repositories;
using Noob.API.Test.Stub;

namespace Noob.API.Test.Commands
{
    [TestFixture]
    public class GiveCommandTest
    {
        [SetUp]
        public void SetUp() => Noobs.Initialize();

        [Test]
        public async Task GiveNothing()
        {
            Noobs.Bill.Niblets = 5;

            var interaction = new InteractionStub(
                Noobs.BillDiscord,
                new List<(string, object)>
                {
                    ("recipient", Noobs.TedDiscord),
                    ("amount", 0L)
                }
            );

            await new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.AreEqual("How many Niblets do you want to give?", interaction.RespondAsyncParams.Text);
        }

        [Test]
        public async Task GiveNegativeAmount()
        {
            Noobs.Bill.Niblets = 5;

            var interaction = new InteractionStub(
                Noobs.BillDiscord,
                new List<(string, object)>
                {
                    ("recipient", Noobs.TedDiscord),
                    ("amount", -1L)
                }
            );

            await new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.AreEqual("Are you trying to /steal Niblets?", interaction.RespondAsyncParams.Text);
        }

        [Test]
        public async Task GiveOneNiblet()
        {
            Noobs.Bill.Niblets = 100;
            Noobs.Ted.Niblets = 3;

            var interaction = new InteractionStub(
                Noobs.BillDiscord,
                new List<(string, object)>
                {
                    ("recipient", Noobs.TedDiscord),
                    ("amount", 1L)
                }
            );

            await new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.AreEqual("You gave Ted 1 Niblet!", interaction.RespondAsyncParams.Text);
            Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(99, Noobs.Bill.Niblets);
            Assert.AreEqual(4, Noobs.Ted.Niblets);
        }

        [Test]
        public async Task NotEnoughNiblets()
        {
            Noobs.Bill.Niblets = 5;

            var interaction = new InteractionStub(
                Noobs.BillDiscord,
                new List<(string, object)>
                {
                    ("recipient", Noobs.TedDiscord),
                    ("amount", 6L)
                }
            );

            await new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.AreEqual("You don't have enough Niblets!", interaction.RespondAsyncParams.Text);
        }

        [Test]
        public async Task GiveNibletsToNonexistentUser()
        {
            Noobs.Bill.Niblets = 5;
            Noobs.Bill.BrowniePoints = 50;
            Noobs.UserRepository.Delete(Noobs.Ted);

            var interaction = new InteractionStub(
                Noobs.BillDiscord,
                new List<(string, object)>
                {
                    ("recipient", Noobs.TedDiscord),
                    ("amount", 5L)
                }
            );

            await new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.AreEqual("You gave Ted 5 Niblets, earning yourself 1 Brownie Point :)", interaction.RespondAsyncParams.Text);
            Assert.AreEqual(51, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(0, Noobs.Bill.Niblets);
            Assert.AreEqual(5, Noobs.Ted.Niblets);
        }

        [Test]
        public async Task GiveFiftyNiblets()
        {
            Noobs.Bill.Niblets = 75;
            Noobs.Bill.BrowniePoints = 50;

            var interaction = new InteractionStub(
                Noobs.BillDiscord,
                new List<(string, object)>
                {
                    ("recipient", Noobs.TedDiscord),
                    ("amount", 50L)
                }
            );

            await new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.AreEqual("You gave Ted 50 Niblets, earning yourself 10 Brownie Points :)", interaction.RespondAsyncParams.Text);
            Assert.AreEqual(60, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(25, Noobs.Bill.Niblets);
            Assert.AreEqual(50, Noobs.Ted.Niblets);
        }

        [Test]
        public async Task GiveSelfNiblets()
        {
            Noobs.Bill.Niblets = 75;
            Noobs.Bill.BrowniePoints = 50;

            var interaction = new InteractionStub(
                Noobs.BillDiscord,
                new List<(string, object)>
                {
                    ("recipient", Noobs.BillDiscord),
                    ("amount", 10L)
                }
            );

            await new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.AreEqual("You gave yourself 10 Niblets!", interaction.RespondAsyncParams.Text);
            Assert.AreEqual(50, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(75, Noobs.Bill.Niblets);
        }

        [Test]
        public async Task GiveSelfOneNiblet()
        {
            Noobs.Bill.Niblets = 75;
            Noobs.Bill.BrowniePoints = 50;

            var interaction = new InteractionStub(
                Noobs.BillDiscord,
                new List<(string, object)>
                {
                    ("recipient", Noobs.BillDiscord),
                    ("amount", 1L)
                }
            );

            await new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.AreEqual("You gave yourself 1 Niblet!", interaction.RespondAsyncParams.Text);
            Assert.AreEqual(50, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(75, Noobs.Bill.Niblets);
        }
    }
}
