using Discord;
using Noob.Discord.Components;
using Noob.DL;

namespace Noob.Discord;

public class SelectMenuHandler
{
    IEnumerable<IComponentHandler> SelectMenuHandlers;

    public SelectMenuHandler(
        IUserRepository userRepository,
        IItemRepository itemRepository,
        IUserItemRepository userItemRepository)
    {
        SelectMenuHandlers = new IComponentHandler[]
        {
            new ShopMenu(userRepository, itemRepository, userItemRepository),
        };
    }

    public async Task HandleAsync(IComponentInteraction interaction)
    {
        var handler = SelectMenuHandlers
            .FirstOrDefault(handler => handler.CustomId == interaction.Data.CustomId);
        if (handler == null) return;
        await handler.HandleAsync(interaction);
    }
}

