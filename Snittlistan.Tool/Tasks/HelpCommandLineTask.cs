using Castle.MicroKernel;

#nullable enable

namespace Snittlistan.Tool.Tasks;
public class HelpCommandLineTask : ICommandLineTask
{
    private readonly IKernel kernel;

    public HelpCommandLineTask(IKernel kernel)
    {
        this.kernel = kernel;
    }

    public Task Run(string[] args)
    {
        ICommandLineTask task = kernel.Resolve<ICommandLineTask>(args[1]);
        Console.WriteLine(task.HelpText);
        return Task.CompletedTask;
    }

    public string HelpText => "Shows command help text";
}
