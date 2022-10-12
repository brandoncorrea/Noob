using System;
using Noob.Core.Models;

namespace Noob.DL
{
    public interface IEquippedItemRepository
    {
        void Equip(User user, Item item);
        void Unequip(User user, Item item);
        IEnumerable<EquippedItem> FindByUser(User user);
        IEnumerable<Item> EquippedItems(User user);
        bool IsSlotted(User user, int slotId);
        int DefenseBonus(User user) => EquippedItems(user).Sum(i => i.Defense);
        int AttackBonus(User user) => EquippedItems(user).Sum(i => i.Attack);
        int SneakBonus(User user) => EquippedItems(user).Sum(i => i.Sneak);
        int PerceptionBonus(User user) => EquippedItems(user).Sum(i => i.Perception);
    }
}
