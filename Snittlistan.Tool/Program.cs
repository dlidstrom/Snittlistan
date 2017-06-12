using Raven.Abstractions.Smuggler;
using Raven.Client.Embedded;
using Raven.Database.Smuggler;

namespace Snittlistan.Tool
{
    class Program
    {
        static void Main(string[] args)
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
                    BackupPath = "raven.dump"
                });
            }
        }
    }
}
