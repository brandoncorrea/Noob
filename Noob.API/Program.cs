using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using Noob.API.Commands;
using Noob.API.Repositories;

public class Program
{
    private DiscordSocketClient client;
    private IUserRepository UserRepository;
    private IUserCommandRepository UserCommandRepository;
    private RecurrentCommand RecurrentCommandHandler;

    public static Task Main(string[] args) => new Program().MainAsync();

    public async Task MainAsync()
    {
        client = new DiscordSocketClient();
        client.Log += Log;

        UserRepository = new JsonUserRepository("db/user.json");
        UserCommandRepository = new JsonUserCommandRepository("db/userCommand.json");
        RecurrentCommandHandler = new RecurrentCommand(UserRepository, UserCommandRepository);

        await client.LoginAsync(TokenType.Bot, File.ReadAllText("discord.token"));
        await client.StartAsync();
        client.SlashCommandExecuted += SlashCommandHandler;
        client.Ready += ClientReady;

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private async Task SlashCommandHandler(SocketSlashCommand command)
    {
        if (command.Data.Name == "daily")
            await command.RespondAsync(RecurrentCommandHandler.Daily(command.User.Id).Message);
        else if (command.Data.Name == "weekly")
            await command.RespondAsync(RecurrentCommandHandler.Weekly(command.User.Id).Message);
    }


    public async Task ClientReady()
    {
        var commands = new Dictionary<String, String>
        {
            { "daily", "Redeem your daily Niblets!" },
            { "weekly", "Redeem your weekly Niblets!" },
        }
        .Select(c => new SlashCommandBuilder
        {
            Name = c.Key,
            Description = c.Value
        }.Build());

        foreach (var guild in client.Guilds)
            foreach (var command in commands)
                await guild.CreateApplicationCommandAsync(command);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}
