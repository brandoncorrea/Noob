using System;
using Noob.Core.Models;

namespace Noob.DL
{
    public class DbContextEquippedItemRepository : IEquippedItemRepository
    {
        private NoobDbContext Db;
        private IItemRepository ItemRepository;

        public DbContextEquippedItemRepository(
            NoobDbContext db,
            IItemRepository itemRepository)
        {
            Db = db;
            ItemRepository = itemRepository;
        }
        
        public IEnumerable<EquippedItem> FindByUser(User user) =>
            Db.EquippedItems.Where(item => item.UserId == user.Id);

        public bool IsSlotted(User user, int slotId) =>
            Db.EquippedItems.FirstOrDefault(i =>
                i.UserId == user.Id && i.SlotId == i.SlotId) != null;

        public void Equip(User user, Item item)
        {
            var equippedItem = new EquippedItem(user, item);
            if (IsSlotted(user, item.SlotId))
                Db.EquippedItems.Update(equippedItem);
            else
                Db.EquippedItems.Add(equippedItem);
        }

        public IEnumerable<Item> EquippedItems(User user) =>
            Db.EquippedItems
                .Where(item => item.UserId == user.Id)
                .AsEnumerable()
                .Select(item => ItemRepository.Find(item.ItemId))
                .Where(item => item != null);
    }
}

