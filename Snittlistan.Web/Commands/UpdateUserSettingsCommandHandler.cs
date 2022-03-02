#nullable enable

using Snittlistan.Model;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;

namespace Snittlistan.Web.Commands;

public class UpdateUserSettingsCommandHandler
    : CommandHandler<UpdateUserSettingsCommandHandler.Command>
{
    public override async Task Handle(HandlerContext<Command> context)
    {
        KeyValueProperty? settingsProperty =
            await CompositionRoot.Databases.Snittlistan.KeyValueProperties.SingleOrDefaultAsync(
                x => x.Key == context.Payload.Key
                    && x.TenantId == CompositionRoot.CurrentTenant.TenantId);

        if (settingsProperty == null)
        {
            settingsProperty = CompositionRoot.Databases.Snittlistan.KeyValueProperties.Add(
                new(
                    CompositionRoot.CurrentTenant.TenantId,
                    context.Payload.Key,
                    new UserSettings(
                        context.Payload.RosterMailEnabled,
                        context.Payload.AbsenceMailEnabled,
                        context.Payload.MatchResultMailEnabled)));
        }
        else
        {
            settingsProperty.ModifyValue<UserSettings>(
                x => x with
                {
                    RosterMailEnabled = context.Payload.RosterMailEnabled,
                    AbsenceMailEnabled = context.Payload.AbsenceMailEnabled,
                    MatchResultMailEnabled = context.Payload.MatchResultMailEnabled
                },
                x => Logger.InfoFormat("before: {@x}", x),
                x => Logger.InfoFormat("after: {@x}", x));
        }
    }

    public record Command(
        string Key,
        bool RosterMailEnabled,
        bool AbsenceMailEnabled,
        bool MatchResultMailEnabled);
}
