using System;
using Discord;
using Noob.API.Models;
using Noob.API.Repositories;

namespace Noob.API.Test.Stub
{
    public static class Noobs
    {
        public static User Bill => UserRepository.Find(1);
        public static User Ted => UserRepository.Find(2);
        public static DiscordUserStub BillDiscord;
        public static DiscordUserStub TedDiscord;
        public static UserCommand BillDaily => UserCommandRepository.Find(1, 1);
        public static UserCommand TedWeekly => UserCommandRepository.Find(2, 2);
        public static IUserRepository UserRepository;
        public static IUserCommandRepository UserCommandRepository;
        public static NoobDbContext Db = new InMemoryNoobDbContext();

        public static void Initialize()
        {
            var bill = new User { Id = 1 };
            var ted = new User { Id = 2 };

            var billDaily = new UserCommand { UserId = bill.Id, CommandId = 1 };
            var tedWeekly = new UserCommand { UserId = ted.Id, CommandId = 2 };

            Db = new InMemoryNoobDbContext();
            Db.Database.EnsureDeleted();
            Db.Database.EnsureCreated();
            Db.Users.Add(bill);
            Db.Users.Add(ted);
            Db.UserCommands.Add(billDaily);
            Db.UserCommands.Add(tedWeekly);
            Db.SaveChanges();

            UserRepository = new DbContextUserRepository(Db);
            UserCommandRepository = new DbContextUserCommandRepository(Db);

            BillDiscord = new DiscordUserStub(bill.Id, "Bill", "billy");
            TedDiscord = new DiscordUserStub(ted.Id, "Ted", "teddy");
        }
    }
}
