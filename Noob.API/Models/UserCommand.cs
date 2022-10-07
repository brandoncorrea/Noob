using System;
namespace Noob.API.Models
{
    public class UserCommand
    {
        public int UserId { get; set; }
        public int CommandId { get; set; }
        public DateTime ExecutedAt { get; set; }
    }
}
