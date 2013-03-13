using System;
using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class ResultViewModel
    {
        public ResultViewModel(ResultHeaderReadModel headerReadModel, ResultReadModel resultReadModel)
        {
            if (headerReadModel == null) throw new ArgumentNullException("headerReadModel");
            if (resultReadModel == null) throw new ArgumentNullException("resultReadModel");
            HeaderReadModel = headerReadModel;
            ResultReadModel = resultReadModel;
        }

        public ResultHeaderReadModel HeaderReadModel { get; private set; }

        public ResultReadModel ResultReadModel { get; private set; }
    }
}