using System;
using Noob.API.Models;

namespace Noob.API.Repositories
{
    public interface IUserCommandRepository
    {
        UserCommand Find(int userId, int commandId);
        void Create(UserCommand command);
        void Update(UserCommand command);
    }
}
