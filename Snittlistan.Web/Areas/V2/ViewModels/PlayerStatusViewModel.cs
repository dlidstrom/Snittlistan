using System.Diagnostics;
using Snittlistan.Web.Areas.V2.Domain;
using Snittlistan.Web.Areas.V2.Indexes;

#nullable enable

namespace Snittlistan.Web.Areas.V2.ViewModels;
public enum CompareMode
{
    SeasonAverage,
    PlayerForm
}

public class PlayerStatusViewModel
{
    private readonly DateTime from;
    private readonly DateTime to;
    private readonly List<AbsenceIndex.Result> absences = new();

    public PlayerStatusViewModel(
        Player player,
        PlayerFormViewModel? playerForm,
        DateTime from,
        DateTime to)
    {
        PlayerId = player.Id;
        Name = player.Name;
        PlayerForm = playerForm;
        this.from = from;
        this.to = to;
        Teams = new List<RosterViewModel>();
    }

    public string PlayerId { get; }

    public string Name { get; }

    public PlayerFormViewModel? PlayerForm { get; }

    public AbsenceIndex.Result[] Absences => absences.ToArray();

    public List<RosterViewModel> Teams { get; }

    public void AddAbsence(AbsenceIndex.Result absence)
    {
        absences.Add(absence);
    }

    public void AddAbsences(IEnumerable<AbsenceIndex.Result> results)
    {
        absences.AddRange(results);
    }

    public override int GetHashCode()
    {
        return PlayerId.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((PlayerStatusViewModel)obj);
    }

    protected bool Equals(PlayerStatusViewModel other)
    {
        return string.Equals(PlayerId, other.PlayerId);
    }

    private AbsenceIndex.Result[] CompleteAbsences()
    {
        return absences.Where(x => x.From <= from && x.To >= to).ToArray();
    }

    public class Comparer : IComparer<PlayerStatusViewModel>
    {
        private readonly Func<PlayerStatusViewModel, PlayerStatusViewModel, int> comparer;

        public Comparer(CompareMode compareMode)
        {
            comparer = compareMode switch
            {
                CompareMode.SeasonAverage => (left, right) => left.PlayerForm!.SeasonAverage.CompareTo(right.PlayerForm!.SeasonAverage),
                CompareMode.PlayerForm => (left, right) => left.PlayerForm!.Last5Average.CompareTo(right.PlayerForm!.Last5Average),
                _ => throw new ArgumentOutOfRangeException(nameof(compareMode), compareMode, null),
            };
        }

        public int Compare(PlayerStatusViewModel left, PlayerStatusViewModel right)
        {
            Debug.Assert(left != null, nameof(left) + " != null");
            Debug.Assert(right != null, nameof(right) + " != null");

            AbsenceIndex.Result[] leftCompleteAbsences = left!.CompleteAbsences();
            AbsenceIndex.Result[] rightCompleteAbsences = right!.CompleteAbsences();

            int a = CompareAbsences(
                leftCompleteAbsences,
                left.Name,
                rightCompleteAbsences,
                right.Name);
            if (a != 0)
            {
                return a;
            }

            int f = CompareForm(left, right);
            if (f != 0)
            {
                return f;
            }

            return comparer.Invoke(left, right);
        }

        private static int CompareAbsences(
            AbsenceIndex.Result[] leftCompleteAbsences,
            string leftName,
            AbsenceIndex.Result[] rightCompleteAbsences,
            string rightName)
        {
            if (leftCompleteAbsences.Length > 0)
            {
                if (rightCompleteAbsences.Length > 0)
                {
                    return string.CompareOrdinal(rightName, leftName);
                }

                return -1;
            }

            if (rightCompleteAbsences.Length > 0)
            {
                return 1;
            }

            return 0;
        }

        private static int CompareForm(PlayerStatusViewModel x, PlayerStatusViewModel y)
        {
            if (x.PlayerForm!.HasResult == false)
            {
                if (y.PlayerForm!.HasResult == false)
                {
                    return string.CompareOrdinal(y.Name, x.Name);
                }

                return -1;
            }

            if (y.PlayerForm!.HasResult == false)
            {
                return 1;
            }

            return 0;
        }
    }
}
