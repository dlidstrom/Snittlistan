#nullable enable

using System.ComponentModel.Composition.Hosting;
using System.Reflection;
using EventStoreLite;
using NLog;
using Raven.Client;
using Raven.Client.Indexes;

namespace Snittlistan.Web.Infrastructure.Indexes;

public static class IndexCreator
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public static void ResetIndexes(IDocumentStore store, EventStore eventStore)
    {
        if (store == null)
        {
            throw new ArgumentNullException(nameof(store));
        }

        if (eventStore == null)
        {
            throw new ArgumentNullException(nameof(eventStore));
        }

        while (true)
        {
            string[] indexNames = store.DatabaseCommands.GetIndexNames(0, 20);
            foreach (string indexName in indexNames)
            {
                if (string.Equals(indexName, "Raven/DocumentsByEntityName", StringComparison.OrdinalIgnoreCase) == false)
                {
                    Log.Info("Deleting index {0}", indexName);
                    store.DatabaseCommands.DeleteIndex(indexName);
                }
            }

            if (indexNames.Length <= 1)
            {
                break;
            }
        }

        // create indexes
        CreateIndexes(store);
        _ = eventStore.Initialize(store);
    }

    public static void CreateIndexes(IDocumentStore store)
    {
        IEnumerable<Type> indexesQuery =
            from type in Assembly.GetExecutingAssembly().GetTypes()
            where type.IsSubclassOf(typeof(AbstractIndexCreationTask))
                    && type.Namespace != null
                    && type.Namespace.StartsWith("EventStore") == false
            select type;

        Type[] indexes = indexesQuery.ToArray();
        foreach (Type index in indexes)
        {
            Log.Info("Creating index {0}", index);
        }

        TypeCatalog typeCatalog = new(indexes);
        IndexCreation.CreateIndexes(new CompositionContainer(typeCatalog), store);
    }
}
