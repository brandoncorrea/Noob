using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Noob.API.Models;

namespace Noob.API.Repositories;

public class DbContextUserRepository : IUserRepository
{
    private NoobDbContext Db;
    public DbContextUserRepository(NoobDbContext db) => Db = db;

    public User Delete(ulong id)
    {
        var user = Find(id);
        if (user == null) return null;
        user = Db.Users.Remove(user).Entity;
        Db.SaveChanges();
        return user;
    }

    public User Find(ulong id) =>
        Db.Users.FirstOrDefault(user => user.Id == id);

    public User Save(User user)
    {
        var saved = Find(user.Id) != null
            ? Db.Users.Update(user).Entity
            : Db.Users.Add(user).Entity;
        Db.SaveChanges();
        return saved;
    }
}
