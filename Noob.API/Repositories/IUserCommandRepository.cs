using System;
using Noob.API.Models;

namespace Noob.API.Repositories;

public interface IUserCommandRepository
{
    UserCommand Find(ulong userId, int commandId);
    void Save(UserCommand command);
    UserCommand Delete(ulong userId, int commandId);
    UserCommand Delete(UserCommand command) => Delete(command.UserId, command.CommandId);
}
