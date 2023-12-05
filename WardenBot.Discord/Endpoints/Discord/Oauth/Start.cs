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
                    Results.Redirect(Config.GenerateOAuthUri()));
            }
        }
    }
}
