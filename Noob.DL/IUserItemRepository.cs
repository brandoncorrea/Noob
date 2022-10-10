using Noob.Core.Models;
namespace Noob.DL;

public interface IUserItemRepository
{
    UserItem Find(User user, Item item);
    UserItem Create(User user, Item item);
    bool Exists(User user, Item item) => Find(user, item) != null;
}