﻿#nullable enable

namespace Snittlistan.Web.Areas.V2.Tasks
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Raven.Abstractions;
    using Snittlistan.Queue.Messages;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.Indexes;
    using Snittlistan.Web.Helpers;

    public class VerifyMatchesTaskHandler : TaskHandler<VerifyMatchesTask>
    {
        public override Task Handle(VerifyMatchesTask task)
        {
            int season = DocumentSession.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
            Roster[] rosters = DocumentSession.Query<Roster, RosterSearchTerms>()
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

                if (roster.IsVerified && task.Force == false)
                {
                    Log.Info($"Skipping {roster.BitsMatchId} because it is already verified.");
                }
                else
                {
                    VerifyMatchTask verifyTask = new(
                        roster.BitsMatchId,
                        roster.Id,
                        task.Force,
                        Trace.CorrelationManager.ActivityId);
                    toVerify.Add(verifyTask);
                }
            }

            foreach (VerifyMatchTask verifyMatchMessage in toVerify)
            {
                Log.Info($"Scheduling verification of {verifyMatchMessage.BitsMatchId}");
                PublishMessage(verifyMatchMessage);
            }

            return Task.CompletedTask;
        }
    }
}