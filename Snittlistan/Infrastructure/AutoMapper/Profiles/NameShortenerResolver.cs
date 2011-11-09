namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    using System.Text.RegularExpressions;
    using Snittlistan.Models;

    public class NameShortenerResolver : global::AutoMapper.ValueResolver<Game, string>
    {
        private static readonly Regex regex = new Regex(@"(?<Forename1>\w+)(?<Forename2>-\w+)? (?<Surname>\w+)");

        protected override string ResolveCore(Game source)
        {
            if (source == null || source.Player == null)
                return string.Empty;

            string shortenedName = source.Player;
            var match = regex.Match(shortenedName);
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