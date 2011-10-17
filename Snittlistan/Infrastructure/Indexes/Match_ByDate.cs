using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Snittlistan.Models;

namespace Snittlistan.Infrastructure.Indexes
{
	public class Match_ByDate : AbstractIndexCreationTask<Match>
	{
		public Match_ByDate()
		{
			Map = matches => from match in matches
							 select new { match.Date };
		}
	}
}