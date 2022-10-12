using Noob.Core.Models;

namespace Noob.DL;

public interface IItemRepository
{
    IEnumerable<Item> FindAll();
    Item Find(int id);
    Item Find(UserItem userItem) => Find(userItem.ItemId);
    Item Find(string id) =>
        int.TryParse(id, out int itemId)
        ? Find(itemId)
        : null;

    Item Delete(Item item);
    Item Save(Item item);
}

