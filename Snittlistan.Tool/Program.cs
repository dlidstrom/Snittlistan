using System;
using Raven.Abstractions.Smuggler;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Database.Smuggler;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;

namespace Snittlistan.Tool
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
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
                var documentStore = new DocumentStore { ConnectionStringName = connectionStringName };
                documentStore.Initialize();
                IndexCreator.CreateIndexes(documentStore);
                using (var documentSession = documentStore.OpenSession())
                {
                    documentSession.Store(new WebsiteConfig(new string[0], false));
                    documentSession.SaveChanges();
                }
            }
            else
            {
                Usage();
            }
        }

        private static void Usage()
        {
            Console.WriteLine("{0} /backup|/initialize", AppDomain.CurrentDomain.FriendlyName);
        }
    }
}
