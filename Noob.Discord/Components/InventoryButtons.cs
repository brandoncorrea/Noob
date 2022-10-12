using Discord;
using Noob.Core.Models;
using Noob.Discord.Helpers;
using Noob.DL;

namespace Noob.Discord.Components;

public class InventoryButtons : IComponentHandler
{
    public string CustomId => "inventory-item-button";

    public InventoryButtons() { }

    public InventoryButtons(
        IUserRepository userRepository,
        IItemRepository itemRepository,
        IUserItemRepository userItemRepository,
        IEquippedItemRepository equippedItemRepository)
    {

    }

    public MessageComponent Render(User user, IEnumerable<Item> inventory, IEnumerable<Item> equippedItems) =>
        new ComponentBuilder()
            .WithButtons(
                inventory.Select(item =>
                    CreateItemButton(user, SlottedItem(equippedItems, item), item)))
            .Build();

    private Item SlottedItem(IEnumerable<Item> equippedItems, Item item) =>
        equippedItems.FirstOrDefault(i => i.SlotId == item.SlotId);

    private ButtonBuilder CreateItemButton(User user, Item slottedItem, Item item) =>
        new ButtonBuilder()
        .WithCustomId($"{CustomId}:{item.Id}")
        .WithLabel(item.Name)
        .WithDisabled(item.Level > user.Level)
        .WithStyle(
            slottedItem != null && slottedItem.Id == item.Id
            ? ButtonStyle.Success
                : user.Level >= item.Level ? ButtonStyle.Primary
                : ButtonStyle.Secondary);

    public Task HandleAsync(IComponentInteraction interaction)
    {
        throw new NotImplementedException();
    }
}

