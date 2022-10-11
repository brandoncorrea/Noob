using Noob.Discord;
using Noob.DL;
using Noob.Migrate;

public class Program
{
    public static async Task Main(string[] args)
    {
        using (var db = SqlLiteDbContext.Create(@"Data Source=./db/noob.db"))
        {
            Migration.Migrate(db);

            var itemRepository = new DbContextItemRepository(db);
            await new Bot(
                new DbContextUserRepository(db),
                new DbContextUserCommandRepository(db),
                itemRepository,
                new DbContextUserItemRepository(db),
                new DbContextEquippedItemRepository(db, itemRepository))
                .StartAsync(File.ReadAllText("discord.token"));

            await Task.Delay(-1);
        }
    }
}
