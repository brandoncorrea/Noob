using System;
using Noob.Core.Models;

namespace Noob.DL;

public class DbContextItemRepository : IItemRepository
{
    private NoobDbContext Db;
    public DbContextItemRepository(NoobDbContext db) => Db = db;

    public Item Delete(Item item)
    {
        var removed = Db.Items.Remove(item).Entity;
        Db.SaveChanges();
        return removed;
    }

    public Item Save(Item item)
    {
        var found = Find(item.Id);
        Item saved;
        if (found == null)
            saved = Db.Items.Add(item).Entity;
        else
            saved = Db.Items.Update(item).Entity;
        Db.SaveChanges();
        return saved;
    }

    public Item Find(int id) =>
        Db.Items.FirstOrDefault(item => item.Id == id);

    public IEnumerable<Item> FindAll() => Db.Items;
}

