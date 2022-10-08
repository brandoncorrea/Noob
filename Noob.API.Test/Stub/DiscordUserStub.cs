using System;
using Discord;

namespace Noob.API.Test.Stub
{
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

        public DiscordUserStub()
        {

        }

        public DiscordUserStub(ulong id, string username)
        {
            Id = id;
            Username = username;
        }

        public Task<IDMChannel> CreateDMChannelAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            throw new NotImplementedException();
        }

        public string GetDefaultAvatarUrl()
        {
            throw new NotImplementedException();
        }
    }
}

