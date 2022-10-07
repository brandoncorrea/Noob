
using Noob.API.Commands;
using Noob.API.Models;
using Noob.API.Test.Stub;

namespace Noob.API.Test.Commands
{
    public class RecurrentCommandTest
    {
        [TestFixture]
        public class DailyCommandTest
        {
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
                var response = new RecurrentCommand(userRepository, commandRepository).Daily(2);
                Assert.False(response.Success);
                Assert.AreEqual("Your daily reward will be ready in 1 hour!", response.Message);
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
                var response = new RecurrentCommand(userRepository, commandRepository).Daily(2);

                Assert.False(response.Success);
                Assert.AreEqual("Your daily reward will be ready in 3 hours and 11 minutes!", response.Message);
            }

            [Test]
            public void SuccessfullDailyWithMissingUser()
            {
                var userRepository = new UserRepositoryStub(new List<User> { });
                var commandRepository = new UserCommandRepositoryStub(new List<UserCommand>());
                var response = new RecurrentCommand(userRepository, commandRepository).Daily(2);
                var command = commandRepository.Find(2, 1);
                var newUser = userRepository.Find(2);

                Assert.Less(command.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(command.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your daily reward of {newUser.Niblets} Niblets!", response.Message);
                Assert.Greater(newUser.Niblets, 0);
            }

            [Test]
            public void SuccessfullDailyWithMissingUserCommand()
            {
                var user = new User { Id = 2 };
                var userRepository = new UserRepositoryStub(new List<User> { user });
                var commandRepository = new UserCommandRepositoryStub(new List<UserCommand>());
                var response = new RecurrentCommand(userRepository, commandRepository).Daily(2);
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
                var response = new RecurrentCommand(userRepository, commandRepository).Daily(3);
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
                var response = new RecurrentCommand(userRepository, commandRepository).Daily(3);
                var updatedCommand = commandRepository.Find(3, 1);

                Assert.Less(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your daily reward of {user.Niblets - oldNiblets} Niblets!", response.Message);
                Assert.Greater(user.Niblets, oldNiblets);
            }
        }

        [TestFixture]
        public class WeeklyCommandTest
        {
            [Test]
            public void OneDayUntilNextExecution()
            {
                var user = new User { Id = 2 };
                var command = new UserCommand
                {
                    CommandId = 2,
                    UserId = 2,
                    ExecutedAt = DateTime.Now.AddDays(-6).AddMinutes(1)
                };

                var userRepository = new UserRepositoryStub(new List<User> { user });
                var commandRepository = new UserCommandRepositoryStub(new List<UserCommand> { command });
                var response = new RecurrentCommand(userRepository, commandRepository).Weekly(2);
                Assert.False(response.Success);
                Assert.AreEqual("Your weekly reward will be ready in 1 day!", response.Message);
            }

            [Test]
            public void ThreeDaysTwelveHoursFourMinutesUntilNextExecution()
            {
                var user = new User { Id = 2 };
                var command = new UserCommand
                {
                    CommandId = 2,
                    UserId = 2,
                    ExecutedAt = DateTime.Now.AddDays(3).AddHours(12).AddMinutes(4).AddDays(-7)
                };

                var userRepository = new UserRepositoryStub(new List<User> { user });
                var commandRepository = new UserCommandRepositoryStub(new List<UserCommand> { command });
                var response = new RecurrentCommand(userRepository, commandRepository).Weekly(2);

                Assert.False(response.Success);
                Assert.AreEqual("Your weekly reward will be ready in 3 days, 12 hours, and 3 minutes!", response.Message);
            }

            [Test]
            public void SuccessfullWeeklyWithMissingUser()
            {
                var userRepository = new UserRepositoryStub(new List<User> { });
                var commandRepository = new UserCommandRepositoryStub(new List<UserCommand>());
                var response = new RecurrentCommand(userRepository, commandRepository).Weekly(2);
                var command = commandRepository.Find(2, 2);
                var newUser = userRepository.Find(2);

                Assert.Less(command.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(command.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your weekly reward of {newUser.Niblets} Niblets!", response.Message);
                Assert.GreaterOrEqual(newUser.Niblets, 100);
            }

            [Test]
            public void SuccessfullDailyWithMissingUserCommand()
            {
                var user = new User { Id = 2 };
                var userRepository = new UserRepositoryStub(new List<User> { user });
                var commandRepository = new UserCommandRepositoryStub(new List<UserCommand>());
                var response = new RecurrentCommand(userRepository, commandRepository).Weekly(2);
                var command = commandRepository.Find(2, 2);

                Assert.Less(command.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(command.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your weekly reward of {user.Niblets} Niblets!", response.Message);
                Assert.GreaterOrEqual(user.Niblets, 100);
            }

            [Test]
            public void SuccessfullDailyWithExistingUserCommand()
            {
                var user = new User { Id = 3 };
                var command = new UserCommand
                {
                    CommandId = 2,
                    UserId = 3,
                    ExecutedAt = DateTime.Now.AddDays(-8)
                };

                var userRepository = new UserRepositoryStub(new List<User> { user });
                var commandRepository = new UserCommandRepositoryStub(new List<UserCommand> { command });
                var response = new RecurrentCommand(userRepository, commandRepository).Weekly(3);
                var updatedCommand = commandRepository.Find(3, 2);

                Assert.Less(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your weekly reward of {user.Niblets} Niblets!", response.Message);
                Assert.GreaterOrEqual(user.Niblets, 100);
            }

            [Test]
            public void SuccessfullDailyWhenUserAlreadyHasNiblets()
            {
                var oldNiblets = 17;
                var user = new User { Id = 3, Niblets = oldNiblets };
                var command = new UserCommand
                {
                    CommandId = 2,
                    UserId = 3,
                    ExecutedAt = DateTime.Now.AddDays(-8)
                };

                var userRepository = new UserRepositoryStub(new List<User> { user });
                var commandRepository = new UserCommandRepositoryStub(new List<UserCommand> { command });
                var response = new RecurrentCommand(userRepository, commandRepository).Weekly(3);
                var updatedCommand = commandRepository.Find(3, 2);

                Assert.Less(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(updatedCommand.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your weekly reward of {user.Niblets - oldNiblets} Niblets!", response.Message);
                Assert.GreaterOrEqual(user.Niblets, oldNiblets + 100);
            }
        }
    }
}
