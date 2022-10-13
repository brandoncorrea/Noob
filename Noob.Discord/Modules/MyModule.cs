using Discord;
using Discord.Interactions;
using Noob.DL;

namespace Noob.Discord.Modules;

/// <summary>
/// Keeping this for example code
/// Delete this once a slash command is implemented as a module
/// </summary>
public class MyModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly IUserRepository UserRepository;

    public MyModule(IUserRepository userRepository)
    {
        userRepository = UserRepository;
    }

    [SlashCommand("ping", "Receive a ping message!")]
    public async Task ThingsAsync(int value)
    {
        await RespondAsync($"Pong {value}");
    }

    [SlashCommand("sling", "Sling message!")]
    public async Task SlingAsync(IUser user)
    {
        await RespondAsync($"Slinging Stuff {user.Username}");
    }
}
