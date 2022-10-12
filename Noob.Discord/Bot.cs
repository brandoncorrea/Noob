using Discord;
using Discord.WebSocket;
using Noob.Discord.SlashCommands;
using Noob.DL;
namespace Noob.Discord;

public class Bot
{
    private DiscordSocketClient Client;
    private SlashCommandHandler SlashCommandHandler;
    private SelectMenuHandler SelectMenuHandler;
    private ButtonHandler ButtonHandler;

    public Bot(
        IUserRepository userRepository,
        IUserCommandRepository userCommandRepository,
        IItemRepository itemRepository,
        IUserItemRepository userItemRepository,
        IEquippedItemRepository equippedItemRepository)
    {
        SlashCommandHandler = new SlashCommandHandler(
            userRepository,
            userCommandRepository,
            itemRepository,
            userItemRepository,
            equippedItemRepository);

        SelectMenuHandler = new SelectMenuHandler(
            userRepository,
            itemRepository,
            userItemRepository);

        ButtonHandler = new ButtonHandler(
            userRepository,
            itemRepository,
            userItemRepository,
            equippedItemRepository);
    }

    public async Task StartAsync(string token)
    {
        Client = new DiscordSocketClient();
        Client.Log += Log;
        Client.SlashCommandExecuted += SlashCommandHandler.HandleAsync;
        Client.SelectMenuExecuted += SelectMenuHandler.HandleAsync;
        Client.ButtonExecuted += ButtonHandler.HandleAsync;
        Client.JoinedGuild += SlashCommandHandler.RegisterGuild;
        Client.Ready += ClientReady;
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();
    }

    private async Task ClientReady()
    {
        foreach (var guild in Client.Guilds)
            await SlashCommandHandler.RegisterGuild(guild);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
