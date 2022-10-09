using System;
namespace Noob.API.Models
{
    public class UserCommand
    {
        public ulong UserId { get; set; }
        public int CommandId { get; set; }
        public DateTime ExecutedAt { get; set; }

        public UserCommand SetExecutedAt(DateTime executedAt)
        {
            ExecutedAt = executedAt;
            return this;
        }
    }
}
