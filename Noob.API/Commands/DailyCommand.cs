using System;
using Noob.API.Extensions;
using Noob.API.Helpers;
using Noob.API.Models;
using Noob.API.Repositories;

namespace Noob.API.Commands
{
    public class DailyCommand
    {
        private const int DailyCommandId = 1;
        private IUserRepository UserRepository { get; set; }
        private IUserCommandRepository UserCommandRepository { get; set; }

        public DailyCommand(
            IUserRepository userRepository,
            IUserCommandRepository userCommandRepository)
        {
            UserRepository = userRepository;
            UserCommandRepository = userCommandRepository;
        }

        public CommandResponse Execute(int userId)
        {
            User user = UserRepository.Find(userId);
            if (user == null)
                return CommandResponse.Fail("Your noob could not be found :(");

            UserCommand userCommand = UserCommandRepository.Find(user.Id, DailyCommandId);
            if (userCommand == null)
                CreateNewUserCommand(user);
            else if (userCommand.ExecutedAt.LessThanOneDayAgo())
                return CommandResponse.Fail($"Your daily will be ready in {Formatting.TimeFromNow(userCommand.ExecutedAt.AddDays(1))}!");
            else
                ResetCommandTimestamp(userCommand);

            user.Niblets += RandomNiblets();
            UserRepository.Update(user);

            return CommandResponse.Ok($"You have redeemed your daily reward of {user.Niblets} Niblets!");
        }

        private void ResetCommandTimestamp(UserCommand userCommand)
        {
            userCommand.ExecutedAt = DateTime.Now;
            UserCommandRepository.Update(userCommand);
        }

        private void CreateNewUserCommand(User user) =>
            UserCommandRepository.Create(new UserCommand
            {
                UserId = user.Id,
                CommandId = DailyCommandId,
                ExecutedAt = DateTime.Now
            });

        private static int RandomNiblets() => new Random().Next(1, 100);
    }
}
