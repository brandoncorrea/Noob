using Discord;
using Noob.Core.Models;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;

namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class ShopCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [TestCase]
    public async Task NoItemsAvailable()
    {
        Noobs.ItemRepository.Delete(Noobs.Stick);
        Noobs.ItemRepository.Delete(Noobs.Shield);
        Noobs.ItemRepository.Delete(Noobs.Crowbar);
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository);
        await command.HandleAsync(interaction);
        Assert.AreEqual("There are no items available to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsNull(interaction.RespondAsyncParams.Components);
    }

    [TestCase]
    public async Task OneItemAvailable()
    {
        Noobs.ItemRepository.Delete(Noobs.Shield);
        Noobs.ItemRepository.Delete(Noobs.Crowbar);
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.First();

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        Assert.AreEqual(1, menu.Options.Count);
        IEnumerable<SelectMenuOption> options = menu.Options;
        Assert.AreEqual("Stick", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual("A wooden stick. | 10 Niblets | Level 1", stickOption.Description);
    }

    [TestCase]
    public async Task TwoItemsAvailable()
    {
        Noobs.ItemRepository.Delete(Noobs.Crowbar);
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");
        var shieldOption = menu.Options.FirstOrDefault(item => item.Value == "2");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        Assert.AreEqual(2, menu.Options.Count);
        IEnumerable<SelectMenuOption> options = menu.Options;
        Assert.AreEqual("Stick", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual("A wooden stick. | 10 Niblets | Level 1", stickOption.Description);
        Assert.AreEqual("Shield", shieldOption.Label);
        Assert.AreEqual("2", shieldOption.Value);
        Assert.AreEqual("Blocks some stuff. | 50 Niblets | Level 2", shieldOption.Description);
    }

    [TestCase]
    public async Task ItemWithoutDescription()
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetDescription(null));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        IEnumerable<SelectMenuOption> options = menu.Options;
        Assert.AreEqual("Stick", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual("10 Niblets | Level 1", stickOption.Description);
    }

    [TestCase]
    public async Task ItemWithEmptyDescription()
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetDescription(""));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        IEnumerable<SelectMenuOption> options = menu.Options;
        Assert.AreEqual("Stick", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual("10 Niblets | Level 1", stickOption.Description);
    }

    [TestCase]
    public async Task ItemWithBlankDescription()
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetDescription("\r\n\t "));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        IEnumerable<SelectMenuOption> options = menu.Options;
        Assert.AreEqual("Stick", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual("10 Niblets | Level 1", stickOption.Description);
    }

    [TestCase]
    public async Task ItemCostIsOneNiblet()
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetPrice(1));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        IEnumerable<SelectMenuOption> options = menu.Options;
        Assert.AreEqual("Stick", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual("A wooden stick. | 1 Niblet | Level 1", stickOption.Description);
    }
}
