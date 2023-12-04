using Discord;
using Discord.WebSocket;
using MediatR;
using System.Reflection;
using System.Reflection.Emit;
using WardenBot.Discord.DiscordEvents;

namespace WardenBot.Discord.BackgroundServices;

public class DiscordBackgroundService : BackgroundService
{
    private readonly ILogger logger;
    private readonly DiscordSocketClient discordSocketClient;
    private readonly IMediator mediator;
    
    public DiscordBackgroundService(
        ILogger<DiscordBackgroundService> logger,
        DiscordSocketClient discordSocketClient,
        IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;

        this.discordSocketClient = discordSocketClient;
        this.discordSocketClient.Log += DiscordSocketClient_Log;

        RegisterEvents();
    }

    private Task DiscordSocketClient_Log(LogMessage discordLogMessage)
    {
        LogLevel logLevel = discordLogMessage.Severity switch
        {
            LogSeverity.Critical => LogLevel.Critical,
            LogSeverity.Error => LogLevel.Error,
            LogSeverity.Warning => LogLevel.Warning,
            LogSeverity.Info => LogLevel.Information,
            LogSeverity.Verbose => LogLevel.Debug,
            LogSeverity.Debug => LogLevel.Trace,
            _ => LogLevel.Trace,
        };

        this.logger.Log(logLevel, discordLogMessage.Exception, "[{source}] {message}", discordLogMessage.Source, discordLogMessage.Message);

        return Task.CompletedTask;
    }

    private void RegisterEvents()
    {
        List<Type> notificationTypes = typeof(Program).Assembly.GetTypes()
            .Where(type => type.GetCustomAttribute<DiscordEventAttribute>() != null)
            .ToList();

        foreach (var notificationType in notificationTypes)
        {
            DiscordEventAttribute? discordEventAttribute 
                = notificationType.GetCustomAttribute<DiscordEventAttribute>();
            if (discordEventAttribute is null 
                || string.IsNullOrWhiteSpace(discordEventAttribute.EventName)) 
            { 
                // TODO: Error
                continue; 
            }

            MethodInfo? discordEventRegistrationMethod
                = notificationType.GetMethods()
                .Where(meth => meth.GetCustomAttribute<DiscordEventRegistrationAttribute>() != null)
                .FirstOrDefault();
            if (discordEventRegistrationMethod is null)
            {
                // TODO: Error
                continue;
            }

            discordEventRegistrationMethod.Invoke(null, [this.discordSocketClient, this.mediator]);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await this.discordSocketClient.LoginAsync(TokenType.Bot, Config.DISCORD_TOKEN);
        await this.discordSocketClient.StartAsync();
        await Task.Delay(Timeout.Infinite, cancellationToken);
        await this.discordSocketClient.StopAsync();
    }
}
