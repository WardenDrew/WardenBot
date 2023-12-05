using Discord;
using Discord.WebSocket;
using MediatR;

namespace WardenBot.Discord.DiscordEvents
{
    [DiscordEvent(nameof(DiscordSocketClient.JoinedGuild))]
    public class JoinedGuild : INotification
    {
        public required SocketGuild Guild { get; set; }

        [DiscordEventRegistration]
        public static void Register(DiscordSocketClient discordSocketClient, IMediator mediator)
        {
            discordSocketClient.JoinedGuild += async (SocketGuild guild) =>
            {
                await mediator.Publish(new JoinedGuild()
                {
                    Guild = guild
                });
            };
        }

        public class Handler : INotificationHandler<JoinedGuild>
        {
            public async Task Handle(JoinedGuild notification, CancellationToken cancellationToken)
            {
                await notification.Guild.SystemChannel.SendMessageAsync(
                    text: "Hi! I'm WardenBot. I'm looking forward to being a helpful part of your server. I'm in development right now so if you see something strange, feel free to let my creator `WardenDrew` know!");
            }
        }
    }
}
