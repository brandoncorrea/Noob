using Noob.Core.Models;

namespace Noob.DL;

public class DbContextUserItemRepository : IUserItemRepository
{
    private NoobDbContext Db;
    public DbContextUserItemRepository(NoobDbContext db) => Db = db;

    public UserItem Find(User user, Item item) =>
        Db.UserItems.FirstOrDefault(userItem =>
            userItem.UserId == user.Id && userItem.ItemId == item.Id);

    public UserItem Create(User user, Item item)
    {
        var userItem = new UserItem { UserId = user.Id, ItemId = item.Id };
        Db.UserItems.Add(userItem);
        Db.SaveChanges();
        return userItem;
    }
}

