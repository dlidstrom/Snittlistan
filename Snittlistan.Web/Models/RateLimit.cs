#nullable enable

namespace Snittlistan.Web.Models;

public class RateLimit
{
    public RateLimit(
        string key,
        double allowance,
        int rate,
        int perSeconds)
    {
        Key = key;
        Allowance = allowance;
        Rate = rate;
        PerSeconds = perSeconds;
    }

    public string Key { get; }

    public double Allowance { get; private set; }

    public int Rate { get; }

    public int PerSeconds { get; }

    public DateTime UpdatedDate { get; private set; }

    public void UpdateAllowance(DateTime when)
    {
        Allowance = (double)(when - UpdatedDate).TotalSeconds * Rate / PerSeconds;
        if (Allowance > Rate)
        {
            Allowance = Rate;
        }
    }

    public void DecreaseAllowance(DateTime when)
    {
        Allowance--;
        UpdatedDate = when;
    }
}
