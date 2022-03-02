#nullable enable

using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Commands;

public class UpdateFeaturesCommandHandler : CommandHandler<UpdateFeaturesCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        KeyValueProperty? settingsProperty =
            await CompositionRoot.Databases.Snittlistan.KeyValueProperties.SingleOrDefaultAsync(
                x => x.Key == TenantFeatures.Key
                    && x.TenantId == CompositionRoot.CurrentTenant.TenantId);

        if (settingsProperty == null)
        {
            settingsProperty = CompositionRoot.Databases.Snittlistan.KeyValueProperties.Add(
                new(
                    CompositionRoot.CurrentTenant.TenantId,
                    TenantFeatures.Key,
                    new TenantFeatures(context.Payload.RosterMailEnabled)));
        }
        else
        {
            settingsProperty.ModifyValue<TenantFeatures>(
                x => x with
                {
                    RosterMailEnabled = context.Payload.RosterMailEnabled
                },
                x => Logger.InfoFormat("before: {@x}", x),
                x => Logger.InfoFormat("after: {@x}", x));
        }
    }

    public record Command(
        bool RosterMailEnabled);
}
