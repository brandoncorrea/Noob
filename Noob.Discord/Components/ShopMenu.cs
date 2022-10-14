using Discord;
using Microsoft.VisualBasic;
using Noob.Core.Enums;
using Noob.Core.Helpers;
using Noob.Core.Models;
using Noob.Discord.Helpers;
using Noob.DL;
namespace Noob.Discord.Components;

public class ShopMenu : IComponentHandler
{
    public string CustomId => "shop-menu";
    private IUserRepository UserRepository;
    private IItemRepository ItemRepository;
    private IUserItemRepository UserItemRepository;

    public ShopMenu() { }

    public ShopMenu(
        IUserRepository userRepository,
        IItemRepository itemRepository,
        IUserItemRepository userItemRepository)
    {
        UserRepository = userRepository;
        ItemRepository = itemRepository;
        UserItemRepository = userItemRepository;
    }

    public MessageComponent Render(IEnumerable<Item> items) =>
        new ComponentBuilder()
        .WithSelectMenu(new SelectMenuBuilder()
            .WithPlaceholder("Select an option")
            .WithCustomId(CustomId)
            .AddOptions(items.Select(CreateMenuOption)))
        .Build();

    private SelectMenuOptionBuilder CreateMenuOption(Item item) =>
        new SelectMenuOptionBuilder(
            GetDescriptor(item),
            item.Id.ToString(),
            string.IsNullOrWhiteSpace(item.Description) ? null : item.Description);

    public async Task HandleAsync(IComponentInteraction interaction)
    {
        var user = UserRepository.FindOrCreate(interaction.User.Id);
        var item = ItemRepository.Find(interaction.Data.Values.FirstOrDefault());
        if (item == null)
            await interaction.RespondAsync("This item does not seem to exist.", ephemeral: true);
        else if (UserItemRepository.Exists(user, item))
            await interaction.RespondAsync($"It looks like you already own a {item.Name}.", ephemeral: true);
        else if (user.Level < item.Level)
            await interaction.RespondAsync($"You need to be level {item.Level} to purchase this item.", ephemeral: true);
        else if (user.Niblets < item.Price)
            await interaction.RespondAsync("You do not have enough Niblets to purchase this item.", ephemeral: true);
        else
        {
            user.Niblets -= item.Price;
            UserRepository.Save(user);
            UserItemRepository.Create(user, item);
            await interaction.RespondAsync($"{item.Name} has been added to your inventory!", ephemeral: true);
        }
    }

    private string GetDescriptor(Item item) =>
        Formatting.TakePopulated(
            $"{item.Name} – {ItemSlot.NameOf(item.SlotId)} – {Formatting.NibletTerm(item.Price)}",
            item.Attack > 0 ? $"⚔️ {item.Attack}" : "",
            item.Defense > 0 ? $"🛡 {item.Defense}" : "",
            item.Sneak > 0 ? $"🥷 {item.Sneak}" : "",
            item.Perception > 0 ? $"👁 {item.Perception}" : "",
            $"⭐️ {item.Level}")
        .Join(" ");
}
