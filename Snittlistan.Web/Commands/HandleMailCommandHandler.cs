#nullable enable

using Castle.Core.Logging;
using Elmah;
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

        DateTime now = DateTime.Now;
        rateLimit.UpdateAllowance(now);
        if (rateLimit.Allowance < 1)
        {
            throw new HandledException($"allowance = {rateLimit.Allowance:N2}, wait to reach 1");
        }

        TEmail email = await CreateEmail(context);
        EmailState state = email.State;
        _ = CompositionRoot.Databases.Snittlistan.SentEmails.Add(new(
            email.From,
            email.To,
            email.Bcc,
            email.Subject,
            state));
        await CompositionRoot.EmailService.SendAsync(email);
        rateLimit.DecreaseAllowance(now);
    }

    protected abstract Task<TEmail> CreateEmail(HandlerContext<TCommand> context);

    protected abstract string GetKey(TCommand command);
}
