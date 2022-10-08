using System;
using Noob.API.Models;

namespace Noob.API.Repositories
{
    public interface IUserRepository
    {
        User Find(ulong id);
        User Save(User user);
        User Delete(ulong id);

        User Delete(User user) => Delete(user.Id);
        User Reload(User user) => Find(user.Id);
        User FindOrCreate(ulong id)
        {
            User user = Find(id);
            if (user == null)
                user = Save(new User { Id = id });
            return user;
        }
    }
}
