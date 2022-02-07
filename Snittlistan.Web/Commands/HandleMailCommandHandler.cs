#nullable enable

using Castle.Core.Logging;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;
using System.Data.Entity;
using System.Web.Caching;

namespace Snittlistan.Web.Commands;

public abstract class HandleMailCommandHandler<TCommand, TEmail>
    : ICommandHandler<TCommand>
    where TEmail : EmailBase
{
    public CompositionRoot CompositionRoot { get; set; } = null!;

    public ILogger Logger { get; set; } = NullLogger.Instance;

    public async Task Handle(HandlerContext<TCommand> context)
    {
        (string key, int rate, int perSeconds) = GetRate(context);

        // check cache
        if (CurrentHttpContext.Instance().Cache.Get(key) is not RateLimit cacheItem)
        {
            cacheItem = new(key, 1, rate, perSeconds);
            CurrentHttpContext.Instance().Cache.Insert(
                key,
                cacheItem,
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Default,
                (key, item, reason) =>
                    Logger.InfoFormat(
                        "cache item '{key}' removed due to '{reason}'",
                        key,
                        reason));
        }

        // check database
        KeyValueProperty? rateLimitProperty = await CompositionRoot.Databases.Snittlistan.KeyValueProperties
            .SingleOrDefaultAsync(x => x.Key == key);
        if (rateLimitProperty == null)
        {
            KeyValueProperty keyValueProperty = new(
                context.Tenant.TenantId,
                key,
                new RateLimit(key, 1, rate, perSeconds));
            rateLimitProperty =
                CompositionRoot.Databases.Snittlistan.KeyValueProperties.Add(keyValueProperty);
        }

        DateTime now = DateTime.Now;
        rateLimitProperty.ModifyValue<RateLimit>(x => x.UpdateAllowance(now));
        cacheItem.UpdateAllowance(now);
        double allowance = rateLimitProperty.GetValue<RateLimit, double>(x => x.Allowance);
        if (allowance < 1 || cacheItem.Allowance < 1)
        {
            string message =
                $"(db) allowance = {allowance:N2}, (cache) allowance = {cacheItem.Allowance:N2}, wait to reach 1";
            throw new HandledException(message);
        }

        int changesSaved = await CompositionRoot.Databases.Snittlistan.SaveChangesAsync();
        if (changesSaved > 0)
        {
            Logger.InfoFormat(
                "saved {changesSaved} to database",
                changesSaved);
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
        rateLimitProperty.ModifyValue<RateLimit>(x => x.DecreaseAllowance(now));
        cacheItem.DecreaseAllowance(now);
    }

    protected abstract Task<TEmail> CreateEmail(HandlerContext<TCommand> context);

    protected abstract RatePerSeconds GetRate(HandlerContext<TCommand> context);

    protected record RatePerSeconds(string Key, int Rate, int PerSeconds);
}
