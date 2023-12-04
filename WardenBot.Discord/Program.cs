using WardenBot.Discord;
using WardenBot.Discord.Extensions.DiscordServiceRegistrations;
using IronWatch.MediatR.MinimalEndpoints;
using WardenBot.Discord.Endpoints;

// Build the app container
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Services.AddMediatR(conf =>
{
    conf.RegisterServicesFromAssemblyContaining<Program>();
});

builder.Services.AddDiscord();

builder.Services.AddRegisteredEndpointTracker();

WebApplication app = builder.Build();

// Get logging instance
ILogger logger = app.Services.GetRequiredService<ILogger<Program>>();

// Validate (and optionally load) ENV
logger.LogInformation("Validating environment variables");
if (!Config.Validate(out List<string> missingVariables))
{
    logger.LogCritical("Missing required enviornment variables: {missing}", missingVariables);
    return;
}

app.BuildEndpoints(typeof(RootEndpointsMarker));

// Run app
app.Run();
