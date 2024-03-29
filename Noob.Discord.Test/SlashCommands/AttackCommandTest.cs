﻿using Discord;
using Noob.Discord.SlashCommands;
using Noob.Discord.Test.Stub;
namespace Noob.Discord.Test.SlashCommands;

[TestFixture]
public class AttackCommandTest
{
    [SetUp]
    public void SetUp() => Noobs.Initialize();

    [TestCase]
    public async Task UserAttacksSelf()
    {
        var interaction = await Attack(Noobs.BillDiscord, Noobs.BillDiscord);
        Assert.AreEqual("Self harm is NOT okay 😭❤️", interaction.RespondAsyncParams.Text);
        Assert.IsTrue(interaction.RespondAsyncParams.Ephemeral);
    }

    [TestCase]
    public async Task TwentyLevelsBelowWithNoExperience()
    {
        Noobs.UserRepository.Save(Noobs.Ted.SetExperience(2000));
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        IsFailMessage(Noobs.BillDiscord, Noobs.TedDiscord, interaction);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(0, Noobs.Bill.Experience);
        Assert.AreEqual(2000, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task TwentyLevelsBelow()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetExperience(50));
        Noobs.UserRepository.Save(Noobs.Ted.SetExperience(2000));
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        IsFailMessage(Noobs.BillDiscord, Noobs.TedDiscord, interaction);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(50, Noobs.Bill.Experience);
        Assert.AreEqual(2000, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task EquipmentWithPlusTwentyAttack()
    {
        Noobs.ItemRepository.Save(Noobs.Stick.SetAttack(20));
        var interaction = await Attack(Noobs.TedDiscord, Noobs.BillDiscord);
        IsSuccessMessage(Noobs.TedDiscord, Noobs.BillDiscord, interaction);
    }

    [TestCase]
    public async Task EquipmentWithPlusTwentyDefense()
    {
        Noobs.ItemRepository.Save(Noobs.Mittens.SetDefense(20));
        var interaction = await Attack(Noobs.TedDiscord, Noobs.BillDiscord);
        IsFailMessage(Noobs.TedDiscord, Noobs.BillDiscord, interaction);
    }

    [TestCase]
    public async Task TwoItemsPlusTenDefense()
    {
        Noobs.ItemRepository.Save(Noobs.Mittens.SetDefense(10));
        Noobs.ItemRepository.Save(Noobs.Slippers.SetDefense(10));
        var interaction = await Attack(Noobs.TedDiscord, Noobs.BillDiscord);
        IsFailMessage(Noobs.TedDiscord, Noobs.BillDiscord, interaction);
    }

    [TestCase]
    public async Task TwentyLevelsAbove()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetExperience(2000));
        Noobs.UserRepository.Save(Noobs.Ted.SetExperience(50));
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        IsSuccessMessage(Noobs.BillDiscord, Noobs.TedDiscord, interaction);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(2000, Noobs.Bill.Experience);
        Assert.AreEqual(50, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task FairFight()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetExperience(50));
        Noobs.UserRepository.Save(Noobs.Ted.SetExperience(50));
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreNotEqual(50, Noobs.Ted.Experience);
        Assert.AreNotEqual(50, Noobs.Bill.Experience);
    }

    [TestCase]
    public async Task TargetDoesNotExist()
    {
        Noobs.UserRepository.Save(Noobs.Bill.SetExperience(2100));
        Noobs.UserRepository.Delete(Noobs.Ted);
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        IsSuccessMessage(Noobs.BillDiscord, Noobs.TedDiscord, interaction);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.AreEqual(2100, Noobs.Bill.Experience);
        Assert.AreEqual(0, Noobs.Ted.Experience);
    }

    [TestCase]
    public async Task UserDoesNotExist()
    {
        Noobs.UserRepository.Delete(Noobs.Bill);
        var interaction = await Attack(Noobs.BillDiscord, Noobs.TedDiscord);
        Assert.IsFalse(interaction.RespondAsyncParams.Ephemeral);
        Assert.IsNotNull(Noobs.Bill.Experience);
    }

    private void IsSuccessMessage(IUser user, IUser victim, InteractionStub interaction) =>
        IsFormattedMessage(AttackCommand.SuccessMessages, user, victim, interaction);

    private void IsFailMessage(IUser user, IUser victim, InteractionStub interaction) =>
        IsFormattedMessage(AttackCommand.FailureMessages, user, victim, interaction);

    private void IsFormattedMessage(string[] formats, IUser user, IUser victim, InteractionStub interaction)
    {
        var messages = formats.Select(s => string.Format(s, user.Username, victim.Username)).ToArray();
        Assert.Contains(interaction.RespondAsyncParams.Text, messages);
    }

    private async Task<InteractionStub> Attack(IUser user, IUser victim)
    {
        var interaction = new InteractionStub(
            user,
            new (string, object)[] { ("victim", victim) }
        );

        await new AttackCommand(
                Noobs.UserRepository,
                Noobs.ItemRepository,
                Noobs.EquippedItemRepository)
            .HandleAsync(interaction);
        return interaction;
    }
}
