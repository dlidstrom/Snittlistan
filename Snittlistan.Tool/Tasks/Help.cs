namespace Snittlistan.Tool.Tasks
{
    using System;
    using Castle.MicroKernel;

    public class Help : ICommandLineTask
    {
        private readonly IKernel kernel;

        public Help(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void Run(string[] args)
        {
            var task = kernel.Resolve<ICommandLineTask>(args[1]);
            Console.WriteLine(task.HelpText);
        }

        public string HelpText => "Shows command help text";
    }
}