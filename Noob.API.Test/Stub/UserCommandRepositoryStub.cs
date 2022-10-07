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

        public UserCommand Find(ulong userId, int commandId) =>
            UserCommands.FirstOrDefault(command =>
                command.UserId == userId && command.CommandId == commandId);

        public void Save(UserCommand command) =>
            UserCommands = UserCommands
                .Where(c =>
                    c.CommandId != command.CommandId ||
                    c.UserId != command.UserId)
                .Append(command);
    }
}
