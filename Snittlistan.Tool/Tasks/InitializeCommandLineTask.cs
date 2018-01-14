using System;
using System.Configuration;
using System.Linq;
using Snittlistan.Queue;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Tool.Tasks
{
    public class InitializeCommandLineTask : ICommandLineTask
    {
        public void Run(string[] args)
        {
            var apiUrl = args[1];
            using (var scope = MsmqGateway.AutoCommitScope())
            {
                var envelope = new MessageEnvelope(
                    new InitializeIndexesMessage(),
                    new Uri(ConfigurationManager.AppSettings[apiUrl]));
                scope.PublishMessage(envelope);
            }
        }

        public string HelpText
        {
            get
            {
                var helpText = $"Initializes indexes and migrates WebsiteConfig. Specify api url. Available api urls:\n{string.Join(Environment.NewLine, CommandLineTaskHelper.AllApiUrls().Select(x => x.ToString()))}";
                return helpText;
            }
        }
    }
}