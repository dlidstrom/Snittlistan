using System;
using System.Linq;
using Raven.Abstractions.Smuggler;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Database.Smuggler;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;
using Snittlistan.Web.Areas.V2.ReadModels;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;

namespace Snittlistan.Tool
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Usage();
                return;
            }

            if (args[0] == "/backup")
            {
                using (var documentStore = new EmbeddableDocumentStore
                {
                    DataDirectory = args[0]
                })
                {
                    documentStore.Initialize();
                    var dumper = new DataDumper(documentStore.DocumentDatabase, new SmugglerOptions());
                    dumper.ExportData(new SmugglerOptions
                    {
                        BackupPath = args[1]
                    });
                }
            }
            else if (args[0] == "/initialize")
            {
                Console.Write("Enter connection string name: ");
                var connectionStringName = Console.ReadLine();
                var documentStore = new DocumentStore { ConnectionStringName = connectionStringName }.Initialize();
                IndexCreator.CreateIndexes(documentStore);
                using (var documentSession = documentStore.OpenSession())
                {
                    if (documentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId) == null)
                    {
                        documentSession.Store(new WebsiteConfig(new WebsiteConfig.TeamNameAndLevel[0], false));
                        documentSession.SaveChanges();
                    }
                }
            }
            else if (args[0] == "/migrate")
            {
                string connectionStringName;
                if (args.Length == 1)
                {
                    Console.Write("Enter connection string name: ");
                    connectionStringName = Console.ReadLine();
                }
                else
                {
                    connectionStringName = args[1];
                }

                var documentStore = new DocumentStore
                {
                    ConnectionStringName = connectionStringName
                }.Initialize();

                Console.WriteLine("Migrating docs");
                var changed = 0;
                var skip = 0;
                while (true)
                {
                    using (var documentSession = documentStore.OpenSession())
                    {
                        var rosters = documentSession.Query<Roster, RosterSearchTerms>()
                                                     .Customize(x => x.WaitForNonStaleResultsAsOfNow())
                                                     .Skip(skip)
                                                     .Take(10)
                                                     .ToArray();
                        if (rosters.Length == 0) break;
                        foreach (var roster in rosters)
                        {
                            var id = TeamOfWeek.IdFromBitsMatchId(roster.BitsMatchId);
                            Console.WriteLine("Loading {0}", id);
                            var teamOfWeek = documentSession.Load<TeamOfWeek>(id);
                            if (teamOfWeek != null && teamOfWeek.RosterId == null)
                            {
                                teamOfWeek.RosterId = roster.Id;
                                changed++;
                            }
                        }

                        documentSession.SaveChanges();
                        skip += rosters.Length;
                    }
                }

                Console.WriteLine("Changed {0} team of weeks", changed);
            }
            else
            {
                Usage();
            }
        }

        private static void Usage()
        {
            Console.WriteLine("{0} /backup|/initialize|/migrate", AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
