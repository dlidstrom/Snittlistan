using System;
using System.Collections.Generic;
using System.Configuration;
using Raven.Client.Document;
using Snittlistan.Web.Infrastructure.Indexes;
using Snittlistan.Web.Models;

namespace Snittlistan.Tool.Tasks
{
    public class Initialize : ICommandLineTask
    {
        public void Run(string[] args)
        {
            var connectionStringName = args[1];
            var documentStore = new DocumentStore { ConnectionStringName = connectionStringName }.Initialize();
            IndexCreator.CreateIndexes(documentStore);
            using (var documentSession = documentStore.OpenSession())
            {
                if (documentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId) == null)
                {
                    documentSession.Store(new WebsiteConfig(new WebsiteConfig.TeamNameAndLevel[0], false));
                    documentSession.SaveChanges();
                }
            }
        }

        public string HelpText
        {
            get
            {
                var list = new List<string>();
                CommandLineTaskHelper.ForAllConnectionStrings(((ConnectionStringSettings cs, Uri uri) tuple) =>
                {
                    list.Add(tuple.cs.Name);
                });

                var helpText = $"Initializes indexes and migrates WebsiteConfig. Specify connection string name. Available connection strings:\n{string.Join(Environment.NewLine, list)}";
                return helpText;
            }
        }
    }
}