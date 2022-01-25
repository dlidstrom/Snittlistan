using Snittlistan.Web.Areas.V2.Domain;
using Raven.Client.Indexes;

namespace Snittlistan.Web.Areas.V2.Indexes;
public class OneTimeTokenIndex : AbstractIndexCreationTask<OneTimeToken>
{
    public OneTimeTokenIndex()
    {
        Map = tokens => from token in tokens
                        select new
                        {
                            token.OneTimeKey,
                            token.PlayerId,
                            token.CreatedDate
                        };
    }
}
