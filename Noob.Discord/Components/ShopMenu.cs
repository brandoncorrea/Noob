using Discord;
using Microsoft.VisualBasic;
using Noob.Core.Helpers;
using Noob.Core.Models;
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

    public MessageComponent Render(IEnumerable<Item> items)
    {
        var menuBuilder = new SelectMenuBuilder()
            .WithPlaceholder("Select an option")
            .WithCustomId(CustomId);

        foreach (var item in items)
        {
            var descriptors = new string[]
            {
                item.Description,
                Formatting.NibletTerm(item.Price),
                $"Level {item.Level}"
            }.Where(i => !string.IsNullOrWhiteSpace(i));
            menuBuilder.AddOption(
                item.Name,
                item.Id.ToString(),
                string.Join(" | ", descriptors));
        }

        return new ComponentBuilder()
            .WithSelectMenu(menuBuilder)
            .Build();
    }

    public async Task HandleAsync(IComponentInteraction interaction)
    {
        var user = UserRepository.FindOrCreate(interaction.User.Id);
        var item = FindById(interaction.Data.Values.FirstOrDefault());
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

    public Item FindById(string id) =>
        int.TryParse(id, out int itemId)
        ? ItemRepository.Find(itemId)
        : null;
}

