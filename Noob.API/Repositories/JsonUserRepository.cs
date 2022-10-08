using System;
using Newtonsoft.Json;
using Noob.API.Models;

namespace Noob.API.Repositories
{
    public class JsonUserRepository : IUserRepository
    {
        private string FilePath { get; set; }
        private IEnumerable<User> Users;

        public JsonUserRepository(string filePath)
        {
            FilePath = filePath;
            Users = JsonConvert.DeserializeObject<IEnumerable<User>>(File.ReadAllText(FilePath));
            if (Users == null)
                Users = new List<User>();
        }

        public User Find(ulong id) => Users.FirstOrDefault(user => user.Id == id);

        public User Save(User user)
        {
            Users = Users.Where(u => u.Id != user.Id).Append(user);
            Commit();
            return user;
        }

        public User Delete(ulong id)
        {
            var found = Find(id);
            if (found == null) return null;
            Users = Users.Where(u => u.Id != id);
            Commit();
            return found;
        }

        private void Commit() =>
            File.WriteAllText(FilePath, JsonConvert.SerializeObject(Users));
    }
}
