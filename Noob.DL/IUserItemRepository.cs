using Noob.Core.Models;
namespace Noob.DL;

public interface IUserItemRepository
{
    IEnumerable<UserItem> FindAll(ulong userId);
    UserItem Find(ulong userId, int itemId);
    UserItem Create(User user, Item item);
    UserItem Find(User user, Item item) => Find(user.Id, item.Id);
    bool Exists(User user, Item item) => Find(user, item) != null;
}