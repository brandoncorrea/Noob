using System;
namespace Noob.API.Models
{
    public class User
    {
        public ulong Id { get; set; }
        public int BrowniePoints { get; set; }
        public int Niblets { get; set; }
        public long Experience { get; set; }
        public long Level => Experience / 100 + 1;
    }
}
