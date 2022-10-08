using System;
using Noob.API.Models;
using Noob.API.Repositories;

namespace Noob.API.Test.Stub
{
    public class UserRepositoryStub : IUserRepository
    {
        private IEnumerable<User> Users;
        public UserRepositoryStub(IEnumerable<User> users) => Users = users;

        public User Create(ulong id)
        {
            var user = new User { Id = id };
            Users = Users.Append(user);
            return user;
        }

        public User Delete(ulong id)
        {
            var user = Find(id);
            Users = Users.Where(u => u.Id != id);
            return user;
        }

        public User Find(ulong id) => Users.FirstOrDefault(i => i.Id == id);
        public User Save(User user)
        {
            Users = Users.Where(u => u.Id != user.Id).Append(user);
            return user;
        }
    }
}
