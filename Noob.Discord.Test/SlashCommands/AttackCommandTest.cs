using Discord;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class AttackCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [TestCase]
    public async Task UserAttacksSelf()
    {
        var interaction = await Attack(Noobs.BillDiscord, Noobs.BillDiscord);
        Assert.AreEqual("Self harm is NOT okay 😭❤️", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task TwentyLevelsBelowWithNoExperience()
    {
        Noobs.UserRepository.Save(Noobs.Ted.SetExperience(2000));
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual("Bill tried attacking Ted and got PWND!", interaction.RespondAsyncParams.Text);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(0, Noobs.Bill.Experience);
        Assert.AreEqual(2000, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task TwentyLevelsBelow()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetExperience(50));
        Noobs.UserRepository.Save(Noobs.Ted.SetExperience(2000));
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual("Bill tried attacking Ted and got PWND!", interaction.RespondAsyncParams.Text);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(50, Noobs.Bill.Experience);
        Assert.AreEqual(2000, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task TwentyLevelsAbove()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetExperience(2000));
        Noobs.UserRepository.Save(Noobs.Ted.SetExperience(50));
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual("Bill just beat the living daylights out of Ted!", interaction.RespondAsyncParams.Text);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(2000, Noobs.Bill.Experience);
        Assert.AreEqual(50, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task FairFight()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetExperience(50));
        Noobs.UserRepository.Save(Noobs.Ted.SetExperience(50));
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreNotEqual(50, Noobs.Ted.Experience);
        Assert.AreNotEqual(50, Noobs.Bill.Experience);
    }

    [TestCase]
    public async Task TargetDoesNotExist()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetExperience(2100));
        Noobs.UserRepository.Delete(Noobs.Ted);
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual($"Bill just beat the living daylights out of Ted!", interaction.RespondAsyncParams.Text);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(2100, Noobs.Bill.Experience);
        Assert.AreEqual(0, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task UserDoesNotExist()
    {
        Noobs.UserRepository.Delete(Noobs.Bill);
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsNotNull(Noobs.Bill.Experience);
    }

    private async Task<InteractionStub> Attack(IUser user, IUser victim)
    {
        var interaction = new InteractionStub(
            user,
            new (string, object)[] { ("victim", victim) }
        );

        await new AttackCommand(Noobs.UserRepository).HandleAsync(interaction);
        return interaction;
    }
}
