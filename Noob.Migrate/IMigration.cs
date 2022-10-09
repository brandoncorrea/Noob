using System;
using Noob.DL;
namespace Noob.Migrate;

public interface IMigration
{
    void Migrate(NoobDbContext db);
}
