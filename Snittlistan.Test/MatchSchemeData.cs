using NUnit.Framework;

namespace Snittlistan.Test;
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
