namespace Snittlistan.Web.Areas.V2.Domain
{
    using System.Text.RegularExpressions;

    public static class NameShortener
    {
        private static readonly Regex ForenameSurnameRegex = new Regex(@"(?<Forename1>\w+)(?<Forename2>-\w+)? (?<Surname>\w+)");

        public static string ShortenName(string name)
        {
            if (name == null)
                return string.Empty;

            var shortenedName = name;
            var match = ForenameSurnameRegex.Match(shortenedName);
            if (match.Groups["Forename2"].Success)
            {
                shortenedName = $"{match.Groups["Forename1"].Value.Substring(0, 1)}-{match.Groups["Forename2"].Value.Substring(1, 1)}. {match.Groups["Surname"].Value}";
            }
            else
            {
                shortenedName = $"{match.Groups["Forename1"].Value.Substring(0, 1)}. {match.Groups["Surname"].Value}";
            }

            return shortenedName;
        }
    }
}