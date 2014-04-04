using System;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class AbsenceViewModel
    {
        public AbsenceViewModel(Absence absence, Player player)
        {
            PlayerName = player.Name;
            From = absence.From;
            To = absence.To;
        }

        public string PlayerName { get; private set; }

        public DateTime From { get; private set; }

        public DateTime To { get; private set; }
    }
}