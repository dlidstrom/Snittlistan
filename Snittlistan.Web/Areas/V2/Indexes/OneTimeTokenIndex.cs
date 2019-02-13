namespace Snittlistan.Web.Areas.V2.Indexes
{
    using System.Linq;
    using Domain;
    using Raven.Client.Indexes;

    public class OneTimeTokenIndex : AbstractIndexCreationTask<OneTimeToken>
    {
        public OneTimeTokenIndex()
        {
            Map = tokens => from token in tokens
                            select new
                            {
                                token.OneTimeKey
                            };
        }
    }
}