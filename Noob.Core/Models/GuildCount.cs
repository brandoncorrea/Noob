using System;
namespace Noob.Core.Models;

public class GuildCount
{
    public ulong GuildId { get; set; }
    public ulong ChannelId { get; set; }
    public ulong LastUserId { get; set; }
    public int Current { get; set; }
    public int Record { get; set; }
}
