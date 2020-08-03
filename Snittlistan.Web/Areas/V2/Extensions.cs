namespace Snittlistan.Web.Areas.V2
{
    using System;

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
    }
}