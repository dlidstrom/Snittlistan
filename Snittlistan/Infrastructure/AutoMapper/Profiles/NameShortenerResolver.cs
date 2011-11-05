using System.Text.RegularExpressions;
using AutoMapper;
using Snittlistan.Models;

namespace Snittlistan.Infrastructure.AutoMapper.Profiles
{
    public class NameShortenerResolver : ValueResolver<Game, string>
    {
        private static readonly Regex regex = new Regex(@"(?<Forename1>\w+)(?<Forename2>-\w+)? (?<Surname>\w+)");

        protected override string ResolveCore(Game source)
        {
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