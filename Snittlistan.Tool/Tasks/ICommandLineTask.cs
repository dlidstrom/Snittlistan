#nullable enable

namespace Snittlistan.Tool.Tasks;
public interface ICommandLineTask
{
    Task Run(string[] args);

    string HelpText { get; }
}
