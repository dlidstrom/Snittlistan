#nullable enable

using Newtonsoft.Json;

namespace Snittlistan.Web.Areas.V2.Domain;

public class Activity
{
    [JsonConstructor]
    private Activity(
        int season,
        string title,
        DateTime date,
        string message,
        string messageHtml,
        string authorId)
    {
        Season = season;
        Title = title;
        Date = date;
        Message = message;
        MessageHtml = messageHtml ?? message;
        AuthorId = authorId;
    }

    public string? Id { get; set; }

    public int Season { get; private set; }

    public string Title { get; private set; }

    public DateTime Date { get; private set; }

    public string Message { get; private set; }

    public string MessageHtml { get; private set; }

    public string AuthorId { get; set; }

    public void Update(
        int season,
        string title,
        DateTime date,
        string message,
        string messageHtml,
        string authorId)
    {
        if (message.Length > 10 * 1024)
        {
            throw new ArgumentOutOfRangeException(nameof(message));
        }

        if (messageHtml.Length > 10 * 1024)
        {
            throw new ArgumentOutOfRangeException(nameof(messageHtml));
        }

        Season = season;
        Title = title;
        Date = date;
        Message = message;
        MessageHtml = messageHtml;
        AuthorId = authorId;
    }

    public static Activity Create(
        int season,
        string title,
        DateTime date,
        string message,
        string messageHtml,
        string authorId)
    {
        if (message.Length > 10 * 1024)
        {
            throw new ArgumentOutOfRangeException(nameof(message));
        }

        if (messageHtml.Length > 10 * 1024)
        {
            throw new ArgumentOutOfRangeException(nameof(messageHtml));
        }

        return new Activity(season, title, date, message, messageHtml, authorId);
    }
}
