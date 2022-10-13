using Discord;
using Noob.Discord.Components;
using Noob.Discord.Test.Stub;

namespace Noob.Discord.Test.Components;

[TestFixture]
public class InventoryButtonsTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [TestCase]
    public async Task UserDoesNotExistButItemLinkDoes()
    {
        Noobs.EquippedItemRepository.Unequip(Noobs.Bill, Noobs.Mittens);
        Noobs.UserRepository.Delete(Noobs.Bill);
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, $"inventory-item-button:{Noobs.Mittens.Id}", null);
        await ToggleItem(interaction);
        Assert.AreEqual("Mittens has been equipped.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsNotNull(Noobs.Bill);
        Assert.IsTrue(Noobs.EquippedItemRepository.IsEquipped(Noobs.Bill, Noobs.Mittens));
    }

    [TestCase]
    public async Task LinkToItemDoesNotExist()
    {
        Noobs.EquippedItemRepository.Unequip(Noobs.Bill, Noobs.Mittens);
        Noobs.UserItemRepository.Dissociate(Noobs.Bill, Noobs.Mittens);
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, $"inventory-item-button:{Noobs.Mittens.Id}", null);
        await ToggleItem(interaction);
        Assert.AreEqual("You do not own this item.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task ItemDoesNotExist()
    {
        var mittensId = Noobs.Mittens.Id;
        Noobs.ItemRepository.Delete(Noobs.Mittens);
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, $"inventory-item-button:{mittensId}", null);
        await ToggleItem(interaction);
        Assert.AreEqual("This item does not seem to exist.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task ItemAlreadyEquipped()
    {
        Noobs.EquippedItemRepository.Equip(Noobs.Bill, Noobs.Mittens);
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, $"inventory-item-button:{Noobs.Mittens.Id}", null);
        await ToggleItem(interaction);
        Assert.AreEqual("Mittens has been unequipped.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsFalse(Noobs.EquippedItemRepository.IsEquipped(Noobs.Bill, Noobs.Mittens));
    }

    [TestCase]
    public async Task UnequipsUnownedItem()
    {
        Noobs.UserItemRepository.Dissociate(Noobs.Bill, Noobs.Mittens);
        Noobs.EquippedItemRepository.Equip(Noobs.Bill, Noobs.Mittens);
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, $"inventory-item-button:{Noobs.Mittens.Id}", null);
        await ToggleItem(interaction);
        Assert.AreEqual("Mittens has been unequipped.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsFalse(Noobs.EquippedItemRepository.IsEquipped(Noobs.Bill, Noobs.Mittens));
    }

    [TestCase]
    public async Task UnequipsItemAboveCurrentLevel()
    {
        Noobs.EquippedItemRepository.Equip(Noobs.Bill, Noobs.Crowbar);
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, $"inventory-item-button:{Noobs.Crowbar.Id}", null);
        await ToggleItem(interaction);
        Assert.AreEqual("Crowbar has been unequipped.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsFalse(Noobs.EquippedItemRepository.IsEquipped(Noobs.Bill, Noobs.Crowbar));
    }

    [TestCase]
    public async Task MalformedId()
    {
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, "inventory-item-button:asd", null);
        await ToggleItem(interaction);
        Assert.AreEqual("This item does not seem to exist.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task MissingId()
    {
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, "inventory-item-button:", null);
        await ToggleItem(interaction);
        Assert.AreEqual("This item does not seem to exist.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task MissingSemicolonSeparator()
    {
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, "inventory-item-button", null);
        await ToggleItem(interaction);
        Assert.AreEqual("This item does not seem to exist.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task RequisiteLevelNotMet()
    {
        Noobs.UserItemRepository.Create(Noobs.Bill, Noobs.Shield);
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, $"inventory-item-button:{Noobs.Shield.Id}", null);
        await ToggleItem(interaction);
        Assert.AreEqual("You must be level 2 to equip this item.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task ItemAlreadyEquippedForSlot()
    {
        Noobs.ItemRepository.Save(Noobs.Crowbar.SetLevel(1));
        Noobs.UserItemRepository.Create(Noobs.Ted, Noobs.Crowbar);
        var interaction = new ComponentInteractionStub(Noobs.TedDiscord, $"inventory-item-button:{Noobs.Crowbar.Id}", null);
        await ToggleItem(interaction);
        Assert.AreEqual("Crowbar has been equipped.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsTrue(Noobs.EquippedItemRepository.IsEquipped(Noobs.Ted, Noobs.Crowbar));
        Assert.IsFalse(Noobs.EquippedItemRepository.IsEquipped(Noobs.Ted, Noobs.Stick));
    }

    [TestCase]
    public async Task HasItemEquippedForDifferentSlot()
    {
        Noobs.UserItemRepository.Create(Noobs.Ted, Noobs.Mittens);
        var interaction = new ComponentInteractionStub(Noobs.TedDiscord, $"inventory-item-button:{Noobs.Mittens.Id}", null);
        await ToggleItem(interaction);
        Assert.AreEqual("Mittens has been equipped.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsTrue(Noobs.EquippedItemRepository.IsEquipped(Noobs.Ted, Noobs.Stick));
        Assert.IsTrue(Noobs.EquippedItemRepository.IsEquipped(Noobs.Ted, Noobs.Mittens));
    }

    private async Task ToggleItem(IComponentInteraction interaction) =>
        await new InventoryButtons(
            Noobs.UserRepository,
            Noobs.ItemRepository,
            Noobs.UserItemRepository,
            Noobs.EquippedItemRepository)
        .HandleAsync(interaction);
}

