using Discord;
using Noob.Core.Helpers;
using Noob.Core.Models;
using Noob.Discord.Components;
using Noob.Discord.Helpers;
using Noob.DL;
namespace Noob.Discord.SlashCommands;

public class InventoryCommand : ISlashCommandHandler
{
    private IUserRepository UserRepository;
    private IUserItemRepository UserItemRepository;
    private IItemRepository ItemRepository;
    private IEquippedItemRepository EquippedItemRepository;

    public InventoryCommand(
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

    public string CommandName => "inventory";

    public SlashCommandProperties GetSlashCommandProperties() =>
        new SlashCommandBuilder
        {
            Name = CommandName,
            Description = "See your inventory!"
        }.Build();

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        var items = UserItemRepository
            .FindAll(command.User.Id)
            .Select(ItemRepository.Find);
        if (!items.Any())
            await command.RespondAsync("You do not have any items in your inventory.", ephemeral: true);
        else
            await DisplayInventory(command, UserRepository.FindOrCreate(command.User.Id), items);
    }

    private async Task DisplayInventory(ISlashCommandInteraction command, User user, IEnumerable<Item> items) =>
        await command.RespondAsync(
            embed: new EmbedBuilder()
                .WithSimpleAuthor(command.User)
                .WithTitle("Inventory")
                .WithDescription(items.Select(GetDescriptor))
                .WithColor(Color.Green)
                .Build(),
            components: new InventoryButtons()
                .Render(
                    user,
                    items,
                    EquippedItemRepository.EquippedItems(user)),
            ephemeral: true);

    private string GetDescriptor(Item item) =>
        Formatting.TakePopulated(
            item.Name,
            item.Attack > 0 ? $"⚔️ {item.Attack}" : "",
            item.Defense > 0 ? $"🛡 {item.Defense}" : "",
            item.Sneak > 0 ? $"🥷 {item.Sneak}" : "",
            item.Perception > 0 ? $"👁 {item.Perception}" : "",
            $"⭐️ {item.Level}")
        .Join(" ");
}
