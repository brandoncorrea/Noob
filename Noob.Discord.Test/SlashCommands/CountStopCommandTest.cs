using Discord;
using Discord.WebSocket;
using Noob.Core.Models;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class CountStopCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [TestCase]
    public async Task CannotCountStopWithoutPermissions()
    {
        Noobs.SocketClient.CurrentUserId = Noobs.BillDiscord.Id;
        Noobs.UserPermissions[Noobs.BillDiscord.Id] = 0;
        var interaction = await CountStop(Noobs.BillDiscord, 1);
        Assert.AreEqual("You do not have permission to do this.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task CommandIsMisingGuildId()
    {
        Noobs.SocketClient.CurrentUserId = Noobs.BillDiscord.Id;
        var interaction = await CountStop(Noobs.BillDiscord, null);
        Assert.AreEqual("You do not have permission to do this.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task MissingGuildCountRecord()
    {
        Noobs.SocketClient.CurrentUserId = Noobs.BillDiscord.Id;
        var interaction = await CountStop(Noobs.BillDiscord, 1);
        Assert.AreEqual("You are no longer counting.", interaction.RespondAsyncParams.Text);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsNull(Noobs.GuildCountRepository.Find(1));
    }

    [TestCase]
    public async Task UpdatesExistingGuildCountRecord()
    {
        Noobs.GuildCountRepository.Save(new GuildCount
        {
            GuildId = 123,
            ChannelId = 456,
            Current = 5,
            LastUserId = 999,
            Record = 10000
        });

        Noobs.SocketClient.CurrentUserId = Noobs.BillDiscord.Id;
        var interaction = await CountStop(Noobs.BillDiscord, 123);
        Assert.AreEqual("You are no longer counting.", interaction.RespondAsyncParams.Text);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);

        var guildCount = Noobs.GuildCountRepository.Find(123);
        Assert.AreEqual(123, guildCount.GuildId);
        Assert.AreEqual(0, guildCount.ChannelId);
        Assert.AreEqual(5, guildCount.Current);
        Assert.AreEqual(10000, guildCount.Record);
        Assert.AreEqual(999, guildCount.LastUserId);
    }

    private async Task<InteractionStub> CountStop(IUser user, ulong? guildId)
    {
        var interaction = new InteractionStub(user)
        {
            GuildId = guildId,
        };

        await new CountStopCommand(Noobs.SocketClient, Noobs.GuildCountRepository).HandleAsync(interaction);
        return interaction;
    }
}

