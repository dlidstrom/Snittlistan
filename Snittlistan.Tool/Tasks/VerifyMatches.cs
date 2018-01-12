using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using log4net;
using Raven.Abstractions;
using Raven.Client.Document;
using Raven.Client.Linq;
using Snittlistan.Queue;
using Snittlistan.Queue.Messages;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Helpers;

namespace Snittlistan.Tool.Tasks
{
    public class VerifyMatches : ICommandLineTask
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Run(string[] args)
        {
            var connectionStringsWithUrl = new List<(string connectionStringName, Uri)>();
            CommandLineTaskHelper.ForAllConnectionStrings(((ConnectionStringSettings cs, Uri uri) tuple) =>
            {
                connectionStringsWithUrl.Add((connectionStringName: tuple.cs.Name, tuple.uri));
            });

            foreach (var (connectionStringName, url) in connectionStringsWithUrl)
            {
                Log.Info($"Opening {connectionStringName}");
                using (var documentStore = new DocumentStore
                {
                    ConnectionStringName = connectionStringName
                }.Initialize(true))
                {
                    MsmqGateway.BeginTransaction();
                    using (var session = documentStore.OpenSession())
                    {
                        var season = session.LatestSeasonOrDefault(SystemTime.UtcNow.Year);
                        var rosters = session.Query<Roster, RosterSearchTerms>()
                                             .Where(x => x.Season == season)
                                             .ToArray();
                        foreach (var roster in rosters)
                        {
                            if (roster.IsVerified)
                            {
                                Log.Info($"Skipping {roster.BitsMatchId} because it is already verified.");
                            }
                            else if (roster.BitsMatchId == 0)
                            {
                                Log.Info($"Skipping {roster.Team}-{roster.Opponent} (turn={roster.Turn}) because it has no BitsMatchId.");
                            }
                            else if (roster.MatchResultId == null)
                            {
                                Log.Info($"Skipping {roster.BitsMatchId} because it has no result yet.");
                            }
                            else
                            {
                                Log.Info($"Need to verify {roster.BitsMatchId}");
                                var message = new VerifyMatchMessage(roster.BitsMatchId, roster.Id);
                                var envelope = new MessageEnvelope(message, url);
                                MsmqGateway.PublishMessage(envelope);
                            }
                        }

                        session.SaveChanges();
                    }

                    MsmqGateway.CommitTransaction();
                }
            }
        }

        public string HelpText => "Verifies matches";
    }
}