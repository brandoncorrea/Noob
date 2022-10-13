using Discord;
using Noob.Core.Models;
using Noob.Discord.Helpers;
using Noob.DL;

namespace Noob.Discord.Components;

public class InventoryButtons : IComponentHandler
{
    public string CustomId => "inventory-item-button";

    private IUserRepository UserRepository;
    private IItemRepository ItemRepository;
    private IUserItemRepository UserItemRepository;
    private IEquippedItemRepository EquippedItemRepository;

    public InventoryButtons() { }

    public InventoryButtons(
        IUserRepository userRepository,
        IItemRepository itemRepository,
        IUserItemRepository userItemRepository,
        IEquippedItemRepository equippedItemRepository)
    {
        UserRepository = userRepository;
        ItemRepository = itemRepository;
        UserItemRepository = userItemRepository;
        EquippedItemRepository = equippedItemRepository;
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

    public async Task HandleAsync(IComponentInteraction interaction)
    {
        var user = UserRepository.FindOrCreate(interaction.User.Id);
        var item = GetReferencedItem(interaction.Data.CustomId);
        if (item == null)
            await interaction.RespondAsync("This item does not seem to exist.", ephemeral: true);
        else if (EquippedItemRepository.IsEquipped(user, item))
        {
            EquippedItemRepository.Unequip(user, item);
            await interaction.RespondAsync($"{item.Name} has been unequipped.", ephemeral: true);
        }
        else if (!UserItemRepository.Exists(user, item))
            await interaction.RespondAsync("You do not own this item.", ephemeral: true);
        else if (user.Level < item.Level)
            await interaction.RespondAsync($"You must be level {item.Level} to equip this item.", ephemeral: true);
        else
        {
            EquippedItemRepository.Equip(user, item);
            await interaction.RespondAsync($"{item.Name} has been equipped.", ephemeral: true);
        }
    }

    private Item GetReferencedItem(string customId)
    {
        try
        {
            return ItemRepository.Find(int.Parse(customId.Split(':')[1]));
        }
        catch (Exception)
        {
            return null;
        }
    }
}

