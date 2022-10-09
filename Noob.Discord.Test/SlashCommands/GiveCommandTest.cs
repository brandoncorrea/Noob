using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class GiveCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [Test]
    public async Task GiveNothing()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(5));
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
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [Test]
    public async Task GiveNegativeAmount()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(5));

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
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [Test]
    public async Task GiveOneNiblet()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(100));
        Noobs.UserRepository.Save(Noobs.Ted.SetNiblets(3));

        var interaction = new InteractionStub(
            Noobs.BillDiscord,
            new List<(string, object)>
            {
                ("recipient", Noobs.TedDiscord),
                ("amount", 1L)
            }
        );

        await new GiveCommand(Noobs.UserRepository).Give(interaction);
        Assert.AreEqual("Bill gave Ted 1 Niblet!", interaction.RespondAsyncParams.Text);
        Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(99, Noobs.Bill.Niblets);
        Assert.AreEqual(4, Noobs.Ted.Niblets);
    }

    [Test]
    public async Task NotEnoughNiblets()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(5));

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
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [Test]
    public async Task GiveNibletsToNonexistentUser()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(5).SetBrowniePoints(50));
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
        Assert.AreEqual("Bill gave Ted 5 Niblets, earning 1 Brownie Point :)", interaction.RespondAsyncParams.Text);
        Assert.AreEqual(51, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(0, Noobs.Bill.Niblets);
        Assert.AreEqual(5, Noobs.Ted.Niblets);
    }

    [Test]
    public async Task GiveFiftyNiblets()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(75).SetBrowniePoints(50));

        var interaction = new InteractionStub(
            Noobs.BillDiscord,
            new List<(string, object)>
            {
                ("recipient", Noobs.TedDiscord),
                ("amount", 50L)
            }
        );

        await new GiveCommand(Noobs.UserRepository).Give(interaction);
        Assert.AreEqual("Bill gave Ted 50 Niblets, earning 10 Brownie Points :)", interaction.RespondAsyncParams.Text);
        Assert.AreEqual(60, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(25, Noobs.Bill.Niblets);
        Assert.AreEqual(50, Noobs.Ted.Niblets);
    }

    [Test]
    public async Task GiveSelfNiblets()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(75).SetBrowniePoints(50));

        var interaction = new InteractionStub(
            Noobs.BillDiscord,
            new List<(string, object)>
            {
                ("recipient", Noobs.BillDiscord),
                ("amount", 10L)
            }
        );

        await new GiveCommand(Noobs.UserRepository).Give(interaction);
        Assert.AreEqual("Bill gave themself 10 Niblets!", interaction.RespondAsyncParams.Text);
        Assert.AreEqual(50, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(75, Noobs.Bill.Niblets);
    }

    [Test]
    public async Task GiveSelfOneNiblet()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(75).SetBrowniePoints(50));

        var interaction = new InteractionStub(
            Noobs.BillDiscord,
            new List<(string, object)>
            {
                ("recipient", Noobs.BillDiscord),
                ("amount", 1L)
            }
        );

        await new GiveCommand(Noobs.UserRepository).Give(interaction);
        Assert.AreEqual("Bill gave themself 1 Niblet!", interaction.RespondAsyncParams.Text);
        Assert.AreEqual(50, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(75, Noobs.Bill.Niblets);
    }
}
