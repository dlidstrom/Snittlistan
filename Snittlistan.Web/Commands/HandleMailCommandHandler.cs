#nullable enable

using Castle.Core.Logging;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;
using System.Data.Entity;

namespace Snittlistan.Web.Commands;

public abstract class HandleMailCommandHandler<TCommand, TEmail>
    : ICommandHandler<TCommand>
    where TEmail : EmailBase
{
    private readonly int rate;
    private readonly int perSeconds;

    protected HandleMailCommandHandler(int rate, int perSeconds)
    {
        this.rate = rate;
        this.perSeconds = perSeconds;
    }

    public CompositionRoot CompositionRoot { get; set; } = null!;

    public ILogger Logger { get; set; } = NullLogger.Instance;

    public async Task Handle(HandlerContext<TCommand> context)
    {
        string key = GetKey(context.Payload);
        RateLimit? rateLimit = await CompositionRoot.Databases.Snittlistan.RateLimits
            .SingleOrDefaultAsync(x => x.Key == key);
        if (rateLimit == null)
        {
            rateLimit = CompositionRoot.Databases.Snittlistan.RateLimits.Add(
                new(key, 1, rate, perSeconds));
        }

        rateLimit.UpdateAllowance(DateTime.Now);
        if (rateLimit.Allowance < 1)
        {
            throw new HandledException($"allowance = {rateLimit.Allowance:N2}, wait to reach 1");
        }

        TEmail email = await CreateEmail(context);
        await CompositionRoot.EmailService.SendAsync(email);
        rateLimit.DecreaseAllowance();
    }

    protected abstract Task<TEmail> CreateEmail(HandlerContext<TCommand> context);

    protected abstract string GetKey(TCommand command);
}
