using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using Noob.API.Commands;
using Noob.API.Discord;
using Noob.API.Repositories;

public class Program
{
    public static async Task Main(string[] args)
    {
        await new Bot(
            new JsonUserRepository("db/user.json"),
            new JsonUserCommandRepository("db/userCommand.json"))
            .StartAsync();

        await Task.Delay(-1);
    }
}
