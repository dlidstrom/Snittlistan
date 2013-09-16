using System.Text.RegularExpressions;

namespace Snittlistan.Web.Infrastructure.AutoMapper.Profiles
{
    public static class NameShortener
    {
        private static readonly Regex ForenameSurnameRegex = new Regex(@"(?<Forename1>\w+)(?<Forename2>-\w+)? (?<Surname>\w+)");

        public static string Shorten(string name)
        {
            if (name == null)
                return string.Empty;

            string shortenedName = name;
            var match = ForenameSurnameRegex.Match(shortenedName);
            if (match.Groups["Forename2"].Success)
            {
                shortenedName = string.Format(
                    "{0}-{1}. {2}",
                    match.Groups["Forename1"].Value.Substring(0, 1),
                    match.Groups["Forename2"].Value.Substring(1, 1),
                    match.Groups["Surname"].Value);
            }
            else
            {
                shortenedName = string.Format(
                    "{0}. {1}",
                    match.Groups["Forename1"].Value.Substring(0, 1),
                    match.Groups["Surname"].Value);
            }

            return shortenedName;
        }
    }
}