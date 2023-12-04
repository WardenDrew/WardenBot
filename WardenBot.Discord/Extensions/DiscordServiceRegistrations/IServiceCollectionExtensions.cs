using Discord;
using Discord.WebSocket;
using WardenBot.Discord.BackgroundServices;

namespace WardenBot.Discord.Extensions.DiscordServiceRegistrations
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscord(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig()
                {
                    AlwaysDownloadUsers = true,
                    GatewayIntents = GatewayIntents.All,
                    LogLevel = LogSeverity.Info
                }))
                .AddHostedService<DiscordBackgroundService>();
        }
    }
}
