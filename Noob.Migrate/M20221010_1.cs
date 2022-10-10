using Noob.Core.Models;
using Noob.DL;

namespace Noob.Migrate;

/// <summary>
/// Add items to the Items table
/// </summary>
public class M20221010_1 : IMigration
{
    public void Migrate(NoobDbContext db)
    {
        var items = new Item[]
        {
            new Item
            {
                Id = 1,
                Name = "Crowbar",
                Level = 3,
                Price = 1000,
                Description = ""
            },
            new Item
            {
                Id = 2,
                Name = "Stick",
                Level = 1,
                Price = 100,
                Description = ""
            },
            new Item
            {
                Id = 3,
                Name = "Propeller Hat",
                Level = 1,
                Price = 100,
                Description = ""
            },
            new Item
            {
                Id = 4,
                Name = "Groucho Glasses",
                Level = 3,
                Price = 1000,
                Description = ""
            },
            new Item
            {
                Id = 5,
                Name = "Mittens",
                Level = 1,
                Price = 100,
                Description = ""
            },
            new Item
            {
                Id = 6,
                Name = "Bunny Slippers",
                Level = 1,
                Price = 100,
                Description = ""
            }
        };

        db.Items.AddRange(items);
        db.SaveChanges();
    }
}

