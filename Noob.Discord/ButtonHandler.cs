using Discord;
using Noob.Discord.Components;
using Noob.DL;
namespace Noob.Discord;

public class ButtonHandler
{
    IEnumerable<IComponentHandler> ButtonHandlers;

    public ButtonHandler(
        IUserRepository userRepository,
        IItemRepository itemRepository,
        IUserItemRepository userItemRepository,
        IEquippedItemRepository equippedItemRepository)
    {
        ButtonHandlers = new IComponentHandler[]
        {
            new InventoryButtons(userRepository, itemRepository, userItemRepository, equippedItemRepository)
        };
    }

    public async Task HandleAsync(IComponentInteraction interaction)
    {
        var handlerId = interaction.Data.CustomId.Split(':').FirstOrDefault();
        var handler = ButtonHandlers.FirstOrDefault(handler => handlerId == handler.CustomId);
        if (handler == null) return;
        await handler.HandleAsync(interaction);
    }
}

