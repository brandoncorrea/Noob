
using Noob.API.Commands;
using Noob.API.Models;
using Noob.API.Test.Stub;

namespace Noob.API.Test.Commands
{
    public class UserServiceTest
    {
        [Test]
        public void UserDoesNotExist()
        {
            var userRepository = new UserRepositoryStub(new List<User>());
            var response = new DailyCommand(userRepository, null).Execute(1);
            Assert.False(response.Success);
            Assert.AreEqual("Your noob could not be found :(", response.Message);
        }

        [Test]
        public void OneHourUntilNextExecution()
        {
            var user = new User { Id = 2 };
            var command = new UserCommand
            {
                CommandId = 1,
                UserId = 2,
                ExecutedAt = DateTime.Now.AddHours(-23).AddMinutes(1)
            };

            var userRepository = new UserRepositoryStub(new List<User> { user });
            var commandRepository = new UserCommandRepositoryStub(new List<UserCommand> { command });
            var response = new DailyCommand(userRepository, commandRepository).Execute(2);
            Assert.False(response.Success);
            Assert.AreEqual("Your daily will be ready in 1 hour!", response.Message);
        }

        [Test]
        public void ThreeHoursTwelveMinutesUntilNextExecution()
        {
            var user = new User { Id = 2 };
            var command = new UserCommand
            {
                CommandId = 1,
                UserId = 2,
                ExecutedAt = DateTime.Now.AddHours(3).AddMinutes(12).AddDays(-1)
            };

            var userRepository = new UserRepositoryStub(new List<User> { user });
            var commandRepository = new UserCommandRepositoryStub(new List<UserCommand> { command });
            var response = new DailyCommand(userRepository, commandRepository).Execute(2);

            Assert.False(response.Success);
            Assert.AreEqual("Your daily will be ready in 3 hours and 11 minutes!", response.Message);
        }

        [Test]
        public void SuccessfullDailyWithMissingUserCommand()
        {
            var user = new User { Id = 2 };
            var userRepository = new UserRepositoryStub(new List<User> { user });
            var commandRepository = new UserCommandRepositoryStub(new List<UserCommand>());
            var response = new DailyCommand(userRepository, commandRepository).Execute(2);
            var command = commandRepository.Find(2, 1);

            Assert.Less(command.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(command.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.True(response.Success);
            Assert.AreEqual($"You have redeemed your daily reward of {user.Niblets} Niblets!", response.Message);
            Assert.Greater(user.Niblets, 0);
        }

        [Test]
        public void SuccessfullDailyWithExistingUserCommand()
        {
            var user = new User { Id = 3 };
            var command = new UserCommand
            {
                CommandId = 1,
                UserId = 3,
                ExecutedAt = DateTime.Now.AddHours(-44)
            };

            var userRepository = new UserRepositoryStub(new List<User> { user });
            var commandRepository = new UserCommandRepositoryStub(new List<UserCommand> { command });
            var response = new DailyCommand(userRepository, commandRepository).Execute(3);
            var updatedCommand = commandRepository.Find(3, 1);

            Assert.Less(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.True(response.Success);
            Assert.AreEqual($"You have redeemed your daily reward of {user.Niblets} Niblets!", response.Message);
            Assert.Greater(user.Niblets, 0);
        }

        [Test]
        public void SuccessfullDailyWhenUserAlreadyHasNiblets()
        {
            var oldNiblets = 15;
            var user = new User { Id = 3, Niblets = oldNiblets };
            var command = new UserCommand
            {
                CommandId = 1,
                UserId = 3,
                ExecutedAt = DateTime.Now.AddHours(-44)
            };

            var userRepository = new UserRepositoryStub(new List<User> { user });
            var commandRepository = new UserCommandRepositoryStub(new List<UserCommand> { command });
            var response = new DailyCommand(userRepository, commandRepository).Execute(3);
            var updatedCommand = commandRepository.Find(3, 1);

            Assert.Less(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.True(response.Success);
            Assert.AreEqual($"You have redeemed your daily reward of {user.Niblets} Niblets!", response.Message);
            Assert.Greater(user.Niblets, oldNiblets);
        }
    }
}
