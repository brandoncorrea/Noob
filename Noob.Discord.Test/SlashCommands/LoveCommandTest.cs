using Discord;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Noob.Core.Models;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class LoveCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [Test]
    public async Task UserLovesThemself()
    {
        var interaction = await Love(Noobs.BillDiscord, Noobs.BillDiscord);
        var messages = LoveCommand.LoveSelfMessages.Select(s => string.Format(s, Noobs.BillDiscord.Username)).ToArray();
        Assert.Contains(interaction.RespondAsyncParams.Text, messages);
    }

    [Test]
    public async Task UserLovesAnotherUser()
    {
        var interaction = await Love(Noobs.BillDiscord, Noobs.TedDiscord);
        var messages = LoveCommand.LoveOthersMessages.Select(s => string.Format(s, Noobs.BillDiscord.Username, Noobs.TedDiscord.Username)).ToArray();
        Assert.Contains(interaction.RespondAsyncParams.Text, messages);
    }

    private async Task<InteractionStub> Love(IUser initiator, IUser user)
    {
        var interaction = new InteractionStub(
            initiator,
            new (string, object)[] { ("user", user) }
        );

        await new LoveCommand().HandleAsync(interaction);
        return interaction;
    }
}
