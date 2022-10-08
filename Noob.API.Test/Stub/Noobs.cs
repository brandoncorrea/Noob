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
        public static IUser BillDiscord;
        public static IUser TedDiscord;
        public static UserCommand BillDaily => UserCommandRepository.Find(1, 1);
        public static UserCommand TedWeekly => UserCommandRepository.Find(2, 2);
        public static IUserRepository UserRepository;
        public static IUserCommandRepository UserCommandRepository;

        public static void Initialize()
        {
            var bill = new User { Id = 1 };
            var ted = new User { Id = 2 };
            UserRepository = new UserRepositoryStub(new List<User> { bill, ted });

            var billDaily = new UserCommand { UserId = bill.Id, CommandId = 1 };
            var tedWeekly = new UserCommand { UserId = ted.Id, CommandId = 2 };
            UserCommandRepository = new UserCommandRepositoryStub(new List<UserCommand> { billDaily, tedWeekly });

            BillDiscord = new DiscordUserStub(bill.Id, "Bill");
            TedDiscord = new DiscordUserStub(ted.Id, "Ted");
        }
    }
}

