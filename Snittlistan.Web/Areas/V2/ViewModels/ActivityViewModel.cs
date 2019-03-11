namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using System.Web;

    public class ActivityViewModel
    {
        public ActivityViewModel(
            string id,
            string title,
            DateTime date,
            string message,
            string author)
        {
            Id = id;
            Title = title;
            ActivityDate = date;
            Message = new HtmlString(message);
            Author = author;
        }

        public string Id { get; }

        public string Title { get; }

        public DateTime ActivityDate { get; }

        public IHtmlString Message { get; }

        public string Author { get; }
    }
}