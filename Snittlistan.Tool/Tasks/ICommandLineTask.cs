#nullable enable

namespace Snittlistan.Tool.Tasks
{
    using System.Threading.Tasks;

    public interface ICommandLineTask
    {
        Task Run(string[] args);

        string HelpText { get; }
    }
}
