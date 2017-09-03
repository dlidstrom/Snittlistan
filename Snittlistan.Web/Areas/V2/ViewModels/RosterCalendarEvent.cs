using System;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class RosterCalendarEvent
    {
        public RosterCalendarEvent(Roster roster, Player[] players)
        {
            Id = roster.Id;
            Date = roster.Date;
            Team = roster.Team;
            Opponent = roster.Opponent;
            Location = roster.Location;
            Description = string.Empty;
            const string TextLineFeed = @"\" + "n";
            if (players.Length >= 8)
            {
                Description = "1 " + players[0].Nickname + " " + players[1].Nickname
                              + TextLineFeed + "2 " + players[2].Nickname + " " + players[3].Nickname
                              + TextLineFeed + "3 " + players[4].Nickname + " " + players[5].Nickname
                              + TextLineFeed + "4 " + players[6].Nickname + " " + players[7].Nickname;
                if (players.Length >= 9)
                {
                    Description += TextLineFeed + "R " + players[8].Nickname;
                    if (players.Length >= 10)
                    {
                        Description += TextLineFeed + "R " + players[9].Nickname;
                    }
                }
            }
            else if (players.Length == 4)
            {
                Description = "1 " + players[0].Nickname
                              + TextLineFeed + "2 " + players[1].Nickname
                              + TextLineFeed + "3 " + players[2].Nickname
                              + TextLineFeed + "4 " + players[3].Nickname;
            }
        }

        public string Id { get; private set; }
        public DateTime Date { get; private set; }
        public string Team { get; private set; }
        public string Opponent { get; private set; }
        public string Location { get; private set; }
        public string Description { get; private set; }
    }
}