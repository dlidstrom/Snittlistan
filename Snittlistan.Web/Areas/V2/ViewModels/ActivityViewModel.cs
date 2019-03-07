namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;

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
            Message = message;
            Author = author;
        }

        public string Id { get; }

        public string Title { get; }

        public DateTime ActivityDate { get; }

        public string Message { get; }

        public string Author { get; }
    }
}