using Discord.WebSocket;
using MediatR;

namespace WardenBot.Discord.DiscordEvents
{
    [DiscordEvent(nameof(DiscordSocketClient.LeftGuild))]
    public class LeftGuild : INotification
    {
        public required SocketGuild Guild { get; set; }

        [DiscordEventRegistration]
        public static void Register(DiscordSocketClient discordSocketClient, IMediator mediator)
        {
            discordSocketClient.LeftGuild += async (SocketGuild guild) =>
            {
                await mediator.Publish(new LeftGuild()
                {
                    Guild = guild
                });
            };
        }

        public class Handler : INotificationHandler<LeftGuild>
        {
            public async Task Handle(LeftGuild notification, CancellationToken cancellationToken)
            {
                Console.WriteLine($"Left: {notification.Guild.Name}");
            }
        }
    }
}
