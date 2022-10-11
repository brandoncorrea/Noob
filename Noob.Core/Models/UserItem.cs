namespace Noob.Core.Models;

public class UserItem
{
    public ulong UserId { get; set; }
    public int ItemId { get; set; }

    public UserItem() { }

    public UserItem(User user, Item item)
    {
        UserId = user.Id;
        ItemId = item.Id;
    }
}
