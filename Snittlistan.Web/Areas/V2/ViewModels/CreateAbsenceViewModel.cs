namespace Snittlistan.Web.Areas.V2.ViewModels
{
    using System;

    using Raven.Abstractions;

    public class CreateAbsenceViewModel
    {
        public CreateAbsenceViewModel()
        {
            From = To = SystemTime.UtcNow.Date;
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Player { get; set; }
    }
}