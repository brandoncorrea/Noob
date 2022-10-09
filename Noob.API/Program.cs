using Noob.Discord;
using Noob.DL;

public class Program
{
    public static async Task Main(string[] args)
    {
        using (var db = SqlLiteDbContext.Create(@"Data Source=./db/noob.db"))
        {
            await new Bot(
                new DbContextUserRepository(db),
                new DbContextUserCommandRepository(db))
                .StartAsync();

            await Task.Delay(-1);
        }
    }
}
