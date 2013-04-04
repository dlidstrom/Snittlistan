using System;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class ResultViewModel
    {
        public ResultViewModel(ResultHeaderReadModel headerReadModel, ResultSeriesReadModel resultReadModel)
        {
            if (headerReadModel == null) throw new ArgumentNullException("headerReadModel");
            if (resultReadModel == null) throw new ArgumentNullException("resultReadModel");
            HeaderReadModel = headerReadModel;
            ResultReadModel = resultReadModel;
        }

        public ResultHeaderReadModel HeaderReadModel { get; private set; }

        public ResultSeriesReadModel ResultReadModel { get; private set; }
    }
}