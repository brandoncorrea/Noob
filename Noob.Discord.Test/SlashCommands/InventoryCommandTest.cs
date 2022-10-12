using Discord;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class InventoryCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [TestCase]
    public async Task UserDoesNotExist()
    {
        Noobs.UserItemRepository.Dissociate(Noobs.Bill, Noobs.Slippers);
        Noobs.UserItemRepository.Dissociate(Noobs.Bill, Noobs.Mittens);
        Noobs.UserRepository.Delete(Noobs.Bill);
        var interaction = new InteractionStub(Noobs.BillDiscord);
        await HandleInventory(interaction);
        Assert.AreEqual("You do not have any items in your inventory.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task UserDoesHaveItems()
    {
        Noobs.UserItemRepository.Dissociate(Noobs.Ted, Noobs.Stick);
        var interaction = new InteractionStub(Noobs.TedDiscord);
        await HandleInventory(interaction);
        Assert.AreEqual("You do not have any items in your inventory.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task UserHasOneItemWhichIsEquipped()
    {
        var interaction = new InteractionStub(Noobs.TedDiscord);
        await HandleInventory(interaction);

        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);

        var embed = interaction.RespondAsyncParams.Embed;
        Assert.AreEqual("Inventory", embed.Title);
        Assert.AreEqual("Ted", embed.Author.Value.Name);
        Assert.AreEqual("Stick ⚔️ 1 ⭐️ 1", embed.Description);

        ButtonComponent button = (ButtonComponent)interaction.RespondAsyncParams.Components.Components.First().Components.First();
        Assert.AreEqual($"inventory-item-button:{Noobs.Stick.Id}", button.CustomId);
        Assert.AreEqual("Stick", button.Label);
        Assert.IsFalse(button.IsDisabled);
        Assert.AreEqual(ButtonStyle.Success, button.Style);
    }

    [TestCase]
    public async Task UserHasOneEquippedAndUnequippedItem()
    {
        Noobs.EquippedItemRepository.Unequip(Noobs.Bill, Noobs.Mittens);
        var interaction = new InteractionStub(Noobs.BillDiscord);
        await HandleInventory(interaction);

        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);

        var embed = interaction.RespondAsyncParams.Embed;
        Assert.AreEqual("Inventory", embed.Title);
        Assert.AreEqual("Bill", embed.Author.Value.Name);
        Assert.AreEqual("Mittens 🥷 1 ⭐️ 1\nSlippers 🥷 1 ⭐️ 1", embed.Description);

        var buttons = interaction.RespondAsyncParams.Components.Components.First().Components;
        ButtonComponent mittens = (ButtonComponent)buttons.First(b => b.CustomId == $"inventory-item-button:{Noobs.Mittens.Id}");
        ButtonComponent slippers = (ButtonComponent)buttons.First(b => b.CustomId == $"inventory-item-button:{Noobs.Slippers.Id}");

        Assert.AreEqual("Mittens", mittens.Label);
        Assert.IsFalse(mittens.IsDisabled);
        Assert.AreEqual(ButtonStyle.Primary, mittens.Style);
        Assert.AreEqual("Slippers", slippers.Label);
        Assert.IsFalse(slippers.IsDisabled);
        Assert.AreEqual(ButtonStyle.Success, slippers.Style);
    }

    [TestCase]
    public async Task UserHasItemsAboveCurrentLevel()
    {
        Noobs.UserItemRepository.Create(Noobs.Ted, Noobs.Crowbar);
        Noobs.UserItemRepository.Create(Noobs.Ted, Noobs.Hat);
        var interaction = new InteractionStub(Noobs.TedDiscord);
        await HandleInventory(interaction);

        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);

        var embed = interaction.RespondAsyncParams.Embed;
        Assert.AreEqual("Inventory", embed.Title);
        Assert.AreEqual("Ted", embed.Author.Value.Name);
        Assert.AreEqual("Stick ⚔️ 1 ⭐️ 1\nCrowbar ⚔️ 3 ⭐️ 3\nHat 👁 1 ⭐️ 1", embed.Description);

        var buttons = interaction.RespondAsyncParams.Components.Components.First().Components;
        ButtonComponent stick = (ButtonComponent)buttons.First(b => b.CustomId == $"inventory-item-button:{Noobs.Stick.Id}");
        ButtonComponent crowbar = (ButtonComponent)buttons.First(b => b.CustomId == $"inventory-item-button:{Noobs.Crowbar.Id}");
        ButtonComponent hat = (ButtonComponent)buttons.First(b => b.CustomId == $"inventory-item-button:{Noobs.Hat.Id}");

        Assert.AreEqual("Stick", stick.Label);
        Assert.IsFalse(stick.IsDisabled);
        Assert.AreEqual(ButtonStyle.Success, stick.Style);
        Assert.AreEqual("Crowbar", crowbar.Label);
        Assert.IsTrue(crowbar.IsDisabled);
        Assert.AreEqual(ButtonStyle.Secondary, crowbar.Style);
        Assert.AreEqual("Hat", hat.Label);
        Assert.IsFalse(hat.IsDisabled);
        Assert.AreEqual(ButtonStyle.Primary, hat.Style);
    }


    private async Task HandleInventory(ISlashCommandInteraction interaction) =>
        await new InventoryCommand(
            Noobs.UserRepository,
            Noobs.ItemRepository,
            Noobs.UserItemRepository,
            Noobs.EquippedItemRepository)
        .HandleAsync(interaction);

}

