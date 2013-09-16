using System;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class Result4ViewModel
    {
        public Result4ViewModel(ResultHeaderReadModel headerReadModel, ResultSeries4ReadModel resultReadModel)
        {
            if (headerReadModel == null) throw new ArgumentNullException("headerReadModel");
            if (resultReadModel == null) throw new ArgumentNullException("resultReadModel");
            HeaderReadModel = headerReadModel;
            ResultReadModel = resultReadModel;
        }

        public ResultHeaderReadModel HeaderReadModel { get; private set; }

        public ResultSeries4ReadModel ResultReadModel { get; private set; }
    }
}