#nullable enable

using Newtonsoft.Json;

namespace Snittlistan.Web.Areas.V2.Domain;

public class Absence
{
    [JsonConstructor]
    private Absence(
        DateTime from,
        DateTime to,
        string player,
        string comment)
    {
        From = from;
        To = to;
        Player = player;
        Comment = comment;
    }

    public int Id { get; set; }

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public string Player { get; set; }

    public string Comment { get; set; }

    public static Absence Create(
        DateTime from,
        DateTime to,
        string player,
        string comment)
    {
        if (comment == null)
        {
            comment = string.Empty;
        }

        if (comment.Length > 160)
        {
            throw new ArgumentException("Comment can be at most 160 characters", nameof(comment));
        }

        return new Absence(from, to, player, comment);
    }
}
