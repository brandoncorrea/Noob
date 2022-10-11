using System;
using Noob.Core.Models;

namespace Noob.DL
{
    public interface IEquippedItemRepository
    {
        void Equip(User user, Item item);
        IEnumerable<EquippedItem> FindByUser(User user);
        IEnumerable<Item> EquippedItems(User user);
        bool IsSlotted(User user, int slotId);
    }
}
