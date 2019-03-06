namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;
    using Domain;

    public class ActivityViewModel
    {
        public ActivityViewModel(Activity activity)
        {
            Id = activity.Id;
            Title = activity.Title;
            ActivityDate = activity.Date;
            Message = activity.Message;
        }

        public string Id { get; }

        public string Title { get; }

        public DateTime ActivityDate { get; }

        public string Message { get; }
    }
}