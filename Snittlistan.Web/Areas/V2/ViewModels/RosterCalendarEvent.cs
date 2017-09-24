using System;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class RosterCalendarEvent
    {
        const string TextLineFeed = @"\" + "n";

        public RosterCalendarEvent(Roster roster, Player[] players, Player teamLeader, ResultHeaderReadModel resultHeaderReadModel)
        {
            Id = roster.Id;
            Date = roster.Date;
            Team = roster.Team;
            Opponent = roster.Opponent;
            Location = roster.Location;
            Description = string.Empty;
            if (resultHeaderReadModel == null)
            {
                GetPlayersDescription(roster, players, teamLeader);
            }
            else
            {
                Description = resultHeaderReadModel.MatchCommentary
                    + TextLineFeed
                    + TextLineFeed
                    + string.Join(TextLineFeed + TextLineFeed, resultHeaderReadModel.BodyText);
            }
        }

        private void GetPlayersDescription(Roster roster, Player[] players, Player teamLeader)
        {
            if (roster.Preliminary == false)
            {
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

            if (teamLeader != null)
            {
                Description += TextLineFeed + "LL " + teamLeader.Nickname;
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