using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class RegisterSerie
    {
        public RegisterSerie(ResultSeriesReadModel.Serie serie, List<SelectListItem> players)
        {
            if (serie == null) throw new ArgumentNullException("serie");
            if (players == null) throw new ArgumentNullException("players");
            Serie = serie;
            Players = players;
        }

        public ResultSeriesReadModel.Serie Serie { get; set; }

        public List<SelectListItem> Players { get; set; }
    }
}