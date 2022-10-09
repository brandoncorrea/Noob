using Discord;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class RecurrentCommandTest
{
    [TestFixture]
    public class DailyCommandTest
    {
        [SetUp]
        public void SetUp() => Noobs.Initialize();

        [Test]
        public async Task OneHourUntilNextExecution()
        {
            Noobs.UserCommandRepository.Save(Noobs.BillDaily.SetExecutedAt(DateTime.Now.AddHours(-23).AddMinutes(1)));
            var interaction = await ExecuteDaily(Noobs.BillDiscord);
            Assert.AreEqual("Your daily reward will be ready in 1 hour!", interaction.RespondAsyncParams.Text);
            Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        }

        [Test]
        public async Task ThreeHoursTwelveMinutesUntilNextExecution()
        {
            Noobs.UserCommandRepository.Save(Noobs.BillDaily.SetExecutedAt(DateTime.Now.AddHours(3).AddMinutes(12).AddDays(-1)));
            var interaction = await ExecuteDaily(Noobs.BillDiscord);
            Assert.AreEqual("Your daily reward will be ready in 3 hours and 11 minutes!", interaction.RespondAsyncParams.Text);
            Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        }

        [Test]
        public async Task SuccessfullDailyWithMissingUser()
        {
            Noobs.UserRepository.Delete(Noobs.Bill);
            var interaction = await ExecuteDaily(Noobs.BillDiscord);
            Assert.Less(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.AreEqual($"Bill received {Noobs.Bill.Niblets} Niblets!", interaction.RespondAsyncParams.Text);
            Assert.Greater(Noobs.Bill.Niblets, 0);
        }

        [Test]
        public async Task SuccessfullDailyWithMissingUserCommand()
        {
            Noobs.UserCommandRepository.Delete(Noobs.BillDaily);
            var interaction = await ExecuteDaily(Noobs.BillDiscord);
            Assert.Less(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.AreEqual($"Bill received {Noobs.Bill.Niblets} Niblets!", interaction.RespondAsyncParams.Text);
            Assert.Greater(Noobs.Bill.Niblets, 0);
        }

        [Test]
        public async Task SuccessfullDailyWithExistingUserCommand()
        {
            Noobs.UserCommandRepository.Save(Noobs.BillDaily.SetExecutedAt(DateTime.Now.AddHours(-44)));
            var interaction = await ExecuteDaily(Noobs.BillDiscord);
            Assert.Less(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.AreEqual($"Bill received {Noobs.Bill.Niblets} Niblets!", interaction.RespondAsyncParams.Text);
            Assert.Greater(Noobs.Bill.Niblets, 0);
        }

        [Test]
        public async Task SuccessfullDailyWhenUserAlreadyHasNiblets()
        {
            Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(15));
            Noobs.UserCommandRepository.Save(Noobs.BillDaily.SetExecutedAt(DateTime.Now.AddHours(-44)));
            var interaction = await ExecuteDaily(Noobs.BillDiscord);
            Assert.Less(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(Noobs.BillDaily.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.AreEqual($"Bill received {Noobs.Bill.Niblets - 15} Niblets!", interaction.RespondAsyncParams.Text);
            Assert.Greater(Noobs.Bill.Niblets, 15);
        }

        private async Task<InteractionStub> ExecuteDaily(IUser user)
        {
            var interaction = new InteractionStub(user);
            await new RecurrentCommand(
                Noobs.UserRepository,
                Noobs.UserCommandRepository)
                .Daily(interaction);
            return interaction;
        }
    }

    [TestFixture]
    public class WeeklyCommandTest
    {
        [SetUp]
        public void SetUp() => Noobs.Initialize();

        [Test]
        public async Task OneDayUntilNextExecution()
        {
            Noobs.UserCommandRepository.Save(Noobs.TedWeekly.SetExecutedAt(DateTime.Now.AddDays(-6).AddMinutes(1)));
            var interaction = await ExecuteWeekly(Noobs.TedDiscord);
            Assert.AreEqual("Your weekly reward will be ready in 1 day!", interaction.RespondAsyncParams.Text);
            Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        }

        [Test]
        public async Task ThreeDaysTwelveHoursFourMinutesUntilNextExecution()
        {
            Noobs.UserCommandRepository.Save(Noobs.TedWeekly.SetExecutedAt(DateTime.Now.AddDays(3).AddHours(12).AddMinutes(4).AddDays(-7)));
            var interaction = await ExecuteWeekly(Noobs.TedDiscord);
            Assert.AreEqual("Your weekly reward will be ready in 3 days, 12 hours, and 3 minutes!", interaction.RespondAsyncParams.Text);
            Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        }

        [Test]
        public async Task SuccessfullWeeklyWithMissingUser()
        {
            Noobs.UserRepository.Delete(Noobs.Ted);
            var interaction = await ExecuteWeekly(Noobs.TedDiscord);
            Assert.AreEqual($"Ted received {Noobs.Ted.Niblets} Niblets!", interaction.RespondAsyncParams.Text);
            Assert.Less(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.GreaterOrEqual(Noobs.Ted.Niblets, 50);
        }

        [Test]
        public async Task SuccessfullWeeklyWithMissingUserCommand()
        {
            Noobs.UserCommandRepository.Delete(Noobs.TedWeekly);
            var interaction = await ExecuteWeekly(Noobs.TedDiscord);
            Assert.Less(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.AreEqual($"Ted received {Noobs.Ted.Niblets} Niblets!", interaction.RespondAsyncParams.Text);
            Assert.GreaterOrEqual(Noobs.Ted.Niblets, 50);
        }

        [Test]
        public async Task SuccessfullDailyWithExistingUserCommand()
        {
            Noobs.UserCommandRepository.Save(Noobs.TedWeekly.SetExecutedAt(DateTime.Now.AddDays(-8)));
            var interaction = await ExecuteWeekly(Noobs.TedDiscord);
            Assert.Less(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.AreEqual($"Ted received {Noobs.Ted.Niblets} Niblets!", interaction.RespondAsyncParams.Text);
            Assert.GreaterOrEqual(Noobs.Ted.Niblets, 50);
        }

        [Test]
        public async Task SuccessfullDailyWhenUserAlreadyHasNiblets()
        {
            Noobs.UserRepository.Save(Noobs.Ted.SetNiblets(17));
            Noobs.UserCommandRepository.Save(Noobs.TedWeekly.SetExecutedAt(DateTime.Now.AddDays(-8)));
            var interaction = await ExecuteWeekly(Noobs.TedDiscord);
            Assert.Less(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(1));
            Assert.Greater(Noobs.TedWeekly.ExecutedAt, DateTime.Now.AddSeconds(-1));
            Assert.AreEqual($"Ted received {Noobs.Ted.Niblets - 17} Niblets!", interaction.RespondAsyncParams.Text);
            Assert.GreaterOrEqual(Noobs.Ted.Niblets, 67);
        }

        private async Task<InteractionStub> ExecuteWeekly(IUser user)
        {
            var interaction = new InteractionStub(user);
            await new RecurrentCommand(
                Noobs.UserRepository,
                Noobs.UserCommandRepository)
                .Weekly(interaction);
            return interaction;
        }
    }
}
