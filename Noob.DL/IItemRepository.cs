using Noob.Core.Models;

namespace Noob.DL;

public interface IItemRepository
{
    IEnumerable<Item> FindAll();
    Item Find(int id);
    Item Delete(Item item);
    Item Save(Item item);
}

