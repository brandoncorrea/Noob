using System;
using Noob.API.Commands;
using Noob.API.Models;
using Noob.API.Repositories;
using Noob.API.Test.Stub;

namespace Noob.API.Test.Commands
{
    [TestFixture]
    public class GiveCommandTest
    {
        [Test]
        public void GiveNothing()
        {
            var from = new User { Id = 1, Niblets = 5 };
            var to = new User { Id = 2 };
            var users = new UserRepositoryStub(new List<User> { from, to });
            var response = new GiveCommand(users).Give(1, 2, "bob", 0);
            Assert.False(response.Success);
            Assert.AreEqual("How many Niblets do you want to give?", response.Message);
        }

        [Test]
        public void GiveNegativeAmount()
        {
            var from = new User { Id = 1, Niblets = 5 };
            var to = new User { Id = 2 };
            var users = new UserRepositoryStub(new List<User> { from, to });
            var response = new GiveCommand(users).Give(1, 2, "bob", -1);
            Assert.False(response.Success);
            Assert.AreEqual("Are you trying to /steal Niblets?", response.Message);
        }

        [Test]
        public void GiveOneNiblet()
        {
            var from = new User { Id = 1, Niblets = 100 };
            var to = new User { Id = 2, Niblets = 3 };
            IUserRepository users = new UserRepositoryStub(new List<User> { from, to });
            var response = new GiveCommand(users).Give(1, 2, "bob", 1);
            from = users.Reload(from);
            to = users.Reload(to);
            Assert.True(response.Success);
            Assert.AreEqual("You gave bob 1 Niblet!", response.Message);
            Assert.AreEqual(0, from.BrowniePoints);
            Assert.AreEqual(99, from.Niblets);
            Assert.AreEqual(4, to.Niblets);
        }

        [Test]
        public void NotEnoughNiblets()
        {
            var from = new User { Id = 1, Niblets = 5 };
            var to = new User { Id = 2 };
            var users = new UserRepositoryStub(new List<User> { from, to });
            var response = new GiveCommand(users).Give(1, 2, "bob", 6);
            Assert.False(response.Success);
            Assert.AreEqual("You don't have enough Niblets!", response.Message);
        }

        [Test]
        public void GiveNibletsToNonexistentUser()
        {
            var from = new User { Id = 1, Niblets = 5, BrowniePoints = 50 };
            var to = new User { Id = 2 };
            IUserRepository users = new UserRepositoryStub(new List<User> { from });
            var response = new GiveCommand(users).Give(1, 2, "charlie", 5);
            from = users.Reload(from);
            to = users.Reload(to);

            Assert.True(response.Success);
            Assert.AreEqual("You gave charlie 5 Niblets, earning yourself 1 Brownie Point :)", response.Message);
            Assert.AreEqual(51, from.BrowniePoints);
            Assert.AreEqual(0, from.Niblets);
            Assert.AreEqual(5, to.Niblets);
        }

        [Test]
        public void GiveFiftyNiblets()
        {
            var from = new User { Id = 1, Niblets = 75, BrowniePoints = 50 };
            var to = new User { Id = 2 };
            IUserRepository users = new UserRepositoryStub(new List<User> { from, to });
            var response = new GiveCommand(users).Give(1, 2, "charlie", 50);
            from = users.Reload(from);
            to = users.Reload(to);

            Assert.True(response.Success);
            Assert.AreEqual("You gave charlie 50 Niblets, earning yourself 10 Brownie Points :)", response.Message);
            Assert.AreEqual(60, from.BrowniePoints);
            Assert.AreEqual(25, from.Niblets);
            Assert.AreEqual(50, to.Niblets);
        }

        [Test]
        public void GiveSelfNiblets()
        {
            var user = new User { Id = 1, Niblets = 75, BrowniePoints = 50 };
            IUserRepository users = new UserRepositoryStub(new List<User> { user });
            var response = new GiveCommand(users).Give(1, 1, "meee", 10);
            user = users.Reload(user);

            Assert.True(response.Success);
            Assert.AreEqual("You gave yourself 10 Niblets!", response.Message);
            Assert.AreEqual(50, user.BrowniePoints);
            Assert.AreEqual(75, user.Niblets);
        }

        [Test]
        public void GiveSelfOneNiblet()
        {
            var user = new User { Id = 1, Niblets = 75, BrowniePoints = 50 };
            IUserRepository users = new UserRepositoryStub(new List<User> { user });
            var response = new GiveCommand(users).Give(1, 1, "meee", 1);
            user = users.Reload(user);

            Assert.True(response.Success);
            Assert.AreEqual("You gave yourself 1 Niblet!", response.Message);
            Assert.AreEqual(50, user.BrowniePoints);
            Assert.AreEqual(75, user.Niblets);
        }
    }
}
