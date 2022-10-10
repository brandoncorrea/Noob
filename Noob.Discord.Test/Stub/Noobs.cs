using Microsoft.EntityFrameworkCore;
using Noob.Core.Models;
using Noob.DL;
namespace Noob.Discord.Test.Stub;

public static class Noobs
{
    public static User Bill => UserRepository.Find(1);
    public static User Ted => UserRepository.Find(2);
    public static Item Stick => ItemRepository.Find(1);
    public static Item Shield => ItemRepository.Find(2);
    public static Item Crowbar => ItemRepository.Find(3);
    public static UserItem TedStick => UserItemRepository.Find(Ted, Stick);
    public static DiscordUserStub BillDiscord;
    public static DiscordUserStub TedDiscord;
    public static UserCommand BillDaily => UserCommandRepository.Find(1, 1);
    public static UserCommand TedWeekly => UserCommandRepository.Find(2, 2);
    public static IUserRepository UserRepository;
    public static IUserCommandRepository UserCommandRepository;
    public static IItemRepository ItemRepository;
    public static IUserItemRepository UserItemRepository;
    public static NoobDbContext Db;

    public static void Initialize()
    {
        var bill = new User { Id = 1 };
        var ted = new User { Id = 2 };

        var billDaily = new UserCommand { UserId = bill.Id, CommandId = 1 };
        var tedWeekly = new UserCommand { UserId = ted.Id, CommandId = 2 };

        var stick = new Item
        {
            Id = 1,
            Name = "Stick",
            Description = "A wooden stick.",
            Price = 10,
            Level = 1
        };

        var shield = new Item
        {
            Id = 2,
            Name = "Shield",
            Description = "Blocks some stuff.",
            Price = 50,
            Level = 2
        };

        var crowbar = new Item
        {
            Id = 3,
            Name = "Crowbar",
            Description = "Wacks some stuff.",
            Price = 100,
            Level = 3
        };

        var tedStick = new UserItem { UserId = ted.Id, ItemId = stick.Id };

        Db = InMemoryDbContext.Create("InMemoryNoobs");
        Db.Database.EnsureDeleted();
        Db.Database.EnsureCreated();
        Db.Users.Add(bill);
        Db.Users.Add(ted);
        Db.UserCommands.Add(billDaily);
        Db.UserCommands.Add(tedWeekly);
        Db.Items.Add(stick);
        Db.Items.Add(shield);
        Db.Items.Add(crowbar);
        Db.UserItems.Add(tedStick);
        Db.SaveChanges();

        UserRepository = new DbContextUserRepository(Db);
        UserCommandRepository = new DbContextUserCommandRepository(Db);
        ItemRepository = new DbContextItemRepository(Db);
        UserItemRepository = new DbContextUserItemRepository(Db);

        BillDiscord = new DiscordUserStub(bill.Id, "Bill", "billy");
        TedDiscord = new DiscordUserStub(ted.Id, "Ted", "teddy");
    }
}
