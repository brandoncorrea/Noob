using Discord;
using Noob.Core.Models;
using Noob.Discord.Components;
using Noob.DL;

namespace Noob.Discord.SlashCommands;

public class ShopCommand : ISlashCommandHandler
{
    private IItemRepository ItemRepository;
    public string CommandName => "shop";
    public ShopCommand(IItemRepository itemRepository) =>
        ItemRepository = itemRepository;

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "Purchase items from the shopkeeper."
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        var items = ItemRepository.FindAll();
        if (items.Any())
            await command.RespondAsync(
                "Choose an item to purchase.",
                components: new ShopMenu().Render(items),
                ephemeral: true);
        else
            await command.RespondAsync(
                "There are no items available to purchase.",
                ephemeral: true);
    }
}
