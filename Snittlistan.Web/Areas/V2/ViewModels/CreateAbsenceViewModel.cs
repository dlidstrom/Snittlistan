using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Raven.Abstractions;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class CreateAbsenceViewModel : IValidatableObject
    {
        public CreateAbsenceViewModel()
        {
            From = To = SystemTime.UtcNow.Date;
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Player { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (To < SystemTime.UtcNow.Date)
                yield return new ValidationResult("Till-datum kan inte vara passerade datum");
            if (From > To)
                yield return new ValidationResult("Från-datum kan inte vara före till-datum");
        }
    }
}