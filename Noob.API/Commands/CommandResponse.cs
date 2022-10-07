using System;
namespace Noob.API.Commands
{
    public class CommandResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static CommandResponse Fail(string message) => new CommandResponse
        {
            Message = message
        };

        public static CommandResponse Ok(string message) => new CommandResponse
        {
            Success = true,
            Message = message
        };
    }
}
