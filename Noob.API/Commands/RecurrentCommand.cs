using System;
using System.ComponentModel.Design;
using Noob.API.Extensions;
using Noob.API.Helpers;
using Noob.API.Models;
using Noob.API.Repositories;

namespace Noob.API.Commands
{
    public class RecurrentCommand
    {
        private const int DailyCommandId = 1;
        private IUserRepository UserRepository { get; set; }
        private IUserCommandRepository UserCommandRepository { get; set; }

        public RecurrentCommand(
            IUserRepository userRepository,
            IUserCommandRepository userCommandRepository)
        {
            UserRepository = userRepository;
            UserCommandRepository = userCommandRepository;
        }

        public CommandResponse Daily(ulong userId) =>
            Recurrent("daily", 1, 1, RandomNibletsDaily, userId);
        
        public CommandResponse Weekly(ulong userId) =>
            Recurrent("weekly", 7, 2, RandomNibletsWeekly, userId);

        private CommandResponse Recurrent(string kind, int interval, int commandId, Func<int> getNiblets, ulong userId)
        {
            User user = UserRepository.Find(userId);
            if (user == null)
                user = new User { Id = userId };

            UserCommand userCommand = UserCommandRepository.Find(user.Id, commandId);
            if (userCommand == null)
                CreateNewUserCommand(user, commandId);
            else if (userCommand.ExecutedAt.LessThanDaysAgo(interval))
                return CommandResponse.Fail($"Your {kind} reward will be ready in {Formatting.TimeFromNow(userCommand.ExecutedAt.AddDays(interval))}!");
            else
                ResetCommandTimestamp(userCommand);

            int newNiblets = getNiblets.Invoke();
            user.Niblets += newNiblets;
            UserRepository.Save(user);
            return CommandResponse.Ok($"You have redeemed your {kind} reward of {newNiblets} Niblets!");
        }

        private void ResetCommandTimestamp(UserCommand userCommand)
        {
            userCommand.ExecutedAt = DateTime.Now;
            UserCommandRepository.Save(userCommand);
        }

        private void CreateNewUserCommand(User user, int commandId) =>
            UserCommandRepository.Save(new UserCommand
            {
                UserId = user.Id,
                CommandId = commandId,
                ExecutedAt = DateTime.Now
            });

        private static int RandomNibletsDaily() => new Random().Next(1, 100);
        private static int RandomNibletsWeekly() => new Random().Next(100, 1000);
    }
}
