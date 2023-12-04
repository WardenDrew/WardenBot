using IronWatch.MediatR.MinimalEndpoints;
using MediatR;

namespace WardenBot.Discord.Endpoints.Discord.Oauth
{
    [Get]
    public class Callback : IRequest<IResult>
    {
        public class Handler : IRequestHandler<Callback, IResult>
        {
            public Task<IResult> Handle(Callback request, CancellationToken cancellationToken)
            {
                return Task.FromResult(Results.Ok());
            }
        }
    }
}
