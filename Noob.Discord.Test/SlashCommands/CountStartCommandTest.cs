using Discord;
using Discord.WebSocket;
using Noob.Core.Models;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class CountStartCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [TestCase]
    public async Task CannotCountStartWithoutPermissions()
    {
        Noobs.SocketClient.CurrentUserId = Noobs.BillDiscord.Id;
        Noobs.UserPermissions[Noobs.BillDiscord.Id] = 0;
        var interaction = await CountStart(Noobs.BillDiscord, 1, 2);
        Assert.AreEqual("You do not have permission to do this.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task CommandIsMisingGuildId()
    {
        Noobs.SocketClient.CurrentUserId = Noobs.BillDiscord.Id;
        var interaction = await CountStart(Noobs.BillDiscord, null, 2);
        Assert.AreEqual("You do not have permission to do this.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task CommandIsMisingChannelId()
    {
        Noobs.SocketClient.CurrentUserId = Noobs.BillDiscord.Id;
        var interaction = await CountStart(Noobs.BillDiscord, 1, null);
        Assert.AreEqual("This is not a channel.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task CreatesNewGuildCountRecord()
    {
        Noobs.SocketClient.CurrentUserId = Noobs.BillDiscord.Id;
        var interaction = await CountStart(Noobs.BillDiscord, 1, 2);
        Assert.AreEqual("You are now counting on this channel.", interaction.RespondAsyncParams.Text);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);

        var guildCount = Noobs.GuildCountRepository.Find(1);
        Assert.AreEqual(1, guildCount.GuildId);
        Assert.AreEqual(2, guildCount.ChannelId);
        Assert.AreEqual(0, guildCount.Current);
        Assert.AreEqual(0, guildCount.Record);
        Assert.AreEqual(0, guildCount.LastUserId);
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
        var interaction = await CountStart(Noobs.BillDiscord, 123, 7);
        Assert.AreEqual("You are now counting on this channel.", interaction.RespondAsyncParams.Text);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);

        var guildCount = Noobs.GuildCountRepository.Find(123);
        Assert.AreEqual(123, guildCount.GuildId);
        Assert.AreEqual(7, guildCount.ChannelId);
        Assert.AreEqual(5, guildCount.Current);
        Assert.AreEqual(10000, guildCount.Record);
        Assert.AreEqual(999, guildCount.LastUserId);
    }

    private async Task<InteractionStub> CountStart(IUser user, ulong? guildId, ulong? channelId)
    {
        var interaction = new InteractionStub(user)
        {
            GuildId = guildId,
            ChannelId = channelId
        };

        await new CountStartCommand(Noobs.SocketClient, Noobs.GuildCountRepository).HandleAsync(interaction);
        return interaction;
    }
}

