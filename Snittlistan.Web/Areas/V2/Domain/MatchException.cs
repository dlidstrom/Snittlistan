using System;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class MatchException : Exception
    {
        public MatchException(string message)
            : base(message)
        {
        }
    }
}