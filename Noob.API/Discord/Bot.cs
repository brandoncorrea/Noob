using System;
using Discord;
using Discord.WebSocket;
using Noob.API.Commands;
using Noob.API.Repositories;
namespace Noob.API.Discord;

public class Bot
{
    private DiscordSocketClient Client;
    private SlashCommandHandler SlashCommandHandler;
    
    public Bot(
        IUserRepository userRepository,
        IUserCommandRepository userCommandRepository) =>
        SlashCommandHandler = new SlashCommandHandler(userRepository, userCommandRepository);

    public async Task StartAsync()
    {
        Client = new DiscordSocketClient();
        Client.Log += Log;
        Client.SlashCommandExecuted += SlashCommandHandler.Handle;
        Client.JoinedGuild += SlashCommandHandler.RegisterGuild;
        Client.Ready += ClientReady;
        await Client.LoginAsync(TokenType.Bot, File.ReadAllText("discord.token"));
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
