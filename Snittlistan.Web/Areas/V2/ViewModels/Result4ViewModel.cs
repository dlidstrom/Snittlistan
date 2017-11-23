using System;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class Result4ViewModel
    {
        public Result4ViewModel(ResultHeaderViewModel headerViewModel, ResultSeries4ReadModel resultReadModel)
        {
            if (headerViewModel == null) throw new ArgumentNullException(nameof(headerViewModel));
            if (resultReadModel == null) throw new ArgumentNullException(nameof(resultReadModel));
            HeaderViewModel = headerViewModel;
            ResultReadModel = resultReadModel;
        }

        public ResultHeaderViewModel HeaderViewModel { get; private set; }

        public ResultSeries4ReadModel ResultReadModel { get; private set; }
    }
}