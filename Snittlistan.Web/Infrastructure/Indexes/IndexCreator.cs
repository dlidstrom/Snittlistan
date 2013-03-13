using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using Raven.Client;
using Raven.Client.Indexes;

namespace Snittlistan.Web.Infrastructure.Indexes
{
    public static class IndexCreator
    {
        public static void CreateIndexes(IDocumentStore store)
        {
            var indexes = from type in Assembly.GetExecutingAssembly().GetTypes()
                          where
                              type.IsSubclassOf(typeof(AbstractIndexCreationTask))
                          select type;

            var typeCatalog = new TypeCatalog(indexes.ToArray());
            IndexCreation.CreateIndexes(new CompositionContainer(typeCatalog), store);
        }
    }
}