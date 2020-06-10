namespace Snittlistan.Web.Areas.V2.Domain.Match
{
    using System;

    public class MatchException : Exception
    {
        public MatchException(string message)
            : base(message)
        {
        }
    }
}