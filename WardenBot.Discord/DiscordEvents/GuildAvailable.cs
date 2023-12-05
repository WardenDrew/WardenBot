using Discord;
using Discord.WebSocket;
using MediatR;

namespace WardenBot.Discord.DiscordEvents
{
    [DiscordEvent(nameof(DiscordSocketClient.GuildAvailable))]
    public class GuildAvailable : INotification
    {
        public required SocketGuild Guild { get; set; }

        [DiscordEventRegistration]
        public static void Register(DiscordSocketClient discordSocketClient, IMediator mediator)
        {
            discordSocketClient.GuildAvailable += async (SocketGuild guild) =>
            {
                await mediator.Publish(new GuildAvailable()
                {
                    Guild = guild
                });
            };
        }

        public class Handler : INotificationHandler<GuildAvailable>
        {
            private readonly DiscordSocketClient discordSocketClient;

            public Handler(DiscordSocketClient discordSocketClient)
            {
                this.discordSocketClient = discordSocketClient;
            }
            
            public async Task Handle(GuildAvailable notification, CancellationToken cancellationToken)
            {
                if (Convert.ToUInt64(Config.OAUTH_PERMISSIONS) != notification.Guild.GetUser(Convert.ToUInt64(Config.CLIENT_ID)).GuildPermissions.RawValue)
                {
                    ComponentBuilder builder = new ComponentBuilder()
                        .WithButton(
                            label: $"Re-Authorize for {notification.Guild.Name}",
                            style: ButtonStyle.Link,
                            url: Config.GenerateOAuthUri(notification.Guild.Id.ToString()));

                    await notification.Guild.Owner.SendMessageAsync(
                        text: $"Hey, the permissions that I use have been updated and need to be regranted for your server: {notification.Guild.Name} [{notification.Guild.Id}]. Please use the following link to re-grant permissions!",
                        components: builder.Build());
                }
            }
        }
    }
}
