using System;
using Newtonsoft.Json;
using Noob.API.Models;

namespace Noob.API.Repositories
{
    public class JsonUserCommandRepository : IUserCommandRepository
    {
        private string FilePath { get; set; }
        private IEnumerable<UserCommand> UserCommands { get; set; }

        public JsonUserCommandRepository(string filePath)
        {
            FilePath = filePath;
            UserCommands = JsonConvert.DeserializeObject<IEnumerable<UserCommand>>(File.ReadAllText(FilePath));
            if (UserCommands == null)
                UserCommands = new List<UserCommand>();
        }

        public UserCommand Find(ulong userId, int commandId) =>
            UserCommands.FirstOrDefault(c => c.UserId == userId && c.CommandId == commandId);

        public void Save(UserCommand command)
        {
            UserCommands = UserCommands
                .Where(c =>
                    c.CommandId != command.CommandId ||
                    c.UserId != command.UserId)
                .Append(command);
            Commit();
        }

        public UserCommand Delete(ulong userId, int commandId)
        {
            var command = Find(userId, commandId);
            if (command == null) return null;
            UserCommands = UserCommands
                .Where(c =>
                    c.CommandId != command.CommandId ||
                    c.UserId != command.UserId);
            Commit();
            return command;
        }

        private void Commit() =>
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(UserCommands));
    }
}

