﻿namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Snittlistan.Web.Areas.V2.ReadModels;

    // TODO Delete
    public class RegisterSerie4
    {
        public RegisterSerie4(ResultSeries4ReadModel.Serie serie, List<SelectListItem> players)
        {
            Serie = serie ?? throw new ArgumentNullException(nameof(serie));
            Players = players ?? throw new ArgumentNullException(nameof(players));
        }

        public ResultSeries4ReadModel.Serie Serie { get; private set; }

        public List<SelectListItem> Players { get; private set; }
    }
}