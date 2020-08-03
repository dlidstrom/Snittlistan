namespace Snittlistan.Test
{
    using System.Collections.Generic;
    using NUnit.Framework;

    public partial class MatchSchemeData
    {
        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return Fif;
                yield return Vartan;
            }
        }
    }
}