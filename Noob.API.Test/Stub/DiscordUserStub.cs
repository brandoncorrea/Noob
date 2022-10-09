using Discord;
namespace Noob.Discord.Test.Stub;

public class DiscordUserStub : IUser
{
    public string AvatarId { get; set; }
    public string Discriminator { get; set; }
    public ushort DiscriminatorValue { get; set; }
    public bool IsBot { get; set; }
    public bool IsWebhook { get; set; }
    public string Username { get; set; }
    public UserProperties? PublicFlags { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public ulong Id { get; set; }
    public string Mention { get; set; }
    public UserStatus Status { get; set; }
    public IReadOnlyCollection<ClientType> ActiveClients { get; set; }
    public IReadOnlyCollection<IActivity> Activities { get; set; }

    // Custom Properties
    public string AvatarUrl { get; set; }

    public DiscordUserStub()
    {

    }

    public DiscordUserStub(ulong id, string username, string avatarUrl)
    {
        Id = id;
        Username = username;
        AvatarUrl = avatarUrl;
    }

    public Task<IDMChannel> CreateDMChannelAsync(RequestOptions options = null)
    {
        throw new NotImplementedException();
    }

    public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128) =>
            AvatarUrl != null ? $"http://localhost/{AvatarUrl}_{size}.{format}/" : null;
    public string GetDefaultAvatarUrl() => "http://localhost/defaultAvatar.jpg/";
}
