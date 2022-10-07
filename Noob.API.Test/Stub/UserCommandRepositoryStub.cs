using System;
using System.ComponentModel.Design;
using System.Linq;
using Noob.API.Models;
using Noob.API.Repositories;

namespace Noob.API.Test.Stub
{
    public class UserCommandRepositoryStub : IUserCommandRepository
    {
        private IEnumerable<UserCommand> UserCommands;

        public UserCommandRepositoryStub(IEnumerable<UserCommand> userCommands) =>
            UserCommands = userCommands;

        public UserCommandRepositoryStub() => UserCommands = new List<UserCommand>();

        public UserCommand Find(int userId, int commandId) =>
            UserCommands.FirstOrDefault(command =>
                command.UserId == userId && command.CommandId == commandId);

        public void Create(UserCommand command) =>
            UserCommands = UserCommands.Append(command);

        public void Update(UserCommand command) =>
            Find(command.UserId, command.CommandId).ExecutedAt = command.ExecutedAt;
    }
}
