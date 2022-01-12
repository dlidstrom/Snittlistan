﻿#nullable enable

using Raven.Abstractions;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Helpers;
using Snittlistan.Web.Infrastructure;

namespace Snittlistan.Web.Commands;

public class VerifyMatchesCommandHandler : CommandHandler<VerifyMatchesCommandHandler.Command>
{
    public override Task Handle(HandlerContext<Command> context)
    {
        int season = CompositionRoot.DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
        Roster[] rosters = CompositionRoot.DocumentSession.Query<Roster, RosterSearchTerms>()
            .Where(x => x.Season == season)
            .ToArray();
        List<VerifyMatchTask> toVerify = new();
        foreach (Roster roster in rosters)
        {
            if (roster.BitsMatchId == 0)
            {
                continue;
            }

            if (roster.Date.ToUniversalTime() > SystemTime.UtcNow)
            {
                Log.Info($"Too early to verify {roster.BitsMatchId}");
                continue;
            }

            if (roster.IsVerified && context.Payload.Force == false)
            {
                Log.Info($"Skipping {roster.BitsMatchId} because it is already verified.");
            }
            else
            {
                VerifyMatchTask verifyTask = new(
                    roster.BitsMatchId,
                    roster.Id!,
                    context.Payload.Force);
                toVerify.Add(verifyTask);
            }
        }

        foreach (VerifyMatchTask verifyMatchMessage in toVerify)
        {
            Log.Info("Scheduling verification of {bitsMatchId}", verifyMatchMessage.BitsMatchId);
            context.PublishMessage(verifyMatchMessage);
        }

        return Task.CompletedTask;
    }

    public record Command(bool Force);
}