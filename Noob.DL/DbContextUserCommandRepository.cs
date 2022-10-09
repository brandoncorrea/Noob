using Noob.Core.Models;
namespace Noob.DL;

public class DbContextUserCommandRepository : IUserCommandRepository
{
    NoobDbContext Db;
    public DbContextUserCommandRepository(NoobDbContext db) => Db = db;

    public UserCommand Delete(ulong userId, int commandId)
    {
        var command = Find(userId, commandId);
        if (command == null) return null;
        var deleted = Db.UserCommands.Remove(command).Entity;
        Db.SaveChanges();
        return deleted;
    }

    public UserCommand Find(ulong userId, int commandId) =>
        Db.UserCommands.FirstOrDefault(c =>
            c.UserId == userId && c.CommandId == commandId);

    public void Save(UserCommand command)
    {
        if (Find(command.UserId, command.CommandId) == null)
            Db.UserCommands.Add(command);
        else
            Db.UserCommands.Update(command);
        Db.SaveChanges();
    }
}
