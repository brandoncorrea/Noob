using Discord;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class StatsCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [TestCase]
    public async Task UserDoesNotExistAndNoAvatar()
    {
        Noobs.UserRepository.Delete(Noobs.Bill);
        Noobs.BillDiscord.AvatarUrl = null;
        var interaction = new InteractionStub(Noobs.BillDiscord);
        await new StatsCommand(Noobs.UserRepository).HandleAsync(interaction);
        var embed = interaction.RespondAsyncParams.Embed;
        Assert.AreEqual("Bill", embed.Author.Value.Name);
        Assert.AreEqual("http://localhost/defaultAvatar.jpg/", embed.Author.Value.IconUrl);
        Assert.AreEqual("Stats", embed.Title);
        Assert.AreEqual("Niblets: 0\nBrownie Points: 0\nLevel: 1\nExperience: 0", embed.Description);
        Assert.AreEqual(Color.Green, embed.Color);
    }

    [TestCase]
    public async Task UserDoesNotExistButHasAnAvatar()
    {
        Noobs.UserRepository.Delete(Noobs.Bill);
        var interaction = new InteractionStub(Noobs.BillDiscord);
        await new StatsCommand(Noobs.UserRepository).HandleAsync(interaction);
        var embed = interaction.RespondAsyncParams.Embed;
        Assert.AreEqual("Bill", embed.Author.Value.Name);
        Assert.AreEqual("http://localhost/billy_128.Auto/", embed.Author.Value.IconUrl);
        Assert.AreEqual("Stats", embed.Title);
        Assert.AreEqual("Niblets: 0\nBrownie Points: 0\nLevel: 1\nExperience: 0", embed.Description);
        Assert.AreEqual(Color.Green, embed.Color);
    }

    [TestCase]
    public async Task UserExistsWithoutStats()
    {
        var interaction = new InteractionStub(Noobs.BillDiscord);
        await new StatsCommand(Noobs.UserRepository).HandleAsync(interaction);
        var embed = interaction.RespondAsyncParams.Embed;
        Assert.AreEqual("Bill", embed.Author.Value.Name);
        Assert.AreEqual("http://localhost/billy_128.Auto/", embed.Author.Value.IconUrl);
        Assert.AreEqual("Stats", embed.Title);
        Assert.AreEqual("Niblets: 0\nBrownie Points: 0\nLevel: 1\nExperience: 0", embed.Description);
        Assert.AreEqual(Color.Green, embed.Color);
    }

    [TestCase]
    public async Task UserExistsWithStats()
    {
        Noobs.UserRepository.Save(Noobs.Ted.SetBrowniePoints(4).SetExperience(155).SetNiblets(32));
        var interaction = new InteractionStub(Noobs.TedDiscord);
        await new StatsCommand(Noobs.UserRepository).HandleAsync(interaction);
        var embed = interaction.RespondAsyncParams.Embed;
        Assert.AreEqual("Ted", embed.Author.Value.Name);
        Assert.AreEqual("http://localhost/teddy_128.Auto/", embed.Author.Value.IconUrl);
        Assert.AreEqual("Stats", embed.Title);
        Assert.AreEqual("Niblets: 32\nBrownie Points: 4\nLevel: 2\nExperience: 155", embed.Description);
        Assert.AreEqual(Color.Green, embed.Color);
    }
}

