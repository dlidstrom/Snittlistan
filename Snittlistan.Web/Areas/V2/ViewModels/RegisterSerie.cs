﻿using System.Web.Mvc;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels;
// TODO Delete
public class RegisterSerie
{
    public RegisterSerie(ResultSeriesReadModel.Serie serie, List<SelectListItem> players)
    {
        Serie = serie ?? throw new ArgumentNullException(nameof(serie));
        Players = players ?? throw new ArgumentNullException(nameof(players));
    }

    public ResultSeriesReadModel.Serie Serie { get; private set; }

    public List<SelectListItem> Players { get; private set; }
}
