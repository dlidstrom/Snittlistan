#nullable enable

using NLog;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Snittlistan.Web.Infrastructure;

public class CustomTraceListener : TraceListener
{
    private static readonly Regex _levelRegex = new(@"[a-zA-Z]+\.([a-zA-Z]+\.?)+ (?<level>[A-Za-z]+)");
    private static readonly Dictionary<string, LogLevel> _toLevel = new()
    {
        { "Critical", LogLevel.Error },
        { "Error", LogLevel.Error },
        { "Verbose", LogLevel.Debug },
        { "Warning", LogLevel.Warn },
        { "Information", LogLevel.Info },
    };

    private readonly Logger _logger;
    private LogLevel? _level;
    private StringBuilder _currentLine = new();

    public CustomTraceListener(string provider)
    {
        _logger = LogManager.GetLogger(provider);
    }

    public override void Write(string message)
    {
        // default level
        _level = LogLevel.Info;
        Match match = _levelRegex.Match(message);
        if (match.Success)
        {
            _ = _toLevel.TryGetValue(match.Groups["level"].Value, out _level);
        }

        _ = _currentLine.Append(message);
    }

    public override void WriteLine(string message)
    {
        _ = _currentLine.Append(message);
        _logger.Log(_level, _currentLine);
        _currentLine = new StringBuilder();
    }
}
