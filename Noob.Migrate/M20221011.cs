using Microsoft.EntityFrameworkCore;
using Noob.Core.Models;
using Noob.DL;

namespace Noob.Migrate;

/// <summary>
/// Create new EquippedItems table and Update Items table
/// Add attribute bonuses to existing items
/// </summary>
public class M20221011 : IMigration
{
    const string EquippedItemsTableSql =
        "CREATE TABLE \"EquippedItems\" (" +
        "    \"UserId\" INTEGER NOT NULL," +
        "    \"SlotId\" INTEGER NOT NULL," +
        "    \"ItemId\" INTEGER NOT NULL," +
        "    CONSTRAINT \"PK_EquippedItems\" PRIMARY KEY (\"UserId\", \"SlotId\")" +
        ");" +
        "ALTER TABLE Items ADD COLUMN SlotId INTEGER NOT NULL DEFAULT 0;" +
        "ALTER TABLE Items ADD COLUMN Attack INTEGER NOT NULL DEFAULT 0;" +
        "ALTER TABLE Items ADD COLUMN Defense INTEGER NOT NULL DEFAULT 0;" +
        "ALTER TABLE Items ADD COLUMN Sneak INTEGER NOT NULL DEFAULT 0;" +
        "ALTER TABLE Items ADD COLUMN Perception INTEGER NOT NULL DEFAULT 0;";

    public void Migrate(NoobDbContext db)
    {
            db.Database.ExecuteSqlRaw(EquippedItemsTableSql);

            var crowbar = db.Items.First(i => i.Id == 1);
            var stick = db.Items.First(i => i.Id == 2);
            var hat = db.Items.First(i => i.Id == 3);
            var glasses = db.Items.First(i => i.Id == 4);
            var mittens = db.Items.First(i => i.Id == 5);
            var slippers = db.Items.First(i => i.Id == 6);

            crowbar.Attack = 3;
            stick.Attack = 1;
            hat.Perception = 1;
            glasses.Sneak = 3;
            mittens.Sneak = 1;
            slippers.Sneak = 1;

            db.Update(crowbar);
            db.Update(stick);
            db.Update(hat);
            db.Update(glasses);
            db.Update(mittens);
            db.Update(slippers);
            db.SaveChanges();
    }
}
