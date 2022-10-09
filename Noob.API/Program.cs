using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using Noob.API.Commands;
using Noob.API.Discord;
using Noob.API.Models;
using Noob.API.Repositories;

public class Program
{
    public static async Task Main(string[] args)
    {
        using (var db = new SqlLiteDbContext())
        {
            await new Bot(
                new DbContextUserRepository(db),
                new DbContextUserCommandRepository(db))
                .StartAsync();

            await Task.Delay(-1);
        }
    }
}
