namespace Noob.Core.Models;

public class UserCommand
{
    public ulong UserId { get; set; }
    public int CommandId { get; set; }
    public DateTime ExecutedAt { get; set; }

    public UserCommand() { }

    public UserCommand(User user, int commandId)
    {
        UserId = user.Id;
        CommandId = commandId;
    }

    public UserCommand SetExecutedAt(DateTime executedAt)
    {
        ExecutedAt = executedAt;
        return this;
    }
}
