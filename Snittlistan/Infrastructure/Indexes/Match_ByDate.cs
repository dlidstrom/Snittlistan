namespace Snittlistan.Infrastructure.Indexes
{
    using System.Linq;
    using Raven.Client.Indexes;
    using Snittlistan.Models;

    public class Match_ByDate : AbstractIndexCreationTask<Match>
    {
        public Match_ByDate()
        {
            Map = matches => from match in matches
                             select new { match.Date };
        }
    }
}