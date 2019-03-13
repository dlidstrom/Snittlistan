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
            string messageHtml,
            string author)
        {
            Id = id;
            Title = title;
            ActivityDate = date;
            MessageHtml = new HtmlString(messageHtml);
            Author = author;
        }

        public string Id { get; }

        public string Title { get; }

        public DateTime ActivityDate { get; }

        public IHtmlString MessageHtml { get; }

        public string Author { get; }
    }
}