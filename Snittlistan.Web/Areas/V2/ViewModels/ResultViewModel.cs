﻿using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class ResultViewModel
{
    public ResultViewModel(ResultHeaderViewModel headerViewModel, ResultSeriesReadModel resultReadModel)
    {
        HeaderViewModel = headerViewModel ?? throw new ArgumentNullException(nameof(headerViewModel));
        ResultReadModel = resultReadModel ?? throw new ArgumentNullException(nameof(resultReadModel));
    }

    public ResultHeaderViewModel HeaderViewModel { get; private set; }

    public ResultSeriesReadModel ResultReadModel { get; private set; }
}
