using IronWatch.MediatR.MinimalEndpoints;
using MediatR;

namespace WardenBot.Discord.Endpoints.Discord.Oauth
{
    [Get]
    public class Start : IRequest<IResult>
    {
        public class Handler : IRequestHandler<Start, IResult>
        {
            public Task<IResult> Handle(Start request, CancellationToken cancellationToken)
            {
                return Task.FromResult(
                    Results.Redirect(
                        $"{Config.OAUTH_URI}?client_id={Config.CLIENT_ID}&scope={Config.OAUTH_SCOPE}&permissions={Config.OAUTH_PERMISSIONS}&redirect_uri={Config.OAUTH_REDIRECT_URI}"));
            }
        }
    }
}
