using Discord;
using Discord.WebSocket;
using Noob.Discord.SlashCommands;
using Noob.DL;
namespace Noob.Discord;

public class SlashCommandHandler
{
    private IEnumerable<SlashCommandProperties> SlashCommands;
    public IEnumerable<ISlashCommandHandler> SlashCommandHandlers;

    public SlashCommandHandler(
        IUserRepository userRepository,
        IUserCommandRepository userCommandRepository,
        IItemRepository itemRepository,
        IUserItemRepository userItemRepository,
        IEquippedItemRepository equippedItemRepository)
    {
        SlashCommandHandlers = new ISlashCommandHandler[]
        {
            new HelpCommand(),
            new LoveCommand(),
            new DailyCommand(userRepository, userCommandRepository),
            new WeeklyCommand(userRepository, userCommandRepository),
            new GiveCommand(userRepository),
            new StealCommand(userRepository, itemRepository, equippedItemRepository),
            new StatsCommand(userRepository),
            new ShopCommand(itemRepository, userItemRepository),
            new AttackCommand(userRepository, itemRepository, equippedItemRepository),
            new InventoryCommand(userRepository, itemRepository, userItemRepository, equippedItemRepository),
        };
        SlashCommands = CreateSlashCommands();
    }

    public IEnumerable<SlashCommandProperties> CreateSlashCommands() =>
        SlashCommandHandlers.Select(h => h.GetSlashCommandProperties());

    public async Task HandleAsync(ISlashCommandInteraction command)
    {
        var handler = SlashCommandHandlers.FirstOrDefault(h => h.CommandName == command.Data.Name);
        if (handler == null) return;
        await handler.HandleAsync(command);
    }

    public async Task RegisterGuild(SocketGuild guild)
    {
        foreach (var command in SlashCommands)
            await guild.CreateApplicationCommandAsync(command);
    }
}
