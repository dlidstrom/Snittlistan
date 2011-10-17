using System.ComponentModel.Composition.Hosting;
using Raven.Client;
using Raven.Client.Indexes;

namespace Snittlistan.Infrastructure.Indexes
{
	public static class IndexCreator
	{
		public static void CreateIndexes(IDocumentStore store)
		{
			var typeCatalog = new TypeCatalog(typeof(Matches_PlayerStats), typeof(Match_ByDate));
			IndexCreation.CreateIndexes(new CompositionContainer(typeCatalog), store);
		}
	}
}