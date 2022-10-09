using System;
using Microsoft.EntityFrameworkCore;
using Noob.API.Models;
using Noob.API.Repositories;

namespace Noob.API.Test.Stub
{
    public class InMemoryNoobDbContext : NoobDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder) =>
            optionbuilder.UseInMemoryDatabase("InMemoryNoobs");
    }
}
