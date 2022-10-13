using Noob.Core.Enums;
using Noob.Core.Models;
using Noob.DL;

namespace Noob.Migrate;

/// <summary>
/// Add items to the shop
/// Update Stick to be an off-hand weapon
/// </summary>
public class M20221013 : IMigration
{
    public void Migrate(NoobDbContext db)
    {
        var items = new Item[]
        {
            new Item
            {
                Attack = 1,
                Level = 1,
                Price = 100,
                SlotId = ItemSlot.MainHand.Id,
                Name = "Water Gun",
                Description = ""
            },
            new Item
            {
                Perception = 1,
                Level = 1,
                Price = 100,
                SlotId = ItemSlot.OffHand.Id,
                Name = "Glowstick",
                Description = ""
            },
            new Item
            {
                Defense = 1,
                Level = 1,
                Price = 100,
                SlotId = ItemSlot.Legs.Id,
                Name = "Diaper",
                Description = "Gives you a little extra cushion."
            },
            new Item
            {
                Defense = 1,
                Level = 1,
                Price = 100,
                SlotId = ItemSlot.Head.Id,
                Name = "Paper Bag",
                Description = "Protects you and your dignity."
            },
            new Item
            {
                Sneak = 2,
                Level = 2,
                Price = 500,
                SlotId = ItemSlot.Feet.Id,
                Name = "Sneakers",
                Description = "As the name implies, these help you sneak."
            },
            new Item
            {
                Attack = 2,
                Level = 2,
                Price = 500,
                SlotId = ItemSlot.MainHand.Id,
                Name = "Cardboard Tube",
                Description = ""
            },
            new Item
            {
                Perception = 2,
                Level = 2,
                Price = 500,
                SlotId = ItemSlot.OffHand.Id,
                Name = "Magnifying Glass",
                Description = ""
            },
        };

        var stick = db.Items.FirstOrDefault(i => i.Name == "Stick");
        stick.SlotId = ItemSlot.OffHand.Id;
        IEnumerable<EquippedItem> equippedItems = db.EquippedItems.Where(i => i.ItemId == stick.Id);
        equippedItems = equippedItems.Select(item =>
        {
            item.SlotId = stick.SlotId;
            return item;
        });

        db.Items.AddRange(items);
        db.Items.Update(stick);
        db.EquippedItems.UpdateRange(equippedItems);
        db.SaveChanges();
    }
}
