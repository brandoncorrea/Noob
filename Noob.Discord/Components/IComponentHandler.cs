using System;
using Discord;

namespace Noob.Discord.Components;

public interface IComponentHandler
{
    public string CustomId { get; }
    Task HandleAsync(IComponentInteraction interaction);
}
