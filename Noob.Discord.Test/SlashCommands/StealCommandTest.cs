﻿using Discord;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class StealCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();


    [TestCase]
    public async Task NoBrowniePoints()
    {
        Noobs.UserRepository.Save(Noobs.Ted.SetNiblets(50));
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual("You need Brownie Points to steal from other players.", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
    }

    [TestCase]
    public async Task OneBrowniePointAndTwentyLevelsBelow()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetBrowniePoints(1).SetExperience(50));
        Noobs.UserRepository.Save(Noobs.Ted.SetNiblets(50).SetExperience(2100));
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        IsFailMessage(Noobs.BillDiscord, Noobs.TedDiscord, interaction);
        Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
        Assert.Less(Noobs.Bill.Experience, 50);
        Assert.AreEqual(2100, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task TwentyLevelsBelowWithNoExperience()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetBrowniePoints(1));
        Noobs.UserRepository.Save(Noobs.Ted.SetNiblets(50).SetExperience(2000));
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        IsFailMessage(Noobs.BillDiscord, Noobs.TedDiscord, interaction);
        Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(0, Noobs.Bill.Experience);
        Assert.AreEqual(2000, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task OneBrowniePointAndTwentyLevelsAbove()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetBrowniePoints(1).SetExperience(2100));
        Noobs.UserRepository.Save(Noobs.Ted.SetNiblets(50).SetExperience(50));
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual($"You stole {Noobs.Bill.Niblets} Niblets from Ted 😈", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(0, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(2100, Noobs.Bill.Experience);
        Assert.Less(Noobs.Ted.Experience, 50);
    }

    [TestCase]
    public async Task EquipmentWithPlusTwentySneak()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetBrowniePoints(1));
        Noobs.UserRepository.Save(Noobs.Ted.SetNiblets(50));
        Noobs.ItemRepository.Save(Noobs.Mittens.SetSneak(20));
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual($"You stole {Noobs.Bill.Niblets} Niblets from Ted 😈", interaction.RespondAsyncParams.Text);
    }

    [TestCase]
    public async Task EquipmentWithPlusTwentyPerception()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetBrowniePoints(1));
        Noobs.ItemRepository.Save(Noobs.Stick.SetPerception(20));
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        IsFailMessage(Noobs.BillDiscord, Noobs.TedDiscord, interaction);
    }

    [TestCase]
    public async Task TwoItemsPlusTenPerception()
    {
        Noobs.UserRepository.Save(Noobs.Ted.SetBrowniePoints(1));
        Noobs.ItemRepository.Save(Noobs.Mittens.SetPerception(10));
        Noobs.ItemRepository.Save(Noobs.Slippers.SetPerception(10));
        var interaction = await Steal(Noobs.TedDiscord, Noobs.BillDiscord);
        IsFailMessage(Noobs.TedDiscord, Noobs.BillDiscord, interaction);
    }

    [TestCase]
    public async Task TargetHasNoNiblets()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetBrowniePoints(1).SetExperience(2100));
        Noobs.UserRepository.Save(Noobs.Ted.SetExperience(50));
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual($"Ted doesn't have any Niblets to steal :(", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(1, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(2100, Noobs.Bill.Experience);
        Assert.AreEqual(50, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task TargetDoesNotExist()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetBrowniePoints(1).SetExperience(2100));
        Noobs.UserRepository.Delete(Noobs.Ted);
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual($"Ted doesn't have any Niblets to steal :(", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(1, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(2100, Noobs.Bill.Experience);
        Assert.AreEqual(0, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task UserDoesNotExist()
    {
        Noobs.UserRepository.Delete(Noobs.Bill);
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsNotNull(Noobs.Bill);
    }

    [TestCase]
    public async Task StealsOneNiblet()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetBrowniePoints(2).SetExperience(2100));
        Noobs.UserRepository.Save(Noobs.Ted.SetNiblets(1).SetExperience(50));
        var interaction = await Steal(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.AreEqual($"You stole 1 Niblet from Ted 😈", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(2100, Noobs.Bill.Experience);
        Assert.Less(Noobs.Ted.Experience, 50);
        Assert.AreEqual(1, Noobs.Bill.BrowniePoints);
        Assert.AreEqual(1, Noobs.Bill.Niblets);
        Assert.AreEqual(0, Noobs.Ted.Niblets);
    }

    [TestCase]
    public async Task StealsFromSelf()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetBrowniePoints(2).SetExperience(2100));
        var interaction = await Steal(Noobs.BillDiscord, Noobs.BillDiscord);
        Assert.AreEqual("You want to steal from... yourself?!", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(2100, Noobs.Bill.Experience);
        Assert.AreEqual(2, Noobs.Bill.BrowniePoints);
    }

    private void IsFailMessage(IUser user, IUser victim, InteractionStub interaction)
    {
        var messages = StealCommand.FailureMessages.Select(s => string.Format(s, user.Username, victim.Username)).ToArray();
        Assert.Contains(interaction.RespondAsyncParams.Text, messages);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
    }

    private async Task<InteractionStub> Steal(IUser user, IUser victim)
    {
        var interaction = new InteractionStub(
            user,
            new (string, object)[] { ("victim", victim) }
        );

        await new StealCommand(
                Noobs.UserRepository,
                Noobs.ItemRepository,
                Noobs.EquippedItemRepository)
            .HandleAsync(interaction);
        return interaction;
    }
}
