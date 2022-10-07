using System;
using Noob.API.Models;
using Noob.API.Repositories;

namespace Noob.API.Test.Stub
{
    public class UserRepositoryStub : IUserRepository
    {
        private IEnumerable<User> Users;
        public UserRepositoryStub(IEnumerable<User> users) => Users = users;
        public User Find(int id) => Users.FirstOrDefault(i => i.Id == id);
        public void Update(User user)
        {
            User found = Find(user.Id);
            found.BrowniePoints = user.BrowniePoints;
            found.Niblets = user.Niblets;
        }
    }
}
