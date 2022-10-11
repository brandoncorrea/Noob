using System;
namespace Noob.Core.Models;

public class EquippedItem
{
    public int ItemId { get; set; }
    public ulong UserId { get; set; }
    public int SlotId { get; set; }

    public EquippedItem() { }

    public EquippedItem(User user, Item item)
    {
        ItemId = item.Id;
        SlotId = item.SlotId;
        UserId = user.Id;
    }
}
