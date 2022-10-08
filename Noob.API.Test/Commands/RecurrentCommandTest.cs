
using Discord;
using Noob.API.Commands;
using Noob.API.Models;
using Noob.API.Test.Stub;

namespace Noob.API.Test.Commands
{
    [TestFixture]
    public class RecurrentCommandTest
    {
        [TestFixture]
        public class DailyCommandTest
        {
            [SetUp]
            public void SetUp() => Noobs.Initialize();

            [Test]
            public void OneHourUntilNextExecution()
            {
                var thing = Noobs.BillDaily;
                thing.ExecutedAt = DateTime.Now.AddHours(-23).AddMinutes(1);
                var response = ExecuteDaily(Noobs.BillDiscord);
                Assert.False(response.Success);
                Assert.AreEqual("Your daily reward will be ready in 1 hour!", response.Message);
            }

            [Test]
            public void ThreeHoursTwelveMinutesUntilNextExecution()
            {
                Noobs.BillDaily.ExecutedAt = DateTime.Now.AddHours(3).AddMinutes(12).AddDays(-1);
                var response = ExecuteDaily(Noobs.BillDiscord);
                Assert.False(response.Success);
                Assert.AreEqual("Your daily reward will be ready in 3 hours and 11 minutes!", response.Message);
            }

            [Test]
            public void SuccessfullDailyWithMissingUser()
            {
                Noobs.UserRepository.Delete(Noobs.Bill);
                Noobs.UserCommandRepository.Delete(Noobs.BillDaily);
                var response = ExecuteDaily(Noobs.BillDiscord);
                var billDaily = Noobs.UserCommandRepository.Find(Noobs.Bill.Id, 1);

                Assert.Less(billDaily.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(billDaily.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your daily reward of {Noobs.Bill.Niblets} Niblets!", response.Message);
                Assert.Greater(Noobs.Bill.Niblets, 0);
            }

            [Test]
            public void SuccessfullDailyWithMissingUserCommand()
            {
                Noobs.UserCommandRepository.Delete(Noobs.BillDaily);
                var response = ExecuteDaily(Noobs.BillDiscord);

                Assert.Less(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your daily reward of {Noobs.Bill.Niblets} Niblets!", response.Message);
                Assert.Greater(Noobs.Bill.Niblets, 0);
            }

            [Test]
            public void SuccessfullDailyWithExistingUserCommand()
            {
                Noobs.BillDaily.ExecutedAt = DateTime.Now.AddHours(-44);
                var response = ExecuteDaily(Noobs.BillDiscord);
                Assert.Less(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your daily reward of {Noobs.Bill.Niblets} Niblets!", response.Message);
                Assert.Greater(Noobs.Bill.Niblets, 0);
            }

            [Test]
            public void SuccessfullDailyWhenUserAlreadyHasNiblets()
            {
                Noobs.Bill.Niblets = 15;
                Noobs.BillDaily.ExecutedAt = DateTime.Now.AddHours(-44);
                var response = ExecuteDaily(Noobs.BillDiscord);
                Assert.Less(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your daily reward of {Noobs.Bill.Niblets - 15} Niblets!", response.Message);
                Assert.Greater(Noobs.Bill.Niblets, 15);
            }

            private CommandResponse ExecuteDaily(IUser user) =>
                new RecurrentCommand(
                    Noobs.UserRepository,
                    Noobs.UserCommandRepository)
                    .Daily(new InteractionStub(user));
        }

        [TestFixture]
        public class WeeklyCommandTest
        {
            [SetUp]
            public void SetUp() => Noobs.Initialize();

            [Test]
            public void OneDayUntilNextExecution()
            {
                Noobs.TedWeekly.ExecutedAt = DateTime.Now.AddDays(-6).AddMinutes(1);
                var response = ExecuteWeekly(Noobs.TedDiscord);
                Assert.False(response.Success);
                Assert.AreEqual("Your weekly reward will be ready in 1 day!", response.Message);
            }

            [Test]
            public void ThreeDaysTwelveHoursFourMinutesUntilNextExecution()
            {
                Noobs.TedWeekly.ExecutedAt = DateTime.Now.AddDays(3).AddHours(12).AddMinutes(4).AddDays(-7);
                var response = ExecuteWeekly(Noobs.TedDiscord);
                Assert.False(response.Success);
                Assert.AreEqual("Your weekly reward will be ready in 3 days, 12 hours, and 3 minutes!", response.Message);
            }

            [Test]
            public void SuccessfullWeeklyWithMissingUser()
            {
                Noobs.UserRepository.Delete(Noobs.Ted);
                Noobs.UserCommandRepository.Delete(Noobs.TedWeekly);
                var response = ExecuteWeekly(Noobs.TedDiscord);
                Assert.Less(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your weekly reward of {Noobs.Ted.Niblets} Niblets!", response.Message);
                Assert.GreaterOrEqual(Noobs.Ted.Niblets, 50);
            }

            [Test]
            public void SuccessfullDailyWithMissingUserCommand()
            {
                Noobs.UserCommandRepository.Delete(Noobs.TedWeekly);
                var response = ExecuteWeekly(Noobs.TedDiscord);
                Assert.Less(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your weekly reward of {Noobs.Ted.Niblets} Niblets!", response.Message);
                Assert.GreaterOrEqual(Noobs.Ted.Niblets, 50);
            }

            [Test]
            public void SuccessfullDailyWithExistingUserCommand()
            {
                Noobs.TedWeekly.ExecutedAt = DateTime.Now.AddDays(-8);
                var response = ExecuteWeekly(Noobs.TedDiscord);
                Assert.Less(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your weekly reward of {Noobs.Ted.Niblets} Niblets!", response.Message);
                Assert.GreaterOrEqual(Noobs.Ted.Niblets, 50);
            }

            [Test]
            public void SuccessfullDailyWhenUserAlreadyHasNiblets()
            {
                Noobs.Ted.Niblets = 17;
                Noobs.TedWeekly.ExecutedAt = DateTime.Now.AddDays(-8);
                var response = ExecuteWeekly(Noobs.TedDiscord);
                Assert.Less(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(1));
                Assert.Greater(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(-1));
                Assert.True(response.Success);
                Assert.AreEqual($"You have redeemed your weekly reward of {Noobs.Ted.Niblets - 17} Niblets!", response.Message);
                Assert.GreaterOrEqual(Noobs.Ted.Niblets, 67);
            }

            private CommandResponse ExecuteWeekly(IUser user) =>
                new RecurrentCommand(
                    Noobs.UserRepository,
                    Noobs.UserCommandRepository)
                    .Weekly(new InteractionStub(user));
        }
    }
}
