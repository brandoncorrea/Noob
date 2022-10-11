using Discord;
using Noob.Core.Models;
using Noob.Discord.Components;
using Noob.DL;

namespace Noob.Discord.SlashCommands;

public class ShopCommand : ISlashCommandHandler
{
    public string CommandName => "shop";
    private IItemRepository ItemRepository;
    private IUserItemRepository UserItemRepository;

    public ShopCommand(
        IItemRepository itemRepository,
        IUserItemRepository userItemRepository)
    {
        ItemRepository = itemRepository;
        UserItemRepository = userItemRepository;
    }

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "Purchase items from the shopkeeper."
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        var excludedItems = UserItemRepository
            .FindAll(command.User.Id)
            .Select(item => item.ItemId);

        var items = ItemRepository
            .FindAll()
            .Where(item => !excludedItems.Contains(item.Id));

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
