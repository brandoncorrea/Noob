using System;
using Newtonsoft.Json;
using Noob.Core.Models;
using Noob.DL;
namespace Noob.Migrate;

/// <summary>
/// Exchange JSON Database for SqlLite Database
/// </summary>
public class M20221009 : IMigration
{
    public void Migrate(NoobDbContext db)
    {
        db.Users.AddRange(GetJsonCollection<User>("./db/user.json"));
        db.UserCommands.AddRange(GetJsonCollection<UserCommand>("./db/userCommand.json"));
        db.SaveChanges();
    }

    private static IEnumerable<T> GetJsonCollection<T>(string path) =>
        JsonConvert.DeserializeObject<IEnumerable<T>>(File.ReadAllText(path));
}
