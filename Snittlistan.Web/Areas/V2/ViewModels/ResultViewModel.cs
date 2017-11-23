using System;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class ResultViewModel
    {
        public ResultViewModel(ResultHeaderViewModel headerViewModel, ResultSeriesReadModel resultReadModel)
        {
            if (headerViewModel == null) throw new ArgumentNullException(nameof(headerViewModel));
            if (resultReadModel == null) throw new ArgumentNullException(nameof(resultReadModel));
            HeaderViewModel = headerViewModel;
            ResultReadModel = resultReadModel;
        }

        public ResultHeaderViewModel HeaderViewModel { get; private set; }

        public ResultSeriesReadModel ResultReadModel { get; private set; }
    }
}