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

        public EquippedItem ForSlot(User user, int slotId) =>
            Db.EquippedItems.FirstOrDefault(i =>
                i.UserId == user.Id && i.SlotId == slotId);

        public void Equip(User user, Item item)
        {
            var equippedItem = new EquippedItem(user, item);
            var existing = ForSlot(user, item.SlotId);
            if (existing != null)
                Db.EquippedItems.Remove(existing);
            Db.EquippedItems.Add(equippedItem);
            Db.SaveChanges();
        }

        public IEnumerable<Item> EquippedItems(User user) =>
            Db.EquippedItems
                .Where(item => item.UserId == user.Id)
                .AsEnumerable()
                .Select(item => ItemRepository.Find(item.ItemId))
                .Where(item => item != null);

        public void Unequip(User user, Item item)
        {
            var equipped = Db.EquippedItems.FirstOrDefault(i =>
                i.ItemId == item.Id &&
                i.SlotId == item.SlotId &&
                i.UserId == user.Id);
            if (equipped == null) return;
            Db.EquippedItems.Remove(equipped);
            Db.SaveChanges();
        }

        public bool IsEquipped(User user, Item item) =>
            Db.EquippedItems.FirstOrDefault(i =>
                i.UserId == user.Id && i.ItemId == item.Id) != null;
    }
}

