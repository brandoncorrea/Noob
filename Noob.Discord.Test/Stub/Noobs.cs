using Discord;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Noob.Core.Enums;
using Noob.Core.Models;
using Noob.Discord.Sockets;
using Noob.DL;
namespace Noob.Discord.Test.Stub;

public static class Noobs
{
    public static Dictionary<ulong, ulong> UserPermissions;
    public static User Bill => UserRepository.Find(1);
    public static User Ted => UserRepository.Find(2);
    public static Item Stick => ItemRepository.Find(1);
    public static Item Shield => ItemRepository.Find(2);
    public static Item Crowbar => ItemRepository.Find(3);
    public static Item Slippers => ItemRepository.Find(4);
    public static Item Mittens => ItemRepository.Find(5);
    public static Item Hat => ItemRepository.Find(6);
    public static UserItem TedStick => UserItemRepository.Find(Ted, Stick);
    public static UserItem BillMittens => UserItemRepository.Find(Bill, Mittens);
    public static UserItem BillSlippers => UserItemRepository.Find(Bill, Slippers);
    public static DiscordUserStub BillDiscord;
    public static DiscordUserStub TedDiscord;
    public static UserCommand BillDaily => UserCommandRepository.Find(1, 1);
    public static UserCommand TedWeekly => UserCommandRepository.Find(2, 2);

    public static SocketClientStub SocketClient;
    public static IUserRepository UserRepository;
    public static IUserCommandRepository UserCommandRepository;
    public static IItemRepository ItemRepository;
    public static IUserItemRepository UserItemRepository;
    public static IEquippedItemRepository EquippedItemRepository;
    public static IGuildCountRepository GuildCountRepository;
    public static NoobDbContext Db;

    public static void Initialize()
    {
        var bill = new User(1);
        var ted = new User(2);
        UserPermissions = new Dictionary<ulong, ulong>
        {
            { bill.Id, ulong.MaxValue},
            { ted.Id, ulong.MaxValue}
        };
        SocketClient = new SocketClientStub(UserPermissions);

        var billDaily = new UserCommand(bill, 1);
        var tedWeekly = new UserCommand(ted, 2);

        var stick = new Item
        {
            Id = 1,
            Name = "Stick",
            Description = "A wooden stick.",
            Price = 10,
            Level = 1,
            Attack = 1,
            SlotId = ItemSlot.MainHand.Id
        };

        var shield = new Item
        {
            Id = 2,
            Name = "Shield",
            Description = "Blocks some stuff.",
            Price = 50,
            Level = 2,
            Defense = 1,
            SlotId = ItemSlot.OffHand.Id
        };

        var crowbar = new Item
        {
            Id = 3,
            Name = "Crowbar",
            Description = "Wacks some stuff.",
            Price = 100,
            Level = 3,
            Attack = 3,
            SlotId = ItemSlot.MainHand.Id
        };

        var slippers = new Item
        {
            Id = 4,
            Name = "Slippers",
            Description = "Makes you sneaky.",
            Price = 100,
            Level = 1,
            Sneak = 1,
            SlotId = ItemSlot.OffHand.Id
        };

        var mittens = new Item
        {
            Id = 5,
            Name = "Mittens",
            Description = "",
            Price = 100,
            Level = 1,
            Sneak = 1,
            SlotId = ItemSlot.Hands.Id
        };

        var hat = new Item
        {
            Id = 6,
            Name = "Hat",
            Description = "",
            Price = 100,
            Level = 1,
            Perception = 1,
            SlotId = ItemSlot.Head.Id
        };

        var tedStick = new UserItem(ted, stick);
        var billMittens = new UserItem(bill, mittens);
        var billSlippers = new UserItem(bill, slippers);

        var equippedTedStick = new EquippedItem(ted, stick);
        var equippedBillMittens = new EquippedItem(bill, mittens);
        var equippedBillSlippers = new EquippedItem(bill, slippers);

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
        Db.Items.Add(slippers);
        Db.Items.Add(mittens);
        Db.Items.Add(hat);
        Db.Items.Add(mittens);
        Db.UserItems.Add(tedStick);
        Db.UserItems.Add(billMittens);
        Db.UserItems.Add(billSlippers);
        Db.EquippedItems.Add(equippedTedStick);
        Db.EquippedItems.Add(equippedBillMittens);
        Db.EquippedItems.Add(equippedBillSlippers);
        Db.SaveChanges();

        UserRepository = new DbContextUserRepository(Db);
        UserCommandRepository = new DbContextUserCommandRepository(Db);
        ItemRepository = new DbContextItemRepository(Db);
        UserItemRepository = new DbContextUserItemRepository(Db);
        EquippedItemRepository = new DbContextEquippedItemRepository(Db, ItemRepository);
        GuildCountRepository = new DbContextGuildCountRepository(Db);

        BillDiscord = new DiscordUserStub(bill.Id, "Bill", "billy");
        TedDiscord = new DiscordUserStub(ted.Id, "Ted", "teddy");
    }
}
