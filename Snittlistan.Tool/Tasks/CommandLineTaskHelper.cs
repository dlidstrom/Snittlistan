using System;
using System.Collections.Generic;
using System.Configuration;

namespace Snittlistan.Tool.Tasks
{
    using Raven.Client;

    public static class CommandLineTaskHelper
    {
        public static IDocumentStore DocumentStore { get; set; }

        public static Uri[] AllApiUrls()
        {
            var list = new List<Uri>();
            foreach (string name in ConfigurationManager.AppSettings)
            {
                if (name.Contains("Snittlistan-"))
                {
                    string url = ConfigurationManager.AppSettings[name];
                    list.Add(new Uri(url));
                }
            }

            return list.ToArray();
        }
    }
}