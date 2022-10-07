using System;
using Noob.API.Models;

namespace Noob.API.Repositories
{
    public interface IUserRepository
    {
        User Find(int id);
        void Update(User user);
    }
}
