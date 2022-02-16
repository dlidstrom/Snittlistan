#nullable enable

namespace Snittlistan.Web.Models;

public class RateLimit
{
    public RateLimit(
        string key,
        double allowance,
        int rate,
        int perSeconds,
        DateTime updatedDate)
    {
        Key = key;
        Allowance = allowance;
        Rate = rate;
        PerSeconds = perSeconds;
        UpdatedDate = updatedDate;
    }

    public static RateLimit Create(
        string key,
        double allowance,
        int rate,
        int perSeconds)
    {
        return new(key, allowance, rate, perSeconds, DateTime.MinValue);
    }

    public string Key { get; }

    public double Allowance { get; private set; }

    public int Rate { get; }

    public int PerSeconds { get; }

    public DateTime UpdatedDate { get; private set; }

    public RateLimit UpdateAllowance(DateTime when)
    {
        Allowance += (double)(when - UpdatedDate).TotalSeconds * Rate / PerSeconds;
        if (Allowance > Rate)
        {
            Allowance = Rate;
        }

        UpdatedDate = when;
        return this;
    }

    public RateLimit DecreaseAllowance()
    {
        Allowance--;
        return this;
    }
}
