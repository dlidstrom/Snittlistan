namespace Snittlistan.Tool.Tasks
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Raven.Client;
    using Snittlistan.Queue.Models;

    public static class CommandLineTaskHelper
    {
        public static IDocumentStore DocumentStore { get; set; }

        public static Uri[] AllApiUrls()
        {
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
            using (IDocumentSession session = DocumentStore.OpenSession())
            {
                SiteWideConfiguration siteWideConfig = session.Load<SiteWideConfiguration>(SiteWideConfiguration.GlobalId);
                Uri[] list = siteWideConfig.TenantConfigurations
                    .Select(x => new Uri($"http://{x.Hostname}:{port}/api/task"))
                    .ToArray();
                return list;
            }
        }
    }
}
