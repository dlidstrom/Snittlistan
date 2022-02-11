using Snittlistan.Web.Areas.V2.ReadModels;

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class Result4ViewModel
{
    public Result4ViewModel(ResultHeaderViewModel headerViewModel, ResultSeries4ReadModel resultReadModel)
    {
        HeaderViewModel = headerViewModel ?? throw new ArgumentNullException(nameof(headerViewModel));
        ResultReadModel = resultReadModel ?? throw new ArgumentNullException(nameof(resultReadModel));
    }

    public ResultHeaderViewModel HeaderViewModel { get; private set; }

    public ResultSeries4ReadModel ResultReadModel { get; private set; }
}
