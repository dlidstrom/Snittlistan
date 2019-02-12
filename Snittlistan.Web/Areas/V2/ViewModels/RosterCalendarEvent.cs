namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using Snittlistan.Web.Areas.V2.Domain;
    using Snittlistan.Web.Areas.V2.ReadModels;

    public class RosterCalendarEvent
    {
        private const string TextLineFeed = @"\" + "n";

        public RosterCalendarEvent(Roster roster, ResultHeaderReadModel resultHeaderReadModel)
        {
            Id = roster.Id;
            Date = roster.Date;
            Team = roster.Team;
            Opponent = roster.Opponent;
            Location = roster.Location;
            Description = string.Empty;
            if (resultHeaderReadModel != null)
            {
                Description = resultHeaderReadModel.MatchCommentary
                    + TextLineFeed
                    + TextLineFeed
                    + string.Join(TextLineFeed + TextLineFeed, resultHeaderReadModel.BodyText);
            }
        }

        public string Id { get; }
        public DateTime Date { get; }
        public string Team { get; }
        public string Opponent { get; }
        public string Location { get; }
        public string Description { get; }
    }
}