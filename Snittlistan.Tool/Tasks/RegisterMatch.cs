using System;
using System.Configuration;
using Snittlistan.Queue;
using Snittlistan.Queue.Messages;

namespace Snittlistan.Tool.Tasks
{
    public class RegisterMatch : ICommandLineTask
    {
        public void Run(string[] args)
        {
            MsmqGateway.BeginTransaction();
            CommandLineTaskHelper.ForAllConnectionStrings(((ConnectionStringSettings cs, Uri uri) tuple) =>
            {
                MsmqGateway.PublishMessage(new MessageEnvelope(new RegisterMatchesMessage(), tuple.uri));
            });
            MsmqGateway.CommitTransaction();
        }

        public string HelpText => "Registers matches from Bits";
    }
}