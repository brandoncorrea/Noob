using System;
using System.Runtime.ConstrainedExecution;
using Microsoft.EntityFrameworkCore;
using Noob.API.Models;

namespace Noob.API.Repositories
{
    public class SqlLiteDbContext : NoobDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionbuilder) =>
            optionbuilder.UseSqlite(@"Data Source=./db/noob.db");
    }
}
