//using System.Reflection;
using Discord;
//using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Noob.Discord.Sockets;
//using Noob.Discord.Modules;
//using Noob.Discord.SlashCommands;
using Noob.DL;
namespace Noob.Discord;

/// <summary>
/// TODO: Figure out how to test Modules and move slash commands out to there
/// Once that's done, uncomment all the pending code in this file
/// </summary>
public class Bot
{
    private ISocketClient Client;
    //private InteractionService Interactions;
    private readonly IServiceProvider ServiceProvider;

    public Bot(
        IGuildCountRepository guildCountRepository,
        IUserRepository userRepository,
        IUserCommandRepository userCommandRepository,
        IItemRepository itemRepository,
        IUserItemRepository userItemRepository,
        IEquippedItemRepository equippedItemRepository) =>
        ServiceProvider = new ServiceCollection()
            .AddSingleton(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged
            })
            .AddSingleton<ISocketClient, DiscordNetSocketClient>()
            //.AddSingleton<InteractionService>()
            .AddSingleton<IUserRepository>(userRepository)
            .AddSingleton<IUserCommandRepository>(userCommandRepository)
            .AddSingleton<IItemRepository>(itemRepository)
            .AddSingleton<IUserItemRepository>(userItemRepository)
            .AddSingleton<IEquippedItemRepository>(equippedItemRepository)
            .AddSingleton<IGuildCountRepository>(guildCountRepository)
            .AddSingleton<SlashCommandHandler>()
            .AddSingleton<SelectMenuHandler>()
            .AddSingleton<ButtonHandler>()
            .BuildServiceProvider();

    public async Task StartAsync(string token)
    {
        //Interactions = ServiceProvider.GetRequiredService<InteractionService>();
        Client = ServiceProvider.GetRequiredService<ISocketClient>();
        Client.Log += Log;
        Client.SlashCommandExecuted += ServiceProvider.GetRequiredService<SlashCommandHandler>().HandleAsync;
        Client.SelectMenuExecuted += ServiceProvider.GetRequiredService<SelectMenuHandler>().HandleAsync;
        Client.ButtonExecuted += ServiceProvider.GetRequiredService<ButtonHandler>().HandleAsync;
        Client.JoinedGuild += ServiceProvider.GetRequiredService<SlashCommandHandler>().RegisterGuild;
        Client.Ready += ClientReady;
        //await RegisterCommandsAsync();
        await Client.LoginAsync(TokenType.Bot, token);
        await Client.StartAsync();
    }

    private async Task ClientReady()
    {
        foreach (var guild in Client.Guilds)
            await ServiceProvider.GetRequiredService<SlashCommandHandler>().RegisterGuild(guild);
            // TODO: Figure out how to test Modules and move slash commands in that direction
            //await Interactions.RegisterCommandsToGuildAsync(guild.Id);
    }

    // TODO: Figure out how to test Modules and move slash commands in that direction
    //private async Task RegisterCommandsAsync()
    //{
    //    await Interactions.AddModulesAsync(Assembly.GetExecutingAssembly(), ServiceProvider);
    //    Client.InteractionCreated += HandleInteractionAsync;
    //}

    //private async Task HandleInteractionAsync(SocketInteraction arg)
    //{
    //    var context = new SocketInteractionContext(Client, arg);
    //    await Interactions.ExecuteCommandAsync(context, ServiceProvider);
    //}

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
