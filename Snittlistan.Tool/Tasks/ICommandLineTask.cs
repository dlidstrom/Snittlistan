namespace Snittlistan.Tool.Tasks
{
    public interface ICommandLineTask
    {
        void Run(string[] args);
        string HelpText { get; }
    }
}