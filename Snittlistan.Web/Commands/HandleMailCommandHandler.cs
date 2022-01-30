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
    public CompositionRoot CompositionRoot { get; set; } = null!;

    public ILogger Logger { get; set; } = NullLogger.Instance;

    public async Task Handle(HandlerContext<TCommand> context)
    {
        string key = GetKey(context.Payload);
        KeyValueProperty? rateLimitProperty = await CompositionRoot.Databases.Snittlistan.KeyValueProperties
            .SingleOrDefaultAsync(x => x.Key == key);
        if (rateLimitProperty == null)
        {
            (int rate, int perSeconds) = GetRate(context.Payload);
            rateLimitProperty = CompositionRoot.Databases.Snittlistan.KeyValueProperties.Add(
                new(key, new RateLimit(key, 1, rate, perSeconds)));
        }

        DateTime now = DateTime.Now;
        RateLimit rateLimit = (RateLimit)rateLimitProperty.Value;
        rateLimit.UpdateAllowance(now);
        if (rateLimit.Allowance < 1)
        {
            rateLimitProperty.SetValue(rateLimit);
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
        rateLimit.DecreaseAllowance(now);
        rateLimitProperty.SetValue(rateLimit);
        int changesSaved = await CompositionRoot.Databases.Snittlistan.SaveChangesAsync();
        if (changesSaved > 0)
        {
            Logger.InfoFormat(
                "saved {changesSaved} to database",
                changesSaved);
        }

        await CompositionRoot.EmailService.SendAsync(email);
    }

    protected abstract Task<TEmail> CreateEmail(HandlerContext<TCommand> context);

    protected abstract string GetKey(TCommand command);

    protected abstract RatePerSeconds GetRate(TCommand command);

    protected record RatePerSeconds(int Rate, int PerSeconds);
}
