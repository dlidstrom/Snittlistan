using System;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using EventStoreLite;
using NLog;
using Raven.Client;
using Raven.Client.Indexes;

namespace Snittlistan.Web.Infrastructure.Indexes
{
    public static class IndexCreator
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public static void ResetIndexes(IDocumentStore store, EventStore eventStore)
        {
            if (store == null) throw new ArgumentNullException(nameof(store));
            if (eventStore == null) throw new ArgumentNullException(nameof(eventStore));
            while (true)
            {
                var indexNames = store.DatabaseCommands.GetIndexNames(0, 20);
                foreach (var indexName in indexNames)
                {
                    if (string.Equals(indexName, "Raven/DocumentsByEntityName", StringComparison.OrdinalIgnoreCase) == false)
                    {
                        Log.Info("Deleting index {0}", indexName);
                        store.DatabaseCommands.DeleteIndex(indexName);
                    }
                }

                if (indexNames.Length <= 1) break;
            }

            // create indexes
            CreateIndexes(store);
            eventStore.Initialize(store);
        }

        public static void CreateIndexes(IDocumentStore store)
        {
            var indexesQuery = from type in Assembly.GetExecutingAssembly().GetTypes()
                               where type.IsSubclassOf(typeof(AbstractIndexCreationTask))
                                     && type.Namespace != null
                                     && type.Namespace.StartsWith("EventStore") == false
                               select type;

            var indexes = indexesQuery.ToArray();
            foreach (var index in indexes)
            {
                Log.Info("Creating index {0}", index);
            }

            var typeCatalog = new TypeCatalog(indexes);
            IndexCreation.CreateIndexes(new CompositionContainer(typeCatalog), store);
        }
    }
}