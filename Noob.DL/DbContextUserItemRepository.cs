using Noob.Core.Models;

namespace Noob.DL;

public class DbContextUserItemRepository : IUserItemRepository
{
    private NoobDbContext Db;
    public DbContextUserItemRepository(NoobDbContext db) => Db = db;

    public IEnumerable<UserItem> FindAll(ulong userId) =>
        Db.UserItems.Where(userItem => userItem.UserId == userId);
    
    public UserItem Find(ulong userId, int itemId) =>
        Db.UserItems.FirstOrDefault(userItem =>
            userItem.UserId == userId && userItem.ItemId == itemId);

    public UserItem Create(User user, Item item)
    {
        var userItem = new UserItem { UserId = user.Id, ItemId = item.Id };
        Db.UserItems.Add(userItem);
        Db.SaveChanges();
        return userItem;
    }

}

