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
        foreach (var item in Noobs.Db.Items)
            Noobs.ItemRepository.Delete(item);
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository);
        await command.HandleAsync(interaction);
        Assert.AreEqual("There are no items available to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsNull(interaction.RespondAsyncParams.Components);
    }

    [TestCase]
    public async Task OneItemAvailable()
    {
        foreach (var item in Noobs.ItemRepository.FindAll().Where(i => i.Id != Noobs.Stick.Id))
            Noobs.ItemRepository.Delete(item);
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.First();
        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        Assert.AreEqual(1, menu.Options.Count);
        Assert.AreEqual("Stick – Main Hand – 10 Niblets ⚔️ 1 ⭐️ 1", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual("A wooden stick.", stickOption.Description);
    }

    [TestCase]
    public async Task TwoItemsAvailable()
    {
        var deletions = Noobs.ItemRepository
            .FindAll()
            .Where(i => i.Id != Noobs.Stick.Id && i.Id != Noobs.Shield.Id);
        foreach (var item in deletions)
            Noobs.ItemRepository.Delete(item);
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");
        var shieldOption = menu.Options.FirstOrDefault(item => item.Value == "2");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        Assert.AreEqual(2, menu.Options.Count);
        Assert.AreEqual("Stick – Main Hand – 10 Niblets ⚔️ 1 ⭐️ 1", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual("A wooden stick.", stickOption.Description);
        Assert.AreEqual("Shield – Off-Hand – 50 Niblets 🛡 1 ⭐️ 2", shieldOption.Label);
        Assert.AreEqual("2", shieldOption.Value);
        Assert.AreEqual("Blocks some stuff.", shieldOption.Description);
    }

    [TestCase]
    public async Task OneItemWithTwoAttributes()
    {
        foreach (var item in Noobs.ItemRepository.FindAll().Where(i => i.Id != Noobs.Hat.Id))
            Noobs.ItemRepository.Delete(item);
        Noobs.ItemRepository.Save(Noobs.Hat.SetSneak(1).SetPerception(2));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var hatOption = menu.Options.First();
        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        Assert.AreEqual(1, menu.Options.Count);
        Assert.AreEqual("Hat – Head – 100 Niblets 🥷 1 👁 2 ⭐️ 1", hatOption.Label);
        Assert.AreEqual("6", hatOption.Value);
    }

    [TestCase]
    public async Task ItemWithoutDescription()
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetDescription(null));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        Assert.AreEqual("Stick – Main Hand – 10 Niblets ⚔️ 1 ⭐️ 1", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.IsNull(stickOption.Description);
    }

    [TestCase]
    public async Task ItemWithEmptyDescription()
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetDescription(""));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        Assert.AreEqual("Stick – Main Hand – 10 Niblets ⚔️ 1 ⭐️ 1", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.IsNull(stickOption.Description);
    }

    [TestCase]
    public async Task ItemWithBlankDescription()
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetDescription("\r\n\t "));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        Assert.AreEqual("Stick – Main Hand – 10 Niblets ⚔️ 1 ⭐️ 1", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.IsNull(stickOption.Description);
    }

    [TestCase]
    public async Task ItemCostIsOneNiblet()
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetPrice(1));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        var command = new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        var stickOption = menu.Options.FirstOrDefault(item => item.Value == "1");

        Assert.AreEqual("Choose an item to purchase.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual("Select an option", menu.Placeholder);
        Assert.AreEqual("shop-menu", menu.CustomId);
        Assert.AreEqual("Stick – Main Hand – 1 Niblet ⚔️ 1 ⭐️ 1", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual("A wooden stick.", stickOption.Description);
    }

    [TestCase(1, "Main Hand")]
    [TestCase(2, "Off-Hand")]
    [TestCase(3, "Head")]
    [TestCase(4, "Torso")]
    [TestCase(5, "Legs")]
    [TestCase(6, "Hands")]
    [TestCase(7, "Feet")]
    [TestCase(8, "Back")]
    public async Task DisplaysHeadLabel(int slotId, string label)
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetSlotId(slotId));
        var interaction = new InteractionStub(Noobs.BillDiscord);
        await new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository).HandleAsync(interaction);

        var stickOption = ((SelectMenuComponent)
            interaction
            .RespondAsyncParams
            .Components
            .Components
            .First()
            .Components
            .First())
            .Options
            .FirstOrDefault(item => item.Value == "1");

        Assert.AreEqual($"Stick – {label} – 10 Niblets ⚔️ 1 ⭐️ 1", stickOption.Label);
        Assert.AreEqual("1", stickOption.Value);
        Assert.AreEqual($"A wooden stick.", stickOption.Description);
    }

    [TestCase]
    public async Task ExcludesItemsAlreadyOwned()
    {
        var interaction = new InteractionStub(Noobs.TedDiscord);
        var command = new ShopCommand(Noobs.ItemRepository, Noobs.UserItemRepository);
        await command.HandleAsync(interaction);

        SelectMenuComponent menu = (SelectMenuComponent)interaction
            .RespondAsyncParams
            .Components
            .Components
            .First()
            .Components
            .First();

        var stickOption = menu
            .Options
            .FirstOrDefault(item => item.Value == Noobs.TedStick.ItemId.ToString());
        Assert.IsNull(stickOption);
    }
}
