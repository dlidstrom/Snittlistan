using System;
using System.Collections.Generic;
using Snittlistan.Web.Areas.V2.Indexes;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public enum CompareMode
    {
        SeasonAverage,
        PlayerForm
    }

    public class PlayerStatusViewModel
    {
        public PlayerStatusViewModel(string name, PlayerFormViewModel playerForm)
        {
            Name = name;
            PlayerForm = playerForm;
            Absences = new List<AbsenceIndex.Result>();
            Teams = new List<RosterViewModel>();
        }

        public string Name { get; private set; }

        public PlayerFormViewModel PlayerForm { get; private set; }

        public List<AbsenceIndex.Result> Absences { get; private set; }

        public List<RosterViewModel> Teams { get; private set; }

        public class Comparer : IComparer<PlayerStatusViewModel>
        {
            private readonly Func<PlayerStatusViewModel, PlayerStatusViewModel, int> comparer;

            public Comparer(CompareMode compareMode)
            {
                switch (compareMode)
                {
                    case CompareMode.SeasonAverage:
                        comparer = (left, right) => left.PlayerForm.SeasonAverage.CompareTo(right.PlayerForm.SeasonAverage);
                        break;

                    case CompareMode.PlayerForm:
                        comparer = (left, right) => left.PlayerForm.Last5Average.CompareTo(right.PlayerForm.Last5Average);
                        break;
                }
            }

            public int Compare(PlayerStatusViewModel x, PlayerStatusViewModel y)
            {
                if (x.Absences.Count > 0 && y.Absences.Count == 0)
                    return 1;

                if (x.Absences.Count == 0 && y.Absences.Count > 0)
                    return -1;

                var a = CompareAbsences(x, y);
                if (a != 0) return a;

                var f = CompareForm(x, y);
                if (f != 0) return f;

                return comparer.Invoke(x, y);
            }

            private static int CompareForm(PlayerStatusViewModel x, PlayerStatusViewModel y)
            {
                if (x.PlayerForm == null)
                {
                    if (y.PlayerForm == null)
                        return string.CompareOrdinal(x.Name, y.Name);
                    return -1;
                }

                if (y.PlayerForm == null)
                    return 1;

                return 0;
            }

            private static int CompareAbsences(PlayerStatusViewModel x, PlayerStatusViewModel y)
            {
                if (x.Absences.Count > 0)
                {
                    if (y.Absences.Count > 0)
                        return string.CompareOrdinal(x.Name, y.Name);
                    return 1;
                }

                if (y.Absences.Count > 0)
                {
                    return -1;
                }

                return 0;
            }
        }
    }
}