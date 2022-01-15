#nullable enable

namespace Snittlistan.Web.Areas.V2;

public static class Extensions
{
    public static DateTime ToDateTime(this string s, int matchTime)
    {
        string yearPart = s.Substring(0, 4);
        string monthPart = s.Substring(5, 2);
        string dayPart = s.Substring(8, 2);
        DateTime result = new DateTime(
            int.Parse(yearPart),
            int.Parse(monthPart),
            int.Parse(dayPart)).AddHours(matchTime / 100).AddMinutes(matchTime % 100);
        return result;
    }

    public static int EditDistanceTo(this string s, string t)
    {
        int n = s.Length;
        int m = t.Length;

        // Verify arguments.
        if (n == 0)
        {
            return m;
        }

        if (m == 0)
        {
            return n;
        }

        // Initialize arrays.
        int[,] d = new int[n + 1, m + 1];
        for (int i = 0; i <= n; d[i, 0] = i++)
        {
        }

        for (int j = 0; j <= m; d[0, j] = j++)
        {
        }

        // Begin looping.
        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                // Compute cost.
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(
                Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                d[i - 1, j - 1] + cost);
            }
        }

        // Return cost.
        return d[n, m];
    }
}
