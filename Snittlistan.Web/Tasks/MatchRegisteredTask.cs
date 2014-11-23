using System;

namespace Snittlistan.Web.Tasks
{
    public class MatchRegisteredTask
    {
        public MatchRegisteredTask(string subject, string team, string opponent, int score, int opponentScore)
        {
            if (subject == null) throw new ArgumentNullException("subject");
            if (team == null) throw new ArgumentNullException("team");
            if (opponent == null) throw new ArgumentNullException("opponent");
            Subject = subject;
            Team = team;
            Opponent = opponent;
            Score = score;
            OpponentScore = opponentScore;
        }

        public string Subject { get; private set; }

        public string Team { get; private set; }

        public string Opponent { get; private set; }

        public int Score { get; private set; }

        public int OpponentScore { get; private set; }
    }
}