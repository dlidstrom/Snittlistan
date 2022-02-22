using System.Web;

namespace Snittlistan.Web.Areas.V2.ViewModels;
public class ActivityViewModel
{
    public ActivityViewModel(
        string id,
        string title,
        DateTime date,
        string messageHtml,
        string author,
        string appleTouchIcon,
        string fullTeamName)
    {
        Id = id;
        Title = title;
        ActivityDate = date;
        MessageHtml = new HtmlString(messageHtml);
        Author = author;
        AppleTouchIcon = appleTouchIcon;
        FullTeamName = fullTeamName;
    }

    public string Id { get; }

    public string Title { get; }

    public DateTime ActivityDate { get; }

    public IHtmlString MessageHtml { get; }

    public string Author { get; }

    public string AppleTouchIcon { get; }

    public string FullTeamName { get; }
}
