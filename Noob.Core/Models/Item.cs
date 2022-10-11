namespace Noob.Core.Models;

public class Item
{
    public int Id { get; set; }
    public int SlotId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int Level { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Sneak { get; set; }
    public int Perception { get; set; }

    public Item SetAttack(int attack)
    {
        Attack = attack;
        return this;
    }

    public Item SetDefense(int defense)
    {
        Defense = defense;
        return this;
    }

    public Item SetDescription(string description)
    {
        Description = description;
        return this;
    }

    public Item SetPerception(int perception)
    {
        Perception = perception;
        return this;
    }

    public Item SetPrice(int price)
    {
        Price = price;
        return this;
    }

    public Item SetSneak(int sneak)
    {
        Sneak = sneak;
        return this;
    }
}
