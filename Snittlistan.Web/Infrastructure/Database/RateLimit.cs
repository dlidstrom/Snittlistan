#nullable enable

namespace Snittlistan.Web.Infrastructure.Database;

public class RateLimit : HasVersion
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
        UpdatedDate = DateTime.Now;
    }

    private RateLimit()
    {
    }

    public int RateLimitId { get; private set; }

    public string Key { get; private set; } = null!;

    public double Allowance { get; private set; }

    public int Rate { get; private set; }

    public int PerSeconds { get; private set; }

    public DateTime UpdatedDate { get; private set; }

    public void UpdateAllowance(DateTime when)
    {
        Allowance += (double)(when - UpdatedDate).TotalSeconds * Rate / PerSeconds;
        if (Allowance > Rate)
        {
            Allowance = Rate;
        }

        UpdatedDate = when;
    }

    public void DecreaseAllowance(DateTime when)
    {
        Allowance--;
    }
}
