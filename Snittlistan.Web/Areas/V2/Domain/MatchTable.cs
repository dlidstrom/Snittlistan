using System;
using System.Collections.Generic;
using Raven.Imports.Newtonsoft.Json;

namespace Snittlistan.Web.Areas.V2.Domain
{
    public class MatchTable
    {
        [JsonProperty(PropertyName = "Games")]
        private readonly List<MatchGame> games = new List<MatchGame>();

        public MatchTable(MatchGame game1, MatchGame game2, int score)
        {
            if (game1 == null) throw new ArgumentNullException("game1");
            if (game2 == null) throw new ArgumentNullException("game2");
            if (score != 0 && score != 1) throw new ArgumentOutOfRangeException("score", score, "Score out of range");
            this.Score = score;
            this.games = new List<MatchGame> { game1, game2 };
        }

        public int Score { get; private set; }

        public IEnumerable<MatchGame> Games
        {
            get
            {
                return this.games;
            }
        }
    }
}