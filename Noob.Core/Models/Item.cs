namespace Noob.Core.Models;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public int Level { get; set; }

    public Item SetDescription(string description)
    {
        Description = description;
        return this;
    }

    public Item SetPrice(int price)
    {
        Price = price;
        return this;
    }
}
