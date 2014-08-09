using System;
using System.Collections.Generic;
using EventStoreLite;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class TeamOfWeek : IReadModel
    {
        public TeamOfWeek(int bitsMatchId, int season, int turn, string team, string teamLevel)
        {
            PlayerScores = new Dictionary<string, PlayerScore>();
            Id = IdFromBitsMatchId(bitsMatchId);
            Season = season;
            Turn = turn;
            Team = team;
            TeamLevel = teamLevel;
        }

        public string Id { get; private set; }

        public Dictionary<string, PlayerScore> PlayerScores { get; private set; }

        public int Season { get; private set; }

        public string Team { get; private set; }

        public string TeamLevel { get; private set; }

        public int Turn { get; private set; }

        public static string IdFromBitsMatchId(int bitsMatchId)
        {
            return "TeamOfWeek-" + bitsMatchId;
        }

        public void AddResultForPlayer(Player player, int score, int pins)
        {
            if (PlayerScores.ContainsKey(player.Id) == false)
            {
                PlayerScores.Add(player.Id, new PlayerScore(player.Id, player.Name, Team, TeamLevel));
            }

            var playerScore = PlayerScores[player.Id];
            playerScore.Score += score;
            playerScore.Pins += pins;
            playerScore.Series++;
        }

        public void AddMedal(AwardedMedalReadModel awardedMedal)
        {
            if (PlayerScores.ContainsKey(awardedMedal.Player) == false)
                throw new ApplicationException(string.Format("No player with id {0} found", awardedMedal.Player));

            PlayerScores[awardedMedal.Player].AddMedal(awardedMedal);
        }

        public void ClearMedals()
        {
            foreach (var key in PlayerScores.Keys)
            {
                PlayerScores[key].ClearMedals();
            }
        }
    }
}