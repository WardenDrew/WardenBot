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
            public async Task Handle(GuildAvailable notification, CancellationToken cancellationToken)
            {
                
            }
        }
    }
}
