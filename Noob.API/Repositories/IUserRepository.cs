using System;
using Noob.API.Models;

namespace Noob.API.Repositories
{
    public interface IUserRepository
    {
        User Find(ulong id);
        User Save(User user);
    }
}
