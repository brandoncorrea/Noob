using Microsoft.EntityFrameworkCore;
namespace Noob.DL;

public class InMemoryDbContext : NoobDbContext
{
    public InMemoryDbContext(DbContextOptions options) : base(options) { }
    public static InMemoryDbContext Create(string databaseName) =>
        new InMemoryDbContext(
            new DbContextOptionsBuilder<NoobDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options);
}
