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
        public void GiveNothing()
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

            var response = new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.False(response.Success);
            Assert.AreEqual("How many Niblets do you want to give?", response.Message);
        }

        [Test]
        public void GiveNegativeAmount()
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

            var response = new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.False(response.Success);
            Assert.AreEqual("Are you trying to /steal Niblets?", response.Message);
        }

        [Test]
        public void GiveOneNiblet()
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

            var response = new GiveCommand(Noobs.UserRepository).Give(interaction);
            var bill = Noobs.UserRepository.Reload(Noobs.Bill);
            var ted = Noobs.UserRepository.Reload(Noobs.Ted);
            Assert.True(response.Success);
            Assert.AreEqual("You gave Ted 1 Niblet!", response.Message);
            Assert.AreEqual(0, bill.BrowniePoints);
            Assert.AreEqual(99, bill.Niblets);
            Assert.AreEqual(4, ted.Niblets);
        }

        [Test]
        public void NotEnoughNiblets()
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

            var response = new GiveCommand(Noobs.UserRepository).Give(interaction);
            Assert.False(response.Success);
            Assert.AreEqual("You don't have enough Niblets!", response.Message);
        }

        [Test]
        public void GiveNibletsToNonexistentUser()
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

            var response = new GiveCommand(Noobs.UserRepository).Give(interaction);
            var bill = Noobs.UserRepository.Reload(Noobs.Bill);
            var ted = Noobs.UserRepository.Reload(Noobs.Ted);

            Assert.True(response.Success);
            Assert.AreEqual("You gave Ted 5 Niblets, earning yourself 1 Brownie Point :)", response.Message);
            Assert.AreEqual(51, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(0, Noobs.Bill.Niblets);
            Assert.AreEqual(5, Noobs.Ted.Niblets);
        }

        [Test]
        public void GiveFiftyNiblets()
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

            var response = new GiveCommand(Noobs.UserRepository).Give(interaction);
            var bill = Noobs.UserRepository.Reload(Noobs.Bill);
            var ted = Noobs.UserRepository.Reload(Noobs.Ted);

            Assert.True(response.Success);
            Assert.AreEqual("You gave Ted 50 Niblets, earning yourself 10 Brownie Points :)", response.Message);
            Assert.AreEqual(60, Noobs.Bill.BrowniePoints);
            Assert.AreEqual(25, Noobs.Bill.Niblets);
            Assert.AreEqual(50, Noobs.Ted.Niblets);
        }

        [Test]
        public void GiveSelfNiblets()
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

            var response = new GiveCommand(Noobs.UserRepository).Give(interaction);
            var bill = Noobs.UserRepository.Reload(Noobs.Bill);

            Assert.True(response.Success);
            Assert.AreEqual("You gave yourself 10 Niblets!", response.Message);
            Assert.AreEqual(50, bill.BrowniePoints);
            Assert.AreEqual(75, bill.Niblets);
        }

        [Test]
        public void GiveSelfOneNiblet()
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

            var response = new GiveCommand(Noobs.UserRepository).Give(interaction);
            var bill = Noobs.UserRepository.Reload(Noobs.Bill);

            Assert.True(response.Success);
            Assert.AreEqual("You gave yourself 1 Niblet!", response.Message);
            Assert.AreEqual(50, bill.BrowniePoints);
            Assert.AreEqual(75, bill.Niblets);
        }
    }
}
