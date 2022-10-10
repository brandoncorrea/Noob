using Discord;
using Microsoft.VisualBasic;
using Noob.Discord.Components;
using Noob.Discord.Test.Stub;

namespace Noob.Discord.Test.Components;

[TestFixture]
public class ShopMenuTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [TestCase]
    public async Task UserHasNoNiblets()
    {
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, Noobs.Stick.Id.ToString());
        await PurchaseItem(interaction);
        Assert.AreEqual("You do not have enough Niblets to purchase this item.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task OneNibletShortOfPrice()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(9));
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, Noobs.Stick.Id.ToString());
        await PurchaseItem(interaction);
        Assert.AreEqual("You do not have enough Niblets to purchase this item.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task UserDoesNotExist()
    {
        Noobs.UserRepository.Delete(Noobs.Bill);
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, Noobs.Stick.Id.ToString());
        await PurchaseItem(interaction);
        Assert.AreEqual("You do not have enough Niblets to purchase this item.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsNotNull(Noobs.Bill);
    }

    [TestCase]
    public async Task UserIsBelowItemLevel2()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(50));
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, Noobs.Shield.Id.ToString());
        await PurchaseItem(interaction);
        Assert.AreEqual("You need to be level 2 to purchase this item.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task UserIsBelowItemLevel3()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(100).SetExperience(150));
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, Noobs.Crowbar.Id.ToString());
        await PurchaseItem(interaction);
        Assert.AreEqual("You need to be level 3 to purchase this item.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task NotEnoughNibletsAndBelowItemLevel()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(49));
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, Noobs.Shield.Id.ToString());
        await PurchaseItem(interaction);
        Assert.AreEqual("You need to be level 2 to purchase this item.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task ValueIsNull()
    {
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, null);
        await PurchaseItem(interaction);
        Assert.AreEqual("This item does not seem to exist.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task ValueIsNotAnInteger()
    {
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, "bleh");
        await PurchaseItem(interaction); Assert.AreEqual("This item does not seem to exist.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task LevelOneUserHasExactNiblets()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(10));
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, Noobs.Stick.Id.ToString());
        await PurchaseItem(interaction);
        Assert.AreEqual("Stick has been added to your inventory!", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(0, Noobs.Bill.Niblets);
        Assert.IsNotNull(Noobs.UserItemRepository.Find(Noobs.Bill, Noobs.Stick));
    }

    [TestCase]
    public async Task LevelTwoUserHasMoreNibletsThanPrice()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetNiblets(51).SetExperience(100));
        var interaction = new ComponentInteractionStub(Noobs.BillDiscord, Noobs.Shield.Id.ToString());
        await PurchaseItem(interaction);
        Assert.AreEqual("Shield has been added to your inventory!", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(1, Noobs.Bill.Niblets);
        Assert.IsNotNull(Noobs.UserItemRepository.Find(Noobs.Bill, Noobs.Shield));
    }

    [TestCase]
    public async Task UserAlreadyOwnsItemBeingPurchased()
    {
        Noobs.UserRepository.Save(Noobs.Ted.SetNiblets(10));
        var interaction = new ComponentInteractionStub(Noobs.TedDiscord, Noobs.Stick.Id.ToString());
        await PurchaseItem(interaction);
        Assert.AreEqual("It looks like you already own a Stick.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(10, Noobs.Ted.Niblets);
        Assert.IsNotNull(Noobs.TedStick);
    }

    private async Task PurchaseItem(IComponentInteraction interaction) =>
        await new ShopMenu(
            Noobs.UserRepository,
            Noobs.ItemRepository,
            Noobs.UserItemRepository)
        .HandleAsync(interaction);
}
