using System;
using System.Collections.Generic;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class RegisterSerie
    {
        public RegisterSerie(
            string aggregateId, string rosterId, int bitsMatchId, List<string> players, ResultReadModel.Serie serie)
        {
            if (players == null) throw new ArgumentNullException("players");
            if (serie == null) throw new ArgumentNullException("serie");
            Players = players;
            Serie = serie;
        }

        public List<string> Players { get; set; }

        public ResultReadModel.Serie Serie { get; set; }
    }
}