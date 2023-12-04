using Discord.WebSocket;
using MediatR;

namespace WardenBot.Discord.DiscordEvents
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DiscordEventAttribute(string eventName) : Attribute
    {
        public string EventName { get; set; } = eventName;
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class DiscordEventRegistrationAttribute() : Attribute
    {
    }
}
