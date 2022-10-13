using System.Collections.ObjectModel;

namespace Noob.Core.Enums;

public class ItemSlot
{
    public readonly static ItemSlot MainHand = new ItemSlot(1, "Main Hand");
    public readonly static ItemSlot OffHand = new ItemSlot(2, "Off-Hand");
    public readonly static ItemSlot Head = new ItemSlot(3, "Head");
    public readonly static ItemSlot Torso = new ItemSlot(4, "Torso");
    public readonly static ItemSlot Legs = new ItemSlot(5, "Legs");
    public readonly static ItemSlot Hands = new ItemSlot(6, "Hands");
    public readonly static ItemSlot Feet = new ItemSlot(7, "Feet");
    public readonly static ItemSlot Back = new ItemSlot(8, "Back");
    public readonly static IReadOnlyDictionary<int, ItemSlot> Slots = new Dictionary<int, ItemSlot>
    {
        { MainHand.Id, MainHand },
        { OffHand.Id, OffHand },
        { Head.Id, Head },
        { Torso.Id, Torso },
        { Legs.Id, Legs },
        { Hands.Id, Hands },
        { Feet.Id, Feet },
        { Back.Id, Back },
    };

    public static ItemSlot Get(int slotId) => Slots[slotId];
    public static ItemSlot GetOrDefault(int slotId) => Slots.GetValueOrDefault(slotId);

    public static string NameOf(int slotId)
    {
        var slot = Slots.GetValueOrDefault(slotId);
        if (slot == null) return null;
        return slot.Name;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }

    public ItemSlot(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
